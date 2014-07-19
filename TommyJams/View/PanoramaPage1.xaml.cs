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
            //App.ViewModel.LoadData();
            /*client = new FacebookClient();
            WebClient wc = new WebClient();
            String defaultUri = "https://testneo4j.azure-mobile.net/api/getPrimaryEvents?";
            String completeUri = defaultUri + "fbid=" + App.FacebookId + "&city=" + App.city + "&country=" + App.country;

            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            wc.DownloadStringAsync(new System.Uri(completeUri));*/
        }
        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            /*if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }*/
            App.viewModel.LoadData();
            //App.ViewModel.LoadData();
            /*WebClient wc = new WebClient();
            String defaultUri = "https://testneo4j.azure-mobile.net/api/getPrimaryEvents?";
            String completeUri = defaultUri + "fbid=" + App.FacebookId +"&city=" + App.city + "&country=" + App.country;

            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            wc.DownloadStringAsync(new System.Uri(completeUri));*/
        }

        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                try
                {
                    String result = e.Result;

                    ObservableCollection<EventItem> json = JsonConvert.DeserializeObject<ObservableCollection<EventItem>>(result) as ObservableCollection<EventItem>;
                    //this.Priority1Items = json;
                    StringBuilder productsString = new StringBuilder();
                    foreach (EventItem aProduct in json)
                    {
                        productsString.AppendFormat("{0}", aProduct.EventName);
                        gigsHeader.Header = aProduct.EventName.ToString();
                        //DateTime date = DateTime.ParseExact(aProduct.EventTime, "yyyyMMdd",CultureInfo.InvariantCulture);
                        break;
                    }
                    //mainHeader.Header = productsString.ToString();

                    
                    //TextBlock.Text = json;
                }
                catch (Exception ex)
                {
                    gigsHeader.Header = "Exception Thrown";
                }


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
            //AudioPlayer.Source = new Uri(data.SongLink,UriKind.Relative);
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Controller/FacebookLoginPage.xaml", UriKind.Relative));
            

        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {

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