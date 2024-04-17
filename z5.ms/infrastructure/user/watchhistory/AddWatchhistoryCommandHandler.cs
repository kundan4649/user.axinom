using System.Threading.Tasks;
using z5.ms.infrastructure.user.repositories;
using MediatR;
using z5.ms.common.abstractions;
using z5.ms.domain.user.watchhistory;

namespace z5.ms.infrastructure.user.watchhistory
{
    /// <summary>Handler for adding a new catalog item to user's watch history command</summary>
    public class AddWatchhistoryCommandHandler : IAsyncRequestHandler<AddWatchhistoryCommand, Result<Success>>
    {
        private readonly IWatchHistoryRepository _watchHistoryRepository;

        /// <inheritdoc />
        public AddWatchhistoryCommandHandler(IWatchHistoryRepository watchHistoryRepository)
        {
            _watchHistoryRepository = watchHistoryRepository;
        }
        
        /// <inheritdoc />
        public async Task<Result<Success>> Handle(AddWatchhistoryCommand message)
        {
            var result = await _watchHistoryRepository.AddItemAsync(message.UserId.Value, message.Item);
            
            if (!result.Success)
                return Result<Success>.FromError(result);

            return Result<Success>.FromValue(new Success
            {
                Code = 1,
                Message = "Watch history was added successfully"
            });
        }
    }
}