using System;
using System.Windows;
using System.Windows.Controls;

namespace XamlCruncher
{
    /// <summary>
    /// X.xaml 的交互逻辑
    /// </summary>
    public partial class XamlTabSpacesDialog
    {
        public XamlTabSpacesDialog()
        {
            InitializeComponent();
            this.txtbox.Focus();
        }

        public int TabSpaces
        {
            set => this.txtbox.Text = value.ToString();
            get => Int32.Parse(this.txtbox.Text);
        }

        void TextBoxOnTextChanged(object sender, TextChangedEventArgs args)
        {
            int result;
            this.btnOK.IsEnabled = (Int32.TryParse(txtbox.Text, out result) && result > 0 && result < 11);
        }

        void OkOnClick(object sender, RoutedEventArgs args)
        {
            DialogResult = true;
        }
    }
}
