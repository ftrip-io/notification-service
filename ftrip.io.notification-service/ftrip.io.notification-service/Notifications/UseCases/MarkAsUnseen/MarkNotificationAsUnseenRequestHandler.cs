using ftrip.io.notification_service.Notifications.Domain;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.notification_service.Notifications.UseCases.MarkAsUnseen
{
    public class MarkNotificationAsUnseenRequestHandler : IRequestHandler<MarkNotificationAsUnseenRequest, Notification>
    {
        private readonly INotificationsQueryHelper _notificationsQueryHelper;
        private readonly INotificationRepository _notificationRepository;

        public MarkNotificationAsUnseenRequestHandler(
            INotificationsQueryHelper notificationsQueryHelper,
            INotificationRepository notificationRepository)
        {
            _notificationsQueryHelper = notificationsQueryHelper;
            _notificationRepository = notificationRepository;
        }

        public async Task<Notification> Handle(MarkNotificationAsUnseenRequest request, CancellationToken cancellationToken)
        {
            var notification = await _notificationsQueryHelper.ReadOrThrow(request.NotificationId, cancellationToken);

            notification.Seen = false;

            return await _notificationRepository.Update(notification, cancellationToken);
        }
    }
}