using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ftrip.io.notification_service.NotificationTypes.Seeder
{
    public static class NotificationTypesSeederExtensions
    {
        public static IHost SeedNotificationTypes(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<INotificationTypeRepository>>();
                var repository = services.GetService<INotificationTypeRepository>();
                var seeder = new NotificationTypesSeeder(repository);

                if (seeder.ShouldSeed().Result)
                {
                    logger.LogInformation($"Seeding notification types.");
                    seeder.Seed().Wait();
                }
                else
                {
                    logger.LogInformation($"Skipped seeding notification types.");
                }
            }

            return host;
        }
    }
}