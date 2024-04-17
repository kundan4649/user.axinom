using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace z5.ms.infrastructure.user.identity
{
    /// <summary>
    /// Configurations for token service
    /// </summary>
    public static class IdentityConfig
    {
        public static Client TokenClient { get; internal set; }
        public static Client RefreshTokenClient { get; internal set; }

        /// <summary>Get api resource configurations</summary>
        public static IEnumerable<ApiResource> GetApiResources()
            => new List<ApiResource>
            {
                new ApiResource("userapi", "User API"),
                new ApiResource("subscriptionapi", "Subscription API")
            };

        /// <summary>Get client configurations</summary>
        public static IEnumerable<Client> GetClients(IConfiguration configuration)
        {
            TokenClient = new Client
            {
                ClientId = "token_client",
                ClientName = "Token Client",
                AllowedGrantTypes = { "delegation" },
                AccessTokenType = AccessTokenType.Jwt,
                AccessTokenLifetime = int.TryParse(configuration["TokenServiceOptions:ClientTokenLifeTime"], out var lt) ? lt : 7200,
                ClientSecrets =
                {
                    new Secret(configuration["TokenServiceOptions:ClientTokenSecret"].Sha256())
                },
                AllowedScopes =
                {
                    "userapi",
                    "subscriptionapi"
                }
            };
            RefreshTokenClient = new Client
            {
                AbsoluteRefreshTokenLifetime = int.TryParse(configuration["TokenServiceOptions:RefreshClientTokenLifeTime"], out var rlt) ? rlt : 1800,
                SlidingRefreshTokenLifetime = int.TryParse(configuration["TokenServiceOptions:SlidingRefreshTokenLifetime"], out var srtf) ? srtf : 1800,
                RefreshTokenUsage = TokenUsage.ReUse,
                RefreshTokenExpiration = TokenExpiration.Sliding,
                UpdateAccessTokenClaimsOnRefresh = true,
                ClientId = "refresh_token_client",
                ClientName = "Refresh Token Client",
                AllowedGrantTypes = { "delegation" },
                AccessTokenType = AccessTokenType.Jwt,
                AccessTokenLifetime = int.TryParse(configuration["TokenServiceOptions:AccessTokenLifeTime"], out var alt) ? alt : 1800,
                ClientSecrets =
                {
                    new Secret(configuration["TokenServiceOptions:RefreshClientTokenSecret"].Sha256())
                },
                AllowedScopes =
                {
                    "userapi",
                    "subscriptionapi",
                    IdentityServerConstants.StandardScopes.OpenId
                },
                AllowOfflineAccess = true // enables refresh tokens
            };
            return new List<Client> { TokenClient, RefreshTokenClient };
        }

        /// <summary>Get identity resource configurations</summary>
        public static IEnumerable<IdentityResource> GetIdentityResources()
            => new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
    }
}