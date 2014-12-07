using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using TommyJams.Model;
using TommyJams.Resources;


namespace TommyJams.ViewModel
{
    public class EventViewModel : INotifyPropertyChanged
    {
        public EventViewModel()
        {
            /*this.Priority1Items = new ObservableCollection<EventItem>();
            this.Priority2Items = new ObservableCollection<EventItem>();
            this.EventItem = new EventItem();
            this.ArtistInfo = new ObservableCollection<ArtistInfo>();
            this.VenueInfo = new VenueInfo();
            this.Model = new Models();*/
        }

        //public ObservableCollection<EventItem> Priority1Items { get; set; }

        private ObservableCollection<EventItem> _priority1Items = new ObservableCollection<EventItem>();
        public ObservableCollection<EventItem> Priority1Items 
        {
            get
            {
                return _priority1Items;
            }

            set
            {
                if (value != _priority1Items)
                {
                    _priority1Items = value;
                    NotifyPropertyChanged("Priority1Items");
                }
            }
        }

        private ObservableCollection<EventItem> _priority2Items = new ObservableCollection<EventItem>();
        public ObservableCollection<EventItem> Priority2Items 
        {
            get
            {
                return _priority2Items;
            }
            set
            {
                if (value != _priority2Items)
                {
                    _priority2Items = value;
                    NotifyPropertyChanged("Priority2Items");
                }
            }
         }
        private EventItem _eventItem = new EventItem();
        public EventItem EventItem 
        {
            get
            {
                return _eventItem;
            }
            set
            {
                if (value != _eventItem)
                {
                    _eventItem = value;
                    NotifyPropertyChanged("EventItem");
                }
            }
        }
        private ObservableCollection<ArtistInfo> _artistInfo = new ObservableCollection<ArtistInfo>();
        public ObservableCollection<ArtistInfo> ArtistInfo
        {
            get
            {
                return _artistInfo;
            }
            set
            {
                if (value != _artistInfo)
                {
                    _artistInfo = value;
                    NotifyPropertyChanged("ArtistInfo");
                }
            }
        }
        private VenueInfo _venueInfo = new VenueInfo();
        public VenueInfo VenueInfo 
        {
            get
            {
                return _venueInfo;
            }
            set
            {
                if (value != _venueInfo)
                {
                    _venueInfo = value;
                    NotifyPropertyChanged("VenueInfo");
                }
            }
        }
        private AppModel _appmodel = new AppModel();
        public AppModel AppModel
        {
            get
            {
                return _appmodel;
            }
            set
            {
                if (value != _appmodel)
                {
                    _appmodel = value;
                    NotifyPropertyChanged("AppModel");
                }
            }
        }


        
        public bool IsDataLoaded
        {
            get;
            set;
        }

        public async Task LoadPrimaryEvents()
        {
            Priority1Items = await AppModel.GetPrimaryEvents();
        }

        public async Task LoadSecondaryEvents()
        {
            Priority2Items = await AppModel.GetSecondaryEvents();
        }

        public async Task<EventItem> LoadEventInfo()
        {
            var eventInfo = await AppModel.GetEventInfo();
            return eventInfo;
        }

        public async Task<ObservableCollection<ArtistInfo>> LoadArtistInfo()
        {
            var artistInfo = await AppModel.GetArtistInfo();
            return artistInfo;
        }

        public async Task<VenueInfo> LoadVenueInfo()
        {
            var venueInfo = await AppModel.GetVenueInfo();
            return venueInfo;
            
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