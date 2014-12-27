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

        private void DoneIconButton_Click(object sender, EventArgs e)
        {
            App.FBViewModel.ClearSelectedFriends();
            var selectedFriends = this.friendList.SelectedItems;
            foreach (Friend friendItem in selectedFriends)
            {
                App.FBViewModel.AddSelectedFriends(friendItem);
            }
            App.ViewModel.DoneSelectedFriends();

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