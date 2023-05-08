using ftrip.io.framework.Installers;
using ftrip.io.notification_service.NotificationTypes;
using ftrip.io.notification_service.UserNotificationTypes;
using Microsoft.Extensions.DependencyInjection;

namespace ftrip.io.notification_service.Installers
{
    public class DependenciesIntaller : IInstaller
    {
        private readonly IServiceCollection _services;

        public DependenciesIntaller(IServiceCollection services)
        {
            _services = services;
        }

        public void Install()
        {
            _services.AddScoped<INotificationTypeRepository, NotificationTypeRepository>();
            _services.AddScoped<IUserNotificationTypeRepository, UserNotificationTypeRepository>();
        }
    }
}