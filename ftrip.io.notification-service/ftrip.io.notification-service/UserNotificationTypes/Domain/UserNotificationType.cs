using ftrip.io.framework.Domain;

namespace ftrip.io.notification_service.UserNotificationTypes.Domain
{
    public class UserNotificationType : Record
    {
        public string Code { get; set; }
        public string UserId { get; set; }
    }
}