using System.Windows.Forms;

namespace devoctomy.Passchamp.Views
{
    public interface IViewLocator
    {
        T CreateInstance<T>() where T : Form;
    }
}
