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
    public class PositiveAnswerOnReservationCreator : IConsumer<ReservationRequestAcceptedEvent>
    {
        private readonly IUserNotificationTypeEligibilityHelper _userNotificationTypeEligibilityHelper;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public PositiveAnswerOnReservationCreator(
            IUserNotificationTypeEligibilityHelper userNotificationTypeEligibilityHelper,
            IMediator mediator,
            ILogger logger)
        {
            _userNotificationTypeEligibilityHelper = userNotificationTypeEligibilityHelper;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ReservationRequestAcceptedEvent> context)
        {
            var reservationRequestAccepted = context.Message;
            var userId = reservationRequestAccepted.GuestId.ToString();
            var code = NotificationTypeCodes.AnswerOnReservation;

            if (!await _userNotificationTypeEligibilityHelper.IsEligible(userId, code))
            {
                _logger.Information("User not eligible - UserId[{UserId}], NotificationType[{NotificationType}]", userId, code);
                return;
            }

            await _mediator.Send(new SaveNotificationRequest()
            {
                UserId = userId,
                Message = FormNotificationMessage(reservationRequestAccepted)
            });
        }

        private string FormNotificationMessage(ReservationRequestAcceptedEvent @event) =>
            string.Format("Host has accepted reservation request for {{Accommodation:{0}}}", @event.AccomodationId);
    }

    public class NegativeAnswerOnReservationCreator : IConsumer<ReservationRequestDeclinedEvent>
    {
        private readonly IUserNotificationTypeEligibilityHelper _userNotificationTypeEligibilityHelper;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public NegativeAnswerOnReservationCreator(
            IUserNotificationTypeEligibilityHelper userNotificationTypeEligibilityHelper,
            IMediator mediator,
            ILogger logger)
        {
            _userNotificationTypeEligibilityHelper = userNotificationTypeEligibilityHelper;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ReservationRequestDeclinedEvent> context)
        {
            var reservationRequestDeclined = context.Message;
            var userId = reservationRequestDeclined.GuestId.ToString();
            var code = NotificationTypeCodes.AnswerOnReservation;

            if (!await _userNotificationTypeEligibilityHelper.IsEligible(userId, code))
            {
                _logger.Information("User not eligible - UserId[{UserId}], NotificationType[{NotificationType}]", userId, code);
                return;
            }

            await _mediator.Send(new SaveNotificationRequest()
            {
                UserId = userId,
                Message = FormNotificationMessage(reservationRequestDeclined)
            });
        }

        private string FormNotificationMessage(ReservationRequestDeclinedEvent @event) =>
            string.Format("Host has declined reservation request for {{Accommodation:{0}}}", @event.AccomodationId);
    }
}