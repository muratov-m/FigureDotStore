using System;

namespace Figures.Core.Domain
{
    public class Triangle : Figure
    {
        public Triangle(float sideA, float sideB, float sideC) : base(FigureType.Triangle)
        {
            SideA = sideA;
            SideB = sideB;
            SideC = sideC;

            if (!CheckTriangleInequality(SideA, SideB, SideC)
                || !CheckTriangleInequality(SideB, SideA, SideC)
                || !CheckTriangleInequality(SideC, SideB, SideA))
            {
                throw new ArgumentException("Triangle restrictions not met");
            }

            bool CheckTriangleInequality(float a, float b, float c) => a < b + c;
        }

        public override double GetArea()
        {
            var p = (SideA + SideB + SideC) / 2;
            return Math.Sqrt(p * (p - SideA) * (p - SideB) * (p - SideC));
        }

        public override decimal GetPrice() => (decimal)GetArea() * 1.2m;
    }
}