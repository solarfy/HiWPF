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
using System.Printing;

namespace PrintBanner
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private TextBox txtbox;

        public MainWindow()
        {
            InitializeComponent();

            Init();
        }

        private void Init()
        {
            this.Title = "Print Banner";
            this.SizeToContent = SizeToContent.WidthAndHeight;

            StackPanel stack = new StackPanel();
            this.Content = stack;

            txtbox = new TextBox();
            txtbox.Width = 250;
            txtbox.Margin = new Thickness(12);
            stack.Children.Add(txtbox);

            Button btn = new Button();
            btn.Content = "_Print...";
            btn.Margin = new Thickness(12);
            btn.HorizontalAlignment = HorizontalAlignment.Center;
            btn.Click += PrintOnClick;
            stack.Children.Add(btn);

            txtbox.Focus();
        }

        private void PrintOnClick(object sender, RoutedEventArgs args)
        {
            PrintDialog dlg = new PrintDialog();

            if (dlg.ShowDialog().GetValueOrDefault())
            {
                //确定方位时垂直的（Portrait）
                PrintTicket prntkt = dlg.PrintTicket;
                prntkt.PageOrientation = PageOrientation.Portrait;  
                dlg.PrintTicket = prntkt;   //用户即使选择水平纸张，此处依旧会将其设置为垂直

                //建立新的BannerDocumentPaginator对象
                BannerDocumentPaginator paginator = new BannerDocumentPaginator();
                paginator.Text = txtbox.Text;

                //根据纸张的尺寸，指定PageSize属性
                paginator.PageSize = new Size(dlg.PrintableAreaWidth, dlg.PrintableAreaHeight);

                //调用PrintDocument以打印出文件
                dlg.PrintDocument(paginator, "Banner: " + txtbox.Text);                
            }
        }
    }
}
