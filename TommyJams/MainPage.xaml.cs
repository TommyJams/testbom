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
        Uri facebookpic = new Uri("../Resources/Image/facebook-icon.jpg", UriKind.Relative);
        CancellationTokenSource cts;
        public MainPage()
        {
            InitializeComponent();
            DataContext = App.ViewModel;

            //Fires when no city is saved on app launch and the user is located in a supported city
            //this condition can happen only once as the city will be saved fist time this event is fired
            //will also be called if location is changed explicitly
            App.cityChanged += App_cityChanged;
            cts  = new CancellationTokenSource();
            AuthenticateAsync(true/*initial load*/);
        }

        void App_cityChanged()
        {
            cts.Cancel();
            cts = new CancellationTokenSource();
            LoadData();
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
            cts.Cancel();
            cts = new CancellationTokenSource();
            LoadData();
        }

        
        public async void LoadData()
        {
            ProgressBar.Visibility = Visibility.Visible;
            Boolean fPrimary = true;
            Boolean fSecondary = true;
            try
            {
                fPrimary = await App.ViewModel.LoadPrimaryEvents(cts.Token);
                primaryNA.Visibility = fPrimary ? Visibility.Collapsed : Visibility.Visible;
                fSecondary = await App.ViewModel.LoadSecondaryEvents(cts.Token);
                secondaryNA.Visibility = fSecondary ? Visibility.Collapsed : Visibility.Visible;
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
            catch(TaskCanceledException)
            { }
            fbUserImage.Source = new BitmapImage(facebookpic);
            //ProgressBar.IsIndeterminate = false;
            ProgressBar.Visibility = Visibility.Collapsed;
            primaryNA.Visibility = fPrimary ? Visibility.Collapsed : Visibility.Visible;
            secondaryNA.Visibility = fSecondary ? Visibility.Collapsed : Visibility.Visible;
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

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            Button selector = sender as Button;
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
            await AuthenticateAsync();
        }

        private async void RefreshButton_Click(object sender, EventArgs e)
        {
            cts.Cancel();
            cts = new CancellationTokenSource();
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
            if (Panorama.SelectedIndex == 3)
            {
                Task.Run(() => {
                    Thread.Sleep(200);
                    Dispatcher.BeginInvoke(()=>ApplicationBar.Mode = ApplicationBarMode.Minimized);                    
                });                
            }
            else
            {
                Task.Run(() =>
                {
                    Thread.Sleep(200);
                    Dispatcher.BeginInvoke(() => ApplicationBar.Mode = ApplicationBarMode.Default);
                }); 
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

        private void About_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/about.xaml", UriKind.Relative));
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
            //resetDefaultTile();
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