﻿using devoctomy.Passchamp.Core.Graph;
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
        public void GivenName_AndSupportedValue_WhenCreate_ThenPinOfCorrectTypeCreated(
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
        [InlineData(float.MinValue)]
        [InlineData(double.MinValue)]
        [InlineData(byte.MinValue)]
        public void GivenName_AndUnsupportedValue_WhenCreate_ThenNotSupportedExceptionThrown(
            object value)
        {
            //Arrange

            //Act & Assert
            Assert.ThrowsAny<NotSupportedException>(() =>
            {
                var result = DataPinFactory.Instance.Create(
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

        [Theory]
        [InlineData(typeof(int))]
        [InlineData(typeof(bool))]
        [InlineData(typeof(string))]
        public void GivenName_AndUnsupportedGenericListValue_WhenCreate_ThenNotSupportedExceptionThrown(Type listType)
        {
            //Arrange
            var genericListType = typeof(List<>);
            var constructedListType = genericListType.MakeGenericType(listType);
            var value = Activator.CreateInstance(constructedListType);

            //Act & Assert
            Assert.ThrowsAny<NotSupportedException>(() =>
            {
                var result = DataPinFactory.Instance.Create(
                    "Test",
                    value);
            });
        }
    }
}
