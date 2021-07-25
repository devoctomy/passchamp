using System;
using System.Windows;
using System.Windows.Controls;

namespace devoctomy.Passchamp.Windows.Views
{
    public partial class DataPinView : UserControl
    {
        public static readonly DependencyProperty PinNameProperty =
                   DependencyProperty.Register(
                       nameof(PinName),
                       typeof(string),
                       typeof(DataPinView),
                       new PropertyMetadata(
                           "",
                           null));

        public string PinName
        {
            get => (string)GetValue(PinNameProperty);
            set => SetValue(PinNameProperty, value);
        }

        public DataPinView()
        {
            InitializeComponent();
        }
    }
}