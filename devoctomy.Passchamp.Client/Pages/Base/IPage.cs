using devoctomy.Passchamp.Client.ViewModels.Base;


namespace devoctomy.Passchamp.Client.Pages.Base;

public interface IPage
{
    public bool TransientViewModel { get; set; }

    public BaseViewModel ViewModel { get; }

    public void ResetViewModel();
}
