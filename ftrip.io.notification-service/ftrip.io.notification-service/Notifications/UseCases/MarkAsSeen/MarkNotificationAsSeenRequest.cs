using ftrip.io.notification_service.Notifications.Domain;
using MediatR;

namespace ftrip.io.notification_service.Notifications.UseCases.MarkAsSeen
{
    public class MarkNotificationAsSeenRequest : IRequest<Notification>
    {
        public string NotificationId { get; set; }
    }
}