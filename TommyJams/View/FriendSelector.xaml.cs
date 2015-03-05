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

namespace TommyJams.View
{
    public partial class FriendSelector : PhoneApplicationPage
    {
        public FriendSelector()
        {
            InitializeComponent();
            this.DataContext = App.FBViewModel;
        }

        private async void DoneIconButton_Click(object sender, EventArgs e)
        {
            App.FBViewModel.ClearSelectedFriends();
            var selectedFriends = this.friendList.SelectedItems;
            foreach (OtherUser friendItem in selectedFriends)
            {
                App.FBViewModel.AddSelectedFriends(friendItem);
            }
            
            try
            {
                await App.ViewModel.DoneSelectedFriends();
                MessageBox.Show("Successfully sent notification to your friend!");
            }
            catch(Exception)
            {
                MessageBox.Show("Sorry, unable to send notification! Invite your friend to our app?");
            }
            
            NavigationService.GoBack();
        }

        /*protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // this runs in the UI thread, so it is ok to modify the 
            // viewmodel directly here
            FacebookData.SelectedFriends.Clear();
            var selectedFriends = this.friendList.SelectedItems;
            foreach (Friend oneFriend in selectedFriends)
            {
                FacebookData.SelectedFriends.Add(oneFriend);
            }

            base.OnNavigatedFrom(e);
        }*/

    }
}