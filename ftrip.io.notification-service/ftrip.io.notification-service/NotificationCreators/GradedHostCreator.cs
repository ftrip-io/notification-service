using ftrip.io.booking_service.contracts.Reviews.Events;
using ftrip.io.notification_service.Notifications.UseCases.Save;
using ftrip.io.notification_service.NotificationTypes;
using ftrip.io.notification_service.UserNotificationTypes;
using MassTransit;
using MediatR;
using Serilog;
using System.Threading.Tasks;

namespace ftrip.io.notification_service.NotificationCreators
{
    public class GradedHostCreator : IConsumer<HostReviewedEvent>
    {
        private readonly IUserNotificationTypeEligibilityHelper _userNotificationTypeEligibilityHelper;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public GradedHostCreator(
            IUserNotificationTypeEligibilityHelper userNotificationTypeEligibilityHelper,
            IMediator mediator,
            ILogger logger)
        {
            _userNotificationTypeEligibilityHelper = userNotificationTypeEligibilityHelper;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<HostReviewedEvent> context)
        {
            var hostReviewed = context.Message;
            var userId = hostReviewed.HostId.ToString();
            var code = NotificationTypeCodes.GradedHost;

            if (!await _userNotificationTypeEligibilityHelper.IsEligible(userId, code))
            {
                _logger.Information("User not eligible - UserId[{UserId}], NotificationType[{NotificationType}]", userId, code);
                return;
            }

            await _mediator.Send(new SaveNotificationRequest()
            {
                UserId = userId,
                Message = FormNotificationMessage(hostReviewed)
            });
        }

        private string FormNotificationMessage(HostReviewedEvent @event) =>
            string.Format("{{User:{0}}} has reviewed you with an average grade of {1}", @event.GuestId, @event.AverageGrade);
    }
}