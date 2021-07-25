using devoctomy.Passchamp.Core.Graph;
using System;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Graph
{
    public class DataPinFactoryTests
    {
        [Theory]
        [InlineData(0, typeof(DataPin<int>))]
        [InlineData("Hello", typeof(DataPin<string>))]
        [InlineData(false, typeof(DataPin<bool>))]
        [InlineData(new[]{ (byte)0, (byte)1, (byte)2 }, typeof(DataPin<byte[]>))]
        public void GivenName_AndValue_WhenCreate_ThentPinOfCorrectTypeCreated(
            object value,
            Type dataPinType)
        {
            //Arrange

            //Act
            var result = DataPinFactory.Instance.Create(
                "Test",
                value);

            //Assert
            Assert.Equal(dataPinType, result.GetType());
        }
    }
}
