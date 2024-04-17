using System;
using System.Diagnostics;
using System.IO;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace z5.ms.user.migrations
{
    public class Program
    {
        static void Main(string[] args)
        {
            // configure commands and parse args
            var app = new CommandLineApplication();
            var appSettings = app.Option("-a|--appsettings <PATH>", "Path to appsettings json file (required)", CommandOptionType.SingleValue, x => x.IsRequired());
            var migrationSettings = app.Option("-m|--migrationsettings <PATH>", "Path to migrationsettings json file (required)", CommandOptionType.SingleValue, x => x.IsRequired());
            Startup.ConfigureCommands(app);
            app.HelpOption(true);
            app.Parse(args);

            // load config files
            Console.WriteLine($"Current dir: {Directory.GetCurrentDirectory()}");
            Console.WriteLine($"Using appsettings file: {Path.GetFullPath(Path.Combine(appSettings.Value()))}");
            Console.WriteLine($"Using migrationsettings file: {Path.GetFullPath(Path.Combine(migrationSettings.Value()))}");
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(Path.GetFullPath(Path.Combine(appSettings.Value())), false)
                .AddJsonFile(Path.GetFullPath(Path.Combine(migrationSettings.Value())), false) // TODO: Make non optional
                .Build();

            // configure di
            var services = new ServiceCollection();
            services.AddLogging(cfg => { cfg.AddConsole().AddDebug(); });
            Startup.ConfigureServices(services, configuration);
            
            // configure & run app
            app.Conventions
                .UseDefaultConventions()
                .SetAppNameFromEntryAssembly()
                .UseConstructorInjection(services.BuildServiceProvider());
            app.Execute();

            // pause if debugging
            if (Debugger.IsAttached) Console.ReadLine();
        }
    }
}
