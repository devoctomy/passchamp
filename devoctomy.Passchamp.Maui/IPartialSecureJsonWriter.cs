namespace devoctomy.Passchamp.Maui
{
    public interface IPartialSecureJsonWriter
    {
        public Task SaveAsync(
            object value,
            Stream stream);
    }
}
