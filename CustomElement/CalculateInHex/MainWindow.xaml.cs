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
using System.Windows.Threading;

namespace CalculateInHex
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        RoundedButton btnDisplay;
        ulong numDisplay;
        ulong numFirst;
        bool bNewNumber = true;
        char chOperation = '=';


        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        void Init()
        {
            this.Title = "Calculate in Hex";
            this.SizeToContent = SizeToContent.WidthAndHeight;
            this.ResizeMode = ResizeMode.CanMinimize;

            //建立一个Grid作为窗口的内容
            Grid grid = new Grid();
            grid.Margin = new Thickness(4);
            this.Content = grid;

            //建立5个列
            for (int i = 0; i < 5; i++)
            {
                ColumnDefinition col = new ColumnDefinition();
                col.Width = GridLength.Auto;
                grid.ColumnDefinitions.Add(col);
            }

            //建立7个行
            for (int i = 0; i < 7; i++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = GridLength.Auto;
                grid.RowDefinitions.Add(row);
            }

            //按钮上面的文字
            string[] strButtons = {"0",
                                   "D", "E", "F", "+", "&",
                                   "A", "B", "C", "-", "|",
                                   "7", "8", "9", "*", "^",
                                   "4", "5", "6", "/", "<<", 
                                   "1", "2", "3", "%", ">>",
                                   "0", "Back", "Equals"};

            int iRow = 0, iCol = 0;
            
            //建立按钮
            foreach(string str in strButtons)
            {
                //建立圆角按钮
                RoundedButton btn = new RoundedButton();
                btn.Focusable = false;
                btn.Height = 32;
                btn.Margin = new Thickness(4);
                btn.Click += Btn_Click;

                //为RoundedButton的孩子，建立TextBlock
                TextBlock txt = new TextBlock();
                txt.Text = str;
                btn.Child = txt;

                //在Grid内加入RountedButton
                grid.Children.Add(btn);
                Grid.SetRow(btn, iRow);
                Grid.SetColumn(btn, iCol);

                //Display按钮的做法例外
                if (iRow == 0 && iCol == 0)
                {
                    btnDisplay = btn;
                    btn.Margin = new Thickness(4, 4, 4, 6);
                    Grid.SetColumnSpan(btn, 5);
                    iRow = 1;
                }
                //Back和Equal
                else if (iRow == 6 && iCol > 0)
                {
                    Grid.SetColumnSpan(btn, 2);
                    iCol += 2;
                }
                //所有其他的按钮
                else
                {
                    btn.Width = 32;
                    if (0 == (iCol = (iCol + 1) % 5))
                        iRow++; 
                }
            }
        }

        //Click事件处理器
        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            //取得点击的按钮
            RoundedButton btn = e.Source as RoundedButton;

            if (btn == null)
                return;

            //取得按钮文字和第一个字符
            string strButton = (btn.Child as TextBlock).Text;
            char chButton = strButton[0];

            //一些特例
            if (strButton == "Equals")
                chButton = '=';
            if (btn == btnDisplay)
                numDisplay = 0;
            else if (strButton == "Back")
                numDisplay /= 16;       //删除最后一位
            //16进制数字
            else if (Char.IsLetterOrDigit(chButton))        //字母或数字
            {
                if (bNewNumber)
                {
                    numFirst = numDisplay;
                    numDisplay = 0;
                    bNewNumber = false;
                }

                if (numDisplay <= ulong.MaxValue >> 4)
                    numDisplay = 16 * numDisplay + (ulong)(chButton - (Char.IsDigit(chButton) ? '0' : 'A' - 10));   //将数字转成16进制的数
            }
            //操作[加、减、乘、除。。。]
            else
            {
                if (!bNewNumber)
                {
                    switch (chOperation)
                    {
                        case '=': break;
                        case '+': numDisplay = numFirst + numDisplay; break;
                        case '-': numDisplay = numFirst - numDisplay; break;
                        case '*': numDisplay = numFirst * numDisplay; break;
                        case '&': numDisplay = numFirst & numDisplay; break;
                        case '|': numDisplay = numFirst | numDisplay; break;
                        case '^': numDisplay = numFirst ^ numDisplay; break;
                        case '<': numDisplay = numFirst << (int)numDisplay; break;
                        case '>': numDisplay = numFirst >> (int)numDisplay; break;
                        case '/': numDisplay = numDisplay != 0 ? numFirst / numDisplay : ulong.MaxValue; break;
                        case '%': numDisplay = numDisplay != 0 ? numFirst % numDisplay : ulong.MaxValue; break;
                        default: numDisplay = 0; break;
                    }
                }
                
                bNewNumber = true;
                chOperation = chButton;
            }

            //格式化
            TextBlock text = new TextBlock();
            text.Text = string.Format("{0:X}", numDisplay);     //显示16进制的数字
            btnDisplay.Child = text;
        }

        protected override void OnTextInput(TextCompositionEventArgs e)
        {
            base.OnTextInput(e);

            if (e.Text.Length == 0)
                return;

            //取得字符输入[比对输入的字符，并触发与该字符相同的按钮]
            char chKey = Char.ToUpper(e.Text[0]);

            //循环按钮处理
            foreach (UIElement child in (this.Content as Grid).Children)
            {
                RoundedButton btn = child as RoundedButton;
                string strButton = (btn.Child as TextBlock).Text;

                //检查按钮是否符合
                if ((chKey == strButton[0]) && btn != btnDisplay && strButton != "Equals" && strButton != "Back" ||
                    (chKey == '=' && strButton == "Equals") ||
                    (chKey == '\r' && strButton == "Equals") ||
                    (chKey == '\b' && strButton == "Back") ||
                    (chKey == '\x1B' && btn == btnDisplay))
                {
                    //模拟Click事件，以处理键盘
                    RoutedEventArgs argsClick = new RoutedEventArgs(RoundedButton.ClickEvent, btn);
                    btn.RaiseEvent(argsClick);

                    //让按钮看起来好像被按下
                    btn.IsPressed = true;

                    //设定计时器，以取消按下
                    DispatcherTimer tmr = new DispatcherTimer();
                    tmr.Interval = TimeSpan.FromMilliseconds(100);
                    tmr.Tag = btn;
                    tmr.Tick += Tmr_Tick;
                    tmr.Start();
                    e.Handled = true;
                }
            }
        }

        private void Tmr_Tick(object sender, EventArgs e)
        {
            //取消按下
            DispatcherTimer tmr = sender as DispatcherTimer;
            RoundedButton btn = tmr.Tag as RoundedButton;
            btn.IsPressed = false;
            //关掉计时器，断开事件处理器
            tmr.Stop();
            tmr.Tick -= Tmr_Tick;
        }
    }
}
