using System;

namespace Figures.Core.Domain
{
    public class Circle : Figure
    {
        public Circle(float side) : base(FigureType.Circle)
        {
            SideA = side;

            if (SideA < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(side), "Circle restrictions not met");
            }
        }

        public override double GetArea() => Math.PI * SideA * SideA;

        public override decimal GetPrice() => (decimal)GetArea() * 0.9m;
    }
}