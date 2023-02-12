namespace devoctomy.Passchamp.Maui.Services
{
    public class ThemeAwareImageResourceService : IThemeAwareImageResourceService
    {
        public string[] SupportedThemes => _options.SupportedThemes;

        private readonly ThemeAwareImageResourceServiceOptions _options;

        public ThemeAwareImageResourceService(ThemeAwareImageResourceServiceOptions options)
        {
            _options = options;
        }

        public string Get(string prefix)
        {
            var requestedTheme = Application.Current.RequestedTheme.ToString().ToLower();
            if(SupportedThemes.Contains(requestedTheme))
            {
                return $"{prefix}_{requestedTheme}.png";
            }

            return $"{prefix}.png";
        }
    }
}
