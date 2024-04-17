using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;

namespace z5.ms.userservice
{
    /// <summary>Application entry</summary>
    public class Program
    {
        /// <summary>
        /// Application entry point.
        /// Starts HTTP listener with managed MVC pipeline.
        /// </summary>
        /// <param name="args">startup parameters (ignored)</param>
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        /// <summary>Build web host</summary>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(opts =>
                {
                    opts.Limits.MinRequestBodyDataRate = null;
                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                //.UseHealthChecks("/hc")
                .Build();
    }
}
