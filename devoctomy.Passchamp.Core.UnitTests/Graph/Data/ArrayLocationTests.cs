using devoctomy.Passchamp.Core.Graph.Data;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Graph.Data
{
    public class ArrayLocationTests
    {
        [Theory]
        [InlineData(Offset.Absolute, 10, 100, 10)]
        [InlineData(Offset.Absolute, 0, 100, 0)]
        [InlineData(Offset.FromEnd, 10, 100, 90)]
        [InlineData(Offset.FromEnd, 0, 100, 100)]
        public void GivenOffset_AndCount_AndDataLength_WhenGetIndex_ThenExpectedIndexReturned(
            Offset offset,
            int count,
            int dataLength,
            int expectedIndex)
        {
            // Arrange
            var sut = new ArrayLocation(
                offset,
                count);

            // Act
            var result = sut.GetIndex(dataLength);

            // Assert
            Assert.Equal(expectedIndex, result);
        }
    }
}
