using devoctomy.Passchamp.Core.Graph.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Graph.Services
{
    public class PinsJsonParserServiceTests
    {
        [Theory]
        [InlineData(
            "Data/pins1.json",
            3,
            new[] { "Pin1", "Pin2", "Pin3" },
            new object[] { "Hello", null, 101 })]
        public async Task GivenJsonData_WhenParse_ThenPinsReturned(
            string fileName,
            int expectedPins,
            string[] expectedPinNames,
            object[] expectedPinValues)
        {
            // Arrange
            var jsonData = await System.IO.File.ReadAllTextAsync(
                fileName,
                CancellationToken.None);
            var pinsJson = JObject.Parse(jsonData);
            var sut = new PinsJsonParserService();

            // Act
            var result = sut.Parse(pinsJson["Pins"].Value<JArray>());

            // Assert
            Assert.Equal(expectedPins, result.Count);
            Assert.Equal(expectedPinNames, result.Select(x => x.Key).ToArray());
            var values = result.Select(x => x.Value.ObjectValue).ToArray();
            for (int i = 0; i < values.Length; i++)
            {
                Assert.Equal(expectedPinValues[i], values[i]);
            }
        }

        [Fact]
        public void GivenJsonData_AndUnknownType_WhenParse_ThenTypeLoadExceptionThrown()
        {
            // Arrange
            var jsonData = new JArray();
            var pin = new JObject();
            pin.Add("Type", new JValue("Pants"));
            jsonData.Add(pin);
            var sut = new PinsJsonParserService();

            // Act & Assert
            Assert.ThrowsAny<TypeLoadException>(() =>
            {
                sut.Parse(jsonData);
            });
        }

        [Fact]
        public void GivenJsonData_AndUnsupportedType_WhenParse_ThenNotSupportedExceptionThrown()
        {
            // Arrange
            var jsonData = new JArray();
            var pin = new JObject();
            pin.Add("Type", new JValue("System.Double"));
            jsonData.Add(pin);
            var sut = new PinsJsonParserService();

            // Act & Assert
            Assert.ThrowsAny<NotSupportedException>(() =>
            {
                sut.Parse(jsonData);
            });
        }
    }
}
