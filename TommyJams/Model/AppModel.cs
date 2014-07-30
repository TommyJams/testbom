using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TommyJams;
using TommyJams.Model;

namespace TommyJams.Model
{
    public class AppModel
    {

        public AppModel()
        {

        }


        public async Task<ObservableCollection<ArtistInfo>> GetArtistInfo()
        {

            String defaultUri = "https://testneo4j.azure-mobile.net/api/getArtistInfo?";
            String completeUri = defaultUri + "eventID=" + App.EventID;
            HttpClient client = new HttpClient();
            Task<String> GetResult = client.GetStringAsync(completeUri);
            string result = await GetResult;
            ObservableCollection<ArtistInfo> artistInfo = JsonConvert.DeserializeObject<ObservableCollection<ArtistInfo>>(result) as ObservableCollection<ArtistInfo>;
            return artistInfo;        

        }


        public async Task<EventItem> GetEventInfo()
        {

            String defaultUri = "https://testneo4j.azure-mobile.net/api/getEventInfo?";
            String completeUri = defaultUri + "eventID=" + App.EventID;
            HttpClient client = new HttpClient();
            Task<String> GetResult = client.GetStringAsync(completeUri);
            string result = await GetResult;

            CivicAddressResolver resolver = new CivicAddressResolver();
            List<EventItem> eventInfo = JsonConvert.DeserializeObject<List<EventItem>>(result) as List<EventItem>;

            return eventInfo[0];



        }

        public async Task<ObservableCollection<EventItem>> GetSecondaryEvents()
        {
            String defaultUri = "https://testneo4j.azure-mobile.net/api/getSecondaryEvents?";
            String completeUri = defaultUri + "fbid=" + App.FacebookId + "&city=" + App.city + "&country=" + App.country;
            HttpClient client = new HttpClient();
            Task<String> GetResult = client.GetStringAsync(completeUri);
            string result = await GetResult;

            ObservableCollection<EventItem> secondaryEvents = JsonConvert.DeserializeObject<ObservableCollection<EventItem>>(result) as ObservableCollection<EventItem>;

            StringBuilder genreString = new StringBuilder();
            foreach (EventItem aProduct in secondaryEvents)
            {
                genreString.Append("genre:");
                foreach (String genre in aProduct.EventTags)
                {
                    genreString.AppendFormat("{0} ", genre);
                }
                aProduct.EventGenre = genreString.ToString();
                genreString.Clear();
                DateTime eventDate = DateTime.ParseExact(aProduct.EventDate, "yyyyMMdd", CultureInfo.InvariantCulture);
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

                String[] location = aProduct.VenueCoordinates.Split(' ');

                Double latitude = Convert.ToDouble(location[0]);
                Double longitude = Convert.ToDouble(location[1]);
                var eventGeo = new GeoCoordinate(latitude, longitude);
                //Stub for replacing myGeoCordinate from App
                var myGeo = new GeoCoordinate(72.2, 84.3);
                double x = eventGeo.GetDistanceTo(myGeo);
                aProduct.EventDistance = (int)(x / 1000);

            }

            return secondaryEvents;

        }

        public async Task<VenueInfo> GetVenueInfo()
        {
            String defaultUri = "https://testneo4j.azure-mobile.net/api/getVenueInfo?";
            String completeUri = defaultUri + "eventID=" + App.EventID;
            
            HttpClient client = new HttpClient();
            Task<String> GetResult = client.GetStringAsync(completeUri);
            string result = await GetResult;
            VenueInfo venue;
            ObservableCollection<VenueInfo> json = JsonConvert.DeserializeObject<ObservableCollection<VenueInfo>>(result) as ObservableCollection<VenueInfo>;
            StringBuilder productsString = new StringBuilder();
            venue = json[0];
            GeoCoordinate a = new GeoCoordinate();
            String[] Location = (venue.VenueCoordinates.Split(' '));
            a.Latitude = Convert.ToDouble(Location[0]);
            a.Longitude = Convert.ToDouble(Location[1]);
            a.Altitude = 0;
            a.Course = 0;
            a.HorizontalAccuracy = 0;
            a.VerticalAccuracy = 0;
            a.Speed = 0;
            venue.VenueGeoCoordinate = a;
            
            return venue;
           

        }

        public async Task<ObservableCollection<EventItem>> GetPrimaryEvents()
        {
            String defaultUri = "https://testneo4j.azure-mobile.net/api/getPrimaryEvents?";
            String completeUri = defaultUri + "fbid=" + App.FacebookId + "&city=" + App.city + "&country=" + App.country;
            
            HttpClient client = new HttpClient();
            Task<String> GetResult = client.GetStringAsync(completeUri);
            string result = await GetResult;
            ObservableCollection<EventItem> primaryEvents = JsonConvert.DeserializeObject<ObservableCollection<EventItem>>(result) as ObservableCollection<EventItem>;

            StringBuilder genreString = new StringBuilder();
            foreach (EventItem aProduct in primaryEvents)
            {
                genreString.Append("genre:");
                foreach (String genre in aProduct.EventTags)
                {
                    genreString.AppendFormat("{0} ", genre);
                }
                aProduct.EventGenre = genreString.ToString();
                genreString.Clear();
                DateTime eventDate = DateTime.ParseExact(aProduct.EventDate, "yyyyMMdd", CultureInfo.InvariantCulture);
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

                String[] location = aProduct.VenueCoordinates.Split(' ');

                Double latitude = Convert.ToDouble(location[0]);
                Double longitude = Convert.ToDouble(location[1]);
                var eventGeo = new GeoCoordinate(latitude, longitude);
                var myGeo = new GeoCoordinate(72.2, 84.3);
                double x = eventGeo.GetDistanceTo(myGeo);
                aProduct.EventDistance = (int)(x / 1000);
                //int a = 0;

            }
            return primaryEvents;

        }
    }

 

}
