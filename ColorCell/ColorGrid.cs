using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Input;

namespace ColorCell
{
    class ColorGrid : Control
    {
        //行于列的个数
        const int yNum = 5;
        const int xNum = 8;

        /* 多维数组与交错数组(亦称锯齿数组):
         * 多维数组每个维度的长度都相同;交错数组即数组的数组,其每一项的长度可不相同.         
         */

        //要显示的颜色
        string[,] strColors = new string[yNum, xNum]
        {
            {"Black","Brown", "DarkGreen", "MidnightBlue", "Navy", "DarkBlue", "Indigo", "DimGray"},
            {"DarkRed", "OrangeRed", "Olive", "Green", "Teal", "Blue", "SlateGray", "Gray" },
            { "Red", "Orange", "YellowGreen", "SeaGreen", "Aqua", "LightBlue", "Violet", "DarkGray"},
            { "Pink", "Gold", "Yellow", "Lime", "Turquoise", "SkyBlue", "Plum", "LightGray" },
            { "LightPink", "Tan", "LightYellow", "LightGreen", "LightCyan", "LightSkyBlue", "Lavender", "White" }
        };

        //要建立的ColorCell对象
        ColorCell[,] cells = new ColorCell[yNum, xNum];
        ColorCell cellSelected;
        ColorCell cellHighlighted;

        //组成此控件的element
        Border bord;
        UniformGrid unGrid;

        //目前选取的颜色
        Color clrSelected = Colors.Black;   //默认为第一个颜色

        //Public "Changed" event
        public event EventHandler SelectedColorChanged;
        
        public Color SelectedColor
        {
            get => clrSelected;
        }

        public ColorGrid()
        {
            //为此控件建立一个Border
            bord = new Border();
            bord.BorderBrush = SystemColors.ControlDarkDarkBrush;
            bord.BorderThickness = new Thickness(1);
            this.AddVisualChild(bord);      //路由事件所必需环节
            this.AddLogicalChild(bord);

            //建立UniformGrid,作为Border的孩子
            unGrid = new UniformGrid();
            unGrid.Background = SystemColors.WindowBrush;
            unGrid.Columns = xNum;
            bord.Child = unGrid;
            
            //用多个ColorCell对象填满UniformGrid
            for(int y = 0; y < yNum; y++)
            {
                for (int x = 0; x < xNum; x++)
                {
                    Color clr = (Color)typeof(Colors).GetProperty(strColors[y, x]).GetValue(null, null);

                    cells[y, x] = new ColorCell(clr);
                    unGrid.Children.Add(cells[y, x]);

                    if (clr == SelectedColor)
                    {
                        cellSelected = cells[y, x];
                        cells[y, x].IsSelected = true;
                    }

                    ToolTip tip = new ToolTip();
                    tip.Content = strColors[y, x];
                    cells[y, x].ToolTip = tip;
                }
            }
        }

        protected override int VisualChildrenCount
        {
            get => 1;
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index > 0)
                throw new ArgumentOutOfRangeException("index");

            return bord;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            bord.Measure(constraint);       
            return bord.DesiredSize;
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            bord.Arrange(new Rect(new Point(0, 0), arrangeBounds));
            return arrangeBounds;
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);

            if (cellHighlighted != null)
            {
                cellHighlighted.IsHighlighted = false;
                cellHighlighted = null;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            ColorCell cell = e.Source as ColorCell;

            if (cell != null)
            {
                if (cellHighlighted != null)
                    cellHighlighted.IsHighlighted = false;

                cellHighlighted = cell;
                cellHighlighted.IsHighlighted = true;
            }
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            if (cellHighlighted != null)
            {
                cellHighlighted.IsHighlighted = false;
                cellHighlighted = null;
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            ColorCell cell = e.Source as ColorCell;

            if (cell != null)
            {
                if (cellHighlighted != null)
                    cellHighlighted.IsSelected = false;

                cellHighlighted = cell;
                cellHighlighted.IsSelected = true;
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            ColorCell cell = e.Source as ColorCell;

            if (cell != null)
            {
                if (cellSelected != null)
                    cellSelected.IsSelected = false;

                cellSelected = cell;
                cellSelected.IsSelected = true;

                clrSelected = (cellSelected.Brush as SolidColorBrush).Color;
                OnSelectedColorChanged(EventArgs.Empty);
            }
        }

        //键盘事件处理
        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);

            if (cellHighlighted == null)
            {
                if (cellSelected != null)
                    cellHighlighted = cellSelected;
                else
                    cellHighlighted = cells[0, 0];

                cellHighlighted.IsHighlighted = true;
            }
        }

        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnLostKeyboardFocus(e);

            if (cellHighlighted != null)
            {
                cellHighlighted.IsHighlighted = false;
                cellHighlighted = null;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            int index = unGrid.Children.IndexOf(cellHighlighted);
            int y = index / xNum;
            int x = index % xNum;

            switch (e.Key)
            {
                case Key.Home:
                    y = 0;
                    x = 0;
                    break;

                case Key.End:
                    y = yNum - 1;
                    x = xNum - 1;
                    break;

                case Key.Down:
                    if ((y = (y + 1) % yNum) == 0)
                        x++;
                    break;

                case Key.Up:
                    if ((y = (y + yNum - 1) % yNum) == yNum - 1)
                        x--;
                    break;

                case Key.Right:
                    if ((x = (x + 1) % xNum) == 0)
                        y++;
                    break;

                case Key.Left:
                    if ((x = (x + xNum - 1) % xNum) == xNum - 1)
                        y--;
                    break;

                case Key.Enter:
                case Key.Space:
                    if (cellSelected != null)
                        cellSelected.IsSelected = false;

                    cellSelected = cellHighlighted;
                    cellSelected.IsSelected = true;
                    clrSelected = (cellSelected.Brush as SolidColorBrush).Color;
                    OnSelectedColorChanged(EventArgs.Empty);
                    break;

                default:
                    return;
            }

            if (x >= xNum || y >= yNum)
                MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));     //将焦点移动到下一个控件
            else if (x < 0 || y < 0)
                MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            else
            {
                cellHighlighted.IsHighlighted = false;
                cellHighlighted = cells[y, x];
                cellHighlighted.IsHighlighted = true;
            }

            e.Handled = true;
        }

        protected virtual void OnSelectedColorChanged(EventArgs args)
        {
            SelectedColorChanged?.Invoke(this, args);
        }
    }
}
