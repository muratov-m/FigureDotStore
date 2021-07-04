namespace Figures.Core.Domain
{
    public abstract class Figure
    {
        public FigureType Type { get; }
        public float SideA { get; protected set; }
        public float SideB { get; protected set; }
        public float SideC { get; protected set; }

        protected Figure(FigureType type)
        {
            Type = type;
        }

        public abstract double GetArea();

        public virtual decimal GetPrice() => (decimal)GetArea();
    }
}