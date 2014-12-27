using Facebook;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TommyJams.Model;

namespace TommyJams.ViewModel
{
    public class FacebookData : INotifyPropertyChanged
    {
        private ObservableCollection<Friend> friends = new ObservableCollection<Friend>();

        public ObservableCollection<Friend> Friends
        {
            get
            {
                return friends;
            }

            set
            {
                if (value != friends)
                {
                    friends = value;
                    NotifyPropertyChanged("Friends");
                }
            }
        }

        private ObservableCollection<Friend> selectedFriends = new ObservableCollection<Friend>();

        public ObservableCollection<Friend> SelectedFriends
        {
            get
            {
                return selectedFriends;
            }

            set
            {
                if (value != selectedFriends)
                {
                    selectedFriends = value;
                    NotifyPropertyChanged("SelectedFriends");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public async Task LoadFacebookFriends()
        {
            var fb = new FacebookClient(App.fbSession.AccessToken);
            var result = (IDictionary<string, object>)await fb.GetTaskAsync("me/friends");
            string fbDataStr = result["data"].ToString();

            Friends = JsonConvert.DeserializeObject<ObservableCollection<Friend>>(fbDataStr) as ObservableCollection<Friend>;
            foreach(Friend friendItem in Friends)
            {
                friendItem.pictureUri = "http://graph.facebook.com/" + friendItem.id + "/picture?type=square";
            }
        }

        public void ClearSelectedFriends()
        {
            SelectedFriends.Clear();
        }

        public void AddSelectedFriends(Friend friendItem)
        {
            SelectedFriends.Add(friendItem);
        }

    }
}
