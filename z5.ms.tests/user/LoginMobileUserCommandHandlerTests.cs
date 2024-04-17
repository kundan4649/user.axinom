using System;
using System.Threading.Tasks;
using z5.ms.common.abstractions;
using Moq;
using Xunit;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.user;
using z5.ms.domain.user.viewmodels;
using z5.ms.infrastructure.user.identity;
using z5.ms.infrastructure.user.services;
using z5.ms.infrastructure.user.user;

namespace user
{
    /// <summary>Tests for login user with Mobile command handler</summary>
    public class LoginMobileUserCommandHandlerTests
    {
        private readonly Mock<IAuthenticationService> _authenticationService = new Mock<IAuthenticationService>();
        private readonly Mock<IAuthTokenService> _tokenService = new Mock<IAuthTokenService>();

        /// <summary>Ensure that handler return success with valid login Mobile user command</summary>
        [Fact]
        public async Task LoginMobileUserCommand_Success()
        {
            // Arrange
            var command = new LoginMobileUserCommand { Country = "EE", Refresh = true };
            var user = new UserEntity { Id = Guid.NewGuid() };
            var token = "token";

            _authenticationService.Setup(r => r.Login(command))
                .ReturnsAsync(Result<UserEntity>.FromValue(user));

            _tokenService.Setup(r => r.GetJwtToken(user.Id, command.Country, command.Refresh,0))
                .ReturnsAsync(Result<OAuthToken>.FromValue(new OAuthToken { AccessToken = token }));

            var handler = new LoginMobileUserCommandHandler(_authenticationService.Object, _tokenService.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(token, result.Value.AccessToken);
        }

        /// <summary>Ensure that handler return error when reading user by Mobile fails from user repository</summary>
        [Fact]
        public async Task LoginMobileUserCommand_Fail()
        {
            // Arrange
            var command = new LoginMobileUserCommand { Country = "EE", Refresh = true };
            var user = new UserEntity { Id = Guid.NewGuid() };
            var error = new Error { Code = 1, Message = "Error message" };

            _authenticationService.Setup(r => r.Login(command))
                .ReturnsAsync(Result<UserEntity>.FromValue(user));

            _tokenService.Setup(r => r.GetJwtToken(user.Id, command.Country, command.Refresh,0))
                .ReturnsAsync(Result<OAuthToken>.FromError(error));

            var handler = new LoginMobileUserCommandHandler(_authenticationService.Object, _tokenService.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(error, result.Error);
        }
    }
}