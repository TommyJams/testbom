using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TommyJams.ViewModel
{
    class TicketInfo
    {
        private string _ticketLink;
        public string ticketLink
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
                }
            }
        }

    }
}
