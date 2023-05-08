using ftrip.io.notification_service.UserNotificationTypes.Domain;
using MediatR;
using System.Collections.Generic;

namespace ftrip.io.notification_service.UserNotificationTypes.UseCases.ReadByUserId
{
    public class ReadByUserIdQuery : IRequest<IEnumerable<UserNotificationType>>
    {
        public string UserId { get; set; }
    }
}