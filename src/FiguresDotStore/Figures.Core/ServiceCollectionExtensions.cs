using Figures.Core.Logic;
using Microsoft.Extensions.DependencyInjection;

namespace Figures.Core
{
    /// <summary>
    /// Класс внедрения зависимости данного слоя
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFiguresCore(this IServiceCollection services)
        {
            services.AddScoped<IOrderService, OrderService>();

            return services;
        }
    }
}