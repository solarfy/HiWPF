using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Documents;

namespace FormatRichText
{
    public partial class FormatRichTextWindow : Window
    {
        ToggleButton[] btnAlignments = new ToggleButton[4];

        void AddParaToolBar(ToolBarTray tray, int band, int index)
        {
            ToolBar toolbar = new ToolBar();
            toolbar.Band = band;
            toolbar.BandIndex = index;
            tray.ToolBars.Add(toolbar);

            toolbar.Items.Add(btnAlignments[0] = CreatButton(TextAlignment.Left, "Align Left", 0, 4));
            toolbar.Items.Add(btnAlignments[1] = CreatButton(TextAlignment.Center, "Center", 2, 2));
            toolbar.Items.Add(btnAlignments[2] = CreatButton(TextAlignment.Right, "Align Rigth", 4, 0));
            toolbar.Items.Add(btnAlignments[3] = CreatButton(TextAlignment.Justify, "Justify", 0, 0));

            this.txtbox.SelectionChanged += TextBoxOnSelectionChanged_Para;
        }

        ToggleButton CreatButton(TextAlignment align, string strTooltip, int offsetLeft, int offsetRight)
        {
            ToggleButton btn = new ToggleButton();
            btn.Tag = align;
            btn.Click += ButtonOnClick;

            Canvas canv = new Canvas();
            canv.Width = 16;
            canv.Height = 16;
            btn.Content = canv;

            for (int i = 0; i < 5; i++)
            {
                Polyline poly = new Polyline();
                poly.Stroke = SystemColors.WindowTextBrush;
                poly.StrokeThickness = 1;

                if ((i & 1) == 0)   //偶数
                {
                    poly.Points = new PointCollection(new Point[] 
                    {
                        new Point(2, 2 + 3 * i), new Point(14, 2 + 3 * i)
                    });
                }
                else
                {
                    poly.Points = new PointCollection(new Point[]
                    {
                        new Point(2 + offsetLeft, 2 + 3 * i), new Point(14 - offsetRight, 2 + 3 * i)
                    });
                }

                canv.Children.Add(poly);
            }

            ToolTip tip = new ToolTip();
            tip.Content = strTooltip;
            btn.ToolTip = tip;

            return btn;
        }

        void ButtonOnClick(object sender, RoutedEventArgs args)
        {
            ToggleButton btn = args.Source as ToggleButton;

            foreach (ToggleButton btnAlign in btnAlignments)
            {
                btn.IsChecked = (btn == btnAlign);
            }

            TextAlignment align = (TextAlignment)btn.Tag;
            this.txtbox.Selection.ApplyPropertyValue(Paragraph.TextAlignmentProperty, align);
        }

        void TextBoxOnSelectionChanged_Para(object sender, RoutedEventArgs args)
        {
            object obj = this.txtbox.Selection.GetPropertyValue(Paragraph.TextAlignmentProperty);

            if (obj != null && obj is TextAlignment align)
            {
                foreach (ToggleButton btn in btnAlignments)
                {
                    btn.IsChecked = (align == (TextAlignment)btn.Tag);
                }
            }
            else
            {
                foreach (ToggleButton btn in btnAlignments)
                {
                    btn.IsChecked = false;
                }
            }
        }
    }
}
