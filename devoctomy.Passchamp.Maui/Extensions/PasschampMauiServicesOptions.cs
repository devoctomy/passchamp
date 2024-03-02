using devoctomy.Passchamp.Maui.Data;
using devoctomy.Passchamp.Maui.Services;

namespace devoctomy.Passchamp.Maui.Extensions;

public class PasschampMauiServicesOptions
{
    public VaultIndexLoaderServiceOptions VaultLoaderServiceOptions { get; set; }
    public ShellNavigationServiceOptions ShellNavigationServiceOptions { get; set; }
    public ThemeAwareImageResourceServiceOptions ThemeAwareImageResourceServiceOptions { get; set; }
}
