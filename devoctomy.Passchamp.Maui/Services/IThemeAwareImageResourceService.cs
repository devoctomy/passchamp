namespace devoctomy.Passchamp.Maui.Services
{
    public interface IThemeAwareImageResourceService
    {
        public string[] SupportedThemes { get; }
        public string Get(string prefix);
    }
}
