using Figures.Core.Domain;
using Figures.Core.Logic;
using NUnit.Framework;

namespace Figures.Tests.Unit.Domain
{
    [TestFixture]
    public class OrderPositionTests
    {
        [Test]
        public void OrderPosition_GetSubTotal_ForTriangle()
        {
            // Arrange
            var figure = FiguresFactory.Create(FigureType.Triangle, 3, 4, 5);
            var orderPosition = new OrderPosition(figure, 10);

            // Act
            var subTotal = orderPosition.GetSubTotal();

            // Assert
            Assert.AreEqual(6 * 1.2m * 10, subTotal);
        }
    }
}