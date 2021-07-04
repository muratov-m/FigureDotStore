namespace Figures.Data.Redis
{
    internal interface IRedisClient
    {
        int Get(string type);
        void Set(string type, int current);
    }
}