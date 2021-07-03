using System.Windows.Forms;

namespace devoctomy.Passchamp.Services
{
    public interface IViewLocator
    {
        T CreateInstance<T>() where T : Form;
    }
}
