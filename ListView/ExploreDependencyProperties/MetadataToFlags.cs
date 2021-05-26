using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace ExploreDependencyProperties
{
    class MetadataToFlags : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            FrameworkPropertyMetadataOptions flags = 0;
            FrameworkPropertyMetadata metadata = value as FrameworkPropertyMetadata;

            if (metadata == null)
            {
                return null;
            }

            /* FrameworkPropertyMetadataOptions 是一个位枚举，所以可以使用位操作 
             * 位枚举的值类似于 0, 1, 2, 4, 8....  即 0, 1<<0, 1<<1, 1<<2, 1<<3 ...             
             */ 

            if (metadata.AffectsMeasure)
            {
                flags |= FrameworkPropertyMetadataOptions.AffectsMeasure;
            }
            if (metadata.AffectsArrange)
            {
                flags |= FrameworkPropertyMetadataOptions.AffectsArrange;
            }
            if (metadata.AffectsParentMeasure)
            {
                flags |= FrameworkPropertyMetadataOptions.AffectsParentMeasure;
            }
            if (metadata.AffectsParentArrange)
            {
                flags |= FrameworkPropertyMetadataOptions.AffectsParentArrange;
            }
            if (metadata.AffectsRender)
            {
                flags |= FrameworkPropertyMetadataOptions.AffectsRender;
            }
            if (metadata.Inherits)
            {
                flags |= FrameworkPropertyMetadataOptions.Inherits;
            }
            if (metadata.OverridesInheritanceBehavior)
            {
                flags |= FrameworkPropertyMetadataOptions.OverridesInheritanceBehavior;
            }
            if (metadata.IsNotDataBindable)
            {
                flags |= FrameworkPropertyMetadataOptions.NotDataBindable;
            }
            if (metadata.BindsTwoWayByDefault)
            {
                flags |= FrameworkPropertyMetadataOptions.BindsTwoWayByDefault;
            }
            if (metadata.Journal)
            {
                flags |= FrameworkPropertyMetadataOptions.Journal;
            }

            return flags;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new FrameworkPropertyMetadata(null, (FrameworkPropertyMetadataOptions)value);
        }
    }
}
