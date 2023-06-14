using ftrip.io.framework.Persistence.Contracts;
using ftrip.io.framework.Persistence.NoSql.Mongodb.Repository;
using ftrip.io.notification_service.UserNotificationTypes.Domain;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.notification_service.UserNotificationTypes
{
    public interface IUserNotificationTypeRepository : IRepository<UserNotificationType, string>
    {
        Task<IEnumerable<UserNotificationType>> ReadByUserId(string userId, CancellationToken cancellationToken);

        Task<UserNotificationType> ReadByUserIdAndCode(string userId, string code, CancellationToken cancellationToken);

        Task<IEnumerable<UserNotificationType>> CreateRange(IEnumerable<UserNotificationType> notificationTypes, CancellationToken cancellationToken);
    }

    public class UserNotificationTypeRepository : Repository<UserNotificationType, string>, IUserNotificationTypeRepository
    {
        public UserNotificationTypeRepository(IMongoDatabase mongoDatabase) :
           base(mongoDatabase)
        {
        }

        public async Task<IEnumerable<UserNotificationType>> ReadByUserId(string userId, CancellationToken cancellationToken)
        {
            return await _collection
                .Find(userNotificationType => userNotificationType.UserId == userId)
                .ToListAsync(cancellationToken);
        }

        public async Task<UserNotificationType> ReadByUserIdAndCode(string userId, string code, CancellationToken cancellationToken)
        {
            return await _collection
                .Find(userNotificationType => userNotificationType.UserId == userId && userNotificationType.Code == code)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<UserNotificationType>> CreateRange(IEnumerable<UserNotificationType> notificationTypes, CancellationToken cancellationToken)
        {
            await _collection.InsertManyAsync(notificationTypes, new InsertManyOptions() { }, cancellationToken);

            return notificationTypes;
        }
    }
}