using ftrip.io.framework.Domain;
using System.Collections.Generic;

namespace ftrip.io.notification_service.NotificationTypes.Domain
{
    public class NotificationType : Record
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string Group { get; set; }
        public List<string> AllowedUserTypes { get; set; }
    }
}