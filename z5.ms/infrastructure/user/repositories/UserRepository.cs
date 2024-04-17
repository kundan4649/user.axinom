using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;
using z5.ms.common.extensions;
using z5.ms.common.infrastructure.db;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.infrastructure.user.repositories
{
    /// <summary> Interface to manage user information </summary>
    public interface IUserRepository : IBaseRepository<UserEntity>
    {
        Task<UserEntity> GetUser(AuthenticationMethod type, string userName);
        Task<List<UserEntity>> GetUsers(AuthenticationMethod type, string userNames);
        Task<UserEntity> GetUserFromRelica(AuthenticationMethod type, string userName);
        void SetLastlogin(Guid userId);
        void UpdateAdditionalInfo(UserEntity userId, int cttl, string deviceId, bool lastLoginUpdate);
    }

    /// <inheritdoc cref="IUserRepository" />
    public class UserRepository : BaseRepository<UserEntity>, IUserRepository
    {
        /// <inheritdoc />
        public UserRepository(IOptions<DbConnectionOptions> dbOptions) : base(dbOptions.Value.MSDatabaseConnection, dbOptions.Value.ReplicaDatabaseConnection)
        {
        }
        
        public async Task<UserEntity> GetUser(AuthenticationMethod type, string userName)
        {
            if (type.EnumMemberValue() == "Email")
                return (await GetItemsWhere(type.EnumMemberValue(), userName, 640))
               .OrderBy(u => u.CreationDate).FirstOrDefault(u => u.State != UserState.Deleted);
            else
                return (await GetItemsWhere(type.EnumMemberValue(), userName))
                    .OrderBy(u => u.CreationDate).FirstOrDefault(u => u.State != UserState.Deleted);
        }
        public async Task<UserEntity> GetUserFromRelica(AuthenticationMethod type, string userName)
        {
            return (await GetItemsWhereFromReplica(type.EnumMemberValue(), userName))
                .OrderBy(u => u.CreationDate).FirstOrDefault(u => u.State != UserState.Deleted);
        }
        public async Task<List<UserEntity>> GetUsers(AuthenticationMethod type, string userNames)
        {
            return (await GetItemsWhereIn(type.EnumMemberValue(), userNames))
                .OrderBy(u => u.CreationDate).Where(u => u.State != UserState.Deleted).ToList();
        }

        public void SetLastlogin(Guid userId)
        {
            try
            {
                using (var connection = Connection)
                {
                    connection.Execute(@"UPDATE Users SET LastLogin = @UtcNow WHERE Id = @UserId", new { userId, DateTime.UtcNow });
                }
            }
            catch
            {
                // ignored
            }
        }

        public void UpdateAdditionalInfo(UserEntity user, int cttl, string deviceId, bool lastLoginUpdate)
        {
            try
            {
                var fieldsToUpdate = new List<string>();
                fieldsToUpdate.Add(nameof(UserEntity.Json));

                //fieldsToUpdate.Add(nameof(UserEntity.LastLogin));

                var json = user.Json.ToJObject();

                if (json.ContainsKey("deviceid"))
                    json.Remove("deviceid");

                if (!string.IsNullOrWhiteSpace(deviceId))
                    json.Add("deviceid", deviceId);



                user.Json = json.ToString();
                if (lastLoginUpdate)
                {
                    fieldsToUpdate.Add(nameof(UserEntity.LastLogin));
                    user.LastLogin = DateTime.UtcNow;
                }

                base.UpdateItemsWhere(user, fieldsToUpdate.ToArray(), nameof(UserEntity.Id), user.Id).Wait();
            }
            catch (Exception ex)
            {
                // ignored
            }
        }
    }
}