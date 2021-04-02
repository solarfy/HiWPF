using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CircleTheButtons
{    
    class RadialPanel : Panel
    {
        public static readonly DependencyProperty OrientationProperty;

        bool showPieLines;
        double angleEach;
        Size sizeLargest;
        double radius;
        double outerEdgeFromCenter;
        double innerEdgeFromCenter;


        public RadialPanelOrientation Orientation
        {
            set { SetValue(OrientationProperty, value); }
            get { return (RadialPanelOrientation)GetValue(OrientationProperty); }
        }

        public bool ShowPieLines
        {
            set
            {
                if (value != showPieLines)
                    InvalidateVisual();

                showPieLines = value;
            }
            get
            {
                return showPieLines;
            }
        }

        static RadialPanel()
        {
            OrientationProperty = DependencyProperty.Register(nameof(Orientation), typeof(RadialPanelOrientation), typeof(RadialPanel), new FrameworkPropertyMetadata(RadialPanelOrientation.ByWidth, FrameworkPropertyMetadataOptions.AffectsParentMeasure));
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (InternalChildren.Count == 0)
                return new Size(0, 0);

            angleEach = 360.0 / InternalChildren.Count;
            sizeLargest = new Size(0, 0);

            foreach(UIElement child in InternalChildren)
            {
                child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));      //默认子元素的可用尺寸为正无穷大

                //找出期望尺寸最大的子元素
                sizeLargest.Width = Math.Max(sizeLargest.Width, child.DesiredSize.Width);
                sizeLargest.Height = Math.Max(sizeLargest.Height, child.DesiredSize.Height);
            }

            if (Orientation == RadialPanelOrientation.ByWidth)
            {
                //计算中心到Element边缘的距离
                innerEdgeFromCenter = sizeLargest.Width / 2 / Math.Tan(Math.PI * angleEach / 360);      
                outerEdgeFromCenter = innerEdgeFromCenter + sizeLargest.Height;

                //以最大孩子为基准，计算椭圆的半径
                radius = Math.Sqrt(Math.Pow(sizeLargest.Width / 2, 2) + Math.Pow(outerEdgeFromCenter, 2));
            }
            else
            {
                //计算中心到Element边缘的距离
                innerEdgeFromCenter = sizeLargest.Height / 2 / Math.Tan(Math.PI * angleEach / 360);
                outerEdgeFromCenter = innerEdgeFromCenter + sizeLargest.Width;

                //以最大孩子为基准，计算椭圆的半径
                radius = Math.Sqrt(Math.Pow(sizeLargest.Height / 2, 2) + Math.Pow(outerEdgeFromCenter, 2));
            }

            //返回该圆的尺寸
            return new Size(2 * radius, 2 * radius);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double angleChild = 0;
            Point ptCenter = new Point(finalSize.Width / 2, finalSize.Height / 2);  //最终尺寸的中心点
            double multiplier = Math.Min(finalSize.Width / (2 * radius), finalSize.Height / (2 * radius));  //缩放比 （最终尺寸 / 期望尺寸）

            foreach (UIElement child in InternalChildren)
            {
                //Reset RenderTransform
                child.RenderTransform = Transform.Identity; 

                if (Orientation == RadialPanelOrientation.ByWidth)
                {
                    //将孩子放在上边
                    child.Arrange(new Rect(ptCenter.X - multiplier * sizeLargest.Width / 2, ptCenter.Y - multiplier * outerEdgeFromCenter, multiplier * sizeLargest.Width, multiplier * sizeLargest.Height));
                }
                else
                {
                    //将孩子放在右边
                    child.Arrange(new Rect(ptCenter.X + multiplier * innerEdgeFromCenter, ptCenter.Y - multiplier * sizeLargest.Height / 2, multiplier * sizeLargest.Width, multiplier * sizeLargest.Height));
                }

                //旋转孩子
                Point pt = TranslatePoint(ptCenter, child);     //将中心点转换成相对于子元素的坐标 

                /*
                 * //实际计算相对点的方式 即将子元素左上角掉作为原点（0,0）
                if (Orientation == RadialPanelOrientation.ByWidth)
                {
                    double wx = ptCenter.X - (ptCenter.X - multiplier * sizeLargest.Width / 2);
                    double hy = ptCenter.Y - (ptCenter.Y - multiplier * outerEdgeFromCenter);
                }
                else
                {
                    double wx = ptCenter.X - (ptCenter.X + multiplier * innerEdgeFromCenter);
                    double hy = ptCenter.Y - (ptCenter.Y - multiplier * sizeLargest.Height / 2);
                }
                *
                */

                child.RenderTransform = new RotateTransform(angleChild, pt.X, pt.Y);
                
                //增加角度
                angleChild += angleEach;
            }

            return finalSize;
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            if (showPieLines)
            {
                Point ptCenter = new Point(RenderSize.Width / 2, RenderSize.Height / 2);
                double multiplier = Math.Min(RenderSize.Width / (2 * radius), RenderSize.Height / (2 * radius));
                Pen pen = new Pen(SystemColors.WindowTextBrush, 1);
                pen.DashStyle = DashStyles.Dash;

                //显示圆
                dc.DrawEllipse(null, pen, ptCenter, multiplier * radius, multiplier * radius);

                //初始化圆
                double angleChild = -angleEach / 2;
                if (Orientation == RadialPanelOrientation.ByWidth)
                    angleChild += 90;

                //遍历子元素，从中心绘制放射线
                foreach (UIElement child in InternalChildren)
                {
                    dc.DrawLine(pen, ptCenter, new Point(ptCenter.X + multiplier * radius * Math.Cos(2 * Math.PI * angleChild / 360), ptCenter.Y + multiplier * radius * Math.Sin(2 * Math.Sin(2 * Math.PI * angleChild / 360))));
                    angleChild += angleEach;
                }

            }
        }
    }
}
