using System.Diagnostics.CodeAnalysis;

namespace devoctomy.Passchamp.Maui.Services
{
    [ExcludeFromCodeCoverage(Justification = "POCO")]
    public class ThemeAwareImageResourceServiceOptions
    {
        public string[] SupportedThemes { get; set; }
    }
}
