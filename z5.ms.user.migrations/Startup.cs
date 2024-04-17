using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using z5.ms.common.infrastructure.db.migrations;

namespace z5.ms.user.migrations
{
    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MsDatabaseConnection");
            Console.WriteLine($"Using connection string: {connectionString}");

            services.AddSingleton(configuration);
            services.AddSingleton<DbConnection>(_ => new SqlConnection(connectionString));
            services.AddSingleton<IMigrationService, MigrationService>();
        }

        public static void ConfigureCommands(CommandLineApplication app)
        {
            var commandOption = app.Option<string>("-c|--command <PATH>", "A command to run: database|version (required)", CommandOptionType.SingleValue, x => x.IsRequired());
            var versionOption = app.Option<string>("-v|--version <VERSION>", "Database version to migrate to: latest|{n}. If omitted the version will be read from migrationsettings.json (SubscriptionsDatabaseVersion)", CommandOptionType.SingleValue);

            app.OnExecute(() =>
            {
                switch (commandOption.Value())
                {
                    case "database":
                        return MigrateDatabase(app, versionOption.Value());

                    case "version":
                        Console.WriteLine(Assembly.GetEntryAssembly().GetName().Version);
                        return 0;
                        
                    default:
                        app.Error.WriteLine($"Error: Unsupported action: {commandOption}");
                        return 1;
                }
            });
        }

        private static int MigrateDatabase(CommandLineApplication app, string version)
        {
            var migrationService = app.GetService<IMigrationService>();
            var configuration = app.GetService<IConfiguration>();

            if (version == "latest")
            {
                Console.WriteLine("Migrating to latest");
                migrationService.MigrateToLatest();
                return 0;
            }

            var versionNum = !string.IsNullOrEmpty(version)
                ? Convert.ToInt64(version)
                : configuration.GetValue<long>("UsersDatabaseVersion");

            if (versionNum == 0)
            {
                // TODO: Remove this code and return error if config is missing
                // migrate to latest if config missing
                Console.WriteLine("Warning: Target database version is not defined in configuration or command line arguments. Assume old codebase and exit without migration.");
                return 0;

                app.Error.WriteLine("Error: Target database version is not defined in configuration or command line arguments");
                return 1;
            }

            Console.WriteLine($"Migrating to v{versionNum}");
            migrationService.MigrateToVersion(versionNum);
            return 0;
        }
    }
}