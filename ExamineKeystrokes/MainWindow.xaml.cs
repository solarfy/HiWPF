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

namespace ExamineKeystrokes
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        StackPanel stack;
        ScrollViewer scoll;

        //string strHeader = "Event";
        string strFormatKey = "{0, -10} {1, -20} {2, -10} {3, -10} {4, -15} {5, -18} {6, -7} {7, -10} {8, 10}";
        string strFormatText = "{0, 10} {1, -10} {2, -10} {3, -10}";

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            this.Title = "Eamine Keystrokes";
            FontFamily = new FontFamily("Courier New");

            Grid grid = new Grid();
            this.Content = grid;

            RowDefinition rowdef = new RowDefinition();
            rowdef.Height = GridLength.Auto;
            grid.RowDefinitions.Add(rowdef);
            grid.RowDefinitions.Add(new RowDefinition());

            //显示标题文字
            TextBlock textHeader = new TextBlock();
            textHeader.FontWeight = FontWeights.Bold;
            textHeader.Text = string.Format(strFormatKey, "Event", "Key", "Sys-Key", "Ime", "KeyStates", "IsDown", "IsUp", "IsToggled", "IsRepeat");
            grid.Children.Add(textHeader);

            //建立StackPanel作为ScrollViewer的孩子，以显示事件
            scoll = new ScrollViewer();
            grid.Children.Add(scoll);
            Grid.SetRow(scoll, 1);

            stack = new StackPanel();
            scoll.Content = stack;
        }

        private void DisplayKeyInfo(KeyEventArgs args)
        {
            string str = string.Format(strFormatKey, args.RoutedEvent.Name, args.Key, args.SystemKey, args.ImeProcessedKey, args.KeyStates, args.IsDown, args.IsUp, args.IsToggled, args.IsRepeat);
        }

        private void DisplayInfo(string str)
        {
            TextBlock text = new TextBlock();
            text.Text = str;
            stack.Children.Add(text);
            scoll.ScrollToBottom();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            DisplayKeyInfo(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            DisplayKeyInfo(e);
        }

        protected override void OnTextInput(TextCompositionEventArgs e)
        {
            base.OnTextInput(e);
            string str = string.Format(strFormatText, e.RoutedEvent.Name, e.Text, e.ControlText, e.SystemText);
            DisplayInfo(str);
        }
    }
}
