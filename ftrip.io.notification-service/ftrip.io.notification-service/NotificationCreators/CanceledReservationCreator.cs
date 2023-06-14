using ftrip.io.booking_service.contracts.Reservations.Events;
using ftrip.io.notification_service.Notifications.UseCases.Save;
using ftrip.io.notification_service.NotificationTypes;
using ftrip.io.notification_service.UserNotificationTypes;
using MassTransit;
using MediatR;
using Serilog;
using System.Threading.Tasks;

namespace ftrip.io.notification_service.NotificationCreators
{
    public class CanceledReservationCreator : IConsumer<ReservationCanceledEvent>
    {
        private readonly IUserNotificationTypeEligibilityHelper _userNotificationTypeEligibilityHelper;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public CanceledReservationCreator(
            IUserNotificationTypeEligibilityHelper userNotificationTypeEligibilityHelper,
            IMediator mediator,
            ILogger logger)
        {
            _userNotificationTypeEligibilityHelper = userNotificationTypeEligibilityHelper;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ReservationCanceledEvent> context)
        {
            var reservationCanceled = context.Message;
            var userId = reservationCanceled.HostId.ToString();
            var code = NotificationTypeCodes.CanceledReservation;

            if (!await _userNotificationTypeEligibilityHelper.IsEligible(userId, code))
            {
                _logger.Information("User not eligible - UserId[{UserId}], NotificationType[{NotificationType}]", userId, code);
                return;
            }

            await _mediator.Send(new SaveNotificationRequest()
            {
                UserId = userId,
                Message = FormNotificationMessage(reservationCanceled)
            });
        }

        private string FormNotificationMessage(ReservationCanceledEvent @event) =>
            string.Format(
                "{{User:{0}}} has cancelled reservation for {{Accommodation:{1}}} from {2} to {3}",
                @event.GuestId, @event.AccomodationId, @event.From.ToShortDateString(), @event.To.ToShortDateString()
            );
    }
}