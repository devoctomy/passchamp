using CommunityToolkit.Maui.Views;
using devoctomy.Passchamp.Client.ViewModels.Base;

namespace devoctomy.Passchamp.Client.Popups.Base
{
    public abstract class BasePopup<TViewModel> : BasePopup where TViewModel : BaseViewModel
    {
        public BasePopup(TViewModel viewModel) : base(viewModel)
        {
        }

        public new TViewModel BindingContext => (TViewModel)base.BindingContext;
    }

    public abstract class BasePopup : Popup
    {
        public BasePopup(object viewModel = null)
        {
            BindingContext = viewModel;
        }
    }
}
