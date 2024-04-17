using System;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using z5.ms.common.abstractions;
using z5.ms.common.infrastructure.id;
using z5.ms.domain.user;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.user;
using z5.ms.domain.user.viewmodels;
using z5.ms.infrastructure.user.repositories;
using z5.ms.infrastructure.user.services;

namespace services
{
    public class ConfirmationServiceTests
    {
        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<IOneTimePassRepository> _otpRepository = new Mock<IOneTimePassRepository>();
        private readonly IOptions<UserServiceOptions> _options = Options.Create(new UserServiceOptions());
        private readonly IOptions<UserErrors> _errors = Options.Create(new UserErrors());

        [Fact]
        public async void ConfirmEmail_Success()
        {
            // Arrange
            var command = new ConfirmUserCommand { Type = AuthenticationMethod.Email, Code = "asdf1234" };
            var user = new UserEntity { Id = Guid.NewGuid(), State = UserState.Registered };
            var otp = new OneTimePassEntity{ UserId = user.Id, Code = command.Code, RecipientAddress = "test@test.com" };

            _otpRepository.Setup(r => r.ValidateCode(OtpGroup.ConfirmationEmail, command.Code))
                .ReturnsAsync(Result<OneTimePassEntity>.FromValue(otp));

            _userRepository.Setup(r => r.Get(user.Id))
                .ReturnsAsync(user);

            _userRepository.Setup(r => r.Update(It.IsAny<UserEntity>()))
                .ReturnsAsync(Result<Success>.FromValue(new Success()));

            var service = new ConfirmationService(_userRepository.Object, _otpRepository.Object, _options, _errors);

            // Act
            var result = await service.Confirm(command);

            // Assert
            Assert.True(result.Success, result.Error?.Message);
        }

        [Fact]
        public async void ConfirmMobile_Success()
        {
            // Arrange
            var command = new ConfirmUserCommand { Type = AuthenticationMethod.Mobile, Code = "asdf1234" };
            var user = new UserEntity { Id = Guid.NewGuid(), State = UserState.Registered };
            var otp = new OneTimePassEntity { UserId = user.Id, Code = command.Code, RecipientAddress = "123456789" };

            _otpRepository.Setup(r => r.ValidateCode(OtpGroup.ConfirmationMobile, command.Code))
                .ReturnsAsync(Result<OneTimePassEntity>.FromValue(otp));

            _userRepository.Setup(r => r.Get(user.Id))
                .ReturnsAsync(user);

            _userRepository.Setup(r => r.Update(It.IsAny<UserEntity>()))
                .ReturnsAsync(Result<Success>.FromValue(new Success()));

            var service = new ConfirmationService(_userRepository.Object, _otpRepository.Object, _options, _errors);

            // Act
            var result = await service.Confirm(command);

            // Assert
            Assert.True(result.Success, result.Error?.Message);
        }

        [Fact]
        public async void ReCreateConfirmationCodeEmail_Success()
        {
            // Arrange
            var command = new ResendConfirmationEmailCommand { Email = "test@test.com" };
            var user = new UserEntity { Id = Guid.NewGuid(), State = UserState.Registered};
            var otp = new OneTimePassEntity { UserId = user.Id, Code = "asdf1234" };

            _userRepository.Setup(r => r.GetUser(AuthenticationMethod.Email, command.Email))
                .ReturnsAsync(user);

            _otpRepository.Setup(r => r.CreateCode(user.Id, command.Email, It.IsAny<OtpDescriptor>()))
                .ReturnsAsync(Result<OneTimePassEntity>.FromValue(otp));

            var service = new ConfirmationService(_userRepository.Object, _otpRepository.Object, _options, _errors);

            // Act
            var result = await service.ReCreateConfirmationCode(command);

            // Assert
            Assert.True(result.Success, result.Error?.Message);
        }

        [Fact]
        public async void ReCreateConfirmationCodeMobile_Success()
        {
            // Arrange
            var command = new ResendConfirmationSmsCommand { Mobile = "123456789" };
            var user = new UserEntity { Id = Guid.NewGuid(), State = UserState.Registered };
            var otp = new OneTimePassEntity { UserId = user.Id, Code = "asdf1234" };

            _userRepository.Setup(r => r.GetUser(AuthenticationMethod.Mobile, command.Mobile))
                .ReturnsAsync(user);

            _otpRepository.Setup(r => r.CreateCode(user.Id, command.Mobile, It.IsAny<OtpDescriptor>()))
                .ReturnsAsync(Result<OneTimePassEntity>.FromValue(otp));

            var service = new ConfirmationService(_userRepository.Object, _otpRepository.Object, _options, _errors);

            // Act
            var result = await service.ReCreateConfirmationCode(command);

            // Assert
            Assert.True(result.Success, result.Error?.Message);
        }

        [Fact]
        public async void ConfirmationCodeForUpdateEmail_Success()
        {
            // Arrange
            var email = "123456789";
            var user = new UserEntity { Id = Guid.NewGuid(), State = UserState.Verified };
            var otp = new OneTimePassEntity { UserId = user.Id, Code = "asdf1234" };

            _userRepository.Setup(r => r.Get(user.Id))
                .ReturnsAsync(user);

            _userRepository.Setup(r => r.GetUser(AuthenticationMethod.Email, email))
                .ReturnsAsync((UserEntity)null);

            _userRepository.Setup(r => r.Update(user))
                .ReturnsAsync(Result<Success>.FromValue(new Success()));

            _otpRepository.Setup(r => r.CreateCode(user.Id, email, It.IsAny<OtpDescriptor>()))
                .ReturnsAsync(Result<OneTimePassEntity>.FromValue(otp));

            var service = new ConfirmationService(_userRepository.Object, _otpRepository.Object, _options, _errors);

            // Act
            var result = await service.ConfirmationCodeForUpdate(AuthenticationMethod.Email, user.Id, email);

            // Assert
            Assert.True(result.Success, result.Error?.Message);
        }

        [Fact]
        public async void ConfirmationCodeForUpdateMobile_Success()
        {
            // Arrange
            var mobile = "123456789";
            var user = new UserEntity { Id = Guid.NewGuid(), State = UserState.Verified };
            var otp = new OneTimePassEntity { UserId = user.Id, Code = "asdf1234" };

            _userRepository.Setup(r => r.Get(user.Id))
                .ReturnsAsync(user);

            _userRepository.Setup(r => r.GetUser(AuthenticationMethod.Mobile, mobile))
                .ReturnsAsync((UserEntity)null);

            _userRepository.Setup(r => r.Update(user))
                .ReturnsAsync(Result<Success>.FromValue(new Success()));

            _otpRepository.Setup(r => r.CreateCode(user.Id, mobile, It.IsAny<OtpDescriptor>()))
                .ReturnsAsync(Result<OneTimePassEntity>.FromValue(otp));

            var service = new ConfirmationService(_userRepository.Object, _otpRepository.Object, _options, _errors);

            // Act
            var result = await service.ConfirmationCodeForUpdate(AuthenticationMethod.Mobile, user.Id, mobile);

            // Assert
            Assert.True(result.Success, result.Error?.Message);
        }
    }
}