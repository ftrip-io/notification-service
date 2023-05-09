using ftrip.io.framework.Mapping;
using ftrip.io.notification_service.Notifications.Domain;
using MediatR;

namespace ftrip.io.notification_service.Notifications.UseCases.Save
{
    [Mappable(Destination = typeof(Notification))]
    public class SaveNotificationRequest : IRequest<Notification>
    {
        public string Message { get; set; }
        public string UserId { get; set; }
    }
}