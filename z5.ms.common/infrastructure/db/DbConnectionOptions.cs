namespace z5.ms.common.infrastructure.db
{
    /// <summary> Options for database connections</summary>
    public class DbConnectionOptions
    {
        /// <summary>User service database connection string</summary>
        public string MSDatabaseConnection { get; set; }

        /// <summary>User service replica database connection string</summary>
        public string ReplicaDatabaseConnection { get; set; }

        /// <summary>Id service database connection string</summary>
        public string IdServerDatabaseConnection { get; set; }

        /// <summary>Storage connection string for the azure storage queue</summary>
        public string QueueStorageConnection { get; set; }

        /// <summary>Watch history database connection string</summary>
        public string WatchHistoryDbConnection { get; set; }
    }
}