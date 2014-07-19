using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TommyJams.Resources;
using Windows.UI.Popups;
using System.Threading;
using Microsoft.WindowsAzure.MobileServices;
using Facebook;
using TommyJams.ViewModel;
using Newtonsoft.Json;

namespace TommyJams
{
    public class TodoItem
    {
        public string Id { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "complete")]
        public bool Complete { get; set; }
    }


    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            //Thread.Sleep(3000);
            DataContext = App.ViewModel;
            //Loaded+=SplashPage_Loaded;
            //Loaded += LoadUserInfo;
            
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }



        /*
        private void LoadUserInfo(object sender, RoutedEventArgs e1)
        {
            var fb = new FacebookClient(App.AccessToken);

            fb.GetCompleted += (o, e) =>
            {
                if (e.Error != null)
                {
                    Dispatcher.BeginInvoke(() => NavigationService.Navigate(new Uri("/View/PanoramaPage1.xaml", UriKind.Relative)));
                    return;
                }
                Dispatcher.BeginInvoke(() => NavigationService.Navigate(new Uri("/View/PanoramaPage2.xaml", UriKind.Relative)));

            };

            //fb.GetTaskAsync("me");
        }
        */
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                //App.ViewModel.LoadData();

            }
            NavigationService.Navigate(new Uri("/View/PanoramaPage1.xaml", UriKind.Relative));
        }
        



        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}