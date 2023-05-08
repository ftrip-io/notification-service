using ftrip.io.notification_service.UserNotificationTypes.UseCases.Configure;
using ftrip.io.notification_service.UserNotificationTypes.UseCases.ReadByUserId;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.notification_service.UserNotificationTypes
{
    [ApiController]
    [Route("api/users")]
    public class UserNotificationTypesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserNotificationTypesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{userId}/notification-types")]
        public async Task<IActionResult> Read(string userId, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new ReadByUserIdQuery() { UserId = userId }, cancellationToken));
        }

        [HttpPut("{userId}/notification-types")]
        public async Task<IActionResult> Configure(string userId, ConfigureRequest request, CancellationToken cancellationToken = default)
        {
            request.UserId = userId;
            return Ok(await _mediator.Send(request, cancellationToken));
        }
    }
}