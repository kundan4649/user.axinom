using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using MediatR;

namespace z5.ms.common.infrastructure.db
{
    /// <summary>Base query handler database queries</summary>
    public abstract class DbQueryHandler<TQuery, TResult> : IAsyncRequestHandler<TQuery, TResult> where TQuery : IRequest<TResult>
    {
        private readonly string _connectionString;

        /// <summary> Database connection instance </summary>
        protected IDbConnection Connection => _connectionString.Contains(".sqlite")
            ? new SqliteConnection(_connectionString) // for testing 
            : (IDbConnection)new SqlConnection(_connectionString);

        /// <inheritdoc />
        protected DbQueryHandler(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>Handle the specified query</summary>
        public abstract Task<TResult> Handle(TQuery message);
    }
}