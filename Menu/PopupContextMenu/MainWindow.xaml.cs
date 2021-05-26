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

namespace PopupContextMenu
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private ContextMenu menu;
        private MenuItem itemBold, itemItalic;
        private MenuItem[] itemDecor;
        private Inline inlClicked;

        public MainWindow()
        {
            InitializeComponent();

            Init();
        }

        private void Init()
        {
            this.Title = "Popup Context Menu";

            menu = new ContextMenu();
            
            itemBold = new MenuItem();
            itemBold.Header = "Bold";
            menu.Items.Add(itemBold);

            itemItalic = new MenuItem();
            itemItalic.Header = "Italic";
            menu.Items.Add(itemItalic);

            TextDecorationLocation[] locs = (TextDecorationLocation[])Enum.GetValues(typeof(TextDecorationLocation));       //枚举文本装饰器的位置

            itemDecor = new MenuItem[locs.Length];
            for (int i = 0;i < locs.Length; i++)
            {
                //创建文本装饰器
                TextDecoration decor = new TextDecoration();    
                decor.Location = locs[i];   //设置文本装饰器的垂直位置

                itemDecor[i] = new MenuItem();
                itemDecor[i].Header = locs[i].ToString();
                itemDecor[i].Tag = decor;
                menu.Items.Add(itemDecor[i]);
            }

            //为所有菜单指定同一个Click处理函数
            menu.AddHandler(MenuItem.ClickEvent, new RoutedEventHandler(MenuOnClick));

            TextBlock text = new TextBlock();
            text.FontSize = 32;
            text.HorizontalAlignment = HorizontalAlignment.Center;
            text.VerticalAlignment = VerticalAlignment.Center;
            this.Content = text;
          
            string strQuote = "To be, or not to be, that is the question";
            string[] strWords = strQuote.Split();

            foreach(string str in strWords)
            {
                Run run = new Run(str);
                run.TextDecorations = new TextDecorationCollection();       //为Run对象显示创建TextDecorationCollection型对象，这个collection默认是不存在的，而且TextDecorations通常是null
                text.Inlines.Add(run);
                text.Inlines.Add(" ");
            }
        }
       
        private void MenuOnClick(object sender, RoutedEventArgs args)
        {
            MenuItem item = args.Source as MenuItem;

            item.IsChecked ^= true;         //同 item.IsChecked = !item.IsChecked;         

            if (item == itemBold)
            {
                inlClicked.FontWeight = (item.IsChecked ? FontWeights.Bold : FontWeights.Normal);
            }
            else if (item == itemItalic)
            {
                inlClicked.FontStyle = (item.IsChecked ? FontStyles.Italic : FontStyles.Normal);
            }
            else
            {
                if (item.IsChecked)
                {
                    inlClicked.TextDecorations.Add(item.Tag as TextDecoration);
                }
                else
                {
                    inlClicked.TextDecorations.Remove(item.Tag as TextDecoration);
                }
            }

            //(inlClicked.Parent as TextBlock).InvalidateVisual();  //多余的执行
        }

        protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonUp(e);

            //获取被点击的单词，Run继承自Inline，根据被点击单词的属性，初始化菜单
            if ((inlClicked = e.Source as Inline) != null)
            {
                itemBold.IsChecked = (inlClicked.FontWeight == FontWeights.Bold);
                itemItalic.IsChecked = (inlClicked.FontStyle == FontStyles.Italic);

                foreach (MenuItem item in itemDecor)
                {
                    item.IsChecked = inlClicked.TextDecorations.Contains(item.Tag as TextDecoration);

                    menu.IsOpen = true;     //打开菜单，出现在鼠标指针的位置
                    e.Handled = true;
                }
            }
        }

        //使鼠标左键同样能进行右键的操作
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            OnMouseRightButtonUp(e);
        }
    }
}
