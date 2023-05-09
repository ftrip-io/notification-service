using ftrip.io.notification_service.Notifications.Domain;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.notification_service.Notifications.UseCases.Delete
{
    public class DeleteNotificationRequestHandler : IRequestHandler<DeleteNotificationRequest, Notification>
    {
        private readonly INotificationsQueryHelper _notificationsQueryHelper;
        private readonly INotificationRepository _notificationRepository;

        public DeleteNotificationRequestHandler(
            INotificationsQueryHelper notificationsQueryHelper,
            INotificationRepository notificationRepository)
        {
            _notificationsQueryHelper = notificationsQueryHelper;
            _notificationRepository = notificationRepository;
        }

        public async Task<Notification> Handle(DeleteNotificationRequest request, CancellationToken cancellationToken)
        {
            var notification = await _notificationsQueryHelper.ReadOrThrow(request.NotificationId, cancellationToken);

            return await _notificationRepository.Delete(notification.Id, cancellationToken);
        }
    }
}