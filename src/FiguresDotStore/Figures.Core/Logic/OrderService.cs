using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Figures.Core.Domain;
using Figures.Core.Storage;

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

        public async Task<decimal> MakeOrderAsync(Order order)
        {
            var groupedPositions = order.Positions
                .GroupBy(x => x.Figure.Type)
                .ToDictionary(x => x.Key.ToString(), x => x.Sum(position => position.Count));

            ReservePositions(groupedPositions);

            try
            {
                return await _orderStorage.SaveAsync(order);
            }
            catch
            {
                UndoReservePositions(groupedPositions);
                throw;
            }
        }

        private void ReservePositions(Dictionary<string, int> groupedPositions)
        {
            lock (FiguresStorageLocker)
            {
                foreach (var (type, count) in groupedPositions)
                {
                    if (!_figuresStorage.CheckIfAvailable(type, count))
                    {
                        throw new InvalidOperationException($"Not available count for position: {type}");
                    }
                }

                foreach (var (type, count) in groupedPositions)
                {
                    _figuresStorage.Reserve(type, count);
                }
            }
        }

        private void UndoReservePositions(Dictionary<string, int> groupedPositions)
        {
            lock (FiguresStorageLocker)
            {
                foreach (var (type, count) in groupedPositions)
                {
                    _figuresStorage.UndoReserve(type, count);
                }
            }
        }
    }
}