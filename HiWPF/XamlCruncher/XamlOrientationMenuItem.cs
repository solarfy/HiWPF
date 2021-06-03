using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Globalization;

namespace XamlCruncher
{
    class XamlOrientationMenuItem : MenuItem
    {
        MenuItem itemChecked;
        Grid grid;
        TextBox txtbox;
        Frame frame;

        public Dock Oreientation
        {
            set
            {
                foreach (MenuItem item in this.Items)
                    if (item.IsChecked = (value == (Dock)item.Tag))
                        itemChecked = item;
            }

            get => (Dock)itemChecked.Tag;
        }

        public XamlOrientationMenuItem(Grid grid, TextBox txtbox, Frame frame)
        {
            this.grid = grid;
            this.txtbox = txtbox;
            this.frame = frame;

            this.Header = "_Orientation";

            for (int i = 0; i < 4; i++)
                this.Items.Add(CreateItem((Dock)i));

            (itemChecked = (MenuItem)this.Items[0]).IsChecked = true;
        }

        MenuItem CreateItem(Dock dock)
        {
            MenuItem item = new MenuItem();
            item.Tag = dock;
            item.Click += ItemOnClick;
            item.Checked += ItemOnCheck;

            FormattedText formtxt1 = CreateFormattedText("Edit");
            FormattedText formtxt2 = CreateFormattedText("Display");
            double widthMax = Math.Max(formtxt1.Width, formtxt2.Width);

            DrawingVisual vis = new DrawingVisual();
            DrawingContext dc = vis.RenderOpen();

            switch(dock)
            {
                case Dock.Left:
                    BoxText(dc, formtxt1, formtxt1.Width, new Point(0, 0));
                    BoxText(dc, formtxt2, formtxt2.Width, new Point(formtxt1.Width + 4, 0));
                    break;

                case Dock.Top:
                    BoxText(dc, formtxt1, widthMax, new Point(0, 0));
                    BoxText(dc, formtxt2, widthMax, new Point(0, formtxt1.Height + 4));
                    break;

                case Dock.Right:
                    BoxText(dc, formtxt2, formtxt2.Width, new Point(0, 0));
                    BoxText(dc, formtxt1, formtxt1.Width, new Point(formtxt2.Width + 4, 0));
                    break;

                case Dock.Bottom:
                    BoxText(dc, formtxt2, widthMax, new Point(0, 0));
                    BoxText(dc, formtxt1, widthMax, new Point(0, formtxt2.Height + 4));
                    break;                    
            }

            dc.Close();

            DrawingImage drawing = new DrawingImage(vis.Drawing);
            Image img = new Image();
            img.Source = drawing;

            item.Header = img;  //将菜单标题设置为该页图像

            return item;
        }

        FormattedText CreateFormattedText(string str)
        {
            return new FormattedText(str, CultureInfo.CurrentCulture, FlowDirection.LeftToRight
                                , new Typeface(SystemFonts.MenuFontFamily, SystemFonts.MenuFontStyle, SystemFonts.MenuFontWeight, FontStretches.Normal)
                                , SystemFonts.MenuFontSize
                                , SystemColors.MenuTextBrush);
        }

        void BoxText(DrawingContext dc, FormattedText formtxt, double width, Point pt)
        {
            Pen pen = new Pen(SystemColors.MenuTextBrush, 1);

            dc.DrawRectangle(null, pen, new Rect(pt.X, pt.Y, width + 4, formtxt.Height + 4));
            double x = pt.X + (width - formtxt.Width) / 2;
            dc.DrawText(formtxt, new Point(x + 2, pt.Y + 2));
        }

        void ItemOnClick(object sender, RoutedEventArgs args)
        {
            itemChecked.IsChecked = false;
            itemChecked = args.Source as MenuItem;
            itemChecked.IsChecked = true;
        }

        void ItemOnCheck(object sender, RoutedEventArgs args)
        {
            MenuItem itemChecked = args.Source as MenuItem;

            for (int i = 1; i < 3; i++)
            {
                grid.RowDefinitions[i].Height = new GridLength(0);
                grid.ColumnDefinitions[i].Width = new GridLength(0);
            }

            Grid.SetRow(txtbox, 0);
            Grid.SetColumn(txtbox, 0);
            Grid.SetRow(frame, 0);
            Grid.SetColumn(frame, 0);

            switch ((Dock)itemChecked.Tag)
            {
                case Dock.Left:
                    grid.ColumnDefinitions[1].Width = GridLength.Auto;
                    grid.ColumnDefinitions[2].Width = new GridLength(100, GridUnitType.Star);
                    Grid.SetColumn(frame, 2);
                    break;

                case Dock.Top:
                    grid.RowDefinitions[1].Height = GridLength.Auto;
                    grid.RowDefinitions[2].Height = new GridLength(100, GridUnitType.Star);
                    Grid.SetRow(frame, 2);
                    break;

                case Dock.Right:
                    grid.ColumnDefinitions[1].Width = GridLength.Auto;
                    grid.ColumnDefinitions[2].Width = new GridLength(100, GridUnitType.Star);
                    Grid.SetColumn(txtbox, 2);
                    break;

                case Dock.Bottom:
                    grid.RowDefinitions[1].Height = GridLength.Auto;
                    grid.RowDefinitions[2].Height = new GridLength(100, GridUnitType.Star);
                    Grid.SetRow(txtbox, 2);
                    break;
            }
        }
    }
}
