using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ChooseFont
{
    /* 一个类似ComBox的控件；list部分一直可见，输入焦点一直停留在TextBox部分；该类是TextBox部分，其组合了list部分
     * 
     */
    
    class TextBoxWithLister : ContentControl
    {
        TextBox txtbox;
        Lister lister;
        bool isReadOnly;

        public event EventHandler SelectionChanged;
        public event TextChangedEventHandler TextChanged;

        public string Text
        {
            get => txtbox.Text;
            set => txtbox.Text = value;
        }

        public bool IsReadOnly
        {
            get => isReadOnly;
            set => isReadOnly = value;
        }

        public object SelectedItem
        {
            set 
            {
                lister.SelectedItem = value;
                if (lister.SelectedItem != null)
                    txtbox.Text = lister.SelectedItem.ToString();
                else
                    txtbox.Text = "";
            }

            get => lister.SelectedItem;
        }

        public int SelectedIndex
        {
            set
            {
                lister.SelectedIndex = value;
                if (lister.SelectedIndex == -1)
                    txtbox.Text = "";
                else
                    txtbox.Text = lister.SelectedItem.ToString();
            }

            get => lister.SelectedIndex;
        }

        public TextBoxWithLister()
        {
            DockPanel dock = new DockPanel();
            this.Content = dock;

            txtbox = new TextBox();
            txtbox.TextChanged += TextBoxOnTextChanged;
            dock.Children.Add(txtbox);
            DockPanel.SetDock(txtbox, Dock.Top);

            lister = new Lister();
            lister.SelectionChanged += ListerOnSelectionChanged;            
            dock.Children.Add(lister);
        }

        public void Add(object obj)
        {
            lister.Add(obj);
        }

        public void Insert(int index, object obj)
        {
            lister.Insert(index, obj);
        }

        public void Clear()
        {
            lister.Clear();
        }

        public bool Contains(object obj)
        {
            return lister.Contains(obj);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            Focus();
        }

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);

            if (e.NewFocus == this)
            {
                txtbox.Focus();
                if (SelectedIndex == -1 && lister.Count > 0)
                    SelectedIndex = 0;
            }
        }
        
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);

            if (IsReadOnly)
            {
                //输入字母的首字符，直接跳转到相关的项中
                lister.GoToLetter(e.Text[0]);
                e.Handled = true;
            }
        }
        
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (SelectedIndex == -1)
                return;

            //根据方向键来改变选取的项目
            switch (e.Key)
            {
                case Key.Home:
                    if (lister.Count > 0)
                        SelectedIndex = 0;
                    break;

                case Key.End:
                    if (lister.Count > 0)
                        SelectedIndex = lister.Count - 1;
                    break;

                case Key.Up:
                    if (SelectedIndex > 0)
                        SelectedIndex--;
                    break;

                case Key.Down:
                    if (SelectedIndex < lister.Count - 1)
                        SelectedIndex++;
                    break;

                case Key.PageUp:
                    lister.PageUp();
                    break;

                case Key.PageDown:
                    lister.PageDown();
                    break;

                default:
                    break;
            }

            //e.Handled = true;
        }

        void ListerOnSelectionChanged(object sender, EventArgs args)
        {
            if (SelectedIndex == -1)
                txtbox.Text = "";
            else
                txtbox.Text = lister.SelectedItem.ToString();

            OnSelectionChanged(args);
        }

        void TextBoxOnTextChanged(object sender, TextChangedEventArgs args)
        {
            if (TextChanged != null)
                TextChanged(this, args);
        }

        protected virtual void OnSelectionChanged(EventArgs args)
        {
            if (SelectionChanged != null)
                SelectionChanged(this, args);
        }
    }
}
