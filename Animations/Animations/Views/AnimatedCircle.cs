using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Animations.Views
{
    /* UIElement所定义的OnRender方法，或者使用DrawingVisual对象时，使用DrawingContext绘图。
     * DrawingContext定义的众多绘图方法，都有AnimationClock参数的overload版本。
     */

    class AnimatedCircle : FrameworkElement
    {
        protected override void OnRender(DrawingContext dc)
        {
            DoubleAnimation anima = new DoubleAnimation();
            anima.From = 0;
            anima.To = 100;
            anima.Duration = new Duration(TimeSpan.FromSeconds(1));
            anima.AutoReverse = true;
            anima.RepeatBehavior = RepeatBehavior.Forever;
            AnimationClock clock = anima.CreateClock();

            dc.DrawEllipse(Brushes.Blue, new Pen(Brushes.Red, 3), new Point(125, 125), null, 0, clock, 0, clock);
        }
    }
}
