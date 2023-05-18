using AutoMapper;
using FluentAssertions;
using ftrip.io.framework.messaging.Publisher;
using ftrip.io.notification_service.contracts.Notifications.Events;
using ftrip.io.notification_service.Notifications;
using ftrip.io.notification_service.Notifications.Domain;
using ftrip.io.notification_service.Notifications.UseCases.Save;
using Moq;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ftrip.io.notification_service.unit_tests.Notifications.UseCases.Save
{
    public class SaveNotificationRequestHandlerTests
    {
        private readonly Mock<INotificationRepository> _notificationRepositoryMock = new Mock<INotificationRepository>();
        private readonly Mock<IMessagePublisher> _messagePublisherMock = new Mock<IMessagePublisher>();
        private readonly Mock<ILogger> _loggerMock = new Mock<ILogger>();

        private readonly SaveNotificationRequestHandler _handler;

        public SaveNotificationRequestHandlerTests()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SaveNotificationRequest, Notification>();
            }).CreateMapper();

            _handler = new SaveNotificationRequestHandler(
                _notificationRepositoryMock.Object,
                mapper,
                _messagePublisherMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_Successful_ReturnsNotification()
        {
            // Arrange
            var request = GetSaveNotificationRequest();

            _notificationRepositoryMock
                .Setup(r => r.Create(It.IsAny<Notification>(), It.IsAny<CancellationToken>()))
                .Returns((Notification n, CancellationToken _) =>
                {
                    n.Id = Guid.NewGuid().ToString();
                    return Task.FromResult(n);
                });

            // Act
            var createdNotification = await _handler.Handle(request, CancellationToken.None);

            // Assert
            createdNotification.Should().NotBeNull();
            createdNotification.Seen.Should().BeFalse();
            _messagePublisherMock.Verify(mp => mp.Send<NotificationSavedEvent, string>(It.IsAny<NotificationSavedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        private SaveNotificationRequest GetSaveNotificationRequest()
        {
            return new SaveNotificationRequest()
            {
                Message = "Test Notification Message",
                UserId = Guid.NewGuid().ToString()
            };
        }
    }
}