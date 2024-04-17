using System.Collections.Generic;
using System.Threading.Tasks;
using z5.ms.infrastructure.user.repositories;
using MediatR;
using z5.ms.common.abstractions;
using z5.ms.domain.user.settings;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.infrastructure.user.settings
{
    /// <summary>Handler for getting user settings query</summary>
    public class GetSettingsQueryHandler : IAsyncRequestHandler<GetSettingsQuery, Result<List<SettingItem>>>
    {
        private readonly ISettingsRepository _repository;
        
        /// <inheritdoc />
        public GetSettingsQueryHandler(ISettingsRepository repository)
        {
            _repository = repository;            
        }

        /// <inheritdoc />
        public async Task<Result<List<SettingItem>>> Handle(GetSettingsQuery message) =>
            await _repository.GetItemsAsync(message.UserId.Value);
    }
}