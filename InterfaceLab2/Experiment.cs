using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Media;


namespace InterfaceLab2
{
    public class Experiment
    {
        const int StartElemCount = 2,
          EndElemCount = 8,
          ElemStep = 1,
          Attempts = 5;

        readonly int[] FontSizes = { 10, 11, 12, 14, 16 };
        readonly Brush[] Colors = { 
            Brushes.White, 
            Brushes.Black, 
            Brushes.Yellow, 
            Brushes.Green, 
            Brushes.Blue, 
            Brushes.Crimson 
        };

        int AttemptI,
            ColorI,
            FontSizeI,
            CurrElemCount;

        // Experiment has two parts: 
        // 1. Traversing FontSizes
        // 2. Traversing Colors
        int Part;


        ExperimentResult CurrentResult;

        Random Random;

        public ObservableCollection<ExperimentResult> Results { get; }
        public bool IsFinished { get; set; }
        public Experiment()
        {
            CurrElemCount = StartElemCount;
            AttemptI = -1;
            ColorI = 0;
            FontSizeI = 0;
            
            Part = 1;

            StartNewSeries();

            IsFinished = false;
            Random = new Random();
            Results = new ObservableCollection<ExperimentResult>();
        }

        public void Next()
        {
            AttemptI++;

            if (AttemptI == Attempts)
            {
                AttemptI = 0;
                CurrElemCount += ElemStep;
            }

            if (CurrElemCount == EndElemCount)
            {
                CurrElemCount = StartElemCount;

                // First part: Font colors
                if (Part == 1)
                {
                    ColorI++;
                    StartNewSeries();

                    if (ColorI == Colors.Length)
                        Part = 2;
                } else
                // Second part: Font sizes
                {
                    FontSizeI++;
                    StartNewSeries();

                    if (FontSizeI == FontSizes.Length)
                        IsFinished = true;
                }
            }
        }

        public void WriteResult(double time)
        {
            CurrentResult.AddResult(CurrElemCount, time);
        }

        private void StartNewSeries()
        {
            CurrentResult = new ExperimentResult()
            {
                Title = (Part == 1) ?
                    "Font size: " + FontSizes[FontSizeI].ToString() :
                    "Color: " + Colors[ColorI].ToString()
            };
            Results.Add(CurrentResult);
        }

        public ObservableCollection<Target> GetItems()
        {
            List<Target> result = new List<Target>();

            for (int i = 1; i <= CurrElemCount; i++)
            {
                result.Add(new Target
                {
                    Foreground = Colors[ColorI],
                    FontSize = FontSizes[FontSizeI],
                    Text = i.ToString()
                });
            }

            return new ObservableCollection<Target>(
                result.OrderBy(x => Random.NextDouble())
            );
        }

        public int GetTargetNumber()
        {
            return Random.Next(1, CurrElemCount + 1);
        }

        public bool ExportToFile(string fileName)
        {
            try
            {
                using (StreamWriter output = new StreamWriter(fileName))
                {
                    output.WriteLine(ExperimentResult.CSV_Header(StartElemCount, EndElemCount));

                    foreach (ExperimentResult res in Results)
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
