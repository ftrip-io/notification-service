using ftrip.io.framework.Domain;
using System;

namespace ftrip.io.notification_service.contracts.Notifications.Events
{
    public class NotificationSavedEvent : Event<string>
    {
        public string NotificationId { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }

        public NotificationSavedEvent()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}