using ftrip.io.notification_service.Notifications.Domain;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.notification_service.Notifications.UseCases.MarkAsSeen
{
    public class MarkNotificationAsSeenRequestHandler : IRequestHandler<MarkNotificationAsSeenRequest, Notification>
    {
        private readonly INotificationsQueryHelper _notificationsQueryHelper;
        private readonly INotificationRepository _notificationRepository;

        public MarkNotificationAsSeenRequestHandler(
            INotificationsQueryHelper notificationsQueryHelper,
            INotificationRepository notificationRepository)
        {
            _notificationsQueryHelper = notificationsQueryHelper;
            _notificationRepository = notificationRepository;
        }

        public async Task<Notification> Handle(MarkNotificationAsSeenRequest request, CancellationToken cancellationToken)
        {
            var notification = await _notificationsQueryHelper.ReadOrThrow(request.NotificationId, cancellationToken);

            notification.Seen = true;

            return await _notificationRepository.Update(notification, cancellationToken);
        }
    }
}