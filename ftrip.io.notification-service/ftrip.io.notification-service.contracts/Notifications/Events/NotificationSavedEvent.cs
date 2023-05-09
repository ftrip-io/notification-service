using ftrip.io.framework.Domain;

namespace ftrip.io.notification_service.contracts.Notifications.Events
{
    public class NotificationSavedEvent : Event<string>
    {
        public string UserId { get; set; }
        public string Message { get; set; }
    }
}