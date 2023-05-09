using MediatR;

namespace ftrip.io.notification_service.Notifications.UseCases.CountByUserId
{
    public class CountByUserIdQuery : IRequest<long>
    {
        public string UserId { get; set; }
        public bool? Seen { get; set; }
    }
}