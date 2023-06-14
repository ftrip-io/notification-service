using ftrip.io.booking_service.contracts.ReservationRequests.Events;
using ftrip.io.notification_service.Notifications.UseCases.Save;
using ftrip.io.notification_service.NotificationTypes;
using ftrip.io.notification_service.UserNotificationTypes;
using MassTransit;
using MediatR;
using Serilog;
using System.Threading.Tasks;

namespace ftrip.io.notification_service.NotificationCreators
{
    public class NewReservationCreator : IConsumer<ReservationRequestCreatedEvent>
    {
        private readonly IUserNotificationTypeEligibilityHelper _userNotificationTypeEligibilityHelper;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public NewReservationCreator(
            IUserNotificationTypeEligibilityHelper userNotificationTypeEligibilityHelper,
            IMediator mediator,
            ILogger logger)
        {
            _userNotificationTypeEligibilityHelper = userNotificationTypeEligibilityHelper;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ReservationRequestCreatedEvent> context)
        {
            var reservationRequestCreated = context.Message;
            var userId = reservationRequestCreated.HostId.ToString();
            var code = NotificationTypeCodes.NewReservation;

            if (!await _userNotificationTypeEligibilityHelper.IsEligible(userId, code))
            {
                _logger.Information("User not eligible - UserId[{UserId}], NotificationType[{NotificationType}]", userId, code);
                return;
            }

            await _mediator.Send(new SaveNotificationRequest()
            {
                UserId = userId,
                Message = FormNotificationMessage(reservationRequestCreated)
            });
        }

        private string FormNotificationMessage(ReservationRequestCreatedEvent @event) =>
            string.Format(
                "{{User:{0}}} has requested reservation for {{Accommodation:{1}}} from {2} to {3}",
                @event.GuestId, @event.AccommodationId, @event.From.ToShortDateString(), @event.To.ToShortDateString()
            );
    }
}