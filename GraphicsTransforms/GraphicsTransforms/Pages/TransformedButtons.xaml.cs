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

namespace GraphicsTransforms.Pages
{
    /// <summary>
    /// TransformedButtons.xaml 的交互逻辑
    /// </summary>
    public partial class TransformedButtons : Page
    {
        public TransformedButtons()
        {
            InitializeComponent();

            AddTransformedButtons();
        }

        public void AddTransformedButtons()
        {
            Button btn = new Button();
            btn.Content = "Untransformed";
            canv.Children.Add(btn);
            Canvas.SetLeft(btn, 50);
            Canvas.SetTop(btn, 300);

            btn = new Button();
            btn.Content = "Translated";
            btn.RenderTransform = new TranslateTransform(-100, 150);
            canv.Children.Add(btn);
            Canvas.SetLeft(btn, 200);
            Canvas.SetTop(btn, 300);

            btn = new Button();
            btn.Content = "Scaled";
            btn.RenderTransform = new ScaleTransform(1.5, 4);
            canv.Children.Add(btn);
            Canvas.SetLeft(btn, 350);
            Canvas.SetTop(btn, 300);

            btn = new Button();
            btn.Content = "Skewed";
            btn.RenderTransform = new SkewTransform(0, 20);
            canv.Children.Add(btn);
            Canvas.SetLeft(btn, 500);
            Canvas.SetTop(btn, 300);

            btn = new Button();
            btn.Content = "Rotated";
            btn.RenderTransform = new RotateTransform(-30);
            canv.Children.Add(btn);
            Canvas.SetLeft(btn, 650);
            Canvas.SetTop(btn, 300);
        }
    }
}
