namespace devoctomy.Passchamp.Client
{
    public partial class App : Application
    {
        public App()
        {
            UserAppTheme = Application.Current.RequestedTheme;
            InitializeComponent();
            MainPage = new AppShell();
        }
    }
}