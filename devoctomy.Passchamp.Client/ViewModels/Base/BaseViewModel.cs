using CommunityToolkit.Mvvm.ComponentModel;

namespace devoctomy.Passchamp.Client.ViewModels.Base
{
    [INotifyPropertyChanged]
    public abstract partial class BaseViewModel
    {
        public virtual Task Return(BaseViewModel fromViewModel)
        {
            throw new NotImplementedException();
        }
    }
}
