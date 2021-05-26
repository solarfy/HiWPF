using System.Windows;
using System.Windows.Controls;

namespace DuplicateCanvas
{
    class CanvasAlmost : Panel
    {
        public static readonly DependencyProperty LeftProperty;
        public static readonly DependencyProperty TopProperty;

        //attached property 附加属性 静态方法
        public static void SetLeft(DependencyObject obj, double value)
        {
            obj.SetValue(LeftProperty, value);
        }
        public static double GetLeft(DependencyObject obj)
        {
            return (double)obj.GetValue(LeftProperty);
        }

        public static void SetTop(DependencyObject obj, double value)
        {
            obj.SetValue(TopProperty, value);
        }
        public static double GetTop(DependencyObject obj)
        {
            return (double)obj.GetValue(TopProperty);
        }


        static CanvasAlmost()
        {
            LeftProperty = DependencyProperty.Register("Left", typeof(double), typeof(CanvasAlmost), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsParentArrange));
            TopProperty = DependencyProperty.Register("Top", typeof(double), typeof(CanvasAlmost), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsParentArrange));
            //NOTE：此处是AffectsParentArrange，不管何时property改变了，都会影响父亲的安排。类似的还有AffectsParentMeasure
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (UIElement child in InternalChildren)
            {
                //调用子元素Measure方法--为后续的ArrangeOverrid方法中提供DesiredSize
                child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));      //默认子元素无穷大
            }

            //返回初始的尺寸（默认为0）
            return base.MeasureOverride(availableSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (UIElement child in InternalChildren)
            {
                child.Arrange(new Rect(new Point(GetLeft(child), GetTop(child)), child.DesiredSize));
            }

            return finalSize;
        }

    }
}
