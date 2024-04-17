using SimpleMigrations;

namespace z5.ms.user.migrations.SqlMigrations
{
    /// <summary>
    /// Database migration definitions
    /// </summary>
    public class MssqlMigrations
    {
        /// <inheritdoc/>
        [Migration(1, "version 1.0")]
        public class CreateDatabaseV1 : Migration
        {
            /// <inheritdoc/>
            protected override void Up()
            {
                Execute(@"CREATE TABLE Users (Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
                                                    Email NVARCHAR(MAX) NULL,
                                                    Mobile NVARCHAR(MAX) NULL,
                                                    FirstName NVARCHAR(MAX) NOT NULL,
                                                    LastName NVARCHAR(MAX) NOT NULL,
                                                    Password NVARCHAR(48) NOT NULL,
                                                    EmailConfirmationKey NVARCHAR(48) NULL,
                                                    MobileConfirmationKey NVARCHAR(48) NULL,
                                                    EmailConfirmationExpiration DATETIME NULL,
                                                    MobileConfirmationExpiration DATETIME NULL,
                                                    IsEmailConfirmed BIT NULL,
                                                    IsMobileConfirmed BIT NULL,
                                                    PasswordResetKey NVARCHAR(48),
                                                    PasswordResetExpiration DATETIME NULL,
                                                    LastLogin DATETIME NULL,
                                                    System INT NULL,
                                                    RegistrationCountry NVARCHAR(MAX) NULL,
                                                    MacAddress NVARCHAR(MAX) NULL,
                                                    Birthday DATETIME NULL,
                                                    Gender INT NULL,
                                                    Activated DATETIME NULL)");
            }

            /// <inheritdoc/>
            protected override void Down()
            {
                Execute("DROP TABLE Users");
            }
        }

        /// <inheritdoc/>
        [Migration(2, "version 1.1")]
        public class CreateDatabaseV2 : Migration
        {
            /// <inheritdoc/>
            protected override void Up()
            {
                Execute(@"sp_rename 'Users.Activated', 'ActivationDate', 'COLUMN'");
                Execute(@"ALTER TABLE dbo.Users ADD Activated bit NULL");
            }

            /// <inheritdoc/>
            protected override void Down()
            {
                Execute(@"ALTER TABLE dbo.Users DROP COLUMN Activated");
                Execute(@"sp_rename 'Users.ActivationDate', 'Activated', 'COLUMN'");
            }
        }

        /// <inheritdoc/>
        [Migration(3, "version 1.2")]
        public class CreateDatabaseV3 : Migration
        {
            /// <inheritdoc/>
            protected override void Up()
            {
                Execute(@"CREATE TABLE Favorites (Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
                                                  UserId UNIQUEIDENTIFIER NOT NULL,
                                                  AssetId NVARCHAR(MAX) NOT NULL,
                                                  AssetType INT NOT NULL,
                                                  Duration INT NULL,
                                                  Date DATETIME NULL)");
                Execute(@"CREATE TABLE Watchlist (Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
                                                  UserId UNIQUEIDENTIFIER NOT NULL,
                                                  AssetId NVARCHAR(MAX) NOT NULL,
                                                  AssetType INT NOT NULL,
                                                  Duration INT NULL,
                                                  Date DATETIME NULL)");
                Execute(@"CREATE TABLE WatchHistory (Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
                                                     UserId UNIQUEIDENTIFIER NOT NULL,
                                                     AssetId NVARCHAR(MAX) NOT NULL,
                                                     AssetType INT NOT NULL,
                                                     Duration INT NULL,
                                                     Date DATETIME NULL)");
            }

            /// <inheritdoc/>
            protected override void Down()
            {
                Execute("DROP TABLE Favorites");
                Execute("DROP TABLE Watchlist");
                Execute("DROP TABLE WatchHistory");
            }
        }

        /// <inheritdoc/>
        [Migration(4, "version 1.3")]
        public class CreateDatabaseV4 : Migration
        {
            /// <inheritdoc/>
            protected override void Up()
            {
                Execute(@"ALTER TABLE dbo.Users ADD FacebookUserId NVARCHAR(MAX) NULL");
                Execute(@"ALTER TABLE dbo.Users ADD GoogleUserId NVARCHAR(MAX) NULL");
                Execute(@"ALTER TABLE dbo.Users ADD TwitterUserId NVARCHAR(MAX) NULL");
            }

            /// <inheritdoc/>
            protected override void Down()
            {
                Execute(@"ALTER TABLE dbo.Users DROP COLUMN FacebookUserId");
                Execute(@"ALTER TABLE dbo.Users DROP COLUMN GoogleUserId");
                Execute(@"ALTER TABLE dbo.Users DROP COLUMN TwitterUserId");
            }
        }

        /// <inheritdoc/>
        [Migration(5, "version 1.4")]
        public class CreateDatabaseV5 : Migration
        {
            /// <inheritdoc/>
            protected override void Up()
            {
                Execute(@"CREATE TABLE Settings (Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
                                                 UserId UNIQUEIDENTIFIER NOT NULL,
                                                 SettingKey NVARCHAR(MAX) NOT NULL,
                                                 SettingValue NVARCHAR(MAX) NOT NULL)");
            }

            /// <inheritdoc/>
            protected override void Down()
            {
                Execute("DROP TABLE Settings");
           }
        }

        /// <inheritdoc/>
        [Migration(6, "version 1.5")]
        public class CreateDatabaseV6 : Migration
        {
            /// <inheritdoc/>
            protected override void Up()
            {
                Execute(@"ALTER TABLE dbo.Users ADD B2BUserId NVARCHAR(MAX) NULL");
                Execute(@"ALTER TABLE dbo.Users ADD CreationDate DATETIME NULL");
            }

            /// <inheritdoc/>
            protected override void Down()
            {
                Execute(@"ALTER TABLE dbo.Users DROP COLUMN B2BUserId");
                Execute(@"ALTER TABLE dbo.Users DROP COLUMN CreationDate");
            }
        }

        /// <inheritdoc/>
        [Migration(7, "version 1.6")]
        public class CreateDatabaseV7 : Migration
        {
            /// <inheritdoc/>
            protected override void Up()
            {
                Execute(@"CREATE TABLE Reminders (Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
                                                  UserId UNIQUEIDENTIFIER NOT NULL,
                                                  AssetId NVARCHAR(MAX) NOT NULL,
                                                  AssetType INT NOT NULL,
                                                  ReminderType INT NOT NULL)");
            }

            /// <inheritdoc/>
            protected override void Down()
            {
                Execute("DROP TABLE Reminders");
            }
        }
        
        /// <inheritdoc/>
        [Migration(8, "version 1.7")]
        public class CreateDatabaseV8 : Migration
        {
            /// <inheritdoc/>
            protected override void Up()
            {
                Execute(@"ALTER TABLE dbo.Users ADD PasswordSalt NVARCHAR(48) NOT NULL DEFAULT 'not_set'");
            }

            /// <inheritdoc/>
            protected override void Down()
            {
                Execute(@"ALTER TABLE dbo.Users DROP COLUMN PasswordSalt");
            }
        }

        /// <inheritdoc/>
        [Migration(9, "version 1.8")]
        public class CreateDatabaseV9 : Migration
        {
            /// <inheritdoc/>
            protected override void Up()
            {
                Execute(@"ALTER TABLE dbo.Users ADD SystemStr NVARCHAR(48) NOT NULL");
                Execute(@"ALTER TABLE dbo.Users DROP COLUMN System");
                Execute(@"EXEC sp_rename 'dbo.Users.SystemStr', 'System', 'COLUMN'");
            }

            /// <inheritdoc/>
            protected override void Down()
            {
                Execute(@"EXEC sp_rename 'dbo.Users.System', 'SystemStr', 'COLUMN'");
                Execute(@"ALTER TABLE dbo.Users ADD System int NULL");
                Execute(@"ALTER TABLE dbo.Users DROP COLUMN SystemStr");
            }
        }

        /// <inheritdoc/>
        [Migration(10, "version 1.9")]
        public class CreateDatabaseV10 : Migration
        {
            /// <inheritdoc/>
            protected override void Up()
            {
                Execute(@"ALTER TABLE Users ALTER COLUMN Email NVARCHAR(320) NULL");
                Execute(@"ALTER TABLE Users ALTER COLUMN Mobile NVARCHAR(32) NULL");
                Execute(@"ALTER TABLE Users ALTER COLUMN FacebookUserId NVARCHAR(64) NULL");
                Execute(@"ALTER TABLE Users ALTER COLUMN GoogleUserId NVARCHAR(64) NULL");
                Execute(@"ALTER TABLE Users ALTER COLUMN TwitterUserId NVARCHAR(64) NULL");
                Execute(@"ALTER TABLE Users ALTER COLUMN B2BUserId NVARCHAR(128) NULL");

                Execute(@"CREATE NONCLUSTERED INDEX Users_Email ON dbo.Users (Email)");
                Execute(@"CREATE NONCLUSTERED INDEX Users_Mobile ON dbo.Users (Mobile)");
                Execute(@"CREATE NONCLUSTERED INDEX Users_FacebookUserId ON dbo.Users (FacebookUserId)");
                Execute(@"CREATE NONCLUSTERED INDEX Users_GoogleUserId ON dbo.Users (GoogleUserId)");
                Execute(@"CREATE NONCLUSTERED INDEX Users_TwitterUserId ON dbo.Users (TwitterUserId)");
                Execute(@"CREATE NONCLUSTERED INDEX Users_EmailConfirmationKey ON dbo.Users (EmailConfirmationKey)");
                Execute(@"CREATE NONCLUSTERED INDEX Users_MobileConfirmationKey ON dbo.Users (MobileConfirmationKey)");
                Execute(@"CREATE NONCLUSTERED INDEX Users_PasswordResetKey ON dbo.Users (PasswordResetKey)");
                Execute(@"CREATE NONCLUSTERED INDEX Users_B2BUserId ON dbo.Users (B2BUserId)");
            }

            /// <inheritdoc/>
            protected override void Down()
            {
                Execute(@"DROP INDEX Users_Email ON dbo.Users");
                Execute(@"DROP INDEX Users_Mobile ON dbo.Users");
                Execute(@"DROP INDEX Users_FacebookUserId ON dbo.Users");
                Execute(@"DROP INDEX Users_GoogleUserId ON dbo.Users");
                Execute(@"DROP INDEX Users_TwitterUserId ON dbo.Users");
                Execute(@"DROP INDEX Users_EmailConfirmationKey ON dbo.Users");
                Execute(@"DROP INDEX Users_MobileConfirmationKey ON dbo.Users");
                Execute(@"DROP INDEX Users_PasswordResetKey ON dbo.Users");
                Execute(@"DROP INDEX Users_B2BUserId ON dbo.Users");
            }
        }

        /// <inheritdoc/>
        [Migration(11, "version 2.0")]
        public class CreateDatabaseV11 : Migration
        {
            /// <inheritdoc/>
            protected override void Up()
            {
                Execute(@"ALTER TABLE Reminders ALTER COLUMN AssetId NVARCHAR(256) NOT NULL");

                // Remove index from primary key
                Execute(@"IF EXISTS(SELECT* FROM sys.objects WHERE type = 'P' AND name = 'RemovePrimaryIndex') DROP PROCEDURE RemovePrimaryIndex;");
                Execute(@"
                    CREATE PROCEDURE RemovePrimaryIndex
                        @TableName nvarchar(50)
                    AS 
                        DECLARE @Command VARCHAR(200);
                        SELECT @Command = 'ALTER TABLE dbo.' + @TableName + ' DROP CONSTRAINT ' + QUOTENAME(I.name)
                            FROM sys.indexes AS I INNER JOIN sys.tables AS T ON I.[object_id] = T.[object_id] WHERE T.name = @TableName AND I.is_primary_key = 1;
                        execute(@command);
                ");

                Execute(@"EXECUTE RemovePrimaryIndex 'Settings'");
                Execute(@"ALTER TABLE dbo.Settings ADD CONSTRAINT PK_Settings PRIMARY KEY NONCLUSTERED (Id)");
                Execute(@"CREATE CLUSTERED INDEX Settings_UserId ON dbo.Settings (UserId)");

                Execute(@"EXECUTE RemovePrimaryIndex 'Reminders'");
                Execute(@"ALTER TABLE dbo.Reminders ADD CONSTRAINT PK_Reminders PRIMARY KEY NONCLUSTERED (Id)");
                Execute(@"CREATE CLUSTERED INDEX Reminders_UserId ON dbo.Reminders (UserId)");
                Execute(@"CREATE NONCLUSTERED INDEX Reminders_AssetId ON dbo.Reminders (AssetId)");
                
                Execute(@"EXECUTE RemovePrimaryIndex 'Watchlist'");
                Execute(@"ALTER TABLE dbo.Watchlist ADD CONSTRAINT PK_Watchlist PRIMARY KEY NONCLUSTERED (Id)");
                Execute(@"CREATE CLUSTERED INDEX Watchlist_UserId ON dbo.Watchlist (UserId)");

                Execute(@"EXECUTE RemovePrimaryIndex 'WatchHistory'");
                Execute(@"ALTER TABLE dbo.WatchHistory ADD CONSTRAINT PK_WatchHistory PRIMARY KEY NONCLUSTERED (Id)");
                Execute(@"CREATE CLUSTERED INDEX WatchHistory_UserId ON dbo.WatchHistory (UserId)");

                Execute(@"EXECUTE RemovePrimaryIndex 'Favorites'");
                Execute(@"ALTER TABLE dbo.Favorites ADD CONSTRAINT PK_Favorites PRIMARY KEY NONCLUSTERED(Id)");
                Execute(@"CREATE CLUSTERED INDEX Favorites_UserId ON dbo.Favorites(UserId)");
            }

            /// <inheritdoc/>
            protected override void Down()
            {
                // No need to revert sproc, varchar lengths or primary indexes
                Execute(@"DROP INDEX Settings_UserId ON dbo.Settings");
                Execute(@"DROP INDEX Reminders_UserId ON dbo.Reminders");
                Execute(@"DROP INDEX Reminders_AssetId ON dbo.Reminders");
                Execute(@"DROP INDEX Watchlist_UserId ON dbo.Watchlist");
                Execute(@"DROP INDEX WatchHistory_UserId ON dbo.WatchHistory");
                Execute(@"DROP INDEX Favorites_UserId ON dbo.Favorites");
            }
            
            /// <inheritdoc/>
            [Migration(12, "version 2.1")]
            public class CreateDatabaseV12 : Migration
            {
                /// <inheritdoc/>
                protected override void Up()
                {
                    // Remove default constraint
                    Execute(@"IF EXISTS(SELECT* FROM sys.objects WHERE type = 'P' AND name = 'RemoveDefault') DROP PROCEDURE RemoveDefault;");
                    Execute(@"
                        CREATE PROCEDURE RemoveDefault
                            @TableName nvarchar(50),
                            @ColumnName nvarchar(50)
                        AS 
                            DECLARE @Command VARCHAR(200);
                            SELECT @Command = 'ALTER TABLE dbo.' + @TableName + ' DROP CONSTRAINT ' + QUOTENAME(I.name)
                                FROM sys.default_constraints AS I INNER JOIN sys.tables AS T ON I.[parent_object_id] = T.[object_id] 
                                INNER JOIN sys.columns AS C ON T.[object_id] = C.[object_id] AND I.[parent_column_id] = C.[column_id] WHERE T.name = @TableName AND C.name = @ColumnName
                            execute(@command);
                    ");
                    Execute(@"EXEC sp_rename 'dbo.Users.Password', 'PasswordHash', 'COLUMN'");
                    Execute(@"ALTER TABLE dbo.Users ALTER COLUMN PasswordHash NVARCHAR(60) NOT NULL");    // to accommodate the bcrypt hash
                    Execute(@"EXECUTE RemoveDefault 'Users', 'PasswordSalt'");
                    Execute(@"ALTER TABLE dbo.Users DROP COLUMN PasswordSalt");
                }

                /// <inheritdoc/>
                protected override void Down()
                {
                    Execute(@"EXEC sp_rename 'dbo.Users.PasswordHash', 'Password', 'COLUMN'");
                    Execute(@"ALTER TABLE dbo.Users ALTER COLUMN Password NVARCHAR(48) NOT NULL");
                    Execute(@"ALTER TABLE dbo.Users ADD PasswordSalt NVARCHAR(48) NOT NULL DEFAULT 'not_set'");
                }
            }

            /// <inheritdoc/>
            [Migration(13, "version 2.2")]
            public class CreateDatabaseV13 : Migration
            {
                /// <inheritdoc/>
                protected override void Up()
                {
                    Execute(@"DROP INDEX Reminders_AssetId ON dbo.Reminders");
                    Execute(@"ALTER TABLE Reminders ALTER COLUMN AssetId NVARCHAR(100) NOT NULL");
                    Execute(@"CREATE NONCLUSTERED INDEX Reminders_AssetId ON dbo.Reminders (AssetId)");

                    Execute(@"DROP INDEX Users_Mobile ON dbo.Users");
                    Execute(@"ALTER TABLE Users ALTER COLUMN Mobile NVARCHAR(64) NULL");
                    Execute(@"CREATE NONCLUSTERED INDEX Users_Mobile ON dbo.Users (Mobile)");
                }

                /// <inheritdoc/>
                protected override void Down()
                {
                    // No need to revert sproc, varchar lengths or primary indexes
                }
            }

            /// <inheritdoc/>
            [Migration(14, "version 2.3")]
            public class CreateDatabaseV14 : Migration
            {
                /// <inheritdoc/>
                protected override void Up()
                {
                    Execute(@"ALTER TABLE dbo.Users ADD NewEmail NVARCHAR(320) NULL");
                    Execute(@"ALTER TABLE dbo.Users ADD NewEmailConfirmationKey NVARCHAR(48) NULL");
                    Execute(@"ALTER TABLE dbo.Users ADD NewEmailConfirmationExpiration DATETIME NULL");
                    Execute(@"CREATE NONCLUSTERED INDEX Users_NewEmailConfirmationKey ON dbo.Users (NewEmailConfirmationKey)");
                }

                /// <inheritdoc/>
                protected override void Down()
                {
                    Execute(@"DROP INDEX Users_NewEmailConfirmationKey ON dbo.Users");
                    Execute(@"ALTER TABLE dbo.Users DROP COLUMN NewEmailConfirmationExpiration");
                    Execute(@"ALTER TABLE dbo.Users DROP COLUMN NewEmailConfirmationKey");
                    Execute(@"ALTER TABLE dbo.Users DROP COLUMN NewEmail");
                }
            }

            /// <inheritdoc/>
            [Migration(15, "version 2.4")]
            public class CreateDatabaseV15 : Migration
            {
                /// <inheritdoc/>
                protected override void Up()
                {
                    Execute(@"ALTER TABLE dbo.Users ADD State INT NULL");
                    Execute(@"UPDATE dbo.Users SET State = CASE 
                                WHEN PasswordHash = '' THEN 99 
                                WHEN Activated = 1 THEN 10
                                ELSE 1 END", int.MaxValue);
                    Execute(@"ALTER TABLE dbo.Users ALTER COLUMN State INT NOT NULL");
                    //Execute(@"CREATE NONCLUSTERED INDEX IN_Users_State ON dbo.Users (State)");

                    Execute(@"ALTER TABLE dbo.Users DROP COLUMN Activated");
                }

                /// <inheritdoc/>
                protected override void Down()
                {
                    Execute(@"ALTER TABLE dbo.Users ADD Activated bit NULL");
                    Execute(@"UPDATE dbo.Users SET Activated = CASE WHEN State = 1 THEN 0 ELSE 1 END", int.MaxValue);

                    //Execute(@"DROP INDEX IN_Users_State ON dbo.Users;");
                    Execute(@"ALTER TABLE dbo.Users DROP COLUMN State");
                }
            }

            /// <inheritdoc/>
            [Migration(16, "version 2.5")]
            public class CreateDatabaseV16 : Migration
            {
                /// <inheritdoc/>
                protected override void Up()
                {
                    Execute(@"IF EXISTS(SELECT * FROM sys.indexes WHERE name = 'IN_Users_State' AND object_id = OBJECT_ID('dbo.Users')) DROP INDEX IN_Users_State ON dbo.Users;", int.MaxValue);
                    Execute(@"CREATE NONCLUSTERED INDEX IN_Users_State ON dbo.Users (State)", int.MaxValue);

                    // Automatic tuning cannot be enabled without elevated privelages. Must be applied manually
                    // Execute(@"ALTER DATABASE current SET AUTOMATIC_TUNING (FORCE_LAST_GOOD_PLAN = ON, CREATE_INDEX = ON, DROP_INDEX = ON)");
                }

                /// <inheritdoc/>
                protected override void Down()
                {
                    Execute(@"DROP INDEX IN_Users_State ON dbo.Users;");
                }
            }

            /// <inheritdoc/>
            [Migration(17, "version 2.6")]
            public class CreateDatabaseV17 : Migration
            {
                /// <inheritdoc/>
                protected override void Up()
                {
                    // Make MobileConfirmationKey unique
                    Execute(@"UPDATE Users SET MobileConfirmationKey = NULL WHERE MobileConfirmationKey = ''", int.MaxValue);
                    Execute(@"DROP INDEX Users_MobileConfirmationKey ON dbo.Users", int.MaxValue);
                    Execute(@"CREATE UNIQUE NONCLUSTERED INDEX UN_Users_MobileConfirmationKey ON dbo.Users(MobileConfirmationKey) WHERE MobileConfirmationKey IS NOT NULL;", int.MaxValue);

                    // Create index on MobileConfirmationExpiry
                    Execute(@"CREATE NONCLUSTERED INDEX IN_Users_MobileConfirmationExpiration ON dbo.Users (MobileConfirmationExpiration)", int.MaxValue);
                }

                /// <inheritdoc/>
                protected override void Down()
                {
                    Execute(@"DROP INDEX UN_Users_MobileConfirmationKey ON dbo.Users");
                    Execute(@"CREATE NONCLUSTERED INDEX Users_MobileConfirmationKey ON dbo.Users(MobileConfirmationKey)");
                    Execute(@"DROP INDEX IN_Users_MobileConfirmationExpiration ON dbo.Users");
                }
            }

            /// <inheritdoc/>
            [Migration(18, "version 2.7")]
            public class CreateDatabaseV18 : Migration
            {
                /// <inheritdoc/>
                protected override void Up()
                {
                    // Make MobileConfirmationKey unique
                    Execute(@"UPDATE Users SET PasswordResetKey = NULL WHERE PasswordResetKey = '' OR (PasswordResetKey IS NOT NULL AND PasswordResetExpiration < GETUTCDATE())", int.MaxValue);
                    Execute(@"DROP INDEX Users_PasswordResetKey ON dbo.Users", int.MaxValue);
                    Execute(@"CREATE UNIQUE NONCLUSTERED INDEX UN_Users_PasswordResetKey ON dbo.Users(PasswordResetKey) WHERE PasswordResetKey IS NOT NULL;", int.MaxValue);
                }

                /// <inheritdoc/>
                protected override void Down()
                {
                    Execute(@"DROP INDEX UN_Users_PasswordResetKey ON dbo.Users");
                    Execute(@"CREATE NONCLUSTERED INDEX Users_PasswordResetKey ON dbo.Users(PasswordResetKey)");
                }
            }

            /// <inheritdoc/>
            [Migration(19, "version 2.8")]
            public class CreateDatabaseV19 : Migration
            {
                /// <inheritdoc/>
                protected override void Up()
                {
                    // Make MobileConfirmationKey unique
                    Execute(@"CREATE TABLE MobileOtpCodes (Code NVARCHAR(4) NOT NULL PRIMARY KEY)");
                    Execute(@"DECLARE @Iteration Integer = 0;
                              WHILE @Iteration <10000 
                              BEGIN  
                              insert into MobileOtpCodes VALUES (RIGHT(CAST(10000 + @Iteration AS NVARCHAR), 4));
                              SET @Iteration += 1;  
                              END;");
                }

                /// <inheritdoc/>
                protected override void Down()
                {
                    Execute(@"DROP TABLE MobileOtpCodes");
                }
            }


            /// <inheritdoc/>
            [Migration(20, "version 2.9")]
            public class CreateDatabaseV20 : Migration
            {
                /// <inheritdoc/>
                protected override void Up()
                {
                    Execute(@"ALTER TABLE dbo.Settings ADD LastUpdate DATETIME NULL");
                    Execute(@"ALTER TABLE dbo.Settings ADD LastMigration DATETIME NULL");
                }

                /// <inheritdoc/>
                protected override void Down()
                {
                    Execute(@"ALTER TABLE dbo.Settings DROP COLUMN LastUpdate");
                    Execute(@"ALTER TABLE dbo.Settings DROP COLUMN LastMigration");
                }
            }

            /// <inheritdoc/>
            [Migration(21, "version 3.0")]
            public class CreateDatabaseV21 : Migration
            {
                /// <inheritdoc/>
                protected override void Up()
                {
                    Execute(@" CREATE TABLE OneTimePasses (
                                                    UserId UNIQUEIDENTIFIER NOT NULL,
                                                    OtpGroup INT NOT NULL,
                                                    Code NVARCHAR(48) NOT NULL,
                                                    RecipientAddress NVARCHAR(320) NOT NULL,
                                                    Expires DATETIME NOT NULL) ");

                    Execute(@" CREATE UNIQUE CLUSTERED INDEX UN_ConfirmationKeys_Code_OtpGroup ON dbo.OneTimePasses(Code, OtpGroup) ", int.MaxValue);
                    Execute(@" CREATE NONCLUSTERED INDEX IN_OtpGroup ON dbo.OneTimePasses(OtpGroup) ", int.MaxValue);
                    Execute(@" CREATE NONCLUSTERED INDEX IN_UserId ON dbo.OneTimePasses(UserId) ", int.MaxValue);
                    Execute(@" CREATE NONCLUSTERED INDEX IN_Expires ON dbo.OneTimePasses(Expires) ", int.MaxValue);

                    Execute(@" INSERT INTO OneTimePasses (UserId, OtpGroup, Code, RecipientAddress, Expires)
                               SELECT
                                    Id, 
                                    IIF(Mobile IS NULL, 1, 2),  
                                    PasswordResetKey,
                                    IIF(Mobile IS NULL, Email, Mobile),
                                    PasswordResetExpiration
                                FROM Users 
                                WHERE PasswordResetKey IS NOT NULL AND PasswordResetExpiration > GETUTCDATE() ", int.MaxValue);
                }

                /// <inheritdoc/>
                protected override void Down()
                {
                    // not defined
                }
            }

            /// <inheritdoc/>
            [Migration(22, "Empty migration")]
            public class Migration22 : Migration
            {
                /// <inheritdoc/>
                protected override void Up()
                {
                    // not defined
                }

                /// <inheritdoc/>
                protected override void Down()
                {
                    // not defined
                }
            }

            /// <inheritdoc/>
            [Migration(23, "version 3.1")]
            public class CreateDatabaseV23 : Migration
            {
                /// <inheritdoc/>
                protected override void Up()
                {
                    Execute(@"ALTER TABLE dbo.Users ADD IpAddress NVARCHAR(50) NULL");
                    Execute(@"ALTER TABLE dbo.Users ADD RegistrationRegion NVARCHAR(50) NULL");
                    Execute(@"ALTER TABLE dbo.Users ADD Json NVARCHAR(MAX) NULL");
                }

                /// <inheritdoc/>
                protected override void Down()
                {
                    Execute(@"ALTER TABLE dbo.Users DROP COLUMN IpAddress");
                    Execute(@"ALTER TABLE dbo.Users DROP COLUMN RegistrationRegion");
                    Execute(@"ALTER TABLE dbo.Users DROP COLUMN Json");
                }
            }
            
            /// <inheritdoc/>
            [Migration(24, "version 3.2")]
            public class CreateDatabaseV24 : Migration
            {
                /// <inheritdoc/>
                protected override void Up()
                {
                    Execute(@"ALTER TABLE dbo.Users ADD AmazonUserId NVARCHAR(64) NULL");
                }

                /// <inheritdoc/>
                protected override void Down()
                {
                    Execute(@"ALTER TABLE dbo.Users DROP COLUMN AmazonUserId");
                }
            }
            
            /// <inheritdoc/>
            [Migration(25, "version 3.3")]
            public class CreateDatabaseV25 : Migration
            {
                /// <inheritdoc/>
                protected override void Up()
                {
                    Execute(@"ALTER TABLE dbo.Users ADD ProviderName NVARCHAR(64) NULL");
                    Execute(@"ALTER TABLE dbo.Users ADD ProviderSubjectId NVARCHAR(128) NULL");
                }

                /// <inheritdoc/>
                protected override void Down()
                {
                    Execute(@"ALTER TABLE dbo.Users DROP COLUMN ProviderName");
                    Execute(@"ALTER TABLE dbo.Users DROP COLUMN ProviderSubjectId");
                }
            }
        }
    }
}