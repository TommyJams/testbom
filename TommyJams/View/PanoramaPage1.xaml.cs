using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TommyJams.Model;
using TommyJams.Resources;
using Facebook;
using Microsoft.WindowsAzure.MobileServices;
using TommyJams.ViewModel;
using Newtonsoft.Json;
using System.Text;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Controls.Primitives;
using System.ComponentModel;
using System.Threading;

namespace TommyJams.View
{
    public partial class PanoramaPage1 : PhoneApplicationPage
    {
        //public ObservableCollection<EventItem> Priority1Items { get; private set; }

        //private MobileServiceUser user;
        private const string FBApi = "657116527691677";
        private FacebookClient client;
        public PanoramaPage1()
        {
            InitializeComponent();
            DataContext = App.ViewModel;
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            /*if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }*/
            App.ViewModel.LoadPrimaryEvents();
            App.ViewModel.LoadSecondaryEvents();
            MainListBox.ItemsSource = App.ViewModel.Priority1Items;
            
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
                Thread.Sleep(8000); //A little dummy delay for show the effect
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
            Button selector = sender as Button;
            EventItem data = selector.DataContext as EventItem;
            //AudioPlayer.Source = new Uri(data.SongLink,UriKind.Relative);
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Controller/FacebookLoginPage.xaml", UriKind.Relative));
            

        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            MainListBox.ItemsSource = App.ViewModel.Priority1Items;
            MainListBox2.ItemsSource = App.ViewModel.Priority2Items;
        }

        private void Invite_Accept(object sender, EventArgs e)
        {

        }

        private void On_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            StackPanel selected = sender as StackPanel;
            EventItem data = selected.DataContext as EventItem;
            App.EventID = data.EventID;
            NavigationService.Navigate(new Uri("/../../View/EventPanoramaPage.xaml", UriKind.RelativeOrAbsolute));

        }

    }
    

}