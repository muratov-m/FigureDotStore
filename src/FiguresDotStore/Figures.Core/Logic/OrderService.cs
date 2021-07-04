using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Figures.Core.Domain;
using Figures.Core.Storages;

[assembly: InternalsVisibleTo("Figures.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Figures.Core.Logic
{
    internal class OrderService : IOrderService
    {
        private readonly IFiguresStorage _figuresStorage;
        private readonly IOrderStorage _orderStorage;

        private static readonly object FiguresStorageLocker = new object();

        public OrderService(IFiguresStorage figuresStorage, IOrderStorage orderStorage)
        {
            _figuresStorage = figuresStorage;
            _orderStorage = orderStorage;
        }

        public async Task<decimal> MakeOrder(Order order)
        {
            var groupedPositions = order.Positions
                .GroupBy(x => x.Figure.Type)
                .ToDictionary(x => x.Key.ToString(), x => x.Sum(position => position.Count));

            lock (FiguresStorageLocker)
            {
                foreach (var position in groupedPositions)
                {
                    if (!_figuresStorage.CheckIfAvailable(position.Key, position.Value))
                    {
                        throw new InvalidOperationException("Not Available");
                    }
                }

                foreach (var position in groupedPositions)
                {
                    _figuresStorage.Reserve(position.Key, position.Value);
                }
            }

            try
            {
                return await _orderStorage.Save(order);
            }
            catch
            {
                foreach (var position in groupedPositions)
                {
                    _figuresStorage.UndoReserve(position.Key, position.Value);
                }

                throw;
            }
        }
    }
}