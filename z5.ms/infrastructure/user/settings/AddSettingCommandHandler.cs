using System.Threading.Tasks;
using z5.ms.infrastructure.user.repositories;
using MediatR;
using z5.ms.common.abstractions;
using z5.ms.domain.user.settings;

namespace z5.ms.infrastructure.user.settings
{
    /// <summary>Command handler for adding a new setting</summary>
    public class AddSettingCommandHandler : IAsyncRequestHandler<AddSettingCommand, Result<Success>>
    {
        private readonly ISettingsRepository _repository;
        
        /// <inheritdoc />
        public AddSettingCommandHandler(ISettingsRepository repository)
        {
            _repository = repository;
        }
        
        /// <inheritdoc />
        public async Task<Result<Success>> Handle(AddSettingCommand message)
        {
            var result = await _repository.AddItemAsync(message.UserId.Value, message.Item);
            
            if (result.Success)
                return Result<Success>.FromValue(new Success());
            
            return Result<Success>.FromError(result.Error);
        }
    }
}