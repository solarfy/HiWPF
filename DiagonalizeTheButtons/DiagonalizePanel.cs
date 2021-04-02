using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DiagonalizeTheButtons
{
    class DiagonalizePanel : FrameworkElement
    {
        List<UIElement> children = new List<UIElement>();
        Size sizeChildrenTotal;

        public static readonly DependencyProperty BackgroundProperty;
        
        public Brush Background
        {
            set { SetValue(BackgroundProperty, value); }
            get { return (Brush)(GetValue(BackgroundProperty)); }
        }

        static DiagonalizePanel()
        {
            BackgroundProperty = DependencyProperty.Register(nameof(Background), typeof(Brush), typeof(DiagonalizePanel), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
        }

        public void Add(UIElement el)
        {
            children.Add(el);
            AddVisualChild(el);
            AddLogicalChild(el);
            InvalidateVisual();     //调用OnRender方法
        }

        public void Remove(UIElement el)
        {
            children.Remove(el);
            AddVisualChild(el);
            AddLogicalChild(el);
            InvalidateVisual();
        }

        public int IndexOf(UIElement el)
        {
            return children.IndexOf(el);
        }

        protected override int VisualChildrenCount
        {
            get { return children.Count; }
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index > children.Count)
                throw new ArgumentOutOfRangeException("index");

            return children[index];
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            sizeChildrenTotal = new Size(0, 0);

            foreach (UIElement child in children)
            {
                //调用每个孩子的Measure...
                child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

                //找出孩子的DesiredSize property
                sizeChildrenTotal.Width += child.DesiredSize.Width;
                sizeChildrenTotal.Height += child.DesiredSize.Height;
            }

            return sizeChildrenTotal;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Point ptChild = new Point(0, 0);        //孩子的摆放位置

            foreach (UIElement child in children)
            {
                Size sizeChild = new Size(0, 0);    //孩子的最终尺寸
                sizeChild.Width = child.DesiredSize.Width * (finalSize.Width / sizeChildrenTotal.Width);        //缩放孩子到面板上的最终尺寸 （finalSize面板的最终尺寸  sizeChildrenTotal面板的期望尺寸  child.DesiredSize孩子的期望尺寸）
                sizeChild.Height = child.DesiredSize.Height * (finalSize.Height / sizeChildrenTotal.Width);

                child.Arrange(new Rect(ptChild, sizeChild));        
                ptChild.X += sizeChild.Width;
                ptChild.Y += sizeChild.Height;
            }

            return finalSize;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(Background, null, new Rect(new Point(0, 0), RenderSize));
        }
    }
}
