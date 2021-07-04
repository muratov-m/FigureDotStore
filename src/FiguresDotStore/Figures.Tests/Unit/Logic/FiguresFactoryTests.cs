using Figures.Core.Domain;
using Figures.Core.Logic;
using NUnit.Framework;

namespace Figures.Tests.Unit.Logic
{
    [TestFixture]
    public class FiguresFactoryTests
    {
        [Test]
        public void FiguresFactory_Create_Square()
        {
            // Act
            var figure = FiguresFactory.Create(FigureType.Square, 3, 4, 5);

            // Assert
            Assert.IsAssignableFrom<Square>(figure);
            Assert.AreEqual(FigureType.Square, figure.Type);
            Assert.AreEqual(3, figure.SideA);
            Assert.AreEqual(0f, figure.SideB);
            Assert.AreEqual(0f, figure.SideC);
        }

        [Test]
        public void FiguresFactory_Create_Circle()
        {
            // Act
            var figure = FiguresFactory.Create(FigureType.Circle, 3, 4, 5);

            // Assert
            Assert.IsAssignableFrom<Circle>(figure);
            Assert.AreEqual(FigureType.Circle, figure.Type);
            Assert.AreEqual(3, figure.SideA);
            Assert.AreEqual(0f, figure.SideB);
            Assert.AreEqual(0f, figure.SideC);
        }

        [Test]
        public void FiguresFactory_Create_Triangle()
        {
            // Act
            var figure = FiguresFactory.Create(FigureType.Triangle, 3, 4, 5);

            // Assert
            Assert.IsAssignableFrom<Triangle>(figure);
            Assert.AreEqual(FigureType.Triangle, figure.Type);
            Assert.AreEqual(3, figure.SideA);
            Assert.AreEqual(4, figure.SideB);
            Assert.AreEqual(5, figure.SideC);
        }
    }
}