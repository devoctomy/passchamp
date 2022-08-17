using CommunityToolkit.Mvvm.Input;
using devoctomy.Passchamp.Client.ViewModels.Base;
using devoctomy.Passchamp.Client.ViewModels.Enums;

namespace devoctomy.Passchamp.Client.ViewModels
{
    public class VaultEditorViewModel : BaseViewModel
    {
        public BaseViewModel ReturnViewModel { get; set; }
        public IAsyncRelayCommand BackCommand { get; private set; }
        public IAsyncRelayCommand OkCommand { get; private set; }

        public VaultEditorViewModel(BaseViewModel returnViewModel)
        {
            AttachCommandHandlers();

            ReturnViewModel = returnViewModel;
        }

        private void AttachCommandHandlers()
        {
            BackCommand = new AsyncRelayCommand(BackCommandHandler);
            OkCommand = new AsyncRelayCommand(OkCommandHandler);
        }

        private async Task BackCommandHandler()
        {
            await ReturnViewModel.Return(null);
        }

        private async Task OkCommandHandler()
        {
            await ReturnViewModel.Return(this);
        }
    }
}
