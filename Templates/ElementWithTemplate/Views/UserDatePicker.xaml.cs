using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace ElementWithTemplate.Views
{
    /// <summary>
    /// UserDatePicker.xaml 的交互逻辑
    /// </summary>
    public partial class UserDatePicker : UserControl
    {
        UniformGrid ungridMonth;
        DateTime datetimeSaved = DateTime.Now;
        DayOfWeek firstDayOfWeek = DayOfWeek.Monday;    //起始星期

        //注：此处的DateTime?可为null
        public DateTime? Date
        {
            set => SetValue(DateProperty, value);
            get => (DateTime?)GetValue(DateProperty);
        }
        
        public static readonly DependencyProperty DateProperty = DependencyProperty.Register("Date", typeof(DateTime?), typeof(UserDatePicker), new PropertyMetadata(new DateTime(), DateChangedCallBack));

        static void DateChangedCallBack(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            (obj as UserDatePicker).OnDateChanged((DateTime?)args.OldValue, (DateTime?)args.NewValue);
        }

        public static readonly RoutedEvent DateChangedEvent = EventManager.RegisterRoutedEvent("DateChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<DateTime?>), typeof(UserDatePicker));
        
        public event RoutedPropertyChangedEventHandler<DateTime?> DateChanged
        {
            add { AddHandler(DateChangedEvent, value); }
            remove { RemoveHandler(DateChangedEvent, value); }
        }


        public UserDatePicker()
        {
            InitializeComponent();

            Date = datetimeSaved;
            Loaded += DatePickerLoaded;                            
        }

        void DatePickerLoaded(object sender, RoutedEventArgs args)
        {
            ungridMonth = FindUniGrid(this.lstboxMonth);

            if (Date != null)
            {
                DateTime dt = Date.Value;
                ungridMonth.FirstColumn = (int)(new DateTime(dt.Year, dt.Month, 1).DayOfWeek);
                CorrectFirstDayOfWeek();
            }
        }

        void CorrectFirstDayOfWeek()
        {
            int firstColumn = ungridMonth.FirstColumn;

            firstColumn -= (int)firstDayOfWeek;

            while (firstColumn < 0)
                firstColumn += 7;

            ungridMonth.FirstColumn = firstColumn;
        }

        UniformGrid FindUniGrid(DependencyObject vis)
        {
            if (vis is UniformGrid)
                return vis as UniformGrid;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(vis); i++)
            {
                Visual visReturn = FindUniGrid(VisualTreeHelper.GetChild(vis, i));

                if (visReturn != null)
                    return visReturn as UniformGrid;
            }

            return null;
        }

        protected virtual void OnDateChanged(DateTime? dtOldValue, DateTime? dtNewValue)
        {
            this.chkboxNull.IsChecked = dtNewValue == null;

            if (dtNewValue != null)
            {
                DateTime dtNew = (DateTime)dtNewValue;

                this.txtblkMonthYear.Text = dtNew.ToString(DateTimeFormatInfo.CurrentInfo.YearMonthPattern);

                if (ungridMonth != null)
                {
                    ungridMonth.FirstColumn = (int)(new DateTime(dtNew.Year, dtNew.Month, 1).DayOfWeek);        //起始星期几        
                    CorrectFirstDayOfWeek();
                }

                int iDaysInMonth = DateTime.DaysInMonth(dtNew.Year, dtNew.Month);   //获取年、月中的天数

                if (iDaysInMonth != this.lstboxMonth.Items.Count)   //天数与当前显示的天数不相同，则对显示天进行重新排列
                {
                    this.lstboxMonth.BeginInit();
                    this.lstboxMonth.Items.Clear();

                    for (int i = 0; i < iDaysInMonth; i++)
                        this.lstboxMonth.Items.Add((i + 1).ToString());

                    this.lstboxMonth.EndInit();
                }
                
                this.lstboxMonth.SelectedIndex = dtNew.Day - 1;     //重新选中
            }

            //激发自定义的DateChangedEvent事件
            RoutedPropertyChangedEventArgs<DateTime?> args = new RoutedPropertyChangedEventArgs<DateTime?>(dtOldValue, dtNewValue, UserDatePicker.DateChangedEvent);
            args.Source = this;
            RaiseEvent(args);
        }

        void FlipPage(bool isBack)
        {
            if (Date == null)
                return;

            DateTime dt = Date.Value;
            int numPages = isBack ? -1 : 1;

            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                numPages *= 12;
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                numPages = Math.Max(-1200, Math.Min(1200, 120 * numPages));     //10年 120个月

            int year = dt.Year + numPages / 12;
            int month = dt.Month + numPages % 12;

            while (month < 1)   //此处用While使得每12个月进行一次年换算
            {
                month += 12;
                year -= 1;
            }
            while (month > 12)
            {
                month -= 12;
                year += 1;
            }

            if (year < DateTime.MinValue.Year)
                Date = DateTime.MinValue.Date;
            else if (year > DateTime.MaxValue.Year)
                Date = DateTime.MaxValue.Date;
            else
                Date = new DateTime(year, month, Math.Min(dt.Day, DateTime.DaysInMonth(year, month)));  //Math.Min(dt.Day, DateTime.DaysInMonth(year, month) 修正当前选择的日期天数。31 30 29 28
        }

        private void ButtonBackOnClick(object sender, RoutedEventArgs e)
        {
            FlipPage(true);
        }

        private void ButtonForwardOnClick(object sender, RoutedEventArgs e)
        {
            FlipPage(false);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (e.Key == Key.PageDown)
            {
                FlipPage(true);
                e.Handled = true;
            }
            else if (e.Key == Key.PageUp)
            {
                FlipPage(false);
                e.Handled = true;
            }
        }

        private void ListBoxOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Date == null)
                return;

            DateTime dt = Date.Value;

            if (lstboxMonth.SelectedIndex != -1)
                Date = new DateTime(dt.Year, dt.Month, Int32.Parse(this.lstboxMonth.SelectedItem as string));
        }

        private void CheckBoxNullOnChecked(object sender, RoutedEventArgs e)
        {
            if (Date != null)
            {
                datetimeSaved = Date.Value;
                Date = null;
            }
        }

        private void CheckBoxNullOnUnchecked(object sender, RoutedEventArgs e)
        {
            Date = datetimeSaved;
        }
    }
}
