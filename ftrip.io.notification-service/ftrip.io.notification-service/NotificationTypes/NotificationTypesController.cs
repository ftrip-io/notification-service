using ftrip.io.notification_service.NotificationTypes.UseCases.ReadByUserType;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.notification_service.NotificationTypes
{
    [ApiController]
    [Route("api/notification-types")]
    public class NotificationTypesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationTypesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Read([FromQuery] string userType, CancellationToken cancellationToken = default)
        {
            return Ok(await _mediator.Send(new ReadByUserTypeQuery() { UserType = userType }, cancellationToken));
        }
    }
}