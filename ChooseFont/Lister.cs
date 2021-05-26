using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Collections;
using System.Windows;
using System.Windows.Input;

namespace ChooseFont
{
    /* 一个类似ComBox的控件；list部分一直可见，输入焦点一直停留在TextBox部分；该类是list部分
     * Lister是一个具有Border的ContentControl，它包含一个ScrollViewer，次ScrollViewer具有一个StackPanel，里面是多个TextBlock项目（表示list部分）
     * 点击TextBlock会将SelectedIndex设定成被点击的TextBlock的索引，此SelectedIndex负责维护TextBlock项目的前景和背景颜色，以表示哪个项目目前被选取，
     * 并用OnSelectionChanged以触发SelectionChanged事件
     */ 

    class Lister : ContentControl
    {
        ScrollViewer scroll;
        StackPanel stack;
        ArrayList list = new ArrayList();
        int indexSelected = -1;

        public event EventHandler SelectionChanged;

        public int SelectedIndex
        {
            set
            {
                if (value < -1 || value >= Count)
                    throw new ArgumentOutOfRangeException("SelectedIndex");
                if (value == indexSelected)
                    return;

                if (indexSelected != -1)
                {
                    TextBlock txtblk = stack.Children[indexSelected] as TextBlock;
                    txtblk.Background = SystemColors.WindowBrush;
                    txtblk.Foreground = SystemColors.WindowTextBrush;
                }

                indexSelected = value;
                
                if (indexSelected > -1)
                {
                    TextBlock txtblk = stack.Children[indexSelected] as TextBlock;
                    txtblk.Background = SystemColors.HighlightBrush;
                    txtblk.Foreground = SystemColors.HighlightTextBrush;
                }
                ScrollIntoView();

                OnSelectionChanged(EventArgs.Empty);               
            }

            get => indexSelected;
        }

        public object SelectedItem
        {         
            set => SelectedIndex = list.IndexOf(value);

            get
            {
                if (SelectedIndex > -1)
                    return list[SelectedIndex];

                return null;
            }
        }

        public Lister()
        {
            this.Focusable = true;

            Border bord = new Border();
            bord.BorderThickness = new Thickness(1);
            bord.BorderBrush = SystemColors.ActiveBorderBrush;
            bord.Background = SystemColors.WindowBrush;
            this.Content = bord;

            scroll = new ScrollViewer();
            scroll.Focusable = false;
            scroll.Padding = new Thickness(2, 0, 0, 0);
            bord.Child = scroll;

            stack = new StackPanel();
            scroll.Content = stack;

            this.AddHandler(TextBlock.MouseLeftButtonDownEvent, new MouseButtonEventHandler(TextBlockOnMouseLeftButtonDown));

            this.Loaded += OnLoad;
        }

        void OnLoad(object sender, RoutedEventArgs args)
        {
            ScrollIntoView();
        }

        public void Add(object obj)
        {
            list.Add(obj);
            TextBlock txtblk = new TextBlock();
            txtblk.Text = obj.ToString();
            stack.Children.Add(txtblk);
        }

        public void Insert(int index, object obj)
        {
            list.Insert(index, obj);
            TextBlock txtblk = new TextBlock();
            txtblk.Text = obj.ToString();
            stack.Children.Insert(index, txtblk);
        }

        public void Clear()
        {
            SelectedIndex = -1;
            stack.Children.Clear();
            list.Clear();
        }

        public bool Contains(object obj)
        {
            return list.Contains(obj);
        }

        public int Count
        {
            get => list.Count;
        }

        public void GoToLetter(char ch)
        {
            int offset = SelectedIndex + 1;

            for (int i = 0; i < Count; i++)
            {
                int index = (i + offset) % Count;                

                if (Char.ToUpper(ch) == Char.ToUpper(list[index].ToString()[0]))
                {
                    SelectedIndex = index;
                    break;
                }
            }
        }

        public void PageUp()
        {
            if (SelectedIndex == -1 || Count == 0)
                return;

            int index = SelectedIndex - (int)(Count * scroll.ViewportHeight / scroll.ExtentHeight); //翻页
            if (index < 0)
                index = 0;

            SelectedIndex = index;
        }

        public void PageDown()
        {
            if (SelectedIndex == -1 || Count == 0)
                return;

            int index = SelectedIndex + (int)(Count * scroll.ViewportHeight / scroll.ExtentHeight);
            if (index > Count - 1)
                index = Count - 1;

            SelectedIndex = index;
        }

        void ScrollIntoView()
        {
            if (Count == 0 || SelectedIndex == -1 || scroll.ViewportHeight > scroll.ExtentHeight)
                return;

            double heightPerItem = scroll.ExtentHeight / Count;
            double offsetItemTop = SelectedIndex * heightPerItem;
            double offsetItemBot = (SelectedIndex + 1) * heightPerItem;

            if (offsetItemTop < scroll.VerticalOffset)
                scroll.ScrollToVerticalOffset(offsetItemTop);
            else if (offsetItemBot > scroll.VerticalOffset + scroll.ViewportHeight)
                scroll.ScrollToVerticalOffset(scroll.VerticalOffset + offsetItemBot - scroll.VerticalOffset - scroll.ViewportHeight);
        }

        void TextBlockOnMouseLeftButtonDown(object sender, MouseButtonEventArgs args)
        {
            if (args.Source is TextBlock)
                SelectedIndex = stack.Children.IndexOf(args.Source as TextBlock);
        }

        protected virtual void OnSelectionChanged(EventArgs args)
        {
            if (SelectionChanged != null)
                SelectionChanged(this, args);
        }
    }
}
