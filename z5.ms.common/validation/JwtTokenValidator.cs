using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Jose;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using z5.ms.common.extensions;
using System.Linq;
using System.Collections.Generic;

namespace z5.ms.common.validation
{
    /// <summary>Token validation result</summary>
    public class JwtTokenValidationResult : ValidationResult
    {
        /// <summary>Authentication token</summary>
        public ClaimsPrincipal ClaimsPrincipal { get; set; }

        /// <summary>Constructor from token value</summary>
        /// <param name="cp"></param>
        /// <returns></returns>
        public JwtTokenValidationResult(ClaimsPrincipal cp)
        {
            this.ClaimsPrincipal = cp;
        }

        /// <inheritdoc />
        public JwtTokenValidationResult(string message, int code = 401)
            : base(message, code)
        {
        }
    }

    /// <summary>
    /// Implementation to validate jwt tokens created by identity server
    /// </summary>
    public class JwtTokenValidator : TokenValidator
    {
        private readonly string _issuer;
        private readonly JsonWebKeySet _jwks;
        private readonly string _audience;
        private readonly ILogger _logger;
        private readonly string _googleCertsUrl;
        private readonly string _googleTokenAudiences;
        private readonly string _googleIssuers;
        /// <inheritdoc />
        public JwtTokenValidator(IConfiguration config, ILoggerFactory loggerFactory) : base(config, loggerFactory)
        {
            _issuer = config["IdServiceBaseUrl"];
            _jwks = new JsonWebKeySet(config["TokenServiceOptions:JwksString"]);
            _audience = config["TokenServiceOptions:Audience"];
            _googleCertsUrl = config["GoogleCertsUrl"];
            _googleIssuers = config["GoogleIssuerUrls"];
            _googleTokenAudiences = config["GoogleTokenAudiences"];
            _logger = loggerFactory.CreateLogger(GetType().Name);
        }

        /// <summary>
        /// Validates provided jwt token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public JwtTokenValidationResult ValidateJwtToken(string token)
        {
            if (!ValidateAlgorithm(token, "RS256"))
                return new JwtTokenValidationResult("Invalid authentication token");
            try
            {
                var cp = new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters
                {
                    ValidIssuer = _issuer.TrimEnd('/'),
                    ValidAudiences = new[] { _audience },
                    IssuerSigningKeys = _jwks.GetSigningKeys()
                }, out var tr);

                return new JwtTokenValidationResult(cp);
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Validation failed for jwt token {token} / Error: {e.Message}");
                return new JwtTokenValidationResult(e.Message);
            }
        }

        /// <inheritdoc />
        public override Guid GetUserId(string token)
        {
            return ValidateJwtToken(token).ClaimsPrincipal.GetCurrentUserId();
        }

        /// <summary>
        /// Validates provided jwt token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<JwtTokenValidationResult> ValidateGoogleJwtToken(string token)
        {
            if (!ValidateAlgorithm(token, "RS256"))
                return new JwtTokenValidationResult("Invalid authentication token");
            try
            {

                WebRequest request = WebRequest.Create(_googleCertsUrl);
                WebResponse response = await request.GetResponseAsync();
                if (response.Headers["Cache-Control"] != null)
                    _logger.LogWarning($"Cache-Control header: {response.Headers["Cache-Control"]}");
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string responseCerts = await reader.ReadToEndAsync();
                JsonWebKeySet _jwksFromWeb = new JsonWebKeySet(responseCerts);

                TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
                {
                    ValidAudiences = _googleTokenAudiences.Contains(",") ? _googleTokenAudiences.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>() { _googleTokenAudiences },
                    IssuerSigningKeys = _jwksFromWeb.GetSigningKeys()
                };

                if (_googleIssuers.Contains(","))
                    tokenValidationParameters.ValidIssuers = _googleIssuers.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
                else
                    tokenValidationParameters.ValidIssuer = _googleIssuers.TrimEnd('/');


                var cp = new JwtSecurityTokenHandler().ValidateToken(token, tokenValidationParameters, out var tr);

                return new JwtTokenValidationResult(cp);
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Validation failed for jwt token {token} / Error: {e.Message}");
                return new JwtTokenValidationResult(e.Message);
            }
        }

        private bool ValidateAlgorithm(string token, string alg)
        {
            try
            {
                if (token.Length < 100)
                    return false;

                var jwtHeaders = JWT.Headers<JObject>(token);
                return !(!jwtHeaders.GetValue("alg")?.Value<string>()?.Equals(alg, StringComparison.OrdinalIgnoreCase) ?? true);
            }
            catch
            {
                return false;
            }
        }
    }
}

