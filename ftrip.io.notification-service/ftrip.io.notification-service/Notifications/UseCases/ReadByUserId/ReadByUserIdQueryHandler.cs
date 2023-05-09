using ftrip.io.notification_service.Notifications.Domain;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.notification_service.Notifications.UseCases.ReadByUserId
{
    public class ReadByUserIdQueryHandler : IRequestHandler<ReadByUserIdQuery, IEnumerable<Notification>>
    {
        private readonly INotificationRepository _notificationRepository;

        public ReadByUserIdQueryHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<IEnumerable<Notification>> Handle(ReadByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await _notificationRepository.ReadByUserIdAndSeen(request.UserId, request.Seen, cancellationToken);
        }
    }
}