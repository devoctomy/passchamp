using devoctomy.Passchamp.Models;

namespace devoctomy.Passchamp.ViewModels
{
    public class ViewModelBase : ObservableObject
    {
        public ModelBase Model { get; init; }

        public ViewModelBase(ModelBase model)
        {
            Model = model;
        }
    }
}
