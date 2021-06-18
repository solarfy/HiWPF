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
    /// WizardPage3.xaml 的交互逻辑
    /// </summary>
    public partial class WizardPage3 : Page
    {
        Vitals vitals;

        public WizardPage3(Vitals vitals)
        {
            InitializeComponent();
            this.vitals = vitals;
        }

        private void PreviousButtonOnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void FinishButtonOnClick(object sender, RoutedEventArgs e)
        {
            vitals.MomsMaidenName = this.txtboxMom.Text;
            vitals.Pet = Vitals.GetCheckedRadioButton(this.grpboxPet).Content as string;
            vitals.Income = Vitals.GetCheckedRadioButton(this.grpboxIncome).Content as string;

            WizardPage4 page = new WizardPage4(vitals); //这里没有使用GoForward方法，是为了调用一次WizardPage4里的构造函数。
            NavigationService.Navigate(page);
        }
    }
}
