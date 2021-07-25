using devoctomy.Passchamp.Core.Graph;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Graph
{
    public class DataPinTests
    {
        [Fact]
        public void GivenDataPin_WhenValueType_ThenCorrectTypeReturned()
        {
            // Arrange
            var sut = new DataPin<string>(
                "Test",
                "Hello World");

            // Act
            var valueType = sut.ValueType;

            // Assert
            Assert.Equal(typeof(string), valueType);
        }
    }
}
