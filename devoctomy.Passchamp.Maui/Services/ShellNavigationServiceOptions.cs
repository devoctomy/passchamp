using System.Diagnostics.CodeAnalysis;

namespace devoctomy.Passchamp.Maui.Services;

[ExcludeFromCodeCoverage(Justification = "POCO")]
public class ShellNavigationServiceOptions
{
    public string HomeRoute { get; set; }
}
