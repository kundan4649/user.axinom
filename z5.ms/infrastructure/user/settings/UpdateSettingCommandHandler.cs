using System.Threading.Tasks;
using z5.ms.infrastructure.user.repositories;
using MediatR;
using z5.ms.common.abstractions;
using z5.ms.domain.user.settings;

namespace z5.ms.infrastructure.user.settings
{
    /// <summary>Update user's single setting command handler</summary>
    public class UpdateSettingCommandHandler : IAsyncRequestHandler<UpdateSettingCommand, Result<Success>>
    {
        private readonly ISettingsRepository _repository;

        /// <inheritdoc />
        public UpdateSettingCommandHandler(ISettingsRepository repository)
        {
            _repository = repository;
        }

        /// <inheritdoc />
        public async Task<Result<Success>> Handle(UpdateSettingCommand message)
        {
            var result = await _repository.UpdateItemAsync(message.UserId.Value, message.Item);
            if (result.Success)
                return Result<Success>.FromValue(new Success());

            return Result<Success>.FromError(result.Error);
        }
    }
}