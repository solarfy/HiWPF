using System;
using System.Windows;
using System.Windows.Threading;

namespace DigitalClock
{
    //继承自DependencyObject，为了使用SetValue和GetValue
    class ClockTicker1 : DependencyObject
    {
        public static DependencyProperty DateTimeProperty = DependencyProperty.Register("DateTime", typeof(DateTime), typeof(ClockTicker1));

        public DateTime DateTime
        {
            set => SetValue(DateTimeProperty, value);
            get => (DateTime)GetValue(DateTimeProperty);
        }

        public ClockTicker1()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += TimerOnTick;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();
        }

        void TimerOnTick(object sender, EventArgs args)
        {
            DateTime = DateTime.Now;
        }


    }
}
