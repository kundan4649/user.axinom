using System;
using AutoMapper;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using z5.ms.common.abstractions;
using z5.ms.common.infrastructure.events;
using z5.ms.common.infrastructure.events.model;
using z5.ms.common.infrastructure.id;
using z5.ms.domain.user;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.user;
using z5.ms.domain.user.viewmodels;
using z5.ms.infrastructure.user.repositories;
using z5.ms.infrastructure.user.services;

namespace services
{
    public class SocialAuthorizationServiceTests
    {
        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<ISocialProfileService> _socialProfileService = new Mock<ISocialProfileService>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IEventPublisher<UserEvent>> _eventPublisher = new Mock<IEventPublisher<UserEvent>>();
        private readonly IOptions<UserServiceOptions> _options = Options.Create(new UserServiceOptions());
        private readonly IOptions<UserErrors> _errors = Options.Create(new UserErrors());

        public SocialAuthorizationServiceTests()
        {
            _mapper.Setup(r => r.Map(It.IsAny<UserEntity>(), It.IsAny<UserEvent>()))
                .Returns(new UserEvent());
            _eventPublisher.Setup(r => r.Publish(It.IsAny<UserEvent>()))
                .ReturnsAsync(Result<Success>.FromValue(new Success()));
        }

        [Theory]
        [InlineData(AuthenticationMethod.Facebook)]
        [InlineData(AuthenticationMethod.Google)]
        [InlineData(AuthenticationMethod.Twitter)]
        [InlineData(AuthenticationMethod.Amazon)]
        public async void Register_Success(AuthenticationMethod type)
        {
            // Arrange
            var command = new RegisterSocialUserCommand { Type = type, AccessToken = "token" };
            var profile = new SocialProfile { Id = "social_id", Email = "test@test.com" };

            _socialProfileService.Setup(r => r.GetProfile(command.Type, command.AccessToken))
                .ReturnsAsync(Result<SocialProfile>.FromValue(profile));

            _userRepository.Setup(r => r.GetUser(command.Type, profile.Id))
                .ReturnsAsync((UserEntity)null);

            _userRepository.Setup(r => r.GetUser(AuthenticationMethod.Email, profile.Email))
                .ReturnsAsync((UserEntity)null);

            _mapper.Setup(r => r.Map<UserEntity>(command))
                .Returns(new UserEntity());

            _userRepository.Setup(r => r.Insert(It.IsAny<UserEntity>()))
                .ReturnsAsync(Result<Success>.FromValue(new Success()));

           // var service = new SocialAuthenticationService(_userRepository.Object, _socialProfileService.Object, 
            //    _mapper.Object, _eventPublisher.Object, _options, _errors);

            // Act
          //  var result = await service.Register(command);

            // Assert
         //   Assert.True(result.Success, result.Error?.Message);
        }

        [Theory]
        [InlineData(AuthenticationMethod.Facebook)]
        [InlineData(AuthenticationMethod.Google)]
        [InlineData(AuthenticationMethod.Twitter)]
        [InlineData(AuthenticationMethod.Amazon)]
        public async void Login_Success(AuthenticationMethod type)
        {
            // Arrange
            var command = new LoginSocialUserCommand { Type = type, AccessToken = "token" };
            var profile = new SocialProfile { Id = "social_id", Email = "test@test.com" };

            _socialProfileService.Setup(r => r.GetProfile(command.Type, command.AccessToken))
                .ReturnsAsync(Result<SocialProfile>.FromValue(profile));

            _userRepository.Setup(r => r.GetUser(command.Type, profile.Id))
                .ReturnsAsync(new UserEntity { State = UserState.Verified });

            //var service = new SocialAuthenticationService(_userRepository.Object, _socialProfileService.Object,
            //    _mapper.Object, _eventPublisher.Object, _options, _errors);

          //  _userRepository.Setup(r => r.SetLastlogin(It.IsAny<Guid>()));

            // Act
          //  var result = await service.Login(command);

            // Assert
          //  Assert.True(result.Success, result.Error?.Message);
        }
    }
}