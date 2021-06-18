using System;
using System.Collections.Generic;
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
    /// WizardPage4.xaml 的交互逻辑
    /// </summary>
    public partial class WizardPage4 : Page
    {
        public WizardPage4(Vitals vitals)
        {
            InitializeComponent();

            this.runName.Text = vitals.Name;
            this.runHome.Text = vitals.Home;
            this.runGender.Text = vitals.Gender;
            this.runOS.Text = vitals.FavoriteOS;
            this.runDirectory.Text = vitals.Directory;
            this.runMomsMaidenName.Text = vitals.MomsMaidenName;
            this.runPet.Text = vitals.Pet;
            this.runIncome.Text = vitals.Income;
        }

        private void PreviousButtonOnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void SubmitButtonOnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Thank you!\n\nYou will be contacted by email in four to six months.", Application.Current.MainWindow.Title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            Application.Current.Shutdown();
        }
    }
}
