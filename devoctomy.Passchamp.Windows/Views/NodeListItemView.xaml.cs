using devoctomy.Passchamp.Core.Graph;
using System.Windows;
using System.Windows.Controls;

namespace devoctomy.Passchamp.Windows.Views
{
    public partial class NodeListItemView : UserControl
    {
        public static readonly DependencyProperty NodeProperty =
           DependencyProperty.Register(
               nameof(Node),
               typeof(INode),
               typeof(NodeListItemView),
               new PropertyMetadata(
                   null,
                   null));

        public INode Node
        {
            get => (INode)GetValue(NodeProperty);
            set => SetValue(NodeProperty, value);
        }

        public NodeListItemView()
        {
            InitializeComponent();
        }
    }
}
