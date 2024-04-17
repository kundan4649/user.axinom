using System.Threading.Tasks;
using z5.ms.domain.user.settings;
using z5.ms.infrastructure.user.repositories;
using z5.ms.common.abstractions;
using MediatR;

namespace z5.ms.infrastructure.user.settings
{
    /// <summary>Command handler for remobing a setting</summary>
    public class DeleteSettingCommandHandler : IAsyncRequestHandler<DeleteSettingCommand, Result<Success>>
    {
        private readonly ISettingsRepository _repository;
        
        /// <inheritdoc />
        public DeleteSettingCommandHandler(ISettingsRepository repository)
        {
            _repository = repository;
        }
        
        /// <inheritdoc />
        public async Task<Result<Success>> Handle(DeleteSettingCommand message)
        {
            var result = await _repository.DeleteItemAsync(message.UserId.Value, message.Item);
            
            if (result.Success)
                return Result<Success>.FromValue(new Success());
            
            return Result<Success>.FromError(result.Error);
        }
    }
}