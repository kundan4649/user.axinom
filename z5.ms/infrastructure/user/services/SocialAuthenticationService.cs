using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using z5.ms.common.abstractions;
using z5.ms.common.extensions;
using z5.ms.common.infrastructure.events;
using z5.ms.common.infrastructure.events.model;
using z5.ms.domain.user;
using z5.ms.domain.user.viewmodels;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.user;
using z5.ms.infrastructure.user.repositories;
using System.Linq;
using System.Collections.Generic;
using IdentityServer4.Services;
using z5.ms.infrastructure.user.identity;
using z5.ms.domain.subscription.viewmodel;

namespace z5.ms.infrastructure.user.services
{
    /// <summary>Service for register and login using social media</summary>
    public interface ISocialAuthenticationService
    {
        /// <summary>
        /// Register using social media
        /// </summary>
        /// <remarks>Social media platforms can be Facebook, Google, Twitter, Amazon</remarks>
        Task<Result<UserEntity>> Register(RegisterSocialUserCommand command);

        /// <summary>
        /// Login using social media
        /// </summary>
        /// <remarks>Social media platforms can be Facebook, Google, Twitter, Amazon</remarks>
        Task<Result<UserEntity>> Login(LoginSocialUserCommand command);
    }

    /// <inheritdoc />
    public class SocialAuthenticationService : ISocialAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISocialProfileService _socialProfileService;
        private readonly IMapper _mapper;
        private readonly IEventPublisher<UserEvent> _eventPublisher;
        private readonly UserServiceOptions _options;
        private readonly UserErrors _errors;
        private readonly IProfileService ProfileService;
        private readonly ISubscriptionAPIService _subscriptionAPIService;
        /// <inheritdoc />

        public SocialAuthenticationService(IUserRepository userRepository, ISocialProfileService socialProfileService, IMapper mapper, IEventPublisher<UserEvent> eventPublisher, IOptions<UserServiceOptions> options, IOptions<UserErrors> errors, IProfileService profileService, ISubscriptionAPIService subscriptionAPIService)
        {
            _socialProfileService = socialProfileService;
            _mapper = mapper;
            _eventPublisher = eventPublisher;
            _options = options.Value;
            _userRepository = userRepository;
            _errors = errors.Value;
            ProfileService = profileService;
            _subscriptionAPIService = subscriptionAPIService;
        }

        /// <inheritdoc />
        public async Task<Result<UserEntity>> Register(RegisterSocialUserCommand command)
        {
            var profile = await _socialProfileService.GetProfile(command.Type, command.AccessToken);
            if(!profile.Success)
                return Result<UserEntity>.FromError(profile);

            var existing = await _userRepository.GetUser(command.Type, profile.Value.Id);
            if (existing != null)
                return Result<UserEntity>.FromError(_errors.UserAlreadyActivated);

            var existingUser = string.IsNullOrEmpty(profile.Value.Email) ? null
                : await _userRepository.GetUser(AuthenticationMethod.Email, profile.Value.Email);
            string duplicateProfilesIds = string.Empty;
            if (existingUser != null)
            {
                typeof(UserEntity).GetProperty(command.Type.EnumMemberValue())?.SetValue(existingUser, profile.Value.Id);
                existingUser.State = UserState.Verified;
                existingUser.IsEmailConfirmed = true;
                if (existingUser.ActivationDate == null)
                    existingUser.ActivationDate = DateTime.UtcNow;

                var updateResult = await _userRepository.Update(existingUser);
                if (!updateResult.Success)
                    return Result<UserEntity>.FromError(updateResult);

                await _eventPublisher.Publish(_mapper.Map(existingUser, new UserEvent { Type = UserEventType.Update }));
                return Result<UserEntity>.FromValue(existingUser);
            }
            else
            {
                if (string.IsNullOrEmpty(profile.Value.Email))
                    duplicateProfilesIds = (await GetAllProfiles(command.Type, command.AccessToken, profile.Value.Id)).Item3;
            }
            var user = _mapper.Map<UserEntity>(command);
            user.Id = Guid.NewGuid();
            user.State = UserState.Verified;
            user.PasswordHash = $"{command.Type.EnumMemberValue()}_user";
            user.System = _options.DefaultSystemType;
            user.ActivationDate = DateTime.UtcNow;
            user.IsEmailConfirmed = !string.IsNullOrWhiteSpace(profile.Value.Email);
            user.CreationDate = DateTime.UtcNow;
            typeof(UserEntity).GetProperty(command.Type.EnumMemberValue())?.SetValue(user, profile.Value.Id);
            user.FirstName = profile.Value.FirstName;
            user.LastName = profile.Value.LastName;
            user.Email = profile.Value.Email;
            user.DuplicateProfileIds = duplicateProfilesIds;

            var insertResult = await _userRepository.Insert(user);
            if (!insertResult.Success)
                return Result<UserEntity>.FromError(insertResult);

            await _subscriptionAPIService.CreatePromotionalSubscription(user.Id.ToString(), command.IpAddress, command.RegistrationCountry);
            await _eventPublisher.Publish(_mapper.Map(user, new UserEvent { Type = UserEventType.Create }));
            return Result<UserEntity>.FromValue(user);
        }

        /// <inheritdoc />
        public async Task<Result<UserEntity>> Login(LoginSocialUserCommand command)
        {
            var profile = await _socialProfileService.GetProfile(command.Type, command.AccessToken);
            if (!profile.Success)
                return Result<UserEntity>.FromError(profile);

            var user = await _userRepository.GetUser(command.Type, profile.Value.Id);

            if (user == null || user.State != UserState.Verified)
                return Result<UserEntity>.FromError(_errors.UserNotFound, 404);
            if (string.IsNullOrEmpty(profile.Value.Email))
            {
                switch (command.Type)
                {
                    case AuthenticationMethod.Facebook:
                        user = await SetUserProfile(command, profile, user);
                        break;
                    case AuthenticationMethod.Email:
                    case AuthenticationMethod.Mobile:
                    case AuthenticationMethod.Google:
                    case AuthenticationMethod.Twitter:
                    case AuthenticationMethod.Amazon:
                    default:
                        break;
                }
            }
            _userRepository.SetLastlogin(user.Id);

            return Result<UserEntity>.FromValue(user);
        }
        //Currently available only for Facebook. Requires modifications in the method to address all other social logins.
        async Task<UserEntity> SetUserProfile(LoginSocialUserCommand command, Result<SocialProfile> profile, UserEntity user)
        {
            var getDuplicateProfilesResponse = await GetAllProfiles(command.Type, command.AccessToken, profile.Value.Id);

            if (getDuplicateProfilesResponse.Item2.Count > 0)
            {
                //query for subscription existence on profile.value.id & getduplicateprofilesresponse.item2.ids
                //select the user profile which has a subscription plan that has max number of devices allowed
                //var availableSusbscriptionsJson = getDuplicateProfilesResponse.Item2.Select(async i => await ((CustomProfileService)ProfileService).GetUserSubscriptions(i.Id.ToString()));
                var availableSusbscriptions = new List<InternalSubscription2>();
                var allProfiles = getDuplicateProfilesResponse.Item2?.Union(new List<UserEntity> { user }).ToList();

                allProfiles.ForEach(i =>
                {
                    //var subscriptions = Newtonsoft.Json.JsonConvert.DeserializeObject<List<InternalSubscription2>>(((CustomProfileService)ProfileService).GetUserSubscriptions(i.Id.ToString()).Result);
                    ////subscriptions.ForEach(j => j.SubscriptionPlan.Price = j.SubscriptionPlan.Price.Substring(0, j.SubscriptionPlan.Price.IndexOf('.')));
                    availableSusbscriptions.AddRange(Newtonsoft.Json.JsonConvert.DeserializeObject<List<InternalSubscription2>>(((CustomProfileService)ProfileService).GetUserSubscriptions(i.Id.ToString()).Result));
                });

                //allProfiles.ForEach(i =>
                //   availableSusbscriptions.AddRange(Newtonsoft.Json.JsonConvert.DeserializeObject<List<InternalSubscription2>>(System.IO.File.ReadAllText(@"C:\Users\pavankumar.g\Desktop\Misc\response_1604487085561.json"))));

                if (availableSusbscriptions.Count > 0)
                {
                    var choosenUserId = availableSusbscriptions.OrderByDescending(i => i.SubscriptionPlan.NumberOfSupportedDevices).Select(i => i.UserId).FirstOrDefault();

                    user = allProfiles.FirstOrDefault(i => i.Id == choosenUserId);
                }
                else//////////below line to be made generic for all social logins. Its made static for facebook for now.
                    user = allProfiles.FirstOrDefault(i => i.FacebookUserId == getDuplicateProfilesResponse.Item1.Where(j => j.AppId == "771609066382248").Select(j => j.SocialProfileId).FirstOrDefault()) ?? user;
            }

            user.DuplicateProfileIds = getDuplicateProfilesResponse.Item3;

            return user;
        }
        async Task<(List<SocialAppProfiles>, List<UserEntity>, string)> GetAllProfiles(AuthenticationMethod authenticationMethod, string socialAccessToken, string socialProfileId)
        {
            var duplicateProfiles = new List<SocialAppProfiles>();
            string duplicateProfileIds = string.Empty;
            var users = new List<UserEntity>();

            switch (authenticationMethod)
            {
                case AuthenticationMethod.Facebook:
                    break;
                case AuthenticationMethod.Mobile:
                case AuthenticationMethod.Email:
                case AuthenticationMethod.Google:
                case AuthenticationMethod.Twitter:
                case AuthenticationMethod.Amazon:
                default:
                    return (duplicateProfiles, users, duplicateProfileIds);
            }

            duplicateProfiles = (await _socialProfileService.GetProfiles(authenticationMethod, socialAccessToken, socialProfileId)).Value;
            //var idsForQueryClause = string.Join(",", duplicateProfiles.Where(i => i.SocialProfileId != socialProfileId).Select(i => "'" + i.SocialProfileId + "'"));
            var idsForQueryClause = string.Join(",", duplicateProfiles.Where(i => i.SocialProfileId != socialProfileId).Select(i => i.SocialProfileId));
            var result = duplicateProfiles.Where(i => i.SocialProfileId != socialProfileId).Select(i => _userRepository.GetUser(authenticationMethod, i.SocialProfileId).Result).Where(i => i != null).ToList();
            if (result.Count > 0)
                users.AddRange(result);
            //users = await _userRepository.GetUsers(authenticationMethod, idsForQueryClause);

            if (users.Count > 0)
            {
                if (authenticationMethod == AuthenticationMethod.Facebook)
                    duplicateProfileIds = string.Join(",", users.Select(i => i.FacebookUserId));
            }

            return (duplicateProfiles, users, duplicateProfileIds);
        }
    }
}
