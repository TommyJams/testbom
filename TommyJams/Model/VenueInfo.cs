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
        private VenueInfo_Foursquare _VenueInfo_Foursquare;
        public VenueInfo_Foursquare venueInfo_Foursquare
        {
            get
            {
                return _VenueInfo_Foursquare;
            }
            set
            {
                if (value != _VenueInfo_Foursquare)
                {
                    _VenueInfo_Foursquare = value;
                    NotifyPropertyChanged("venueInfo_Foursquare");
                }
            }
        }

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

        private string _venueID;
        public string VenueID
        {
            get
            {
                return _venueID;
            }
            set
            {
                if (value != _venueID)
                {
                    _venueID = value;
                    GetFourSquareInfo(value);                    
                    NotifyPropertyChanged("VenueID");
                }
            }
        }

        private async void GetFourSquareInfo(string value)
        {
            VenueInfo_Foursquare vif = new VenueInfo_Foursquare();
            vif.FoursquareInfo_IsLoading = true;

            venueInfo_Foursquare = vif;

            try
            {
                vif = await App.ViewModel.AppModel.GetVenueInfo_Foursquare(value);
            }
            catch{}

            vif.FoursquareInfo_IsLoading = false;
            venueInfo_Foursquare = vif;
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

    public class VenueInfo_Foursquare : INotifyPropertyChanged
    {
        private bool _FoursquareInfo_Visibility = false;
        public bool FoursquareInfo_Visibility
        {
            get
            {
                return _FoursquareInfo_Visibility;
            }
            set
            {
                if (value != _FoursquareInfo_Visibility)
                {
                    _FoursquareInfo_Visibility = value;
                    NotifyPropertyChanged("FoursquareInfo_Visibility");
                }
            }
        }

        private bool _FoursquareInfo_IsLoading;
        public bool FoursquareInfo_IsLoading
        {
            get
            {
                return _FoursquareInfo_IsLoading;
            }
            set
            {
                if (value != _FoursquareInfo_IsLoading)
                {
                    _FoursquareInfo_IsLoading = value;
                    NotifyPropertyChanged("FoursquareInfo_IsLoading");
                }
            }
        }

        private long _venueZCheckin;
        public long VenueZCheckin
        {
            get
            {
                return _venueZCheckin;
            }
            set
            {
                if (value != _venueZCheckin)
                {
                    _venueZCheckin = value;
                    NotifyPropertyChanged("VenueZCheckin");
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

        private string _venueZPrice;
        public string VenueZPrice
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
