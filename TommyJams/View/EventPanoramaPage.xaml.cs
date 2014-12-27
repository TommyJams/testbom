﻿using System;
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

            panel5_date.Text = Textblock_date.Text = App.ViewModel.NotificationItem.EventDate;
            panel5_price.Text = Textblock_price.Text = App.ViewModel.NotificationItem.EventPrice.ToString();
            panel5_time.Text = Textblock_time.Text = App.ViewModel.NotificationItem.EventTime;
            panel5_venue.Text = Textblock_venue.Text = App.ViewModel.NotificationItem.VenueName;
            if(App.ViewModel.NotificationItem.InviteExists)
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
                Panorama.Background = imageBrush;
            }

            Panorama.Title = App.ViewModel.NotificationItem.EventName;
            mainHeader.Header = "@"+App.ViewModel.NotificationItem.VenueName;
            
            LoadData();
            AddButtons();

            this.DataContext = App.ViewModel;
        }

        public async void LoadData()
        {
            App.ViewModel.NotificationItem = await App.ViewModel.LoadEventInfo();
            App.ViewModel.ArtistInfo = await App.ViewModel.LoadArtistInfo();
            App.ViewModel.VenueInfo = await App.ViewModel.LoadVenueInfo();

            ArtistListBox.ItemsSource = App.ViewModel.ArtistInfo;
            VenueGrid.DataContext = App.ViewModel.VenueInfo;
            venueMap.Center = App.ViewModel.VenueInfo.VenueGeoCoordinate;
            if (App.ViewModel.NotificationItem.EventImage != null)
            {
                BitmapImage bitmapImage = new BitmapImage(new Uri(App.ViewModel.NotificationItem.EventImage, UriKind.Absolute));
                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource = bitmapImage;
                Panorama.Background = imageBrush;
            }
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
            this.JoinEvent();

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
            if (Panorama.SelectedIndex != 4)
            {
                //add bookmark button
                ApplicationBarIconButton pinevent = new ApplicationBarIconButton();
                pinevent.Text = "Pin Event";
                pinevent.IconUri = new Uri("/Resources/Images/pinevent.jpg", UriKind.Relative);
                ApplicationBar.Buttons.Add(pinevent);
            }

            if (App.MobileService.CurrentUser != null && (Panorama.SelectedIndex == -1 || Panorama.SelectedIndex == 0 || Panorama.SelectedIndex == 3))
            {
                //accept button
                ApplicationBarIconButton accept = new ApplicationBarIconButton();
                accept.Text = "Accept";
                accept.IconUri = new Uri("/Resources/Images/accept.jpg", UriKind.Relative);
                accept.Click += Event_Accept;
                ApplicationBar.Buttons.Add(accept);

                //add invite friend button
                ApplicationBarIconButton invitefriend = new ApplicationBarIconButton();
                invitefriend.Text = "Invite Friends";
                invitefriend.IconUri = new Uri("/Resources/Images/invitefriends.jpg", UriKind.Relative);
                invitefriend.Click += Invite_Friends;
                ApplicationBar.Buttons.Add(invitefriend);
            }
            else if (App.MobileService.CurrentUser != null && Panorama.SelectedIndex == 4)
            {
                ApplicationBarIconButton proceed = new ApplicationBarIconButton();
                proceed.Text = "Proceed";
                proceed.IconUri = new Uri("/Resource/Images/proceed.jpg", UriKind.Relative);
                ApplicationBar.Buttons.Add(proceed);
            }
        }

        private void Panorama_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Removing buttons
            ApplicationBar.Buttons.Clear();
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

        private void Settings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/Settings.xaml",UriKind.Relative));
        }
    }
}