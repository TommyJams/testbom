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
    public class EventViewModel
    {
        public EventViewModel()
        {
            this.Priority1Items = new ObservableCollection<EventItem>();
            this.Priority2Items = new ObservableCollection<EventItem>();
            this.eventItem = new EventItem();
            this.artistInfo = new ObservableCollection<ArtistInfo>();
            this.venueInfo = new VenueInfo();
            this.model = new Models();
        }

        public ObservableCollection<EventItem> Priority1Items { get; set; }
        public ObservableCollection<EventItem> Priority2Items { get; set; }
        public EventItem eventItem { get; set; }
        public ObservableCollection<ArtistInfo> artistInfo { get; set; }
        public VenueInfo venueInfo { get; set; }
        public Models model { get; set; }

        private string _sampleProperty = "Sample Runtime Property Value";
        public string SampleProperty
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }

        public string LocalizedSampleProperty
        {
            get
            {
                return AppResources.SampleProperty;
            }
        }

        public bool IsDataLoaded
        {
            get;
            set;
        }

        public async Task<ObservableCollection<EventItem>> LoadPrimaryEvents()
        {

            var primaryItems = await model.GetPrimaryEvents();
            return primaryItems;


            //download.Wait();
            //var result = download.Result;
        }

        public async Task<ObservableCollection<EventItem>> LoadSecondaryEvents()
        {
            var secondaryItems = await model.GetSecondaryEvents();
            return secondaryItems;
        }

        public async Task<EventItem> LoadEventInfo()
        {
            var eventInfo = await model.GetEventInfo();
            return eventInfo;
        }

        public async Task<ObservableCollection<ArtistInfo>> LoadArtistInfo()
        {
            var artistInfo = await model.GetArtistInfo();
            return artistInfo;
        }

        public async Task<VenueInfo> LoadVenueInfo()
        {
            var venueInfo = await model.GetVenueInfo();
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