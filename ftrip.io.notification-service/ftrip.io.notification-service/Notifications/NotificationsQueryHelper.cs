using ftrip.io.framework.ExceptionHandling.Exceptions;
using ftrip.io.framework.Globalization;
using ftrip.io.notification_service.Notifications.Domain;
using Serilog;
using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.notification_service.Notifications
{
    public interface INotificationsQueryHelper
    {
        Task<Notification> ReadOrThrow(string id, CancellationToken cancellationToken = default);
    }

    public class NotificationsQueryHelper : INotificationsQueryHelper
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IStringManager _stringManager;
        private readonly ILogger _logger;

        public NotificationsQueryHelper(
            INotificationRepository notificationRepository,
            IStringManager stringManager,
            ILogger logger)
        {
            _notificationRepository = notificationRepository;
            _stringManager = stringManager;
            _logger = logger;
        }

        public async Task<Notification> ReadOrThrow(string id, CancellationToken cancellationToken = default)
        {
            var notification = await _notificationRepository.Read(id, cancellationToken);
            if (notification == null)
            {
                _logger.Error("Notification not found - NotificationId[{NotificationId}]", id);
                throw new MissingEntityException(_stringManager.Format("Common_MissingEntity", id));
            }

            return notification;
        }
    }
}