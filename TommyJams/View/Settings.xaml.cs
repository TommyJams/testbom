﻿using System;
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
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Phone.Scheduler;

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
                    if (settings_extension.Location_setting_status())
                        GetLocation();
                    else
                        MessageBox.Show("TommyJams doesn't have the permission to access your location. Please enable 'Location' in app's 'settings' to perform this action.");
                }
                else
                {
                    city_name.FontStyle = System.Windows.FontStyles.Normal;
                    settings_extension.City_setting(c.City_Name);
                    App.city = c.City_Name;
                }
                city_name.Text = c.City_Name;
                city_list.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            PushNotification_toggle.IsChecked = settings_extension.PushNotification_setting_status();
            PushNotification_toggle.Content = settings_extension.PushNotification_setting_status() ? "On" : "Off";
            Location_Toggle.IsChecked = settings_extension.Location_setting_status();
            Location_Toggle.Content = settings_extension.Location_setting_status() ? "On" : "Off";
            Calender_Toggle.IsChecked = settings_extension.CalenderEntries_setting_status();
            Calender_Toggle.Content = settings_extension.CalenderEntries_setting_status() ? "On" : "Off";

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

        private async void LoadUserInfo()
        {
            /*var fb = new FacebookClient(App.AccessToken);

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

            fb.GetTaskAsync("me");*/
            if (App.MobileService.CurrentUser != null && App.FacebookId != null)
            {
                BitmapImage bm = new BitmapImage();
                bm.CreateOptions = BitmapCreateOptions.BackgroundCreation;
                bm.UriSource = new Uri("http://graph.facebook.com/" + App.FacebookId + "/picture?width=100&height=100", UriKind.Absolute);
                bm.DownloadProgress+=bm_DownloadProgress;
                Dispatcher.BeginInvoke(() =>
                 { 
                     user_profile_text.Visibility = Visibility.Visible;
                     user_profile.Visibility = Visibility.Visible;
                     user_profile_logout.Visibility = Visibility.Visible;
                     fb_data_progressbar.Visibility = System.Windows.Visibility.Visible;
                 });
                try
                {
                    await get_fbname();
                }
                catch(WebExceptionWrapper) { }
            }
            else 
            {
                Dispatcher.BeginInvoke(() =>
                 {
                     user_profile_text.Visibility = Visibility.Collapsed;
                     user_profile.Visibility = Visibility.Collapsed;
                     user_profile_logout.Visibility = Visibility.Collapsed;
                     fb_data_progressbar.Visibility = System.Windows.Visibility.Collapsed;
                 });
            }
        }

        private void bm_DownloadProgress(object sender, DownloadProgressEventArgs e)
        {
            if (e.Progress == 100)
            {
                Dispatcher.BeginInvoke(() =>
                 {
                     this.MyImage.Source = sender as BitmapImage;
                 });
            }
        }

        private async Task get_fbname()
        {
            var fb = new Facebook.FacebookClient(App.fbSession.AccessToken);
            dynamic result = await fb.GetTaskAsync("me");
            var currentUser = new Facebook.Client.GraphUser(result);
            Dispatcher.BeginInvoke(() => 
            { 
                MyName.Text = currentUser.Name;
                username.Text = currentUser.UserName;
                fb_data_progressbar.Visibility = System.Windows.Visibility.Collapsed;
            });
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
                    string Result_city = e.Result[0].Information.Address.City.ToLower();
                    bool matchFound = false;
                    if (Result_city == "bengaluru")
                        Result_city = "bangalore";
                    foreach (Cities c in city_list.cities)
                    {
                        if (Result_city == c.City_Name.ToLower())
                        {
                            city_name.Text = c.City_Name;
                            city_name.FontStyle = System.Windows.FontStyles.Normal;
                            matchFound = true;
                            settings_extension.City_setting(c.City_Name);
                            App.city = c.City_Name;
                        }
                    }
                    if(!matchFound)
                    {
                        MessageBox.Show("Your location is not supported yet!");                        
                    }
                }
                else
                {
                    MessageBox.Show("Your location is not supported yet!");
                }
            }
            else
            {
                MessageBox.Show("Your location is not supported yet!");
                //MessageBox.Show(e.Error.Message);
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
            PushNotification_toggle.Content = "On";
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
            PushNotification_toggle.Content = "Off";
            var channel = HttpNotificationChannel.Find(App.PushChannel);
            if (channel != null)
            {
                channel.Close();
            }
        }

        private void Calender_Toggle_Checked(object sender, RoutedEventArgs e)
        {
            settings_extension.CalenderEntries_setting(true);
            Calender_Toggle.Content = "On";
        }

        private void Calender_Toggle_Unchecked(object sender, RoutedEventArgs e)
        {
            settings_extension.CalenderEntries_setting(false);
            Calender_Toggle.Content= "Off";
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            App.ViewModel.LogoutFromFacebook();
            LoadUserInfo();
            var channel = HttpNotificationChannel.Find(App.PushChannel);
            if (channel != null)
            {
                channel.Close();
            }
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/About.xaml",UriKind.Relative));
        }

        private void Location_Toggle_Checked(object sender, RoutedEventArgs e)
        {
            settings_extension.Location_setting(true);
            Location_Toggle.Content = "On";
        }

        private void Location_Toggle_Unchecked(object sender, RoutedEventArgs e)
        {
            settings_extension.Location_setting(false);
            Location_Toggle.Content = "Off";
        }
    }

    /// <summary>
    /// A library to make any access, modify Apps settings
    /// </summary>
    abstract class settings_extension
    {
        private static IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        private static void Save_setting(string key, string value)
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
        private static string get_value(string key)
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
        public static bool Location_setting_status()
        {
            if (settings.Contains("LocationSetting"))
            {
                if (settings["LocationSetting"].ToString().ToLower() == "true")
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
                settings.Add("LocationSetting", "true");
                settings.Save();
                return true;
            }
        }
        public static void Location_setting(bool value)
        {
            if (!settings.Contains("LocationSetting"))
            {
                settings.Add("LocationSetting", "false");
            }
            settings["LocationSetting"] = value.ToString();
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
            //if (string.Compare(value.ToString() ,"false", StringComparison.InvariantCultureIgnoreCase)==0)
            //    disableAllReminders();
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
        public async static void GetLocation()
        {
            ReverseGeocodeQuery MyReverseGeocodeQuery = null;
            GeoCoordinate MyCoordinate = null;
            
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
            }
        }

        private static void ReverseGeocodeQuery_QueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
        {
            ObservableCollection<Cities> cities = new ObservableCollection<Cities>();

            cities.Add(new Cities("Bangalore"));
            cities.Add(new Cities("Chennai"));
            cities.Add(new Cities("Delhi"));
            cities.Add(new Cities("Hyderabad"));
            cities.Add(new Cities("Kolkata"));
            if (e.Error == null)
            {
                if (e.Result.Count > 0)
                {
                    string Result_city = e.Result[0].Information.Address.City.ToLower();
                    bool matchFound = false;
                    if (Result_city == "bengaluru")
                        Result_city = "bangalore";
                    foreach (Cities c in cities)
                    {
                        if (Result_city == c.City_Name.ToLower())
                        {
                            App.city = c.City_Name;
                            City_setting(c.City_Name);
                            matchFound = true;
                        }
                    }
                    if (!matchFound)
                    {
                        //default
                        App.city = "Bangalore";
                    }
                }
            }
            else
            {
                //default
                App.city = "Bangalore";
            }
        }
        #region obsolete Code
        //public static List<Reminder> _Reminders;
        //public static List<Reminder> Reminders
        //{
        //    get
        //    {
        //        if (_Reminders == null)                 
        //            _Reminders =  GetReminder();
        //        return _Reminders;
        //    }
        //    set
        //    {
        //        _Reminders = value;
        //    }
        //}
        //private static string saveToPath = "Reminders.xml";
        //private static List<Reminder> GetReminder()
        //{
        //    List<Reminder> reminders = new List<Reminder>();
        //    using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
        //    {
        //        if (myIsolatedStorage.FileExists(saveToPath))
        //        {
        //            using (IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile(saveToPath, FileMode.Open, FileAccess.Read))
        //            {
                        
        //                XmlSerializer serializer = new XmlSerializer(typeof(List<Reminder>));
        //                reminders = (List<Reminder>) serializer.Deserialize(fileStream);
        //            }
        //        }
        //        else
        //        {
        //            _Reminders = reminders;
        //            saveReminders();
        //        }
        //    }

        //    return reminders;
        //}
        //public static void saveReminders()
        //{
        //    using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
        //    {
        //        using (IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile(saveToPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
        //        {
        //            XmlSerializer serializer = new XmlSerializer(typeof(List<Reminder>));
        //            serializer.Serialize(fileStream, _Reminders);
        //        }
        //    }
        //}
        //public static void disableAllReminders()
        //{
        //    foreach(Reminder r in Reminders)
        //    {
        //        try
        //        {
        //            ScheduledActionService.Remove(r.Name);
        //        }
        //        catch { }
        //    }
        //    Reminders = new List<Reminder>();
        //    saveReminders();
        //}
        #endregion
    }
    public class Reminder
    {
        public string Name;
        public Reminder() { Name = ""; }
        public Reminder(string ID) { Name = ID; }
    }
}