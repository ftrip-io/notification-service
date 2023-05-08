using ftrip.io.notification_service.UserNotificationTypes.Domain;
using MediatR;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ftrip.io.notification_service.UserNotificationTypes.UseCases.Configure
{
    public class ConfigureRequest : IRequest<IEnumerable<UserNotificationType>>
    {
        [JsonIgnore]
        public string UserId { get; set; }

        public List<string> Codes { get; set; }
    }
}