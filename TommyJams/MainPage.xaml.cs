using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
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

namespace TommyJams.View
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
            DataContext = App.ViewModel;
            LoadData();
        }

        //TODO: Move this to viewmodel
        private async Task AuthenticateAsync()
        {
            if (App.MobileService.CurrentUser == null) //Login
            {
                try
                {
                    await App.ViewModel.LoginToFacebook();

                    if (App.MobileService.CurrentUser != null)
                    {
                        BitmapImage bm = new BitmapImage(new Uri("http://graph.facebook.com/" + App.FacebookId + "/picture?type=square", UriKind.Absolute));
                        fbUserImage.Source = bm;

                        ToggleNotifications();
                        LoadData();
                    }
                }
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Sorry, could not authenticate using Facebook!");
                }
                catch (NullReferenceException)
                {
                    MessageBox.Show("Sorry, could not load data!");
                }
                catch (HttpRequestException e)
                {
                    MessageBox.Show("Sorry, could not connect to the internet!");
                }
            }
            else //Logout
            {
                App.ViewModel.LogoutFromFacebook();

                if (App.MobileService.CurrentUser == null)
                {
                    BitmapImage bm = new BitmapImage(new Uri("../Resources/Image/facebook_icon_large.gif", UriKind.Relative));
                    fbUserImage.Source = bm;

                    LoadData();
                }
            }
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
           
            //ProgressBar.IsIndeterminate = false;
            ProgressBar.Visibility = Visibility.Collapsed;
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


        private void On_Tap_Event(object sender, System.Windows.Input.GestureEventArgs e)
        {
            StackPanel selected = sender as StackPanel;
            EventItem data = selected.DataContext as EventItem;
            App.EventID = data.EventID;
            App.ViewModel.NotificationItem.EventName = data.EventName;
            App.ViewModel.NotificationItem.EventDate = data.EventDate;
            App.ViewModel.NotificationItem.EventTime = data.EventTime;
            App.ViewModel.NotificationItem.EventPrice = data.EventPrice;
            App.ViewModel.NotificationItem.EventDistance = data.EventDistance;
            App.ViewModel.NotificationItem.EventImage = data.EventImage;
            App.ViewModel.NotificationItem.VenueName = data.VenueName;
            App.ViewModel.NotificationItem.EventImage = data.EventImage;
            NavigationService.Navigate(new Uri("/../../View/EventPanoramaPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void On_Tap_Invite(object sender, System.Windows.Input.GestureEventArgs e)
        {
            StackPanel selected = sender as StackPanel;
            NotificationItem data = selected.DataContext as NotificationItem;
            App.EventID = data.EventID;
            App.ViewModel.NotificationItem.EventName = data.EventName;
            App.ViewModel.NotificationItem.EventDate = data.EventDate;
            App.ViewModel.NotificationItem.EventTime = data.EventTime;
            App.ViewModel.NotificationItem.EventPrice = data.EventPrice;
            App.ViewModel.NotificationItem.EventDistance = data.EventDistance;
            App.ViewModel.NotificationItem.EventImage = data.EventImage;
            App.ViewModel.NotificationItem.VenueName = data.VenueName;
            App.ViewModel.NotificationItem.EventImage = data.EventImage;
            App.ViewModel.NotificationItem.InviteExists = data.InviteExists;
            App.ViewModel.NotificationItem.InviteeName = data.InviteeName;
            App.ViewModel.NotificationItem.InviteeImage = data.InviteeImage;
            NavigationService.Navigate(new Uri("/../../View/EventPanoramaPage.xaml", UriKind.RelativeOrAbsolute));
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
    }
}