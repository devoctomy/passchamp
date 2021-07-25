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

        [Fact]
        public void GivenName_AndValue_AndValueType_AndValueTypeDiffersFromTypeOfValue_WhenConstruct_ThenArgumentExceptionThrown()
        {
            // Arrange
            var value = 100;
            var valueTye = typeof(string);

            // Act
            Assert.ThrowsAny<ArgumentException>(() =>
            {
                var result = new DataPin("Test", value, valueTye);
            });
        }

        [Fact]
        public void GivenName_AndValue_AndValueIsNull_WhenConstruct_ThenNullReferenceExceptionThrown()
        {
            // Arrange
            object value = null;

            // Act
            Assert.ThrowsAny<NullReferenceException>(() =>
            {
                var result = new DataPin("Test", value);
            });
        }
    }
}
