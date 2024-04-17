using System;
using System.Net;
using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog.Extensions.Logging;
using Dapper.FastCrud;
using z5.ms.common.abstractions;
using z5.ms.common.attributes;
using z5.ms.common.extensions;
using z5.ms.common.validation;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Logging;
using z5.ms.common;
using z5.ms.common.controllers;
using z5.ms.common.infrastructure.db;
using z5.ms.common.infrastructure.events;
using z5.ms.common.infrastructure.geoip;
using z5.ms.common.notifications;
using z5.ms.common.validation.authproviders;
using z5.ms.domain.user;
using z5.ms.domain.user.datamodels;
using z5.ms.infrastructure.user;
using z5.ms.infrastructure.user.identity;
using z5.ms.infrastructure.user.repositories;
using z5.ms.infrastructure.user.services;
using z5.ms.userservice.controllers;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace z5.ms.userservice
{
    /// <summary>Application startup</summary>
    public class Startup
    {
        /// <summary>Application configuration root</summary>
        public IConfigurationRoot Configuration { get; protected set; }

        /// <summary>Application hosting environment</summary>
        public IHostingEnvironment Env { get; protected set; }

        /// <summary>Application startup controller</summary>
        public Startup(IHostingEnvironment env)
        {
            Env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        /// <summary>This method gets called by the runtime. Use this method to add services to the container.</summary>
        public virtual void ConfigureServices(IServiceCollection services)
        {
            //CORS - first middleware to be registered!
            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            corsBuilder.SetIsOriginAllowed(o => true);
            corsBuilder.AllowCredentials();
            services.AddCors(options => { options.AddPolicy("AllowAll", corsBuilder.Build()); });

            services.Configure<TokenServiceOptions>(Configuration.GetSection("TokenServiceOptions"));
            services.Configure<HipiEndpointSettings>(Configuration.GetSection("HipiEndpointSettings"));
            services.Configure<DbConnectionOptions>(Configuration.GetSection("ConnectionStrings"));
            services.Configure<AccessValidationByIPSettings>(Configuration.GetSection("AccessValidtionByIPSettings"));

            IdServerInitialize.ConfigureServices(services, Configuration);

            IdentityModelEventSource.ShowPII = true;

            //Add AutoMapper
            services.AddAutoMapper();

            //Add configuration singleton (in addition to Options)
            services.AddSingleton<IConfiguration>(Configuration);

            var featureProvider = new ConfigureControllerFeatureProvider();

            //Sync options
            services.Configure<UserServiceOptions>(Configuration);

            //Generic event subscriber and publisher using azure service bus
            services.Configure<EventBusOptions>(Configuration);

            // TODO: remove feature switching code
            services.TryAdd(Convert.ToBoolean(Configuration["MsEventsFeatureSwitch"])
                ? ServiceDescriptor.Singleton(typeof(IEventPublisher<>), typeof(EventPublisher<>))
                : ServiceDescriptor.Singleton(typeof(IEventPublisher<>), typeof(DummyEventPublisher<>)));

            OrmConfiguration.DefaultDialect = SqlDialect.MsSql;

            services.AddAWSService<Amazon.S3.IAmazonS3>();
            services.AddAWSService<Amazon.SQS.IAmazonSQS>();
            services.AddAWSService<Amazon.SimpleNotificationService.IAmazonSimpleNotificationService>();

            //Notification service
            services.Configure<NotificationOptions>(Configuration);
            services.AddSingleton<INotificationQueue, NotificationQueue>();
            services.AddSingleton<IAWSNotificationQueue, AWSNotificationQueue>();
            services.AddSingleton<INotificationClient, NotificationClient>();
            services.AddSingleton<IAWSSnsClient, AWSSnsClient>();

            //Authproviders
            services.AddScoped<JwtAuthProvider>();
            services.AddScoped<OAuthJwtAuthProvider>();
            services.AddScoped<AccessValidationByIPFilterAttribute>();
            //bootstrap automapper
            var mapperConfiguration = new MapperConfigurationExpression();
            services.AddSingleton<IMapper>(sp => new Mapper(new MapperConfiguration(mapperConfiguration)));

            //register component modules
            services.AddModule<UserModule, UserRegistrationOptions>(Configuration, featureProvider, mapperConfiguration,
                new UserRegistrationOptions
                {
                    UseAsyncRepositories = Convert.ToBoolean(Configuration["UseAsyncRepositories"] ?? "false")
                });

            services.AddUserServiceHealthChecks(Configuration);

            //Add framework services.
            services.AddMvc(config =>
            {
                //Apply request logging to all controllers
                config.Filters.Add<RequestLoggingFilterAttribute>();
            })
                .AddJsonOptions(options => { options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc; })
                .ConfigureApplicationPartManager(apm => apm.FeatureProviders.Add(featureProvider));

            //Swagger help
            services.AddSwaggerGen(o => o.SetApiSwaggerOptions("User Service API", "v1", Configuration));

            //Token verification and user data extraction
            services.AddScoped<ITokenValidator, TokenValidator>();
            services.AddSingleton<JwtTokenValidator>();

            // Intialize controllers
            featureProvider.AddController<VersionController>();
            featureProvider.AddController<CustomerController>();
            featureProvider.AddController<FavoritesController>();
            featureProvider.AddController<RemindersController>();
            featureProvider.AddController<SettingsController>();
            featureProvider.AddController<UserController>();
            featureProvider.AddController<WatchHistoryController>();
            featureProvider.AddController<WatchlistController>();
            featureProvider.AddController<CheckController>();
        }

        /// <summary>This method gets called by the runtime. Use this method to configure the HTTP request pipeline</summary>
        /// <remarks>Note! pay attention to middleware registration order!</remarks>
        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //nlog with debug and console loggers
            loggerFactory.ConfigureNLog("nlog.config");
            loggerFactory.AddConsole(LogLevel.Debug);
            loggerFactory.AddDebug();
            loggerFactory.AddNLog();  //NLog

            //CORS - configure the pipeline to use the 'AllowAll' policy (see ConfigureServices)
            //Note! must be added as the first element in the pipeline
            app.UseCors("AllowAll");

            //Error handler
            app.UseExceptionHandler(HandleError);

            app.UseAuthentication();

            //MVC
            app.UseMvc();

            //Identity server
            IdServerInitialize.Configure(app, env);
            app.UseStaticFiles();

            //Swagger middleware and Swagger UI
            app.AddApiSwaggerWithUI("User Server API", "v1", Configuration.GetSection("SwaggerRouteSecret")?.Value);

            //Services initialization and startup
            StartServices(app);
        }

        /// <summary>API error handler</summary>
        private void HandleError(IApplicationBuilder builder)
        {
            builder.Run(async context =>
            {
                var ex = context.Features.Get<IExceptionHandlerFeature>().Error;

                // Log current request with exception 
                var logger = context.RequestServices.GetService<ILoggerFactory>().CreateLogger("ExceptionLogging");
                logger.LogRequestWithException(context.Request, ex);

                // Send exception message in response
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new Error { Code = 1, Message = ex.Message })).ConfigureAwait(false);
            });
        }

        /// <summary>Start / initialize all singletons</summary>
        protected virtual void StartServices(IApplicationBuilder app, bool startAsync = true)
        {
            //Initialize Session DB
            //var assetsCache = app.ApplicationServices.GetService<IAssetsCache>();
            //assetsCache.RegisterAdapter<MovieAdapter>();
            //assetsCache.Initialize(startAsync);

            // Init watch history repo
            app.ApplicationServices.GetService<WatchHistoryRepositoryAsync>();
        }
    }
}