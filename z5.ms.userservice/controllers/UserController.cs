using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using z5.ms.common.abstractions;
using z5.ms.common.extensions;
using z5.ms.common.infrastructure.geoip;
using z5.ms.common.validation;
using z5.ms.common.validation.authproviders;
using z5.ms.domain.user.user;
using z5.ms.domain.user.viewmodels;
using z5.ms.infrastructure.user.identity;
using z5.ms.domain.user;

namespace z5.ms.userservice.controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("/v1/user")]
    public class UserController : Controller
    {
        private readonly IAuthTokenService _tokenService;
        private readonly IGeoIpService _geoIpService;
        private readonly IMediator _mediator;

        private Guid CurrentUserId => User.GetCurrentUserId();
        private string UserCountryByIp => _geoIpService.LookupCountry(Request.GetRemoteIp()).CountryCode;

        /// <inheritdoc />
        public UserController(IAuthTokenService tokenService, IGeoIpService geoIpService, IMediator mediator)
        {
            _tokenService = tokenService;
            _geoIpService = geoIpService;
            _mediator = mediator;
        }

        /// <summary>
        /// Get current user
        /// </summary>
        /// <remarks>Get the details of the current user</remarks>
        /// <response code="200">User</response>
        [HttpGet]
        [Authorize(typeof(JwtAuthProvider), typeof(OAuthJwtAuthProvider))]
        [ProducesResponseType(typeof(User), 200)]
        public virtual async Task<IActionResult> GetUser()
        {
            var result = await _mediator.Send(new GetUserQuery { UserId = CurrentUserId });
            return result.ToJsonResult();
        }

        /// <summary>
        /// Update a user
        /// </summary>
        /// <remarks>Update the details of the current user</remarks>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [Authorize(typeof(JwtAuthProvider), typeof(OAuthJwtAuthProvider))]
        [HttpPut]
        [ProducesResponseType(typeof(Success), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> UpdateUser([Required] [FromBody]UpdateUserCommand command)
        {
            command.UserId = CurrentUserId;
            command.IpAddress = Request.GetRemoteIp();
            command.CountryCode = UserCountryByIp;
            command.RawRequest = Request.GetBody();

            var result = await _mediator.Send(command);
            return result.ToJsonResult();
        }

        /// <summary>
        /// Delete the current user
        /// </summary>
        /// <remarks>Delete the current user or mark him as deleted</remarks>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpDelete]
        [Authorize(typeof(JwtAuthProvider), typeof(OAuthJwtAuthProvider))]
        [ProducesResponseType(typeof(Success), 200)]
        public virtual async Task<IActionResult> DeleteUser()
        {
            var result = await _mediator.Send(new DeleteUserCommand { UserId = CurrentUserId });
            return result.ToJsonResult();
        }

        /// <summary>
        /// Change password
        /// </summary>
        /// <remarks>Change the password of the currently logged in user</remarks>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPut]
        [ValidateModel]
        [Route("changepassword")]
        [Authorize(typeof(JwtAuthProvider), typeof(OAuthJwtAuthProvider))]
        [ProducesResponseType(typeof(Success), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ServiceFilter(typeof(AccessValidationByIPFilterAttribute))]
        public virtual async Task<IActionResult> ChangePassword([Required][FromBody]ChangePasswordCommand command)
        {
            command.UserId = CurrentUserId;
            command.IpAddress = Request.GetRemoteIp();
            command.CountryCode = UserCountryByIp;
            command.RawRequest = Request.GetBody();
            var result = await _mediator.Send(command);
            return result.ToJsonResult();
        }

        /// <summary>
        /// Change password - v2
        /// </summary>
        /// <remarks>Change the password of the currently logged in user</remarks>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [Obsolete]
        [HttpPut]
        [ValidateModel]
        [Route("/v2/user/changepassword")]
        [Authorize(typeof(JwtAuthProvider), typeof(OAuthJwtAuthProvider))]
        [ProducesResponseType(typeof(Success), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> ChangePasswordv2([Required][FromBody] ChangePasswordCommandv2 command)
        {
            command.UserId = CurrentUserId;
            command.IpAddress = Request.GetRemoteIp();
            command.CountryCode = UserCountryByIp;
            command.RawRequest = Request.GetBody();
            var result = await _mediator.Send(command);
            return result.ToJsonResult();
        }

        /// <summary>
        /// Confirm email address
        /// </summary>
        /// <remarks>Confirms the email address via a random code that was sent to the users email address during email based registration.</remarks>
        /// <param name="code">The random confirmation code</param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPut]
        [Route("confirmemail")]
        [ProducesResponseType(typeof(Success), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ServiceFilter(typeof(AccessValidationByIPFilterAttribute))]
        public virtual async Task<IActionResult> ConfirmEmail([Required][FromBody]string code)
        {
            var result = await _mediator.Send(new ConfirmUserCommand { Type = AuthenticationMethod.Email, Code = code });
            return result.ToJsonResult();
        }

        /// <summary>
        /// Confirm email address - v2
        /// </summary>
        /// <remarks>Confirms the email address via a random code that was sent to the users email address during email based registration.</remarks>
        /// <param name="code">The random confirmation code</param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPut]
        [Route("/v2/user/confirmemail")]
        [ProducesResponseType(typeof(Success), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> ConfirmEmailv2([Required][FromBody] ConfirmUserCommandv2 confirmUserCommandv2)
        {
            var result = await _mediator.Send(new ConfirmUserCommandv2 { Type = AuthenticationMethod.Email, Code = confirmUserCommandv2.Code, RecipientAddress = confirmUserCommandv2.RecipientAddress });
            return result.ToJsonResult();
        }

        /// <summary>
        /// Confirm mobile phone
        /// </summary>
        /// <remarks>Confirms the mobile phone number via a random code that was sent as SMS during mobile phone based registration.</remarks>
        /// <param name="code">The random confirmation code</param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPut]
        [Route("confirmmobile")]
        [ProducesResponseType(typeof(Success), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ServiceFilter(typeof(AccessValidationByIPFilterAttribute))]
        public virtual async Task<IActionResult> ConfirmMobile([Required][FromBody]string code)
        {
            var result = await _mediator.Send(new ConfirmUserCommand { Type = AuthenticationMethod.Mobile, Code = code });
            return result.ToJsonResult();
        }

        /// <summary>
        /// Confirm mobile phone - v2
        /// </summary>
        /// <remarks>Confirms the mobile phone number via a random code that was sent as SMS during mobile phone based registration.</remarks>
        /// <param name="code">The random confirmation code</param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPut]
        [Route("/v2/user/confirmmobile")]
        [ProducesResponseType(typeof(Success), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> ConfirmMobilev2([Required][FromBody] ConfirmUserCommandv2 confirmUserCommandv2)
        {
            var result = await _mediator.Send(new ConfirmUserCommandv2 { Type = AuthenticationMethod.Mobile, Code = confirmUserCommandv2.Code, RecipientAddress = confirmUserCommandv2.RecipientAddress });
            return result.ToJsonResult();
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <remarks>Log in a user with email address and password</remarks>
        /// <param name="email">The email address of the user</param>
        /// <param name="password">The password of the user.</param>
        /// <response code="200">Bearer authorization token for the user</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpGet]
        [Route("loginemail")]
        [ProducesResponseType(typeof(Token), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> LoginWithEmail([FromQuery][Required]string email, [FromQuery][Required]string password)
        {
            var command = new LoginEmailUserCommand
            {
                Email = email,
                Password = password,
                Country = UserCountryByIp,
                Refresh = false
            };

            var result = await _mediator.Send(command);
            return result.Success
                ? Json(new Token { AuthToken = result.Value.AccessToken })
                : new JsonErrorResult(result);
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <remarks>Log in a user with email address and password</remarks>
        /// <response code="200">Bearer authorization token for the user</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [Route("/v2/user/loginemail")]
        [ValidateModel]
        [ProducesResponseType(typeof(JObject), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> LoginWithEmailV2([Required][FromBody] LoginEmailUserCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ToJsonResult();
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <remarks>Log in a user with email address,password,cttl and device_id</remarks>
        /// <response code="200">Bearer authorization token for the user</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [Route("/v3/user/loginemail")]
        [ValidateModel]
        [ProducesResponseType(typeof(JObject), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> LoginWithEmailV3([Required][FromBody] LoginEmailUserCommandV3 command)
        {
            var result = await _mediator.Send(command);
            return result.ToJsonResult();
        }


        /// <summary>
        /// Login
        /// </summary>
        /// <remarks>Log in a user with mobile phone number and password</remarks>
        /// <param name="mobile">The mobile phone of the user</param>
        /// <param name="password">The password of the user.</param>
        /// <response code="200">Bearer authorization token for the user</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpGet]
        [Route("loginmobile")]
        [ProducesResponseType(typeof(Token), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> LoginWithMobile([FromQuery][Required]string mobile, [FromQuery][Required]string password)
        {
            var command = new LoginMobileUserCommand
            {
                Mobile = mobile,
                Password = password,
                Country = UserCountryByIp,
                Refresh = false
            };

            var result = await _mediator.Send(command);
            return result.Success
                ? Json(new Token { AuthToken = result.Value.AccessToken })
                : new JsonErrorResult(result);
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <remarks>Log in a user with mobile phone number and password</remarks>
        /// <response code="200">Bearer authorization token for the user</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [Route("/v2/user/loginmobile")]
        [ProducesResponseType(typeof(JObject), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> LoginWithMobileV2([Required] [FromBody]LoginMobileUserCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ToJsonResult();
        }

        
        /// <summary>
        /// Login
        /// </summary>
        /// <remarks>Log in a user with mobile phone number and password</remarks>
        /// <response code="200">Bearer authorization token for the user</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [Route("/v3/user/loginmobile")]
        [ProducesResponseType(typeof(JObject), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> LoginWithMobileV3([Required][FromBody] LoginMobileUserCommandV3 command)
        {
            var result = await _mediator.Send(command);
            return result.ToJsonResult();
        }


        /// <summary>
        /// Login
        /// </summary>
        /// <remarks>Log in a user with facebook access token</remarks>
        /// <param name="accessToken"></param>
        /// <response code="200">Bearer authorization token for the user</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpGet]
        [Route("loginfacebook")]
        [ProducesResponseType(typeof(Token), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> LoginWithFacebook([FromQuery(Name = "access_token")]string accessToken)
        {
            var command = new LoginSocialUserCommand
            {
                AccessToken = accessToken,
                Country = UserCountryByIp,
                Refresh = false,
                Type = AuthenticationMethod.Facebook
            };

            var result = await _mediator.Send(command);
            return result.Success
                ? Json(new Token { AuthToken = result.Value.AccessToken })
                : new JsonErrorResult(result);
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <remarks>Log in a user with facebook access token</remarks>
        /// <response code="200">Bearer authorization token for the user</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [Route("/v2/user/loginfacebook")]
        [FillHiddenProperties]
        [ProducesResponseType(typeof(JObject), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> LoginWithFacebookV2([Required] [FromBody]LoginSocialUserCommand command)
        {
            command.Type = AuthenticationMethod.Facebook;
            var result = await _mediator.Send(command);
            return result.ToJsonResult();
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <remarks>Log in a user with google access token</remarks>
        /// <param name="accessToken"></param>
        /// <response code="200">Bearer authorization token for the user</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpGet]
        [Route("logingoogle")]
        [ProducesResponseType(typeof(Token), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> LoginWithGoogle([Required][FromQuery(Name = "access_token")]string accessToken)
        {
            var command = new LoginSocialUserCommand
            {
                AccessToken = accessToken,
                Country = UserCountryByIp,
                Refresh = false,
                Type = AuthenticationMethod.Google
            };

            var result = await _mediator.Send(command);
            return result.Success
                ? Json(new Token { AuthToken = result.Value.AccessToken })
                : new JsonErrorResult(result);
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <remarks>Log in a user with google access token</remarks>
        /// <response code="200">Bearer authorization token for the user</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [Route("/v2/user/logingoogle")]
        [FillHiddenProperties]
        [ProducesResponseType(typeof(JObject), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> LoginWithGoogleV2([Required] [FromBody]LoginSocialUserCommand command)
        {
            command.Type = AuthenticationMethod.Google;
            var result = await _mediator.Send(command);
            return result.ToJsonResult();
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <remarks>Log in a user with google access token</remarks>
        /// <param name="idToken"></param>
        /// <response code="200">Bearer authorization token for the user</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpGet]
        [Route("/v3/user/logingoogle")]
        [ProducesResponseType(typeof(Token), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> LoginWithGoogleV3([Required][FromQuery(Name = "id_token")] string idToken)
        {
            var command = new LoginSocialUserCommand
            {
                AccessToken = idToken,
                Country = UserCountryByIp,
                Refresh = false,
                Type = AuthenticationMethod.GoogleWithTokenId
            };

            //_tokenService.GetJwtToken()
            var result = await _mediator.Send(command);
            return result.Success
                ? Json(new Token { AuthToken = result.Value.AccessToken })
                : new JsonErrorResult(result);
        }


        /// <summary>
        /// Login
        /// </summary>
        /// <remarks>Log in a user with twitter access token</remarks>
        /// <param name="accessToken"></param>
        /// <response code="200">Bearer authorization token for the user</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpGet]
        [Route("logintwitter")]
        [ProducesResponseType(typeof(Token), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> LoginWithTwitter([FromQuery(Name = "access_token")]string accessToken)
        {
            var command = new LoginSocialUserCommand
            {
                AccessToken = accessToken,
                Country = UserCountryByIp,
                Refresh = false,
                Type = AuthenticationMethod.Twitter
            };

            var result = await _mediator.Send(command);
            return result.Success
                ? Json(new Token { AuthToken = result.Value.AccessToken })
                : new JsonErrorResult(result);
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <remarks>Log in a user with twitter access token</remarks>
        /// <response code="200">Bearer authorization token for the user</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [Route("/v2/user/logintwitter")]
        [FillHiddenProperties]
        [ProducesResponseType(typeof(JObject), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> LoginWithTwitterV2([Required] [FromBody]LoginSocialUserCommand command)
        {
            command.Type = AuthenticationMethod.Twitter;
            var result = await _mediator.Send(command);
            return result.ToJsonResult();
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <remarks>Log in a user with amazon access token</remarks>
        /// <response code="200">Bearer authorization token for the user</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [Route("/v2/user/loginamazon")]
        [FillHiddenProperties]
        [ProducesResponseType(typeof(JObject), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> LoginWithAmazonV2([Required] [FromBody]LoginSocialUserCommand command)
        {
            command.Type = AuthenticationMethod.Amazon;
            var result = await _mediator.Send(command);
            return result.ToJsonResult();
        }
        
        /// <summary>
        /// Forgot password
        /// </summary>
        /// <remarks>Send an email to the user with a random code to create a new password.</remarks>
        /// <param name="email">The email address of the user</param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [Route("passwordforgottenemail")]
        [ProducesResponseType(typeof(Success), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> SendPasswordForgottenEmail([Required] [FromBody]string email)
        {
            var result = await _mediator.Send(new ForgotPasswordEmailCommand { Email = email });
            return result.ToJsonResult();
        }


        /// <summary>
        /// Forgot password
        /// </summary>
        /// <remarks>Send a SMS to the user with a random code to create a new password.</remarks>
        /// <param name="mobile">The mobile phone number of the user</param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [Route("passwordforgottenmobile")]
        [ProducesResponseType(typeof(Success), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> SendPasswordForgottenSms([Required] [FromBody]string mobile)
        {
            var result = await _mediator.Send(new ForgotPasswordMobileCommand { Mobile = mobile });
            return result.ToJsonResult();
        }

        /// <summary>
        /// Forgot password
        /// </summary>
        /// <remarks>Send a SMS to the user with a random code to create a new password.</remarks>
        /// <param name="mobile">The mobile phone number of the user</param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [Route("/v2/user/passwordforgottenmobile")]
        [ProducesResponseType(typeof(Success), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> SendPasswordForgottenSmsV2([Required] [FromBody]string mobile)
        {
            var result = await _mediator.Send(new ForgotPasswordMobileCommand { Mobile = mobile, Version = 2 });
            return result.ToJsonResult();
        }

        /// <summary>
        /// Confirm email change
        /// </summary>
        /// <remarks>Confirms the email address change via a random code that was sent to the users email address during user update.</remarks>
        /// <param name="code">The random confirmation code</param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [Obsolete]
        [HttpPut]
        [Route("confirmemailchange")]
        [ProducesResponseType(typeof(Success), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> ConfirmEmailChange([Required] [FromBody]string code)
        {
            var result = await _mediator.Send(new ConfirmUserCommand { Type = AuthenticationMethod.Email, Code = code });
            return result.ToJsonResult();
        }


        /// <summary>
        /// Recreate a forgotten password
        /// </summary>
        /// <remarks>Create a new password based on the verification code that was sent to the users email address.</remarks>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [Route("recreatepasswordemail")]
        [ProducesResponseType(typeof(Success), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ServiceFilter(typeof(AccessValidationByIPFilterAttribute))]
        public virtual async Task<IActionResult> RecreatePasswordWithEmail([Required] [FromBody]ResetPasswordCommand command)
        {
            command.IpAddress = Request.GetRemoteIp();
            command.CountryCode = UserCountryByIp;
            command.RawRequest = Request.GetBody();
            var result = await _mediator.Send(command);
            return result.ToJsonResult();
        }

        /// <summary>
        /// Recreate a forgotten password - v2
        /// </summary>
        /// <remarks>Create a new password based on the verification code that was sent to the users email address.</remarks>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [Route("/v2/user/recreatepasswordemail")]
        [ProducesResponseType(typeof(Success), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> RecreatePasswordWithEmailv2([Required][FromBody] ResetPasswordCommandv2 command)
        {
            command.Type = RecreateType.Email;
            command.IpAddress = Request.GetRemoteIp();
            command.CountryCode = UserCountryByIp;
            command.RawRequest = Request.GetBody();
            var result = await _mediator.Send(command);
            return result.ToJsonResult();
        }

        /// <summary>
        /// Recreate a forgotten password
        /// </summary>
        /// <remarks>Create a new password based on the verification code that was sent to the users mobile phone.</remarks>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [Route("recreatepasswordmobile")]
        [ProducesResponseType(typeof(Success), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ServiceFilter(typeof(AccessValidationByIPFilterAttribute))]
        public virtual async Task<IActionResult> RecreatePasswordWithSms([Required] [FromBody]ResetPasswordCommand command)
        {
            command.IpAddress = Request.GetRemoteIp();
            command.CountryCode = UserCountryByIp;
            command.RawRequest = Request.GetBody();
            var result = await _mediator.Send(command);
            return result.ToJsonResult();
        }

        /// <summary>
        /// Recreate a forgotten password - v2
        /// </summary>
        /// <remarks>Create a new password based on the verification code that was sent to the users mobile phone.</remarks>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [Route("/v2/user/recreatepasswordmobile")]
        [ProducesResponseType(typeof(Success), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> RecreatePasswordWithSmsv2([Required][FromBody] ResetPasswordCommandv2 command)
        {
            command.Type = RecreateType.Mobile;
            command.IpAddress = Request.GetRemoteIp();
            command.CountryCode = UserCountryByIp;
            command.RawRequest = Request.GetBody();
            var result = await _mediator.Send(command);
            return result.ToJsonResult();
        }

        /// <summary>
        /// Create a new user with email activation
        /// </summary>
        /// <remarks>Create a new user. A verification email will be sent to his email address with an activation code.</remarks>
        /// <param name="command"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [ValidateModel]
        [Route("registeremail")]
        [FillHiddenProperties]
        [ProducesResponseType(typeof(Success), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> RegisterWithEmail([Required] [FromBody]RegisterEmailUserCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ToJsonResult();
        }


        /// <summary>
        /// Create a new user with SMS activation
        /// </summary>
        /// <remarks>Create a new user. A verification SMS will be sent to his mobile phone number with an activation code.</remarks>
        /// <param name="command"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [ValidateModel]
        [Route("registermobile")]
        [FillHiddenProperties]
        [ProducesResponseType(typeof(Success), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> RegisterWithMobile([Required][FromBody]RegisterMobileUserCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ToJsonResult();
        }


        /// <summary>
        /// Create a new user with SMS activation
        /// </summary>
        /// <remarks>Create a new user. A verification SMS will be sent to his mobile phone number with an activation code.</remarks>
        /// <param name="command"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [ValidateModel]
        [Route("/v2/user/registermobile")]
        [FillHiddenProperties]
        [ProducesResponseType(typeof(Success), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> RegisterWithMobileV2([Required] [FromBody]RegisterMobileUserCommand command)
        {
            command.Version = 2;
            var result = await _mediator.Send(command);
            return result.ToJsonResult();
        }


        /// <summary>
        /// Create a new user via facebook access token
        /// </summary>
        /// <remarks>
        /// Acquire an authorization token from facebook and call this endpoint 
        /// see "ACCESS_TOKEN" in section "Handling Login Dialog Response" 
        /// https://developers.facebook.com/docs/facebook-login/manually-build-a-login-flow)
        /// </remarks>
        /// <param name="accessToken"></param>
        /// <response code="200">Bearer authorization token for the user</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [Route("registerfacebook")]
        [ProducesResponseType(typeof(Token), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> RegisterWithFacebook([FromQuery(Name = "access_token")]string accessToken)
        {
            var command = new RegisterSocialUserCommand
            {
                AccessToken = accessToken,
                RegistrationCountry = UserCountryByIp,
                Refresh = false,
                Type = AuthenticationMethod.Facebook
            };

            var result = await _mediator.Send(command);
            return result.Success
                ? Json(new Token { AuthToken = result.Value.AccessToken })
                : new JsonErrorResult(result);
        }


        /// <summary>
        /// Create a new user via google access token
        /// </summary>
        /// <remarks>
        /// Acquire an authorization token from google and call this endpoint 
        /// see "token response" � check the section "Web server applications"
        /// https://developers.google.com/identity/protocols/OAuth2
        /// </remarks>
        /// <param name="accessToken"></param>
        /// <response code="200">Bearer authorization token for the user</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [Route("registergoogle")]
        [ProducesResponseType(typeof(Token), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> RegisterWithGoogle([FromQuery(Name = "access_token")]string accessToken)
        {
            var command = new RegisterSocialUserCommand
            {
                AccessToken = accessToken,
                RegistrationCountry = UserCountryByIp,
                Refresh = false,
                Type = AuthenticationMethod.Google
            };

            var result = await _mediator.Send(command);
            return result.Success
                ? Json(new Token { AuthToken = result.Value.AccessToken })
                : new JsonErrorResult(result);
        }


        /// <summary>
        /// Create a new user via twitter access token
        /// </summary>
        /// <remarks>
        /// Acquire an authorization token from twitter and call this endpoint 
        /// see the "oauth_token" and "oauth_token_secret" combine them via a pipe characters "|"
        /// e.g. "7588892-kagSNqWge8gB1WwE3plnFsJHAZVfxWD7Vb57p0b4|PbKfYqSryyeKDWz4ebtY3o5ogNLG11WJuZBc9fQrQo"
        /// https://dev.twitter.com/web/sign-in/implementing
        /// </remarks>
        /// <param name="accessToken"></param>
        /// <response code="200">Bearer authorization token for the user</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [Route("registertwitter")]
        [ProducesResponseType(typeof(Token), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> RegisterWithTwitter([FromQuery(Name = "access_token")]string accessToken)
        {
            var command = new RegisterSocialUserCommand
            {
                AccessToken = accessToken,
                RegistrationCountry = UserCountryByIp,
                Refresh = false,
                Type = AuthenticationMethod.Twitter
            };

            var result = await _mediator.Send(command);
            return result.Success
                ? Json(new Token { AuthToken = result.Value.AccessToken })
                : new JsonErrorResult(result);
        }

        /// <summary>
        /// Create a new user via facebook access token
        /// </summary>
        /// <remarks>
        /// Acquire an authorization token from facebook and call this endpoint 
        /// see "ACCESS_TOKEN" in section "Handling Login Dialog Response" 
        /// https://developers.facebook.com/docs/facebook-login/manually-build-a-login-flow)
        /// </remarks>
        /// <response code="200">Bearer authorization token for the user</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [ValidateModel]
        [Route("/v2/user/registerfacebook")]
        [FillHiddenProperties]
        [ProducesResponseType(typeof(Token), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> RegisterWithFacebook([Required] [FromBody]RegisterSocialUserCommand command)
        {
            command.Type = AuthenticationMethod.Facebook;
            command.Refresh = false;
            var result = await _mediator.Send(command);
            return result.Success
                ? Json(new Token { AuthToken = result.Value.AccessToken })
                : new JsonErrorResult(result);
        }


        /// <summary>
        /// Create a new user via google access token
        /// </summary>
        /// <remarks>
        /// Acquire an authorization token from google and call this endpoint 
        /// see "token response" � check the section "Web server applications"
        /// https://developers.google.com/identity/protocols/OAuth2
        /// </remarks>
        /// <response code="200">Bearer authorization token for the user</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [ValidateModel]
        [Route("/v2/user/registergoogle")]
        [FillHiddenProperties]
        [ProducesResponseType(typeof(Token), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> RegisterWithGoogle([Required][FromBody]RegisterSocialUserCommand command)
        {
            command.Type = AuthenticationMethod.Google;
            command.Refresh = false;
            var result = await _mediator.Send(command);
            return result.Success
                ? Json(new Token { AuthToken = result.Value.AccessToken })
                : new JsonErrorResult(result);
        }


        /// <summary>
        /// Create a new user via twitter access token
        /// </summary>
        /// <remarks>
        /// Acquire an authorization token from twitter and call this endpoint 
        /// see the "oauth_token" and "oauth_token_secret" combine them via a pipe characters "|"
        /// e.g. �7588892-kagSNqWge8gB1WwE3plnFsJHAZVfxWD7Vb57p0b4|PbKfYqSryyeKDWz4ebtY3o5ogNLG11WJuZBc9fQrQo�
        /// https://dev.twitter.com/web/sign-in/implementing
        /// </remarks>
        /// <response code="200">Bearer authorization token for the user</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [ValidateModel]
        [Route("/v2/user/registertwitter")]
        [FillHiddenProperties]
        [ProducesResponseType(typeof(Token), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> RegisterWithTwitter([Required][FromBody]RegisterSocialUserCommand command)
        {
            command.Type = AuthenticationMethod.Twitter;
            command.Refresh = false;
            var result = await _mediator.Send(command);
            return result.Success
                ? Json(new Token { AuthToken = result.Value.AccessToken })
                : new JsonErrorResult(result);
        }

        /// <summary>
        /// Create a new user via facebook access token
        /// </summary>
        /// <remarks>
        /// Acquire an authorization token from facebook and call this endpoint 
        /// see "ACCESS_TOKEN" in section "Handling Login Dialog Response" 
        /// https://developers.facebook.com/docs/facebook-login/manually-build-a-login-flow)
        /// </remarks>
        /// <response code="200">Bearer authorization token for the user</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [ValidateModel]
        [Route("/v3/user/registerfacebook")]
        [FillHiddenProperties]
        [ProducesResponseType(typeof(JObject), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> RegisterWithFacebookV3([Required][FromBody]RegisterSocialUserCommand command)
        {
            command.Type = AuthenticationMethod.Facebook;
            var result = await _mediator.Send(command);
            return result.ToJsonResult();
        }


        /// <summary>
        /// Create a new user via google access token
        /// </summary>
        /// <remarks>
        /// Acquire an authorization token from google and call this endpoint 
        /// see "token response" � check the section "Web server applications"
        /// https://developers.google.com/identity/protocols/OAuth2
        /// </remarks>
        /// <response code="200">Bearer authorization token for the user</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [ValidateModel]
        [Route("/v3/user/registergoogle")]
        [FillHiddenProperties]
        [ProducesResponseType(typeof(Token), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> RegisterWithGoogleV3([Required][FromBody]RegisterSocialUserCommand command)
        {
            command.Type = AuthenticationMethod.Google;
            var result = await _mediator.Send(command);
            return result.ToJsonResult();
        }

        /// <summary>
        /// Create a new user via google idtoken
        /// </summary>
        /// <remarks>
        /// Acquire an id token from google and call this endpoint 
        /// see below url for more details
        /// https://developers.google.com/identity/sign-in/web/backend-auth
        /// </remarks>
        /// <response code="200">token details for the user</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [ValidateModel]
        [Route("/v4/user/registergoogle")]
        [FillHiddenProperties]
        [ProducesResponseType(typeof(Token), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> RegisterWithGoogleV4([Required][FromBody] RegisterSocialUserCommand command)
        {
            command.Type = AuthenticationMethod.GoogleWithTokenId;
            var result = await _mediator.Send(command);
            return result.ToJsonResult();
        }

        /// <summary>
        /// Create a new user via twitter access token
        /// </summary>
        /// <remarks>
        /// Acquire an authorization token from twitter and call this endpoint 
        /// see the "oauth_token" and "oauth_token_secret" combine them via a pipe characters "|"
        /// e.g. �7588892-kagSNqWge8gB1WwE3plnFsJHAZVfxWD7Vb57p0b4|PbKfYqSryyeKDWz4ebtY3o5ogNLG11WJuZBc9fQrQo�
        /// https://dev.twitter.com/web/sign-in/implementing
        /// </remarks>
        /// <response code="200">Bearer authorization token for the user</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [ValidateModel]
        [Route("/v3/user/registertwitter")]
        [FillHiddenProperties]
        [ProducesResponseType(typeof(Token), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> RegisterWithTwitterV3([Required][FromBody]RegisterSocialUserCommand command)
        {
            command.Type = AuthenticationMethod.Twitter;
            var result = await _mediator.Send(command);
            return result.ToJsonResult();
        }

        /// <summary>
        /// Create a new user via amazon access token
        /// </summary>
        /// <remarks>
        /// Acquire an authorization token from amazon and call this endpoint 
        /// https://developer.amazon.com/docs/login-with-amazon/web-docs.html
        /// </remarks>
        /// <response code="200">Bearer authorization token for the user</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [ValidateModel]
        [Route("/v3/user/registeramazon")]
        [FillHiddenProperties]
        [ProducesResponseType(typeof(Token), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> RegisterWithAmazonV3([Required][FromBody]RegisterSocialUserCommand register)
        {
            register.Type = AuthenticationMethod.Amazon;
            var result = await _mediator.Send(register);
            return result.ToJsonResult();
        }


        /// <summary>
        /// Send confirmation to email address again
        /// </summary>
        /// <remarks>Resends the confirmation code again in case it got lost.</remarks>
        /// <param name="email">The email address that was used before</param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [Route("resendconfirmationemail")]
        [ProducesResponseType(typeof(Success), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> ResendConfirmationEmail([Required][FromBody]string email)
        {
            var result = await _mediator.Send(new ResendConfirmationEmailCommand { Email = email });
            return result.ToJsonResult();
        }


        /// <summary>
        /// Send confirmation to mobile phone again
        /// </summary>
        /// <remarks>Resends the confirmation code again in case it got lost.</remarks>
        /// <param name="mobile">The mobile phone number that was used before</param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [Route("resendconfirmationmobile")]
        [ProducesResponseType(typeof(Success), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> ResendConfirmationSms([Required][FromBody]string mobile)
        {
            var result = await _mediator.Send(new ResendConfirmationSmsCommand { Mobile = mobile });
            return result.ToJsonResult();
        }

        /// <summary>
        /// Send confirmation to mobile phone again
        /// </summary>
        /// <remarks>Resends the confirmation code again in case it got lost.</remarks>
        /// <param name="mobile">The mobile phone number that was used before</param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [Route("/v2/user/resendconfirmationmobile")]
        [ProducesResponseType(typeof(Success), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> ResendConfirmationSmsV2([Required][FromBody]string mobile)
        {
            var result = await _mediator.Send(new ResendConfirmationSmsCommand { Mobile = mobile, Version = 2 });
            return result.ToJsonResult();
        }

        /// <summary>
        /// Get new access token
        /// </summary>
        /// <remarks>Provides new access token using refresh token</remarks>
        /// <param name="refreshToken">Refresh token that provided during login or social media registration</param>
        /// <response code="200">TokenResponse</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [Route("/v2/user/renew")]
        [ProducesResponseType(typeof(JObject), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> RenewToken([FromQuery(Name = "refresh_token")] string refreshToken)
        {
            var tokenResponse = await _tokenService.GetJwtToken(refreshToken);
            return tokenResponse.ToJsonResult();
        }

        /// <summary>
        /// Get new access token
        /// </summary>
        /// <remarks>Provides new access token using refresh token</remarks>
        /// <param name="refreshToken">Refresh token that provided during login or social media registration</param>
        /// <param name="cttl"> Custom time to live for the refresh token</param>
        /// <response code="200">TokenResponse</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [Route("/v3/user/renew")]
        [ProducesResponseType(typeof(JObject), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> RenewTokenV3([FromQuery(Name = "refresh_token")] string refreshToken, [FromQuery(Name = "cttl")] int cttl)
        {
            var tokenResponse = await _tokenService.GetJwtToken(refreshToken,cttl);
            return tokenResponse.ToJsonResult();
        }

        /// <summary>
        /// Get new access token
        /// </summary>
        /// <remarks>Provides new access token using a valid access token</remarks>
        /// <response code="200">TokenResponse</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpGet]
        [Route("/v1/user/tokenexchange")]
        [Authorize(typeof(JwtAuthProvider), typeof(OAuthJwtAuthProvider))]
        public virtual async Task<IActionResult> ExchangeToken()
        {
            var tokenResponse = await _tokenService.GetJwtToken(CurrentUserId, UserCountryByIp);
            return tokenResponse.Success
                ? Json(new Token { AuthToken = tokenResponse.Value.AccessToken })
                : new JsonErrorResult(tokenResponse);
        }

        /// <summary>
        /// Get new access token
        /// </summary>
        /// <remarks>Provides new access token along with a refresh token using a valid access token</remarks>
        /// <response code="200">TokenResponse</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPost]
        [Route("/v2/user/tokenexchange")]
        [Authorize(typeof(JwtAuthProvider), typeof(OAuthJwtAuthProvider))]
        public virtual async Task<IActionResult> ExchangeTokenV2()
        {
            var tokenResponse = await _tokenService.GetJwtToken(CurrentUserId, UserCountryByIp, true);
            return tokenResponse.ToJsonResult();
        }
    }
}
