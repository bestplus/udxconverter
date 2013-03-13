using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace UdxConverter.Win.Model
{
    public class PhoneNumber : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Number { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String name)
        {
            var @event = PropertyChanged;
            if (@event != null)
            {
                @event(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
