using ftrip.io.framework.Persistence.Contracts;
using ftrip.io.framework.Persistence.NoSql.Mongodb.Repository;
using ftrip.io.notification_service.Notifications.Domain;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.notification_service.Notifications
{
    public interface INotificationRepository : IRepository<Notification, string>
    {
        Task<IEnumerable<Notification>> ReadByUserIdAndSeen(string userId, bool? seen, CancellationToken cancellationToken);

        Task<long> CountByUserIdAndSeen(string userId, bool? seen, CancellationToken cancellationToken);
    }

    public class NotificationRepository : Repository<Notification, string>, INotificationRepository
    {
        public NotificationRepository(IMongoDatabase mongoDatabase) :
            base(mongoDatabase)
        {
        }

        public async Task<IEnumerable<Notification>> ReadByUserIdAndSeen(string userId, bool? seen, CancellationToken cancellationToken)
        {
            return await _collection
                .Find(notification => notification.UserId == userId && (seen == null || notification.Seen == seen))
                .SortByDescending(notification => notification.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<long> CountByUserIdAndSeen(string userId, bool? seen, CancellationToken cancellationToken)
        {
            return await _collection
                .Find(notification => notification.UserId == userId && (seen == null || notification.Seen == seen))
                .CountDocumentsAsync(cancellationToken);
        }
    }
}