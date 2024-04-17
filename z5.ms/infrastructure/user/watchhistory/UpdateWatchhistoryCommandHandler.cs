using System.Threading.Tasks;
using z5.ms.infrastructure.user.repositories;
using MediatR;
using z5.ms.common.abstractions;
using z5.ms.domain.user.watchhistory;

namespace z5.ms.infrastructure.user.watchhistory
{
    /// <summary>Handler for updating an existing catalog item in user's watch history command</summary>
    public class UpdateWatchhistoryCommandHandler : IAsyncRequestHandler<UpdateWatchhistoryCommand, Result<Success>>
    {
        private readonly IWatchHistoryRepository _watchHistoryRepository;

        /// <inheritdoc />
        public UpdateWatchhistoryCommandHandler(IWatchHistoryRepository watchHistoryRepository)
        {
            _watchHistoryRepository = watchHistoryRepository;
        }
        
        /// <inheritdoc />
        public async Task<Result<Success>> Handle(UpdateWatchhistoryCommand message)
        {
            var result = await _watchHistoryRepository.UpdateItemAsync(message.UserId.Value, message.Item);

            if (!result.Success)
                return Result<Success>.FromError(result.Error);

            return Result<Success>.FromValue(new Success
            {
                Code = 1,
                Message = "Watch history was updated successfully"
            });
        }
    }
}