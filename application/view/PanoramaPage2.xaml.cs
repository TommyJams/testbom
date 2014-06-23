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

namespace TommyJams.View
{
    public partial class PanoramaPage2 : PhoneApplicationPage
    {
        public PanoramaPage2()
        {
            InitializeComponent();
            DataContext = App.ViewModel;
        }
        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
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
            AudioPlayer.Source = new Uri(data.SongLink, UriKind.RelativeOrAbsolute);
            //AudioPlayer.Play();
            //selector.SelectedItem = null;
        }
    }
}