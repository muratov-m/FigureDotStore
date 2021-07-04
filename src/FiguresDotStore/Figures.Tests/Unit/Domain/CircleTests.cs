using System;
using Figures.Core.Domain;
using NUnit.Framework;

namespace Figures.Tests.Unit.Domain
{
    [TestFixture]
    public class CircleTests
    {
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(1000f)]
        public void Circle_WhenValidArgument_DoesNotThrowException(float side)
        {
            Assert.DoesNotThrow(() => new Circle(side));
        }

        [TestCase(-1)]
        [TestCase(-1000f)]
        public void Circle_WhenInvalidArgument_ShouldThrowsException(float side)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Circle(side));
        }

        [TestCase(1, ExpectedResult = Math.PI)]
        [TestCase(10.5f, ExpectedResult = 346.36059005827474d)]
        public double Circle_GetArea_Tests(float sideA)
        {
            var triangle = new Circle(sideA);

            return triangle.GetArea();
        }
    }
}