using devoctomy.Passchamp.Windows.Extensions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;

namespace devoctomy.Passchamp.Windows.UnitTests.Extensions
{
    public class ListExtensionsTests
    {
        [Fact]
        public void GivenList_WhenToObservableCollection_ThenObservableCollectionReturned()
        {
            // Arrange
            var sut = new List<string>
            {
                "Hello",
                "World"
            };

            // Act
            var observableCollection = sut.ToObservableCollection();

            // Assert
            Assert.IsType<ObservableCollection<string>>(observableCollection);
            Assert.Equal(string.Join(",", sut), string.Join(",", observableCollection));
        }
    }
}
