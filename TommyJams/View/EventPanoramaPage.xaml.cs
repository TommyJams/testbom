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
using Microsoft.Phone.Scheduler;

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
        bool fromTile = false;
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
            try
            {
                App.ViewModel.NotificationItem = await App.ViewModel.LoadEventInfo();
            }
            catch(System.Net.Http.HttpRequestException)
            {            }
            if (fromTile)
            {
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
            }

            App.ViewModel.ArtistInfo = await App.ViewModel.LoadArtistInfo();
            App.ViewModel.VenueInfo = await App.ViewModel.LoadVenueInfo();
            try
            {
                App.ViewModel.SocialInfo = await App.ViewModel.LoadSocialInfo();
            }
            catch (System.Net.Http.HttpRequestException e) { }
            App.ViewModel.TicketInfo = await App.ViewModel.LoadTicketInfo();

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
            if (e.NavigationMode == NavigationMode.New)
            {
                LoadData();
                string eventdate, eventprice, eventtime, eventvenue, eventaddress, distance, eventname;
                if (NavigationContext.QueryString.TryGetValue("eventdate", out eventdate))
                {
                    panel5_date.Text = Textblock_date.Text = eventdate;
                    fromTile = true;
                }
                if (NavigationContext.QueryString.TryGetValue("eventprice", out eventprice))
                {
                    panel5_total.Text = panel5_price.Text = Textblock_price.Text = eventprice;
                }
                if (NavigationContext.QueryString.TryGetValue("eventtime", out eventtime))
                {
                    panel5_time.Text = Textblock_time.Text = eventtime;
                }
                if (NavigationContext.QueryString.TryGetValue("eventvenue", out eventvenue))
                {
                    panel3_venue.Text = panel5_venue.Text = Textblock_venue.Text = eventvenue;
                    mainHeader.Header = "@" + eventvenue;
                }
                if (NavigationContext.QueryString.TryGetValue("eventaddress", out eventaddress))
                {
                    panel3_address.Text = panel5_address.Text = eventaddress;
                }
                if (NavigationContext.QueryString.TryGetValue("distance", out distance))
                {
                    Textblock_distance.Text = distance;
                }
                if (NavigationContext.QueryString.TryGetValue("eventname", out eventname))
                {
                    Panorama.Title = eventname;
                }
            }
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

        private void Buy_Tickets(object sender, EventArgs e)
        {
            WebBrowserTask wbt = new WebBrowserTask();
            int numTickets = TicketSelector.SelectedIndex + 1;
            wbt.Uri = new Uri("http://" + App.ViewModel.TicketInfo.TicketLink + "?tickets=" + numTickets, UriKind.Absolute);
            wbt.Show();
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
            catch(Exception e)
            {
                MessageBox.Show("Sorry, could not join event!");
                return;
            }
            try
            {
                if (settings_extension.CalenderEntries_setting_status())
                    //createReminder();
                    createAppointment();
            }
            catch(Exception e)
            {
            }
        }

        private void createAppointment()
        {
            DateTime beginTime = DateTime.ParseExact(App.ViewModel.NotificationItem.EventDate, "yyyyMMdd", CultureInfo.InvariantCulture)
                + DateTime.ParseExact(App.ViewModel.NotificationItem.EventTime, "HHmm", CultureInfo.InvariantCulture).TimeOfDay;
            if (beginTime < DateTime.Now)
            {
                return;
            }
            DateTime expirationTime = beginTime + TimeSpan.FromHours(3);

            Microsoft.Phone.Tasks.SaveAppointmentTask sat = new SaveAppointmentTask();
            sat.AppointmentStatus = Microsoft.Phone.UserData.AppointmentStatus.Busy;

            sat.Details = App.ViewModel.NotificationItem.EventName;

            sat.EndTime = expirationTime;

            sat.IsAllDayEvent = false;

            sat.Location = App.ViewModel.NotificationItem.VenueAddress;

            sat.Reminder = Microsoft.Phone.Tasks.Reminder.OneDay;

            sat.StartTime = beginTime;

            sat.Subject = App.ViewModel.NotificationItem.VenueName;
            sat.Show();
        }

        #region  obsoleteCode
        //private void createReminder()
        //{
        //    string name = App.ViewModel.NotificationItem.EventID;
        //    DateTime beginTime = DateTime.ParseExact(App.ViewModel.NotificationItem.EventDate, "yyyyMMdd", CultureInfo.InvariantCulture)
        //        + DateTime.ParseExact(App.ViewModel.NotificationItem.EventTime,"HHmm",CultureInfo.InvariantCulture).TimeOfDay ;
        //    if (beginTime < DateTime.Now)
        //    {
        //        return;
        //    }
        //    DateTime expirationTime = beginTime + TimeSpan.FromHours(1);

        //    // Determine which recurrence radio button is checked.
        //    RecurrenceInterval recurrence = RecurrenceInterval.None;
        //    string uri = string.Format("/View/EventPanoramaPage.xaml?eventid={0}&eventdate={1}&eventtime={3}",
        //            App.ViewModel.NotificationItem.EventID, Textblock_date.Text, panel5_price.Text, Textblock_time.Text, panel3_venue.Text, panel3_address.Text, Textblock_distance.Text, Panorama.Title);
        //    Microsoft.Phone.Scheduler.Reminder reminder = new Microsoft.Phone.Scheduler.Reminder(name);
        //    reminder.Title = App.ViewModel.NotificationItem.VenueName;
        //    reminder.Content = App.ViewModel.NotificationItem.VenueAddress;
        //    reminder.BeginTime = beginTime;
        //    reminder.ExpirationTime = expirationTime;
        //    reminder.RecurrenceType = recurrence;
        //    //uri = HttpUtility.HtmlEncode(uri);
        //    reminder.NavigationUri = new Uri(uri, UriKind.Relative);

        //    // Register the reminder with the system.
        //    try
        //    {
        //        ScheduledActionService.Remove(reminder.Name);
        //    }
        //    catch { }
        //    ScheduledActionService.Add(reminder);
        //    bool exist = false;
        //    foreach(Reminder r in settings_extension.Reminders)
        //    {
        //        if(r.Name == name) {exist = true;}
        //    }
        //    if(!exist) settings_extension.Reminders.Add(new Reminder(name));
        //    settings_extension.saveReminders();
        //}
#endregion

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
                proceed.Click += Buy_Tickets;
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
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.ApplicationId = App.MAP_APPLICATION_ID;
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.AuthenticationToken = App.MAP_AUTHENTICATION_TOKEN;
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
            if (panel5_total != null && App.ViewModel.NotificationItem.EventPrice!=null)
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
                string uri = string.Format("/View/EventPanoramaPage.xaml?eventid={0}&eventdate={1}&eventprice={2}&eventtime={3}&eventvenue={4}&eventaddress={5}&distance={6}&eventname={7}",
                    App.ViewModel.NotificationItem.EventID, Textblock_date.Text, panel5_price.Text, Textblock_time.Text, panel3_venue.Text, panel3_address.Text, Textblock_distance.Text, Panorama.Title);
                // Create the Tile and pin it to Start. This will cause a navigation to Start and a deactivation of our app.
                ShellTile.Create(new Uri(uri, UriKind.Relative), NewTileData);
            }
            catch(Exception)
            {
                MessageBox.Show("Sorry, could not create a repeat tile!");
            }
        }
    }
}