using devoctomy.Passchamp.Maui.Converters;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Maui.UnitTests.Converters
{
    public class DoubleToGridLengthConverterTests
    {

        [Fact]
        public void GivenAnyParams_AndConvertBack_ThenNotImplementedExceptionThrown()
        {
            // Arrange
            var sut = new DoubleToGridLengthConverter();

            // Act & Assert
            Assert.ThrowsAny<NotImplementedException>(() =>
            {
                sut.ConvertBack(
                    null,
                    null,
                    null,
                    null);
            });
        }

        [Theory]
        [InlineData(100d, "Absolute", 100d, "Absolute")]
        [InlineData(200d, "Auto", 0d, "Auto")]
        [InlineData(300d, "Star", 0d, "Star")]
        public void GivenDouble_AndUnit_WhenConvert_ThenCorrectGridSizeReturned(
            double value,
            string unit,
            double expectedValue,
            string expectedUnit)
        {
            // Arrange
            var sut = new DoubleToGridLengthConverter();

            // Act
            var result = (GridLength)sut.Convert(value, typeof(GridLength), unit, null);

            // Assert
            Assert.Equal(expectedValue, result.Value);
            Assert.Equal(Enum.Parse(typeof(GridUnitType), expectedUnit, true), result.GridUnitType);
        }
    }
}
