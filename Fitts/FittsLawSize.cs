using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace InterfaceLab1.Fitts
{
    class FittsLawSize: IExperiment
    {
        int[] Sizes = new int[] { 8, 10, 12, 15, 20, 30, 40, 50, 70, 100 };
        const int Distance = 200;
        const int AttemptsN = 5;
        int Size = 0,
            ExperimentNo = 0,
            AttemptNo = -1;
        Button Button;
        Random Random;

        public string Name { get; set; } = "Size";
        public ObservableCollection<Result> Results { get; }
        public bool IsFinished { get; set; }
        public FittsLawSize()
        {
            IsFinished = false;
            Random = new Random();
            Results = new ObservableCollection<Result>();
            Size = Sizes[0];

            Button = new Button
            {
                Height = Size,
                Width = 2 * Size,
                Content = "Click!",
                FontSize = Size / 2,
                Background = Brushes.Gray,
                Foreground = Brushes.White
            };
        }

        public void Next()
        {
            AttemptNo++;
            if (AttemptNo >= AttemptsN)
            {
                AttemptNo = 0;
                ExperimentNo++;
            }

            if (ExperimentNo == Sizes.Length)
                IsFinished = true;
        }

        public void WriteResult(double time)
        {
            Results.Add(new Result
            {
                Distance = Distance,
                No = ExperimentNo + 1,
                Size = Size,
                Time = time
            });
        }

        public Button UpdateButton()
        {
            Size = Sizes[ExperimentNo];
            Button.Height = Size;
            Button.Width = 2 * Size;
            Button.FontSize = Size / 2;
            return Button;
        }

        public Point GetPosition(double center_x, double center_y)
        {
            // randomize button coordinates
            const double from = 0;
            const double to = 2 * Math.PI;
            double rndAngle = (Random.NextDouble() * (to - from)) + from;

            double dx = Distance * Math.Cos(rndAngle),
                   dy = Distance * Math.Sin(rndAngle),
                   left, top;

            if (dy > 0)
                top = center_y + dy;
            else
                top = center_y + dy - Button.Height;

            if (dx > 0)
                left = center_x + dx;
            else
                left = center_x + dx - Button.Width;

            return new Point(left, top);
        }

        public int GetRadius()
        {
            return Distance;
        }

        public bool ExportToFile(string fileName)
        {
            try
            {
                using (StreamWriter output = new StreamWriter(fileName))
                {
                    int prev = -1;
                    foreach (Result res in Results)
                    {
                        if (prev != res.No)
                        {
                            output.Write("\r\n" + res.No + ", " + res.Distance + ", ");
                        }
                        output.Write(res.Time + ", ");
                        prev = res.No;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

    }
}
