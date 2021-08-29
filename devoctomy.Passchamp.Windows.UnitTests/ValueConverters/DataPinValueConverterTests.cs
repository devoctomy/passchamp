using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Windows.ValueConverters;
using Xunit;

namespace devoctomy.Passchamp.Windows.UnitTests.ValueConverters
{
    public class DataPinValueConverterTests
    {
        [Theory]
        [InlineData(123456, "123456")]
        [InlineData(true, "True")]
        [InlineData(false, "False")]
        [InlineData(1.23456f, "1.23456")]
        [InlineData(1.23456d, "1.23456")]
        public void GivenDataPin_WhenConvert_ThenCorrectValueReturned(
            object value,
            string returnValue)
        {
            // Arrange
            var dataPin = DataPinFactory.Instance.Create("Test", value);
            var sut = new DataPinValueConverter();

            // Act
            var result = sut.Convert(
                dataPin.ObjectValue,
                null,
                null,
                null);

            // Assert
            Assert.Equal(returnValue, result);
        }

        [Theory]
        [InlineData(123456, "123456")]
        [InlineData(true, "True")]
        [InlineData(false, "False")]
        [InlineData(1.23456f, "1.23456")]
        [InlineData(1.23456d, "1.23456")]
        public void GivenDataPin_WhenConvertBack_ThenCorrectValueReturned(
            object value,
            string returnValue)
        {
            // Arrange
            var dataPin = DataPinFactory.Instance.Create("Test", value);
            var sut = new DataPinValueConverter();

            // Act
            var result = sut.ConvertBack(
                returnValue,
                value.GetType(),
                null,
                null);

            // Assert
            Assert.Equal(value, result);
        }
    }
}
