using AutoMapper;
using ftrip.io.framework.messaging.Publisher;
using ftrip.io.notification_service.contracts.Notifications.Events;
using ftrip.io.notification_service.Notifications.Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.notification_service.Notifications.UseCases.Save
{
    public class SaveNotificationRequestHandler : IRequestHandler<SaveNotificationRequest, Notification>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;
        private readonly IMessagePublisher _messagePublisher;

        public SaveNotificationRequestHandler(
            INotificationRepository notificationRepository,
            IMapper mapper,
            IMessagePublisher messagePublisher)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
            _messagePublisher = messagePublisher;
        }

        public async Task<Notification> Handle(SaveNotificationRequest request, CancellationToken cancellationToken)
        {
            var notification = _mapper.Map<Notification>(request);
            notification.Seen = false;
            notification.CreatedAt = DateTime.Now;

            var createdNotification = await _notificationRepository.Create(notification, cancellationToken);

            await PublishNotificationSavedEvent(createdNotification, cancellationToken);

            return createdNotification;
        }

        private async Task PublishNotificationSavedEvent(Notification notification, CancellationToken cancellationToken)
        {
            var notificationSaved = new NotificationSavedEvent()
            {
                UserId = notification.UserId.ToString(),
                Message = notification.Message,
            };

            await _messagePublisher.Send<NotificationSavedEvent, string>(notificationSaved, cancellationToken);
        }
    }
}