using System;
using System.ComponentModel;    //for INotifyPropertyChanged
using System.Windows.Threading;

namespace DigitalClock
{
    //通过INotifyPropertyChanged接口，实现绑定
    class ClockTicker2 : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;   //必须的

        public DateTime DateTime
        {
            get => DateTime.Now;
        }

        public ClockTicker2()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += TimerOnTick;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();
        }

        void TimerOnTick(object sender, EventArgs args)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(DateTime)));
        }
    }
}
