using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DuplicateUniformGrid
{
    class UniformGridAlmost : Panel
    {
        public static readonly DependencyProperty ColumnsProperty;

        public int Columns
        {
            set { SetValue(ColumnsProperty, value); }
            get { return (int)GetValue(ColumnsProperty); }            
        }

        public int Rows
        {
            get { return (InternalChildren.Count + Columns - 1) / Columns; }
        }

        static UniformGridAlmost()
        {
            ColumnsProperty = DependencyProperty.Register(nameof(Columns), typeof(int), typeof(UniformGridAlmost), new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsMeasure));
            //FrameworkPropertyMetadataOptions.AffectsMeasure:当Columns属性方式变化时,将会出发MeasureOverride方法
        }

        // 测量、均分空间
        protected override Size MeasureOverride(Size availableSize)
        {
            //根据统一的行与列,计算孩子的尺寸
            Size sizeChild = new Size(availableSize.Width / Columns, availableSize.Height / Rows);

            //存储孩子中最大的尺寸
            double maxwidth = 0;
            double maxheight = 0;

            /* 使用InternalChildren而不使用Children；
             * 因为InternalChildren不止包括正常的Children，还包括通过数据绑定加进来的孩子
             */ 

            foreach (UIElement child in InternalChildren)
            {
                //调用孩子的Measure－测量孩子的尺寸，在ArrangeOverride期间会用到
                child.Measure(sizeChild);

                //找出孩子中最大的DesiredSize
                maxwidth = Math.Max(maxwidth, child.DesiredSize.Width);
                maxheight = Math.Max(maxheight, child.DesiredSize.Height);
            }

            //现在为grid本身,计算想要的尺寸(desired size)
            return new Size(Columns * maxwidth, Rows * maxheight);            
        }

        //摆放孩子
        protected override Size ArrangeOverride(Size finalSize)
        {
            //根据统一的行与列,计算孩子的尺寸
            Size sizeChild = new Size(finalSize.Width / Columns, finalSize.Height / Rows);

            for (int index = 0; index < InternalChildren.Count; index++)
            {
                int row = index / Columns;
                int col = index % Columns;

                //计算每个孩子在finalSize内的矩形
                Rect rectChild = new Rect(new Point(col * sizeChild.Width, row * sizeChild.Height), sizeChild);

                //调用孩子的Arrange
                InternalChildren[index].Arrange(rectChild);
            }

            return finalSize;
        }
    }
}
