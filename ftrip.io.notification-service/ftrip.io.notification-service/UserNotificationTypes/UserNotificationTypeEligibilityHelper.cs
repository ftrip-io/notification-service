using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.notification_service.UserNotificationTypes
{
    public interface IUserNotificationTypeEligibilityHelper
    {
        Task<bool> IsEligible(string userId, string code, CancellationToken cancellationToken = default);
    }

    public class UserNotificationTypeEligibilityHelper : IUserNotificationTypeEligibilityHelper
    {
        private readonly IUserNotificationTypeRepository _userNotificationTypeRepository;

        public UserNotificationTypeEligibilityHelper(IUserNotificationTypeRepository userNotificationTypeRepository)
        {
            _userNotificationTypeRepository = userNotificationTypeRepository;
        }

        public async Task<bool> IsEligible(string userId, string code, CancellationToken cancellationToken = default)
        {
            return await _userNotificationTypeRepository.ReadByUserIdAndCode(userId, code, cancellationToken) != null;
        }
    }
}