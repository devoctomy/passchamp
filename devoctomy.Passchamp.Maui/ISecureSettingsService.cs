namespace devoctomy.Passchamp.Maui
{
    public interface ISecureSettingsService
    {
        Task Load();
        Task Save();
    }
}
