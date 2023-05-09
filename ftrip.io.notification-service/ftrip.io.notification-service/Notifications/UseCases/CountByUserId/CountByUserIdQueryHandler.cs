using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.notification_service.Notifications.UseCases.CountByUserId
{
    public class CountByUserIdQueryHandler : IRequestHandler<CountByUserIdQuery, long>
    {
        private readonly INotificationRepository _notificationRepository;

        public CountByUserIdQueryHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<long> Handle(CountByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await _notificationRepository.CountByUserIdAndSeen(request.UserId, request.Seen, cancellationToken);
        }
    }
}