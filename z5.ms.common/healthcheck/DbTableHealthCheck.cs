using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.HealthChecks;

namespace z5.ms.common.healthcheck
{
    /// <summary> Health check definition for database connections </summary>
    public class DbTableHealthCheck : IHealthCheck
    {
        private readonly string _connectionString;
        private readonly string _table;

        private SqlConnection Connection => new SqlConnection(_connectionString);

        /// <inheritdoc />
        public DbTableHealthCheck(string connectionString, string tableName)
        {
            _connectionString = connectionString;
            _table = tableName;
        }

        /// <inheritdoc />
        public ValueTask<IHealthCheckResult> CheckAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return new ValueTask<IHealthCheckResult>(CheckTable(_table));
        }

        private IHealthCheckResult CheckTable(string tableName)
        {
            using (var conn = Connection)
            using (var comm = new SqlCommand($"SELECT TOP 1 * FROM {tableName}", conn))
            {
                try
                {
                    conn.Open();
                    var result = comm.ExecuteReader();
                    return result != null
                        ? HealthCheckResult.Healthy("Table health check successful")
                        : HealthCheckResult.Unhealthy("Table health check failed");
                }
                catch (Exception e)
                {
                    return HealthCheckResult.Unhealthy(e.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}
