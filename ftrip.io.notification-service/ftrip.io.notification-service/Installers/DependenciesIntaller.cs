using ftrip.io.framework.Installers;
using ftrip.io.framework.Proxies;
using ftrip.io.notification_service.Notifications;
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
            _services.AddProxiedScoped<INotificationTypeRepository, NotificationTypeRepository>();
            _services.AddProxiedScoped<IUserNotificationTypeRepository, UserNotificationTypeRepository>();
            _services.AddProxiedScoped<INotificationRepository, NotificationRepository>();
            _services.AddProxiedScoped<INotificationsQueryHelper, NotificationsQueryHelper>();
            _services.AddProxiedScoped<IUserNotificationTypeEligibilityHelper, UserNotificationTypeEligibilityHelper>();
        }
    }
}