using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace InterfaceLab2
{
    public class ExperimentResult: INotifyPropertyChanged
    {
        public string Title { get; set; }

        // Map (element_n => [attempts])
        private Dictionary<int, List<double>> Results = new Dictionary<int, List<double>>();

        public void AddResult(int elCount, double time)
        {
            if (!Results.ContainsKey(elCount))
                Results.Add(elCount, new List<double>());
            
            Results[elCount].Add(time);

            TotalAttempts++;
        }

        /**
         * DYNAMIC PROPERTIES
         */
        private int totalAttempts = 0;
        public int TotalAttempts
        {
            get { return totalAttempts; }
            set { totalAttempts = value; OnPropertyChanged(); }
        }

        public static string CSV_Header(int ElMin, int ElMax)
        {
            string header = "; ";
            for (int i = ElMin; i <= ElMax; i++)
                header += i + ";";
            return header;
        }

        public string ToCSV()
        {
            string result = Title + "; ";

            foreach (int elCount in Results.Keys.OrderBy(x => x))
            {
                result += Results[elCount].Average() + "; ";
            }
            return result;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
