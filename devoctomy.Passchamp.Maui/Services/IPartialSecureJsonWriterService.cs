namespace devoctomy.Passchamp.Maui.Services
{
    public interface IPartialSecureJsonWriterService
    {
        public Task SaveAsync(
            object value,
            Stream stream);
    }
}
