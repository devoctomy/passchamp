using devoctomy.Passchamp.Core.Data;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Data;

public class IOServiceTests
{
    [Fact]
    public void GivenValidPath_WhenDelete_TheFileDeleted()
    {
        // Arrange
        var file = Path.GetTempFileName();
        var sut = new IOService();

        // Act
        sut.Delete(file);

        // Assert
        Assert.False(File.Exists(file));
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public void GivenPath_WhenExists_TheCorrectValueReturned(
        bool deleteBeforeTest,
        bool expectedResult)
    {
        // Arrange
        var file = Path.GetTempFileName();
        if(deleteBeforeTest)
        {
            File.Delete(file);
        }

        var sut = new IOService();

        // Act
        _ = sut.Exists(file);

        // Assert
        Assert.Equal(expectedResult, File.Exists(file));

        if (!deleteBeforeTest)
        {
            File.Delete(file);
        }
    }

    [Fact]
    public void GivenValidPath_WhenOpenRead_TheReadOnlyStreamReturned()
    {
        // Arrange
        var file = Path.GetTempFileName();
        var sut = new IOService();

        // Act
        using var stream = sut.OpenRead(file);

        // Assert
        Assert.True(stream.CanRead);
        Assert.False(stream.CanWrite);

        stream.Close();
        File.Delete(file);
    }

    [Fact]
    public void GivenPath_AndFileNotExists_WhenOpenNewRead_TheFileCreated_ReadWriteStreamReturned()
    {
        // Arrange
        var file = Path.GetTempFileName();
        File.Delete(file);
        var sut = new IOService();

        // Act
        using var stream = sut.OpenNewWrite(file);

        // Assert
        Assert.True(File.Exists(file));
        Assert.True(stream.CanRead);
        Assert.True(stream.CanWrite);

        stream.Close();
        File.Delete(file);
    }


    [Fact]
    public async Task GivenPath_AndFileExists_WhenReadAllTextAsync_TheCorrectFileContentsReturned()
    {
        // Arrange
        var file = Path.GetTempFileName();
        System.IO.File.WriteAllText(file, "Hello World!");
        var sut = new IOService();

        // Act
        var result = await sut.ReadAllTextAsync(
            file,
            CancellationToken.None);

        // Assert
        Assert.Equal("Hello World!", result);

        File.Delete(file);
    }

    [Fact]
    public async Task GivenValidPath_AndStringData_WhenWriteDataAsync_TheReadOnlyStreamReturned()
    {
        // Arrange
        var file = Path.GetTempFileName();
        var data = "Hello World!";
        var sut = new IOService();

        // Act
        await sut.WriteDataAsync(
            file,
            data,
            CancellationToken.None);

        // Assert
        Assert.Equal(data, System.IO.File.ReadAllText(file));

       File.Delete(file);
    }

    [Fact]
    public async Task GivenValidPath_AndByteData_WhenWriteDataAsync_TheReadOnlyStreamReturned()
    {
        // Arrange
        var file = Path.GetTempFileName();
        var data = System.Text.Encoding.UTF8.GetBytes("Hello World!");
        var sut = new IOService();

        // Act
        await sut.WriteDataAsync(
            file,
            data,
            CancellationToken.None);

        // Assert
        Assert.Equal(System.Text.Encoding.UTF8.GetString(data), System.IO.File.ReadAllText(file));

        File.Delete(file);
    }

    [Fact]
    public void GivenPath_WhenCreatePathDirectory_ThenPathDirectoryCreated()
    {
        // Arrange
        var tempPath = Path.GetTempPath();
        var tempFile = Path.Combine(tempPath, "apple/orange/pear/something.bin");
        var sut = new IOService();

        // Act
        sut.CreatePathDirectory(tempFile);

        // Assert
        Assert.True(Directory.Exists(Path.Combine(tempPath, "apple/orange/pear/")));
        Directory.Delete(Path.Combine(tempPath, "apple/"), true);
    }
}
