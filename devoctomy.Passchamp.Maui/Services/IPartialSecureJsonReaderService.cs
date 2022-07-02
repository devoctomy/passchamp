namespace devoctomy.Passchamp.Maui.Services
{
    public interface IPartialSecureJsonReaderService
    {
        public Task<T> LoadAsync<T>(Stream stream);
    }
}