using System;
using System.Windows;
using System.Windows.Controls;

namespace NoRP_Example
{
    public partial class InputBox : Window
    {
        public string Text { get => TextBox.Text; set => TextBox.Text = value; }


        public InputBox() => InitializeComponent();

        private void Button_Click(object sender, RoutedEventArgs e) => DialogResult = true;
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            TextBox.SelectAll();
            TextBox.Focus();
        }
    }
}
