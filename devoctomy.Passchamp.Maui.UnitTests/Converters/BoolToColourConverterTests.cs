using devoctomy.Passchamp.Maui.Converters;
using Microsoft.Maui.Graphics;
using System;

namespace devoctomy.Passchamp.Maui.UnitTests.Converters
{
    public class BoolToColourConverterTests
    {
        [Theory]
        [InlineData(false, "Black,White", "#FF000000")]
        [InlineData(true, "Black,White", "#FFFFFFFF")]
        [InlineData(false, "Apple,Orange", "#FF000000")]
        [InlineData(true, "Apple,Orange", "#FFFFFFFF")]
        public void GivenBool_AndColourNames_WhenConvert_ThenCorrectColourReturned(
            bool value,
            string colourNames,
            string expectedColourARGBHex)
        {
            // Arrange
            var resources = new Microsoft.Maui.Controls.ResourceDictionary
            {
                { "Apple", Colors.Black },
                { "Orange", Colors.White }
            };
            var sut = BoolToColourConverter.CreateInstance(resources);

            // Act
            var result = sut.Convert(value, typeof(Color), colourNames, null) as Color;

            // Assert
            Assert.Equal(expectedColourARGBHex, result.ToArgbHex(true));
        }

        [Fact]
        public void GivenAnyParams_AndConvertBack_ThenNotImplementedExceptionThrown()
        {
            // Arrange
            var sut = new BoolToColourConverter();

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
    }
}
