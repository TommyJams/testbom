using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TommyJams.Model
{
    public class VenueInfo:INotifyPropertyChanged
    {
        private string _venueName;
        public string VenueName
        {
            get
            {
                return _venueName;
            }
            set
            {
                if (value != _venueName)
                {
                    _venueName = value;
                    NotifyPropertyChanged("VenueName");
                }
            }
        }

        private string _venueAddress;
        public string VenueAddress
        {
            get
            {
                return _venueAddress;
            }
            set
            {
                if (value != _venueAddress)
                {
                    _venueAddress = value;
                    NotifyPropertyChanged("VenueAddress");
                }
            }
        }

        private string _venueCity;
        public string VenueCity
        {
            get
            {
                return _venueCity;
            }
            set
            {
                if (value != _venueCity)
                {
                    _venueCity = value;
                    NotifyPropertyChanged("VenueCity");
                }
            }
        }

        private string _venueCoordinates;
        public string VenueCoordinates
        {
            get
            {
                return _venueCoordinates;
            }
            set
            {
                if (value != _venueCoordinates)
                {
                    _venueCoordinates = value;
                    NotifyPropertyChanged("VenueCoordinates");
                }
            }
        }

        private double _venueZRating;
        public double VenueZRating
        {
            get
            {
                return _venueZRating;
            }
            set
            {
                if (value != _venueZRating)
                {
                    _venueZRating = value;
                    NotifyPropertyChanged("VenueZRating");
                }
            }
        }

        private string _venueZType;
        public string VenueZType
        {
            get
            {
                return _venueZType;
            }
            set
            {
                if (value != _venueZType)
                {
                    _venueZType = value;
                    NotifyPropertyChanged("VenueZType");
                }
            }
        }

        private int _venueZPrice;
        public int VenueZPrice
        {
            get
            {
                return _venueZPrice;
            }
            set
            {
                if (value != _venueZPrice)
                {
                    _venueZPrice = value;
                    NotifyPropertyChanged("VenueZPrice");
                }
            }
        }

        private string _venueFacebook;
        public string VenueFacebook
        {
            get
            {
                return _venueFacebook;
            }
            set
            {
                if (value != _venueFacebook)
                {
                    _venueFacebook = value;
                    NotifyPropertyChanged("VenueFacebook");
                }
            }
        }

        private string _venueZomato;
        public string VenueZomato
        {
            get
            {
                return _venueZomato;
            }
            set
            {
                if (value != _venueZomato)
                {
                    _venueZomato = value;
                    NotifyPropertyChanged("VenueZomato");
                }
            }
        }

        private GeoCoordinate _venueGeoCoordinate;
        public GeoCoordinate VenueGeoCoordinate
        {
            get
            {
                return _venueGeoCoordinate;
            }
            set
            {
                if (value != _venueGeoCoordinate)
                {
                    _venueGeoCoordinate = value;
                    NotifyPropertyChanged("VenueGeoCoordinate");
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
