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
          Attempts = 3;

        readonly int[] FontSizes = { 10, 11, 12, 14, 16 };
        readonly Brush[] Colors = {
            Brushes.White,
            Brushes.Yellow,
            Brushes.Green, 
            Brushes.Blue,
            Brushes.Crimson
        };
        readonly string[] ColorNames = {
            "Белый",
            "Жёлтый",
            "Зелёный",
            "Синий",
            "Красный",
        };

        int AttemptI,
            FontSizeI,
            ColorI,
            CurrElemCount;

        static int BaseFont = 15;
        static Brush BaseColor = Brushes.Black;

        int CurrFontSize {
            get { return (FontSizeI < 0 || FontSizeI >= FontSizes.Length)
                    ? BaseFont 
                    : FontSizes[FontSizeI]; 
            }
        }
        Brush CurrColor {
            get { return (ColorI < 0 || ColorI >= Colors.Length) 
                    ? BaseColor 
                    : Colors[ColorI]; 
            }
        }

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
            FontSizeI = -1;

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
                    FontSizeI = -1;

                    if (ColorI == Colors.Length)
                    {
                        Part = 2;
                        FontSizeI = 0;
                    }

                    StartNewSeries();
                } else
                // Second part: Font sizes
                {
                    FontSizeI++;
                    ColorI = -1;

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
                    "Color: " + ColorNames[ColorI] :
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
                    Foreground = BaseColor,
                    FontSize = BaseFont,
                    Text = i.ToString()
                });
            }

            int target = GetTargetNumber();
            result[target] = new Target
            {
                Foreground = CurrColor,
                FontSize = CurrFontSize,
                Text = (target + 1).ToString(),
            };

            return new ObservableCollection<Target>(
                result.OrderBy(x => Random.NextDouble())
            );
        }

        private int GetTargetNumber()
        {
            return Random.Next(0, CurrElemCount);
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
