using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace InterfaceLab1.Fitts
{
    class FittsLawDistSizeRatio: IExperiment
    {
        int[] Distances = new int[] { 0, 20, 40, 60, 100, 150, 200, 250, 300, 350 };
        int[] Sizes = new int[] { 8, 10, 12, 15, 20, 30, 40, 50, 70, 100 };
        const int Attempts = 1;
        Button Button;
        Random Random;

        int SizeI = 0,
            DistI = 0,
            AttemptI = 0;

        public string Name { get; set; } = "Distance-size ratio";
        public ObservableCollection<Result> Results { get; }
        public bool IsFinished { get; set; }
        public FittsLawDistSizeRatio()
        {
            IsFinished = false;
            Random = new Random();
            Results = new ObservableCollection<Result>();

            Button = new Button
            {
                Height = Sizes[SizeI],
                Width = 2 * Sizes[SizeI],
                Content = "Click!",
                FontSize = Sizes[SizeI] / 2,
                Background = Brushes.Gray,
                Foreground = Brushes.White
            };
        }

        public void Next()
        {
            AttemptI++;

            if (AttemptI == Attempts)
            {
                AttemptI = 0;
                SizeI++;

                if (SizeI == Sizes.Length)
                {
                    SizeI = 0;
                    DistI++;
                }
            }

            if (DistI == Distances.Length)
                IsFinished = true;
        }

        public void WriteResult(double time)
        {
            Results.Add(new Result
            {
                Distance = Distances[DistI],
                No = DistI * Sizes.Length + SizeI + 1,
                Size = Sizes[SizeI],
                Time = time
            });
        }

        public Button UpdateButton()
        {
            int D = Sizes[SizeI];
            Button.Height = D;
            Button.Width = 2 * D;
            Button.FontSize = D / 2;
            return Button;
        }

        public Point GetPosition(double center_x, double center_y)
        {
            // randomize button coordinates
            const double from = 0;
            const double to = 2 * Math.PI;
            double rndAngle = (Random.NextDouble() * (to - from)) + from;

            double dx = Distances[DistI] * Math.Cos(rndAngle),
                   dy = Distances[DistI] * Math.Sin(rndAngle),
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
            return Distances[DistI];
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
