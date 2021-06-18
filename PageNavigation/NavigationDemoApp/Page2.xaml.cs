using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace NavigationDemoApp
{
    /// <summary>
    /// Page2.xaml 的交互逻辑
    /// </summary>
    public partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
        }

        private void ButtonOnClick(object sender, RoutedEventArgs e)
        {
            //NavigationService 此Page所在的NavigationWindow的导航能力
            //若想获得NaviagationWindow的对象，可通过Application.Current.MainWindow属性，前提是要先设置该窗口为主窗口
            //若想在类中进行导航而不是在Page中，可通过NavigationService.GetNavigationService获得一个NavigationSercie对象，但是此类必须在NavigationWindow或Frame内的某个地方调用Navigate
            NavigationService.Navigate(new Uri("Page1.xaml", UriKind.Relative));
        }

        private void HyperlinkOnRequentNavigate(object sender, RequestNavigateEventArgs e)
        {
            NavigationService.Navigate(e.Uri);
            e.Handled = true;   //已经进行过导航，不需要再做任何事了
        }
    }
}
