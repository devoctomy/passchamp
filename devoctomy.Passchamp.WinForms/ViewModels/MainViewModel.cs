using devoctomy.Passchamp.Dialogs;
using devoctomy.Passchamp.Models;
using devoctomy.Passchamp.Services;
using Microsoft.Extensions.Logging;
using System.Windows.Forms;

namespace devoctomy.Passchamp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ILogger<MainViewModel> Logger { get; }
        public IViewLocator ViewLocator { get; }

        public MainViewModel(
            ILogger<MainViewModel> logger,
            IViewLocator viewLocator,
            MainModel mainModel)
            : base(mainModel)
        {
            Logger = logger;
            ViewLocator = viewLocator;
        }

        public DialogResult DisplayGraphTesterDialog()
        {
            using var dialog = ViewLocator.CreateInstance<GraphTesterDialog>();
            return dialog.ShowDialog();
        }
    }
}
