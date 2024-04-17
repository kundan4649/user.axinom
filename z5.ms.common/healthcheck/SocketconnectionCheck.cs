using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.HealthChecks;

namespace z5.ms.common.healthcheck
{
    /// <summary> Health check definition for database connection </summary>
    public class SocketConnectionCheck : IHealthCheck
    {
        /// <inheritdoc />
        public ValueTask<IHealthCheckResult> CheckAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return new ValueTask<IHealthCheckResult>(CheckSocketConnection());
        }

        private IHealthCheckResult CheckSocketConnection()
        {
            var ipAddress = Dns.GetHostEntry("google.com").AddressList[0];
            using (var socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
            {
                try
                {
                    socket.Connect(new IPEndPoint(ipAddress, 80));                   
                    return socket.Connected
                        ? HealthCheckResult.Healthy("Socket health check successful")
                        : HealthCheckResult.Unhealthy("Socket health check failed");
                }
                catch (Exception e)
                {
                    return HealthCheckResult.Unhealthy(e.Message);
                }
                finally
                {
                    socket.Disconnect(false);
                }
            }
        }
    }
}
