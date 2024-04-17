using System.Data.Common;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SimpleMigrations;
using SimpleMigrations.DatabaseProvider;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace z5.ms.common.infrastructure.db.migrations
{
    /// <inheritdoc />
    public class MigrationService : IMigrationService
    {
        private readonly ILogger _logger;
        private readonly SimpleMigrator _migrator;

        /// <summary> Configuration instance </summary>
        public static IConfiguration Configuration;

        /// <inheritdoc />
        public MigrationService(DbConnection connection, ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger(GetType().Name);
            var databaseProvider = new MssqlDatabaseProvider(connection);
            _migrator = new SimpleMigrator(Assembly.GetEntryAssembly(), databaseProvider)
            {
                Logger = new SimpleMigrationsLogger(_logger)
            };

            Configuration = configuration;
        }

        /// <inheritdoc />
        public void MigrateToVersion(long version)
        {
            _migrator.Load();
            _migrator.MigrateTo(version);
            _logger.LogInformation($"Database migration finished (v{_migrator.CurrentMigration.Version}).");
        }

        /// <inheritdoc />
        public void MigrateToLatest()
        {
            _migrator.Load();
            _migrator.MigrateToLatest();
            _logger.LogInformation($"Database migration finished (v{_migrator.CurrentMigration.Version}).");
        }
    }
}