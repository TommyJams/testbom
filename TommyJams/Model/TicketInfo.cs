using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TommyJams.Model
{
    public class TicketInfo : INotifyPropertyChanged
    {
        private string _ticketLink;
        public string TicketLink
        {
            get
            {
                return _ticketLink;
            }
            set
            {
                if (value != _ticketLink)
                {
                    _ticketLink = value;
                    NotifyPropertyChanged("TicketLink");
                }
            }
        }

        private int _ticketAvailability;
        public int TicketAvailability
        {
            get
            {
                return _ticketAvailability;
            }
            set
            {
                if (value != _ticketAvailability)
                {
                    _ticketAvailability = value;
                    NotifyPropertyChanged("TicketAvailability");
                }
            }
        }

        private int _ticketPrice;
        public int TicketPrice
        {
            get
            {
                return _ticketPrice;
            }
            set
            {
                if (value != _ticketPrice)
                {
                    _ticketPrice = value;
                    NotifyPropertyChanged("TicketPrice");
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
