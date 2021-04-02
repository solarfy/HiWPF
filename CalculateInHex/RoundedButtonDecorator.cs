using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CalculateInHex
{
    class RoundedButtonDecorator : Decorator
    {
        /*  Decorator类中自带Child属性
         * 
         * 
         */ 

        public static readonly DependencyProperty IsPressedProperty;

        public bool IsPressed
        {
            set { SetValue(IsPressedProperty, value); }
            get { return (bool)GetValue(IsPressedProperty); }
        }

        static RoundedButtonDecorator()
        {
            IsPressedProperty = DependencyProperty.Register(nameof(IsPressed), typeof(bool), typeof(RoundedButtonDecorator), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));
            //将IsPressProperty示置成AffectsRender，当该属性变化时将会执行OnRender方法
        }

        protected override Size MeasureOverride(Size constraint)
        {
            Size desiredSize = new Size(2, 2);
            constraint.Width -= 2;
            constraint.Height -= 2;

            if (Child != null)
            {
                Child.Measure(constraint);
                desiredSize.Width += Child.DesiredSize.Width;
                desiredSize.Height += Child.DesiredSize.Height;
            }

            return desiredSize;
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            if (Child != null)
            {
                Point ptChild = new Point(Math.Max(1, (arrangeSize.Width - Child.DesiredSize.Width) / 2), Math.Max(1, (arrangeSize.Height - Child.DesiredSize.Height) / 2));
                Child.Arrange(new Rect(ptChild, Child.DesiredSize));
            }

            return arrangeSize;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            RadialGradientBrush brush = new RadialGradientBrush(IsPressed ? SystemColors.ControlDarkColor : SystemColors.ControlLightColor, SystemColors.ControlColor);
            brush.GradientOrigin = IsPressed ? new Point(0.75, 0.75) : new Point(0.25, 0.25);

            drawingContext.DrawRoundedRectangle(brush, new Pen(SystemColors.ControlDarkDarkBrush, 1), new Rect(new Point(0, 0), RenderSize), RenderSize.Height / 2, RenderSize.Height / 2);     //RenderSize 渲染尺寸
        }
    }

}
