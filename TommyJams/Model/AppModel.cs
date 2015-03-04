using FourSquare.SharpSquare.Core;
using FourSquare.SharpSquare.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TommyJams;
using TommyJams.Model;
using Windows.Devices.Geolocation;

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
            eventInfo[0].EventPrice = "₹ " + eventInfo[0].EventPrice;
            return eventInfo[0];
        }

        public async Task<ObservableCollection<EventItem>> GetSecondaryEvents(CancellationToken ct)
        {
            String defaultUri = "https://testneo4j.azure-mobile.net/api/getSecondaryEvents?";
            String completeUri = defaultUri + "fbid=" + App.FacebookId + "&city=" + App.city + "&country=" + App.country;
            HttpClient client = new HttpClient( new HttpClientHandler
                {
                    AutomaticDecompression = DecompressionMethods.GZip
                                             | DecompressionMethods.Deflate
                });
            HttpResponseMessage GetResult = await client.GetAsync(completeUri, HttpCompletionOption.ResponseContentRead, ct);
            
            string result = await GetResult.Content.ReadAsStringAsync();

            ObservableCollection<EventItem> secondaryEvents = JsonConvert.DeserializeObject<ObservableCollection<EventItem>>(result) as ObservableCollection<EventItem>;

            StringBuilder genreString = new StringBuilder();
            foreach (EventItem aProduct in secondaryEvents)
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

                aProduct.EventDate = eventDate.ToString("ddd") + ", " + eventDate.Day + " " + eventDate.ToString("MMM");
                aProduct.EventTime = eventDate.ToString("hh:mm tt");

                String[] location = aProduct.VenueCoordinates.Split(' ');

                Double latitude = Convert.ToDouble(location[0]);
                Double longitude = Convert.ToDouble(location[1]);
                var eventGeo = new GeoCoordinate(latitude, longitude);

                Geolocator myGeolocator = new Geolocator();
                Geoposition myGeoposition = await myGeolocator.GetGeopositionAsync();
                Geocoordinate myGeocoordinate = myGeoposition.Coordinate;
                var myGeo = new GeoCoordinate(myGeocoordinate.Latitude, myGeocoordinate.Longitude);
                double x = eventGeo.GetDistanceTo(myGeo);

                aProduct.EventDistance = ((int)(x / 1000)).ToString() + " Kms";
                aProduct.EventPrice = "₹ " + aProduct.EventPrice;
                aProduct.EventHotness =  aProduct.EventHotness;
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
                person.pictureUri = "http://graph.facebook.com/" + person.id + "/picture?width=100&height=100";
            }

            return people;
        }

        public async Task<TicketInfo> GetTicketInfo()
        {
            String defaultUri = "https://testneo4j.azure-mobile.net/api/getTicketInfo?";
            String completeUri = defaultUri + "eventID=" + App.EventID;

            HttpClient client = new HttpClient();
            Task<String> GetResult = client.GetStringAsync(completeUri);
            string result = await GetResult;
            TicketInfo ticketInfo = new TicketInfo();
            ObservableCollection<TicketInfo> json = JsonConvert.DeserializeObject<ObservableCollection<TicketInfo>>(result) as ObservableCollection<TicketInfo>;
            
            return json[0];
        }

        public async Task<VenueInfo_Foursquare> GetVenueInfo_Foursquare(string VenueID)
        {
            SharpSquare sharpSquare = new SharpSquare(Constants.Foursquare_ClientID, Constants.Foursquare_ClientSecret);
            VenueInfo_Foursquare venueInfo_Foursquare = new VenueInfo_Foursquare();
            Venue venue = await sharpSquare.GetVenue(VenueID);
            if (venue != null)
            {
                venueInfo_Foursquare.VenueZCheckin = Math.Round((double)venue.stats.checkinsCount/1000,1) + "k";

                if (venue.price != null)
                {
                    if (venue.price.tier == "1")
                        venueInfo_Foursquare.VenueZPrice = "min";
                    if (venue.price.tier == "2" || venue.price.tier == "3")
                        venueInfo_Foursquare.VenueZPrice = "avg";
                    if (venue.price.tier == "4")
                        venueInfo_Foursquare.VenueZPrice = "max";
                }

                venueInfo_Foursquare.VenueZRating = venue.rating;
                venueInfo_Foursquare.FoursquareInfo_Visibility = true;
                venueInfo_Foursquare.VenueZLink = venue.canonicalUrl;
                foreach (Category tempC in venue.categories)
                    venueInfo_Foursquare.VenueZDesc += "#" + tempC.name + " ";
                if(venue.description != null)
                {
                    venueInfo_Foursquare.VenueZDesc += "\n" + venue.description;
                }
            }

            return venueInfo_Foursquare;
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
                aProduct.EventDate =  eventDate.ToShortDateString();
                aProduct.EventPrice = "₹ " + aProduct.EventPrice;
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
                aProduct.EventDate =  eventDate.ToShortDateString();
                aProduct.EventPrice = "₹ " + aProduct.EventPrice;

                if (aProduct.InviteeFBID.Length > 0)
                {
                    aProduct.InviteExists = true;
                    aProduct.InviteeImage = "http://graph.facebook.com/" + aProduct.InviteeFBID + "/picture?width=100&height=100";
                }
            }

            return notificationsList;
        }

        public async Task<ObservableCollection<EventItem>> GetPrimaryEvents(CancellationToken ct)
        {
            String defaultUri = "https://testneo4j.azure-mobile.net/api/getPrimaryEvents?";
            String completeUri = defaultUri + "fbid=" + App.FacebookId + "&city=" + App.city + "&country=" + App.country;
            
            HttpClient client = new HttpClient();
            HttpResponseMessage GetResult = await client.GetAsync(completeUri, ct);

            string result = GetResult.Content.ReadAsStringAsync().Result;
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

                aProduct.EventDate = eventDate.ToString("ddd") + ", " + eventDate.Day + " " + eventDate.ToString("MMM");
                aProduct.EventTime = eventDate.ToString("hh:mm tt");

                String[] location = aProduct.VenueCoordinates.Split(' ');

                Double latitude = Convert.ToDouble(location[0]);
                Double longitude = Convert.ToDouble(location[1]);
                var eventGeo = new GeoCoordinate(latitude, longitude);
                
                Geolocator myGeolocator = new Geolocator();
                Geoposition myGeoposition = await myGeolocator.GetGeopositionAsync();
                Geocoordinate myGeocoordinate = myGeoposition.Coordinate;
                var myGeo = new GeoCoordinate(myGeocoordinate.Latitude, myGeocoordinate.Longitude);
                double x = eventGeo.GetDistanceTo(myGeo);
                
                aProduct.EventDistance = ((int)(x / 1000)).ToString() + " Km";
                aProduct.EventPrice = "₹ " + aProduct.EventPrice;
                aProduct.EventHotness =  aProduct.EventHotness;
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
           
            if (jResult.Count != 0)
            {
                User u = jResult[0];
                return (u.fbid != null) ? 1 : 0;
            }
            else
                return 0;
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
                    //postData += "{\"fbid\":\"" + "1572155721" + "\"},";
                    inviteeExists = true;
                }

                if (inviteeExists)
                    postData = postData.Remove(postData.Length - 1);
                postData += "]}";

                HttpClient client = new HttpClient();
                HttpResponseMessage response = null;
                response = await client.PostAsync(defaultUri, new StringContent(postData, Encoding.UTF8, "application/json"));
                try
                {
                    response.EnsureSuccessStatusCode();
                }
                catch (HttpRequestException ex)
                {
                    return null;
                }

                string content = await response.Content.ReadAsStringAsync();


                return content;
            }
            else
            {
                throw new Exception("Login!");
            }
        }
    }

 

}
