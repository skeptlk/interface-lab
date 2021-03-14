using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace InterfaceLab1
{
    public class FittsLawDistance : IExperiment
    {
        int[] Distances = new int[] { 0, 20, 40, 60, 100, 150, 200, 250, 300, 350 };
        const int
            AttemptsN = 1,
            Size = 30;
        int
            ExperimentNo = 0,
            AttemptNo = 0;
        Button Button;
        Random Random;

        public string Name { get; set; } = "Distance";
        public ObservableCollection<Result> Results { get; }
        public bool IsFinished { get; set; }
        public FittsLawDistance()
        {
            IsFinished = false;
            Random = new Random();
            Results = new ObservableCollection<Result>();

            Button = new Button
            {
                Height = Size,
                Width = 2 * Size,
                Content = "Click here!"
            };
        }

        public void Next()
        {
            AttemptNo++;
            if (AttemptNo > AttemptsN)
            {
                AttemptNo = 0;
                ExperimentNo++;
            }

            if (ExperimentNo == Distances.Length)
                IsFinished = true;
        }

        public void WriteResult(double time)
        {
            Results.Add(new Result
            {
                Distance = Distances[ExperimentNo],
                No = ExperimentNo + 1,
                Size = Size,
                Time = time
            });
        }

        public Button UpdateButton()
        {
            return Button;
        }

        public Point GetPosition(double center_x, double center_y)
        {
            // randomize button coordinates
            const double from = 0;
            const double to = 2 * Math.PI;
            double rndAngle = (Random.NextDouble() * (to - from)) + from;

            double dist = Distances[ExperimentNo],
                   dx = dist * Math.Cos(rndAngle),
                   dy = dist * Math.Sin(rndAngle),
                   btn_w = Button.Width,
                   btn_h = Button.Height,
                   left, top;

            if (dy > 0)
                top = center_y + dy;
            else
                top = center_y + dy - btn_h;

            if (dx > 0)
                left = center_x + dx;
            else
                left = center_x + dx - btn_w;

            return new Point(left, top);
        }

        public int GetRadius()
        {
            return Distances[ExperimentNo];
        }

        public bool ExportToFile(string fileName)
        {
            try
            {
                using (StreamWriter output = new StreamWriter(fileName))
                {
                    foreach (Result res in Results)
                    {
                        output.WriteLine(res.ToCSV());
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
