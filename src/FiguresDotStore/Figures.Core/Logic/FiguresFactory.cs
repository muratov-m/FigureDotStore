using System;
using Figures.Core.Domain;

namespace Figures.Core.Logic
{
    public static class FiguresFactory
    {
        public static Figure Create(FigureType type, float sideA, float? sideB = null, float? sideC = null)
        {
            return type switch
            {
                FigureType.Square => new Square(sideA),
                FigureType.Circle => new Circle(sideA),
                FigureType.Triangle => new Triangle(sideA, sideB.GetValueOrDefault(), sideC.GetValueOrDefault()),

                _ => throw new ArgumentOutOfRangeException(nameof(type), $"Unknown position type: {type}")
            };
        }
    }
}