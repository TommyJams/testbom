using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
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
        public string appbarTileId = "EventSecondaryTile";
        public EventPanoramaPage()
        {
            InitializeComponent();
            LoadBasicView();
            this.DataContext = App.ViewModel;
        }

        private void LoadBasicView()
        {
            panel5_date.Text = Textblock_date.Text = App.ViewModel.NotificationItem.EventDate;
            panel5_total.Text = panel5_price.Text = Textblock_price.Text = App.ViewModel.NotificationItem.EventPrice;
            panel5_time.Text = Textblock_time.Text = App.ViewModel.NotificationItem.EventTime;
            panel3_venue.Text = panel5_venue.Text = Textblock_venue.Text = App.ViewModel.NotificationItem.VenueName;
            panel3_address.Text = panel5_address.Text = App.ViewModel.NotificationItem.VenueAddress;
            Textblock_distance.Text = App.ViewModel.NotificationItem.EventDistance;
            if (App.ViewModel.NotificationItem.InviteExists)
            {
                panel4InviteeName.Text = Textblock_inviteeName.Text = App.ViewModel.NotificationItem.InviteeName;

                BitmapImage bitmapImage = new BitmapImage(new Uri(App.ViewModel.NotificationItem.InviteeImage, UriKind.Absolute));
                ImageBrush imageBrush = new ImageBrush();
                panel4InviteeImage.Source = Textblock_inviteeImage.Source = bitmapImage;

                panel4Invite.Visibility = panel1Invite.Visibility = Visibility.Visible;
            }

            if (App.ViewModel.NotificationItem.EventImage != null)
            {
                BitmapImage bitmapImage = new BitmapImage(new Uri(App.ViewModel.NotificationItem.EventImage, UriKind.Absolute));
                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource = bitmapImage;
                imageBrush.Opacity = 0.5;
                Panorama.Background = imageBrush;
            }

            Panorama.Title = App.ViewModel.NotificationItem.EventName;
            mainHeader.Header = "@" + App.ViewModel.NotificationItem.VenueName;
        }

        public async void LoadData()
        {
            App.ViewModel.NotificationItem = await App.ViewModel.LoadEventInfo();
            //LoadBasicView();

            App.ViewModel.ArtistInfo = await App.ViewModel.LoadArtistInfo();
            App.ViewModel.VenueInfo = await App.ViewModel.LoadVenueInfo();
            App.ViewModel.SocialInfo = await App.ViewModel.LoadSocialInfo();

            ArtistListBox.ItemsSource = App.ViewModel.ArtistInfo;
            VenueGrid.DataContext = App.ViewModel.VenueInfo;
            venueMap.Center = App.ViewModel.VenueInfo.VenueGeoCoordinate;

            int countAttendees = App.ViewModel.SocialInfo.Count();
            SocialListBox.ItemsSource = App.ViewModel.SocialInfo;
            if(countAttendees>0)
            {
                BitmapImage bitmapImage0 = new BitmapImage(new Uri(App.ViewModel.SocialInfo[0].pictureUri, UriKind.Absolute));
                panel1SocialImage0.Source = bitmapImage0;
                panel1SocialImage0.Visibility = Visibility.Visible;
                countAttendees--;
                if (countAttendees>0)
                {
                    BitmapImage bitmapImage1 = new BitmapImage(new Uri(App.ViewModel.SocialInfo[1].pictureUri, UriKind.Absolute));
                    panel1SocialImage1.Source = bitmapImage1;
                    panel1SocialImage1.Visibility = Visibility.Visible;
                    countAttendees--;
                    if (countAttendees>0)
                    {
                        BitmapImage bitmapImage2 = new BitmapImage(new Uri(App.ViewModel.SocialInfo[2].pictureUri, UriKind.Absolute));
                        panel1SocialImage2.Source = bitmapImage2;
                        panel1SocialImage2.Visibility = Visibility.Visible;
                        countAttendees--;
                    }
                }
                panel1NumberAttending.Text = "+" + countAttendees + " attending";
                panel1Attending.Visibility = Visibility.Visible;
            }

            AddButtons();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string paramEventID;
            if (NavigationContext.QueryString.TryGetValue("eventid", out paramEventID))
            {
                App.EventID = paramEventID;
                if (App.MobileService.CurrentUser == null)
                try
                {
                    App.ViewModel.LoginToFacebook();
                }
                catch (Exception)
                {
                }
            }
            LoadData();
        }

        private void Event_Accept(object sender, EventArgs e)
        {
            this.JoinEvent();
            LoadData();
        }

        private void Invite_Friends(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/../../View/FriendSelector.xaml", UriKind.RelativeOrAbsolute));
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

        public async void JoinEvent()
        {
            try
            {
                string responseJoinEvent = await App.ViewModel.JoinEvent();
            }
            catch(Exception)
            {
                MessageBox.Show("Sorry, could not join event!");
            }
        }

        private void AddButtons()
        {
            //Removing buttons
            ApplicationBar.Buttons.Clear();

            if (Panorama.SelectedIndex != 4)
            {
                //add bookmark button
                ApplicationBarIconButton pinevent = new ApplicationBarIconButton();
                pinevent.Text = "Pin Event";
                pinevent.IconUri = new Uri("/Resources/Image/bar_events_pin.png", UriKind.Relative);
                pinevent.Click += Pin_Click;
                ApplicationBar.Buttons.Add(pinevent);
            }

            if (App.MobileService.CurrentUser != null && (Panorama.SelectedIndex == -1 || Panorama.SelectedIndex == 0 || Panorama.SelectedIndex == 3))
            {
                if (App.ViewModel.NotificationItem.UserAttending != "1")
                {
                    //accept button
                    ApplicationBarIconButton accept = new ApplicationBarIconButton();
                    accept.Text = "Join";
                    accept.IconUri = new Uri("/Resources/Image/bar_events_join.png", UriKind.Relative);
                    accept.Click += Event_Accept;
                    ApplicationBar.Buttons.Add(accept);
                }

                //add invite friend button
                ApplicationBarIconButton invitefriend = new ApplicationBarIconButton();
                invitefriend.Text = "Invite Friends";
                invitefriend.IconUri = new Uri("/Resources/Image/bar_events_invite.png", UriKind.Relative);
                invitefriend.Click += Invite_Friends;
                ApplicationBar.Buttons.Add(invitefriend);
            }
            else if (App.MobileService.CurrentUser != null && Panorama.SelectedIndex == 4)
            {
                ApplicationBarIconButton proceed = new ApplicationBarIconButton();
                proceed.Text = "Proceed";
                proceed.IconUri = new Uri("/Resources/Image/bar_events_proceed.png", UriKind.Relative);
                ApplicationBar.Buttons.Add(proceed);
            }
        }

        private void Panorama_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AddButtons();
        }

        //TODO: Get these authentication tokens when publishing app
        private void venueMap_Loaded(object sender, RoutedEventArgs e)
        {
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.ApplicationId = "ApplicationID";
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.AuthenticationToken = "AuthenticationToken";
        }

        private void venueMap_Tap(object sender, RoutedEventArgs e)
        {
            MapsDirectionsTask mapsDirectionsTask = new MapsDirectionsTask();
            LabeledMapLocation venueLML = new LabeledMapLocation(App.ViewModel.VenueInfo.VenueName, App.ViewModel.VenueInfo.VenueGeoCoordinate);
            mapsDirectionsTask.End = venueLML;
            mapsDirectionsTask.Show();
        }

        private void icon_tap(object sender, RoutedEventArgs e)
        {
            var realSender = (Image)sender;
            WebBrowserTask wbt = new WebBrowserTask();
            wbt.Uri = new Uri("https://" + realSender.Tag.ToString(), UriKind.Absolute);
            wbt.Show(); 
        }

        private void Settings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/Settings.xaml",UriKind.Relative));
        }

        private void Price_SelectionChanged(object sender, EventArgs e)
        {
            if (panel5_total != null)
            {
                var picker = sender as ListPicker;
                int totalPrice = (picker.SelectedIndex + 1) * Convert.ToInt32(App.ViewModel.NotificationItem.EventPrice.Substring(1, App.ViewModel.NotificationItem.EventPrice.Length - 1));
                panel5_total.Text = App.ViewModel.NotificationItem.EventPrice[0] + totalPrice.ToString();
            }
        }

        private void Pin_Click(object sender, EventArgs e)
        {
            StandardTileData NewTileData = new StandardTileData
            {
                Title = "Event: " + App.ViewModel.NotificationItem.EventName,
                Count = 0,
                BackgroundImage = new Uri("/Assets/Tiles/FlipCycleTileMedium.png", UriKind.Relative),
                BackTitle = DateTime.ParseExact(App.ViewModel.NotificationItem.EventDate, "yyyyMMdd",CultureInfo.InvariantCulture).ToShortDateString(),
                BackContent = App.ViewModel.NotificationItem.VenueName + ", " + App.ViewModel.NotificationItem.VenueAddress,
                BackBackgroundImage = new Uri(App.ViewModel.NotificationItem.EventImage, UriKind.Absolute)
            };

            try
            {
                // Create the Tile and pin it to Start. This will cause a navigation to Start and a deactivation of our app.
                ShellTile.Create(new Uri("/View/EventPanoramaPage.xaml?eventid=" + App.ViewModel.NotificationItem.EventID, UriKind.Relative), NewTileData);
            }
            catch(Exception)
            {
                MessageBox.Show("Sorry, could not create a repeat tile!");
            }
        }
    }
}