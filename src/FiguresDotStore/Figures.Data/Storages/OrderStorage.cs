using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Figures.Core.Domain;
using Figures.Core.Storages;

[assembly: InternalsVisibleTo("Figures.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Figures.Data.Storages
{
    internal class OrderStorage : IOrderStorage
    {
        public async Task<decimal> SaveAsync(Order order)
        {
            // Имитируем задержку при сохранении заказа
            await Task.Delay(1000);

            return order.GetTotal();
        }
    }
}