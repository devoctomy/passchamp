namespace devoctomy.Passchamp.Maui.Services
{
    public class ThemeAwareImageResourceService : IThemeAwareImageResourceService
    {
        public string Get(string prefix)
        {
            return $"{prefix}_{Application.Current.RequestedTheme.ToString().ToLower()}.png";
        }
    }
}
