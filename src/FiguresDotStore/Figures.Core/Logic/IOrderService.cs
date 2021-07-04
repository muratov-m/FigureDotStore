using System.Threading.Tasks;
using Figures.Core.Domain;

namespace Figures.Core.Logic
{
    public interface IOrderService
    {
        public Task<decimal> MakeOrderAsync(Order order);
    }
}