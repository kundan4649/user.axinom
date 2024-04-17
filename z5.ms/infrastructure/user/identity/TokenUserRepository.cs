using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dapper;
using Dapper.FastCrud;
using IdentityModel;
using Microsoft.Extensions.Options;
using z5.ms.common.infrastructure.db;
using z5.ms.common.infrastructure.id;
using z5.ms.domain.user.datamodels;

namespace z5.ms.infrastructure.user.identity
{
    /// <summary>User repository for token service</summary>
    public interface ITokenUserRepository
    {
        /// <summary>
        /// Find subject(user) by user id from user db  
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserEntity> FindBySubjectId(string id);

        /// <summary>
        /// Find subject(user) by user name from user db  
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<UserEntity> FindByUsername(string username);

        /// <summary>
        /// Find subject(user) by external provider info from user db  
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="providerSubjectId"></param>
        /// <returns></returns>
        Task<UserEntity> FindByExternalProvider(string provider, string providerSubjectId);

        /// <summary>
        /// Initialize and create a new user 
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="providerSubjectId"></param>
        /// <param name="claims"></param>
        /// <returns></returns>
        Task<UserEntity> AutoProvisionUser(string provider, string providerSubjectId, IEnumerable<Claim> claims);

        /// <summary>
        /// Validate user credentials 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> ValidateCredentials(string username, string password);
    }

    /// <inheritdoc />
    public class TokenUserRepository : ITokenUserRepository
    {
        private readonly IPasswordEncryptionStrategy _encryptionStrategy;
        private readonly DbConnectionOptions _dbConnectionOptions;

        /// <inheritdoc />
        public TokenUserRepository(IPasswordEncryptionStrategy encryptionStrategy, IOptions<DbConnectionOptions> dbOptions)
        {
            _encryptionStrategy = encryptionStrategy;
            _dbConnectionOptions = dbOptions.Value;
        }

        /// <summary> Database connection instance </summary>
        private IDbConnection Connection => new SqlConnection(_dbConnectionOptions.MSDatabaseConnection);

        /// <inheritdoc />
        public async Task<UserEntity> FindBySubjectId(string id)
        {
            const string query = "SELECT * FROM Users WHERE Id = @UserId";
            using (var conn = Connection)
            {
                return await conn.QuerySingleOrDefaultAsync<UserEntity>(query, new { UserId = id });
            }
        }

        /// <inheritdoc />
        public async Task<UserEntity> FindByUsername(string username)
        {
            const string query = "SELECT * FROM Users WHERE Email = @Email";
            using (var conn = Connection)
            {
                return await conn.QuerySingleOrDefaultAsync<UserEntity>(query, new { Email = username });
            }
        }

        /// <inheritdoc />
        public async Task<UserEntity> FindByExternalProvider(string provider, string providerSubjectId)
        {
            const string query = "SELECT * FROM Users "
                                 + "WHERE ProviderName = @ProviderName AND ProviderSubjectId = @ProviderSubjectId";
            using (var conn = Connection)
            {
                return await conn.QuerySingleOrDefaultAsync<UserEntity>(query,
                    new
                    {
                        ProviderName = provider,
                        ProviderSubjectId = providerSubjectId
                    });
            }
        }

        /// <inheritdoc />
        public async Task<UserEntity> AutoProvisionUser(string provider, string providerSubjectId, IEnumerable<Claim> claims)
        {
            // create a list of claims that we want to transfer into our store
            var filtered = new List<Claim>();

            foreach (var claim in claims)
            {
                // if the external system sends a display name - translate that to the standard OIDC name claim
                if (claim.Type == ClaimTypes.Name)
                {
                    filtered.Add(new Claim(JwtClaimTypes.Name, claim.Value));
                }
                // if the JWT handler has an outbound mapping to an OIDC claim use that
                else if (JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.ContainsKey(claim.Type))
                {
                    filtered.Add(new Claim(JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap[claim.Type], claim.Value));
                }
                // copy the claim as-is
                else
                {
                    filtered.Add(claim);
                }
            }

            // if no display name was provided, try to construct by first and/or last name
            if (filtered.All(x => x.Type != JwtClaimTypes.Name))
            {
                var first = filtered.FirstOrDefault(x => x.Type == JwtClaimTypes.GivenName)?.Value;
                var last = filtered.FirstOrDefault(x => x.Type == JwtClaimTypes.FamilyName)?.Value;
                if (first != null && last != null)
                {
                    filtered.Add(new Claim(JwtClaimTypes.Name, first + " " + last));
                }
                else if (first != null)
                {
                    filtered.Add(new Claim(JwtClaimTypes.Name, first));
                }
                else if (last != null)
                {
                    filtered.Add(new Claim(JwtClaimTypes.Name, last));
                }
            }

            // create a new unique subject id
            var sub = Guid.NewGuid();

            // check if a display name is available, otherwise fallback to subject id
            var name = filtered.FirstOrDefault(c => c.Type == JwtClaimTypes.Name)?.Value ?? sub.ToString();

            var email = filtered.FirstOrDefault(c => c.Type == JwtClaimTypes.Email)?.Value ?? $"{sub.ToString()}@example.com";

            var user = new UserEntity
            {
                Id = sub,
                FirstName = name,
                LastName = name,
                Email = email,
                PasswordHash = _encryptionStrategy.HashPassword(Guid.NewGuid().ToString("N")),
                System = "Internal",
                State = UserState.Registered,
                ProviderName = provider,
                ProviderSubjectId = providerSubjectId
            };

            using (var conn = Connection)
            {
                await conn.InsertAsync(user);
            }

            return user;
        }

        /// <inheritdoc />
        public async Task<bool> ValidateCredentials(string username, string password)
        {
            var user = await FindByUsername(username);

            return user != null && _encryptionStrategy.VerifyPassword(password, user.PasswordHash);
        }
    }
}