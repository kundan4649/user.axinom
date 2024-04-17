using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using z5.ms.common.abstractions;
using z5.ms.common.attributes;
using z5.ms.common.extensions;
using z5.ms.common.validation;
using z5.ms.common.validation.authproviders;
using z5.ms.domain.user.customer;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.user;
using z5.ms.domain.user.viewmodels;
using z5.ms.infrastructure.user.identity;
using z5.ms.infrastructure.user.repositories;
using z5.ms.common.infrastructure.geoip;

namespace z5.ms.userservice.controllers
{
    /// <summary>
    /// User endpoints for internal usage
    /// </summary>
    [Route("/v1/manage/customer")]
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IAuthTokenService _tokenService;
        private readonly UserServiceOptions _options;
        private readonly IMediator _mediator;
        private readonly IGeoIpService _geoIpService;
        private string UserCountryByIp => _geoIpService.LookupCountry(Request.GetRemoteIp()).CountryCode;

        /// <inheritdoc />
        public CustomerController(ICustomerRepository customerRepository, IMapper mapper, IOptions<UserServiceOptions> options,
            IUserRepository userRepository, IAuthTokenService tokenService, IGeoIpService geoIpService, IMediator mediator)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _options = options.Value;
            _mediator = mediator;
            _geoIpService = geoIpService;
        }


        /// <summary>
        /// Create a new customer using email address
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Customer</response>
        /// <response code="400">
        /// Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;.
        /// That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error.
        /// Then some specific error code will be returned and the API endpoint will have a reference to this definition.
        /// </response>
        [HttpPost("email")]
        [ValidateModel]
        [FillHiddenProperties]
        [Authorize(typeof(CrmSignedAuthProvider))]
        [ProducesResponseType(typeof(Customer), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> AddCustomer([Required] [FromBody] RegisterEmailUserCommand customerCreateEmail)
        {
            var customer = _mapper.Map<RegisterEmailUserCommand, CustomerCreate>(customerCreateEmail,
                    opts => opts.AfterMap((src, dest) => dest.System = _options.DefaultSystemType));

            var result = await _customerRepository.CreateCustomer(customer);
            return result.ToJsonResult();
        }

        /// <summary>
        /// Create a new customer using mobile phone number
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Customer</response>
        /// <response code="400">
        /// Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;.
        /// That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error.
        /// Then some specific error code will be returned and the API endpoint will have a reference to this definition.
        /// </response>
        [HttpPost("mobile")]
        [ValidateModel]
        [FillHiddenProperties]
        [Authorize(typeof(CrmSignedAuthProvider))]
        [ProducesResponseType(typeof(Customer), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> AddCustomer([Required] [FromBody] RegisterMobileUserCommand customerCreateMobile)
        {
            var customer = _mapper.Map<RegisterMobileUserCommand, CustomerCreate>(customerCreateMobile,
                    opts => opts.AfterMap((src, dest) => dest.System = _options.DefaultSystemType));

            var result = await _customerRepository.CreateCustomer(customer);
            return result.ToJsonResult();
        }

        /// <summary>
        /// Change the password of a Customer
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Customer</response>
        /// <response code="400">
        /// Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;.
        /// That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error.
        /// Then some specific error code will be returned and the API endpoint will have a reference to this definition.
        /// </response>
        [HttpPut("{customer_id}/password")]
        [Authorize(typeof(CrmSignedAuthProvider))]
        [ValidateModel]
        [ProducesResponseType(typeof(Customer), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> SetCustomerPassword([Required][NotEmpty][FromRoute(Name = "customer_id")]Guid customerId, [Required] [FromBody] string newPassword)
        {
            var IpAddress = Request.GetRemoteIp();
            var CountryCode = UserCountryByIp;
            var result = await _customerRepository.SetCustomerPassword(customerId, newPassword, IpAddress, CountryCode);
            return result.ToJsonResult();
        }

        /// <summary>
        /// Delete the customer
        /// </summary>
        /// <remarks>Mark the selected customer as deleted</remarks>
        /// <param name="customerId">The ID of the customer</param>
        /// <response code="200">Success</response>
        /// <response code="400">
        /// Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;.
        /// That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error.
        /// Then some specific error code will be returned and the API endpoint will have a reference to this definition.
        /// </response>
        [HttpDelete("{customer_id}")]
        [ValidateModel]
        [Authorize(typeof(CmsInternalAuthProvider))]
        [ProducesResponseType(typeof(Success), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> DeleteCustomer([Required][NotEmpty][FromRoute(Name = "customer_id")]Guid customerId)
        {
            var result = await _customerRepository.DeleteCustomer(customerId);
            return result.ToJsonResult();
        }


        /// <summary>
        /// Get a specific customer
        /// </summary>
        /// <remarks>Get the details of a specific customer</remarks>
        /// <param name="customerId">The ID of the customer</param>
        /// <response code="200">Customer</response>
        /// <response code="400">
        /// Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;.
        /// That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error.
        /// Then some specific error code will be returned and the API endpoint will have a reference to this definition.
        /// </response>
        /// <response code="404">Not found. A resource (customer, product, collection...) could not be found.</response>
        [HttpGet("{customer_id}")]
        [Authorize(typeof(CmsInternalAuthProvider), typeof(CrmSignedAuthProvider))]
        [ValidateModel]
        [ProducesResponseType(typeof(Customer), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 404)]
        public virtual async Task<IActionResult> GetCustomer([Required][NotEmpty][FromRoute(Name = "customer_id")] Guid customerId)
        {
            var result = await _customerRepository.GetCustomer(customerId,true);
            return result.ToJsonResult();
        }


        /// <summary>
        /// Update customer
        /// </summary>
        /// <remarks>Update the details of a customer</remarks>
        /// <param name="customerChangeDetails"></param>
        /// <response code="200">Customer</response>
        /// <response code="400">Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;. That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error. Then some specific error code will be returned and the API endpoint will have a reference to this definition.</response>
        [HttpPut]
        [ValidateModel]
        [Authorize(typeof(CmsInternalAuthProvider))]
        [ProducesResponseType(typeof(Customer), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> UpdateCustomer([Required] [FromBody] CustomerChangeDetails customerChangeDetails)
        {
            customerChangeDetails.IpAddress = Request.GetRemoteIp();
            customerChangeDetails.CountryCode = UserCountryByIp;
            customerChangeDetails.RawRequest = Request.GetBody();
            var result = await _customerRepository.UpdateCustomer(customerChangeDetails);
            return result.ToJsonResult();
        }


        /// <summary>
        /// Get customers
        /// </summary>
        /// <remarks>Get a list of all customers in the system. Filters, paging and sorting can be applied.</remarks>
        /// <param name="query">Query containing all relevant information</param>
        /// <response code="200">Customers</response>
        /// <response code="400">
        /// Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;.
        /// That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error.
        /// Then some specific error code will be returned and the API endpoint will have a reference to this definition.
        /// </response>
        [HttpGet]
        [Authorize(typeof(CmsInternalAuthProvider), typeof(CrmSignedAuthProvider))]
        [ProducesResponseType(typeof(Customers), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> GetCustomers(GetCustomersQuery query)
        {
            return (await _mediator.Send(query)).ToJsonResult();
        }

        /// <summary>
        /// Get a JWT for a specific customer
        /// </summary>
        /// <response code="200">Authentication token</response>
        /// <response code="400">
        /// Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;.
        /// That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error.
        /// Then some specific error code will be returned and the API endpoint will have a reference to this definition.
        /// </response>
        [HttpGet("token")]
        [Authorize(typeof(CrmSignedAuthProvider))]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> ImpersonateCustomer([FromQuery(Name = "email")] string email, [FromQuery(Name = "mobile")] string mobile)
        {
            if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(mobile))
            {
                return new JsonErrorResult(new Error { Code = 1, Message = "One of email or mobile is required" });
            }

            var user = string.IsNullOrWhiteSpace(email)
                ? await _userRepository.GetUser(AuthenticationMethod.Mobile, mobile)
                : await _userRepository.GetUser(AuthenticationMethod.Email, email);
            if (user == null)
                return new JsonErrorResult(1, "User not found", HttpStatusCode.NotFound);

            var tokenResult = await _tokenService.GetJwtToken(user.Id, user.RegistrationCountry);
            return tokenResult.Success
                ? Json(new Token { AuthToken = tokenResult.Value.AccessToken })
                : new JsonErrorResult(tokenResult);
        }


        /// <summary>
        /// Get a token details for a specific customer
        /// </summary>
        /// <response code="200">Authentication token</response>
        /// <response code="400">
        /// Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;.
        /// That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error.
        /// Then some specific error code will be returned and the API endpoint will have a reference to this definition.
        /// </response>
        [HttpGet("tokenwithrefresh")]
        [Authorize(typeof(CrmSignedAuthProvider))]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> ImpersonateCustomerWithToken([FromQuery(Name = "email")] string email, [FromQuery(Name = "mobile")] string mobile)
        {
            if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(mobile))
            {
                return new JsonErrorResult(new Error { Code = 1, Message = "One of email or mobile is required" });
            }

            var user = string.IsNullOrWhiteSpace(email)
               ? await _userRepository.GetUserFromRelica(AuthenticationMethod.Mobile, mobile)
               : await _userRepository.GetUserFromRelica(AuthenticationMethod.Email, email);

            if (user == null)
                return new JsonErrorResult(1, "User not found", HttpStatusCode.NotFound);

            var tokenResult = await _tokenService.GetJwtToken(user.Id, user.RegistrationCountry,true);
            return tokenResult.Success
                ? tokenResult.ToJsonResult()
                : new JsonErrorResult(tokenResult);
        }


        /// <summary>
        /// Get a token details for a specific customer
        /// </summary>
        /// <response code="200">Authentication token</response>
        /// <response code="400">
        /// Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;.
        /// That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error.
        /// Then some specific error code will be returned and the API endpoint will have a reference to this definition.
        /// </response>
        [HttpGet("/v2/manage/customer/tokenwithrefresh")]
        [Authorize(typeof(CmsInternalAuthProvider), typeof(CrmSignedAuthProvider))]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public virtual async Task<IActionResult> ImpersonateCustomerWithTokenV2([FromQuery(Name = "email")] string email, [FromQuery(Name = "mobile")] string mobile, [FromQuery(Name = "device_id")] string device_id, [FromQuery(Name = "cttl")] int cttl)
        {
            if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(mobile))
            {
                return new JsonErrorResult(new Error { Code = 1, Message = "One of email or mobile  is required" });
            }

            var user = string.IsNullOrWhiteSpace(email)
              ? await _userRepository.GetUserFromRelica(AuthenticationMethod.Mobile, mobile)
              : await _userRepository.GetUserFromRelica(AuthenticationMethod.Email, email);


            if (user == null)
                return new JsonErrorResult(1, "User not found", HttpStatusCode.NotFound);
            _userRepository.UpdateAdditionalInfo(user, cttl, device_id, false);
            var tokenResult = await _tokenService.GetJwtToken(user.Id, user.RegistrationCountry, true, cttl);
            return tokenResult.Success
                ? tokenResult.ToJsonResult()
                : new JsonErrorResult(tokenResult);
        }


        /// <summary>
        /// Get a token details for a specific customer using userid
        /// </summary>
        /// <response code="200">Authentication token</response>
        /// <response code="400">
        /// Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;.
        /// That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error.
        /// Then some specific error code will be returned and the API endpoint will have a reference to this definition.
        /// </response>
        /// <response code="404">User not found</response>
        [HttpGet("tokenwithrefresh/{user_id}")]
        [Authorize(typeof(CrmSignedAuthProvider))]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 404)]
        public virtual async Task<IActionResult> ImpersonateCustomerWithToken([Required][NotEmpty][FromRoute(Name = "user_id")] Guid userId)
        {
             var  user = await _userRepository.SingleOrDefaultWhereFromReplica("Id", userId);

            if (user == null)
                return new JsonErrorResult(1, "User not found", HttpStatusCode.NotFound);

            var tokenResult = await _tokenService.GetJwtToken(user.Id, user.RegistrationCountry, true);
            return tokenResult.Success
                ? tokenResult.ToJsonResult()
                : new JsonErrorResult(tokenResult);
        }

        /// <summary>
        /// Get a token details for a specific customer using userid
        /// </summary>
        /// <response code="200">Authentication token</response>
        /// <response code="400">
        /// Bad Request. The request could not be validated. It can be invalid according to the API definition. This will return the error code \&quot;0\&quot;.
        /// That response can happen for almost every API endpoint and is thus not included everywhere. Or it can happen as part of a business logic validation error.
        /// Then some specific error code will be returned and the API endpoint will have a reference to this definition.
        /// </response>
        /// <response code="404">User not found</response>
        [HttpGet("/v2/manage/customer/tokenwithrefresh/{user_id}")]
        [Authorize(typeof(CmsInternalAuthProvider), typeof(CrmSignedAuthProvider))]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 404)]
        public virtual async Task<IActionResult> ImpersonateCustomerWithTokenV2([Required][NotEmpty][FromRoute(Name = "user_id")] Guid userId, [FromQuery(Name = "device_id")] string device_id, [FromQuery(Name = "cttl")] int cttl)
        {
            var user = await _userRepository.SingleOrDefaultWhereFromReplica("Id", userId);

            if (user == null)
                return new JsonErrorResult(1, "User not found", HttpStatusCode.NotFound);
            _userRepository.UpdateAdditionalInfo(user, cttl, device_id, false);
            var tokenResult = await _tokenService.GetJwtToken(user.Id, user.RegistrationCountry, true, cttl);
            return tokenResult.Success
                ? tokenResult.ToJsonResult()
                : new JsonErrorResult(tokenResult);
        }
    }
}