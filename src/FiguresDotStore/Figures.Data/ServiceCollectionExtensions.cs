using Figures.Core.Storages;
using Figures.Data.Redis;
using Figures.Data.Storages;
using Microsoft.Extensions.DependencyInjection;

namespace Figures.Data
{
    /// <summary>
    /// Класс внедрения зависимости данного слоя
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFiguresData(this IServiceCollection services)
        {
            services.AddScoped<IRedisClient, RedisClientStub>();
            services.AddScoped<IFiguresStorage, FiguresStorage>();
            services.AddScoped<IOrderStorage, OrderStorage>();

            return services;
        }
    }
}