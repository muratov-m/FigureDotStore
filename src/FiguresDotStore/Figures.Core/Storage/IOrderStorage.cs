using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Figures.Core.Domain;

[assembly: InternalsVisibleTo("Figures.Data")]
namespace Figures.Core.Storage
{
    internal interface IOrderStorage
    {
        // сохраняет оформленный заказ и возвращает сумму
        Task<decimal> SaveAsync(Order order);
    }
}