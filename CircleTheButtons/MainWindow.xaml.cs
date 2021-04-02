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

namespace CircleTheButtons
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Title = "Circle the Buttons";
            Grid grid = new Grid();
            ColumnDefinition col = new ColumnDefinition();
            col.Width = new GridLength(1, GridUnitType.Star);
            grid.ColumnDefinitions.Add(col);
            col = new ColumnDefinition();
            col.Width = new GridLength(1, GridUnitType.Star);
            grid.ColumnDefinitions.Add(col);
            this.Content = grid;

            RadialPanel pnl1 = new RadialPanel();
            pnl1.Orientation = RadialPanelOrientation.ByHeight;
            pnl1.ShowPieLines = true;
            RadialPanel pnl2 = new RadialPanel();
            pnl2.Orientation = RadialPanelOrientation.ByWidth;
            pnl2.ShowPieLines = true;

            grid.Children.Add(pnl1);
            Grid.SetColumn(pnl2, 0);
            grid.Children.Add(pnl2);
            Grid.SetColumn(pnl2, 1);

            Random rand = new Random();
            for (int i = 0; i < 10; i++)
            {
                Button btn1 = new Button();
                btn1.Content = $"Button Number {i + 1}";
                btn1.FontSize += rand.Next(10);
                pnl1.Children.Add(btn1);

                Button btn2 = new Button();
                btn2.Content = $"Button Number {i + 1}";
                btn2.FontSize += rand.Next(10);
                pnl2.Children.Add(btn2);
            }
        }
    }
}
