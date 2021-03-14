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
using System.Collections.ObjectModel;

namespace InterfaceLab2
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        bool IsExperimentStarted = false;
        bool MouseMoved = false;

        ObservableCollection<string> Items = new ObservableCollection<string>();

        string status;
        public string Status
        {
            get { return status; }
            set { status = value; NotifyPropertyChanged(); }
        }
        
        public MainWindow()
        {
            InitializeComponent();

            Status = "Press Enter to start...";

            Items.Add("1");
            Items.Add("2");
            Items.Add("3");
            Items.Add("4");

            Targets.ItemsSource = Items;

            SetupTimer();

        }


        // Handle Enter and Ctrl+S
        void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                //StartNextExperiment();
            }
            else if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
            {
                //SaveResults();
            }
        }

        void Window_MouseMove(object _, MouseEventArgs __)
        {
            if (IsExperimentStarted && !MouseMoved)
            {
                MouseMoved = true;
                StartMeasuringTime();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
