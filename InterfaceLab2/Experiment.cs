using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace InterfaceLab2
{
    public class Experiment
    {
        const int StartElemCount = 2,
          EndElemCount = 8,
          ElemStep = 1,
          Attempts = 5;

        int AttemptN = 0;
        int ElemCount = 0;

        Random Random;

        public ObservableCollection<ExperimentResult> Results { get; }
        public bool IsFinished { get; set; }
        public Experiment()
        {
            ElemCount = StartElemCount;

            IsFinished = false;
            Random = new Random();
            Results = new ObservableCollection<ExperimentResult>();
        }

        public void Next()
        {
            AttemptN++;

            if (AttemptN > Attempts)
            {
                AttemptN = 0;
                ElemCount += ElemStep;
            }

            if (ElemCount == EndElemCount)
                IsFinished = true;
        }

        public void WriteResult(double time)
        {
            if (Results.Count > 0 && Results.Last().AttemptsMade < Attempts)
            {
                ExperimentResult last = Results.Last();
                last.Attempts.Add(time);
            }
            else
            {
                var res = new ExperimentResult()
                {
                    No = ElemCount - StartElemCount + 1,
                    ElemCount = ElemCount
                };
                res.Attempts.Add(time);
                Results.Add(res);
            }
        }

        public ObservableCollection<string> GetItems()
        {
            List<string> items = new List<string>(ElemCount);

            items.AddRange(
                Enumerable
                    .Range(1, ElemCount)
                    .Select(i => i.ToString())
                    .OrderBy(x => Random.NextDouble())
                );

            return new ObservableCollection<string>(items);
        }

        public int GetTargetNumber()
        {
            return Random.Next(1, ElemCount + 1);
        }

        public bool ExportToFile(string fileName)
        {
            try
            {
                using (StreamWriter output = new StreamWriter(fileName))
                {
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
