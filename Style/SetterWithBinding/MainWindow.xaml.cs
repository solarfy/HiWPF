using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SetterWithBinding
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenDocumentOnClick(object sender, RoutedEventArgs e)
        {           
            NavigationWindow win = new NavigationWindow();
            win.Navigate(new DocumentStylesPage());         
            win.Show();
        }

        private void ButtonOnClick(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            MessageBox.Show($"The button labled {btn.Content} has been clicked.", this.Title);
        }
    }
}
