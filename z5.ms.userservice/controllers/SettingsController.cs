using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using z5.ms.common.abstractions;
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
    [Route("/v1/settings")]
    public class SettingsController : Controller
    {
        private readonly ISettingsRepository _settingsRepository;
        private Guid CurrentUserId => User.GetCurrentUserId();

        /// <inheritdoc />
        public SettingsController(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        /// <summary>
        /// Delete setting
        /// </summary>
        /// <remarks>Delete a setting for the current user. Settings are client side values that should be remembered accross devices. The contents of the settings are purely client defined.</remarks>
        /// <param name="key"></param>
        /// <response code="200">Success</response>
        [HttpDelete]
        [Route("")]
        [ProducesResponseType(typeof(Success), 200)]
        public virtual async Task<IActionResult> DeleteSetting([Required][FromQuery] string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return new JsonErrorResult(new Error
                {
                    Code = 3,
                    Message = "Key field is required"
                });
            }
            
            var result = await _settingsRepository.DeleteItemAsync(CurrentUserId, new SettingItem {SettingKey = key});

            if (!result.Success)
                return new JsonErrorResult(result);

            return Json(new Success
            {
                Code = 1,
                Message = "Delete successful"
            });
        }

        /// <summary>
        /// Delete setting
        /// </summary>
        /// <remarks>Delete a setting for the current user. Settings are client side values that should be remembered accross devices. The contents of the settings are purely client defined.</remarks>
        /// <param name="key"></param>
        /// <param name="current_profile_id"></param>
        /// <response code="200">Success</response>
        [HttpDelete]
        [Route("/v2/settings")]
        [ProducesResponseType(typeof(Success), 200)]
        public virtual async Task<IActionResult> DeleteSettingV2([Required][FromQuery] string key, [Required][FromQuery] Guid current_profile_id)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return new JsonErrorResult(new Error
                {
                    Code = 3,
                    Message = "Key field is required"
                });
            }

            var result = await _settingsRepository.DeleteItemAsync(current_profile_id, new SettingItem { SettingKey = key });

            if (!result.Success)
                return new JsonErrorResult(result);

            return Json(new Success
            {
                Code = 1,
                Message = "Delete successful"
            });
        }


        /// <summary>
        /// Settings
        /// </summary>
        /// <remarks>Get all the settings of the user. Settings are client side values that should be remembered accross devices. The contents of the settings are purely client defined.</remarks>
        /// <response code="200">Settings</response>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(List<SettingItem>), 200)]
        public virtual async Task<IActionResult> GetSettings()
        {
            var result = await _settingsRepository.GetItemsAsync(CurrentUserId);

            if (!result.Success)
                return new JsonErrorResult(result);

            return Json(result.Value);
        }

        /// <summary>
        /// Settings
        /// </summary>
        /// <remarks>Get all the settings of the user. Settings are client side values that should be remembered accross devices. The contents of the settings are purely client defined.</remarks>
        /// <param name="current_profile_id"></param>
        /// <response code="200">Settings</response>
        [HttpGet]
        [Route("/v2/settings")]
        [ProducesResponseType(typeof(List<SettingItem>), 200)]
        public virtual async Task<IActionResult> GetSettingsV2([Required][FromQuery] Guid current_profile_id)
        {
            var result = await _settingsRepository.GetItemsAsync(current_profile_id);

            if (!result.Success)
                return new JsonErrorResult(result);

            return Json(result.Value);
        }


        


        /// <summary>
        /// Add a setting
        /// </summary>
        /// <remarks>Add a setting for the current user. Settings are client side values that should be remembered accross devices. The contents of the settings are purely client defined.</remarks>
        /// <param name="add"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(Success), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> AddSetting([Required] [FromBody] SettingItem add)
        {
            if (add == null)
                return new JsonErrorResult(new Error
                {
                    Code = 3,
                    Message = "No item in request"
                });

            var result = await _settingsRepository.AddItemAsync(CurrentUserId, add);

            if (!result.Success)
                return new JsonErrorResult(result);

            return Json(new Success
            {
                Code = 1,
                Message = "Setting was added successfully"
            });
        }

        /// <summary>
        /// Add a setting
        /// </summary>
        /// <remarks>Add a setting for the current user. Settings are client side values that should be remembered accross devices. The contents of the settings are purely client defined.</remarks>
        /// <param name="add"></param>
        /// <param name="current_profile_id"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [Route("/v2/settings")]
        [ProducesResponseType(typeof(Success), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> AddSettingV2([Required][FromBody] SettingItem add,[Required][FromQuery] Guid current_profile_id)
        {
            if (add == null)
                return new JsonErrorResult(new Error
                {
                    Code = 3,
                    Message = "No item in request"
                });

            var result = await _settingsRepository.AddItemAsync(current_profile_id, add);

            if (!result.Success)
                return new JsonErrorResult(result);

            return Json(new Success
            {
                Code = 1,
                Message = "Setting was added successfully"
            });
        }


       


        /// <summary>
        /// Update a setting
        /// </summary>
        /// <remarks>Update a setting for the current user. Settings are client side values that should be remembered accross devices. The contents of the settings are purely client defined.</remarks>
        /// <param name="update"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        /// <response code="404">Not found. A resource (user, favorite...) could not be found.</response>
        [HttpPut]
        [Route("")]
        [ProducesResponseType(typeof(Success), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 404)]
        public virtual async Task<IActionResult> UpdateSetting([Required] [FromBody] SettingItem update)
        {
            if (update == null)
                return new JsonErrorResult(new Error
                {
                    Code = 3,
                    Message = "No item in request"
                });

            var result = await _settingsRepository.UpdateItemAsync(CurrentUserId, update);

            if (!result.Success)
                return new JsonErrorResult(result);

            return Json(new Success
            {
                Code = 1,
                Message = "Setting was updated successfully"
            });
        }

        /// <summary>
        /// Update a setting
        /// </summary>
        /// <remarks>Update a setting for the current user. Settings are client side values that should be remembered accross devices. The contents of the settings are purely client defined.</remarks>
        /// <param name="update"></param>
        /// <param name="current_profile_id"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        /// <response code="404">Not found. A resource (user, favorite...) could not be found.</response>
        [HttpPut]
        [Route("/v2/settings")]
        [ProducesResponseType(typeof(Success), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 404)]
        public virtual async Task<IActionResult> UpdateSettingV2([Required][FromBody] SettingItem update, [Required][FromQuery] Guid current_profile_id)
        {
            if (update == null)
                return new JsonErrorResult(new Error
                {
                    Code = 3,
                    Message = "No item in request"
                });

            var result = await _settingsRepository.UpdateItemAsync(current_profile_id, update);

            if (!result.Success)
                return new JsonErrorResult(result);

            return Json(new Success
            {
                Code = 1,
                Message = "Setting was updated successfully"
            });
        }
    }
}