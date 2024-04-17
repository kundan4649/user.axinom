using AutoMapper;
using AutoMapper.EquivalencyExpression;
using FluentValidation;
using z5.ms.infrastructure.user.customer;
using z5.ms.infrastructure.user.favorites;
using z5.ms.infrastructure.user.mapping;
using z5.ms.infrastructure.user.repositories;
using z5.ms.infrastructure.user.services;
using z5.ms.infrastructure.user.settings;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using z5.ms.common;
using z5.ms.common.infrastructure.geoip;
using z5.ms.common.infrastructure.id;
using z5.ms.common.infrastructure.logging;
using z5.ms.common.validation;
using z5.ms.domain.user;
using z5.ms.domain.user.customer;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.user;
using z5.ms.infrastructure.user.identity;
using z5.ms.infrastructure.user.user;

namespace z5.ms.infrastructure.user
{
    /// <inheritdoc />
    /// <summary>User BC registration module</summary>
    public class UserModule : IComponentModule<UserRegistrationOptions>
    {
        /// <inheritdoc />
        public IServiceCollection Register(IServiceCollection services, IConfiguration config, ConfigureControllerFeatureProvider provider, IMapperConfigurationExpression mapperConfig, UserRegistrationOptions options = null)
        {
            if (options == null)
                options = UserRegistrationOptions.Default;

            //Validation behavior for all command and query handlers
            //Important! Behaviors will execute in the registered order!
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LogBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            // Customer repository (internal user API)
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            // Customer queries
            services.AddMediatR(typeof(GetCustomersQueryHandler));
            services.AddSingleton<IValidator<GetCustomersQuery>, GetCustomersQueryValidator>();

            // Authentication services
            services.AddScoped<IAuthTokenService, AuthTokenService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ISocialAuthenticationService, SocialAuthenticationService>();
            services.AddScoped<ISocialProfileService, SocialProfileService>();
            services.AddScoped<ISubscriptionAPIService, SubscriptionAPIService>();
            // User confirmation service
            services.AddScoped<IConfirmationService, ConfirmationService>();

            // Maxmind GeoIp service
            services.AddSingleton<IGeoIpService, GeoIpService>();

            // Password services
            services.AddSingleton<IPasswordService, PasswordService>();
            services.AddSingleton<IOneTimePassRepository, OneTimePassRepository>();
            services.AddSingleton<IPasswordEncryptionStrategy, BCryptPasswordStrategy>();

            // Repositories
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IRemindersRepository, RemindersRepository>();
            services.AddSingleton<ISettingsRepository, SettingsRepository>();
            services.AddSingleton<IFavoritesRepository, FavoritesRepository>();
            services.AddSingleton<IWatchlistRepository, WatchlistRepository>();
            services.Configure<WatchHistoryOptions>(config.GetSection("WatchHistoryOptions"));
            services.AddSingleton<WatchHistoryRepository>();
            services.AddSingleton<WatchHistoryRepositoryAsync>();
            services.AddSingleton<IWatchHistoryRepository, WatchHistoryRepoWrapper>();
            services.AddSingleton<IAccessValidationByIPRepository, AccessValidationByIPRepository>();
            services.AddSingleton<IPartnerSettingsMasterRepository, PartnerSettingsMasterRepository>();
            services.AddSingleton<IUserProfileUpdateHistoryRepository, UserProfileUpdateHistoryRepository>();

            // Register commands
            services.AddMediatR(typeof(RegisterEmailUserCommandHandler), typeof(RegisterMobileUserCommandHandler), typeof(RegisterSocialUserCommandHandler));
            services.AddSingleton<IValidator<RegisterEmailUserCommand>, RegisterEmailUserCommandValidator>();
            services.AddSingleton<IValidator<RegisterMobileUserCommand>, RegisterMobileUserCommandValidator>();
            services.AddSingleton<IValidator<RegisterSocialUserCommand>, RegisterSocialUserCommandValidator>();

            // Login commands
            services.AddMediatR(typeof(LoginEmailUserCommandHandler), typeof(LoginMobileUserCommandHandler), typeof(LoginSocialUserCommandHandler));
            services.AddMediatR(typeof(LoginEmailUserCommandV3Handler), typeof(LoginMobileUserCommandV3Handler)); //, typeof(LoginSocialUserCommandHandler)
            services.AddSingleton<IValidator<LoginEmailUserCommand>, LoginEmailUserCommandValidator>();
            services.AddSingleton<IValidator<LoginEmailUserCommandV3>, LoginEmailUserCommandV3Validator>();
            services.AddSingleton<IValidator<LoginMobileUserCommandV3>, LoginMobileUserCommandV3Validator>();
            services.AddSingleton<IValidator<LoginMobileUserCommand>, LoginMobileUserCommandValidator>();
            services.AddSingleton<IValidator<LoginSocialUserCommand>, LoginSocialUserCommandValidator>();

            // Confirmation commands
            services.AddMediatR(typeof(ConfirmUserCommandHandler), typeof(ResendConfirmationEmailCommandHandler), typeof(ResendConfirmationSmsCommandHandler));
            services.AddSingleton<IValidator<ConfirmUserCommand>, ConfirmUserCommandValidator>();
            services.AddSingleton<IValidator<ConfirmUserCommandv2>, ConfirmUserCommandv2Validator>();
            services.AddSingleton<IValidator<ResendConfirmationEmailCommand>, ResendConfirmationEmailCommandValidator>();
            services.AddSingleton<IValidator<ResendConfirmationSmsCommand>, ResendConfirmationSmsCommandValidator>();

            // Profile commands
            services.AddMediatR(typeof(GetUserQuery), typeof(DeleteUserCommand), typeof(UpdateUserCommand));
            services.AddSingleton<IValidator<GetUserQuery>, GetUserQueryValidator>();
            services.AddSingleton<IValidator<UpdateUserCommand>, UpdateUserCommandValidator>();
            services.AddSingleton<IValidator<DeleteUserCommand>, DeleteUserCommandValidator>();
            services.AddSingleton<IHipiHandler, HipiHandler>();

            // Password commands
            services.AddMediatR(typeof(ResetPasswordCommandHandler), typeof(ChangePasswordCommandHandler), typeof(ForgotPasswordEmailCommandHandler), typeof(ForgotPasswordMobileCommandHandler));
            services.AddSingleton<IValidator<ResetPasswordCommand>, ResetPasswordCommandValidator>();
            services.AddSingleton<IValidator<ResetPasswordCommandv2>, ResetPasswordCommandv2Validator>();
            services.AddSingleton<IValidator<ChangePasswordCommand>, ChangePasswordCommandValidator>();
            services.AddSingleton<IValidator<ChangePasswordCommandv2>, ChangePasswordCommandv2Validator>();
            services.AddSingleton<IValidator<ForgotPasswordEmailCommand>, ForgotPasswordEmailCommandValidator>();
            services.AddSingleton<IValidator<ForgotPasswordMobileCommand>, ForgotPasswordMobileCommandValidator>();

            // Add AutoMapper profiles
            mapperConfig.AddCollectionMappers();
            mapperConfig.AddProfile<UserProfile>();
            mapperConfig.AddProfile<FavoritesProfile>();
            mapperConfig.AddProfile<WatchHistoryProfile>();
            mapperConfig.AddProfile<WatchlistProfile>();
            mapperConfig.AddProfile<SettingsProfile>();
            mapperConfig.AddProfile<CustomerProfile>();
            mapperConfig.AddProfile<RemindersProfile>();

            return services;
        }
    }
}