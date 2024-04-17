using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

using Dapper;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Events;
using IdentityServer4.ResponseHandling;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using z5.ms.common.abstractions;
using z5.ms.common.extensions;
using z5.ms.common.infrastructure.db;
using z5.ms.common.validation;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.viewmodels;
using Microsoft.Extensions.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace z5.ms.infrastructure.user.identity
{
    /// <summary>Auth token service</summary>
    /// <remarks>
    /// This service created to get jwt tokens from identity server directly without any http requests
    /// </remarks>
    public interface IAuthTokenService
    {
        /// <summary>
        /// Generate Jwt token for user
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="country">Country of the user</param>
        /// <param name="refresh">Bool value to indicate refresh token is needed</param>
        /// <param name="cttl"> integer value to set custom time to live</param>
        /// <returns></returns>
        Task<Result<OAuthToken>> GetJwtToken(Guid userId, string country, bool refresh = false, int cttl = 0);

        /// <summary>
        /// Generate Jwt token for user using refresh token
        /// </summary>
        /// <param name="refreshToken">Refresh token</param>
        /// <returns></returns>
        Task<Result<OAuthToken>> GetJwtToken(string refreshToken);

        /// <summary>
        /// Generate Jwt token for user using refresh token v3 
        /// </summary>
        /// <param name="refreshToken">Refresh token</param>
        /// <param name="cttl">cttl</param>
        /// <returns></returns>
        Task<Result<OAuthToken>> GetJwtToken(string refreshToken,int cttl);

        Task<JwtTokenValidationResult> ValidateGoogleJwtToken(string token);
    }

    /// <inheritdoc />
    public class AuthTokenService : IAuthTokenService
    {
        private readonly ITokenRequestValidator _tokenRequestValidator;
        private readonly ITokenResponseGenerator _tokenResponseGenerator;
        private readonly IEventService _eventService;
        private readonly JwtTokenValidator _jwtTokenValidator;
        private readonly IConfiguration _config;


        /// <inheritdoc />
        public AuthTokenService(ITokenRequestValidator tokenRequestValidator, ITokenResponseGenerator tokenResponseGenerator, IEventService eventService, JwtTokenValidator jwtTokenValidator, IConfiguration config)
        {
            _tokenRequestValidator = tokenRequestValidator;
            _tokenResponseGenerator = tokenResponseGenerator;
            _eventService = eventService;
            _jwtTokenValidator = jwtTokenValidator;
            _config = config;
        }

        /// <inheritdoc />
        public async Task<Result<OAuthToken>> GetJwtToken(Guid userId, string country, bool refresh = false, int cttl = 0)
        {
            var coll = new NameValueCollection
            {
                {"grant_type", "delegation"},
                {"scope", $"userapi subscriptionapi{(refresh ? " offline_access" : "")}"},
                {"sub", $"{userId}"},
                {"current_country", country}
            };

            var clientResult = new ClientSecretValidationResult
            {
                Client = refresh ? IdentityConfig.RefreshTokenClient : IdentityConfig.TokenClient
            };

            UpdateAccessTokenLifetime(cttl, clientResult);
            return await Validate(coll, clientResult);
        }

        private void UpdateAccessTokenLifetime(int cttl, ClientSecretValidationResult clientResult)
        {
            if (cttl > 0)
                clientResult.Client.AccessTokenLifetime = cttl;
            else
            {
                if (clientResult.Client.ClientId == "ref_tkn_cl")
                    clientResult.Client.AccessTokenLifetime = int.TryParse(_config["TokenServiceOptions:AccessTokenLifeTime"], out var alt) ? alt : 1800;
                else
                    clientResult.Client.AccessTokenLifetime = int.TryParse(_config["TokenServiceOptions:ClientTokenLifeTime"], out var lt) ? lt : 7200;
            }
        }

        /// <inheritdoc />
        public async Task<Result<OAuthToken>> GetJwtToken(string refreshToken)
        {
            var coll = new NameValueCollection
            {
                {"grant_type", "refresh_token"},
                {"refresh_token", refreshToken}
            };

            var clientResult = new ClientSecretValidationResult
            {
                Client = IdentityConfig.RefreshTokenClient
            };
            return await Validate(coll, clientResult);
        }

        //for v3 renew 
        public async Task<Result<OAuthToken>> GetJwtToken(string refreshToken,int cttl)
        {
            var coll = new NameValueCollection
            {
                {"grant_type", "refresh_token"},
                {"refresh_token", refreshToken}
            };

            var clientResult = new ClientSecretValidationResult
            {
                Client = IdentityConfig.RefreshTokenClient
            };

            UpdateAccessTokenLifetime(cttl, clientResult);
            return await Validate(coll, clientResult);
        }

        private async Task<Result<OAuthToken>> Validate(NameValueCollection collection, ClientSecretValidationResult clientResult)
        {
            var requestValidationResult = await _tokenRequestValidator.ValidateRequestAsync(collection, clientResult);
            //if (requestValidationResult.IsError)
            //    return Result<OAuthToken>.FromError(1, $"{requestValidationResult.Error}-" +
            //                                        $"{requestValidationResult.ErrorDescription}-" +
            //                                        $"{JsonConvert.SerializeObject(requestValidationResult.CustomResponse)}", 401);

            if (requestValidationResult.IsError)
                return Result<OAuthToken>.FromError(401, "Please login again", 401);

            var response = await _tokenResponseGenerator.ProcessAsync(requestValidationResult);
         
            await _eventService.RaiseAsync(new TokenIssuedSuccessEvent(response, requestValidationResult));
           
            return Result<OAuthToken>.FromValue(new OAuthToken
            {
                IdToken = !string.IsNullOrWhiteSpace(response.IdentityToken) ? response.IdentityToken : null,
                AccessToken = response.AccessToken,
                ExpiresIn = response.AccessTokenLifetime,
                RefreshToken = !string.IsNullOrWhiteSpace(response.RefreshToken) ? response.RefreshToken : null
            });
        }

        public async Task<JwtTokenValidationResult> ValidateGoogleJwtToken(string token)
        {
            return await _jwtTokenValidator.ValidateGoogleJwtToken(token);
        }

    }
}
