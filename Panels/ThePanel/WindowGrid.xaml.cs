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
using System.Windows.Shapes;

namespace ThePanel
{
    /// <summary>
    /// WindowGrid.xaml 的交互逻辑
    /// </summary>
    public partial class WindowGrid : Window
    {
        TextBox txtboxBegin, txtboxEnd;
        Label lblLifeYears;

        public WindowGrid()
        {
            InitializeComponent();

            Grid grid = new Grid();
            grid.ShowGridLines = true;

            for (int i = 0; i < 3; i++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(1, GridUnitType.Star);
                grid.RowDefinitions.Add(row);
            }

            for (int i = 0; i < 2; i++)
            {
                ColumnDefinition column = new ColumnDefinition();
                column.Width = new GridLength(1, GridUnitType.Star);
                grid.ColumnDefinitions.Add(column);
            }

            this.Content = grid;

            //第一个label
            Label label = new Label();
            label.Content = "Begin Date:";
            grid.Children.Add(label);
            Grid.SetRow(label, 0);  //同 label.SetValue(Grid.RowProperty, 0);  
            Grid.SetColumn(label, 0);

            //第二个label
            label = new Label();
            label.Content = "End Date:";
            grid.Children.Add(label);
            Grid.SetRow(label, 1);
            Grid.SetColumn(label, 0);

            //第三个label
            label = new Label();
            label.Content = "Life Year:";
            grid.Children.Add(label);
            Grid.SetRow(label, 2);
            Grid.SetColumn(label, 0);

            //计算结果的label
            lblLifeYears = new Label();
            grid.Children.Add(lblLifeYears);
            Grid.SetRow(lblLifeYears, 2);
            Grid.SetColumn(lblLifeYears, 1);

            //输入起始日期
            txtboxBegin = new TextBox();            
            txtboxBegin.PreviewTextInput += TxtboxEnd_PreviewTextInput;
            grid.Children.Add(txtboxBegin);
            Grid.SetRow(txtboxBegin, 0);
            Grid.SetColumn(txtboxBegin, 1);

            //输入结束日期
            txtboxEnd = new TextBox();            
            txtboxEnd.PreviewTextInput += TxtboxEnd_PreviewTextInput;
            grid.Children.Add(txtboxEnd);
            Grid.SetRow(txtboxEnd, 1);
            Grid.SetColumn(txtboxEnd, 1);

            //设定边界
            Thickness thick = new Thickness(5);
            grid.Margin = thick;
            foreach (Control ctrl in grid.Children)
            {
                ctrl.Margin = thick;
            }

            //设定文本焦点和驱动事件处理器
            txtboxBegin.Focus();
            txtboxEnd.Text = DateTime.Now.ToShortDateString();

            GridSplitter splitter = new GridSplitter();
            splitter.HorizontalAlignment = HorizontalAlignment.Stretch;
            splitter.VerticalAlignment = VerticalAlignment.Bottom;
            splitter.Height = 5;
            splitter.Background = Brushes.Orange;
            splitter.ShowsPreview = true;       //鼠标放开后，才会改变格子尺寸

            Grid.SetRow(splitter, 1);
            Grid.SetColumnSpan(splitter, 3);
            grid.Children.Add(splitter);
        }

        private void TxtboxEnd_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == "\r")     //回车键
            {
                DateTime dtBeg, dtEnd;

                if (DateTime.TryParse(txtboxBegin.Text, out dtBeg) && DateTime.TryParse(txtboxEnd.Text, out dtEnd))
                {
                    int iYear = dtEnd.Year - dtBeg.Year;
                    int iMonths = dtEnd.Month - dtBeg.Month;
                    int iDays = dtEnd.Day - dtBeg.Day;

                    if (iDays < 0)
                    {
                        iDays += DateTime.DaysInMonth(dtEnd.Year, 1 + (dtEnd.Month + 10) % 12);

                        iMonths -= 1;
                    }

                    if (iMonths < 0)
                    {
                        iMonths += 12;
                        iYear -= 1;
                    }                    

                    lblLifeYears.Content = 
                        string.Format("{0} year{1}, {2} month{3}, {4} day{5}",
                                        iYear, iYear == 1 ? "" : "s",
                                        iMonths, iMonths == 1 ? "" : "s",
                                        iDays, iDays == 1 ? "" : "s");

                }
                else
                {
                    lblLifeYears.Content = "";
                }
            }
        }

    }
}
