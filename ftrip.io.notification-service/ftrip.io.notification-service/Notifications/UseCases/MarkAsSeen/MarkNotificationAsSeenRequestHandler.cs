using ftrip.io.notification_service.Notifications.Domain;
using MediatR;
using Serilog;
using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.notification_service.Notifications.UseCases.MarkAsSeen
{
    public class MarkNotificationAsSeenRequestHandler : IRequestHandler<MarkNotificationAsSeenRequest, Notification>
    {
        private readonly INotificationsQueryHelper _notificationsQueryHelper;
        private readonly INotificationRepository _notificationRepository;
        private readonly ILogger _logger;

        public MarkNotificationAsSeenRequestHandler(
            INotificationsQueryHelper notificationsQueryHelper,
            INotificationRepository notificationRepository,
            ILogger logger)
        {
            _notificationsQueryHelper = notificationsQueryHelper;
            _notificationRepository = notificationRepository;
            _logger = logger;
        }

        public async Task<Notification> Handle(MarkNotificationAsSeenRequest request, CancellationToken cancellationToken)
        {
            var notification = await _notificationsQueryHelper.ReadOrThrow(request.NotificationId, cancellationToken);

            notification.Seen = true;

            var seenNotification = await _notificationRepository.Update(notification, cancellationToken);

            _logger.Information("Notification seen - NotificationId[{NotificationId}]", seenNotification.Id);

            return seenNotification;
        }
    }
}