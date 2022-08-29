namespace devoctomy.Passchamp.Maui.Services
{
    public class ShellNavigationService : IShellNavigationService
    {
        private readonly ShellNavigationServiceOptions _options;

        private Stack<ShellNavigationState> _stateStack;

        public ShellNavigationService(ShellNavigationServiceOptions options)
        {
            _options = options;
            _stateStack = new Stack<ShellNavigationState>();
        }

        public async Task GoBackAsync()
        {
            _stateStack.Pop();
            if(_stateStack.Count == 0)
            {
                await Shell.Current.GoToAsync(_options.HomeRoute);
            }
            else
            {
                var state = _stateStack.Peek();
                await Shell.Current.GoToAsync(state);
            }
        }

        public async Task GoToAsync(ShellNavigationState state)
        {
            _stateStack.Push(state);
            await Shell.Current.GoToAsync(state);
        }
    }
}
