using devoctomy.Passchamp.SignTool.Services.CommandLineParser;
using Xunit;

namespace devoctomy.Passchamp.SignTool.UnitTests.Services.CommandLineParser
{
    public class PropertyValueSetterServiceTests
    {
        [Theory]
        [InlineData("StringValue", "Bob Hoskins")]
        [InlineData("IntValue", "199")]
        [InlineData("BoolValue", "True")]
        [InlineData("FloatValue", "1.4")]
        public void GivenOptionsInstance_AndProperty_AndValue_WhenSetPropertyValue_ThenPropertyValueSet(
            string propertyName,
            string value)
        {
            // Arrange
            var optionsInstance = new CommandLineTestOptions();
            var property = typeof(CommandLineTestOptions).GetProperty(propertyName);
            var sut = new PropertyValueSetterService();

            // Act
            sut.SetPropertyValue(
                optionsInstance,
                property,
                value);

            // Assert
            Assert.Equal(property.GetValue(optionsInstance).ToString(), value);
        }
    }
}
