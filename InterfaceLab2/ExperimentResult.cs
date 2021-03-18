using System.Collections.Generic;
using System.Linq;

namespace InterfaceLab2
{
    public class ExperimentResult
    {
        public int No { get; set; }
        public int ElemCount { get; set; }
        public List<double> Attempts = new List<double>();
        public int AttemptsMade { get { return Attempts.Count; } }
        public double Average { get { return Attempts.Average(); } }

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
    }
}
