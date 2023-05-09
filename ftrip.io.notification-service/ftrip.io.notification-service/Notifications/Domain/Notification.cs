using ftrip.io.framework.Domain;
using System;

namespace ftrip.io.notification_service.Notifications.Domain
{
    public class Notification : Record
    {
        public string Message { get; set; }
        public bool Seen { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}