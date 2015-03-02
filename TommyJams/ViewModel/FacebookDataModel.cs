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
        private ObservableCollection<OtherUser> friends = new ObservableCollection<OtherUser>();

        public ObservableCollection<OtherUser> Friends
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

        private ObservableCollection<OtherUser> selectedFriends = new ObservableCollection<OtherUser>();

        public ObservableCollection<OtherUser> SelectedFriends
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

            Friends = JsonConvert.DeserializeObject<ObservableCollection<OtherUser>>(fbDataStr) as ObservableCollection<OtherUser>;
            foreach(OtherUser friendItem in Friends)
            {
                friendItem.pictureUri = "http://graph.facebook.com/" + friendItem.id + "/picture?width=100&height=100";
                friendItem.friend = "Facebook";
            }
        }

        public void ClearSelectedFriends()
        {
            SelectedFriends.Clear();
        }

        public void AddSelectedFriends(OtherUser friendItem)
        {
            SelectedFriends.Add(friendItem);
        }

    }
}
