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

namespace TommyJams.Model
{
    public class EventItem
    {
        private string _bandname;
        /// <summary>
        /// Sample ViewModel property; this property is used to identify the object.
        /// </summary>
        /// <returns></returns>
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
                }
            }
        }

        private string _genre;
        /// <summary>
        /// Sample ViewModel property; this property is used to identify the object.
        /// </summary>
        /// <returns></returns>
        public string Genre
        {
            get
            {
                return _genre;
            }
            set
            {
                if (value != _genre)
                {
                    _genre = value;
                    NotifyPropertyChanged("Genre");
                }
            }
        }

        private string _venue;
        /// <summary>
        /// Sample ViewModel property; this property is used to identify the object.
        /// </summary>
        /// <returns></returns>
        public string Venue
        {
            get
            {
                return _venue;
            }
            set
            {
                if (value != _venue)
                {
                    _venue = value;
                    NotifyPropertyChanged("Venue");
                }
            }
        }

        private string _distance;
        /// <summary>
        /// Sample ViewModel property; this property is used to identify the object.
        /// </summary>
        /// <returns></returns>
        public string Distance
        {
            get
            {
                return _distance;
            }
            set
            {
                if (value != _distance)
                {
                    _distance = value;
                    NotifyPropertyChanged("Distance");
                }
            }
        }

        private string _songlink;
        /// <summary>
        /// Sample ViewModel property; this property is used to identify the object.
        /// </summary>
        /// <returns></returns>
        public string SongLink
        {
            get
            {
                return _songlink;
            }
            set
            {
                if (value != _songlink)
                {
                    _songlink = value;
                    NotifyPropertyChanged("SongLink");
                }
            }
        }

        private string _image;
        /// <summary>
        /// Sample ViewModel property; this property is used to identify the object.
        /// </summary>
        /// <returns></returns>
        public string Image
        {
            get
            {
                return _image;
            }
            set
            {
                if (value != _image)
                {
                    _image = value;
                    NotifyPropertyChanged("Image");
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
