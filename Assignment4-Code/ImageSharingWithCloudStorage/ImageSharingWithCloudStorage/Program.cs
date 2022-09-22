using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageSharingWithCloudStorage.DAL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ImageSharingWithCloudStorage
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;

                var db = serviceProvider.GetRequiredService<ApplicationDbContext>();

                var logger = serviceProvider.GetRequiredService<ILogger<ApplicationDbInitializer>>();

                var logs = serviceProvider.GetRequiredService<ILogContext>();

                await new ApplicationDbInitializer(db, logs, logger).SeedDatabase(serviceProvider);
            }

            await host.RunAsync();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddAzureWebAppDiagnostics();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureLogging((ctx, logging) =>
                    {
                        logging.AddEventLog(options =>
                        {
                            options.SourceName = "MyApp";
                        });
                    });
                });
    }
}
