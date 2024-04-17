using System.Threading.Tasks;
using z5.ms.infrastructure.user.repositories;
using MediatR;
using z5.ms.common.abstractions;
using z5.ms.domain.user.watchhistory;

namespace z5.ms.infrastructure.user.watchhistory
{
    /// <summary>Handler for deleting a catalog item from user's watch history command</summary>
    public class DeleteWatchhistoryCommandHandler : IAsyncRequestHandler<DeleteWatchhistoryCommand, Result<Success>>
    {
        private readonly IWatchHistoryRepository _watchHistoryRepository;

        /// <inheritdoc />
        public DeleteWatchhistoryCommandHandler(IWatchHistoryRepository watchHistoryRepository)
        {
            _watchHistoryRepository = watchHistoryRepository;
        }

        /// <inheritdoc />
        public async Task<Result<Success>> Handle(DeleteWatchhistoryCommand message)
        {
            var result = await _watchHistoryRepository.DeleteItemAsync(message.UserId.Value, message.Item);

            if (!result.Success)
                return Result<Success>.FromError(result.Error);

            return Result<Success>.FromValue(new Success {Code = 1, Message = "Delete successful"});
        }
    }
}