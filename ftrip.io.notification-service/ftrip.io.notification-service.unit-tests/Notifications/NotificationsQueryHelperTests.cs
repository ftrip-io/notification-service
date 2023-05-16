using FluentAssertions;
using ftrip.io.framework.ExceptionHandling.Exceptions;
using ftrip.io.framework.Globalization;
using ftrip.io.notification_service.Notifications;
using ftrip.io.notification_service.Notifications.Domain;
using Moq;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ftrip.io.notification_service.unit_tests.Notifications
{
    public class NotificationsQueryHelperTests
    {
        private readonly Mock<INotificationRepository> _notificationRepositoryMock = new Mock<INotificationRepository>();
        private readonly Mock<IStringManager> _stringManagerMock = new Mock<IStringManager>();
        private readonly Mock<ILogger> _loggerMock = new Mock<ILogger>();

        private readonly NotificationsQueryHelper _notificationsQueryHelper;

        public NotificationsQueryHelperTests()
        {
            _notificationsQueryHelper = new NotificationsQueryHelper(
                _notificationRepositoryMock.Object,
                _stringManagerMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public void Handle_NotificationDoesNotExist_ThrowsException()
        {
            // Arrange
            var nonExistingId = "64566f1f0b5a9507e22990b0";

            _notificationRepositoryMock
                .Setup(r => r.Read(It.Is((string id) => id == nonExistingId), It.IsAny<CancellationToken>()))
                .Returns((string id, CancellationToken _) => null);

            // Act
            Func<Task<Notification>> query = () => _notificationsQueryHelper.ReadOrThrow(nonExistingId, CancellationToken.None);

            // Assert
            query.Should().ThrowExactlyAsync<MissingEntityException>();
        }

        [Fact]
        public async Task Handle_Successful_ReturnsNotification()
        {
            // Arrange
            var existingId = "64566f1f0b5a9507e22990b0";

            _notificationRepositoryMock
                .Setup(r => r.Read(It.Is((string id) => id == existingId), It.IsAny<CancellationToken>()))
                .Returns((string id, CancellationToken _) => Task.FromResult(
                    new Notification() { Id = existingId }
                ));

            // Act
            var notification = await _notificationsQueryHelper.ReadOrThrow(existingId, CancellationToken.None);

            // Assert
            notification.Should().NotBeNull();
        }
    }
}