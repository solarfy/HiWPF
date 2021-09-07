using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace DataEntryDemo.View
{
    /// <summary>
    /// NavigationBar.xaml 的交互逻辑
    /// </summary>
    public partial class NavigationBar : ToolBar
    {
        /* System.Windows.Data.CollectionView - 流通管理员（使用索引变量引用当前的记录）
         * ListCollectionView lstcollview = new ListCollectionView(coll)    coll必须实现IList接口
         * 静态CollectionViewSource.GetDefaultView的方法返回一个CollectionView或一个ListCollectionView对象
         * CollectionView对象无法对集合进行排序或者分组；ListCollectionView对象则可以
         */

        IList coll;
        ICollectionView collview;
        Type typeItem;

        public NavigationBar()
        {
            InitializeComponent();
        }        

        public IList Collection
        {
            set
            {
                coll = value;

                //创建CollectionView并设置相关事件处理
                collview = CollectionViewSource.GetDefaultView(coll);
                collview.CurrentChanged += CollectionViewOnCurrentChanged;
                collview.CollectionChanged += CollectionViewOnCollectionChanged;

                //手动触发事件，用于初始化TextBox和Buttons
                CollectionViewOnCurrentChanged(null, null);

                this.txtblkTotal.Text = coll.Count.ToString();
            }

            get
            {
                return coll;
            }
        }

        public Type ItemType
        {
            set => typeItem = value;
            get => typeItem;
        }

        void CollectionViewOnCurrentChanged(object sender, EventArgs args)
        {
            this.txtboxCurrent.Text = (1 + collview.CurrentPosition).ToString();
            this.btnPrev.IsEnabled = collview.CurrentPosition > 0;
            this.btnNext.IsEnabled = collview.CurrentPosition < coll.Count - 1;
            this.btnDel.IsEnabled = coll.Count > 1;
        }

        void CollectionViewOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            this.txtblkTotal.Text = coll.Count.ToString();
        }

        private void FirstOnClick(object sender, RoutedEventArgs e)
        {
            collview.MoveCurrentToFirst();
        }

        private void PreviousOnClick(object sender, RoutedEventArgs e)
        {
            collview.MoveCurrentToPrevious();
        }

        private void NextOnClick(object sender, RoutedEventArgs e)
        {
            collview.MoveCurrentToNext();
        }

        private void LastOnClick(object sender, RoutedEventArgs e)
        {
            collview.MoveCurrentToLast();
        }

        private void AddOnClick(object sender, RoutedEventArgs e)
        {
            ConstructorInfo info = typeItem.GetConstructor(System.Type.EmptyTypes);     //通过反射获取该类的默认构造函数
            coll.Add(info.Invoke(null));    //调用构造函数，创建一个新对象
            collview.MoveCurrentToLast();
        }
                
        private void DeleteOnClick(object sender, RoutedEventArgs e)
        {
            coll.RemoveAt(collview.CurrentPosition);
        }

        string strOriginal; //txtboxCurrent 源字符串
        private void TextBoxOnGotFoucus(object sender, KeyboardFocusChangedEventArgs e)
        {
            strOriginal = this.txtboxCurrent.Text;
        }

        private void TextBoxOnLostFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            int current;

            if (Int32.TryParse(this.txtboxCurrent.Text, out current))
            {
                if (current > 0 && current <= coll.Count)
                    collview.MoveCurrentToPosition(current - 1);
            }
            else
            {
                this.txtboxCurrent.Text = strOriginal;
            }
        }

        private void TextBoxOnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.txtboxCurrent.Text = strOriginal;
                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                e.Handled = true;
            }
            else
                return;

            MoveFocus(new TraversalRequest(FocusNavigationDirection.Right));    //将焦点移动到另一个控件
        }
    }
}
