using devoctomy.Passchamp.Core.Services;
using System;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Services
{
    public class TypeResolverServiceTests
    {
        [Theory]
        [InlineData("System.Int32", typeof(int))]
        [InlineData("devoctomy.Passchamp.Core:devoctomy.Passchamp.Core.Services.TypeResolverService", typeof(TypeResolverService))]
        public void GivenKnownType_WhenGetType_ThenCorrectTypeReturned(
            string typeName,
            Type expectedType)
        {
            // Arrange
            var sut = new TypeResolverService();

            // Act
            var type = sut.GetType(typeName);

            // Assert
            Assert.Equal(expectedType, type);
        }
    }
}
