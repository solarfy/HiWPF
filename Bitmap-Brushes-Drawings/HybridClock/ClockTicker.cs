using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace HybridClock
{
    class ClockTicker : INotifyPropertyChanged
    {
        string strFormat = "F";
        public event PropertyChangedEventHandler PropertyChanged;

        public string DateTime
        {
            get { return System.DateTime.Now.ToString(strFormat); }
        }

        public string Format
        {
            set { strFormat = value; }
            get { return strFormat; }
        }

        public ClockTicker()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += TimerOnTick;
            timer.Interval = TimeSpan.FromSeconds(0.10);
            timer.Start();
        }

        void TimerOnTick(object sender, EventArgs args)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateTime)));
        }
    }
}
