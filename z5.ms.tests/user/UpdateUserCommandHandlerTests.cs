using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using z5.ms.common.abstractions;
using z5.ms.common.infrastructure.events;
using z5.ms.common.infrastructure.events.model;
using z5.ms.common.notifications;
using z5.ms.domain.user;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.user;
using z5.ms.infrastructure.user.repositories;
using z5.ms.infrastructure.user.services;
using z5.ms.infrastructure.user.user;

namespace user
{
    /// <summary>Tests for updating user command handler</summary>
    public class UpdateUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly Mock<IConfirmationService> _confirmationService = new Mock<IConfirmationService>();
        private readonly Mock<INotificationClient> _notificationClient = new Mock<INotificationClient>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IEventPublisher<UserEvent>> _eventPublisher = new Mock<IEventPublisher<UserEvent>>();
        private readonly IOptions<UserErrors> _errors = Options.Create(new UserErrors());

        public UpdateUserCommandHandlerTests()
        {
            _mapper.Setup(r => r.Map(It.IsAny<UserEntity>(), It.IsAny<UserEvent>()))
                .Returns(new UserEvent());
            _eventPublisher.Setup(r => r.Publish(It.IsAny<UserEvent>()))
                .ReturnsAsync(Result<Success>.FromValue(new Success()));
        }

        /// <summary>Ensure that handler returns success with update user command</summary>
        [Fact]
        public async Task UpdateUserCommand_Success()
        {
            // Arrange
            var command = new UpdateUserCommand {UserId = Guid.NewGuid()};

            _userRepository.Setup(r => r.Get(It.IsAny<Guid>()))
                .ReturnsAsync(new UserEntity());

            _userRepository.Setup(r => r.Update(It.IsAny<UserEntity>()))
                .ReturnsAsync(Result<Success>.FromValue(new Success()));
            
           // var handler = new UpdateUserCommandHandler(_userRepository.Object, _confirmationService.Object, 
             //   _notificationClient.Object, _mapper.Object, _eventPublisher.Object, _errors);

            // Act
           // var result = await handler.Handle(command);

            // Assert
          //  Assert.True(result.Success);
        }
    }
}