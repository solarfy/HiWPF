using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SelectColor
{
    class ColorCell : FrameworkElement
    {
        static readonly Size sizeCell = new Size(42, 42);
        DrawingVisual visColor;
        Brush brush;

        public static readonly DependencyProperty IsSelectedProperty;
        public static readonly DependencyProperty IsHighlightedPropetry;

        public Brush Brush
        {
            get => brush;
        }

        //选中
        public bool IsSelected 
        {
            set { SetValue(IsSelectedProperty, value); }
            get { return (bool)GetValue(IsSelectedProperty); }
        }

        //加亮显示
        public bool IsHighlighted
        {
            set { SetValue(IsHighlightedPropetry, value); }
            get { return (bool)GetValue(IsHighlightedPropetry); }
        }

        static ColorCell()
        {
            IsSelectedProperty = DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(ColorCell), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));
            IsHighlightedPropetry = DependencyProperty.Register(nameof(IsHighlighted), typeof(bool), typeof(ColorCell), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));
        }

        //构造函数需要Color参数
        public ColorCell(Color clr)
        {
            //建立一个新的DrawingVisual，且储存在字段中
            visColor = new DrawingVisual();     //该对象不能调用Measure和和Arrange方法,因为这些方式是由UIElement中所定义的(UIElement与DrawingVisual都是Visual的子类)
            DrawingContext dc = visColor.RenderOpen();      //除了OnRender方法中获取DrawingContext对象，此处是唯一一个获取到DrawingContext对象的方式

            //使用颜色参数，绘制一个矩形
            Rect rect = new Rect(new Point(0, 0), sizeCell);
            rect.Inflate(-4, -4);   //使方格进行指定的高度和宽度进行展开和缩放,此处为缩放
            Pen pen = new Pen(SystemColors.ControlTextBrush, 1);
            brush = new SolidColorBrush(clr);
            dc.DrawRectangle(Brush, pen, rect);     //Brush方格背景色,pen方格边框色
            dc.Close();

            //AddVisualChild对于event routing来说是有必要的
            this.AddVisualChild(visColor);
            this.AddLogicalChild(visColor);
        }

        protected override int VisualChildrenCount
        {
            get => 1;
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index > 0)
                throw new ArgumentOutOfRangeException("index");

            return visColor;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            return sizeCell;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            /* 在指定的方格上画出一个背景框,并该背景框大于该方格              
             */

            Rect rect = new Rect(new Point(0, 0), this.RenderSize);
            rect.Inflate(-1, -1);   //缩放长度与高度小于实际方格
            Pen pen = new Pen(SystemColors.HighlightBrush, 1);
            if (IsHighlighted)
            {
                drawingContext.DrawRectangle(SystemColors.ControlDarkBrush, pen, rect);
            }
            else if (IsSelected)
            {
                drawingContext.DrawRectangle(SystemColors.ControlLightBrush, pen, rect);
            }
            else
            {
                drawingContext.DrawRectangle(Brushes.Transparent, null, rect);
            }
        }
    }
}
