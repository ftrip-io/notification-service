using ftrip.io.notification_service.NotificationTypes.Domain;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.notification_service.NotificationTypes.UseCases.ReadByUserType
{
    public class ReadByUserTypeQueryHandler : IRequestHandler<ReadByUserTypeQuery, IEnumerable<NotificationType>>
    {
        private readonly INotificationTypeRepository _notificationTypeRepository;

        public ReadByUserTypeQueryHandler(INotificationTypeRepository notificationTypeRepository)
        {
            _notificationTypeRepository = notificationTypeRepository;
        }

        public async Task<IEnumerable<NotificationType>> Handle(ReadByUserTypeQuery request, CancellationToken cancellationToken)
        {
            return await _notificationTypeRepository
                .Query()
                .Where(notificationType => notificationType.AllowedUserTypes.Contains(request.UserType))
                .ToListAsync(cancellationToken);
        }
    }
}