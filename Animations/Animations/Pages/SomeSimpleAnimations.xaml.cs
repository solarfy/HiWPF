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
using System.Windows.Media.Animation;

namespace Animations.Pages
{
    /// <summary>
    /// SomeSimpleAnimations.xaml 的交互逻辑
    /// </summary>
    public partial class SomeSimpleAnimations : Page
    {
        const double initFontSize = 12;
        const double maxFontSize = 48;
        Button btn;

        public SomeSimpleAnimations()
        {
            InitializeComponent();
            EnlargeButtonWithAnimation();
        }

        void EnlargeButtonWithAnimation()
        {
            btn = new Button();
            btn.Content = "Expanding Button";
            btn.FontSize = initFontSize;
            btn.HorizontalAlignment = HorizontalAlignment.Center;
            btn.VerticalAlignment = VerticalAlignment.Center;
            btn.Click += ButtonOnClick;
            this.grid.Children.Add(btn);
        }

        private void ButtonOnClick(object sender, RoutedEventArgs e)
        {
            DoubleAnimation anima = new DoubleAnimation();
            anima.Duration = new Duration(TimeSpan.FromSeconds(2));
            anima.From = initFontSize;
            anima.To = maxFontSize;
            anima.FillBehavior = FillBehavior.Stop; //HoldEnd:保持最后的状态；Stop:恢复到动画开始前的状态
            btn.BeginAnimation(Button.FontSizeProperty, anima);     //必须是依赖属性

            //替代BeginAnimation方法 使用ApplyAnimationClock，该方法由UIElement定义
            //btn.ApplyAnimationClock(Button.FontSizeProperty, anima.CreateClock());  //CreatClock方法由AnimationTimeline定义
        }
    }
}
