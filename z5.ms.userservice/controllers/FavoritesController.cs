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
    [Route("/v1/favorites")]
    [Authorize(typeof(JwtAuthProvider), typeof(OAuthJwtAuthProvider))]
    public class FavoritesController : Controller
    {
        private readonly IFavoritesRepository _favoritesRepository;
        private Guid CurrentUserId => User.GetCurrentUserId();

        /// <inheritdoc />
        public FavoritesController(IFavoritesRepository favoritesRepository)
        {
            _favoritesRepository = favoritesRepository;
        }

        /// <summary>
        /// Delete favorite
        /// </summary>
        /// <remarks>Delete a favorite from the current user</remarks>
        /// <param name="id"></param>
        /// <param name="assetType"></param>
        /// <response code="200">Success</response>
        [HttpDelete]
        [Route("")]
        [ProducesResponseType(typeof(Success), 200)]
        public virtual async Task<IActionResult> DeleteFavorite([Required] [FromQuery] string id,
            [Required] [FromQuery(Name = "asset_type")]
            AssetType assetType = AssetType.Unknown)
        {
            if (assetType == AssetType.Unknown)
                return new JsonErrorResult(new Error
                {
                    Code = 3,
                    Message = "Invalid asset type"
                });

            if (id.GetAssetTypeOrDefault() != (int) assetType)
                return new JsonErrorResult(new Error
                {
                    Code = 3,
                    Message = "Asset type is not matched with the specified asset id"
                });

            var result = await _favoritesRepository.DeleteItemAsync(CurrentUserId, new CatalogItem {AssetId = id, AssetType = assetType});

            if (!result.Success)
                return new JsonErrorResult(result);

            return Json(new Success
            {
                Code = 1,
                Message = "Delete successful"
            });
        }


        /// <summary>
        /// Favorites
        /// </summary>
        /// <remarks>Get the favorites of the user</remarks>
        /// <response code="200">Favorites</response>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(List<CatalogItem>), 200)]
        public virtual async Task<IActionResult> GetFavorites()
        {
            var result = await _favoritesRepository.GetItemsAsync(CurrentUserId);

            if (!result.Success)
                return new JsonErrorResult(result);

            return Json(result.Value);
        }


        /// <summary>
        /// Add a favorite
        /// </summary>
        /// <remarks>Add a favorite for the current user</remarks>
        /// <param name="add"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(Success), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> AddFavorite([Required] [FromBody] CatalogItem add)
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

            if (add.AssetId.GetAssetTypeOrDefault() != (int) add.AssetType)
                return new JsonErrorResult(new Error
                {
                    Code = 3,
                    Message = "Asset type is not matched with the specified asset id"
                });

            var result = await _favoritesRepository.AddItemAsync(CurrentUserId, add);

            if (!result.Success)
                return new JsonErrorResult(result);

            return Json(new Success
            {
                Code = 1,
                Message = "Favorite was added successfully"
            });
        }


        /// <summary>
        /// Update a favorite
        /// </summary>
        /// <remarks>Update the time of a favorite</remarks>
        /// <param name="update"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        /// <response code="404">Not found. A resource (user, favorite...) could not be found.</response>
        [HttpPut]
        [Route("")]
        [ProducesResponseType(typeof(Success), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 404)]
        public virtual async Task<IActionResult> UpdateFavorite([Required] [FromBody] CatalogItem update)
        {
            if (update == null)
                return new JsonErrorResult(new Error
                {
                    Code = 1,
                    Message = "No item in request"
                });

            var result = await _favoritesRepository.UpdateItemAsync(CurrentUserId, update);

            if (!result.Success)
                return new JsonErrorResult(result);

            return Json(new Success
            {
                Code = 1,
                Message = "Favorite was updated successfully"
            });
        }
    }
}