using ftrip.io.notification_service.Notifications.Domain;
using MediatR;
using Serilog;
using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.notification_service.Notifications.UseCases.MarkAsUnseen
{
    public class MarkNotificationAsUnseenRequestHandler : IRequestHandler<MarkNotificationAsUnseenRequest, Notification>
    {
        private readonly INotificationsQueryHelper _notificationsQueryHelper;
        private readonly INotificationRepository _notificationRepository;
        private readonly ILogger _logger;

        public MarkNotificationAsUnseenRequestHandler(
            INotificationsQueryHelper notificationsQueryHelper,
            INotificationRepository notificationRepository,
            ILogger logger)
        {
            _notificationsQueryHelper = notificationsQueryHelper;
            _notificationRepository = notificationRepository;
            _logger = logger;
        }

        public async Task<Notification> Handle(MarkNotificationAsUnseenRequest request, CancellationToken cancellationToken)
        {
            var notification = await _notificationsQueryHelper.ReadOrThrow(request.NotificationId, cancellationToken);

            notification.Seen = false;

            var unseenNotification = await _notificationRepository.Update(notification, cancellationToken);

            _logger.Information("Notification unseen - NotificationId[{NotificationId}]", unseenNotification.Id);

            return unseenNotification;
        }
    }
}