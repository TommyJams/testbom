using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Maps.Toolkit;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Tasks;
using Newtonsoft.Json;
using TommyJams;
using TommyJams.Model;
using TommyJams.Resources;
using TommyJams.ViewModel;

namespace TommyJams.View
{
    class Product
    {
        public String fbid { get; set; }
        public String eventID { get; set; }
    }

    public partial class EventPanoramaPage : PhoneApplicationPage
    {
        public EventPanoramaPage()
        {
            InitializeComponent();

            panel5_date.Text = Textblock_date.Text = App.ViewModel.EventItem.EventDate;
            panel5_price.Text = Textblock_price.Text = App.ViewModel.EventItem.EventPrice.ToString();
            panel5_time.Text = Textblock_time.Text = App.ViewModel.EventItem.EventTime;
            panel5_venue.Text = Textblock_venue.Text = App.ViewModel.EventItem.VenueName;
            Panorama.Title = App.ViewModel.EventItem.EventName;
            mainHeader.Header = "@"+App.ViewModel.EventItem.VenueName;

            LoadData();
            AddButtons();

            this.DataContext = App.ViewModel;
        }

        public async void LoadData()
        {
            App.ViewModel.EventItem = await App.ViewModel.LoadEventInfo();
            App.ViewModel.ArtistInfo = await App.ViewModel.LoadArtistInfo();
            App.ViewModel.VenueInfo = await App.ViewModel.LoadVenueInfo();

            ArtistListBox.ItemsSource = App.ViewModel.ArtistInfo;
            VenueGrid.DataContext = App.ViewModel.VenueInfo;
            venueMap.Center = App.ViewModel.VenueInfo.VenueGeoCoordinate;
        }

        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                try
                {
                    String result = e.Result;
                    CivicAddressResolver resolver = new CivicAddressResolver();
                    List<EventItem> json = JsonConvert.DeserializeObject<List<EventItem>>(result) as List<EventItem>;
                    StringBuilder productsString = new StringBuilder();
                    foreach (EventItem aProduct in json)
                    {
                    productsString.AppendFormat("{0}", aProduct.EventName);
                    GeoCoordinate a = new GeoCoordinate();
                    String[] Location = (aProduct.VenueCoordinates.Split(' '));
                    a.Latitude = 32.16;
                    a.Longitude = -117.71;
                        //a.Latitude = Convert.ToDouble(Location[0]);
                    //a.Longitude = Convert.ToDouble(Location[1]);
                    a.Altitude = 0;
                    a.Course = 0;
                    a.HorizontalAccuracy = 0;
                    a.VerticalAccuracy = 0;
                    a.Speed = 0;
                    /*var pushpin = MapExtensions.GetChildren(Map).OfType<Pushpin>().First(p => p.Name == "RouteDirectionsPushPin");
                    pushpin.GeoCoordinate = a;
                    Map.Center = a;
                    Map.ZoomLevel = 15;
                    //map_reference.Text = aProduct.VenueName + " " + aProduct.VenueAddress + " " + aProduct.VenueCity;
                    
                    MapOverlay overlay = new MapOverlay
                    {
                        GeoCoordinate = Map.Center,
                        Content = new Ellipse
                        {
                            Fill = new SolidColorBrush(Colors.Red),
                            Width = 40,
                            Height = 40
                        }
                        
                    };
                    MapLayer layer = new MapLayer();
                    layer.Add(overlay);

                    Map.Layers.Add(layer);*/
                    break;
                    }
                    mainHeader.Header = productsString.ToString();
                    //TextBlock.Text = json;
                }
                catch (Exception ex)
                {
                    mainHeader.Header = "Exception Thrown"; 
                }


            }
        }

        private void Event_Accept(object sender, EventArgs e)
        {
            this.AcceptInvite();

        }

        public void Demo_Refresh(object sender, EventArgs e)
        {
            ArtistListBox.ItemsSource = App.ViewModel.ArtistInfo;
            //var pushpin = MapExtensions.GetChildren(Map).OfType<Pushpin>().First(p => p.Name == "RouteDirectionsPushPin");
            //pushpin.GeoCoordinate = App.ViewModel.VenueInfo.VenueGeoCoordinate;
            /*Map.Center = App.ViewModel.VenueInfo.VenueGeoCoordinate;
            Map.ZoomLevel = 15;
            //map_reference.Text = App.ViewModel.VenueInfo.VenueName + " " + App.ViewModel.VenueInfo.VenueAddress + " " + App.ViewModel.VenueInfo.VenueCity;

            MapOverlay overlay = new MapOverlay
            {
                GeoCoordinate = Map.Center,
                Content = new Ellipse
                {
                    Fill = new SolidColorBrush(Colors.Red),
                    Width = 40,
                    Height = 40
                }

            };
            MapLayer layer = new MapLayer();
            layer.Add(overlay);

            Map.Layers.Add(layer);
            */

        }

        public void AcceptInvite()
        {
            Uri myUri = new Uri("https://testneo4j.azure-mobile.net/api/joinEvent");
            HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(myUri);
            myRequest.Method = "POST" ;
            myRequest.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), myRequest);
            //RouteDirectionsPushPin.GeoCoordinate=
        }

        void GetRequestStreamCallback(IAsyncResult callbackResult)
        {
            HttpWebRequest myRequest = (HttpWebRequest)callbackResult.AsyncState;

            // End the stream request operation
            Stream postStream = myRequest.EndGetRequestStream(callbackResult);

            // Create the post data
            string postData = "{\"fbid\":\"56784957689798\",\"eventID\":\"5\"}";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            // Add the post data to the web request
            postStream.Write(byteArray, 0, byteArray.Length);
            postStream.Close();

            // Start the web request
            myRequest.BeginGetResponse(new AsyncCallback(GetResponsetStreamCallback), myRequest);
        }

        void GetResponsetStreamCallback(IAsyncResult callbackResult)
        {
            //lib = new ApiLibrary();

            try
            {
                HttpWebRequest request = (HttpWebRequest)callbackResult.AsyncState;
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(callbackResult);
                string result = "";
                using (StreamReader httpWebStreamReader = new StreamReader(response.GetResponseStream()))
                {
                    result = httpWebStreamReader.ReadToEnd();
                }

                string APIResult = result;
                //description.Text = "asdasda";
                String a = "";
            }
            catch (Exception e)
            {

            }
        }

        private void AddButtons()
        {
            //accept button
            ApplicationBarIconButton accept = new ApplicationBarIconButton();
            accept.Text = "Accept";
            accept.IconUri = new Uri("/Resources/Images/accept.jpg",UriKind.Relative);
            accept.Click += Event_Accept;
            ApplicationBar.Buttons.Add(accept);
            //reject button
            ApplicationBarIconButton decline = new ApplicationBarIconButton();
            decline.Text = "Decline";
            decline.IconUri = new Uri("/Resources/Images/decline.jpg",UriKind.Relative);
            ApplicationBar.Buttons.Add(decline);
            //add bookmark button
            ApplicationBarIconButton pinevent = new ApplicationBarIconButton();
            pinevent.Text = "Pin Event";
            pinevent.IconUri = new Uri("/Resources/Images/pinevent.jpg", UriKind.Relative);
            ApplicationBar.Buttons.Add(pinevent);
            //add invite friend button
            ApplicationBarIconButton invitefriend = new ApplicationBarIconButton();
            invitefriend.Text = "Invite Friends";
            invitefriend.IconUri = new Uri("/Resources/Images/invitefriends.jpg",UriKind.Relative);
            ApplicationBar.Buttons.Add(invitefriend);
            
        }

        private void Panorama_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Removing buttons
            if (Panorama.SelectedIndex == 0)
            { if(ApplicationBar.Buttons.Count == 1)
                {
                    ApplicationBar.Buttons.RemoveAt(0);
                    AddButtons();
                }

            }
            else if(Panorama.SelectedIndex == 1)
            {
                if(ApplicationBar.Buttons.Count > 1)
                {
                    ApplicationBar.Buttons.RemoveAt(0);
                    ApplicationBar.Buttons.RemoveAt(0);
                    ApplicationBar.Buttons.RemoveAt(1);
                }
            }
            else if(Panorama.SelectedIndex == 2)
            {
                if(ApplicationBar.Buttons.Count > 1)
                {
                    ApplicationBar.Buttons.RemoveAt(0);
                    ApplicationBar.Buttons.RemoveAt(0);
                    ApplicationBar.Buttons.RemoveAt(0);
                }
            }
            else if(Panorama.SelectedIndex == 3)
            {
                if(ApplicationBar.Buttons.Count ==1)
                {
                    ApplicationBar.Buttons.RemoveAt(0);
                    AddButtons();
                }
                
            }
            else if(Panorama.SelectedIndex == 4)
            {
                ApplicationBar.Buttons.RemoveAt(0);
                ApplicationBar.Buttons.RemoveAt(0);
                ApplicationBar.Buttons.RemoveAt(0);
                ApplicationBar.Buttons.RemoveAt(0);
                ApplicationBarIconButton proceed = new ApplicationBarIconButton();
                proceed.Text = "Proceed";
                proceed.IconUri=new Uri("/Resource/Images/proceed.jpg",UriKind.Relative);
                ApplicationBar.Buttons.Add(proceed);
            }
        }

        private void venueMap_Loaded(object sender, RoutedEventArgs e)
        {
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.ApplicationId = "ApplicationID";
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.AuthenticationToken = "AuthenticationToken";
        }

        private async void venueMap_Tap(object sender, RoutedEventArgs e)
        {
            MapsDirectionsTask mapsDirectionsTask = new MapsDirectionsTask();
            LabeledMapLocation venueLML = new LabeledMapLocation(App.ViewModel.VenueInfo.VenueName, App.ViewModel.VenueInfo.VenueGeoCoordinate);
            mapsDirectionsTask.End = venueLML;
            mapsDirectionsTask.Show();
        }
    }
}