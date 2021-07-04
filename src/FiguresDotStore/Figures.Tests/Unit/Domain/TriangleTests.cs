using System;
using Figures.Core.Domain;
using NUnit.Framework;

namespace Figures.Tests.Unit.Domain
{
    [TestFixture]
    public class TriangleTests
    {
        [TestCase(3, 4, 5)]
        [TestCase(11, 12.1f, 23)]
        public void Triangle_WhenValidArgument_DoesNotThrowException(float sideA, float sideB, float sideC)
        {
            Assert.DoesNotThrow(() => new Triangle(sideA, sideB, sideC));
        }

        [TestCase(1, 2, 3)]
        [TestCase(1, 2, 10)]
        [TestCase(10, 1, 2)]
        [TestCase(2, 10, 1)]
        public void Triangle_WhenInvalidArgument_ShouldThrowsException(float sideA, float sideB, float sideC)
        {
            Assert.Throws<ArgumentException>(() => new Triangle(sideA, sideB, sideC));
        }

        [TestCase(3, 4, 5, ExpectedResult = 6d)]
        [TestCase(2.5f, 2.3f, 3.7f, ExpectedResult = 2.8243084018770994d)]
        public double Triangle_GetArea_Tests(float sideA, float sideB, float sideC)
        {
            var triangle = new Triangle(sideA, sideB, sideC);

            return triangle.GetArea();
        }

        [Test]
        public void Triangle_GetPrice()
        {
            // Arrange
            var triangle = new Triangle(3, 4, 5);

            // Act
            var price = triangle.GetPrice();

            // Assert
            Assert.AreEqual(6 * 1.2m, price);
        }
    }
}