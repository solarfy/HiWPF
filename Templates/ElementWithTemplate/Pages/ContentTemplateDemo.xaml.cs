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
using ElementWithTemplate.Views;
using ElementWithTemplate.Models;

namespace ElementWithTemplate.Pages
{
    /// <summary>
    /// ContentTemplateDemo.xaml 的交互逻辑
    /// </summary>
    public partial class ContentTemplateDemo : Page
    {
        public ContentTemplateDemo()
        {
            InitializeComponent();

            EmployeeButton btn = new EmployeeButton();
            btn.Content = new Employee("Jim", "Resources/face.png", new DateTime(1991, 11, 11), false);
            this.stack.Children.Add(btn);
        }

        private void EmployeeButtonOnClick(object sender, RoutedEventArgs e)
        {
            Button btn = e.Source as Button;
            Employee emp = btn.Content as Employee;
            MessageBox.Show($"{emp.Name} button clicked", this.Title);
        }
    }
}
