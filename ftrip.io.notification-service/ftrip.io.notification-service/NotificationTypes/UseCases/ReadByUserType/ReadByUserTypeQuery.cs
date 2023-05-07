using ftrip.io.notification_service.NotificationTypes.Domain;
using MediatR;
using System.Collections.Generic;

namespace ftrip.io.notification_service.NotificationTypes.UseCases.ReadByUserType
{
    public class ReadByUserTypeQuery : IRequest<IEnumerable<NotificationType>>
    {
        public string UserType { get; set; }
    }
}