using devoctomy.Passchamp.Core.Graph;
using System.Windows;
using System.Windows.Controls;

namespace devoctomy.Passchamp.Windows.Views
{
    public partial class DataPinView : UserControl
    {
        public static readonly DependencyProperty DataPinProperty =
                   DependencyProperty.Register(
                       nameof(DataPin),
                       typeof(IPin),
                       typeof(DataPinView),
                       new PropertyMetadata(
                           null,
                           null));

        public IPin DataPin
        {
            get => (IPin)GetValue(DataPinProperty);
            set => SetValue(DataPinProperty, value);
        }

        public DataPinView()
        {
            InitializeComponent();
        }
    }
}