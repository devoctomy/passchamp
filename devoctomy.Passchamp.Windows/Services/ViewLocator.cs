using devoctomy.Passchamp.Windows.Views;

namespace devoctomy.Passchamp.Windows.Services
{
    public class ViewLocator : IViewLocator
    {
        public GraphTester GraphTester => new GraphTester();
    }
}
