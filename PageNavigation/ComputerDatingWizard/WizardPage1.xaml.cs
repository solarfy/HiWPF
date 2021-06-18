using System.Windows;
using System.Windows.Controls;

namespace ComputerDatingWizard
{
    /// <summary>
    /// WizardPage1.xaml 的交互逻辑
    /// </summary>
    public partial class WizardPage1 : Page
    {
        Vitals vitals;

        public WizardPage1(Vitals vitals)
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
            vitals.Name = this.txtboxName.Text;
            vitals.Home = Vitals.GetCheckedRadioButton(this.grpboxHome).Content as string;
            vitals.Gender = Vitals.GetCheckedRadioButton(this.grpboxGender).Content as string;

            if (NavigationService.CanGoForward)
                NavigationService.GoForward();
            else
            {
                WizardPage2 page = new WizardPage2(vitals);
                NavigationService.Navigate(page);
            }
        }
    }
}
