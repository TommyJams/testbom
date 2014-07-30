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
using TommyJams.ViewModel;
using Facebook;
using System.Windows.Media.Imaging;

namespace TommyJams.View
{
    public partial class PanoramaPage2 : PhoneApplicationPage
    {
        public PanoramaPage2()
        {
            InitializeComponent();
            DataContext = App.ViewModel;
            LoadUserInfo();
            friendSelectorTextBlockHandler();
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



        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                //App.ViewModel.LoadData();
            }
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            //entItem selector = sender as EventItem;
            //if (selector == null)
            //    return;
            //EventItem data = selector.SelectedItem as EventItem;

            //if (data == null)
            //    return;

            Button selector = sender as Button;
            EventItem data = selector.DataContext as EventItem;
            //AudioPlayer.Source = new Uri(data.SongLink, UriKind.RelativeOrAbsolute);
            //AudioPlayer.Play();
            //selector.SelectedItem = null;
        }


        private void friendSelectorTextBlockHandler()
        {
            FacebookClient fb = new FacebookClient(App.AccessToken);

            fb.GetCompleted += (o, e) =>
            {
                if (e.Error != null)
                {
                    Dispatcher.BeginInvoke(() => MessageBox.Show(e.Error.Message));
                    return;
                }

                var result = (IDictionary<string, object>)e.GetResultData();

                var data = (IEnumerable<object>)result["data"]; ;


                Dispatcher.BeginInvoke(() =>
                {
                    // The observable collection can only be updated from within the UI thread. See 
                    // http://10rem.net/blog/2012/01/10/threading-considerations-for-binding-and-change-notification-in-silverlight-5
                    // If you try to update the bound data structure from a different thread, you are going to get a cross
                    // thread exception.
                    foreach (var item in data)
                    {
                        var friend = (IDictionary<string, object>)item;

                        FacebookData.Friends.Add(new Friend { Name = (string)friend["name"], id = (string)friend["id"], PictureUri = new Uri(string.Format("https://graph.facebook.com/{0}/picture?type={1}&access_token={2}", (string)friend["id"], "square", App.AccessToken)) });
                    }

                    NavigationService.Navigate(new Uri("/Pages/FriendSelector.xaml", UriKind.Relative));
                });

            };

            fb.GetTaskAsync("/me/friends");
        }

    }
}