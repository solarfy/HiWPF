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
using System.Windows;

namespace DependencyPro
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly DependencyProperty SpaceProperty;

        public int Space
        {
            get => (int)GetValue(SpaceProperty);

            set => SetValue(SpaceProperty, value);
        }

        static MainWindow()
        {
            FrameworkPropertyMetadata metadata = new FrameworkPropertyMetadata();
            metadata.Inherits = true;

            /* 注：此处是另一个“拥有者”加入到SpaceButton类注册的Space property。
             * 当一个类将另一个“拥有者”加入之前注册的dependedcy propferty时，原始的metadata不会被采用，此类必须建立自己的metadata。
             */ 
            SpaceProperty = SpaceButton.SpaceProperty.AddOwner(typeof(MainWindow));
            SpaceProperty.OverrideMetadata(typeof(MainWindow), metadata);
        }


        public MainWindow()
        {
            InitializeComponent();

            this.Title = "Set Space Property";
            
            this.SizeToContent = SizeToContent.WidthAndHeight;
            this.ResizeMode = ResizeMode.CanMinimize;
            int[] iSpaces = { 0, 1, 2 };

            Grid grid = new Grid();
            this.Content = grid;

            for (int i = 0; i < 2; i++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = GridLength.Auto;
                grid.RowDefinitions.Add(row);
            }

            for (int i = 0; i < iSpaces.Length; i++)
            {
                ColumnDefinition col = new ColumnDefinition();
                col.Width = GridLength.Auto;
                grid.ColumnDefinitions.Add(col);
            }

            for (int i = 0; i < iSpaces.Length; i++)
            {
                SpaceButton btn = new SpaceButton();
                btn.Text = $"Set window Space to {iSpaces[i]}";
                btn.Tag = iSpaces[i];
                btn.VerticalAlignment = VerticalAlignment.Center;
                btn.HorizontalAlignment = HorizontalAlignment.Center;
                btn.Click += WindowPropertyOnClick;
                btn.Height = 100;
                grid.Children.Add(btn);
                Grid.SetRow(btn, 0);
                Grid.SetColumn(btn, i);

                btn = new SpaceButton();
                btn.Text = $"Set button Space to {iSpaces[i]}";
                btn.Tag = iSpaces[i];
                btn.VerticalAlignment = VerticalAlignment.Center;
                btn.HorizontalAlignment = HorizontalAlignment.Center;
                btn.Click += ButtonPropertyOnClick;
                btn.Height = 100;
                grid.Children.Add(btn);
                Grid.SetRow(btn, 1);
                Grid.SetColumn(btn, i);
            }
        }

        private void WindowPropertyOnClick(object sender, RoutedEventArgs e)
        {
            //假设窗体中同类按钮未独立设置该属性时，将窗体中所有同类按钮设置为相同属性，独立设置过属性的按钮将无效果，类似于FontSize的设置
            SpaceButton btn = e.Source as SpaceButton;
            Space = (int)btn.Tag;            
        }

        private void ButtonPropertyOnClick(object sender, RoutedEventArgs e)
        {
            SpaceButton btn = e.Source as SpaceButton;
            //btn.Space = (int)btn.Tag;
            SpaceButton.SetSpaceCount(btn, (int)btn.Tag);       //通过附加属性的方式修改             
        }
    }
}
