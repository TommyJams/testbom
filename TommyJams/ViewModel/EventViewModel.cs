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
using System.Threading;
using System.Threading.Tasks;
using TommyJams.Model;
using TommyJams.Resources;
using Windows.Security.Credentials;


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

        private NotificationItem _notificationItem = new NotificationItem();
        public NotificationItem NotificationItem
        {
            get
            {
                return _notificationItem;
            }
            set
            {
                if (value != _notificationItem)
                {
                    _notificationItem = value;
                    NotifyPropertyChanged("NotificationItem");
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

        private ObservableCollection<OtherUser> _socialInfo = new ObservableCollection<OtherUser>();
        public ObservableCollection<OtherUser> SocialInfo
        {
            get
            {
                return _socialInfo;
            }
            set
            {
                if (value != _socialInfo)
                {
                    _socialInfo = value;
                    NotifyPropertyChanged("SocialInfo");
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
            // Use the PasswordVault to securely store and access credentials.
            PasswordVault vault = new PasswordVault();
            PasswordCredential credential = null;
            PasswordCredential fbcredential = null;

            while (credential == null)
            {
                try
                {
                    // Try to get an existing credential from the vault.
                    credential = vault.FindAllByResource("Facebook").FirstOrDefault();
                    fbcredential = vault.FindAllByResource("FacebookAccessToken").FirstOrDefault();
                }
                catch (Exception)
                {
                    // When there is no matching resource an error occurs, which we ignore.
                }

                if (credential != null)
                {
                    // Create a user from the stored credentials.
                    App.MobileService.CurrentUser = new MobileServiceUser(credential.UserName);
                    credential.RetrievePassword();
                    App.MobileService.CurrentUser.MobileServiceAuthenticationToken = credential.Password;

                    try
                    {
                        //Check fb access token expiry
                        fbcredential.RetrievePassword();
                        var client = new FacebookClient(fbcredential.Password);
                        dynamic result = client.GetTaskAsync("me/friends");
                        App.fbSession = new Facebook.Client.FacebookSession();
                        App.FacebookId = App.fbSession.FacebookId = fbcredential.UserName;
                        App.fbSession.AccessToken = fbcredential.Password;
                    }
                    catch (FacebookOAuthException)
                    {
                        // Remove the credential with the expired token.
                        vault.Remove(credential);
                        vault.Remove(fbcredential);
                        credential = null;
                        fbcredential = null;
                        continue;
                    }

                    try
                    {
                        // Try to return an item now to determine if the cached credential has expired.
                        await App.MobileService.GetTable<testEvent>().Take(1).ToListAsync();
                    }
                    catch (MobileServiceInvalidOperationException ex)
                    {
                        // Remove the credential with the expired token.
                        vault.Remove(credential);
                        vault.Remove(fbcredential);
                        credential = null;
                        fbcredential = null;
                        continue;
                    }
                }
                else
                {
                    App.fbSession = await App.FacebookSession.LoginAsync("email, user_birthday");
                    var client = new FacebookClient(App.fbSession.AccessToken);
                    var fbToken = JObject.FromObject(new
                    {
                        access_token = App.fbSession.AccessToken,
                    });

                    // Login with the identity provider.
                    await App.MobileService.LoginAsync(MobileServiceAuthenticationProvider.Facebook, fbToken);

                    if (App.MobileService.CurrentUser != null)
                    {
                        // Create and store the user credentials.
                        credential = new PasswordCredential("Facebook", App.MobileService.CurrentUser.UserId, App.MobileService.CurrentUser.MobileServiceAuthenticationToken);
                        fbcredential = new PasswordCredential("FacebookAccessToken", App.fbSession.FacebookId, App.fbSession.AccessToken);
                        vault.Add(credential);
                        vault.Add(fbcredential);

                        App.FacebookId = App.MobileService.CurrentUser.UserId.Substring(9); //Get facebook id number
                        App.fbUser = new User();
                        App.fbUser.fbid = App.FacebookId;
                        if ((await AppModel.GetUserPresence()) == 0)
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
            }
        }

        public void LogoutFromFacebook()
        {
            PasswordVault vault = new PasswordVault();
            PasswordCredential credential = null;
            PasswordCredential fbcredential = null;

            try
            {
                // Try to get an existing credential from the vault.
                credential = vault.FindAllByResource("Facebook").FirstOrDefault();
                fbcredential = vault.FindAllByResource("FacebookAccessToken").FirstOrDefault();
                vault.Remove(credential);
                vault.Remove(fbcredential);
                credential = null;
                fbcredential = null;
            }
            catch (Exception)
            {
                // When there is no matching resource an error occurs, which we ignore.
            }
            
            if (App.MobileService.CurrentUser != null)
            {
                App.FacebookSession.Logout();
                App.MobileService.Logout();
                if (App.MobileService.CurrentUser == null)
                {
                    App.FacebookId = App.FACEBOOK_DEFAULT_ID;
                    App.fbUser = null;
                    App.fbSession = null;
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
            return await AppModel.PushJoinEvent();
        }

        public async Task LoadPrimaryEvents(CancellationToken ct)
        {
            Priority1Items = await AppModel.GetPrimaryEvents(ct);
        }

        public async Task LoadSecondaryEvents(CancellationToken ct)
        {
            Priority2Items = await AppModel.GetSecondaryEvents(ct);
        }

        public async Task LoadNotifications()
        {
            UpcomingEvents = await AppModel.GetUpcomingEvents();
            NotificationItems = await AppModel.GetInvitations();
        }

        public async Task<NotificationItem> LoadEventInfo()
        {
            NotificationItem nItem = new NotificationItem();
            
            //TODO: HUGE HACK!!! REMOVE!!!
            EventItem eventInfo = await AppModel.GetEventInfo();
            nItem.EventDate = eventInfo.EventDate;
            nItem.EventDistance = eventInfo.EventDistance;
            nItem.EventGenre = eventInfo.EventGenre;
            nItem.EventHotness = eventInfo.EventHotness;
            nItem.EventID = eventInfo.EventID;
            nItem.EventImage = eventInfo.EventImage;
            nItem.EventName = eventInfo.EventName;
            nItem.EventPrice = eventInfo.EventPrice;
            nItem.EventSong = eventInfo.EventSong;
            nItem.EventStartingTime = eventInfo.EventStartingTime;
            nItem.EventTags = eventInfo.EventTags;
            nItem.EventTime = eventInfo.EventTime;
            nItem.TheTime = eventInfo.TheTime;
            nItem.VenueAddress = eventInfo.VenueAddress;
            nItem.VenueCity = eventInfo.VenueCity;
            nItem.VenueCoordinates = eventInfo.VenueCoordinates;
            nItem.VenueName = eventInfo.VenueName;
            nItem.UserAttending = eventInfo.UserAttending;
            nItem.InviteeFBID = NotificationItem.InviteeFBID;
            nItem.InviteeImage = NotificationItem.InviteeImage;
            nItem.InviteExists = NotificationItem.InviteExists;
            nItem.InviteeName = NotificationItem.InviteeName;
            return nItem;
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

        public async Task<ObservableCollection<OtherUser>> LoadSocialInfo()
        {
            var socialinfo = await AppModel.GetSocialInfo();
            return socialinfo;
        }

        public async Task DoneSelectedFriends()
        {
            await AppModel.PushInviteFriends();
        }

        public async Task<VenueInfo_Foursquare> LoadVenueInfo_Foursquare(String VenueID)
        {
            var venueInfo_fs = await AppModel.GetVenueInfo_Foursquare(VenueID);
            return venueInfo_fs;
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