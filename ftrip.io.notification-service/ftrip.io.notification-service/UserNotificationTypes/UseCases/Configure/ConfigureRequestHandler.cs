using ftrip.io.notification_service.UserNotificationTypes.Domain;
using MediatR;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.notification_service.UserNotificationTypes.UseCases.Configure
{
    public class ConfigureRequestHandler : IRequestHandler<ConfigureRequest, IEnumerable<UserNotificationType>>
    {
        private readonly IUserNotificationTypeRepository _userNotificationTypeRepository;
        private readonly ILogger _logger;

        public ConfigureRequestHandler(
            IUserNotificationTypeRepository userNotificationTypeRepository,
            ILogger logger)
        {
            _userNotificationTypeRepository = userNotificationTypeRepository;
            _logger = logger;
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

            _logger.Information("Deleted {Count} user notification types - UserId[{UserId}]", currentConfiguration.Count(), userId);
        }

        private async Task<IEnumerable<UserNotificationType>> CreateConfigurationForUser(ConfigureRequest request, CancellationToken cancellationToken)
        {
            var newConfiguration = request.Codes.Select(code => new UserNotificationType()
            {
                Code = code,
                UserId = request.UserId
            });

            var createdUserNotificationTypes = await _userNotificationTypeRepository.CreateRange(newConfiguration, cancellationToken);

            _logger.Information("Created {Count} user notification types - UserId[{UserId}]", request.Codes.Count, request.UserId);

            return createdUserNotificationTypes;
        }
    }
}