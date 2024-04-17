using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using z5.ms.common.abstractions;
using z5.ms.common.extensions;
using z5.ms.domain.user;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.user;
using z5.ms.domain.user.viewmodels;
using z5.ms.infrastructure.user.repositories;

namespace z5.ms.infrastructure.user.services
{
    /// <summary>Service for confirmation of the contact information</summary>
    public interface IConfirmationService
    {
        /// <summary>
        /// Confirm users contact info
        /// </summary>
        /// <remarks>Verification of email or mobile number</remarks>
        Task<Result<Success>> Confirm(ConfirmUserCommand command);

        /// <summary>
        /// Confirm users contact info
        /// </summary>
        /// <remarks>Verification of email or mobile number</remarks>
        Task<Result<Success>> Confirmv2(ConfirmUserCommandv2 command);

        /// <summary>
        /// Regenerates a confirmation code to verify contact info
        /// </summary>
        Task<Result<UserEntity>> ReCreateConfirmationCode(ResendConfirmationCommand command);

        /// <summary>
        /// Generates a confirmation code for contact info updates
        /// </summary>
        /// <param name="type">Authentication method type(Email,Mobile)</param>
        /// <param name="userId">Id of the user</param>
        /// <param name="userName">username of the user (can be email or mobile number)</param>
        /// <returns></returns>
        Task<Result<UserEntity>> ConfirmationCodeForUpdate(AuthenticationMethod type, Guid userId, string userName);
    }

    /// <inheritdoc />
    public class ConfirmationService : IConfirmationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IOneTimePassRepository _otpRepository;
        private readonly UserServiceOptions _options;
        private readonly UserErrors _errors;

        /// <inheritdoc />
        public ConfirmationService(IUserRepository userRepository, IOneTimePassRepository otpRepository, IOptions<UserServiceOptions> options, IOptions<UserErrors> errors)
        {
            _userRepository = userRepository;
            _otpRepository = otpRepository;
            _errors = errors.Value;
            _options = options.Value;
        }

        /// <inheritdoc />
        public async Task<Result<Success>> Confirm(ConfirmUserCommand command)
        {
            var otp = await _otpRepository.ValidateCode(GetOtpGroup(command.Type), command.Code);
            if (!otp.Success)
                return Result<Success>.FromError(otp);

            var user = await _userRepository.Get(otp.Value.UserId);
            if (user == null || user.State == UserState.Deleted)
                return Result<Success>.FromError(_errors.ConfirmationNotFound, 404);

            SetConfirmed(command.Type, user, otp.Value.RecipientAddress);

            var updateResult = await _userRepository.Update(user);
            return !updateResult.Success
                ? Result<Success>.FromError(updateResult)
                : Result<Success>.FromValue(new Success { Code = 1, Message = "Confirmation successful" });
        }

        /// <inheritdoc />
        public async Task<Result<Success>> Confirmv2(ConfirmUserCommandv2 command)
        {
            var otp = await _otpRepository.ValidateCodev2(GetOtpGroup(command.Type), command);
            if (!otp.Success)
                return Result<Success>.FromError(otp);

            var user = await _userRepository.Get(otp.Value.UserId);
            if (user == null || user.State == UserState.Deleted)
                return Result<Success>.FromError(_errors.ConfirmationNotFound, 404);

            if (command.RecipientAddress == user.Email || command.RecipientAddress == user.Mobile)
            {
                SetConfirmed(command.Type, user, otp.Value.RecipientAddress);

                var updateResult = await _userRepository.Update(user);
                return !updateResult.Success
                    ? Result<Success>.FromError(updateResult)
                    : Result<Success>.FromValue(new Success { Code = 1, Message = "Confirmation successful" });
            }
            else
            {
                return Result<Success>.FromError(_errors.UserNotFound);
            }
        }

        /// <inheritdoc />
        public async Task<Result<UserEntity>> ReCreateConfirmationCode(ResendConfirmationCommand command)
        {
            //username can be either email or mobile number
            var userName = command.GetValue<string>(command.Type.EnumMemberValue());
            var user = await _userRepository.GetUser(command.Type, userName);

            if (user == null)
                return Result<UserEntity>.FromError(_errors.UserNotFound, 404);

            if (IsConfirmed(command.Type, user))
                return Result<UserEntity>.FromError(_errors.UserAlreadyActivated);

            var otp = await _otpRepository.CreateCode(user.Id, userName, GetOtpDescriptor(command.Type, command.Version));
           
            SetConfirmationKey(command.Type, user, otp.Value.Code);

            return Result<UserEntity>.FromValue(user);
        }

        /// <inheritdoc />
        public async Task<Result<UserEntity>> ConfirmationCodeForUpdate(AuthenticationMethod type, Guid userId, string userName)
        {
            var user = await _userRepository.Get(userId);
            if (user == null)
                return Result<UserEntity>.FromError(_errors.UserNotFound, 404);

            if (!IsVerified(type, user))
                return Result<UserEntity>.FromError(_errors.NotActivated);

            var exists = await _userRepository.GetUser(type, userName);
            if (exists != null)
                return Result<UserEntity>.FromError(_errors.EmailOrMobileAlreadyUsed);

            //commented this and disable the update query to the db to address ZNA-4905
            //if (SetUserName(type, user, userName))
            //    await _userRepository.Update(user);
            //else
            //    //return Result<UserEntity>.FromValue(user);
            //    return Result<UserEntity>.FromError(type == AuthenticationMethod.Mobile ? _errors.MobileUnconfirmed : _errors.EmailUnconfirmed);

            if (!SetUserName(type, user, userName))
                return Result<UserEntity>.FromError(type == AuthenticationMethod.Mobile ? _errors.MobileUnconfirmed : _errors.EmailUnconfirmed);

            var otp = await _otpRepository.CreateCode(user.Id, userName, GetOtpDescriptor(type, 2));

            SetConfirmationKey(type, user, otp.Value.Code);

            return Result<UserEntity>.FromValue(user);
        }

        private OtpGroup GetOtpGroup(AuthenticationMethod type)
        {
            switch (type)
            {
                case AuthenticationMethod.Email:
                    return OtpGroup.ConfirmationEmail;
                case AuthenticationMethod.Mobile:
                    return OtpGroup.ConfirmationMobile;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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

        private static void SetConfirmed(AuthenticationMethod type, UserEntity user, string recipient)
        {
            switch (type)
            {
                case AuthenticationMethod.Email:
                    user.Email = recipient;
                    user.IsEmailConfirmed = true;
                    break;
                case AuthenticationMethod.Mobile:
                    user.Mobile = recipient;
                    user.IsMobileConfirmed = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            user.ActivationDate = user.ActivationDate ?? DateTime.UtcNow;
            user.State = UserState.Verified;
        }

        private bool IsVerified(AuthenticationMethod type, UserEntity user)
        {
            switch (type)
            {
                case AuthenticationMethod.Email:
                    return user.State == UserState.Verified ||
                           _options.SkipVerify(type, user.RegistrationCountry);
                case AuthenticationMethod.Mobile:
                    return user.State == UserState.Verified ||
                           _options.SkipVerify(type, user.RegistrationCountry);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private bool IsConfirmed(AuthenticationMethod type, UserEntity user)
        {
            switch (type)
            {
                case AuthenticationMethod.Email:
                    return user.IsEmailConfirmed;
                case AuthenticationMethod.Mobile:
                    return user.IsMobileConfirmed;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private static void SetConfirmationKey(AuthenticationMethod type, UserEntity user, string code)
        {
            switch (type)
            {
                case AuthenticationMethod.Email:
                    user.EmailConfirmationKey = code; return;
                case AuthenticationMethod.Mobile:
                    user.MobileConfirmationKey = code; return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool SetUserName(AuthenticationMethod type, UserEntity user, string userName)
        {
            switch (type)
            {
                case AuthenticationMethod.Email:
                    if (user.Email.EqualsIgnoreCase(userName) || !string.IsNullOrWhiteSpace(user.Email) && !_options.SkipVerify(type, user.RegistrationCountry))
                        return false;
                    //for International bug 28120 - When user register with mobile no and update email id then without verification isemailconfirmed is marking as 1.
                    if (user.RegistrationCountry == "IN")
                    {
                        if (user.IsEmailConfirmed || user.IsMobileConfirmed)
                        {
                            user.Email = userName;
                            user.IsEmailConfirmed = true;
                            return true;
                        }
                    }
                    else
                    {
                        user.Email = userName;
                        user.IsEmailConfirmed = false;
                        return true;
                    }
                    return false;
                case AuthenticationMethod.Mobile:
                    if (user.RegistrationCountry == "IN")
                    {
                        if (!user.Mobile.EqualsIgnoreCase(userName) && !string.IsNullOrWhiteSpace(userName))
                        {
                            user.Mobile = userName;
                            user.IsMobileConfirmed = false;
                            return true;
                        }
                        else
                            return false;
                    }
                    else
                    {
                        if (user.Mobile.EqualsIgnoreCase(userName) || !_options.SkipVerify(type, user.RegistrationCountry))
                            return false;

                        user.Mobile = userName;
                        user.IsMobileConfirmed = false;
                        return true;
                    }

                    if (!user.Mobile.EqualsIgnoreCase(userName) && !string.IsNullOrWhiteSpace(userName))
                    {
                        user.Mobile = userName;
                        user.IsMobileConfirmed = false;
                        return true;
                    }
                    //if (user.Mobile.EqualsIgnoreCase(userName) || !string.IsNullOrWhiteSpace(user.Mobile))
                    return false;
                //user.Mobile = userName;
                //user.IsMobileConfirmed = false;
                //return true;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
