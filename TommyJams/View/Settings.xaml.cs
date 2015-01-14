using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Facebook;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Tasks;
using System.IO.IsolatedStorage;
using Windows.Devices.Geolocation;
using Microsoft.Phone.Maps.Services;
using System.Windows.Threading;
using System.Device.Location;
using Microsoft.Phone.Notification;

namespace TommyJams.View
{
    public partial class Settings : PhoneApplicationPage
    {
        public Settings()
        {
            InitializeComponent();
            LoadUserInfo();
            city_list.City_LongListSelector.SelectionChanged += City_LongListSelector_SelectionChanged;   
        }

        void City_LongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (city_list.City_LongListSelector != null && city_list.City_LongListSelector.SelectedItem != null)
            {
                Cities c = city_list.City_LongListSelector.SelectedItem as Cities;
                if (c.City_Name == "Current Location")
                {
                    city_name.FontStyle = System.Windows.FontStyles.Italic;
                    GetLocation();
                }
                else
                {
                    city_name.FontStyle = System.Windows.FontStyles.Normal;
                    settings_extension.City_setting(c.City_Name);
                }
                city_name.Text = c.City_Name;
                city_list.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            PushNotification_toggle.IsChecked = settings_extension.PushNotification_setting_status();
            Calender_Toggle.IsChecked = settings_extension.CalenderEntries_setting_status();
            if (settings_extension.City_setting_status() != "")
                city_name.Text = settings_extension.City_setting_status();
            else
                city_name.Text = "Select a location";
        }
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if(city_list.Visibility== System.Windows.Visibility.Visible)
            {
                city_list.Visibility = System.Windows.Visibility.Collapsed;
                e.Cancel = true;
            }
        }

        private void LoadUserInfo()
        {
            var fb = new FacebookClient(App.AccessToken);

            fb.GetCompleted += (o, e) =>
            {
                if (e.Error != null)
                {
                    Dispatcher.BeginInvoke(() => MessageBox.Show(e.Error.Message));
                    return;
                }

                var result = (IDictionary<string, object>)e.GetResultData();

                Dispatcher.BeginInvoke(() =>
                {
                    var profilePictureUrl = string.Format("https://graph.facebook.com/{0}/picture?type={1}&access_token={2}", App.FacebookId, "square", App.AccessToken);

                    this.MyImage.Source = new BitmapImage(new Uri(profilePictureUrl));
                    this.MyName.Text = String.Format("{0} {1}", (string)result["first_name"], (string)result["last_name"]);
                });
            };

            fb.GetTaskAsync("me");
        }

        private void RateApp_Click(object sender, RoutedEventArgs e)
        {
            MarketplaceReviewTask RateTask = new MarketplaceReviewTask();
            RateTask.Show();
        }
        private async void GetLocation()
        {
            ReverseGeocodeQuery MyReverseGeocodeQuery = null;
            GeoCoordinate MyCoordinate = null;
            Dispatcher.BeginInvoke(() =>
            {
                location_progress.Visibility = System.Windows.Visibility.Visible;
            });
            try
            {
                var geoLocator = new Geolocator();
                Geoposition currentPosition = await geoLocator.GetGeopositionAsync();
                MyCoordinate = new GeoCoordinate(currentPosition.Coordinate.Latitude, currentPosition.Coordinate.Longitude);
 
                if (MyReverseGeocodeQuery == null || !MyReverseGeocodeQuery.IsBusy)
                {
                    MyReverseGeocodeQuery = new ReverseGeocodeQuery();
                    MyReverseGeocodeQuery.GeoCoordinate = new GeoCoordinate(MyCoordinate.Latitude, MyCoordinate.Longitude);
                    MyReverseGeocodeQuery.QueryCompleted += ReverseGeocodeQuery_QueryCompleted;
                    MyReverseGeocodeQuery.QueryAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Location service is not enabled!");
            }            
        }

        private void ReverseGeocodeQuery_QueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                location_progress.Visibility = System.Windows.Visibility.Collapsed;
            });
            if (e.Error == null)
            {
                if (e.Result.Count > 0)
                {
                    bool matchFound = false;
                    foreach (Cities c in city_list.cities)
                    {
                        if (e.Result[0].Information.Address.City.ToLower() == c.City_Name.ToLower())
                        {
                            city_name.Text = c.City_Name;
                            city_name.FontStyle = System.Windows.FontStyles.Normal;
                            matchFound = true;
                            settings_extension.City_setting(c.City_Name);
                        }
                    }
                    if(!matchFound)
                    {
                        MessageBox.Show("Your location is not supported yet!");                        
                    }
                }
            }
            else
            {
                MessageBox.Show(e.Error.Message);
            }
        }
        private void city_selection_button_Click(object sender, RoutedEventArgs e)
        {
            city_list.Visibility = System.Windows.Visibility.Visible;
        }

        /// <summary>
        /// Push Notification Enabled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PushNotification_toggle_Checked(object sender, RoutedEventArgs e)
        {
            settings_extension.PushNotification_setting(true);
            App.InitNotifications();
        }

        /// <summary>
        /// Push Notification Disabled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PushNotification_toggle_Unchecked(object sender, RoutedEventArgs e)
        {
            settings_extension.PushNotification_setting(false);
            var channel = HttpNotificationChannel.Find(App.PushChannel);
            if (channel != null)
            {
                channel.Close();
            }
        }

        private void Calender_Toggle_Checked(object sender, RoutedEventArgs e)
        {
            settings_extension.CalenderEntries_setting(true);
        }

        private void Calender_Toggle_Unchecked(object sender, RoutedEventArgs e)
        {
            settings_extension.CalenderEntries_setting(false);
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            App.MobileService.Logout();
            LoadUserInfo();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/About.xaml",UriKind.Relative));
        }
    }

    /// <summary>
    /// A library to make any access, modify Apps settings
    /// </summary>
    abstract class settings_extension
    {
        private static IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings ;
        public static void Save_setting(string key, string value)
        {
            if (settings.Contains(key))
            {
                settings[key] = value;                
            }
            else
            {
                settings.Add(key, value);
            }
            settings.Save();
        }
        public static string get_value(string key)
        {
            if (settings.Contains(key))
            {
                return settings[key].ToString();
            }
            return "";
        }

        public static bool PushNotification_setting_status()
        {
            if (settings.Contains("PushNotification"))
            {
                if (settings["PushNotification"].ToString().ToLower() == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            //Default Value
            else
            {
                settings.Add("PushNotification", "true");
                settings.Save();
                return true;
            }
        }
        public static void PushNotification_setting(bool value)
        {
            if (!settings.Contains("PushNotification"))
            {
                settings.Add("PushNotification", "false");
            }
            settings["PushNotification"] = value.ToString();
            settings.Save();
        }

        public static bool CalenderEntries_setting_status()
        {
            if (settings.Contains("CalenderEntries"))
            {
                if (settings["CalenderEntries"].ToString().ToLower() == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            //Default Value
            else
            {
                settings.Add("CalenderEntries", "true");
                settings.Save();
                return true;
            }
        }
        public static void CalenderEntries_setting(bool value)
        {
            if (!settings.Contains("CalenderEntries"))
            {
                settings.Add("CalenderEntries", "false");
            }
            settings["CalenderEntries"] = value.ToString();
            settings.Save();
        }
        public static string City_setting_status()
        {
            if (settings.Contains("City"))
            {                
                return settings["City"].ToString();                
            }
            //Default Value
            else
            {
                settings.Add("City", "");
                settings.Save();
                return "";
            }
        }
        public static void City_setting(string value)
        {
            if (!settings.Contains("City"))
            {
                settings.Add("City", "");
            }
            settings["City"] = value;
            settings.Save();
        }
    }
}