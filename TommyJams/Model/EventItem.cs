using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Microsoft.Phone.Controls;
using System.Collections.ObjectModel;
using System.Globalization;
using TommyJams;

namespace TommyJams.Model
{
    public class EventItem :INotifyPropertyChanged
    {
        private int _eventID;
        public int EventID
        {
            get
            {
                return _eventID;
            }
            set
            {
                if (value != _eventID)
                {
                    _eventID = value;
                }
            }
        }

        private string _eventName;
        public string EventName
        {
            get
            {
                return _eventName;
            }
            set
            {
                if (value != _eventName)
                {
                    _eventName = value;
                }
            }
        }


        private string _eventTime;
        public string EventTime
        {
            get
            {
                return _eventTime.ToString();
            }
            set
            {
                if (_eventTime != value)
                {
                    _eventTime = value;
                }
            }
        }

        private DateTime _theTime;
        public DateTime TheTime
        {
            get
            {
                return _theTime;
            }
            set
            {
                if (value != _theTime)
                {
                    _theTime = value;
                }
            }
        }


        private string _eventDate;
        public string EventDate
        {
            get
            {
                return _eventDate;
            }
            set
            {
                if (value != _eventDate)
                {
                    _eventDate = value;
                }
            }
        }

        private string _eventHotness;
        public string EventHotness
        {
            get
            {
                return _eventHotness;
            }
            set
            {
                if (value != _eventHotness)
                {
                    _eventHotness = value;
                }
            }
        }


        private string _eventPrice;
        public string EventPrice
        {
            get
            {
                return _eventPrice;
            }
            set
            {
                if (value != _eventPrice)
                {
                    _eventPrice = value;
                }
            }
        }

        
        private ObservableCollection<String> _eventTags;
        public ObservableCollection<String> EventTags
        {
            get
            {
                return _eventTags;
            }
            set
            {
                if (value != _eventTags)
                {
                    _eventTags = value;
                    NotifyPropertyChanged("EventTags");
                }
            }
        }

        private string _eventStartingTime;
        public string EventStartingTime
        {
            get
            {
                return _eventStartingTime;
            }
            set
            {
                if (value != _eventStartingTime)
                {
                    _eventStartingTime = value;
                    NotifyPropertyChanged("EventStartingTime");
                }
            }
        }


        private string _eventGenre;
        public string EventGenre
        {
            get
            {
                return _eventGenre;
            }
            set
            {
                if (value != _eventGenre)
                {
                    _eventGenre = value;
                    NotifyPropertyChanged("EventGenre");
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

        private string _eventDistance;
        public string EventDistance
        {
            get
            {
                return _eventDistance;
            }
            set
            {
                if (value != _eventDistance)
                {
                    _eventDistance = value;
                    NotifyPropertyChanged("EventDistance");
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

        private string _eventSong;
        public string EventSong
        {
            get
            {
                return _eventSong;
            }
            set
            {
                if (value != _eventSong)
                {
                    _eventSong = value;
                    NotifyPropertyChanged("EventSong");
                }
            }
        }

        private string _eventImage;
        public string EventImage
        {
            get
            {
                return _eventImage;
            }
            set
            {
                if (value != _eventImage)
                {
                    _eventImage = value;
                    NotifyPropertyChanged("EventImage");
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

    public class NotificationItem : EventItem
    {
        private string _inviteeName;
        public string InviteeName
        {
            get
            {
                return _inviteeName;
            }
            set
            {
                if (value != _inviteeName)
                {
                    _inviteeName = value;
                    NotifyPropertyChanged("InviteeName");
                }
            }
        }

        private string _inviteeFBID;
        public string InviteeFBID
        {
            get
            {
                return _inviteeFBID;
            }
            set
            {
                if (value != _inviteeFBID)
                {
                    _inviteeFBID = value;
                    NotifyPropertyChanged("InviteeFBID");
                }
            }
        }

        private string _inviteeImage;
        public string InviteeImage
        {
            get
            {
                return _inviteeImage;
            }
            set
            {
                if (value != _inviteeImage)
                {
                    _inviteeImage = value;
                    NotifyPropertyChanged("InviteeImage");
                }
            }
        }

        private bool _inviteExists = false;
        public bool InviteExists
        {
            get
            {
                return _inviteExists;
            }
            set
            {
                if (value != _inviteExists)
                {
                    _inviteExists = value;
                    NotifyPropertyChanged("InviteExists");
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

    public class User : INotifyPropertyChanged
    {
        private string _fbid;
        public string fbid
        {
            get
            {
                return _fbid;
            }
            set
            {
                if (value != _fbid)
                {
                    _fbid = value;
                    NotifyPropertyChanged("fbid");
                }
            }
        }

        private string _name;
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    NotifyPropertyChanged("name");
                }
            }
        }

        private string _email;
        public string email
        {
            get
            {
                return _email;
            }
            set
            {
                if (value != _email)
                {
                    _email = value;
                    NotifyPropertyChanged("email");
                }
            }
        }

        private string _dob;
        public string dob
        {
            get
            {
                return _dob;
            }
            set
            {
                if (value != _dob)
                {
                    _dob = value;
                    NotifyPropertyChanged("dob");
                }
            }
        }

        private string _gender;
        public string gender
        {
            get
            {
                return _gender;
            }
            set
            {
                if (value != _gender)
                {
                    _gender = value;
                    NotifyPropertyChanged("gender");
                }
            }
        }

        private string _city;
        public string city
        {
            get
            {
                return _city;
            }
            set
            {
                if (value != _city)
                {
                    _city = value;
                    NotifyPropertyChanged("city");
                }
            }
        }

        private string _country;
        public string country
        {
            get
            {
                return _country;
            }
            set
            {
                if (value != _country)
                {
                    _country = value;
                    NotifyPropertyChanged("country");
                }
            }
        }

        private string _phone;
        public string phone
        {
            get
            {
                return _phone;
            }
            set
            {
                if (value != _phone)
                {
                    _phone = value;
                    NotifyPropertyChanged("phone");
                }
            }
        }

        private string _ip;
        public string ip
        {
            get
            {
                return _ip;
            }
            set
            {
                if (value != _ip)
                {
                    _ip = value;
                    NotifyPropertyChanged("ip");
                }
            }
        }

        private string _lastLoginCity;
        public string lastLoginCity
        {
            get
            {
                return _lastLoginCity;
            }
            set
            {
                if (value != _lastLoginCity)
                {
                    _lastLoginCity = value;
                    NotifyPropertyChanged("lastLoginCity");
                }
            }
        }

        private string _lastLoginCountry;
        public string lastLoginCountry
        {
            get
            {
                return _lastLoginCountry;
            }
            set
            {
                if (value != _lastLoginCountry)
                {
                    _lastLoginCountry = value;
                    NotifyPropertyChanged("lastLoginCountry");
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
