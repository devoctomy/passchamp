using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Data;
using System;
using System.Collections.Generic;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Graph;

public class DataPinFactoryTests
{
    [Theory]
    [InlineData(0, typeof(DataPin<int>))]
    [InlineData("Hello", typeof(DataPin<string>))]
    [InlineData(false, typeof(DataPin<bool>))]
    [InlineData(new[]{ (byte)0, (byte)1, (byte)2 }, typeof(DataPin<byte[]>))]
    [InlineData(110.5d, typeof(DataPin<double>))]
    [InlineData(110.5f, typeof(DataPin<Single>))]
    public void GivenName_AndSupportedValue_WhenCreate_ThenPinOfCorrectTypeCreated_AndValueCorrect(
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
        Assert.Equal(value, result.ObjectValue);
    }

    [Theory]
    [InlineData(byte.MinValue)]
    public void GivenName_AndUnsupportedValue_WhenCreate_ThenNotSupportedExceptionThrown(
        object value)
    {
        //Arrange

        //Act & Assert
        Assert.ThrowsAny<NotSupportedException>(() =>
        {
            _ = DataPinFactory.Instance.Create(
                "Test",
                value);
        });
    }

    [Theory]
    [InlineData(typeof(DataParserSection))]
    public void GivenName_AndSupportedGenericListValue_WhenCreate_ThenPinOfCorrectTypeCreated(Type listType)
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

    [Fact]
    public void GivenName_AndNull_WhenCreate_ThenArgumentExceptionThrown()
    {
        // Arrange

        // Act & Assert
        Assert.ThrowsAny<ArgumentException>(() =>
        {
            DataPinFactory.Instance.Create(
                "Test",
                null);
        });
    }
}
