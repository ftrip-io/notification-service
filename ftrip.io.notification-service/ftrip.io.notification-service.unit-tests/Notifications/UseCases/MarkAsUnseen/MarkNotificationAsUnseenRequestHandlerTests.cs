using FluentAssertions;
using ftrip.io.notification_service.Notifications;
using ftrip.io.notification_service.Notifications.Domain;
using ftrip.io.notification_service.Notifications.UseCases.MarkAsUnseen;
using Moq;
using Serilog;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ftrip.io.notification_service.unit_tests.Notifications.UseCases.MarkAsUnseen
{
    public class MarkNotificationAsUnseenRequestHandlerTests
    {
        private readonly Mock<INotificationsQueryHelper> _notificationsQueryHelperMock = new Mock<INotificationsQueryHelper>();
        private readonly Mock<INotificationRepository> _notificationRepositoryMock = new Mock<INotificationRepository>();
        private readonly Mock<ILogger> _loggerMock = new Mock<ILogger>();

        private readonly MarkNotificationAsUnseenRequestHandler _handler;

        public MarkNotificationAsUnseenRequestHandlerTests()
        {
            _handler = new MarkNotificationAsUnseenRequestHandler(
                _notificationsQueryHelperMock.Object,
               _notificationRepositoryMock.Object,
               _loggerMock.Object
           );
        }

        [Fact]
        public async Task Handle_Successful_ReturnsNotification()
        {
            // Arrange
            var existingId = "64566f1f0b5a9507e22990b0";
            var request = new MarkNotificationAsUnseenRequest()
            {
                NotificationId = existingId
            };

            _notificationsQueryHelperMock
                .Setup(queryHelper => queryHelper.ReadOrThrow(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns((string id, CancellationToken _) =>
                {
                    return Task.FromResult(new Notification()
                    {
                        Id = id
                    });
                });

            _notificationRepositoryMock
                .Setup(r => r.Update(It.IsAny<Notification>(), It.IsAny<CancellationToken>()))
                .Returns((Notification n, CancellationToken _) => Task.FromResult(n));

            // Act
            var updatedNotification = await _handler.Handle(request, CancellationToken.None);

            // Assert
            updatedNotification.Should().NotBeNull();
            updatedNotification.Seen.Should().BeFalse();
        }
    }
}