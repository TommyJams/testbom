using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TommyJams.ViewModel;

namespace TommyJams.ViewModel
{
    public class BandMember
    {
        private string _bandmembername;
        public string BandMemberName
        {
            get
            {
                return _bandmembername;
            }
            set
            {
                if (value != _bandmembername)
                {
                    _bandmembername = value;
                    NotifyPropertyChanged("BandMemberName");
                }
            }
        }

        private string _bandmemberrole;
        public string BandMemberRole
        {
            get
            {
                return _bandmemberrole;
            }
            set
            {
                if (value != _bandmemberrole)
                {
                    _bandmemberrole = value;
                    NotifyPropertyChanged("BandMemberRole");
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
