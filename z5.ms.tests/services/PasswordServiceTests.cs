using System;
using FluentAssertions;
using z5.ms.common.abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using z5.ms.common.infrastructure.id;
using z5.ms.common.notifications;
using z5.ms.domain.user;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.user;
using z5.ms.domain.user.viewmodels;
using z5.ms.infrastructure.user.repositories;
using z5.ms.infrastructure.user.services;

namespace services
{ 
    public class PasswordServiceTests
    {
        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<INotificationClient> _notificationClient = new Mock<INotificationClient>();
        private readonly Mock<IOneTimePassRepository> _otpRepository = new Mock<IOneTimePassRepository>();
        private readonly IOptions<UserServiceOptions> _options = Options.Create(new UserServiceOptions());
        private readonly IOptions<UserErrors> _errors = Options.Create(new UserErrors());
        private readonly PasswordService _service;
        

        private UserEntity _user = new UserEntity
        {
            Id = Guid.Parse("B57DC02C-0676-45CB-8C68-5DE3D8DA7C79"),
            Email = "exists@email.com",
            Mobile = "12345678",
            System = "Internal",
            State = UserState.Verified
        };

        private OneTimePassEntity _oneTimePass = new OneTimePassEntity
        {
            UserId = Guid.Parse("B57DC02C-0676-45CB-8C68-5DE3D8DA7C79"),
            Code = "abc123",
            Expires = DateTime.MaxValue,
            OtpGroup = OtpGroup.ResetPasswordMobile
        };

        private string _hashedPassword = "HASHASHASHASSH";

        public PasswordServiceTests()
        {
          //  var passwordStrategy = new Mock<IPasswordEncryptionStrategy>();
          ////  _service = new PasswordService(_userRepository.Object, _notificationClient.Object,
          //    //  _otpRepository.Object, passwordStrategy.Object, _options, _errors);

          //  passwordStrategy.Setup(x => x.HashPassword(It.IsAny<String>())).Returns(_hashedPassword);

          //  _userRepository.Setup(x => x.GetUser(AuthenticationMethod.Email, It.IsAny<string>())).ReturnsAsync((UserEntity)null);
          //  _userRepository.Setup(x => x.GetUser(AuthenticationMethod.Email, _user.Email)).ReturnsAsync(_user);
          //  _userRepository.Setup(x => x.GetUser(AuthenticationMethod.Mobile, It.IsAny<string>())).ReturnsAsync((UserEntity)null);
          //  _userRepository.Setup(x => x.GetUser(AuthenticationMethod.Mobile, _user.Mobile)).ReturnsAsync(_user);
          //  _userRepository.Setup(x => x.Update(It.IsAny<UserEntity>())).ReturnsAsync(new Result<Success>());
          //  _userRepository.Setup(x => x.Get(It.IsAny<Guid>())).ReturnsAsync((UserEntity)null);
          //  _userRepository.Setup(x => x.Get(_user.Id)).ReturnsAsync(_user);

          //  _otpRepository.Setup(x => x.CreateCode(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<OtpDescriptor>()))
          //      .ReturnsAsync(Result<OneTimePassEntity>.FromValue(_oneTimePass));
          //  _otpRepository.Setup(x => x.SingleOrDefaultWhere("Code", _oneTimePass.Code, null, null))
          //      .ReturnsAsync(_oneTimePass);
        }

        [Fact]
        public async void SendPasswordResetEmail_ReturnsErrorIfUserIsNotFound()
        {
            var result = await _service.SendPasswordResetNotification(new ForgotPasswordEmailCommand{ Email = "unknown@email.com" });
            Assert.False(result.Success);
        }

        [Fact]
        public async void SendPasswordResetSms_ReturnsErrorIfUserIsNotFound()
        {
            var result = await _service.SendPasswordResetNotification(new ForgotPasswordMobileCommand { Mobile = "987654321" });
            Assert.False(result.Success);
        }

        [Fact]
        public async void RecreatePasswordAsync_ReturnsErrorIfConfirmationKeyIsNotFound()
        {
            var result = await _service.ResetPassword(new ResetPasswordCommand { Code = "12345678", NewPassword = "123456" });
            Assert.False(result.Success);
        }

        [Fact]
        public async void RecreatePasswordAsync_ReturnsSuccessIfConfirmationKeyIsFound()
        {
            var result = await _service.ResetPassword(new ResetPasswordCommand { Code = _oneTimePass.Code, NewPassword = "123456" });
            Assert.True(result.Success);
            _userRepository.Verify(x => x.Update(_user));
            _otpRepository.Verify(x => x.DeleteCodes(_user.Id, _oneTimePass.OtpGroup));
        }

        [Fact]
        public async void RecreatePasswordAsync_UpdatesUserIfConfirmationKeyIsFound()
        {
            var result = await _service.ResetPassword(new ResetPasswordCommand { Code = _oneTimePass.Code, NewPassword = "123456" });
            Assert.True(result.Success);
            _userRepository.Verify(x => x.Update(_user));
            _otpRepository.Verify(x => x.DeleteCodes(_user.Id, _oneTimePass.OtpGroup));
        }

        [Fact]
        public async void RecreatePasswordAsync_DeletesCodesIfConfirmationKeyIsFound()
        {
            var result = await _service.ResetPassword(new ResetPasswordCommand { Code = _oneTimePass.Code, NewPassword = "123456" });
            Assert.True(result.Success);
            _userRepository.Verify(x => x.Update(_user));
            _otpRepository.Verify(x => x.DeleteCodes(_user.Id, _oneTimePass.OtpGroup));
        }

        [Fact]
        public async void RecreatePasswordAsync_HashesUsersPasswordIfConfirmationKeyIsFound()
        {
            var result = await _service.ResetPassword(new ResetPasswordCommand { Code = _oneTimePass.Code, NewPassword = "123456" });
            Assert.True(result.Success);
            _user.PasswordHash.Should().Be(_hashedPassword);
        }

        [Fact]
        public async void RecreatePasswordAsync_ReturnsErrorIfConfirmationKeyIsNotAPasswordResetType()
        {
            _oneTimePass.OtpGroup = default(OtpGroup);
            var result = await _service.ResetPassword(new ResetPasswordCommand { Code = _oneTimePass.Code, NewPassword = "123456" });
            Assert.False(result.Success);
        }

        [Fact]
        public async void RecreatePasswordAsync_ReturnsErrorIfUserIsDeleted()
        {
            _user.State = UserState.Deleted;
            var result = await _service.ResetPassword(new ResetPasswordCommand { Code = _oneTimePass.Code, NewPassword = "123456" });
            Assert.False(result.Success);
        }

        [Fact]
        public async void RecreatePasswordAsync_ReturnsErrorIfConfirmationCodeIsExpired()
        {
            _oneTimePass.Expires = DateTime.MinValue;
            var result = await _service.ResetPassword(new ResetPasswordCommand { Code = _oneTimePass.Code, NewPassword = "123456" });
            Assert.False(result.Success);
        }

        [Fact]
        public async void RecreatePasswordAsync_ActivatesUserEmailIfUserIsNotActivated()
        {
            _user.State = UserState.Registered;
            _oneTimePass.OtpGroup = OtpGroup.ResetPasswordEmail;
            var result = await _service.ResetPassword(new ResetPasswordCommand { Code = _oneTimePass.Code, NewPassword = "123456" });
            Assert.True(result.Success);
            _user.State.Should().Be(UserState.Verified);
            _user.IsEmailConfirmed.Should().Be(true);
            _user.IsMobileConfirmed.Should().Be(false);
        }

        [Fact]
        public async void RecreatePasswordAsync_ActivatesUserMobileIfUserIsNotActivated()
        {
            _user.State = UserState.Registered;
            _oneTimePass.OtpGroup = OtpGroup.ResetPasswordMobile;
            var result = await _service.ResetPassword(new ResetPasswordCommand { Code = _oneTimePass.Code, NewPassword = "123456" });
            Assert.True(result.Success);
            _user.State.Should().Be(UserState.Verified);
            _user.IsMobileConfirmed.Should().Be(true);
            _user.IsEmailConfirmed.Should().Be(false);
        }
    }
}
