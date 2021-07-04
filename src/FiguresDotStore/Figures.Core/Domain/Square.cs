using System;

namespace Figures.Core.Domain
{
    public class Square : Figure
    {
        public Square(float side) : base(FigureType.Square)
        {
            SideA = side;

            if (SideA < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(SideA), "Square restrictions not met");
            }
        }

        public override double GetArea() => SideA * SideA;
    }
}