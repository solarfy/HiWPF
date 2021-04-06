﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SelectColorFromGrid
{
    class ColorGridBox : ListBox
    {
        string[] strColors =
        {
           "Black","Brown", "DarkGreen", "MidnightBlue", "Navy", "DarkBlue", "Indigo", "DimGray",
           "DarkRed", "OrangeRed", "Olive", "Green", "Teal", "Blue", "SlateGray", "Gray",
           "Red", "Orange", "YellowGreen", "SeaGreen", "Aqua", "LightBlue", "Violet", "DarkGray",
           "Pink", "Gold", "Yellow", "Lime", "Turquoise", "SkyBlue", "Plum", "LightGray" ,
           "LightPink", "Tan", "LightYellow", "LightGreen", "LightCyan", "LightSkyBlue", "Lavender", "White"
        };

        public ColorGridBox()
        {
            //定义ItemPanel template 使ListBox使用UniformGrid面板
            FrameworkElementFactory factroyUngrid = new FrameworkElementFactory(typeof(UniformGrid));
            factroyUngrid.SetValue(UniformGrid.ColumnsProperty, 8);
            this.ItemsPanel = new ItemsPanelTemplate(factroyUngrid);

            //将项目添加到ListBox
            foreach (string strColor in strColors)
            {
                //建立Rectangle，将它加到ListBox
                Rectangle rect = new Rectangle();
                rect.Width = 36;
                rect.Height = 36;
                rect.Margin = new Thickness(4);
                rect.Fill = (Brush)typeof(Brushes).GetProperty(strColor).GetValue(null, null);
                this.Items.Add(rect);

                //为Rectangel建立ToolTip
                ToolTip tip = new ToolTip();
                tip.Content = strColor;
                rect.ToolTip = tip;
            }

            //指出SelectedValue是Rectange项目Fill属性
            this.SelectedValuePath = nameof(Shape.Fill);
        }
    }
}
