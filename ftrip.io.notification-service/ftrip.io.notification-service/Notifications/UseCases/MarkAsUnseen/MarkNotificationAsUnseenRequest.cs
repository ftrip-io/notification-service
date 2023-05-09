using ftrip.io.notification_service.Notifications.Domain;
using MediatR;

namespace ftrip.io.notification_service.Notifications.UseCases.MarkAsUnseen
{
    public class MarkNotificationAsUnseenRequest : IRequest<Notification>
    {
        public string NotificationId { get; set; }
    }
}