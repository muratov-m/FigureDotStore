using Figures.Core.Storage;
using Figures.Data.Redis;

namespace Figures.Data.Storage
{
    internal class FiguresStorage : IFiguresStorage
    {
        // корректно сконфигурированный и готовый к использованию клиент Редиса
        private IRedisClient RedisClient { get; }

        public FiguresStorage(IRedisClient redisClient)
        {
            RedisClient = redisClient;
        }

        public bool CheckIfAvailable(string type, int count)
        {
            return RedisClient.Get(type) >= count;
        }

        public void Reserve(string type, int count)
        {
            var current = RedisClient.Get(type);

            RedisClient.Set(type, current - count);
        }

        public void UndoReserve(string type, int count)
        {
            var current = RedisClient.Get(type);

            RedisClient.Set(type, current + count);
        }
    }
}