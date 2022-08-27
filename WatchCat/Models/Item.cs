using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WatchCat.Models
{
    public class Item : INotifyPropertyChanged
    {
        private double _avarage;
        private int _volume;

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Name { get; set; }
        public double Avarage 
        { 
            get { return _avarage; }
            set 
            { 
                if(value != _avarage)
                {
                    _avarage = value;
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
    }
}
