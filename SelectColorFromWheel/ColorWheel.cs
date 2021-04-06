using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Reflection;
using System.Windows.Data;

namespace SelectColorFromWheel
{
    class ColorWheel : ListBox
    {
        public ColorWheel()
        {
            FrameworkElementFactory factoryRadialPanel = new FrameworkElementFactory(typeof(RadialPanel));
            this.ItemsPanel = new ItemsPanelTemplate(factoryRadialPanel);

            DataTemplate template = new DataTemplate(typeof(Brush));
            this.ItemTemplate = template;

            FrameworkElementFactory elRectangle = new FrameworkElementFactory(typeof(Rectangle));
            elRectangle.SetValue(Rectangle.WidthProperty, 9.0);
            elRectangle.SetValue(Rectangle.HeightProperty, 20.0);
            elRectangle.SetValue(Rectangle.MarginProperty, new Thickness(1, 8, 1, 8));
            elRectangle.SetBinding(Rectangle.FillProperty, new Binding(""));

            template.VisualTree = elRectangle;

            PropertyInfo[] props = typeof(Brushes).GetProperties();
            foreach (PropertyInfo prop in props)
            {
                this.Items.Add((Brush)prop.GetValue(null, null));
            }
        }
    }
}
