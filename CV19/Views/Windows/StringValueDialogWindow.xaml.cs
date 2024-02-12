using System.Windows;
using System.Windows.Controls;

namespace CV19.Views.Windows
{
    public partial class StringValueDialogWindow : Window
    {
        public string Message { get => MessageTextBlock.Text; set => MessageTextBlock.Text = value; }

        public string Value { get => ValueTextBox.Text; set => ValueTextBox.Text = value; }

        public StringValueDialogWindow() => InitializeComponent();

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            if (!(e.Source is Button button)) return;
            DialogResult = !button.IsCancel;
            Close();
        }
    }
}
