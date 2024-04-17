using System;
using System.Threading.Tasks;
using AutoMapper;
using z5.ms.domain.user.user;
using z5.ms.common.abstractions;
using MediatR;
using Microsoft.Extensions.Options;
using z5.ms.common.infrastructure.events;
using z5.ms.common.infrastructure.events.model;
using z5.ms.domain.user;
using z5.ms.domain.user.datamodels;
using z5.ms.infrastructure.user.repositories;

namespace z5.ms.infrastructure.user.user
{
    /// <summary>Handler for delete user command</summary>
    public class DeleteUserCommandHandler : IAsyncRequestHandler<DeleteUserCommand, Result<Success>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IEventPublisher<UserEvent> _eventPublisher;
        private readonly UserErrors _errors;

        /// <inheritdoc />
        public DeleteUserCommandHandler(IUserRepository userRepository, IMapper mapper, IEventPublisher<UserEvent> eventPublisher, IOptions<UserErrors> errors)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _eventPublisher = eventPublisher;
            _errors = errors.Value;
        }
        
        /// <inheritdoc />
        public async Task<Result<Success>> Handle(DeleteUserCommand command)
        {
            var user = await _userRepository.Get(command.UserId);
            if (user == null)
                return Result<Success>.FromError(_errors.UserNotFound, 404);

            user.State = UserState.Deleted;
            user.PasswordHash = string.Empty;

            if (!string.IsNullOrEmpty(user.Email))
            {
                user.Email = $"{user.Email}_deleted_{DateTime.UtcNow:yyyyMMddhhmmss}";
            }

            if (!string.IsNullOrEmpty(user.Mobile))
            {
                user.Mobile = $"{user.Mobile}_deleted_{DateTime.UtcNow:yyyyMMddhhmmss}";
            }

            user.FacebookUserId = "";
            user.GoogleUserId = "";
            user.TwitterUserId = "";
            user.AmazonUserId = "";

            var updateResult = await _userRepository.Update(user);
            if (!updateResult.Success)
                return Result<Success>.FromError(updateResult);

            // raise event
            await _eventPublisher.Publish(_mapper.Map(user, new UserEvent { Type = UserEventType.Delete }));

            return Result<Success>.FromValue(new Success { Code = 1, Message = "User was deleted successfully" });
        }
    }
}