using System.Windows;
using System.Timers;
using System.Windows.Input;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Diagnostics;
using System;
using Microsoft.Win32;
using InterfaceLab1.Fitts;
using System.Collections.ObjectModel;

namespace InterfaceLab1
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        bool IsExperimentStarted = false;
        bool MouseMoved = false;
        int CountDown = 3;
        string status;
        public string Status
        {
            get { return status; }
            set { status = value; NotifyPropertyChanged(); }
        }

        ObservableCollection<IExperiment> Experiments;
        IExperiment Exp { get
            {
                return (IExperiment) ExpCombo.SelectedItem;
            } 
        }
        Button Target;
        static Timer Timer;
        static Stopwatch StopWatch;

        public MainWindow()
        {
            InitializeComponent();

            SetupTimer();

            Experiments = new ObservableCollection<IExperiment>();
            Experiments.Add(new FittsLawDistance());
            Experiments.Add(new FittsLawSize());
            Experiments.Add(new FittsLawDistSizeRatio());
            ExpCombo.ItemsSource = Experiments;
            ExpCombo.SelectedItem = Experiments[0];

            //Target = Exp.UpdateButton();
            //Target.Click += StopExperiment;
            //BtnCanvas.Children.Add(Target);

            Status = "Press Enter to start...";
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
        
        void StartMeasuringTime()
        {
            StopWatch.Start();
        }

        void StartNextExperiment()
        {
            if (IsExperimentStarted) return;

            Exp.Next();
            if (Exp.IsFinished)
            {
                Status = "Experiment series is complete!";
            }
            else
            {
                StartCountdown();
                SetMouse();
                PositionTarget();
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

                Exp.WriteResult(StopWatch.Elapsed.TotalMilliseconds);

                StopWatch.Reset();
            }
        }

        void SetupTimer ()
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
            CountDown = 2;
            Status = CountDown.ToString();
            Timer.Start();
        }

        void CountDownTick (object sender, ElapsedEventArgs e)
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

        void SaveResults()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == false)
            {
                return;
            }
            if (Exp.ExportToFile(saveFileDialog.FileName))
                MessageBox.Show("Exported to file!");
            else
                MessageBox.Show("Exporting results failed");
        }

        void PositionTarget()
        {
            double center_x = BtnCanvas.ActualWidth / 2,
                   center_y = BtnCanvas.ActualHeight / 2;

            Button btn = Exp.UpdateButton();
            Point pos = Exp.GetPosition(center_x, center_y);

            Canvas.SetTop(btn, pos.Y);
            Canvas.SetLeft(btn, pos.X);

            double radius = Exp.GetRadius();

            Canvas.SetLeft(Circle, center_x - radius);
            Canvas.SetTop(Circle, center_y - radius);
            
            Circle.Width = Circle.Height = 2 * radius;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        void SetMouse()
        {
            int center_x, center_y;
            TransformToPixels(
                (Left + Width / 2),
                (Top + Height / 2),
                out center_x,
                out center_y
            );

            SetCursorPos(center_x, center_y + 15);
        }

        [DllImport("User32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        [DllImport("Gdi32.dll")]
        public static extern int GetDeviceCaps(IntPtr hDc, int nIndex);

        [DllImport("User32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("User32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDc);

        public const int LOGPIXELSX = 88;
        public const int LOGPIXELSY = 90;

        public void TransformToPixels(double unitX,
                                      double unitY,
                                      out int pixelX,
                                      out int pixelY)
        {
            IntPtr hDc = GetDC(IntPtr.Zero);
            if (hDc != IntPtr.Zero)
            {
                int dpiX = GetDeviceCaps(hDc, LOGPIXELSX);
                int dpiY = GetDeviceCaps(hDc, LOGPIXELSY);

                ReleaseDC(IntPtr.Zero, hDc);

                pixelX = (int)((double) dpiX / 96 * unitX);
                pixelY = (int)((double) dpiY / 96 * unitY);
            }
            else
                throw new ArgumentNullException("Failed to get DC.");
        }

        private void ExpCombo_Selected(object sender, RoutedEventArgs e)
        {
            BtnCanvas.Children.Remove(Target);
            Target = Exp.UpdateButton();
            Target.Click += StopExperiment;
            BtnCanvas.Children.Add(Target);

            ResultsTable.DataContext = Exp.Results;
        }
    }

}
