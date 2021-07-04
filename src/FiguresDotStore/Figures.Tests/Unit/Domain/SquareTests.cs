using System;
using Figures.Core.Domain;
using NUnit.Framework;

namespace Figures.Tests.Unit.Domain
{
    public class SquareTests
    {
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(1000f)]
        public void Square_WhenValidArgument_DoesNotThrowException(float side)
        {
            Assert.DoesNotThrow(() => new Square(side));
        }

        [TestCase(-1)]
        [TestCase(-1000f)]
        public void Square_WhenInvalidArgument_ShouldThrowsException(float side)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Square(side));
        }

        [TestCase(2, ExpectedResult = 4)]
        [TestCase(2.5f, ExpectedResult = 6.25d)]
        public double Square_GetArea_Tests(float sideA)
        {
            var triangle = new Square(sideA);

            return triangle.GetArea();
        }
    }
}