using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ComputerDatingWizard
{
    /// <summary>
    /// WizardPage0.xaml 的交互逻辑
    /// </summary>
    public partial class WizardPage0 : Page
    {
        public WizardPage0()
        {
            InitializeComponent();
        }

        private void BeginButtonOnClick(object sender, RoutedEventArgs e)
        {
            //NavigationService获得的是主窗口中的Frame中Navigation
            //使用GoBack与GoFoward的优点是，页面已经被建立、加载，而且或许被用户修改过，使用这些方法导航会保留用户输入的数据

            if (NavigationService.CanGoForward)
                NavigationService.GoForward();
            else
            {
                Vitals vitals = new Vitals();
                WizardPage1 page = new WizardPage1(vitals);
                NavigationService.Navigate(page);
            }
        }
    }
}
