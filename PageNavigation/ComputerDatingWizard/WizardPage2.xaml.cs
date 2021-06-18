using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ComputerDatingWizard
{
    /// <summary>
    /// WizardPage2.xaml 的交互逻辑
    /// </summary>
    public partial class WizardPage2 : Page
    {
        Vitals vitals;
        
        public WizardPage2(Vitals vitals)
        {
            InitializeComponent();
            this.vitals = vitals;
        }

        private void PreviousButtonOnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void NextButtonOnClick(object sender, RoutedEventArgs e)
        {
            vitals.FavoriteOS = this.txtboxFavoriteOS.Text;
            vitals.Directory = txtboxFavoriteDir.Text;

            if (NavigationService.CanGoForward)
                NavigationService.GoForward();
            else
            {
                WizardPage3 page = new WizardPage3(vitals);
                NavigationService.Navigate(page);
            }
        }

        private void BrowseButtonOnClick(object sender, RoutedEventArgs e)
        {
            DirectoryPage page = new DirectoryPage();
            page.Return += DirPageOnReturn;
            NavigationService.Navigate(page);       //PageFunction完成之后，因为RemoveFromJournal属性默认是ture，所以此PageFunction会自动把自己从路径中移除。
        }

        private void DirPageOnReturn(object sender, ReturnEventArgs<DirectoryInfo> e)
        {
            if (e.Result != null)
                this.txtboxFavoriteDir.Text = e.Result.FullName;
        }
    }
}
