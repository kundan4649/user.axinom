namespace z5.ms.common.infrastructure.db.migrations
{
    /// <summary>SimpleMigrations database migration service</summary>
    public interface IMigrationService
    {
        /// <summary>Migrate the database to a specific version</summary>
        /// <param name="version"></param>
        void MigrateToVersion(long version);

        /// <summary>Migrate the database to the latest version</summary>
        void MigrateToLatest();
    }
}