using ftrip.io.notification_service.Notifications.Domain;
using MediatR;

namespace ftrip.io.notification_service.Notifications.UseCases.Delete
{
    public class DeleteNotificationRequest : IRequest<Notification>
    {
        public string NotificationId { get; set; }
    }
}