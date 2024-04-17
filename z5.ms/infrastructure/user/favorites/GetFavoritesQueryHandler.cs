using System.Collections.Generic;
using System.Threading.Tasks;
using z5.ms.infrastructure.user.repositories;
using MediatR;
using z5.ms.common.abstractions;
using z5.ms.domain.user.favorites;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.infrastructure.user.favorites
{
    /// <summary>Handler for getting user favorites query</summary>
    public class GetFavoritesQueryHandler : IAsyncRequestHandler<GetFavoritesQuery, Result<List<CatalogItem>>>                
    {
        private readonly IFavoritesRepository _repository;
        
        /// <inheritdoc />
        public GetFavoritesQueryHandler(IFavoritesRepository repository)
        {
            _repository = repository;            
        }

        /// <inheritdoc />
        public async Task<Result<List<CatalogItem>>> Handle(GetFavoritesQuery message) =>
            await _repository.GetItemsAsync(message.UserId.Value);
    }
}