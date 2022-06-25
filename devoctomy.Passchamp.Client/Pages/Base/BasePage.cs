using devoctomy.Passchamp.Client.ViewModels.Base;
using System.Diagnostics;

namespace devoctomy.Passchamp.Client.Pages.Base
{
    public abstract class BasePage<TViewModel> : BasePage where TViewModel : BaseViewModel
    {
        public BasePage(TViewModel viewModel) : base(viewModel)
        {
        }

        public new TViewModel BindingContext => (TViewModel)base.BindingContext;
    }

    public abstract class BasePage : ContentPage
    {
        public BasePage(object viewModel = null)
        {
            BindingContext = viewModel;
            Padding = 12;

            SetDynamicResource(BackgroundColorProperty, "AppBackgroundColor");

            if (string.IsNullOrWhiteSpace(Title))
            {
                Title = GetType().Name;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Debug.WriteLine($"OnAppearing: {Title}");
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            Debug.WriteLine($"OnDisappearing: {Title}");
        }
    }
}
