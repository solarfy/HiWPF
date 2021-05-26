using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Globalization;

namespace RenderTheGraphic
{
    class SimpleEllipse : FrameworkElement
    {
        public static readonly DependencyProperty FillProperty;
        public static readonly DependencyProperty StrokeProperty;
        public static readonly DependencyProperty TextProperty;

        public Brush Fill
        {
            set { SetValue(FillProperty, value); }
            get { return (Brush)GetValue(FillProperty); }
        }

        public Pen Stroke
        {
            set { SetValue(StrokeProperty, value); }
            get { return (Pen)GetValue(StrokeProperty); }
        }

        public string Text
        {
            set { SetValue(TextProperty, value == null ? " " : value); }
            get { return (string)GetValue(TextProperty); }
        }

        static SimpleEllipse()
        {
            FillProperty = DependencyProperty.Register(nameof(Fill), typeof(Brush), typeof(SimpleEllipse), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
            //AffectsRender：当Fill property被改变时，InvalidateVisual会被调用，这又将导致OnRender被调用

            StrokeProperty = DependencyProperty.Register(nameof(Stroke), typeof(Pen), typeof(SimpleEllipse), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure));
            //AffectMeasure：当Stoke property被改变时，InvalidateMeasure会被调用，这又将导致MeasureOverride被调用

            TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(SimpleEllipse), new PropertyMetadata("Hello, ellipse!"));
        }


        protected override void OnRender(DrawingContext drawingContext)
        {
            //InvalidateVisual会强制调用OnRender。

            /* 除了OnRender方法可获取DrawingContext外，也可以同过DrawVisual的方式获取
             * DrawVisual drawvis = new DrawVisual();
             * DrawingContext dc = drawvis.RenderOpen();
             * //在dc上绘图
             *  ...
             * dc.Close(); 
             */ 

            Size size = RenderSize;

            //调整Pen的宽度
            if (Stroke != null)
            {
                size.Width = Math.Max(0, size.Width - Stroke.Thickness);
                size.Height = Math.Max(0, size.Height - Stroke.Thickness);
            }

            //绘制椭圆
            drawingContext.DrawEllipse(Fill, Stroke, new Point(RenderSize.Width / 2, RenderSize.Height / 2), size.Width / 2, size.Height / 2);

            //绘制文字
            FormattedText formtxt = new FormattedText(Text, CultureInfo.CurrentCulture, FlowDirection, new Typeface("Times New Roman Italic"), 24, Brushes.DarkBlue);
            Point ptText = new Point((RenderSize.Width - formtxt.Width) / 2, (RenderSize.Height - formtxt.Height) / 2);
            drawingContext.DrawText(formtxt, ptText);

            //设定裁剪区
            drawingContext.PushClip(new RectangleGeometry(new Rect(new Point(0, 0), RenderSize)));
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            //InvalidateMeasure会强制调用MeasureOverride


            Size sizeDesired = base.MeasureOverride(availableSize);

            //sizeDesired.Width = 0;      //此处设置值为0，并不表示Element会以如此的尺寸显示
            //sizeDesired.Height = 0;

            if (Stroke != null)
            {
                sizeDesired = new Size(Stroke.Thickness, Stroke.Thickness);
            }

            //当sizeDesired超过availableSize参数时,OnRender会进行裁剪

            return sizeDesired;
        }
    }
}
