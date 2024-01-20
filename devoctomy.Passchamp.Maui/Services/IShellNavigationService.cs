using System.Diagnostics.CodeAnalysis;

namespace devoctomy.Passchamp.Maui.Services;

public interface IShellNavigationService
{
    Task GoHomeAsync(bool clearStack);
    Task GoToAsync(ShellNavigationState shellNavigationState);
    Task GoBackAsync();
}
