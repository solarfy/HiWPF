﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace ListColorNames
{
    class ColorListBoxItem : ListBoxItem
    {
        string str;
        Rectangle rect;
        TextBlock text;

        public ColorListBoxItem()
        {
            StackPanel stack = new StackPanel();
            stack.Orientation = Orientation.Horizontal;
            this.Content = stack;

            //建立矩形，以显示颜色
            rect = new Rectangle();
            rect.Width = 16;
            rect.Height = 16;
            rect.Margin = new Thickness(2);
            rect.Stroke = SystemColors.WindowTextBrush;
            stack.Children.Add(rect);

            //建立TextBlock，以显示颜色名称
            text = new TextBlock();
            text.VerticalAlignment = VerticalAlignment.Center;
            stack.Children.Add(text);
        }

        public string Text
        {
            set
            {
                str = value;
                string strSpace = str[0].ToString();

                for (int i = 1; i < str.Length; i++)
                {
                    strSpace += (char.IsUpper(str[i]) ? " " : "") + str[i].ToString();
                }

                text.Text = strSpace;
            }

            get => str;
        }

        public Color Color
        {
            set { rect.Fill = new SolidColorBrush(value); }
            get 
            {
                SolidColorBrush brush = rect.Fill as SolidColorBrush;
                return brush == null ? Colors.Transparent : brush.Color;
            }
        }

        //当项目被选中
        protected override void OnSelected(RoutedEventArgs e)
        {
            base.OnSelected(e);
            text.FontWeight = FontWeights.Bold;
        }
        //当项目被反选
        protected override void OnUnselected(RoutedEventArgs e)
        {
            base.OnUnselected(e);
            text.FontWeight = FontWeights.Regular;
        }

        public override string ToString()
        {
            return str;
        }

    }
}
