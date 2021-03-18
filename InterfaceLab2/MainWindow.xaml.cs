using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace InterfaceLab2
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        bool IsExperimentStarted = false;
        bool MouseMoved = false;

        Experiment Experiment = new Experiment();

        ObservableCollection<string> Items = new ObservableCollection<string>();

        string status;
        public string Status
        {
            get { return status; }
            set { status = value; NotifyPropertyChanged(); }
        }

        // Shows number user must click on
        int targetNumber;
        public int TargetNumber
        {
            get { return targetNumber; }
            set { targetNumber = value; NotifyPropertyChanged(); }
        }

        public MainWindow()
        {
            InitializeComponent();

            Status = "Press Enter to start...";

            ResultsTable.ItemsSource = Experiment.Results;

            SetupTimer();
        }


        // Handle Enter and Ctrl+S
        void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                StartNextExperiment();
            }
            else if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
            {
                SaveResults();
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

        void StartNextExperiment()
        {
            if (IsExperimentStarted) return;

            Experiment.Next();

            if (Experiment.IsFinished)
            {
                Status = "Experiment series is complete!";
            }
            else
            {
                Items = Experiment.GetItems();
                Targets.ItemsSource = Items;
                TargetNumber = Experiment.GetTargetNumber();
                IsExperimentStarted = true;
                Status = "Go!";
                SetMouse();
            }
        }

        void StopExperiment(object _, RoutedEventArgs __)
        {
            if (IsExperimentStarted && MouseMoved)
            {
                StopWatch.Stop();
                IsExperimentStarted = false;
                MouseMoved = false;
                Status = "Press Enter to start...";

                Experiment.WriteResult(StopWatch.Elapsed.TotalMilliseconds);
                MessageBox.Show(StopWatch.Elapsed.TotalMilliseconds.ToString());
                StopWatch.Reset();
            }
        }


        void SaveResults()
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            if (saveDialog.ShowDialog() == false)
            {
                return;
            }
            if (Experiment.ExportToFile(saveDialog.FileName))
                MessageBox.Show("Exported to file!");
            else
                MessageBox.Show("Failed to export results");
        }


        public event PropertyChangedEventHandler PropertyChanged;
        void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
