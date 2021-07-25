using devoctomy.Passchamp.Core.Graph;
using System;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Graph
{
    public class DataPinTests
    {
        [Fact]
        public void GivenDataPin_AndStringValue_WhenGetStringValue_ThenValueReturned()
        {
            // Arrange
            var value = "Hello World!";
            var sut = new DataPin("Test", value);

            // Act
            var result = sut.GetValue<string>();

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        public void GivenDataPin_AndIntValue_WhenGetStringValue_ThenInvalidCastExceptionThrown()
        {
            // Arrange
            var value = 100;
            var sut = new DataPin("Test", value);

            // Act
            Assert.ThrowsAny<InvalidCastException>(() =>
            {
                var result = sut.GetValue<string>();
            });
        }
    }
}
