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
            foreach (ArtistInfo artist in artistInfo)
            {
                artist.ArtistImage = "http://" + artist.ArtistImage;
            }
            return artistInfo;        

        }


        public async Task<EventItem> GetEventInfo()
        {

            String defaultUri = "https://testneo4j.azure-mobile.net/api/getEventInfo?";
            String completeUri = defaultUri + "fbid=" + App.FacebookId + "&eventID=" + App.EventID;
            HttpClient client = new HttpClient();
            string result = await client.GetStringAsync(completeUri);

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
                else if ((eventDate - currentDate).TotalDays <= 7)
                {
                    aProduct.EventStartingTime = eventDate.DayOfWeek.ToString();
                }
                else
                {
                    aProduct.EventStartingTime = "on " + eventDate.Day + "/" + eventDate.Month;
                }

                String[] location = aProduct.VenueCoordinates.Split(' ');

                Double latitude = Convert.ToDouble(location[0]);
                Double longitude = Convert.ToDouble(location[1]);
                var eventGeo = new GeoCoordinate(latitude, longitude);
                //Stub for replacing myGeoCordinate from App
                var myGeo = new GeoCoordinate(72.2, 84.3);
                double x = eventGeo.GetDistanceTo(myGeo);
                aProduct.EventDistance = "• " + ((int)(x / 1000)).ToString() + " Kms";
                aProduct.EventPrice = "₹ " + aProduct.EventPrice;
                aProduct.EventHotness = "• Hotness Level :" + aProduct.EventHotness;
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
            VenueInfo venue = new VenueInfo();
            /*dynamic json = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(result);
            var VenueName = json[0]["venueName"];
            venue.VenueName = VenueName.ToString();
            venue.VenueCity = json[0]["venueCity"].ToString();
            venue.VenueAddress = json[0]["venueAddress"].ToString();
            venue.VenueFacebook = json[0]["venueFacebook"].ToString();
            venue.VenueZomato = json[0]["venueZomato"].ToString();
            venue.VenueZPrice = json[0]["venueZPrice"];
            venue.VenueZRating = json[0]["venueZRating"];
            venue.VenueZType = json[0]["venueZType"].ToString();*/
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

        public async Task<ObservableCollection<OtherUser>> GetSocialInfo()
        {
            String defaultUri = "https://testneo4j.azure-mobile.net/api/getSocialInfo?";
            String completeUri = defaultUri + "fbid=" + App.FacebookId + "&eventID=" + App.EventID;

            HttpClient client = new HttpClient();
            Task<String> GetResult = client.GetStringAsync(completeUri);
            string result = await GetResult;
            
            ObservableCollection<OtherUser> people = JsonConvert.DeserializeObject<ObservableCollection<OtherUser>>(result) as ObservableCollection<OtherUser>;
            
            foreach (OtherUser person in people)
            {
                person.pictureUri = "http://graph.facebook.com/" + person.id + "/picture?type=square";
            }

            return people;
        }

        public async Task<ObservableCollection<EventItem>> GetUpcomingEvents()
        {
            String defaultUri = "https://testneo4j.azure-mobile.net/api/getUpcomingEvents?";
            String completeUri = defaultUri + "fbid=" + App.FacebookId;
            HttpClient client = new HttpClient();
            Task<String> GetResult = client.GetStringAsync(completeUri);
            string result = await GetResult;

            ObservableCollection<EventItem> upcomingEventsList = JsonConvert.DeserializeObject<ObservableCollection<EventItem>>(result) as ObservableCollection<EventItem>;

            StringBuilder genreString = new StringBuilder();
            foreach (EventItem aProduct in upcomingEventsList)
            {
                foreach (String genre in aProduct.EventTags)
                {
                    genreString.AppendFormat("#{0} ", genre);
                }
                aProduct.EventGenre = genreString.ToString();
                genreString.Clear();

                DateTime eventTime = DateTime.ParseExact(aProduct.EventTime, "HHmm", CultureInfo.InvariantCulture);
                aProduct.EventTime = eventTime.ToShortTimeString();

                DateTime eventDate = DateTime.ParseExact(aProduct.EventDate, "yyyyMMdd", CultureInfo.InvariantCulture);
                aProduct.EventDate = " • " + eventDate.ToShortDateString();
                aProduct.EventPrice = " • ₹" + aProduct.EventPrice;
            }

            return upcomingEventsList;
        }

        public async Task<ObservableCollection<NotificationItem>> GetInvitations()
        {
            String defaultUri = "https://testneo4j.azure-mobile.net/api/getInvitations?";
            String completeUri = defaultUri + "fbid=" + App.FacebookId;
            HttpClient client = new HttpClient();
            Task<String> GetResult = client.GetStringAsync(completeUri);
            string result = await GetResult;

            ObservableCollection<NotificationItem> notificationsList = JsonConvert.DeserializeObject<ObservableCollection<NotificationItem>>(result) as ObservableCollection<NotificationItem>;

            StringBuilder genreString = new StringBuilder();
            foreach (NotificationItem aProduct in notificationsList)
            {
                foreach (String genre in aProduct.EventTags)
                {
                    genreString.AppendFormat("#{0} ", genre);
                }
                aProduct.EventGenre = genreString.ToString();
                genreString.Clear();

                DateTime eventTime = DateTime.ParseExact(aProduct.EventTime, "HHmm", CultureInfo.InvariantCulture);
                aProduct.EventTime = eventTime.ToShortTimeString();

                DateTime eventDate = DateTime.ParseExact(aProduct.EventDate, "yyyyMMdd", CultureInfo.InvariantCulture);
                aProduct.EventDate = " • " + eventDate.ToShortDateString();
                aProduct.EventPrice = " • ₹" + aProduct.EventPrice;

                if (aProduct.InviteeFBID.Length > 0)
                {
                    aProduct.InviteExists = true;
                    aProduct.InviteeImage = "http://graph.facebook.com/" + aProduct.InviteeFBID + "/picture?type=square";
                }
            }

            return notificationsList;
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
                foreach (String genre in aProduct.EventTags)
                {
                    genreString.AppendFormat("#{0} ", genre);
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
                else if ((eventDate - currentDate).TotalDays <= 7)
                {
                    aProduct.EventStartingTime = eventDate.DayOfWeek.ToString();
                }
                else
                {
                    aProduct.EventStartingTime = "on " + eventDate.Day + "/" + eventDate.Month;
                }

                String[] location = aProduct.VenueCoordinates.Split(' ');

                Double latitude = Convert.ToDouble(location[0]);
                Double longitude = Convert.ToDouble(location[1]);
                var eventGeo = new GeoCoordinate(latitude, longitude);
                var myGeo = new GeoCoordinate(72.2, 84.3);
                double x = eventGeo.GetDistanceTo(myGeo);
                
                aProduct.EventDistance = " • " + ((int)(x / 1000)).ToString() + "Km";
                aProduct.EventPrice = " ₹" + aProduct.EventPrice;
                aProduct.EventHotness = " • " + aProduct.EventHotness;
            }
            return primaryEvents;

        }

        public async Task<int> GetUserPresence()
        {
            String defaultUri = "https://testneo4j.azure-mobile.net/api/getUserPresence?";
            String completeUri = defaultUri + "fbid=" + App.FacebookId;

            HttpClient client = new HttpClient();
            string result = await client.GetStringAsync(completeUri);
            var jResult = JsonConvert.DeserializeObject<List<User>>(result);
            User u = jResult[0];

            return (u.fbid != null) ? 1 : 0;
        }

        public async Task<string> PushAddUser(User u)
        {
            String defaultUri = "https://testneo4j.azure-mobile.net/api/createuser";

            string postData = JsonConvert.SerializeObject(u);
            
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(defaultUri, new StringContent(postData, Encoding.UTF8, "application/json"));
            string content = await response.Content.ReadAsStringAsync();
            
            //response.EnsureSuccessStatusCode();
            if(!response.IsSuccessStatusCode)
            {
                if(!content.Contains("already exists"))
                {
                    throw new HttpRequestException();
                }
            }

            return content;
        }

        public async Task<string> PushJoinEvent()
        {
            String defaultUri = "https://testneo4j.azure-mobile.net/api/joinevent";

            if (App.MobileService.CurrentUser != null)
            {
                string postData = "{\"fbid\":\"" + App.FacebookId + "\", \"eventID\":\"" + App.ViewModel.NotificationItem.EventID + "\"}";

                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PostAsync(defaultUri, new StringContent(postData, Encoding.UTF8, "application/json"));
                string content = await response.Content.ReadAsStringAsync();

                response.EnsureSuccessStatusCode();
                
                return content;
            }
            else
            {
                throw new Exception("Login!");
            }
        }

        public async Task<string> PushInviteFriends()
        {
            String defaultUri = "https://testneo4j.azure-mobile.net/api/invitefriends";

            if (App.MobileService.CurrentUser != null)
            {
                string postData = "{\"fbid\":\"" + App.FacebookId + "\", \"eventID\":\"" + App.ViewModel.NotificationItem.EventID + "\", \"invites\":[";
                bool inviteeExists = false;

                foreach(OtherUser friendItem in App.FBViewModel.SelectedFriends)
                {
                    postData += "{\"fbid\":\"" + friendItem.id + "\"},";
                    inviteeExists = true;
                }

                if (inviteeExists)
                    postData = postData.Remove(postData.Length - 1);
                postData += "]}";

                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PostAsync(defaultUri, new StringContent(postData, Encoding.UTF8, "application/json"));
                string content = await response.Content.ReadAsStringAsync();

                response.EnsureSuccessStatusCode();

                return content;
            }
            else
            {
                throw new Exception("Login!");
            }
        }
    }

 

}
