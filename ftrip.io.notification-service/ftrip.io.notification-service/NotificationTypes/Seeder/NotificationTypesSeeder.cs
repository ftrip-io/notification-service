using ftrip.io.notification_service.NotificationTypes.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.notification_service.NotificationTypes.Seeder
{
    public class NotificationTypesSeeder
    {
        private readonly INotificationTypeRepository _notificationTypeRepository;

        public NotificationTypesSeeder(INotificationTypeRepository notificationTypeRepository)
        {
            _notificationTypeRepository = notificationTypeRepository;
        }

        public async Task<bool> ShouldSeed()
        {
            return !await _notificationTypeRepository.Query().AnyAsync();
        }

        public async Task Seed()
        {
            var notificationTypes = new List<NotificationType>()
            {
                new NotificationType()
                {
                    Code = NotificationTypeCodes.AnswerOnReservation,
                    Description = "Get a notification when the host answers your reservation.",
                    Group = "Reservation",
                    AllowedUserTypes = new List<string>() { "Guest" }
                },
                new NotificationType()
                {
                    Code = NotificationTypeCodes.NewReservation,
                    Description = "Get a notification when a guest creates a new reservation for your accommodation.",
                    Group = "Reservation",
                    AllowedUserTypes = new List<string>() { "Host" }
                },
                new NotificationType()
                {
                    Code = NotificationTypeCodes.CanceledReservation,
                    Description = "Get a notification when a guest cancels a reservation for your accommodation.",
                    Group = "Reservation",
                    AllowedUserTypes = new List<string>() { "Host" }
                },
                new NotificationType()
                {
                    Code = NotificationTypeCodes.GradedHost,
                    Description = "Get a notification when someone grades you.",
                    Group = "Grade",
                    AllowedUserTypes = new List<string>() { "Host" }
                },
                new NotificationType()
                {
                    Code = NotificationTypeCodes.GradedAccommodation,
                    Description = "Get a notification when someone grades your accommodation.",
                    Group = "Grade",
                    AllowedUserTypes = new List<string>() { "Host" }
                }
            };

            await _notificationTypeRepository.CreateRange(notificationTypes, CancellationToken.None);
        }
    }
}