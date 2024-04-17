using System;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using z5.ms.common.infrastructure.id;

namespace z5.ms.infrastructure.user.identity
{
    /// <summary>Static class to initialize id service inside of user service</summary>
    public static class IdServerInitialize
    {
        /// <summary>
        /// Run from startup manually to configure services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddMvc();

            var migrationsAssembly = typeof(IdServerInitialize).GetTypeInfo().Assembly.GetName().Name;

            var rsaStr = Encoding.UTF8.GetString(Convert.FromBase64String(configuration["TokenServiceOptions:RsaKey"]));
            var rsaParameters = JsonConvert.DeserializeObject<RSAParameters>(rsaStr, new JsonSerializerSettings { ContractResolver = new RsaKeyContractResolver() });
          
            // configure identity server with in-memory stores, keys, clients and scopes
            services.AddIdentityServer(options =>
                {
                    options.PublicOrigin = configuration["IdServiceBaseUrl"];
                    options.IssuerUri = configuration["IdServiceBaseUrl"];
                })
                    .AddSigningCredential(new RsaSecurityKey(rsaParameters))
                    .AddInMemoryIdentityResources(IdentityConfig.GetIdentityResources())
                    .AddInMemoryApiResources(IdentityConfig.GetApiResources())
                    .AddInMemoryClients(IdentityConfig.GetClients(configuration))
                    .AddOperationalStore(options =>
                    {
                        options.ConfigureDbContext = builder =>
                            builder.UseSqlServer(configuration.GetConnectionString("IdServerDatabaseConnection"),
                                sql => sql.MigrationsAssembly(migrationsAssembly));

                    })
                    .AddProfileService<CustomProfileService>()
                    .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                    .AddExtensionGrantValidator<DelegationGrantValidator>();

            // Add repositories, other 'our' stuff
            services.AddSingleton<IPasswordEncryptionStrategy, BCryptPasswordStrategy>();
            services.AddSingleton<ITokenUserRepository, TokenUserRepository>();

            services.AddHttpClient();
        }

        /// <summary>
        /// Run from startup manually to configure application
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public static void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // this will do the initial DB population
            InitializeDatabase(app);

            app.UseIdentityServer();

            app.UseMvcWithDefaultRoute();
        }

        private static void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
            }
        }

        private class RsaKeyContractResolver : DefaultContractResolver
        {
            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                var property = base.CreateProperty(member, memberSerialization);

                property.Ignored = false;

                return property;
            }
        }
    }
}