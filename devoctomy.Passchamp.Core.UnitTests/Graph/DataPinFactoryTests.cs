using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Data;
using System;
using System.Collections.Generic;
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

        [Theory]
        [InlineData(typeof(DataParserSection))]
        public void GivenName_AndGenericListValue_WhenCreate_ThentPinOfCorrectTypeCreated(Type listType)
        {
            //Arrange
            var genericListType = typeof(List<>);
            var constructedListType = genericListType.MakeGenericType(listType);
            var value = Activator.CreateInstance(constructedListType);

            //Act
            var result = DataPinFactory.Instance.Create(
                "Test",
                value);

            //Assert
            var genericDataPinType = typeof(DataPin<>);
            var constructedDataPinType = genericDataPinType.MakeGenericType(constructedListType);
            Assert.Equal(constructedDataPinType, result.GetType());
        }
    }
}
