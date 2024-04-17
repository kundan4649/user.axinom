using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using z5.ms.common.abstractions;
using z5.ms.common.extensions;
using z5.ms.common.infrastructure.id;
using z5.ms.common.notifications;
using z5.ms.domain.user;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.user;
using z5.ms.domain.user.viewmodels;
using z5.ms.infrastructure.user.repositories;

namespace z5.ms.infrastructure.user.services
{
    /// <summary>Service for resetting user passwords using confirmation email / sms</summary>
    public interface IPasswordService
    {
        /// <summary>Send a confirmation code to a user to reset user's password</summary>
        Task<Result<Success>> SendPasswordResetNotification(ForgotPasswordCommand command);

        /// <summary>Reset user's password with a confirmation code</summary>
        Task<Result<Success>> ResetPassword(ResetPasswordCommand command);

        /// <summary>Change password of user</summary>
        Task<Result<Success>> ChangePassword(ChangePasswordCommand command);

        /// <summary>Change password of user</summary>
        Task<Result<Success>> ChangePasswordv2(ChangePasswordCommandv2 command);

        /// <summary>Reset user's password with a confirmation code</summary>
        Task<Result<Success>> ResetPasswordv2(ResetPasswordCommandv2 command);
    }

    /// <inheritdoc />
    public class PasswordService : IPasswordService
    {
        private readonly IUserRepository _userRepository;
        private readonly INotificationClient _notificationClient;
        private readonly IOneTimePassRepository _oneTimePassRepository;
        private readonly IPasswordEncryptionStrategy _passwordStrategy;
        private readonly UserServiceOptions _options;
        private readonly UserErrors _errors;
        IUserProfileUpdateHistoryRepository _userProfileUpdateHistoryRepository { get; }

        /// <inheritdoc />
        public PasswordService(IUserRepository userRepository, INotificationClient notificationClient, IOneTimePassRepository oneTimePassRepository,
            IPasswordEncryptionStrategy passwordStrategy, IOptions<UserServiceOptions> options, IUserProfileUpdateHistoryRepository userProfileUpdateHistoryRepository, IOptions<UserErrors> errors)
        {
            _userRepository = userRepository;
            _notificationClient = notificationClient;
            _oneTimePassRepository = oneTimePassRepository;
            _passwordStrategy = passwordStrategy;
            _errors = errors.Value;
            _options = options.Value;
            _userProfileUpdateHistoryRepository = userProfileUpdateHistoryRepository;
        }

        /// <inheritdoc />
        public async Task<Result<Success>> SendPasswordResetNotification(ForgotPasswordCommand command)
        {
            //username can be either email or mobile number
            var userName = command.GetValue<string>(command.Type.EnumMemberValue());
            var user = await _userRepository.GetUser(command.Type, userName);
            if (user == null)
                return Result<Success>.FromError(_errors.UserNotFound, 404);

            if (!IsConfirmed(command.Type, user))
                return Result<Success>.FromError(command.Type == AuthenticationMethod.Mobile 
                    ? _errors.MobileUnconfirmed : _errors.EmailUnconfirmed, 401);

            var otpResult = await _oneTimePassRepository.CreateCode(user.Id, userName, GetOtpDescriptor(command.Type, command.Version));

            if (!otpResult.Success)
                return Result<Success>.FromError(otpResult);

            if (command.Type == AuthenticationMethod.Email)
            {
                _notificationClient.SendPasswordResetEmail(userName, otpResult.Value.Code, user.RegistrationCountry);
            }
            else if (command.Type == AuthenticationMethod.Mobile)
            {
                _notificationClient.SendPasswordResetSms(userName, otpResult.Value.Code, user.RegistrationCountry);
            }


            return Result<Success>.FromValue(new Success { Code = 1, Message = "Notification has been sent to queue" });
        }

        /// <inheritdoc />
        public async Task<Result<Success>> ResetPassword(ResetPasswordCommand command)
        {
            var otp = await _oneTimePassRepository.SingleOrDefaultWhere(nameof(OneTimePassEntity.Code), command.Code);
            if (otp == null || (otp.OtpGroup != OtpGroup.ResetPasswordEmail && otp.OtpGroup != OtpGroup.ResetPasswordMobile))
                return Result<Success>.FromError(_errors.NoConfirmationCode, 404);

            var user = await _userRepository.Get(otp.UserId);
            if (user == null)
                return Result<Success>.FromError(_errors.NoConfirmationCode, 404);

            if (otp.Expires < DateTime.UtcNow || (user.State != UserState.Verified && user.State != UserState.Registered))
                return Result<Success>.FromError(_errors.ConfirmationExpired);

            user.PasswordHash = _passwordStrategy.HashPassword(command.NewPassword);
            user.PasswordResetKey = null;
            user.IsEmailConfirmed = otp.OtpGroup == OtpGroup.ResetPasswordEmail || user.IsEmailConfirmed;
            user.IsMobileConfirmed = otp.OtpGroup == OtpGroup.ResetPasswordMobile || user.IsMobileConfirmed;
            user.State = UserState.Verified;

            var result = await _userRepository.Update(user);
            await _oneTimePassRepository.DeleteCodes(user.Id, otp.OtpGroup);

            if (result.Success)
            {
                if (otp.OtpGroup == OtpGroup.ResetPasswordEmail)
                {
                    _notificationClient.SendPasswordRecreationConfirmationEmail(user.Email, user.RegistrationCountry);
                }
                else if (otp.OtpGroup == OtpGroup.ResetPasswordMobile)
                {
                    _notificationClient.SendPasswordRecreationConfirmationSms(user.Mobile, user.RegistrationCountry);
                }
                await _userProfileUpdateHistoryRepository.AddItemAsync(new UserProfileUpdateHistoryItem
                {
                    UserId = user.Id,
                    EmailId = user.Email,
                    MobileNumber = user.Mobile,
                    IpAddress = command.IpAddress,
                    CountryCode = command.CountryCode,
                    RequestPayload = hidepassword(command.RawRequest),
                    PasswordUpdated = true
                });
            }

            return result.Success
                ? Result<Success>.FromValue(new Success { Code = 1, Message = "Password recreation successful" })
                : result;
        }

        /// <inheritdoc />
        public async Task<Result<Success>> ResetPasswordv2(ResetPasswordCommandv2 command)
        {
            var otp = await _oneTimePassRepository.SingleOrDefaultWhere(nameof(OneTimePassEntity.Code), command.Code, nameof(command.RecipientAddress), command.RecipientAddress);
            if (otp == null || (otp.OtpGroup != OtpGroup.ResetPasswordEmail && otp.OtpGroup != OtpGroup.ResetPasswordMobile))
                return Result<Success>.FromError(_errors.NoConfirmationCode, 404);

            var user = await _userRepository.Get(otp.UserId);
            if (user == null)
                return Result<Success>.FromError(_errors.NoConfirmationCode, 404);

            if (otp.Expires < DateTime.UtcNow || (user.State != UserState.Verified && user.State != UserState.Registered))
                return Result<Success>.FromError(_errors.ConfirmationExpired);

            user.PasswordHash = _passwordStrategy.HashPassword(command.NewPassword);
            user.PasswordResetKey = null;
            user.IsEmailConfirmed = otp.OtpGroup == OtpGroup.ResetPasswordEmail || user.IsEmailConfirmed;
            user.IsMobileConfirmed = otp.OtpGroup == OtpGroup.ResetPasswordMobile || user.IsMobileConfirmed;
            user.State = UserState.Verified;

            var result = await _userRepository.Update(user);
            await _oneTimePassRepository.DeleteCodes(user.Id, otp.OtpGroup);

            if (result.Success)
            {
                if (otp.OtpGroup == OtpGroup.ResetPasswordEmail)
                {
                    _notificationClient.SendPasswordRecreationConfirmationEmail(user.Email, user.RegistrationCountry);
                }
                else if (otp.OtpGroup == OtpGroup.ResetPasswordMobile)
                {
                    _notificationClient.SendPasswordRecreationConfirmationSms(user.Mobile, user.RegistrationCountry);
                }
                await _userProfileUpdateHistoryRepository.AddItemAsync(new UserProfileUpdateHistoryItem
                {
                    UserId = user.Id,
                    EmailId = user.Email,
                    MobileNumber = user.Mobile,
                    IpAddress = command.IpAddress,
                    CountryCode = command.CountryCode,
                    RequestPayload = hidepassword(command.RawRequest),
                    PasswordUpdated = true
                });
            }

            return result.Success
                ? Result<Success>.FromValue(new Success { Code = 1, Message = "Password recreation successful" })
                : result;
        }

        /// <inheritdoc />
        public async Task<Result<Success>> ChangePassword(ChangePasswordCommand command)
        {
            var user = await _userRepository.Get(command.UserId);
            if (user == null)
                return Result<Success>.FromError(_errors.UserNotFound, 404);

            if (!_passwordStrategy.VerifyPassword(command.OldPassword, user.PasswordHash))
                return Result<Success>.FromError(_errors.IncorrectOldPassword);

            user.PasswordHash = _passwordStrategy.HashPassword(command.NewPassword);
            var updateResult = await _userRepository.Update(user);
            if (updateResult.Success)
            {
                await _userProfileUpdateHistoryRepository.AddItemAsync(new UserProfileUpdateHistoryItem
                {
                    UserId = user.Id,
                    EmailId = user.Email,
                    MobileNumber = user.Mobile,
                    IpAddress = command.IpAddress,
                    CountryCode = command.CountryCode,
                    RequestPayload = hidepassword(command.RawRequest),
                    PasswordUpdated = true
                });
            }
            return !updateResult.Success
                ? Result<Success>.FromError(_errors.PasswordUpdate)
                : Result<Success>.FromValue(new Success { Code = 1, Message = "Password was changed successfully" });
        }

        public async Task<Result<Success>> ChangePasswordv2(ChangePasswordCommandv2 command)
        {
            var user = await _userRepository.Get(command.UserId);
            if (user == null)
                return Result<Success>.FromError(_errors.UserNotFound, 404);

            if (!_passwordStrategy.VerifyPassword(command.OldPassword, user.PasswordHash))
                return Result<Success>.FromError(_errors.IncorrectOldPassword);

            if (user.Email == command.RecipientAddress || user.Mobile == command.RecipientAddress)
            {
                user.PasswordHash = _passwordStrategy.HashPassword(command.NewPassword);
                var updateResult = await _userRepository.Update(user);
                if (updateResult.Success)
                {
                    await _userProfileUpdateHistoryRepository.AddItemAsync(new UserProfileUpdateHistoryItem
                    {
                        UserId = user.Id,
                        EmailId = user.Email,
                        MobileNumber = user.Mobile,
                        IpAddress = command.IpAddress,
                        CountryCode = command.CountryCode,
                        RequestPayload = hidepassword(command.RawRequest),
                        PasswordUpdated = true
                    });
                }
                return !updateResult.Success
                    ? Result<Success>.FromError(_errors.PasswordUpdate)
                    : Result<Success>.FromValue(new Success { Code = 1, Message = "Password was changed successfully" });
            }
            else
            {
                return Result<Success>.FromError(_errors.UserNotFound);
            }
        }

        private OtpDescriptor GetOtpDescriptor(AuthenticationMethod type, int version)
        {
            switch (type)
            {
                case AuthenticationMethod.Email:
                    return new OtpDescriptor
                    {
                        OtpGroup = OtpGroup.ResetPasswordEmail,
                        Duration = TimeSpan.FromDays(1),
                        OtpCreationStrategy = OtpCreationStrategy.AlphaNumeric,
                        KeyLength = 8
                    };
                case AuthenticationMethod.Mobile:
                    return version != 2
                        ? new OtpDescriptor
                        {
                            OtpGroup = OtpGroup.ResetPasswordMobile,
                            Duration = TimeSpan.FromMinutes(20),
                            OtpCreationStrategy = OtpCreationStrategy.Numeric,
                            KeyLength = 8
                        }
                        : new OtpDescriptor
                        {
                            OtpGroup = OtpGroup.ResetPasswordMobile,
                            Duration = TimeSpan.FromMinutes(15),
                            OtpCreationStrategy = OtpCreationStrategy.FourDigitPregenerated,
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

        private string hidepassword(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return null;
            var result = JsonConvert.DeserializeObject<JObject>(json);
            var json1 = result.ContainsKey("new_password") ? (result.Property("new_password").Value = "--##--##--") : result;
            var json2 = result.ContainsKey("old_password") ? (result.Property("old_password").Value = "--##--##--") : result;
            return JsonConvert.SerializeObject(result);

        }
    }
}
