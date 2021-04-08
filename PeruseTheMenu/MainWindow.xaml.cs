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
using System.Reflection;

namespace PeruseTheMenu
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        MenuItem itemChecked;
        TextBlock text;

        protected MenuItem itemCut, itemCopy, itemPaste, itemDelete;
        KeyGesture gestCut = new KeyGesture(Key.X, ModifierKeys.Control);
        KeyGesture gestCopy = new KeyGesture(Key.C, ModifierKeys.Control);
        KeyGesture gestPaste = new KeyGesture(Key.V, ModifierKeys.Control);
        KeyGesture gestDelete = new KeyGesture(Key.Delete);

        Dictionary<KeyGesture, MenuItem> gests = new Dictionary<KeyGesture, MenuItem>();

        public MainWindow()
        {
            InitializeComponent();
            Init();            
        }

        void Init()
        {
            this.Title = "Peruse the Menu";

            DockPanel dock = new DockPanel();
            this.Content = dock;

            Menu menu = new Menu();
            dock.Children.Add(menu);
            DockPanel.SetDock(menu, Dock.Top);

            text = new TextBlock();
            text.Text = this.Title;
            text.FontSize = 32;
            //text.TextAlignment = TextAlignment.Center;
            text.VerticalAlignment = VerticalAlignment.Center;
            text.HorizontalAlignment = HorizontalAlignment.Center;
            text.TextWrapping = TextWrapping.Wrap;
            text.Background = SystemColors.WindowBrush;
            text.Foreground = SystemColors.WindowTextBrush;
            dock.Children.Add(text);

            #region 创建File菜单
            MenuItem itemFile = new MenuItem();
            itemFile.Header = "_File";
            menu.Items.Add(itemFile);

            MenuItem itemNew = new MenuItem();
            itemNew.Header = "_New";
            itemNew.Click += UnimplentedOnClick;
            itemFile.Items.Add(itemNew);

            MenuItem itemOpen = new MenuItem();
            itemOpen.Header = "_Open";
            itemOpen.Click += UnimplentedOnClick;
            itemFile.Items.Add(itemOpen);

            MenuItem itemSave = new MenuItem();
            itemSave.Header = "_Save";
            itemSave.Click += UnimplentedOnClick;
            itemFile.Items.Add(itemSave);

            itemFile.Items.Add(new Separator());

            MenuItem itemExit = new MenuItem();
            itemExit.Header = "E_xit";
            itemExit.Click += ExitOnClick;
            itemFile.Items.Add(itemExit);

            #endregion

            #region 创建Edit菜单
            MenuItem itemEdit = new MenuItem();
            itemEdit.Header = "_Edit";
            itemEdit.SubmenuOpened += EditOnOpened;
            menu.Items.Add(itemEdit);

            itemCut = new MenuItem();
            itemCut.Header = "Cu_t";
            itemCut.InputGestureText = "Ctrl+X";
            itemCut.Click += CutOnClick;
            Image img = new Image();
            img.Source = new BitmapImage(new Uri("pack://application:,,/Images/cut.png"));
            itemCut.Icon = img;
            itemEdit.Items.Add(itemCut);

            itemCopy = new MenuItem();
            itemCopy.Header = "_Copy";
            itemCopy.InputGestureText = "Ctrl+C";
            itemCopy.Click += CopyOnClick;
            img = new Image();
            img.Source = new BitmapImage(new Uri("pack://application:,,/Images/copy.png"));
            itemCopy.Icon = img;
            itemEdit.Items.Add(itemCopy);

            itemPaste = new MenuItem();
            itemPaste.Header = "_Paste";
            itemPaste.InputGestureText = "Ctrl+V";
            itemPaste.Click += PasteOnClick;
            img = new Image();
            img.Source = new BitmapImage(new Uri("pack://application:,,/Images/paste.png"));
            itemPaste.Icon = img;
            itemEdit.Items.Add(itemPaste);

            itemDelete = new MenuItem();
            itemDelete.Header = "_Delete";
            itemDelete.InputGestureText = "Delete";
            itemDelete.Click += DeleteOnClick;
            img = new Image();
            img.Source = new BitmapImage(new Uri("pack://application:,,/Images/delete.png"));
            itemDelete.Icon = img;
            itemEdit.Items.Add(itemDelete);

            //添加手势键
            gests.Add(gestCut, itemCut);
            gests.Add(gestCopy, itemCopy);
            gests.Add(gestPaste, itemPaste);
            gests.Add(gestDelete, itemDelete);
            #endregion

            #region 创建Window菜单

            MenuItem itemWindow = new MenuItem();
            itemWindow.Header = "Window";
            menu.Items.Add(itemWindow);

            MenuItem itemTaskbar = new MenuItem();
            itemTaskbar.Header = "Show in Taskbar";
            itemTaskbar.IsCheckable = true;
            itemTaskbar.IsChecked = this.ShowInTaskbar;
            itemTaskbar.Click += TaskbarOnClick;
            itemWindow.Items.Add(itemTaskbar);

            MenuItem itemSize = new MenuItem();
            itemSize.Header = "Size to Content";
            itemSize.IsCheckable = true;
            itemSize.IsChecked = this.SizeToContent == SizeToContent.WidthAndHeight;
            itemSize.Checked += SizeOnCheck;
            itemSize.Unchecked += SizeOnCheck;
            itemWindow.Items.Add(itemSize);

            MenuItem itemResize = new MenuItem();
            itemResize.Header = "Resizable";
            itemResize.IsCheckable = true;
            itemResize.IsChecked = this.ResizeMode == ResizeMode.CanResize;
            itemResize.Click += ResizeOnClick;
            itemWindow.Items.Add(itemResize);

            MenuItem itemTopmost = new MenuItem();
            itemTopmost.Header = "Topmost";
            itemTopmost.IsCheckable = true;
            itemTopmost.IsChecked = this.Topmost;
            itemTopmost.Checked += TopmostOnCheck;
            itemTopmost.Unchecked += TopmostOnCheck;
            itemWindow.Items.Add(itemTopmost);

            //MenuItem itemHa = new MenuItem();
            //itemHa.Header = "_哈";     //测试中文快捷键
            //menu.Items.Add(itemHa);

            #endregion

            #region 创建Style菜单

            MenuItem itemStyle = new MenuItem();
            itemStyle.Header = "Style";
            menu.Items.Add(itemStyle);

            itemStyle.Items.Add(CreateMenuItem("No border or caption", WindowStyle.None));
            itemStyle.Items.Add(CreateMenuItem("Single-border window", WindowStyle.SingleBorderWindow));
            itemStyle.Items.Add(CreateMenuItem("3D border window", WindowStyle.ThreeDBorderWindow));
            itemStyle.Items.Add(CreateMenuItem("Tool window", WindowStyle.ToolWindow));

            #endregion

            #region 创建Color菜单
            MenuItem itemColor = new MenuItem();
            itemColor = new MenuItem();
            itemColor.Header = "Color";
            menu.Items.Add(itemColor);

            MenuItem itemForeground = new MenuItem();
            itemForeground.Header = "Foreground";
            itemForeground.SubmenuOpened += ForegroundOnOpened; //SubmenuOpened打开子菜单事件
            itemColor.Items.Add(itemForeground);

            FillWithColors(itemForeground, ForegroundOnClick);  //填充Foreground子集菜单

            MenuItem itemBackground = new MenuItem();
            itemBackground.Header = "Background";
            itemBackground.SubmenuOpened += BackgroundOnOpened;
            itemColor.Items.Add(itemBackground);

            FillWithColors(itemBackground, BackgroundOnClick);  //填充Background子集菜单

            #endregion

            #region 创建ColorSelector菜单
            itemColor = new MenuItem();
            itemColor.Header = "ColorSelector";
            menu.Items.Add(itemColor);

            itemForeground = new MenuItem();
            itemForeground.Header = "Foreground";
            itemColor.Items.Add(itemForeground);

            //建立ColorBox，并且将它绑定到文本的Foreground
            ColorGridBox clr = new ColorGridBox();
            clr.SetBinding(ColorGridBox.SelectedValueProperty, nameof(Foreground));
            clr.DataContext = text;
            itemForeground.Items.Add(clr);

            itemBackground = new MenuItem();
            itemBackground.Header = "Background";
            itemColor.Items.Add(itemBackground);

            //建立ColorBox，并且将它绑定到文本的Background
            clr = new ColorGridBox();
            clr.SetBinding(ColorGridBox.SelectedValueProperty, nameof(Background));
            clr.DataContext = text;
            itemBackground.Items.Add(clr);

            #endregion
        }

        #region File

        private void UnimplentedOnClick(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            string strItem = item.Header.ToString().Replace("_", "");
            MessageBox.Show("The " + strItem + " option has not yet been implemented", this.Title);
        }

        private void ExitOnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Edit
        private void EditOnOpened(object sender, RoutedEventArgs args)
        {
            itemCut.IsEnabled = itemCopy.IsEnabled = itemDelete.IsEnabled = !string.IsNullOrEmpty(text.Text);
            itemPaste.IsEnabled = Clipboard.ContainsText();
        }

        protected void CutOnClick(object sender, RoutedEventArgs args)
        {
            CopyOnClick(sender, args);
            DeleteOnClick(sender, args);
        }

        protected void CopyOnClick(object sender, RoutedEventArgs args)
        {
            if (text.Text != null && text.Text.Length > 0)
            {
                Clipboard.SetText(text.Text);   //将文字设定给剪贴板
            }
        }

        protected void PasteOnClick(object sender, RoutedEventArgs args)
        {
            if (Clipboard.ContainsText())
                text.Text += Clipboard.GetText();    //获取剪贴板中的文字
        }

        protected void DeleteOnClick(object sender, RoutedEventArgs args)
        {
            text.Text = null;
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

/*
            e.Handled = true;   //先将其设置为true，如果此键不匹配定义的任何一个手势，再将其设为false

            if (gestCut.Matches(null, e))
            {
                CutOnClick(this, e);
            }
            else if (gestCopy.Matches(null, e))
            {
                CopyOnClick(this, e);
            }
            else if (gestPaste.Matches(null, e))
            {
                PasteOnClick(this, e);
            }
            else if (gestDelete.Matches(null, e))
            {
                DeleteOnClick(this, e);
            }
            else
            {
                e.Handled = false;
            }
*/
            foreach(KeyGesture gest in gests.Keys)
            {
                if (gest.Matches(null, e))
                {
                    gests[gest].RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent, gests[gest]));
                    e.Handled = true;
                }
            }
        }

        #endregion

        #region Window        
        private void TaskbarOnClick(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            this.ShowInTaskbar = item.IsChecked;
        }

        private void SizeOnCheck(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            this.SizeToContent = item.IsChecked ? SizeToContent.WidthAndHeight : SizeToContent.Manual;
        }

        private void ResizeOnClick(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            this.ResizeMode = item.IsChecked ? ResizeMode.CanResize : ResizeMode.NoResize;
        }

        private void TopmostOnCheck(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            this.Topmost = item.IsChecked;
        }
        #endregion

        #region Style

        private MenuItem CreateMenuItem(string str, WindowStyle style)
        {
            MenuItem item = new MenuItem();
            item.Header = str;
            item.Tag = style;
            item.IsChecked = (style == this.WindowStyle);
            item.Click += StyleOnClick;

            if (item.IsChecked)
                itemChecked = item;

            return item;
        }

        private void StyleOnClick(object sender, RoutedEventArgs e)
        {
            itemChecked.IsChecked = false;      //先取消勾选
            itemChecked = e.Source as MenuItem;
            itemChecked.IsChecked = true;   //再打勾

            this.WindowStyle = (WindowStyle)itemChecked.Tag;
        }

        #endregion

        #region Color        
        private void FillWithColors(MenuItem itemParent, RoutedEventHandler handler)
        {
            foreach(PropertyInfo prop in typeof(Colors).GetProperties())
            {
                Color clr = (Color)prop.GetValue(null, null);
                int iCount = 0;

                iCount += clr.R == 0 || clr.R == 255 ? 1 : 0;
                iCount += clr.G == 0 || clr.G == 255 ? 1 : 0;
                iCount += clr.B == 0 || clr.B == 255 ? 1 : 0;

                //选取RGB三者中至少有两者为0或255的颜色
                if (clr.A == 255 && iCount > 1)
                {
                    MenuItem item = new MenuItem();
                    item.Header = prop.Name;
                    item.Tag = clr;
                    item.Click += handler;

                    Rectangle rect = new Rectangle();
                    rect.Fill = new SolidColorBrush(clr);
                    rect.Width = 2 * (rect.Height = 12);
                    item.Icon = rect;       //勾选符号和Icon会占用相同的空间
                   
                    itemParent.Items.Add(item);                    
                }
            }
        }

        private void ForegroundOnOpened(object sender, RoutedEventArgs e)
        {
            MenuItem itemParent = sender as MenuItem;
            //打开子菜单时，遍历所有的子集菜单，当所设置的属性与对应菜单属性相同时，勾选该菜单；不同时，取消勾选
            foreach (MenuItem item in itemParent.Items)
            {
                item.IsChecked = ((text.Foreground as SolidColorBrush).Color == (Color)item.Tag);
            }
        }

        private void BackgroundOnOpened(object sender, RoutedEventArgs e)
        {
            MenuItem itemParent = sender as MenuItem;
            //打开子菜单时，遍历所有的子集菜单，当所设置的属性与对应菜单属性相同时，勾选该菜单；不同时，取消勾选
            foreach(MenuItem item in itemParent.Items)
            {
                item.IsChecked = ((text.Background as SolidColorBrush).Color == (Color)item.Tag);
            }
        }

        private void ForegroundOnClick(object sender, RoutedEventArgs args)
        {
            MenuItem item = sender as MenuItem;            
            Color clr = (Color)item.Tag;
            text.Foreground = new SolidColorBrush(clr);
        }

        private void BackgroundOnClick(object sender, RoutedEventArgs args)
        {
            MenuItem item = sender as MenuItem;
            Color clr = (Color)item.Tag;
            text.Background = new SolidColorBrush(clr);
        }
        #endregion
    }
}
