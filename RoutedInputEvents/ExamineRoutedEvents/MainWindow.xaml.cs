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

namespace ExamineRoutedEvents
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly FontFamily fontfam = new FontFamily("Lucida Console");
        const string strFormat = "{0,-30} {1,-15} {2,-15} {3,-15}";
        StackPanel stackOutput;
        DateTime dtLast;

        public MainWindow()
        {
            InitializeComponent();

            this.Title = "Examine Routed Events";

            //建立Grid，当作窗口的内容Window
            Grid grid = new Grid();
            this.Content = grid;

            //建立3行
            RowDefinition rowdef = new RowDefinition();
            rowdef.Height = GridLength.Auto;
            grid.RowDefinitions.Add(rowdef);

            rowdef = new RowDefinition();
            rowdef.Height = GridLength.Auto;
            grid.RowDefinitions.Add(rowdef);

            rowdef = new RowDefinition();
            rowdef.Height = new GridLength(100, GridUnitType.Star);
            grid.RowDefinitions.Add(rowdef);

            //建立Button，并将他加入到Grid
            Button btn = new Button();
            btn.HorizontalAlignment = HorizontalAlignment.Center;
            btn.Margin = new Thickness(24);
            btn.Padding = new Thickness(24);
            grid.Children.Add(btn);

            //建立TextBlock，并将他加入到Button
            TextBlock text = new TextBlock();
            text.FontSize = 24;
            text.Text = this.Title;
            btn.Content = text;

            //建立标题，显示在ScrollViewer的上面
            TextBlock textHeadings = new TextBlock();
            textHeadings.FontFamily = fontfam;
            textHeadings.Inlines.Add(new Underline(new Run(string.Format(strFormat, "Routed Event", "Sender", "Source", "OriginalSource"))));
            grid.Children.Add(textHeadings);
            Grid.SetRow(textHeadings, 1);

            //建立ScollViewer
            ScrollViewer scroll = new ScrollViewer();
            grid.Children.Add(scroll);
            Grid.SetRow(scroll, 2);

            //建立StackPanel以显示事件
            stackOutput = new StackPanel();
            scroll.Content = stackOutput;

            //新增事件处理器
            UIElement[] els = { this, grid, btn, text };
            foreach (UIElement el in els)
            {
                //键盘
                el.PreviewKeyDown += AllPurposeEventHandker;
                el.PreviewKeyUp += AllPurposeEventHandker;
                el.PreviewTextInput += AllPurposeEventHandker;
                el.KeyDown += AllPurposeEventHandker;
                el.KeyUp += AllPurposeEventHandker;
                el.TextInput += AllPurposeEventHandker;

                //鼠标
                el.MouseDown += AllPurposeEventHandker;
                el.MouseUp += AllPurposeEventHandker;
                el.PreviewKeyDown += AllPurposeEventHandker;
                el.PreviewKeyUp += AllPurposeEventHandker;

                el.MouseLeftButtonDown += AllPurposeEventHandker;
                el.MouseLeftButtonUp += AllPurposeEventHandker;
                el.PreviewMouseLeftButtonDown += AllPurposeEventHandker;
                el.PreviewMouseLeftButtonUp += AllPurposeEventHandker;

                el.MouseRightButtonDown += AllPurposeEventHandker;
                el.MouseRightButtonUp += AllPurposeEventHandker;
                el.PreviewMouseRightButtonDown += AllPurposeEventHandker;
                el.PreviewMouseRightButtonUp += AllPurposeEventHandker;

                //手写笔
                el.StylusDown += AllPurposeEventHandker;
                el.StylusUp += AllPurposeEventHandker;
                el.PreviewStylusDown += AllPurposeEventHandker;
                el.PreviewStylusUp += AllPurposeEventHandker;

                //Click
                el.AddHandler(Button.ClickEvent, new RoutedEventHandler(AllPurposeEventHandker));       //这种写法是因为Window和Grid无相应的Click事件，所以此处安装一个Button.Click事件
            }
        }

        private void AllPurposeEventHandker(object sender, RoutedEventArgs e)
        {
            //如果有事件缝隙的话，增加空白行
            DateTime dtNow = DateTime.Now;
            if (dtNow - dtLast > TimeSpan.FromMilliseconds(100))
            {
                stackOutput.Children.Add(new TextBlock(new Run(" ")));
            }
            dtLast = dtNow;

            //显示事件信息
            TextBlock text = new TextBlock();
            text.FontFamily = fontfam;
            text.Text = string.Format(strFormat
                , e.RoutedEvent.Name
                , TypeWithoutNameSpace(sender)
                , TypeWithoutNameSpace(e.Source)
                , TypeWithoutNameSpace(e.OriginalSource));

            stackOutput.Children.Add(text);
            (stackOutput.Parent as ScrollViewer).ScrollToBottom();
        }

        private string TypeWithoutNameSpace(object obj)
        {
            string[] astr = obj.GetType().ToString().Split('.');
            return astr[astr.Length - 1];
        }
    }
}
