using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using z5.ms.common.abstractions;
using z5.ms.common.infrastructure.events;
using z5.ms.common.infrastructure.events.model;
using z5.ms.domain.user;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.user;
using z5.ms.infrastructure.user.repositories;
using z5.ms.infrastructure.user.user;

namespace user
{
    /// <summary>Tests for delete user command handler</summary>
    public class DeleteUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IEventPublisher<UserEvent>> _eventPublisher = new Mock<IEventPublisher<UserEvent>>();
        private readonly IOptions<UserErrors> _errors = Options.Create(new UserErrors());

        public DeleteUserCommandHandlerTests()
        {
            _mapper.Setup(r => r.Map(It.IsAny<UserEntity>(), It.IsAny<UserEvent>()))
                .Returns(new UserEvent());
            _eventPublisher.Setup(r => r.Publish(It.IsAny<UserEvent>()))
                .ReturnsAsync(Result<Success>.FromValue(new Success()));
        }

        /// <summary>Ensure that handler returns success with delete user command</summary>
        [Fact]
        public async Task DeleteUserCommand_Success()
        {
            // Arrange
            var command = new DeleteUserCommand { UserId = Guid.NewGuid() };

            _userRepository.Setup(r => r.Get(It.IsAny<Guid>()))
                .ReturnsAsync(new UserEntity());
            _userRepository.Setup(r => r.Update(It.IsAny<UserEntity>()))
                .ReturnsAsync(Result<Success>.FromValue(new Success()));

            var handler = new DeleteUserCommandHandler(_userRepository.Object, _mapper.Object, _eventPublisher.Object, _errors);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.True(result.Success);
        }

        /// <summary>Ensure that handler returns success with delete user command</summary>
        [Fact]
        public async Task DeleteUserCommand_GetUser_Fail()
        {
            // Arrange
            var command = new DeleteUserCommand { UserId = Guid.NewGuid() };

            _userRepository.Setup(r => r.Get(It.IsAny<Guid>()))
                .ReturnsAsync((UserEntity)null);

            var handler = new DeleteUserCommandHandler(_userRepository.Object, _mapper.Object, _eventPublisher.Object, _errors);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(_errors.Value.UserNotFound, result.Error);
        }

        /// <summary>Ensure that handler returns success with delete user command</summary>
        [Fact]
        public async Task DeleteUserCommand_UpdateUser_Fail()
        {
            // Arrange
            var command = new DeleteUserCommand { UserId = Guid.NewGuid() };
            var error = new Error { Code = 123, Message = "error" };
            _userRepository.Setup(r => r.Get(It.IsAny<Guid>()))
                .ReturnsAsync(new UserEntity());
            _userRepository.Setup(r => r.Update(It.IsAny<UserEntity>()))
                .ReturnsAsync(Result<Success>.FromError(error));

            var handler = new DeleteUserCommandHandler(_userRepository.Object, _mapper.Object, _eventPublisher.Object, _errors);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(error, result.Error);
        }
    }
}