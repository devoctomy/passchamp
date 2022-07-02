namespace devoctomy.Passchamp.Maui
{
    public interface IPartialSecureJsonReader
    {
        public Task<T> LoadAsync<T>(Stream stream);
    }
}
