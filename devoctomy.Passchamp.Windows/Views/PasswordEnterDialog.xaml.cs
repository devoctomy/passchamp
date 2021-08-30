using Microsoft.Toolkit.Mvvm.Input;
using System.Security;
using System.Windows;
using System.Windows.Input;

namespace devoctomy.Passchamp.Windows.Views
{
    public partial class PasswordEnterDialog : Window
    {
        public ICommand Accept { get; }
        public SecureString Password { get; set; }

        public PasswordEnterDialog()
        {
            InitializeComponent();

            Accept = new RelayCommand(DoAccept);
            DataContext = this;
        }

        private void DoAccept()
        {
            Password = PasswordInput.SecurePassword;
            DialogResult = true;
            Close();
        }
    }
}
