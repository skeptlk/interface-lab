using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace InterfaceLab2
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        bool IsExperimentStarted = false;

        Experiment Experiment = new Experiment();

        ObservableCollection<string> Items = new ObservableCollection<string>();

        string status;
        public string Status
        {
            get { return status; }
            set { status = value; NotifyPropertyChanged(); }
        }

        // Shows number that user must click on
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
                Status = "Go!";
                Items = Experiment.GetItems();
                Targets.ItemsSource = Items;
                TargetNumber = Experiment.GetTargetNumber();
                SetMouse();

                IsExperimentStarted = true;
                StartMeasuringTime();
            }
        }

        void StopExperiment(object _, RoutedEventArgs __)
        {
            if (IsExperimentStarted)
            {
                IsExperimentStarted = false;
                Status = "Press Enter to start...";

                double time = StopMeasuringTime();
                Experiment.WriteResult(time);
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
