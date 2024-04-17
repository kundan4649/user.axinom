using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using z5.ms.common.abstractions;
using z5.ms.common.infrastructure.events;
using z5.ms.common.infrastructure.events.model;
using z5.ms.common.notifications;
using z5.ms.domain.user;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.user;
using z5.ms.domain.user.viewmodels;
using z5.ms.infrastructure.user.repositories;
using z5.ms.infrastructure.user.services;
using System.Linq;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Net.Http;

namespace z5.ms.infrastructure.user.user
{
    /// <summary>Handler for updating user details command</summary>
    public class UpdateUserCommandHandler : IAsyncRequestHandler<UpdateUserCommand, Result<Success>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfirmationService _confirmationService;
        private readonly INotificationClient _notificationClient;
        private readonly IMapper _mapper;
        private readonly IEventPublisher<UserEvent> _eventPublisher;
        private readonly UserErrors _errors;
        private readonly ILogger _logger;
        IUserProfileUpdateHistoryRepository _userProfileUpdateHistoryRepository { get; }

        IHipiHandler HipiHandler { get; }
        /// <inheritdoc />
        public UpdateUserCommandHandler(IUserRepository userRepository, IConfirmationService confirmationService, INotificationClient notificationClient,
            IMapper mapper, IEventPublisher<UserEvent> eventPublisher, IOptions<UserErrors> errors, IHipiHandler hipiHandler, IUserProfileUpdateHistoryRepository userProfileUpdateHistoryRepository, ILoggerFactory loggerFactory)
        {
            _userRepository = userRepository;
            _confirmationService = confirmationService;
            _notificationClient = notificationClient;
            _mapper = mapper;
            _eventPublisher = eventPublisher;
            _errors = errors.Value;
            HipiHandler = hipiHandler;
            _logger = loggerFactory.CreateLogger("UpdateUserCommandHandler");
            _userProfileUpdateHistoryRepository = userProfileUpdateHistoryRepository;
        }

        /// <inheritdoc />
        public async Task<Result<Success>> Handle(UpdateUserCommand command)
        {
            var existingUserProfile = await _userRepository.Get(command.UserId);

            var result = await UpdateUser(command, existingUserProfile);

            if (!result.Success)
                return Result<Success>.FromError(result);

            var confirmationResult = await SendConfirmation(command.UserId, command.Email, command.Mobile);
            if (!confirmationResult.Success)
                return Result<Success>.FromError(confirmationResult);

            var hipiUpdateResult = await HipiHandler.PUTDataToHipiEndPoint(JsonConvert.SerializeObject(
                new HipiPutUserProfile { FirstName = command.FirstName, LastName = command.LastName, Dob = command.Birthday, Email = command.Email, PhoneNumber = command.Mobile }
                ), command.UserId.ToString());

            if (!hipiUpdateResult.Success)
                _logger.LogError($"Error while updating Hipi Put Node : {hipiUpdateResult.Error?.Message}");

            await _userProfileUpdateHistoryRepository.AddItemAsync(new UserProfileUpdateHistoryItem
            {
                UserId = command.UserId,
                EmailId = existingUserProfile.Email,
                MobileNumber = existingUserProfile.Mobile,
                IpAddress = command.IpAddress,
                CountryCode = command.CountryCode,
                RequestPayload = command.RawRequest
            });
            
            return Result<Success>.FromValue(new Success { Code = 1, Message = "Update successful" });
        }

        private async Task<Result<Success>> UpdateUser(UpdateUserCommand command, UserEntity user)
        {
            //var user = await _userRepository.Get(command.UserId);
            if (user == null)
                return Result<Success>.FromError(_errors.UserNotFound, 404);

            // only update non empty values
            if (!string.IsNullOrWhiteSpace(command.FirstName)) user.FirstName = command.FirstName;
            if (!string.IsNullOrWhiteSpace(command.LastName)) user.LastName = command.LastName;
            if (command.Birthday != null) user.Birthday = command.Birthday;
            if (command.Gender != null) user.Gender = command.Gender;
            if (command.MacAddress != null) user.MacAddress = command.MacAddress;
            var updateResult = await _userRepository.Update(user);
            if (!updateResult.Success)
                return Result<Success>.FromError(updateResult);

            // raise event
            await _eventPublisher.Publish(_mapper.Map(user, new UserEvent { Type = UserEventType.Update }));

            return Result<Success>.FromValue(new Success());
        }

        private async Task<Result<Success>> SendConfirmation(Guid userId, string email, string mobile)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                var emailResult = await _confirmationService.ConfirmationCodeForUpdate(AuthenticationMethod.Email, userId, email);
                if (!emailResult.Success)
                    return Result<Success>.FromError(emailResult);

                if (emailResult.Value.Email != email || !emailResult.Value.IsEmailConfirmed)
                {
                    _notificationClient.SendEmailAddEmail(email, emailResult.Value.EmailConfirmationKey, emailResult.Value.RegistrationCountry);
                    _notificationClient.SendEmailAddSms(emailResult.Value.Mobile, email, emailResult.Value.RegistrationCountry);
                }
            }

            if (!string.IsNullOrWhiteSpace(mobile))
            {
                var mobileResult = await _confirmationService.ConfirmationCodeForUpdate(AuthenticationMethod.Mobile, userId, mobile);
                if (!mobileResult.Success)
                    return Result<Success>.FromError(mobileResult);

                if (mobileResult.Value.Mobile != mobile || !mobileResult.Value.IsMobileConfirmed)
                {
                    _notificationClient.SendMobileAddSms(mobile, mobileResult.Value.MobileConfirmationKey, mobileResult.Value.RegistrationCountry);
                    _notificationClient.SendMobileAddEmail(mobileResult.Value.Email, mobile, mobileResult.Value.RegistrationCountry);
                }
            }

            return Result<Success>.FromValue(new Success());
        }
    }
}