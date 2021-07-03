using devoctomy.Passchamp.Binding;
using devoctomy.Passchamp.Models;
using System.Windows.Forms;
using System.Windows.Input;

namespace devoctomy.Passchamp.ViewModels
{
    public class GraphTesterViewModel : ViewModelBase
    {
        private RelayCommand _browseCommand;

        public ICommand BrowseCommand
        {
            get
            {
                _browseCommand = _browseCommand ?? new RelayCommand(param => Browse());
                return _browseCommand;
            }
        }

        public GraphTesterViewModel(GraphTesterModel model)
            : base(model)
        {
        }

        private void Browse()
        {
            MessageBox.Show("Browse!");
        }
    }
}
