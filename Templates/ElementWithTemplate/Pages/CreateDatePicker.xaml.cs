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

namespace ElementWithTemplate.Pages
{
    /// <summary>
    /// CreateDatePicker.xaml 的交互逻辑
    /// </summary>
    public partial class CreateDatePicker : Page
    {
        public CreateDatePicker()
        {
            InitializeComponent();
        }

        private void DatePickerOnChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            if (e.NewValue != null)
            {
                DateTime dt = (DateTime)e.NewValue;
                this.txtblkDate.Text = dt.ToString("d");
            }
            else
                this.txtblkDate.Text = string.Empty;
        }
    }
}
