using System.Windows;

namespace ComputerDatingWizard
{
    /// <summary>
    /// ComputerDatingWizard.xaml 的交互逻辑
    /// </summary>
    public partial class ComputerDatingWizardWindow : Window
    {
        public ComputerDatingWizardWindow()
        {
            InitializeComponent();

            this.frame.Navigate(new WizardPage0());
        }
    }
}
