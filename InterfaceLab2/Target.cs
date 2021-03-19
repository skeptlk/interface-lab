using System.ComponentModel;
using System.Windows.Media;
using System.Runtime.CompilerServices;

namespace InterfaceLab2
{
    public class Target: INotifyPropertyChanged
    {
        private int fontSize;
        public int FontSize
        {
            get { return fontSize; }
            set { fontSize = value; OnPropertyChanged(); }
        }

        private string text;
        public string Text
        {
            get { return text; }
            set { text = value; OnPropertyChanged(); }
        }

        private Brush foreground;
        public Brush Foreground
        {
            get { return foreground; }
            set { foreground = value; OnPropertyChanged(); }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
