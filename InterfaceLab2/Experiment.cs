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

        const int FontSizeIDefault = 2,
                  ColorIDefault = 0;

        // Experiment has two parts: 
        // 1. Traversing Colors
        // 2. Traversing FontSizes
        int Part;


        ExperimentResult CurrentResult;

        Random Random;

        public ObservableCollection<ExperimentResult> Results { get; }
        public bool IsFinished { get; set; }
        public Experiment()
        {
            Random = new Random();
         
            CurrElemCount = StartElemCount;
            AttemptI = -1;
            ColorI = 0;
            FontSizeI = 0;

            FontSizeI = FontSizeIDefault;

            Part = 1;

            Results = new ObservableCollection<ExperimentResult>();

            StartNewSeries();

            IsFinished = false;
        }

        public void Next()
        {
            AttemptI++;

            if (AttemptI == Attempts)
            {
                AttemptI = 0;
                CurrElemCount += ElemStep;
            }

            if (CurrElemCount > EndElemCount)
            {
                CurrElemCount = StartElemCount;

                // First part: Font colors
                if (Part == 1)
                {
                    ColorI++;

                    if (ColorI == Colors.Length)
                    {
                        ColorI = ColorIDefault;
                        Part = 2;
                        FontSizeI = 0;
                    }

                    StartNewSeries();
                } else
                // Second part: Font sizes
                {
                    FontSizeI++;

                    if (FontSizeI == FontSizes.Length)
                    {
                        IsFinished = true;
                        return;
                    }

                    StartNewSeries();
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
                    "Color: " + Colors[ColorI].ToString() :
                    "Font size: " + FontSizes[FontSizeI].ToString()
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
