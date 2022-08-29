namespace devoctomy.Passchamp.Maui.Services
{
    public interface IShellNavigationService
    {
        Task GoToAsync(ShellNavigationState shellNavigationState);
        Task GoBackAsync();
    }
}
