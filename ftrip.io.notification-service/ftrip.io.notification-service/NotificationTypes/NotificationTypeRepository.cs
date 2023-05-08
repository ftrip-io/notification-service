using ftrip.io.framework.Persistence.Contracts;
using ftrip.io.framework.Persistence.NoSql.Mongodb.Repository;
using ftrip.io.notification_service.NotificationTypes.Domain;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.notification_service.NotificationTypes
{
    public interface INotificationTypeRepository : IRepository<NotificationType, string>
    {
        Task<IEnumerable<NotificationType>> CreateRange(IEnumerable<NotificationType> notificationTypes, CancellationToken cancellationToken);
    }

    public class NotificationTypeRepository : Repository<NotificationType, string>, INotificationTypeRepository
    {
        public NotificationTypeRepository(IMongoDatabase mongoDatabase) :
            base(mongoDatabase)
        {
        }

        public async Task<IEnumerable<NotificationType>> CreateRange(IEnumerable<NotificationType> notificationTypes, CancellationToken cancellationToken)
        {
            await _collection.InsertManyAsync(notificationTypes, new InsertManyOptions() { }, cancellationToken);

            return notificationTypes;
        }
    }
}