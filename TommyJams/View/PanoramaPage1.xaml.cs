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
using TommyJams;

namespace TommyJams.View
{
    public partial class PanoramaPage1 : PhoneApplicationPage
    {
        //private MobileServiceUser user;
        private const string FBApi = "657116527691677";
        private FacebookClient client;
        public PanoramaPage1()
        {
            InitializeComponent();
            DataContext = App.ViewModel;
            client = new FacebookClient();
        }
        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
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
            Button selector = sender as Button;
            EventItem data = selector.DataContext as EventItem;
            AudioPlayer.Source = new Uri(data.SongLink,UriKind.Relative);
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Controller/FacebookLoginPage.xaml", UriKind.Relative));
            

        }

    }
    

}