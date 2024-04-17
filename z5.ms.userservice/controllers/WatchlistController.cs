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
    [Route("/v1/watchlist")]
    public class WatchlistController : Controller
    {
        private readonly IWatchlistRepository _watchlistRepository;
        private Guid CurrentUserId => User.GetCurrentUserId();

        /// <inheritdoc />
        public WatchlistController(IWatchlistRepository watchlistRepository)
        {
            _watchlistRepository = watchlistRepository;
        }

        /// <summary>
        /// Delete item from the watch list
        /// </summary>
        /// <remarks>Delete an item from the watch list of the current user</remarks>
        /// <param name="id"></param>
        /// <param name="assetType"></param>
        /// <response code="200">Success</response>
        [HttpDelete]
        [Route("")]
        [ProducesResponseType(typeof(Success), 200)]
        public virtual async Task<IActionResult> DeleteWatchlist([Required] [FromQuery]string id,[Required] [FromQuery(Name = "asset_type")]AssetType assetType = AssetType.Unknown)
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

            var result = await _watchlistRepository.DeleteItemAsync(CurrentUserId, new CatalogItem { AssetId = id, AssetType = assetType});

            if (!result.Success)
                return new JsonErrorResult(result);

            return Json(new Success
            {
                Code = 1,
                Message = "Delete successful"
            });
        }

        /// <summary>
        /// Delete item from the watch list
        /// </summary>
        /// <remarks>Delete an item from the watch list of the current user</remarks>
        /// <param name="id"></param>
        /// <param name="assetType"></param>
        /// <param name="current_profile_id"></param>
        /// <response code="200">Success</response>
        [HttpDelete]
        [Route("/v2/watchlist")]
        [ProducesResponseType(typeof(Success), 200)]
        public virtual async Task<IActionResult> DeleteWatchlistV2([Required][FromQuery] string id, [Required][FromQuery] Guid current_profile_id, [Required][FromQuery(Name = "asset_type")] AssetType assetType = AssetType.Unknown)
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

            var result = await _watchlistRepository.DeleteItemAsync(current_profile_id, new CatalogItem { AssetId = id, AssetType = assetType });

            if (!result.Success)
                return new JsonErrorResult(result);

            return Json(new Success
            {
                Code = 1,
                Message = "Delete successful"
            });
        }


        /// <summary>
        /// Watchlist
        /// </summary>
        /// <remarks>Get the watchlist of the user</remarks>
        /// <response code="200">Watchlist</response>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(List<CatalogItem>), 200)]
        public virtual async Task<IActionResult> GetWatchlist()
        {
            var result = await _watchlistRepository.GetItemsAsync(CurrentUserId);

            if (!result.Success)
                return new JsonErrorResult(result);

            return Json(result.Value);
        }

        /// <summary>
        /// Watchlist
        /// </summary>
        /// <param name="current_profile_id"></param>
        /// <remarks>Get the watchlist of the user</remarks>
        /// <response code="200">Watchlist</response>
        [HttpGet]
        [Route("/v2/watchlist")]
        [ProducesResponseType(typeof(List<CatalogItem>), 200)]
        public virtual async Task<IActionResult> GetWatchlistV2([FromQuery] Guid current_profile_id)
        {
            if (current_profile_id == Guid.Empty)
                current_profile_id = CurrentUserId;
             var result = await _watchlistRepository.GetItemsAsync(current_profile_id);

            if (!result.Success)
                return new JsonErrorResult(result);

            return Json(result.Value);
        }

        /// <summary>
        /// Add item to watch list
        /// </summary>
        /// <remarks>Add an item to the watch list of the current user</remarks>
        /// <param name="add"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(Success), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> AddWatchlist([Required][FromBody]CatalogItem add)
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

            var result = await _watchlistRepository.AddItemAsync(CurrentUserId, add);

            if (!result.Success)
                return new JsonErrorResult(result);

            return Json(new Success
            {
                Code = 1,
                Message = "Watchlist was added successfully"
            });
        }

        /// <summary>
        /// Add item to watch list
        /// </summary>
        /// <remarks>Add an item to the watch list of the current user</remarks>
        /// <param name="add"></param>
        /// <param name="current_profile_id"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [Route("/v2/watchlist")]
        [ProducesResponseType(typeof(Success), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> AddWatchlistV2([Required][FromBody] CatalogItem add, [Required][FromQuery] Guid current_profile_id)
        {

            if (current_profile_id == Guid.Empty)
                return new JsonErrorResult(new Error
                {
                    Code = 3,
                    Message = "Please provide profile id"
                });

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

            var result = await _watchlistRepository.AddItemAsync(current_profile_id, add);

            if (!result.Success)
                return new JsonErrorResult(result);

            return Json(new Success
            {
                Code = 1,
                Message = "Watchlist was added successfully"
            });
        }


      

        /// <summary>
        /// Update item in the watch list
        /// </summary>
        /// <remarks>Update an item in the watch list of the current user</remarks>
        /// <param name="update"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        /// <response code="404">Not found. A resource (user, favorite...) could not be found.</response>
        [HttpPut]
        [Route("")]
        [ProducesResponseType(typeof(Success), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 404)]
        public virtual async Task<IActionResult> UpdateWatchlist([Required][FromBody]CatalogItem update)
        {
            if (update == null)
                return new JsonErrorResult(new Error
                {
                    Code = 3,
                    Message = "No item in request"
                });
            var result = await _watchlistRepository.UpdateItemAsync(CurrentUserId, update);

            if (!result.Success)
                return new JsonErrorResult(result);

            return Json(new Success
            {
                Code = 1,
                Message = "Watchlist was updated successfully"
            });
        }
    }
}
