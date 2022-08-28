using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WatchCat.Models
{
    public class Item : INotifyPropertyChanged
    {
        private double _average;
        private int _volume;

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Name { get; set; }
        public double Average 
        { 
            get { return _average; }
            set 
            { 
                if(value != _average)
                {
                    _average = value;
                    NotifyPropertyChanged();
                }
            } 
        }
        public int Ducats { get; set; }
        public bool Vaulted { get; set; }
        public int Volume 
        { 
            get { return _volume; } 
            set 
            { 
                if(value != _volume)
                {
                    _volume = value;
                    NotifyPropertyChanged();
                }
            } 
        }
        public string WFMUrl;
        public List<string> Relics = new List<string>();
    }
}
