using ftrip.io.notification_service.UserNotificationTypes.Domain;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.notification_service.UserNotificationTypes.UseCases.Configure
{
    public class ConfigureRequestHandler : IRequestHandler<ConfigureRequest, IEnumerable<UserNotificationType>>
    {
        private readonly IUserNotificationTypeRepository _userNotificationTypeRepository;

        public ConfigureRequestHandler(IUserNotificationTypeRepository userNotificationTypeRepository)
        {
            _userNotificationTypeRepository = userNotificationTypeRepository;
        }

        public async Task<IEnumerable<UserNotificationType>> Handle(ConfigureRequest request, CancellationToken cancellationToken)
        {
            await DeleteConfigurationForUser(request.UserId, cancellationToken);

            if (!request.Codes.Any())
            {
                return default;
            }

            return await CreateConfigurationForUser(request, cancellationToken);
        }

        private async Task DeleteConfigurationForUser(string userId, CancellationToken cancellationToken)
        {
            var currentConfiguration = await _userNotificationTypeRepository.ReadByUserId(userId, cancellationToken);

            await _userNotificationTypeRepository.DeleteRange(currentConfiguration, cancellationToken);
        }

        private async Task<IEnumerable<UserNotificationType>> CreateConfigurationForUser(ConfigureRequest request, CancellationToken cancellationToken)
        {
            var newConfiguration = request.Codes.Select(code => new UserNotificationType()
            {
                Code = code,
                UserId = request.UserId
            });

            return await _userNotificationTypeRepository.CreateRange(newConfiguration, cancellationToken);
        }
    }
}