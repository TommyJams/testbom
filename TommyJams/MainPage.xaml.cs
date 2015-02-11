using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
//TODO: Remove reference to newtonsoft from view
using TommyJams.Model;
using TommyJams.Resources;
using TommyJams.ViewModel;
using Newtonsoft.Json;
using System.Text;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Controls.Primitives;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Popups;
using System.Diagnostics;
using Windows.UI.Notifications;

namespace TommyJams.View
{
    public partial class MainPage : PhoneApplicationPage
    {
        Uri facebookpic = new Uri("../Resources/Image/facebook_icon_large.gif", UriKind.Relative);
        public MainPage()
        {
            InitializeComponent();
            DataContext = App.ViewModel;
            AuthenticateAsync(true/*initial load*/);
        }

        private async Task AuthenticateAsync(bool fInitialLoad = false)
        {
            if (App.MobileService.CurrentUser == null) //Login
            {
                try
                {
                    await App.ViewModel.LoginToFacebook();

                    ToggleConnect();
                        ToggleNotifications();
                    }
                catch (InvalidOperationException)
                {
                    if(!fInitialLoad)
                        MessageBox.Show("Sorry, could not authenticate using Facebook!");
                }
                catch (NullReferenceException)
                {
                    if (!fInitialLoad)
                        MessageBox.Show("Sorry, could not load data!");
                }
                catch (HttpRequestException e)
                {
                    if (!fInitialLoad)
                        MessageBox.Show("Sorry, could not connect to the internet!");
                }
            }

            LoadData();
        }

        public async void LoadData()
        {
            ProgressBar.Visibility = Visibility.Visible;
            try
            {
                await App.ViewModel.LoadPrimaryEvents();
                await App.ViewModel.LoadSecondaryEvents();
                if(App.MobileService.CurrentUser != null)
                {
                    await App.ViewModel.LoadNotifications();
                    await App.FBViewModel.LoadFacebookFriends();
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Sorry, could not load data!");
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Sorry, could not connect to the internet!");
            }
            fbUserImage.Source = new BitmapImage(facebookpic);
            //ProgressBar.IsIndeterminate = false;
            ProgressBar.Visibility = Visibility.Collapsed;
        }

        public void ToggleConnect()
        {
            if (App.MobileService.CurrentUser == null)
            {
                panelConnect.Visibility = Visibility.Visible;
            }
            else
            {
                panelConnect.Visibility = Visibility.Collapsed;
            }
        }

        public void ToggleNotifications()
        {
            if (App.MobileService.CurrentUser != null)
            {
                panelNotifications.Visibility = Visibility.Visible;
            }
            else
            {
                panelNotifications.Visibility = Visibility.Collapsed;
            }
        }

        /*
        private void MainLongListSelector1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LongListSelector selector = sender as LongListSelector;
            if (selector == null)
                return;
            EventItem data = selector.SelectedItem as EventItem;

            if (data == null)
                return;

            AudioPlayer.Source = new Uri(data.SongLink,UriKind.RelativeOrAbsolute);
            selector.SelectedItem = null;
        }*/

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            HyperlinkButton selector = sender as HyperlinkButton;
            EventItem data = selector.DataContext as EventItem;
            //AudioPlayer.Source = new Uri(data.SongLink,UriKind.Relative);
            try
            {
                if(data.EventSong.StartsWith("www."))
                {
                    data.EventSong= data.EventSong.Insert(0, "http://");
                }
                WebBrowserTask webBrowserTask = new WebBrowserTask();
                webBrowserTask.Uri = new Uri(data.EventSong, UriKind.Absolute);
                webBrowserTask.Show();
            }
            catch(UriFormatException)
            {
                MessageBox.Show("Sorry, this stream could not be processed.");
            }
        }

        private async void FacebookLogin_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new Uri("/View/FacebookLoginPage.xaml", UriKind.Relative));
            
            await AuthenticateAsync();
        }

        private async void RefreshButton_Click(object sender, EventArgs e)
        {
            LoadData();
        }
        private void MainListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ListBox).SelectedIndex != -1)
            {
                if ((sender as ListBox).SelectedItem.GetType() == typeof(EventItem))
                {
                    EventItem selected = (sender as ListBox).SelectedItem as EventItem;
                    App.EventID = selected.EventID;
                    App.ViewModel.NotificationItem.EventName = selected.EventName;
                    App.ViewModel.NotificationItem.EventDate = selected.EventDate;
                    App.ViewModel.NotificationItem.EventTime = selected.EventTime;
                    App.ViewModel.NotificationItem.EventPrice = selected.EventPrice;
                    App.ViewModel.NotificationItem.EventDistance = selected.EventDistance;
                    App.ViewModel.NotificationItem.EventImage = selected.EventImage;
                    App.ViewModel.NotificationItem.VenueName = selected.VenueName;
                    App.ViewModel.NotificationItem.EventImage = selected.EventImage;
        }
                else
                {
                    NotificationItem selected = (sender as ListBox).SelectedItem as NotificationItem;

                    App.EventID = selected.EventID;
                    App.ViewModel.NotificationItem.EventName = selected.EventName;
                    App.ViewModel.NotificationItem.EventDate = selected.EventDate;
                    App.ViewModel.NotificationItem.EventTime = selected.EventTime;
                    App.ViewModel.NotificationItem.EventPrice = selected.EventPrice;
                    App.ViewModel.NotificationItem.EventDistance = selected.EventDistance;
                    App.ViewModel.NotificationItem.EventImage = selected.EventImage;
                    App.ViewModel.NotificationItem.VenueName = selected.VenueName;
                    App.ViewModel.NotificationItem.EventImage = selected.EventImage;
                    App.ViewModel.NotificationItem.InviteExists = selected.InviteExists;
                    App.ViewModel.NotificationItem.InviteeName = selected.InviteeName;
                    App.ViewModel.NotificationItem.InviteeImage = selected.InviteeImage;
                }
            NavigationService.Navigate(new Uri("/../../View/EventPanoramaPage.xaml", UriKind.RelativeOrAbsolute));
        }
        }

        private void Panorama_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Panorama.SelectedIndex == 2)
            {
                ApplicationBar.IsVisible = false;
            }
            else
            {
                ApplicationBar.IsVisible = true;
            }

        }

        private void ShowSplash()
        {
            Popup popup = new Popup();
            // Create an object of type SplashScreen.
            WindowsPhoneControl1 splash = new WindowsPhoneControl1();
            popup.Child = splash;
            popup.IsOpen = true;
            // Create an object of type BackgroundWorker and its events.
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (s, a) =>
            {
                //This event occurs while the task is executing.
                Thread.Sleep(4000); //A little dummy delay for show the effect
            };
            bw.RunWorkerCompleted += (s, a) =>
            {
                //This event occurs when the execution of the task has terminated.
                popup.IsOpen = false;
            };
            // Call to the Async Function, that produce the delay of the progress bar.
            // After that the pictured "Smoked by Windows Phone shown"
            bw.RunWorkerAsync();
        }

        public BackgroundWorker backroungWorker { get; set; }

        private void Settings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/Settings.xaml", UriKind.Relative));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.NavigationMode == NavigationMode.Back)
            {
                MainListBox2.SelectedIndex = -1;
                MainListBox.SelectedIndex = -1;
                upcomingEventsListBox.SelectedIndex = -1;
                invitationListBox.SelectedIndex = -1;
                //LoadData();
            }
            ToggleConnect();
            ToggleNotifications();
            resetDefaultTile();
        }

        private void resetDefaultTile()
        {
            ShellTile myTile = ShellTile.ActiveTiles.First();
            if (myTile != null)
            {
                // Create a new data to update my tile with
                FlipTileData myTileData = new FlipTileData
                {
                    Title = "TommyJams",
                    BackgroundImage = new Uri("Assets/Tiles/FlipCycleTileMedium.png", UriKind.Relative),
                    BackTitle = "TommyJams",
                    Count = 0,

                    SmallBackgroundImage = new Uri("Assets/Tiles/FlipCycleTileSmall.png", UriKind.Relative),
                    WideBackgroundImage = new Uri("Assets/Tiles/FlipCycleTileLarge.png", UriKind.Relative),
                    BackBackgroundImage = new Uri("", UriKind.Relative),
                    BackContent = "Description"
                };
                myTile.Update(myTileData);
            }
        }

        

    }
}