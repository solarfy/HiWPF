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

namespace DiagonalizeTheButtons
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Title = "Diagonalize the Buttons";
            DiagonalizePanel pnl = new DiagonalizePanel();
            this.Content = pnl;

            Random rand = new Random();

            for (int i = 0; i < 5; i++)
            {
                Button btn = new Button();
                btn.Content = $"Button Number {i + 1}";
                btn.FontSize += rand.Next(20);
                pnl.Add(btn);
            }
        }
    }
}
