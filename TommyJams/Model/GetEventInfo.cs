using Microsoft.Phone.Maps.Toolkit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TommyJams;
using TommyJams.ViewModel;
using TommyJams.View;

namespace TommyJams.Model
{
    class GetEventInfo
    {
        public Boolean dataLoaded = false;

        public GetEventInfo()
        {

        }

        public void LoadData()
        {
            WebClient wc = new WebClient();
            String defaultUri = "https://testneo4j.azure-mobile.net/api/getEventInfo?";
            String completeUri = defaultUri + "eventID=" + App.EventID;
            
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            wc.DownloadStringAsync(new System.Uri(completeUri));


        }

        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                try
                {
                    String result = e.Result;
                    CivicAddressResolver resolver = new CivicAddressResolver();
                    List<EventItem> json = JsonConvert.DeserializeObject<List<EventItem>>(result) as List<EventItem>;
                    StringBuilder productsString = new StringBuilder();
                    foreach (EventItem aProduct in json)
                    {
                        App.viewModel.eventItem = aProduct;
                        productsString.AppendFormat("{0}", aProduct.EventName);
                        GeoCoordinate a = new GeoCoordinate();
                        String[] Location = (aProduct.VenueCoordinates.Split(' '));
                        a.Latitude = 32.16;
                        a.Longitude = -117.71;
                        //a.Latitude = Convert.ToDouble(Location[0]);
                        //a.Longitude = Convert.ToDouble(Location[1]);
                        a.Altitude = 0;
                        a.Course = 0;
                        a.HorizontalAccuracy = 0;
                        a.VerticalAccuracy = 0;
                        a.Speed = 0;
                    }
                    //mainHeader.Header = productsString.ToString();
                    //TextBlock.Text = json;
                    dataLoaded = true;
                }
                catch (Exception ex)
                {
                    //mainHeader.Header = "Exception Thrown";
                }


            }
        }


    }
}
