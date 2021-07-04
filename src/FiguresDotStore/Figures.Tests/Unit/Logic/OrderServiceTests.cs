using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Figures.Core.Domain;
using Figures.Core.Logic;
using Figures.Core.Storages;
using Moq;
using NUnit.Framework;

namespace Figures.Tests.Unit.Logic
{
    [TestFixture]
    public class OrderServiceTests
    {
        [Test(Description = "Проверка резервирования позиции и сохранения заказа")]
        public async Task MakeOrder_ShouldReservePosition_AndSaveOrder()
        {
            // Arrange
            var order = new Order
            {
                Positions = new List<OrderPosition>
                {
                    new OrderPosition(
                        FiguresFactory.Create(FigureType.Circle, 1),
                        1
                    )
                }
            };

            var figuresStorageMock = new Mock<IFiguresStorage>();
            figuresStorageMock
                .Setup(x => x.CheckIfAvailable(FigureType.Circle.ToString(), 1))
                .Returns(true);

            var orderStorageMock = new Mock<IOrderStorage>();

            var orderService = new OrderService(figuresStorageMock.Object, orderStorageMock.Object);

            // Act
            await orderService.MakeOrder(order);

            // Assert
            figuresStorageMock.Verify(x => x.CheckIfAvailable(FigureType.Circle.ToString(), 1));
            figuresStorageMock.Verify(x => x.Reserve(FigureType.Circle.ToString(), 1));
            figuresStorageMock.Verify(x => x.UndoReserve(FigureType.Circle.ToString(), 1), Times.Never);
            orderStorageMock.Verify(x => x.Save(order));
        }

        [Test(Description = "При резервировании позиций происходит группировка по типу фигур (чтобы уменьшить кол-во обращений к хранилищу)")]
        public async Task MakeOrder_ReservePositions_ShouldSumByFigureType()
        {
            // Arrange
            var order = new Order
            {
                Positions = new List<OrderPosition>
                {
                    new OrderPosition(
                        FiguresFactory.Create(FigureType.Circle, 1),
                        1
                    ),
                    new OrderPosition(
                        FiguresFactory.Create(FigureType.Square, 1),
                        10
                    ),
                    new OrderPosition(
                        FiguresFactory.Create(FigureType.Circle, 1),
                        2
                    ),
                    new OrderPosition(
                        FiguresFactory.Create(FigureType.Triangle, 2, 3, 4),
                        5
                    ),
                    new OrderPosition(
                        FiguresFactory.Create(FigureType.Square, 1),
                        10
                    )
                }
            };

            var figuresStorageMock = new Mock<IFiguresStorage>();
            figuresStorageMock
                .Setup(x => x.CheckIfAvailable(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(true);

            var orderStorageMock = new Mock<IOrderStorage>();

            var orderService = new OrderService(figuresStorageMock.Object, orderStorageMock.Object);

            // Act
            await orderService.MakeOrder(order);

            // Assert
            figuresStorageMock.Verify(x => x.Reserve(FigureType.Circle.ToString(), 3));
            figuresStorageMock.Verify(x => x.Reserve(FigureType.Square.ToString(), 20));
            figuresStorageMock.Verify(x => x.Reserve(FigureType.Triangle.ToString(), 5));
        }

        [Test(Description = "При недоступности нужного количества возникает исключение")]
        public void MakeOrder_WhenPositionCountNotAvailable_ShouldThrowsException()
        {
            // Arrange
            var order = new Order
            {
                Positions = new List<OrderPosition>
                {
                    new OrderPosition(
                        FiguresFactory.Create(FigureType.Circle, 1),
                        1
                    )
                }
            };

            var figuresStorageMock = new Mock<IFiguresStorage>();
            figuresStorageMock
                .Setup(x => x.CheckIfAvailable(FigureType.Circle.ToString(), 1))
                .Returns(false);

            var orderStorageMock = new Mock<IOrderStorage>();

            var orderService = new OrderService(figuresStorageMock.Object, orderStorageMock.Object);

            // Act, Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => orderService.MakeOrder(order));
        }

        [Test(Description = "При возникновении ошибки при сохранении заказа происходит отмена резерва позиций заказа")]
        public void MakeOrder_WhenOrderSaveIsFailed_UndoReversePositions()
        {
            // Arrange
            var order = new Order
            {
                Positions = new List<OrderPosition>
                {
                    new OrderPosition(
                        FiguresFactory.Create(FigureType.Circle, 1),
                        5
                    ),
                    new OrderPosition(
                        FiguresFactory.Create(FigureType.Square, 1),
                        10
                    )
                }
            };

            var figuresStorageMock = new Mock<IFiguresStorage>();
            figuresStorageMock
                .Setup(x => x.CheckIfAvailable(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(true);

            var orderStorageMock = new Mock<IOrderStorage>();
            orderStorageMock
                .Setup(x => x.Save(order))
                .Throws<Exception>();

            var orderService = new OrderService(figuresStorageMock.Object, orderStorageMock.Object);

            // Act
            Assert.ThrowsAsync<Exception>(() => orderService.MakeOrder(order));

            // Assert
            figuresStorageMock.Verify(x => x.UndoReserve(FigureType.Circle.ToString(), 5));
            figuresStorageMock.Verify(x => x.UndoReserve(FigureType.Square.ToString(), 10));
        }
    }
}