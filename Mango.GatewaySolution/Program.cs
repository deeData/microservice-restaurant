using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.GatewaySolution
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                   .UseKestrel()
                   .UseContentRoot(Directory.GetCurrentDirectory())
                   .ConfigureAppConfiguration((hostingContext, config) =>
                   {

                       config
                           .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                           .AddJsonFile("appsettings.json", true, true)
                           .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                           .AddEnvironmentVariables();
                   })
                   .ConfigureLogging((hostingContext, logging) =>
                   {
                       logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                       logging.AddConsole();
                       logging.AddDebug();
                       logging.AddEventSourceLogger();
                   })
                   .UseIISIntegration();
                    webBuilder.UseStartup<Startup>();
                });

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //    .ConfigureWebHostDefaults(webBuilder =>
        //    {
        //        webBuilder.UseStartup<Startup>();
        //    });

    }
}
