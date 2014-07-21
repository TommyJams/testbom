﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TommyJams.ViewModel;
using TommyJams.View;
using TommyJams;
using System.Net;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.Globalization;
using System.Windows.Navigation;
using System.Threading;
using System.Device.Location;

namespace TommyJams.Model
{
    class GetPrimaryEvents
    {
        EventViewModel viewModel = App.viewModel;
        WebClient wc = new WebClient();
        AutoResetEvent waitHandle = new AutoResetEvent(false);
        TaskCompletionSource<String> tcs = new TaskCompletionSource<string>();
            
        public GetPrimaryEvents()
        {
            
        }

        public void LoadData()
        {
            String defaultUri = "https://testneo4j.azure-mobile.net/api/getPrimaryEvents?";
            String completeUri = defaultUri + "fbid=" + App.FacebookId + "&city=" + App.city + "&country=" + App.country;
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);

            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            

             wc.DownloadStringAsync(new System.Uri(completeUri));
            
        }

        private void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                try
                {
                    String result = e.Result;

                    this.viewModel.Priority1Items = JsonConvert.DeserializeObject<ObservableCollection<EventItem>>(result) as ObservableCollection<EventItem>;
                    
                    StringBuilder genreString = new StringBuilder();
                    foreach (EventItem aProduct in this.viewModel.Priority1Items)
                    {
                        genreString.Append("genre:");
                        foreach (String genre in aProduct.EventTags)
                        {
                            genreString.AppendFormat("{0} ", genre);
                        }
                        aProduct.EventGenre = genreString.ToString();
                        genreString.Clear();
                        DateTime eventDate = DateTime.ParseExact(aProduct.EventDate, "yyyyMMdd", CultureInfo.InvariantCulture);
                        DateTime currentDate = DateTime.Now;
                        if ((eventDate.Day == currentDate.Day) && (eventDate.Month == currentDate.Month))
                        {
                            DateTime eventTime = DateTime.ParseExact(aProduct.EventTime, "HHmm", CultureInfo.InvariantCulture);
                            var diff = eventTime.Hour - currentDate.Hour;
                            aProduct.EventStartingTime = "in " + diff + " hours";
                        }
                        else
                        {
                            aProduct.EventStartingTime = eventDate.ToString();
                        }

                        String[] location = aProduct.VenueCoordinates.Split(' ');
                        
                        Double latitude = Convert.ToDouble(location[0]);
                        Double longitude = Convert.ToDouble(location[1]);
                        var eventGeo = new GeoCoordinate(latitude, longitude);
                        var myGeo = new GeoCoordinate(72.2,84.3);
                        double x = eventGeo.GetDistanceTo(myGeo);
                        aProduct.EventDistance = (int) (x/1000);
                        int a = 0;
                        
                    }
                    //App.ViewModel.IsDataLoaded = true;
                    //tcs.SetResult(e.Result);
                    
                }
                catch (Exception ex)
                {

                }


            }
        }
    }
}