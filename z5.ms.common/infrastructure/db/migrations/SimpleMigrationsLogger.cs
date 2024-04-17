using System;
using Microsoft.Extensions.Logging;
using SimpleMigrations;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace z5.ms.common.infrastructure.db.migrations
{
    /// <inheritdoc />
    internal class SimpleMigrationsLogger : SimpleMigrations.ILogger
    {
        private readonly ILogger _logger;

        /// <inheritdoc />
        public SimpleMigrationsLogger(ILogger logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public void Info(string message) => _logger.LogInformation(message);

        /// <inheritdoc />
        public void LogSql(string sql) => _logger.LogTrace(sql);

        /// <inheritdoc />
        public void BeginSequence(MigrationData @from, MigrationData to) 
            => _logger.LogInformation($"Beginning migration from v{from.Version} to v{to.Version}");

        /// <inheritdoc />
        public void EndSequence(MigrationData @from, MigrationData to)
            => _logger.LogInformation($"Finished migration from v{from.Version} to v{to.Version}");

        public void EndSequenceWithError(Exception exception, MigrationData @from, MigrationData currentVersion)
            => _logger.LogError($"Error migrating from v{from.Version}. Final version v{currentVersion.Version}");

        /// <inheritdoc />
        public void BeginMigration(MigrationData migration, MigrationDirection direction)
            => _logger.LogInformation($"Migrating {direction}: v{migration.Version} ({migration.Description})");

        /// <inheritdoc />
        public void EndMigration(MigrationData migration, MigrationDirection direction)
            => _logger.LogDebug($"Finished migrating {direction}: v{migration.Version} ({migration.Description})");

        /// <inheritdoc />
        public void EndMigrationWithError(Exception exception, MigrationData migration, MigrationDirection direction)
            => _logger.LogError(exception, $"Error migrating {direction}: v{migration.Version} ({migration.Description})");
    }
}