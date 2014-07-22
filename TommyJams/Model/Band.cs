using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TommyJams.Model
{
    class Band
    {

        private string _bandname;
        public string BandName
        {
            get
            {
                return _bandname;
            }
            set
            {
                if (value != _bandname)
                {
                    _bandname = value;
                    NotifyPropertyChanged("BandName");
                }
            }
        }

        private string _banddescription;
        public string BandDescription
        {
            get
            {
                return _banddescription;
            }
            set
            {
                if (value != _banddescription)
                {
                    _banddescription = value;
                    NotifyPropertyChanged("BandDescription");
                }
            }
        }

        private string _bandpic;
        public string BandPic
        {
            get
            {
                return _bandpic;
            }
            set
            {
                if (value != _bandpic)
                {
                    _bandpic = value;
                    NotifyPropertyChanged("BandPic");
                }
            }
        }


        private string _bandfacebook;
        public string BandFacebook
        {
            get
            {
                return _bandfacebook;
            }
            set
            {
                if (value != _bandfacebook)
                {
                    _bandfacebook = value;
                    NotifyPropertyChanged("BandFacebook");
                }
            }
        }

        
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
