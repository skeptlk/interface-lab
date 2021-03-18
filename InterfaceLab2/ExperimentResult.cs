using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace InterfaceLab2
{
    public class ExperimentResult: INotifyPropertyChanged
    {
        public int No { get; set; }
        public int ElemCount { get; set; }
        
        private List<double> Attempts = new List<double>();
        public void AddAttempt(double time)
        {
            Attempts.Add(time);
            AttemptsCount = Attempts.Count;
            Average = Attempts.Average();
        }

        /**
         * DYNAMIC PROPERTIES
         */
        private int attemptsCount;
        public int AttemptsCount { 
            get { return attemptsCount; } 
            set { attemptsCount = value; OnPropertyChanged(); }
        }
        
        private double average;
        public double Average { 
            get { return average; } 
            set { average = value; OnPropertyChanged(); }
        }

        public string ToCSV()
        {
            string result = No + "; " + ElemCount + "; ";

            foreach (double a in Attempts)
            {
                result += a + "; ";
            }
            
            result += Average;
            return result;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
