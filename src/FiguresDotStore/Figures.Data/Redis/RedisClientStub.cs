using System.Collections.Concurrent;
using Figures.Core.Domain;

namespace Figures.Data.Redis
{
    internal class RedisClientStub : IRedisClient
    {
        private static readonly ConcurrentDictionary<string, int> Values = new ConcurrentDictionary<string, int>();

        static RedisClientStub()
        {
            Values.TryAdd(FigureType.Circle.ToString(), 10);
            Values.TryAdd(FigureType.Square.ToString(), 100);
            Values.TryAdd(FigureType.Triangle.ToString(), 1000);
        }

        public int Get(string type)
        {
            return Values.TryGetValue(type, out var value) ? value : 0;
        }

        public void Set(string type, int current)
        {
            Values.AddOrUpdate(type, current, (key, _) => current);
        }
    }
}