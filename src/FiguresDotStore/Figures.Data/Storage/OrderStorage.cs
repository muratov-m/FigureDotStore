using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Figures.Core.Domain;
using Figures.Core.Storage;

[assembly: InternalsVisibleTo("Figures.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Figures.Data.Storage
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