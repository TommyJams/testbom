using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TommyJams.Resources;
using Microsoft.Phone.Controls;
using TommyJams.ViewModel;
using System.Net;
using Newtonsoft.Json;
using System.Text;

namespace TommyJams.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Priority1Items = new ObservableCollection<EventItem>();
            this.PriorityItems = new ObservableCollection<EventItem>();
            this.Priority2Items = new ObservableCollection<EventItem>();
            //this.Event1 = new BandMember();
            
        }

        public ObservableCollection<EventItem> Priority1Items { get; private set; }
        public ObservableCollection<EventItem> PriorityItems { get; private set; }
        public ObservableCollection<EventItem> Priority2Items { get; private set; }


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
            private set;
        }

        public void LoadData()
        {
            //client = new FacebookClient();
            WebClient wc = new WebClient();
            String defaultUri = "https://testneo4j.azure-mobile.net/api/getPrimaryEvents?";
            String completeUri = defaultUri + "fbid=" + App.FacebookId + "city=" + App.city + "country=" + App.country;

            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            wc.DownloadStringAsync(new System.Uri(completeUri));

            /*this.Priority1Items.Add(new EventItem() { EventName = "Aebhasdhsdarosmith", EventLocation = "Hard Rock Cafe", EventSong = "/Model/Babel.mp3", EventImage = "../Model/play.gif" });
            this.Priority1Items.Add(new EventItem() { EventName = "Something", EventLocation = "10D", EventSong = "/Model/I Will Wait.mp3", EventImage = "../Model/play.gif" });
            
            this.Priority2Items.Add(new EventItem() { Genre = "alternative rock", BandName = "Aerosmith", Distance = "3 Km", Venue = "Hard Rock Cafe", SongLink = "../Model/Babel.mp3", Image = "../Model/play.gif" });
            this.Priority2Items.Add(new EventItem() { Genre = "alternative rock", BandName = "Something", Distance = "5 Km", Venue = "10D", SongLink = "/Model/I Will Wait.mp3", Image = "../Model/play.gif" });
            this.Priority2Items.Add(new EventItem() { Genre = "alternative rock", BandName = "Aerosmith", Distance = "3 Km", Venue = "Hard Rock Cafe", SongLink = "/Model/Babel.mp3", Image = "../Model/play.gif" });
            */
            this.IsDataLoaded = true;
        }

        private void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                try
                {
                    String result = e.Result;

                    ObservableCollection<EventItem> json = JsonConvert.DeserializeObject<ObservableCollection<EventItem>>(result) as ObservableCollection<EventItem>;
                    this.Priority1Items = json;
                    StringBuilder productsString = new StringBuilder();
                    foreach (EventItem aProduct in json)
                    {
                        productsString.AppendFormat("{0}", aProduct.EventName);
                        //gigsHeader.Header = aProduct.EventName.ToString();
                        break;
                    }
                    //mainHeader.Header = productsString.ToString();

                    //TextBlock.Text = json;
                }
                catch (Exception ex)
                {
                    //gigsHeader.Header = "Exception Thrown";
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