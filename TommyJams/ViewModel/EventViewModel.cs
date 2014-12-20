using Facebook;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using TommyJams.Model;
using TommyJams.Resources;


namespace TommyJams.ViewModel
{
    public class EventViewModel : INotifyPropertyChanged
    {
        public EventViewModel()
        {
            /*this.Priority1Items = new ObservableCollection<EventItem>();
            this.Priority2Items = new ObservableCollection<EventItem>();
            this.EventItem = new EventItem();
            this.ArtistInfo = new ObservableCollection<ArtistInfo>();
            this.VenueInfo = new VenueInfo();
            this.Model = new Models();*/
        }

        //public ObservableCollection<EventItem> Priority1Items { get; set; }

        private ObservableCollection<EventItem> _priority1Items = new ObservableCollection<EventItem>();
        public ObservableCollection<EventItem> Priority1Items 
        {
            get
            {
                return _priority1Items;
            }

            set
            {
                if (value != _priority1Items)
                {
                    _priority1Items = value;
                    NotifyPropertyChanged("Priority1Items");
                }
            }
        }

        private ObservableCollection<EventItem> _priority2Items = new ObservableCollection<EventItem>();
        public ObservableCollection<EventItem> Priority2Items 
        {
            get
            {
                return _priority2Items;
            }
            set
            {
                if (value != _priority2Items)
                {
                    _priority2Items = value;
                    NotifyPropertyChanged("Priority2Items");
                }
            }
        }

        private ObservableCollection<EventItem> _upcomingEvents = new ObservableCollection<EventItem>();
        public ObservableCollection<EventItem> UpcomingEvents
        {
            get
            {
                return _upcomingEvents;
            }
            set
            {
                if (value != _upcomingEvents)
                {
                    _upcomingEvents = value;
                    NotifyPropertyChanged("UpcomingEvents");
                }
            }
        }

        private ObservableCollection<NotificationItem> _notificationItems = new ObservableCollection<NotificationItem>();
        public ObservableCollection<NotificationItem> NotificationItems
        {
            get
            {
                return _notificationItems;
            }
            set
            {
                if (value != _notificationItems)
                {
                    _notificationItems = value;
                    NotifyPropertyChanged("NotificationItems");
                }
            }
        }

        private EventItem _eventItem = new EventItem();
        public EventItem EventItem 
        {
            get
            {
                return _eventItem;
            }
            set
            {
                if (value != _eventItem)
                {
                    _eventItem = value;
                    NotifyPropertyChanged("EventItem");
                }
            }
        }

        private ObservableCollection<ArtistInfo> _artistInfo = new ObservableCollection<ArtistInfo>();
        public ObservableCollection<ArtistInfo> ArtistInfo
        {
            get
            {
                return _artistInfo;
            }
            set
            {
                if (value != _artistInfo)
                {
                    _artistInfo = value;
                    NotifyPropertyChanged("ArtistInfo");
                }
            }
        }
        private VenueInfo _venueInfo = new VenueInfo();
        public VenueInfo VenueInfo 
        {
            get
            {
                return _venueInfo;
            }
            set
            {
                if (value != _venueInfo)
                {
                    _venueInfo = value;
                    NotifyPropertyChanged("VenueInfo");
                }
            }
        }
        private AppModel _appmodel = new AppModel();
        public AppModel AppModel
        {
            get
            {
                return _appmodel;
            }
            set
            {
                if (value != _appmodel)
                {
                    _appmodel = value;
                    NotifyPropertyChanged("AppModel");
                }
            }
        }


        
        public bool IsDataLoaded
        {
            get;
            set;
        }

        public async Task LoginToFacebook()
        {
            if (App.MobileService.CurrentUser == null)
            {
                App.fbSession = await App.FacebookSession.LoginAsync("email, user_birthday");
                var client = new FacebookClient(App.fbSession.AccessToken);
                var fbToken = JObject.FromObject(new
                {
                    access_token = App.fbSession.AccessToken,
                });

                await App.MobileService.LoginAsync(MobileServiceAuthenticationProvider.Facebook, fbToken);

                if (App.MobileService.CurrentUser != null)
                {
                    App.FacebookId = App.MobileService.CurrentUser.UserId.Substring(9); //Get facebook id number
                    App.fbUser = new User(); 
                    App.fbUser.fbid = App.FacebookId;
                    if((await AppModel.GetUserPresence())== 0)
                    {
                        var fb = new FacebookClient(App.fbSession.AccessToken);
                        dynamic result = await fb.GetTaskAsync("me", new { fields = new[] { "name, gender, location, email, birthday" } });

                        try
                        {
                            App.fbUser.name = (string)result["name"];
                            App.fbUser.gender = (string)result["gender"];
                            //TODO: Add required checks here
                            App.fbUser.city = ((string)result["location"]["name"]).Split(',')[0].Trim();
                            App.fbUser.country = ((string)result["location"]["name"]).Split(',')[1].Trim();
                            App.fbUser.email = (string)result["email"];
                            App.fbUser.dob = (string)result["birthday"];
                            //TODO: add current ip
                            App.fbUser.ip = "0.0.0.0";
                        }
                        catch (Exception)
                        {
                            //There might be empty keys
                        }

                        string responseAddUser = await AddUser(App.fbUser);
                    }
                }
                else
                {
                    App.FacebookId = App.FACEBOOK_DEFAULT_ID;
                }
            }
            else
            {
                //Already logged in
            }
        }

        public void LogoutFromFacebook()
        {
            if (App.MobileService.CurrentUser != null)
            {
                App.MobileService.Logout();
                if (App.MobileService.CurrentUser == null)
                {
                    App.FacebookId = App.FACEBOOK_DEFAULT_ID;
                    App.fbUser = null;
                }
            }
            else
            {
                //Already logged out
            }
        }

        public async Task<string> AddUser(User u)
        {
            return await AppModel.PushAddUser(u);
        }

        public async Task<string> JoinEvent()
        {
            return await AppModel.PushJoinEvent(EventItem.EventID);
        }

        public async Task LoadPrimaryEvents()
        {
            Priority1Items = await AppModel.GetPrimaryEvents();
        }

        public async Task LoadSecondaryEvents()
        {
            Priority2Items = await AppModel.GetSecondaryEvents();
        }

        public async Task LoadNotifications()
        {
            UpcomingEvents = await AppModel.GetUpcomingEvents();
            NotificationItems = await AppModel.GetInvitations();
        }

        public async Task<EventItem> LoadEventInfo()
        {
            var eventInfo = await AppModel.GetEventInfo();
            return eventInfo;
        }

        public async Task<ObservableCollection<ArtistInfo>> LoadArtistInfo()
        {
            var artistInfo = await AppModel.GetArtistInfo();
            return artistInfo;
        }

        public async Task<VenueInfo> LoadVenueInfo()
        {
            var venueInfo = await AppModel.GetVenueInfo();
            return venueInfo;
            
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

        
    }
}