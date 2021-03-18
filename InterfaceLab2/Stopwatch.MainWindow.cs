using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows;


namespace InterfaceLab2
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        Stopwatch StopWatch;

        void SetupTimer()
        {
            StopWatch = new Stopwatch();
        }

        void StartMeasuringTime()
        {
            StopWatch.Start();
        }

        double StopMeasuringTime()
        {
            StopWatch.Stop();
            double time = StopWatch.Elapsed.TotalMilliseconds;
            StopWatch.Reset();
            return time;
        }

    }
}
