using ftrip.io.notification_service.UserNotificationTypes.Domain;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.notification_service.UserNotificationTypes.UseCases.ReadByUserId
{
    public class ReadByUserIdQueryHandler : IRequestHandler<ReadByUserIdQuery, IEnumerable<UserNotificationType>>
    {
        private readonly IUserNotificationTypeRepository _userNotificationTypeRepository;

        public ReadByUserIdQueryHandler(IUserNotificationTypeRepository userNotificationTypeRepository)
        {
            _userNotificationTypeRepository = userNotificationTypeRepository;
        }

        public async Task<IEnumerable<UserNotificationType>> Handle(ReadByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await _userNotificationTypeRepository.ReadByUserId(request.UserId, cancellationToken);
        }
    }
}