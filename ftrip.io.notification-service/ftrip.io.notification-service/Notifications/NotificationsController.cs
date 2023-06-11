using ftrip.io.notification_service.Attributes;
using ftrip.io.notification_service.Notifications.UseCases.CountByUserId;
using ftrip.io.notification_service.Notifications.UseCases.Delete;
using ftrip.io.notification_service.Notifications.UseCases.MarkAsSeen;
using ftrip.io.notification_service.Notifications.UseCases.MarkAsUnseen;
using ftrip.io.notification_service.Notifications.UseCases.ReadByUserId;
using ftrip.io.notification_service.Notifications.UseCases.Save;
using ftrip.io.user_service.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.notification_service.Notifications
{
    [ApiController]
    [Route("api/users")]
    public class NotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [UserSpecific]
        [HttpGet("{userId}/notifications")]
        public async Task<IActionResult> Read(string userId, [FromQuery] bool? seen, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new ReadByUserIdQuery() { UserId = userId, Seen = seen }, cancellationToken));
        }

        [Authorize]
        [UserSpecific]
        [HttpGet("{userId}/notifications/count")]
        public async Task<IActionResult> Count(string userId, [FromQuery] bool? seen, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new CountByUserIdQuery() { UserId = userId, Seen = seen }, cancellationToken));
        }

        [HttpPost("{userId}/notifications")]
        public async Task<IActionResult> Save(string userId, SaveNotificationRequest request, CancellationToken cancellationToken = default)
        {
            request.UserId = userId;
            return Ok(await _mediator.Send(request, cancellationToken));
        }

        [Authorize]
        [UserSpecific]
        [UserSpecificNotification]
        [HttpPut("{userId}/notifications/{notificationId}/seen")]
        public async Task<IActionResult> Seen(string notificationId, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new MarkNotificationAsSeenRequest() { NotificationId = notificationId }, cancellationToken));
        }

        [Authorize]
        [UserSpecific]
        [UserSpecificNotification]
        [HttpPut("{userId}/notifications/{notificationId}/unseen")]
        public async Task<IActionResult> Unseen(string notificationId, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new MarkNotificationAsUnseenRequest() { NotificationId = notificationId }, cancellationToken));
        }

        [Authorize]
        [UserSpecific]
        [UserSpecificNotification]
        [HttpDelete("{userId}/notifications/{notificationId}")]
        public async Task<IActionResult> Delete(string notificationId, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new DeleteNotificationRequest() { NotificationId = notificationId }, cancellationToken));
        }
    }
}