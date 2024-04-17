using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Jose;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using z5.ms.common.extensions;

namespace z5.ms.common.validation
{
    /// <inheritdoc />
    public class TokenValidator : ITokenValidator
    {
        private const string AuthScheme = "bearer";

        private readonly byte[] _sessionKeyBytes;
        private readonly ILogger _logger;
        private readonly IConfiguration _config;

        /// <inheritdoc />
        public TokenValidator(IConfiguration config, ILoggerFactory loggerFactory)
        {
            _sessionKeyBytes = Convert.FromBase64String(config.GetSection("SessionKey").Value ?? "");
            _logger = loggerFactory.CreateLogger("TokenValidationService");
            _config = config;
        }

        ///<inheritdoc />
        public TokenValidationResult ValidateUser(ActionExecutingContext context)
        {
            var result = ValidateHeader(context, out var tokenString, AuthScheme);
            return !result.IsValid ? new TokenValidationResult(result.Error.Message) : ValidateUserToken(tokenString);
        }

        ///<inheritdoc />
        //public ValidationResult ValidateSecret(ActionExecutingContext context, string scheme, string configEntryName)
        //{
        //    var secret = _config.GetSection(configEntryName).Value;
        //    if (string.IsNullOrEmpty(secret))
        //    {
        //        _logger.LogError($"Config entry is missing - {configEntryName}");
        //        return new ValidationResult("Invalid authentication token");
        //    }

        //    if (scheme == null)
        //    {
        //        var secretParts = secret.Split(' ');
        //        if (secretParts.Length != 2)
        //        {
        //            _logger.LogError($"Scheme missing from config entry - {configEntryName}");
        //            return new ValidationResult("Invalid authentication token");
        //        }
        //        scheme = secretParts[0];
        //        secret = secretParts[1];
        //    }

        //    var result = ValidateHeader(context, out var tokenString, scheme);
        //    return !result.IsValid ? result : ValidateSecret(tokenString, secret);
        //}

        public ValidationResult ValidateSecret(ActionExecutingContext context, string scheme, string configEntryName)
        {
            var secrets = _config.GetSection(configEntryName).Value?.Split('|');
            var validationResult = new ValidationResult("No configuration identified for this category");

            foreach (var secretEntity in secrets)
            {
                var actualSecretEntity = secretEntity;

                if (string.IsNullOrEmpty(actualSecretEntity))
                {
                    _logger.LogError($"Config entry is missing - {configEntryName}");
                    return new ValidationResult("Invalid authentication token");
                }

                if (scheme == null)
                {
                    var secretParts = actualSecretEntity.Split(' ');
                    if (secretParts.Length != 2)
                    {
                        _logger.LogError($"Scheme missing from config entry - {configEntryName}");
                        return new ValidationResult("Invalid authentication token");
                    }
                    scheme = secretParts[0];
                    actualSecretEntity = secretParts[1];
                }

                var result = ValidateHeader(context, out var tokenString, scheme);

                if (!result.IsValid)
                {
                    validationResult = result;
                    break;
                }

                validationResult = ValidateSecret(tokenString, actualSecretEntity);

                if (validationResult.IsValid)
                    break;
                else
                    scheme = null;
            }

            return validationResult;
        }

        ///<inheritdoc />
        public ValidationResult ValidateSecretAndSignature(ActionExecutingContext context, string scheme, string secretConfigEntryName, string signingKeyConfigEntryName)
        {
            var secrets = _config.GetSection(secretConfigEntryName).Value?.Split('|');
            var validationResult = new ValidationResult("No configuration identified for this category");

            foreach (var secretEntity in secrets)
            {
                var actualSecretEntity = secretEntity;

                if (string.IsNullOrEmpty(actualSecretEntity))
                {
                    _logger.LogError($"Config entry is missing - {secretConfigEntryName}");
                    return new ValidationResult("Invalid authentication token");
                }

                if (scheme == null)
                {
                    var secretParts = actualSecretEntity.Split(' ');
                    if (secretParts.Length != 2)
                    {
                        _logger.LogError($"Scheme missing from config entry - {secretConfigEntryName}");
                        return new ValidationResult("Invalid authentication token");
                    }
                    scheme = secretParts[0];
                    actualSecretEntity = secretParts[1];
                }

                var result = ValidateHeader(context, out var tokenString, scheme);
                if (!result.IsValid)
                {
                    validationResult = result;
                    break;
                }

                var parts = tokenString.Split(':');
                if (parts.Length < 2)
                    return new ValidationResult("Signature is required");

                validationResult = ValidateSecret(parts[0], actualSecretEntity);

                if (validationResult.IsValid)
                {
                    validationResult = ValidateQuerySignature(context, parts[1], signingKeyConfigEntryName);

                    if (validationResult.IsValid)
                        break;
                }
                else
                    scheme = null;
            }

            return validationResult;
        }

        ///<inheritdoc />
        public ValidationResult ValidateUserBasic(ActionExecutingContext context, string scheme, string usernameConfigEntryName, string passwordConfigEntryName)
        {
            var result = ValidateHeader(context, out var tokenString, scheme);
            return !result.IsValid ? result : ValidateUserBasic(tokenString, usernameConfigEntryName, passwordConfigEntryName);
        }

        private ValidationResult ValidateHeader(ActionContext context, out string tokenString, string authScheme)
        {
            tokenString = "";
            _logger.LogTrace($"Validating token - {context.HttpContext.Request.Path}");

            var authHeader = context.HttpContext.Request.Headers.FirstOrDefault(h => h.Key == "Authorization").Value
                .ToString();
            if (string.IsNullOrEmpty(authHeader))
            {
                _logger.LogTrace($"token header is null - {context.HttpContext.Request.Path}");
                return new ValidationResult("Authentication header is required");
            }

            var headerSegments = authHeader.Split(' ');
            if (headerSegments.Length != 2)
            {
                _logger.LogTrace($"invalid token header format - {authHeader}");
                return new ValidationResult("Invalid authentication header format");
            }

            var scheme = headerSegments[0];
            tokenString = headerSegments[1];


            if (string.IsNullOrEmpty(scheme) || !authScheme.Equals(scheme, StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(tokenString))
            {
                _logger.LogTrace($"request authorization scheme or token string are invalid or missing - {authHeader}");
                return new ValidationResult("Authentication header is missing or empty");
            }

            return new ValidationResult();
        }

        ///<inheritdoc />
        public TokenValidationResult ValidateUserToken(string tokenString)
        {
            if (!ValidateAlgorithm(tokenString, "HS256"))
                return new TokenValidationResult("Invalid authentication token");

            AuthToken token;
            try
            {
                token = JWT.Decode<AuthToken>(tokenString, _sessionKeyBytes);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(
                    $"Failed to decode token - {JsonConvert.SerializeObject(tokenString)}. Error : {ex.Message}");
                return new TokenValidationResult("Invalid authentication token");
            }

            if (token == null || token.UserId == Guid.Empty)
            {
                _logger.LogTrace($"token or user ID in token is empty - {JsonConvert.SerializeObject(tokenString)}");
                return new TokenValidationResult("Missing session id");
            }

            _logger.LogTrace($"Token valid - {JsonConvert.SerializeObject(tokenString)}");

            return new TokenValidationResult(token);
        }

        ///<inheritdoc />
        public virtual Guid GetUserId(string tokenString)
        {
            return ValidateUserToken(tokenString)?.Token?.UserId ?? Guid.Empty;
        }

        private ValidationResult ValidateSecret(string tokenString, string secret)
        {
            if (tokenString != secret)
            {
                _logger.LogTrace($"Token is invalid or missing - {tokenString}");
                return new ValidationResult("Invalid authentication token");
            }

            _logger.LogTrace($"Token valid - {JsonConvert.SerializeObject(tokenString)}");

            return new ValidationResult();
        }

        private ValidationResult ValidateQuerySignature(ActionExecutingContext context, string signature, string signingKeyConfigEntryName)
        {
            // get signing key
            var signingKey = _config.GetSection(signingKeyConfigEntryName).Value;
            if (string.IsNullOrEmpty(signingKey))
            {
                _logger.LogError($"Config entry is missing - {signingKeyConfigEntryName}");
                return new ValidationResult("Invalid signature");
            }

            // concatenate request content
            var pathAndQuery = context.HttpContext.Request.Path.Value + context.HttpContext.Request.QueryString.Value;
            var body = context.HttpContext.Request.GetBody();
            var content = Regex.Replace($"{body}:{pathAndQuery}", "\\s*", "");

            // calculate key & validate
            var hmac = new HMACSHA256 { Key = Encoding.UTF8.GetBytes(signingKey) };
            var rawComputedSig = hmac.ComputeHash(Encoding.UTF8.GetBytes(content));
            var computedSig = Convert.ToBase64String(rawComputedSig);
            return signature == computedSig
                ? new ValidationResult()
                : new ValidationResult("Invalid signature");
        }

        private ValidationResult ValidateUserBasic(string tokenString, string usernameConfigEntryName, string passwordConfigEntryName)
        {
            var username = _config.GetSection(usernameConfigEntryName).Value;
            if (string.IsNullOrEmpty(username))
            {
                _logger.LogTrace($"Config entry is missing - {usernameConfigEntryName}");
                return new ValidationResult("Invalid authentication token");
            }

            var password = _config.GetSection(passwordConfigEntryName).Value;
            if (string.IsNullOrEmpty(password))
            {
                _logger.LogTrace($"Config entry is missing - {passwordConfigEntryName}");
                return new ValidationResult("Invalid authentication token");
            }

            var secret = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
            if (tokenString != secret)
            {
                _logger.LogTrace($"Token is invalid or missing - {tokenString}");
                return new ValidationResult("Invalid authentication token");
            }

            _logger.LogTrace($"Token valid - {JsonConvert.SerializeObject(tokenString)}");

            return new ValidationResult();
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
