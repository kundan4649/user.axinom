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
    [Route("/v1/reminders")]
    public class RemindersController : Controller
    {
        private readonly IRemindersRepository _remindersRepository;
        private Guid CurrentUserId => User.GetCurrentUserId();
        private string CurrentCountry => User.GetClaim("current_country") ?? "IN";

        /// <inheritdoc />
        public RemindersController(IRemindersRepository remindersRepository)
        {
            _remindersRepository = remindersRepository;
        }

        /// <summary>
        /// Delete reminder
        /// </summary>
        /// <remarks>Delete a reminder from the current user</remarks>
        /// <param name="id"></param>
        /// <param name="assetType"></param>
        /// <response code="200">Success</response>
        [HttpDelete]
        [Route("")]
        [ProducesResponseType(typeof(Success), 200)]
        public virtual async Task<IActionResult> DeleteReminder([Required] [FromQuery] string id,[Required] [FromQuery(Name = "asset_type")] AssetType assetType = AssetType.Unknown)
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

            var result = await _remindersRepository.DeleteItemAsync(CurrentUserId, new ReminderItem {AssetId = id, AssetType = assetType});

            if (!result.Success)
                return new JsonErrorResult(result);

            return Json(new Success
            {
                Code = 1,
                Message = "Delete successful"
            });
        }


        /// <summary>
        /// Get the reminders of the user
        /// </summary>
        /// <remarks>Get the reminders of the user</remarks>
        /// <response code="200">Reminders</response>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(List<ReminderItem>), 200)]
        public virtual async Task<IActionResult> GetReminders()
        {
            var result = await _remindersRepository.GetItemsAsync(CurrentUserId);

            if (!result.Success)
                return new JsonErrorResult(result);

            return Json(result.Value);
        }


        /// <summary>
        /// Add a reminder
        /// </summary>
        /// <remarks>Add a reminder for the current user</remarks>
        /// <param name="add"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(Success), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> AddReminder([Required] [FromBody] ReminderItem add)
        {
            if (add == null)
                return new JsonErrorResult(new Error
                {
                    Code = 3,
                    Message = "No item in request"
                });

            if (add.AssetType != AssetType.TvShow && add.AssetType != AssetType.Episode)
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

            var result = await _remindersRepository.AddItemAsync(CurrentUserId, add, CurrentCountry);

            if (!result.Success)
                return new JsonErrorResult(result);

            return Json(new Success
            {
                Code = 1,
                Message = "Reminder was added successfully"
            });
        }


        /// <summary>
        /// Update a reminder
        /// </summary>
        /// <remarks>Update the time of a reminder</remarks>
        /// <param name="update"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        /// <response code="404">Not found. A resource (user, favorite...) could not be found.</response>
        [HttpPut]
        [Route("")]
        [ProducesResponseType(typeof(Success), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 404)]
        public virtual async Task<IActionResult> UpdateReminder([Required] [FromBody] ReminderItem update)
        {
            if (update == null)
                return new JsonErrorResult(new Error
                {
                    Code = 3,
                    Message = "No item in request"
                });

            var result = await _remindersRepository.UpdateItemAsync(CurrentUserId, update, CurrentCountry);

            if (!result.Success)
                return new JsonErrorResult(result);

            return Json(new Success
            {
                Code = 1,
                Message = "Reminder was updated successfully"
            });
        }
    }
}