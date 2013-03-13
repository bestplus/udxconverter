using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace UdxConverter.Win.Model
{
    public class OpenFilePageModel : INotifyPropertyChanged
    {
        private string _fileName;
        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
                OnPropertyChanged("FileName");
            }
        }

        public BindingList<PhoneNumber> Phones { get; set; }

        public OpenFilePageModel()
        {
            Phones = new BindingList<PhoneNumber>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
