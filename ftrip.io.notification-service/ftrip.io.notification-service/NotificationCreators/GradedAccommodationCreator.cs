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
    public class GradedAccommodationCreator : IConsumer<AccomodationReviewedEvent>
    {
        private readonly IUserNotificationTypeEligibilityHelper _userNotificationTypeEligibilityHelper;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public GradedAccommodationCreator(
            IUserNotificationTypeEligibilityHelper userNotificationTypeEligibilityHelper,
            IMediator mediator,
            ILogger logger)
        {
            _userNotificationTypeEligibilityHelper = userNotificationTypeEligibilityHelper;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<AccomodationReviewedEvent> context)
        {
            var accomodationReviewed = context.Message;
            var userId = accomodationReviewed.HostId.ToString();
            var code = NotificationTypeCodes.GradedAccommodation;

            if (!await _userNotificationTypeEligibilityHelper.IsEligible(userId, code))
            {
                _logger.Information("User not eligible - UserId[{UserId}], NotificationType[{NotificationType}]", userId, code);
                return;
            }

            await _mediator.Send(new SaveNotificationRequest()
            {
                UserId = userId,
                Message = FormNotificationMessage(accomodationReviewed)
            });
        }

        private string FormNotificationMessage(AccomodationReviewedEvent @event) =>
            string.Format(
                "{{User:{0}}} has reviewed your {{Accommodation:{1}}} with an average grade of {2}",
                @event.GuestId, @event.AccomodationId, @event.AverageGrade
            );
    }
}