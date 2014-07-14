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
using TommyJams.ViewModel;
using System.Collections.ObjectModel;
using System.Globalization;

namespace TommyJams.ViewModel
{
    public class EventItem
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

        private int _eventHotness;
        public int EventHotness
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


        private int _eventPrice;
        public int EventPrice
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

        private string _eventLocation;
        public string EventLocation
        {
            get
            {
                return _eventLocation;
            }
            set
            {
                if (value != _eventLocation)
                {
                    _eventLocation = value;
                    NotifyPropertyChanged("EventLocation");
                }
            }
        }

        private int _eventDistance;
        public int EventDistance
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

        private string _eventCoordinates;
        public string EventCoordinates
        {
            get
            {
                return _eventCoordinates;
            }
            set
            {
                if (value != _eventCoordinates)
                {
                    _eventCoordinates = value;
                    NotifyPropertyChanged("EventCoordinates");
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
}
