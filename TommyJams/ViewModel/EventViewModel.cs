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
        }

        public ObservableCollection<EventItem> Priority1Items { get; private set; }
        public ObservableCollection<EventItem> PriorityItems { get; private set; }
        public ObservableCollection<EventItem> Priority2Items { get; private set; }
        public const double EarthRadiusInKilometers = 6367.0;
        //public ObservableCollection<BandMember> ParvaazMembers { get; private set; }
        //public Band Parvaaz { get; private set; }
        //public EventItem CurrentItem { get; private set; }

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

        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                try
                {
                    String result = e.Result;

                    this.Priority1Items = JsonConvert.DeserializeObject<ObservableCollection<EventItem>>(result) as ObservableCollection<EventItem>;
                    //this.Priority1Items = json;
                    StringBuilder genreString = new StringBuilder();
                    foreach (EventItem aProduct in Priority1Items)
                    {
                        genreString.Append("genre:");
                        foreach (String genre in aProduct.EventTags)
                        {
                            genreString.AppendFormat("{0} ", genre);
                        }
                        aProduct.EventGenre = genreString.ToString();
                        genreString.Clear();
                        DateTime eventDate = DateTime.ParseExact(aProduct.EventDate, "yyyyMMdd",CultureInfo.InvariantCulture);
                        DateTime currentDate = DateTime.Now;
                        if ((eventDate.Day == currentDate.Day) && (eventDate.Month == currentDate.Month))
                        {
                            DateTime eventTime = DateTime.ParseExact(aProduct.EventTime, "HHmm", CultureInfo.InvariantCulture);
                            var diff = eventTime.Hour - currentDate.Hour;
                            aProduct.EventStartingTime = "in " + diff + " hours";
                        }
                        else
                        {
                            aProduct.EventStartingTime = eventDate.ToString();
                        }

                        /*String[] location = aProduct.EventCoordinates.Split(' ');
                        String a = "9.12";
                        Double s = Convert.ToDouble(a);
                        aProduct.EventDistance = (int) CalcDistance(App.myGeocoordinate.Latitude,App.myGeocoordinate.Longitude,Convert.ToDouble(location[0]),Convert.ToDouble(location[1]));
                        *///break;
                    }
                    
                }
                catch (Exception ex)
                {
                    int a = 0;
                    //gigsHeader.Header = "Exception Thrown";
                }


            }
        }

        public void LoadData()
        {
            WebClient wc = new WebClient();
            String defaultUri = "https://testneo4j.azure-mobile.net/api/getPrimaryEvents?";
            String completeUri = defaultUri + "fbid=" + App.FacebookId + "&city=" + App.city + "&country=" + App.country;

            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            wc.DownloadStringAsync(new System.Uri(completeUri));


            /*
            this.Priority1Items.Add(new EventItem() { Genre = "alternative rock", BandName = "Aebhasdhsdarosmith", Distance = "3 Km", Venue = "Hard Rock Cafe", SongLink = "/Model/Babel.mp3", Image = "../Model/play.gif" });
            this.Priority1Items.Add(new EventItem() { Genre = "alternative rock", BandName = "Something", Distance = "5 Km", Venue = "10D", SongLink = "/Model/I Will Wait.mp3", Image = "../Model/play.gif" });

            this.Priority2Items.Add(new EventItem() { Genre = "alternative rock", BandName = "Aerosmith", Distance = "3 Km", Venue = "Hard Rock Cafe", SongLink = "../Model/Babel.mp3", Image = "../Model/play.gif" });
            this.Priority2Items.Add(new EventItem() { Genre = "alternative rock", BandName = "Something", Distance = "5 Km", Venue = "10D", SongLink = "/Model/I Will Wait.mp3", Image = "../Model/play.gif" });
            this.Priority2Items.Add(new EventItem() { Genre = "alternative rock", BandName = "Aerosmith", Distance = "3 Km", Venue = "Hard Rock Cafe", SongLink = "/Model/Babel.mp3", Image = "../Model/play.gif" });
            */ 
            //this.ParvaazMembers.Add(new BandMember() {BandMemberName="LoremIpsum",BandMemberRole="Lead Guitarist"});
            //this.ParvaazMembers.Add(new BandMember() { BandMemberName = "LoremIpsum2", BandMemberRole = "Lead Vocalist" });
            //this.Parvaaz = new Band() { BandMembers = ParvaazMembers, BandDescription = "LoremIpsum blah blah", BandName = "Parvaaz",BandFacebook="www.blah.com" };
            //this.CurrentItem = new EventItem() { Genre = "Alternative Rock", BandName = "Parvaaz", Band = Parvaaz, Venue="Hard Rock Cafe"  };


            this.IsDataLoaded = true;
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