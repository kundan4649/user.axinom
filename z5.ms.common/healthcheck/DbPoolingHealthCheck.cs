using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.HealthChecks;

namespace z5.ms.common.healthcheck
{
    /// <summary> Health check definition for database connection pooling </summary>
    public class DbPoolingHealthCheck : IHealthCheck
    {
        private readonly string _connectionString;

        private SqlConnection Connection => new SqlConnection(_connectionString);

        /// <inheritdoc />
        public DbPoolingHealthCheck(string connectionString)
        {
            _connectionString = connectionString + ";Min Pool Size = 1;";
        }

        /// <inheritdoc />
        public ValueTask<IHealthCheckResult> CheckAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return new ValueTask<IHealthCheckResult>(CheckDbPooling());
        }


        private IHealthCheckResult CheckDbPooling()
        {
            using (var conn = Connection)
            using (var comm = new SqlCommand("SELECT 1", conn))
            {
                try
                {
                    conn.Open();
                    var result = comm.ExecuteScalar();
                    return (int)result == 1
                        ? HealthCheckResult.Healthy("Pooling health check successful")
                        : HealthCheckResult.Unhealthy("Pooling health check failed");
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
