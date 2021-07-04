namespace Figures.Core.Domain
{
    public class OrderPosition
    {
        public Figure Figure { get; }

        public int Count { get; }

        public OrderPosition(Figure figure, int count)
        {
            Figure = figure;
            Count = count;
        }

        public decimal GetSubTotal() => Figure.GetPrice() * Count;
    }
}