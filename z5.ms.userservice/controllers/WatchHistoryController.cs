using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using z5.ms.common;
using z5.ms.common.abstractions;
using z5.ms.common.assets.common;
using z5.ms.common.extensions;
using z5.ms.common.validation;
using z5.ms.common.validation.authproviders;
using z5.ms.domain.user.viewmodels;
using z5.ms.infrastructure.user.repositories;

namespace z5.ms.userservice.controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize(typeof(JwtAuthProvider), typeof(OAuthJwtAuthProvider))]
    [Route("/v1/watchhistory")]
    public class WatchHistoryController : Controller
    {
        private readonly IWatchHistoryRepository _watchHistoryRepository;
        private Guid CurrentUserId => User.GetCurrentUserId();

        /// <inheritdoc />
        public WatchHistoryController(IWatchHistoryRepository watchHistoryRepository)
        {
            _watchHistoryRepository = watchHistoryRepository;
        }

        /// <summary>
        /// Delete item from the watch history
        /// </summary>
        /// <remarks>Delete an item from the watch history of the current user</remarks>
        /// <param name="id"></param>
        /// <param name="assetType"></param>
        /// <response code="200">Success</response>
        [HttpDelete]
        [ProducesResponseType(typeof(Success), 200)]
        public virtual async Task<IActionResult> DeleteWatchHistory([Required] [FromQuery]string id, [Required] [FromQuery(Name = "asset_type")]AssetType assetType = AssetType.Unknown)
        {
            if (assetType == AssetType.Unknown)
                return new JsonErrorResult(new Error
                {
                    Code = 3,
                    Message = "Invalid asset type"
                });

            if (id.GetAssetTypeOrDefault() != (int)assetType)
                return new JsonErrorResult(new Error
                {
                    Code = 3,
                    Message = "Asset type is not matched with the specified asset id"
                });

            var result = await _watchHistoryRepository.DeleteItemAsync(CurrentUserId, new CatalogItem { AssetId = id, AssetType = assetType});

            if (!result.Success)
                return new JsonErrorResult(result);

            return Json(new Success
            {
                Code = 1,
                Message = "Delete successful"
            });
        }

        /// <summary>
        /// Get watch history items
        /// </summary>
        /// <remarks>Get the watchhistory of the user</remarks>
        /// <response code="200">Watchhistory</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<CatalogItem>), 200)]
        public virtual async Task<IActionResult> GetWatchHistory()
        {
            var result = await _watchHistoryRepository.GetItemsAsync(CurrentUserId);

            if (!result.Success)
                return new JsonErrorResult(result);

            return Json(result.Value);
        }

        /// <summary>
        /// Watch history by asset id
        /// </summary>
        /// <remarks>Get the watchhistory of the user filtered by asset id</remarks>
        /// <response code="200">Watchhistory</response>
        [HttpGet("{asset_id}")]
        [ProducesResponseType(typeof(CatalogItem), 200)]
        public virtual async Task<IActionResult> GetWatchHistoryByAssetId([FromRoute(Name = "asset_id")]string assetId)
        {
            var result = await _watchHistoryRepository.GetItemsByAssetIdAsync(CurrentUserId, assetId);

            if (!result.Success)
                return new JsonErrorResult(result);

            return Json(result.Value);
        }

        /// <summary>
        /// Add item to watch history
        /// </summary>
        /// <remarks>Add an item to the watch history of the current user</remarks>
        /// <param name="add"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Success), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> AddWatchHistory([Required][FromBody]CatalogItem add)
        {
            if (add == null)
                return new JsonErrorResult(new Error
                {
                    Code = 3,
                    Message = "No item in request"
                });

            if (add.AssetType == AssetType.Unknown)
                return new JsonErrorResult(new Error
                {
                    Code = 3,
                    Message = "Invalid asset type"
                });

            if (add.AssetId.GetAssetTypeOrDefault() != (int)add.AssetType)
                return new JsonErrorResult(new Error
                {
                    Code = 3,
                    Message = "Asset type is not matched with the specified asset id"
                });

            var result = await _watchHistoryRepository.AddItemAsync(CurrentUserId, add);

            if (!result.Success)
                return new JsonErrorResult(result);

            return Json(new Success
            {
                Code = 1,
                Message = "Watch history was added successfully"
            });
        }

        

        /// <summary>
        /// Update item in the watch history
        /// </summary>
        /// <remarks>Update an item in the watch history of the current user</remarks>
        /// <param name="update"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        /// <response code="404">Not found. A resource (user, favorite...) could not be found.</response>
        [HttpPut]
        [ProducesResponseType(typeof(Success), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 404)]
        public virtual async Task<IActionResult> UpdateWatchHistory([Required][FromBody]CatalogItem update)
        {
            if (update == null)
                return new JsonErrorResult(new Error
                {
                    Code = 3,
                    Message = "No item in request"
                });
            update.Date = DateTime.UtcNow; //Updating date when watch history was re-watched in the same bucket to get asset in 1st place.
            var result = await _watchHistoryRepository.UpdateItemAsync(CurrentUserId, update);

            if (!result.Success)
                return new JsonErrorResult(result);

            return Json(new Success
            {
                Code = 1,
                Message = "Watch history was updated successfully"
            });
        }

        
    }
}
