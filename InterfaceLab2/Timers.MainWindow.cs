using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;


namespace InterfaceLab2
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        int CountDown = 3;
        Timer Timer;
        Stopwatch StopWatch;

        void SetupTimer()
        {
            Timer = new Timer
            {
                Interval = 1000.0,
                AutoReset = true
            };
            Timer.Elapsed += CountDownTick;
            StopWatch = new Stopwatch();
        }

        void StartCountdown()
        {
            CountDown = 3;
            Status = CountDown.ToString();
            Timer.Start();
        }

        void StartMeasuringTime()
        {
            StopWatch.Start();
        }

        void CountDownTick(object sender, ElapsedEventArgs e)
        {
            CountDown -= 1;

            if (CountDown > 0)
            {
                Status = CountDown.ToString();
            }
            else
            {
                Timer.Stop();
                IsExperimentStarted = true;
                Status = "Go!";
                CountDown = 3;
            }
        }

    }
}
