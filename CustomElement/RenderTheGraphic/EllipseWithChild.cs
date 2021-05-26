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
    class EllipseWithChild : SimpleEllipse
    {
        /*
         * 执行次序 MessureOverride -> ArrangeOverride -> OnRender
         * 但反过来 OnRender并不一定要ArrangeOverride，ArrangeOveride并不一定要MeasureOverride
         */

        UIElement child;

        public UIElement Child
        {
            set
            {
                if (child != null)
                {
                    this.RemoveVisualChild(child);
                    this.RemoveLogicalChild(child);
                }

                if ((child = value) != null)
                {
                    this.AddVisualChild(child);
                    this.AddLogicalChild(child);
                }
            }
            get
            {
                return child;
            }
        }

        //Override of VisualChildrenCount，如果Child非null，返回1
        protected override int VisualChildrenCount
        {
            get
            {
                return child != null ? 1 : 0;
            }
        }

        //Override of GetVisualChildren，返回Child
        protected override Visual GetVisualChild(int index)
        {
            if (index > 0 || Child == null)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            return Child;
        }

        // Override of MeasureOverride，调用孩子的Measure的方法
        protected override Size MeasureOverride(Size availableSize)
        {
            Size sizeDesired = new Size(0, 0);  //所需尺寸(0到无穷大)
            if (Stroke != null)
            {
                sizeDesired.Width += 2 * Stroke.Thickness;
                sizeDesired.Height += 2 * Stroke.Thickness;

                //实际的可用尺寸需减去边框的所占用的尺寸
                availableSize.Width = Math.Max(0, availableSize.Width - 2 * Stroke.Thickness);
                availableSize.Height = Math.Max(0, availableSize.Height - 2 * Stroke.Thickness);
            }

            //如果有孩子，需调用孩子的Measure方法，再重新计算所需尺寸
            if (Child != null)
            {
                Child.Measure(availableSize);   //此方法会调用孩子的MeassureOverride方法
                sizeDesired.Width += Child.DesiredSize.Width;       //Child.DesiredeSize属性包含了Margin属性，所以此处可直接用来操作
                sizeDesired.Height += Child.DesiredSize.Height;
            }

            return sizeDesired;
        }

        //Override of ArrangeOverride，调用孩子的Arrange方法  摆放操作
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (Child != null)
            {
                Rect rect = new Rect(new Point((finalSize.Width - Child.DesiredSize.Width) / 2, (finalSize.Height - Child.DesiredSize.Height) / 2), Child.DesiredSize);
                Child.Arrange(rect);
            }

            return finalSize;
        }
    }
}
