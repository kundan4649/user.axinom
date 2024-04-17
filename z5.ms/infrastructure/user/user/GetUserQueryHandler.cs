using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using z5.ms.common.abstractions;
using z5.ms.domain.user;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.user;
using z5.ms.domain.user.viewmodels;
using z5.ms.infrastructure.user.repositories;

namespace z5.ms.infrastructure.user.user
{
    /// <summary>Handler for get user query</summary>
    public class GetUserQueryHandler : IAsyncRequestHandler<GetUserQuery, Result<User>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly UserErrors _errors;

        /// <inheritdoc />
        public GetUserQueryHandler(IUserRepository userRepository, IMapper mapper, IOptions<UserErrors> errors)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _errors = errors.Value;
        }
        
        /// <inheritdoc />
        public async Task<Result<User>> Handle(GetUserQuery command)
        {
            var user = await _userRepository.Get(command.UserId);

            if (user == null || user.State == UserState.Deleted)
                return Result<User>.FromError(_errors.UserNotFound, 404);

            var result = _mapper.Map<UserEntity, User>(user);
            result.IsPasswordAttached = !user.PasswordHash.EndsWith("_user");
            return Result<User>.FromValue(result);
        }
    }
}