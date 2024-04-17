using System;
using AutoMapper;
using Microsoft.Extensions.Logging;
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
    public class AuthorizationServiceTests
    {
        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IEventPublisher<UserEvent>> _eventPublisher = new Mock<IEventPublisher<UserEvent>>();
        private readonly Mock<IPasswordEncryptionStrategy> _passwordStrategy = new Mock<IPasswordEncryptionStrategy>();
        private readonly Mock<IOneTimePassRepository> _otpRepository = new Mock<IOneTimePassRepository>();
        private readonly IOptions<UserServiceOptions> _options = Options.Create(new UserServiceOptions());
        private readonly IOptions<UserErrors> _errors = Options.Create(new UserErrors());
        private readonly Mock<ISubscriptionAPIService> _subscriptionAPIService = new Mock<ISubscriptionAPIService>();
        private readonly Mock<ILogger<AuthenticationService>> _logger = new Mock<ILogger<AuthenticationService>>();
        public AuthorizationServiceTests()
        {
            _mapper.Setup(r => r.Map(It.IsAny<UserEntity>(), It.IsAny<UserEvent>()))
                .Returns(new UserEvent());
            _eventPublisher.Setup(r => r.Publish(It.IsAny<UserEvent>()))
                .ReturnsAsync(Result<Success>.FromValue(new Success()));
        }

        [Fact]
        public async void RegisterEmail_Success()
        {
            // Arrange
            var command = new RegisterEmailUserCommand { Type = AuthenticationMethod.Email, Email = "test@test.com", Password = "123456" };
            var otp = new OneTimePassEntity { UserId = new Guid(), Code = "asdf1234" };
            var passwordHash = "passhash";

            _userRepository.Setup(r => r.GetUser(command.Type, command.Email))
                .ReturnsAsync((UserEntity)null);

            _mapper.Setup(r => r.Map<UserEntity>(command))
                .Returns(new UserEntity());

            _passwordStrategy.Setup(r => r.HashPassword(passwordHash))
                .Returns(passwordHash);

            _otpRepository.Setup(r => r.CreateCode(It.IsAny<Guid>(), command.Email, It.IsAny<OtpDescriptor>()))
                .ReturnsAsync(Result<OneTimePassEntity>.FromValue(otp));

            _userRepository.Setup(r => r.Insert(It.IsAny<UserEntity>()))
                .ReturnsAsync(Result<Success>.FromValue(new Success()));

            _subscriptionAPIService.Setup(r => r.GetSubscriptionPlanDetails("IN")).ReturnsAsync((string)null);

            var service = new AuthenticationService(_userRepository.Object, _mapper.Object, _eventPublisher.Object,
                _passwordStrategy.Object, _otpRepository.Object, _options, _errors, _subscriptionAPIService.Object, _logger.Object);

            // Act
            var result = await service.Register(command);

            // Assert
            Assert.True(result.Success, result.Error?.Message);
        }

        [Fact]
        public async void RegisterMobile_Success()
        {
            // Arrange
            var command = new RegisterMobileUserCommand { Type = AuthenticationMethod.Mobile, Mobile = "123456789", Password = "123456" };
            var otp = new OneTimePassEntity { UserId = new Guid(), Code = "asdf1234" };
            var passwordHash = "passhash";

            _userRepository.Setup(r => r.GetUser(command.Type, command.Mobile))
                .ReturnsAsync((UserEntity)null);

            _mapper.Setup(r => r.Map<UserEntity>(command))
                .Returns(new UserEntity());

            _passwordStrategy.Setup(r => r.HashPassword(passwordHash))
                .Returns(passwordHash);

            _otpRepository.Setup(r => r.CreateCode(It.IsAny<Guid>(), command.Mobile, It.IsAny<OtpDescriptor>()))
                .ReturnsAsync(Result<OneTimePassEntity>.FromValue(otp));

            _userRepository.Setup(r => r.Insert(It.IsAny<UserEntity>()))
                .ReturnsAsync(Result<Success>.FromValue(new Success()));

            _subscriptionAPIService.Setup(r => r.GetSubscriptionPlanDetails("IN")).ReturnsAsync((string)null);

            var service = new AuthenticationService(_userRepository.Object, _mapper.Object, _eventPublisher.Object,
                _passwordStrategy.Object, _otpRepository.Object, _options, _errors, _subscriptionAPIService.Object,_logger.Object);

            // Act
            var result = await service.Register(command);

            // Assert
            Assert.True(result.Success, result.Error?.Message);
        }

        [Fact]
        public async void LoginEmail_Success()
        {
            // Arrange
            var command = new LoginEmailUserCommand { Type = AuthenticationMethod.Email, Email = "test@test.com", Password = "123456"};
            var user = new UserEntity { Id = Guid.NewGuid(), State = UserState.Verified, IsEmailConfirmed = true, PasswordHash = "passhash"};

            _userRepository.Setup(r => r.GetUser(command.Type, command.Email))
                .ReturnsAsync(user);

            _passwordStrategy.Setup(r => r.VerifyPassword(command.Password, user.PasswordHash))
                .Returns(true);

            _userRepository.Setup(r => r.SetLastlogin(user.Id));

            var service = new AuthenticationService(_userRepository.Object, _mapper.Object, _eventPublisher.Object, 
                _passwordStrategy.Object, _otpRepository.Object, _options, _errors, _subscriptionAPIService.Object, _logger.Object);

            // Act
            var result = await service.Login(command);

            // Assert
            Assert.True(result.Success, result.Error?.Message);
        }

        [Fact]
        public async void LoginMobile_Success()
        {
            // Arrange
            var command = new LoginMobileUserCommand { Type = AuthenticationMethod.Mobile, Mobile = "123456789", Password = "123456" };
            var user = new UserEntity { Id = Guid.NewGuid(), State = UserState.Verified, IsMobileConfirmed = true, PasswordHash = "passhash" };

            _userRepository.Setup(r => r.GetUser(command.Type, command.Mobile))
                .ReturnsAsync(user);

            _passwordStrategy.Setup(r => r.VerifyPassword(command.Password, user.PasswordHash))
                .Returns(true);

            _userRepository.Setup(r => r.SetLastlogin(user.Id));

            var service = new AuthenticationService(_userRepository.Object, _mapper.Object, _eventPublisher.Object,
                _passwordStrategy.Object, _otpRepository.Object, _options, _errors, _subscriptionAPIService.Object, _logger.Object);

            // Act
            var result = await service.Login(command);

            // Assert
            Assert.True(result.Success, result.Error?.Message);
        }
    }
}