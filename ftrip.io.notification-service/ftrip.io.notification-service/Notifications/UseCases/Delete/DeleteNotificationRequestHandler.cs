using ftrip.io.notification_service.Notifications.Domain;
using MediatR;
using Serilog;
using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.notification_service.Notifications.UseCases.Delete
{
    public class DeleteNotificationRequestHandler : IRequestHandler<DeleteNotificationRequest, Notification>
    {
        private readonly INotificationsQueryHelper _notificationsQueryHelper;
        private readonly INotificationRepository _notificationRepository;
        private readonly ILogger _logger;

        public DeleteNotificationRequestHandler(
            INotificationsQueryHelper notificationsQueryHelper,
            INotificationRepository notificationRepository,
            ILogger logger)
        {
            _notificationsQueryHelper = notificationsQueryHelper;
            _notificationRepository = notificationRepository;
            _logger = logger;
        }

        public async Task<Notification> Handle(DeleteNotificationRequest request, CancellationToken cancellationToken)
        {
            var notification = await _notificationsQueryHelper.ReadOrThrow(request.NotificationId, cancellationToken);

            var deletedNotification = await _notificationRepository.Delete(notification.Id, cancellationToken);

            _logger.Information("Notification deleted - NotificationId[{NotificationId}]", deletedNotification.Id);

            return deletedNotification;
        }
    }
}