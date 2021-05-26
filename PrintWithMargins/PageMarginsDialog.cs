using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace PrintWithMargins
{
    class PageMarginsDialog : Window
    {
        enum Side { Left, Right, Top, Bottom};

        //进行数字输入的4个TextBox
        TextBox[] txtbox = new TextBox[4];
        Button btnOK;

        //代表页面边界的Thickness property
        public Thickness PagesMargins
        {
            set
            {
                txtbox[(int)Side.Left].Text = (value.Left / 96).ToString("F3");
                txtbox[(int)Side.Right].Text = (value.Left / 96).ToString("F3");
                txtbox[(int)Side.Top].Text = (value.Left / 96).ToString("F3");
                txtbox[(int)Side.Bottom].Text = (value.Left / 96).ToString("F3");
            }

            get
            {
                return new Thickness(Double.Parse(txtbox[(int)Side.Left].Text) * 96
                    , Double.Parse(txtbox[(int)Side.Right].Text) * 96
                    , Double.Parse(txtbox[(int)Side.Top].Text) * 96
                    , Double.Parse(txtbox[(int)Side.Bottom].Text) * 96);
            }
        }

        public PageMarginsDialog()
        {
            this.Title = "Page Setup";
            this.ShowInTaskbar = false;
            this.WindowStyle = WindowStyle.ToolWindow;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            this.SizeToContent = SizeToContent.WidthAndHeight;
            this.ResizeMode = ResizeMode.NoResize;

            //建立StackPanel，作为窗口的内容
            StackPanel stack = new StackPanel();
            this.Content = stack;

            //建立GroupBox，作为StackPanel的孩子
            GroupBox grpbox = new GroupBox();
            grpbox.Header = "Margins (inches)";
            grpbox.Margin = new Thickness(12);
            stack.Children.Add(grpbox);

            //建立Grid，作为GroupBox的内容
            Grid grid = new Grid();
            grid.Margin = new Thickness(6);
            grpbox.Content = grid;

            //2行，4列
            for (int i = 0; i < 2; i++)
            {
                RowDefinition rowdef = new RowDefinition();
                rowdef.Height = GridLength.Auto;
                grid.RowDefinitions.Add(rowdef);
            }
            for (int i = 0;i < 4; i++)
            {
                ColumnDefinition coldef = new ColumnDefinition();
                coldef.Width = GridLength.Auto;
                grid.ColumnDefinitions.Add(coldef);
            }

            //在Grid内放置Label和TextBox控件
            for (int i = 0; i < 4; i++)
            {
                Label lbl = new Label();
                lbl.Content = "_" + Enum.GetName(typeof(Side), i) + ":";
                lbl.Margin = new Thickness(6);
                lbl.VerticalAlignment = VerticalAlignment.Center;
                grid.Children.Add(lbl);
                Grid.SetRow(lbl, i / 2);
                Grid.SetColumn(lbl, 2 * (i % 2));

                txtbox[i] = new TextBox();
                txtbox[i].TextChanged += TextBoxOnTextChanged;
                txtbox[i].MinWidth = 48;
                txtbox[i].Margin = new Thickness(6);
                grid.Children.Add(txtbox[i]);
                Grid.SetRow(txtbox[i], i / 2);
                Grid.SetColumn(txtbox[i], 2 * (i % 2) + 1);
            }

            //使用UniformGrid，放置OK和Cancel按钮
            UniformGrid ungrid = new UniformGrid();
            ungrid.Rows = 1;
            ungrid.Columns = 2;
            stack.Children.Add(ungrid);

            btnOK = new Button();
            btnOK.Content = "OK";
            btnOK.IsDefault = false;    //按下Enter键触发该按钮
            btnOK.IsEnabled = false;
            btnOK.MinWidth = 60;
            btnOK.Margin = new Thickness(12);
            btnOK.HorizontalAlignment = HorizontalAlignment.Center;
            btnOK.Click += OKButtonOnClick;
            ungrid.Children.Add(btnOK);

            Button btnCancel = new Button();
            btnCancel.Content = "Cancel";
            btnCancel.IsCancel = true;  //按下ESC键 关闭控件
            btnCancel.MinWidth = 60;
            btnCancel.Margin = new Thickness(12);
            btnCancel.HorizontalAlignment = HorizontalAlignment.Center;
            ungrid.Children.Add(btnCancel);
        }

        private void TextBoxOnTextChanged(object sender, TextChangedEventArgs e)
        {
            double result;
            btnOK.IsEnabled = Double.TryParse(txtbox[(int)Side.Left].Text, out result)
                && Double.TryParse(txtbox[(int)Side.Right].Text, out result)
                && Double.TryParse(txtbox[(int)Side.Top].Text, out result)
                && Double.TryParse(txtbox[(int)Side.Bottom].Text, out result);
        }

        private void OKButtonOnClick(object sender, RoutedEventArgs args)
        {
            this.DialogResult = true;
        }
    }
}
