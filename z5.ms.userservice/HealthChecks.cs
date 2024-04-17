using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using z5.ms.common.healthcheck;

namespace z5.ms.userservice
{
    public static class HealthChecks
    {
        /// <summary>
        /// Extension to initialize health checks in startup
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddUserServiceHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks(checks =>
            {
                checks.AddCustomCheck("db_status", "Health check for user db connection",
                    new DbHealthCheck(configuration["ConnectionStrings:MSDatabaseConnection"]));
                checks.AddCustomCheck("iddb_status", "Health check for identity db connection",
                    new DbHealthCheck(configuration["ConnectionStrings:IdServerDatabaseConnection"]));
                checks.AddCustomCheck("watchhistory_table_status", "Health check for user db WatchHistory table",
                    new DbTableHealthCheck(configuration["ConnectionStrings:MSDatabaseConnection"], "watchhistory"));
                checks.AddCustomCheck("favorites_table_status", "Health check for user db Favorites table",
                    new DbTableHealthCheck(configuration["ConnectionStrings:MSDatabaseConnection"], "favorites"));
                checks.AddCustomCheck("reminders_table_status", "Health check for user db Reminders table",
                    new DbTableHealthCheck(configuration["ConnectionStrings:MSDatabaseConnection"], "reminders"));
                checks.AddCustomCheck("settings_table_status", "Health check for user db Settings table",
                    new DbTableHealthCheck(configuration["ConnectionStrings:MSDatabaseConnection"], "settings"));
                checks.AddCustomCheck("db_pooling_status", "Health check for db connection pooling",
                    new DbPoolingHealthCheck(configuration["ConnectionStrings:MSDatabaseConnection"]));
                checks.AddCustomCheck("db_clear_pool", "Health check to clear db connection pool",
                    new DbClearPoolHealthCheck(configuration["ConnectionStrings:MSDatabaseConnection"]));
                checks.AddCustomCheck("socket_status", "Health check for socket connection status", new SocketConnectionCheck());
            });
        }
    }
}
