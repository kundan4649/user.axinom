using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.HealthChecks;

namespace z5.ms.common.healthcheck
{
    /// <summary> Health check definition for database connection pooling </summary>
    public class DbClearPoolHealthCheck : IHealthCheck
    {
        private readonly string _connectionString;

        private SqlConnection Connection => new SqlConnection(_connectionString);

        /// <inheritdoc />
        public DbClearPoolHealthCheck(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <inheritdoc />
        public ValueTask<IHealthCheckResult> CheckAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return new ValueTask<IHealthCheckResult>(ClearDbPool());
        }
        
        private IHealthCheckResult ClearDbPool()
        {
            using (var conn = Connection)
            using (var comm = new SqlCommand("SELECT 1", conn))
            {
                try
                {
                    SqlConnection.ClearPool(conn);
                    return HealthCheckResult.Healthy("Clear pool health check successful");
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
