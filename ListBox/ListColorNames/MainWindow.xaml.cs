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

namespace ListColorNames
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Title = "List Color Names";

            InitGrid_1();
            InitGrid_2();
            InitGrid_3();
            InitGird_4();
            InitGrid_5();
            InitGrid_6();
            InitGrid_7();
            InitGrid_8();
        }

        void InitGrid_1()
        {
            ListBox lstbox = new ListBox();
            lstbox.Width = 180;
            lstbox.Height = 300;
            lstbox.SelectionChanged += (sender, e) => 
            {
                string str = lstbox.SelectedItem as string;
                Color clr = (Color)typeof(Colors).GetProperty(str).GetValue(null, null);       
                grid1.Background = new SolidColorBrush(clr);

            };
            this.grid1.Children.Add(lstbox);

            //将颜色名称填入ListBox
            PropertyInfo[] props = typeof(Colors).GetProperties();
            foreach (PropertyInfo prop in props)
                lstbox.Items.Add(prop.Name);

            //默认选取项目
            lstbox.SelectedItem = nameof(Colors.Magenta);   //先初始化SelectionChanged事件，再设置选取项目，让其触发SelectionChanged事件
            lstbox.ScrollIntoView(lstbox.SelectedItem);     //滚动到选取的项目，让选取的项目可见
            lstbox.Focus();

            lstbox.SelectionMode = SelectionMode.Single;    //选择模式
            /* SelectionMode.Multiple  鼠标左键点选，可以选取或清除单个项目；也可以使用方向键移动焦点矩形，用空格键选取或清除单个项目
             * SelectionMode.Extended  Ctrl或Shift键 使用多选操作 使用此模式必须使用SelectedItems属性（注意名称复数）SelectionChanged事件中的SelectionChangedEventArgs类型对象，其中具有AddItems和RemoveItems方法
             */
        }

        void InitGrid_2()
        {
            ListBox lstbox = new ListBox();
            lstbox.Width = 180;
            lstbox.Height = 300;
            lstbox.SelectionChanged += (sender, e) =>
            {
                Color clr = (Color)lstbox.SelectedItem;
                grid2.Background = new SolidColorBrush(clr);
            };
            this.grid2.Children.Add(lstbox);

            //将颜色名称填入ListBox
            PropertyInfo[] props = typeof(Colors).GetProperties();
            foreach (PropertyInfo prop in props)
                lstbox.Items.Add(prop.GetValue(null, null));     //添加的是Color的对象

            //默认选取项目
            lstbox.SelectedItem = Colors.Linen;     //先初始化SelectionChanged事件，再设置选取项目，让其触发SelectionChanged事件
            lstbox.ScrollIntoView(lstbox.SelectedItem);     //滚动到选取的项目，让选取的项目可见                  
        }

        void InitGrid_3()
        {
            ListBox lstbox = new ListBox();
            lstbox.Width = 180;
            lstbox.Height = 300;           
            this.grid3.Children.Add(lstbox);
           
            lstbox.ItemsSource = NamedColor.All;
            lstbox.DisplayMemberPath = nameof(NamedColor.Name);     //显示Name属性
            lstbox.SelectedValuePath = nameof(NamedColor.Color);    //选中Color属性

            lstbox.SelectionChanged += (sender, e) =>
            {
                Color clr = (Color)lstbox.SelectedValue;
                this.grid3.Background = new SolidColorBrush(clr);
            };
        }

        void InitGird_4()
        {
            ListBox lstbox = new ListBox();
            lstbox.Width = 180;
            lstbox.Height = 300;
            this.grid4.Children.Add(lstbox);

            lstbox.ItemsSource = NamedBrush.All;
            lstbox.DisplayMemberPath = nameof(NamedBrush.Name);     //显示Name属性
            lstbox.SelectedValuePath = nameof(NamedBrush.Brush);    //选中Brush属性

            //使用版定方法，不再使用事件处理函数
            lstbox.SetBinding(ListBox.SelectedValueProperty, (nameof(Background)));
            lstbox.DataContext = this.grid4;
        }

        void InitGrid_5()
        {
            ListBox lstbox = new ListBox();
            lstbox.Width = 180;
            lstbox.Height = 300;
            this.grid5.Children.Add(lstbox);

            foreach (NamedBrush nb in NamedBrush.All)
            {
                Ellipse ellip = new Ellipse();
                ellip.Width = 100;
                ellip.Height = 25;
                ellip.Margin = new Thickness(10, 5, 0, 5);      //使其不能完全遮挡住选中时的背景色
                ellip.Fill = nb.Brush;                
                lstbox.Items.Add(ellip);
            }

            lstbox.SelectedValuePath = nameof(Shape.Fill);
            lstbox.SetBinding(ListBox.SelectedValueProperty, nameof(Background));
            lstbox.DataContext = this.grid5;
        }

        void InitGrid_6()
        {
            ListBox lstbox = new ListBox();
            lstbox.Width = 180;
            lstbox.Height = 300;
            this.grid6.Children.Add(lstbox);

            foreach (NamedColor nb in NamedColor.All)
            {
                Color clr = nb.Color;
                bool isBlack = .222 * clr.R + .707 * clr.G + .071 * clr.B > 128;        //根据3原色标准加权平均值，判断文字是以黑色还是白色显示在彩色的背景上

                ListBoxItem item = new ListBoxItem();
                item.Content = nb.Name;
                item.Background = new SolidColorBrush(clr);
                item.Foreground = isBlack ? Brushes.Black : Brushes.White;
                item.HorizontalContentAlignment = HorizontalAlignment.Center;
                item.Padding = new Thickness(2);
                lstbox.Items.Add(item);
            }

            lstbox.SelectionChanged += ( object sender, SelectionChangedEventArgs e) =>
            {
                ListBoxItem item;

                //反选恢复ListBoxItem中默认文字样式
                if (e.RemovedItems.Count > 0)
                {
                    item = e.RemovedItems[0] as ListBoxItem;
                    string str = item.Content as string;
                    item.Content = str.Substring(2, str.Length - 4);
                    item.FontWeight = FontWeights.Regular;
                }
                //选中更改ListBoxItem中文字样式
                if (e.AddedItems.Count > 0)
                {
                    item = e.AddedItems[0] as ListBoxItem;
                    string str = item.Content as string;
                    item.Content = "[ " + str + " ]";
                    item.FontWeight = FontWeights.Bold;
                }

                item = lstbox.SelectedItem as ListBoxItem;

                if (item != null)
                {
                    this.grid6.Background = item.Background;
                }
            };
        }

        void InitGrid_7()
        {
            ColorListBox lstbox = new ColorListBox();
            lstbox.Width = 180;
            lstbox.Height = 300;
            this.grid7.Children.Add(lstbox);

            lstbox.SelectionChanged += (sender, e) =>
            {
                this.grid7.Background = new SolidColorBrush(lstbox.SelectedColor);
            };

            //初始化SelectedColor
            lstbox.SelectedColor = SystemColors.WindowColor;
        }

        void InitGrid_8()
        {
            //建立一个DataTemplate （接收的是NamedBrush类型数据）
            DataTemplate template = new DataTemplate(typeof(NamedBrush));

            //根据StackPanel建立一个FramworkElementFactory
            FrameworkElementFactory factoryStack = new FrameworkElementFactory(typeof(StackPanel));
            factoryStack.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            //让它变成DataTemplate视觉树根
            template.VisualTree = factoryStack;

            //根据Rectangle建立FrameworkElementFactory
            FrameworkElementFactory factoryRectangle = new FrameworkElementFactory(typeof(Rectangle));
            factoryRectangle.SetValue(Rectangle.WidthProperty, 16.0);
            factoryRectangle.SetValue(Rectangle.HeightProperty, 16.0);
            factoryRectangle.SetValue(Rectangle.MarginProperty, new Thickness(2));
            factoryRectangle.SetValue(Rectangle.StrokeProperty, SystemColors.WindowTextBrush);
            factoryRectangle.SetValue(Rectangle.FillProperty, new Binding(nameof(Brush)));      //亦可写SetBinding
            //将其加入StackPanel
            factoryStack.AppendChild(factoryRectangle);

            //根据TextBlock建立FrameworkElementFactory
            FrameworkElementFactory factoryTextBlock = new FrameworkElementFactory(typeof(TextBlock));
            factoryTextBlock.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            factoryTextBlock.SetValue(TextBlock.TextProperty, new Binding(nameof(NamedBrush.Name)));    //亦可写SetBinding
            //将其加入StackPanel
            factoryStack.AppendChild(factoryTextBlock);

            //建立一个ListBox
            ListBox lstbox = new ListBox();
            lstbox.Width = 180;
            lstbox.Height = 300;
            this.grid8.Children.Add(lstbox);

            //将上面建立的template设定给ItemTemplate属性
            lstbox.ItemTemplate = template;
            //设定ItemSource为NamedBrush对象数组
            lstbox.ItemsSource = NamedBrush.All;
            //将SelectedValue绑定到Background
            lstbox.SelectedValuePath = nameof(NamedBrush.Brush);
            lstbox.SetBinding(ListBox.SelectedValueProperty, nameof(Background));
            lstbox.DataContext = lstbox.Parent;
        }
    }
}
