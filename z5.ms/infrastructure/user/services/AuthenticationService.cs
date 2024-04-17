using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using z5.ms.common.abstractions;
using z5.ms.common.extensions;
using z5.ms.common.infrastructure.events;
using z5.ms.common.infrastructure.events.model;
using z5.ms.common.infrastructure.id;
using z5.ms.domain.user;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.user;
using z5.ms.domain.user.viewmodels;
using z5.ms.infrastructure.user.repositories;

namespace z5.ms.infrastructure.user.services
{
    /// <summary>Service for register and login using credentials</summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Register using credentials
        /// </summary>
        /// <remarks>Credentials can be email or mobile number</remarks>
        Task<Result<UserEntity>> Register(RegisterUserCommand command);

        /// <summary>
        /// Login using credentials
        /// </summary>
        /// <remarks>Credentials can be email or mobile number</remarks>
        Task<Result<UserEntity>> Login(LoginUserCommand command);
    }

    /// <inheritdoc />
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IEventPublisher<UserEvent> _eventPublisher;
        private readonly IPasswordEncryptionStrategy _passwordStrategy;
        private readonly IOneTimePassRepository _otpRepository;
        private readonly UserServiceOptions _options;
        private readonly UserErrors _errors;
        private readonly ISubscriptionAPIService _subscriptionAPIService;
        private readonly ILogger _logger;
        /// <inheritdoc />
        public AuthenticationService(IUserRepository userRepository, IMapper mapper, IEventPublisher<UserEvent> eventPublisher, 
            IPasswordEncryptionStrategy passwordStrategy, IOneTimePassRepository otpRepository, IOptions<UserServiceOptions> options, IOptions<UserErrors> errors, ISubscriptionAPIService subscriptionAPIService,ILogger<AuthenticationService> logger)
        {
            _mapper = mapper;
            _eventPublisher = eventPublisher;
            _passwordStrategy = passwordStrategy;
            _otpRepository = otpRepository;
            _options = options.Value;
            _userRepository = userRepository;
            _errors = errors.Value;
            _subscriptionAPIService = subscriptionAPIService;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<Result<UserEntity>> Register(RegisterUserCommand command)
        {
            //username can be either email or mobile number
            var userName = command.GetType().GetProperty(command.Type.EnumMemberValue())?.GetValue(command)?.ToString();
            var existing = await _userRepository.GetUser(command.Type, userName);
            if (existing != null)
                return Result<UserEntity>.FromError(_errors.UserAlreadyActivated);

            var user = _mapper.Map<UserEntity>(command);
            user.Id = Guid.NewGuid();
            user.State = UserState.Registered;
            user.PasswordHash = _passwordStrategy.HashPassword(user.PasswordHash);
            user.System = _options.DefaultSystemType;
            user.CreationDate = DateTime.UtcNow;
            if(command.Type.Equals(AuthenticationMethod.Email))
            {
                user.IsEmailConfirmed = !string.IsNullOrWhiteSpace(user.Email);
                user.State = UserState.Verified;
            }

            _logger.LogInformation($"Registration: code creation | UserName: {userName}| UserId: {user.Id}");
            var otp = await _otpRepository.CreateCode(user.Id, userName, GetOtpDescriptor(command.Type, command.Version));
          
            _logger.LogInformation($"Registration: db insert | UserName: {userName}| UserId: {user.Id}");

            var insertResult = await _userRepository.Insert(user);
            if (!insertResult.Success)
                return Result<UserEntity>.FromError(insertResult);
            _logger.LogInformation($"Registration: db insert completed | UserName: {userName}| UserId: {user.Id}");

            await _subscriptionAPIService.CreatePromotionalSubscription(user.Id.ToString(), command.IpAddress, command.RegistrationCountry);
            
            _logger.LogInformation($"Registration: SetConfirmationKey | UserName: {userName}| UserId: {user.Id}");

            SetConfirmationKey(command.Type, user, otp.Value.Code);

            return Result<UserEntity>.FromValue(user);
        }

        /// <inheritdoc />
        public async Task<Result<UserEntity>> Login(LoginUserCommand command)
        {
            //username can be either email or mobile number
            var userName = command.GetValue<string>(command.Type.EnumMemberValue());
            var user = await _userRepository.GetUser(command.Type, userName);

            if (user?.PasswordHash == null)
                return Result<UserEntity>.FromError(command.Type == AuthenticationMethod.Mobile 
                    ? _errors.WrongMobilePassword : _errors.WrongEmailPassword, 401);

            if (user.PasswordHash.EndsWith("_user"))
                return Result<UserEntity>.FromError(
                    _errors.LoginAccount.WithFields(new ErrorField
                    {
                        Field = "account",
                        Message = user.PasswordHash.Split('_')?[0]
                    }), 401);

            if (!_passwordStrategy.VerifyPassword(command.GetValue<string>("Password"), user.PasswordHash))
                return Result<UserEntity>.FromError(command.Type == AuthenticationMethod.Mobile
                    ? _errors.WrongMobilePassword : _errors.WrongEmailPassword, 401);

            if (!IsConfirmed(command.Type, user))
                return Result<UserEntity>.FromError(command.Type == AuthenticationMethod.Mobile
                    ? _errors.MobileUnconfirmed : _errors.EmailUnconfirmed, 401);

            var json = user.Json.ToJObject();
            var deviceId = command.GetValue<string>("DeviceId");
            int cttl = command.GetValue<int>("Cttl");

            _userRepository.UpdateAdditionalInfo(user, cttl, deviceId, true);


            return Result<UserEntity>.FromValue(user);
        }

        private OtpDescriptor GetOtpDescriptor(AuthenticationMethod type, int version)
        {
            switch (type)
            {
                case AuthenticationMethod.Email:
                    return new OtpDescriptor
                    {
                        Duration = TimeSpan.FromHours(_options.EmailConfirmationKeyExpiry),
                        KeyLength = 8,
                        OtpCreationStrategy = OtpCreationStrategy.AlphaNumeric,
                        OtpGroup = OtpGroup.ConfirmationEmail
                    };
                case AuthenticationMethod.Mobile:
                    return version != 2
                        ? new OtpDescriptor
                        {
                            Duration = TimeSpan.FromMinutes(_options.MobileConfirmationKeyExpiry),
                            KeyLength = _options.MobileConfirmationKeyLength,
                            OtpCreationStrategy = OtpCreationStrategy.AlphaNumeric,
                            OtpGroup = OtpGroup.ConfirmationMobile
                        }
                        : new OtpDescriptor
                        {
                            Duration = TimeSpan.FromMinutes(15),
                            KeyLength = 4,
                            OtpCreationStrategy = OtpCreationStrategy.Numeric,
                            OtpGroup = OtpGroup.ConfirmationMobile
                        };
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool IsConfirmed(AuthenticationMethod type, UserEntity user)
        {
            switch (type)
            {
                case AuthenticationMethod.Email:
                    return user.State == UserState.Verified && user.IsEmailConfirmed || 
                           _options.SkipVerify(type, user.RegistrationCountry);
                case AuthenticationMethod.Mobile:
                    return user.State == UserState.Verified && user.IsMobileConfirmed ||
                           _options.SkipVerify(type, user.RegistrationCountry);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private void SetConfirmationKey(AuthenticationMethod type, UserEntity user, string code)
        {
            switch (type)
            {
                case AuthenticationMethod.Email:
                    user.EmailConfirmationKey = code; return;
                case AuthenticationMethod.Mobile:
                    user.MobileConfirmationKey = code; return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
