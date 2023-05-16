using ftrip.io.framework.Logging;
using ftrip.io.notification_service.NotificationTypes.Seeder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace ftrip.io.notification_service
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .SeedNotificationTypes()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilogLogging((hostingContext) =>
                {
                    return new LoggingOptions()
                    {
                        ApplicationName = hostingContext.HostingEnvironment.ApplicationName,
                        ApplicationLabel = "notifications",
                        ClientIdAttribute = "X-Forwarded-For",
                        GrafanaLokiUrl = Environment.GetEnvironmentVariable("GRAFANA_LOKI_URL")
                    };
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}