using Figures.Data.Redis;
using Figures.Data.Storages;
using Moq;
using NUnit.Framework;

namespace Figures.Tests.Unit.Storages
{
    [TestFixture]
    public class FiguresStorageTests
    {
        [TestCase(10, 5, ExpectedResult = true)]
        [TestCase(10, 10, ExpectedResult = true)]
        [TestCase(10, 11, ExpectedResult = false)]
        public bool CheckIfAvailable_TestCases(int availableCount, int count)
        {
            // Arrange
            var redisClientMock = new Mock<IRedisClient>();
            redisClientMock
                .Setup(x => x.Get("Key"))
                .Returns(availableCount);

            var figuresStorage = new FiguresStorage(redisClientMock.Object);

            // Act, Assert
            return figuresStorage.CheckIfAvailable("Key", count);
        }

        [Test]
        public void Reserve_ShouldBeDecrementCurrentValue()
        {
            // Arrange
            var redisClientMock = new Mock<IRedisClient>();
            redisClientMock
                .Setup(x => x.Get("Key"))
                .Returns(10);

            var figuresStorage = new FiguresStorage(redisClientMock.Object);

            // Act
            figuresStorage.Reserve("Key", 4);

            // Assert
            redisClientMock.Verify(x => x.Set("Key", 6));
        }
    }
}