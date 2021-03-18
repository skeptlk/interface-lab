using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace InterfaceLab1
{
    public class Result
    {
        public int No { get; set; }
        public int Distance { get; set; }
        public int Size { get; set; }
        public double Time { get; set; }

        public string ToCSV()
        {
            return No + ", " + Distance + ", " + Size + ", " + Time;
        }
    }

    public interface IExperiment
    {
        bool IsFinished { get; set; }
        string Name { get; set; }
        void Next();
        void WriteResult(double time);
        Button UpdateButton();
        Point GetPosition(double center_x, double center_y);
        int GetRadius();
        bool ExportToFile(string fileName);
        ObservableCollection<Result> Results { get; }
    }
}

