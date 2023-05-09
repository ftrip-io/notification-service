using ftrip.io.notification_service.Notifications.Domain;
using MediatR;
using System.Collections.Generic;

namespace ftrip.io.notification_service.Notifications.UseCases.ReadByUserId
{
    public class ReadByUserIdQuery : IRequest<IEnumerable<Notification>>
    {
        public string UserId { get; set; }
        public bool? Seen { get; set; }
    }
}