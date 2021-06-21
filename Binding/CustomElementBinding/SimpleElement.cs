using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Globalization;

namespace CustomElementBinding
{
    class SimpleElement : FrameworkElement
    {
        /* 注：想要顶一个dependency property，不一定要继承自FrameworkElement。
         * 如果数据源不是一个视觉（Visual）对象。可以继承自DependencyObject这个类，该类提供了你要用来实现dependency property所需的SetValue和GetValue方法。        
         */ 
        public static DependencyProperty NumberProperty;

        static SimpleElement()
        {
            NumberProperty = DependencyProperty.Register(nameof(Number), typeof(double), typeof(SimpleElement), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));
            //FrameworkPropertyMetadataOptions.NotDataBindable  不允许将数据绑定到此依赖属性。 其他元素仍然可以绑定到这个依赖属性，但是无法在该属性本身定义一个数据绑定，即该属性不可以当成数据绑定的目标，可当做源。
            //FrameworkPropertyMetadataOptions.BindsTwoWayByDefault  此依赖属性上的数据绑定的 System.Windows.Data.BindingMode 默认为 System.Windows.Data.BindingMode.TwoWay；只会影响该属性为目标的绑定。
        }

        public double Number
        {
            set => SetValue(NumberProperty, value); 
            get => (double)GetValue(NumberProperty);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            return new Size(200, 50);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawText(new FormattedText(Number.ToString(), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Times New Roman"), 12, SystemColors.WindowTextBrush)
                                    , new Point(0, 0));
        }
    }
}
