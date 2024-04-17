using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using Dapper.FastCrud;
using z5.ms.common.abstractions;
using z5.ms.common.extensions;

namespace z5.ms.common.infrastructure.db
{
    /// <summary>Base database repository</summary>
    public interface IBaseRepository<T>
    {
        /// <summary>Get a new connection</summary>
        IDbConnection Connection { get; }

        /// <summary>Get specified item from database for specified type</summary>
        Task<T> Get(T item);

        /// <summary>Get all the items from database for specified type</summary>
        Task<List<T>> Get();

        /// <summary>Get specified item from database for specified type</summary>
        Task<T> Get(Guid id);

        /// <summary>Get specified item from Replica database for specified type</summary>
        Task<T> GetFromReplica(Guid id);

        /// <summary>Update specified item from database for specified type using existing connection</summary>
        Task<Result<Success>> Update(T item, IDbConnection conn, IDbTransaction trn = null);

        /// <summary>Insert a specified item to database for specified type using existing connection</summary>
        Task<Result<Success>> Insert(T item, IDbConnection conn, IDbTransaction trn = null);

        /// <summary>Delete specified item from database for specified type using existing connection</summary>
        Task<Result<Success>> Delete(T item, IDbConnection conn, IDbTransaction trn = null);

        /// <summary>Update specified item from database for specified type</summary>
        Task<Result<Success>> Update(T item);

        /// <summary>Insert a specified item to database for specified type</summary>
        Task<Result<Success>> Insert(T item);

        /// <summary>Delete specified item from database for specified type</summary>
        Task<Result<Success>> Delete(T item);

        /// <summary>Get items from database for specified type and criteria parameters using an AND clause</summary>
        Task<IEnumerable<T>> GetItemsWhere(string name, object value, string name2 = null, object value2 = null);

        /// <summary>Get items from Watch list table for specified type and criteria parameters using an AND clause</summary>
        Task<IEnumerable<T>> GetWatchListItemsWhere(string name, object value, string name2 = null, object value2 = null);

        /// <summary>Get items from settings table for specified type and criteria parameters using an AND clause</summary>
        Task<IEnumerable<T>> GetSettingItemsWhere(string name, object value, string name2 = null, object value2 = null);

        /// <summary>Get items from database for specified type and criteria parameters using an AND clause</summary>
        Task<IEnumerable<T>> GetItemsWhere(string name, object value, int valueLength);

        /// <summary>Get items from database for specified type and criteria parameters using an AND clause</summary>
        Task<IEnumerable<T>> GetItemsWhereIn(string name, object value);

        /// <summary>Get count of items from database for specified type and criteria parameters using an AND clause</summary>
        Task<int> CountItemsWhere(string name, object value, string name2 = null, object value2 = null);

        /// <summary>Update items in the database for specified type and criteria parameters using an AND clause</summary>
        Task<int> UpdateItemsWhere(T updateItem, string[] fieldsToUpdate, string name, object value, string name2 = null, object value2 = null);

        /// <summary>Delete items from database for specified type and criteria parameters using an AND clause</summary>
        Task<int> DeleteItemsWhere(string name, object value, string name2 = null, object value2 = null);

        /// <summary>Get a single result from replica database for specified type and criteria parameters using an AND clause</summary>
        Task<T> SingleOrDefaultWhereFromReplica(string name, object value, string name2 = null, object value2 = null);
        /// <summary>Get a single result for specified type and criteria parameters using an AND clause</summary>
        Task<T> SingleOrDefaultWhere(string name, object value, string name2 = null, object value2 = null);

        /// <summary>Checks there is any result for specified type and criteria parameters using an AND clause</summary>
        Task<bool> IsAnyWhere(string name, object value, string name2 = null, object value2 = null);

        /// <summary>Determine whether an error returned by an Insert, Update or Delete method call is caused by a unique constraint violation</summary>
        bool IsUniqueConstraintViolation(Error error);
    }

    /// <inheritdoc />
    public class BaseRepository<T> : IBaseRepository<T>
    {
        private readonly string _connectionString;
        private readonly string _connectionStringReplica;

        /// <inheritdoc />
        protected BaseRepository(string connectionString,string connectionStringReplica = null)
        {
            _connectionString = connectionString;
            _connectionStringReplica = !string.IsNullOrEmpty(connectionStringReplica) ? connectionStringReplica : connectionString;
        }

        /// <inheritdoc />
        /// <summary> Database connection instance </summary>
        public IDbConnection Connection => _connectionString.Contains(".sqlite")
            ? new SQLiteConnection(_connectionString) // for testing 
            : (IDbConnection)new SqlConnection(_connectionString);

        public IDbConnection ConnectionReplicaDB => _connectionStringReplica.Contains(".sqlite")
           ? new SQLiteConnection(_connectionStringReplica) // for testing 
           : (IDbConnection)new SqlConnection(_connectionStringReplica);

        /// <inheritdoc />
        public async Task<T> Get(T item)
        {
            using (var connection = Connection)
            {
                return await connection.GetAsync(item);
            }
        }

        /// <inheritdoc />
        public async Task<List<T>> Get()
        {
            using (var connection = Connection)
            {
                var result = await connection.FindAsync<T>(statement => statement.Where($"1 = 1"));
               return result.ToList<T>();
            }
        }

        /// <inheritdoc />
        public async Task<T> Get(Guid id)
        {
            var item = Activator.CreateInstance<T>();
            var prop = item.GetType().GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);
            if (null != prop && prop.CanWrite)
                prop.SetValue(item, id);

            using (var connection = Connection)
            {
                return await connection.GetAsync(item);
            }
        }

        /// <inheritdoc />
        public async Task<T> GetFromReplica(Guid id)
        {
            var item = Activator.CreateInstance<T>();
            var prop = item.GetType().GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);
            if (null != prop && prop.CanWrite)
                prop.SetValue(item, id);

            using (var connection = ConnectionReplicaDB)
            {
                return await connection.GetAsync(item);
            }
        }

        /// <inheritdoc />
        public async Task<Result<Success>> Update(T item, IDbConnection conn, IDbTransaction trn = null)
        {
            try
            {
                var result = await conn.UpdateAsync(item,
                    x =>
                    {
                        if (trn != null) x.AttachToTransaction(trn);
                    });

                return !result
                    ? Result<Success>.FromError(2, $"Update {typeof(T).Name} failed (not found)")
                    : new Result<Success>();
            }
            catch (SqlException ex) when (ex.IsUniqueConstraintViolation())
            {
                return Result<Success>.FromError(99, $"Update {typeof(T).Name} failed (unique constraint violation)");
            }
        }

        /// <inheritdoc />
        public async Task<Result<Success>> Update(T item)
        {
            using (var connection = Connection)
            {
                return await Update(item, connection);
            }
        }

        /// <inheritdoc />
        public async Task<Result<Success>> Insert(T item, IDbConnection conn, IDbTransaction trn = null)
        {
            try
            {
                await conn.InsertAsync(item,
                    x =>
                    {
                        if (trn != null) x.AttachToTransaction(trn);
                    });

                return new Result<Success>();
            }
            catch (DbException ex) when (ex.IsUniqueConstraintViolation())
            {
                return Result<Success>.FromError(99, $"Update {typeof(T).Name} failed (unique constraint violation)");
            }
        }

        /// <inheritdoc />
        public async Task<Result<Success>> Insert(T item)
        {
            using (var connection = Connection)
            {
                return await Insert(item, connection);
            }
        }

        /// <inheritdoc />
        public async Task<Result<Success>> Delete(T item, IDbConnection conn, IDbTransaction trn = null)
        {
            var result = await conn.DeleteAsync(item,
                x =>
                {
                    if (trn != null) x.AttachToTransaction(trn);
                });

            return !result
                ? Result<Success>.FromError(2, $"Delete {typeof(T).Name} failed (not found)")
                : new Result<Success>();
        }

        /// <inheritdoc />
        public async Task<Result<Success>> Delete(T item)
        {
            using (var connection = Connection)
            {
                return await Delete(item, connection);
            }
        }

        /// <inheritdoc />
        public async Task<int> CountItemsWhere(string name, object value, string name2 = null, object value2 = null)
        {
            using (var connection = Connection)
            {
                return name2 == null
                    ? await connection.CountAsync<T>(statement => statement
                        .Where($"{name}=@value")
                        .WithParameters(new { value }))

                    : await connection.CountAsync<T>(statement => statement
                        .Where($"{name}=@value AND {name2}=@value2")
                        .WithParameters(new { value, value2 }));
            }
        }
        public async Task<IEnumerable<T>> GetWatchListItemsWhere(string name, object value, string name2 = null, object value2 = null)
        {
            using (var connection = ConnectionReplicaDB)
            {
                if (name2 == null)
                    return await connection.QueryAsync<T>($"Select Id,UserId,AssetId,AssetType,Duration,Date from Watchlist with (Nolock) WHERE {name} = '{value}'");
                else
                    return await connection.QueryAsync<T>($"Select Id,UserId,AssetId,AssetType,Duration,Date from Watchlist with (Nolock) WHERE {name} = '{value}' AND  {name2} = '{value2}' ");
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<T>> GetSettingItemsWhere(string name, object value, string name2 = null, object value2 = null)
        {
            using (var connection = Connection)
            {
                if (name2 == null)
                    return await connection.QueryAsync<T>($"Select Id,UserId,SettingKey,SettingValue,LastUpdate from Settings with (Nolock) WHERE {name} = '{value}'");
                else
                    return await connection.QueryAsync<T>($"Select Id,UserId,SettingKey,SettingValue,LastUpdate from Settings with (Nolock) WHERE {name} = '{value}' AND  {name2} = '{value2}' ");
            }
        }



        /// <inheritdoc />
        public async Task<IEnumerable<T>> GetItemsWhereFromReplica(string name, object value, string name2 = null, object value2 = null)
        {
            using (var connection = ConnectionReplicaDB)
            {
                return name2 == null
                    ? await connection.FindAsync<T>(statement => statement
                        .Where($"{name}=@value")
                        .WithParameters(new { value }))
                    : await connection.FindAsync<T>(statement => statement
                        .Where($"{name}=@value AND {name2}=@value2")
                        .WithParameters(new { value, value2 }));
            }

        }

        /// <inheritdoc />
        public async Task<IEnumerable<T>> GetItemsWhere(string name, object value, string name2 = null, object value2 = null)
        {
            using (var connection = Connection)
            {
                return name2 == null
                    ? await connection.FindAsync<T>(statement => statement
                        .Where($"{name}=@value")
                        .WithParameters(new { value }))

                    : await connection.FindAsync<T>(statement => statement
                        .Where($"{name}=@value AND {name2}=@value2")
                        .WithParameters(new { value, value2 }));
            }
        }

        public async Task<IEnumerable<T>> GetItemsWhere(string name, object value, int valueLength)
        {
            var parameters = new DynamicParameters();

            if(valueLength > 0)
            parameters.Add("@value", value, DbType.String, ParameterDirection.Input, valueLength);
            else
            parameters.Add("@value", value);

            using (var connection = Connection)
            {
                return await connection.FindAsync<T>(statement => statement
                       .Where($"{name}=@value")
                       .WithParameters(parameters));                    
            }
        }

        public async Task<IEnumerable<T>> GetItemsWhereIn(string name, object value)
        {
            using (var connection = Connection)
            {
                return await connection.FindAsync<T>(statement => statement
                        .Where($"{name} in (@value)")
                        .WithParameters(new { value }));
            }
        }

        /// <inheritdoc />
        public async Task<int> UpdateItemsWhere(T updateItem, string[] fieldsToUpdate, string name, object value, string name2 = null, object value2 = null)
        {
            if (fieldsToUpdate.Length == 0)
                return 0;

            // https://github.com/MoonStorm/Dapper.FastCRUD/issues/61
            var partialMapping = OrmConfiguration
                .GetDefaultEntityMapping<T>().Clone()
                .UpdatePropertiesExcluding(propertyMapping => propertyMapping.IsExcludedFromUpdates = true, fieldsToUpdate);

            var condition = $"{name}={value.SqlValue()} ";

            if (!string.IsNullOrWhiteSpace(name2))
                condition += $"AND {name2}={value2.SqlValue()}";

            using (var connection = Connection)
            {
                return await connection
                    .BulkUpdateAsync(updateItem, statement => statement
                        .Where($"{condition}")
                        .WithEntityMappingOverride(partialMapping));
            }
        }

        /// <inheritdoc />
        public async Task<int> DeleteItemsWhere(string name, object value, string name2 = null, object value2 = null)
        {
            using (var connection = Connection)
            {
                return name2 == null
                    ? await connection.BulkDeleteAsync<T>(statement => statement
                        .Where($"{name}=@value")
                        .WithParameters(new { value }))

                    : await connection.BulkDeleteAsync<T>(statement => statement
                        .Where($"{name}=@value AND {name2}=@value2")
                        .WithParameters(new { value, value2 }));
            }
        }

        /// <inheritdoc />
        public async Task<T> SingleOrDefaultWhere(string name, object value, string name2 = null, object value2 = null)
        {
            using (var connection = Connection)
            {
                var result = name2 == null
                    ? await connection.FindAsync<T>(statement => statement
                        .Where($"{name}=@value")
                        .WithParameters(new { value }).OrderBy($"1").Top(2))

                    : await connection.FindAsync<T>(statement => statement
                        .Where($"{name}=@value AND {name2}=@value2")
                        .WithParameters(new { value, value2 }).OrderBy($"1").Top(2));

                return result.SingleOrDefault();
            }
        }

        /// <inheritdoc />
        public async Task<T> SingleOrDefaultWhereFromReplica(string name, object value, string name2 = null, object value2 = null)
        {
            using (var connection = ConnectionReplicaDB)
            {
                var result = name2 == null
                    ? await connection.FindAsync<T>(statement => statement
                        .Where($"{name}=@value")
                        .WithParameters(new { value }).OrderBy($"1").Top(2))

                    : await connection.FindAsync<T>(statement => statement
                        .Where($"{name}=@value AND {name2}=@value2")
                        .WithParameters(new { value, value2 }).OrderBy($"1").Top(2));

                return result.SingleOrDefault();
            }
        }
        /// <inheritdoc />
        public async Task<T> SingleOrDefaultWhere2(string name, object value, string name2 = null, object value2 = null, string name3 = null, object value3 = null)
        {
            FormattableString clause;
            object parameters;
            if (name2 == null)
            {
                parameters = new { value };
                clause = $"{name} = @value";
            }
            else if (name3 == null)
            {
                parameters = new { value, value2 };
                clause = $"{name} = @value AND {name2} = @value2";
            }
            else
            {
                parameters = new { value, value2, value3 };
                clause = $"{name} = @value AND {name2} = @value2 AND {name3} = @value3";
            }
            using (var connection = Connection)
            {
                var result = await connection.FindAsync<T>(statement => statement
                      .Where(clause)
                      .WithParameters(parameters).OrderBy($"1").Top(2));
                return result.SingleOrDefault();
            }
        }

        /// <inheritdoc />
        public async Task<bool> IsAnyWhere(string name, object value, string name2 = null, object value2 = null)
        {
            using (var connection = Connection)
            {
                var result = name2 == null
                    ? await connection.FindAsync<T>(statement => statement
                        .Where($"{name}=@value")
                        .WithParameters(new { value }).OrderBy($"1").Top(1))

                    : await connection.FindAsync<T>(statement => statement
                        .Where($"{name}=@value AND {name2}=@value2")
                        .WithParameters(new { value, value2 }).OrderBy($"1").Top(1));

                return result.Any();
            }
        }

        /// <inheritdoc />
        public bool IsUniqueConstraintViolation(Error error) => error?.Code == 99;
    }

    /// <summary>Database extensions</summary>
    public static class DatabaseExtensions
    {
        /// <summary>Determines whether an exception is due to a unique constraint violation</summary>
        public static bool IsUniqueConstraintViolation(this DbException ex)
        {
            var msg = ex.Message.ToLower(CultureInfo.InvariantCulture);
            return msg.Contains("unique") || msg.Contains("duplicate");
        }

        /// <summary>Opens the connection and creates a new transaction</summary>
        public static IDbTransaction GetTransaction(this IDbConnection connection, IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            return connection.BeginTransaction(isolationLevel);
        }
    }
}
