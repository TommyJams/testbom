using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Maps.Toolkit;
using Microsoft.Phone.Maps.Controls;
using System.Device.Location;
using Newtonsoft.Json;
using System.Text;
using TommyJams.ViewModel;

namespace TommyJams.View
{
    public partial class EventPanoramaPage : PhoneApplicationPage
    {
        public EventPanoramaPage()
        {
            InitializeComponent();
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
                    
                    List<EventItem> json = JsonConvert.DeserializeObject<List<EventItem>>(result) as List<EventItem>;
                    StringBuilder productsString = new StringBuilder();
                    foreach (EventItem aProduct in json)
                    {
                    productsString.AppendFormat("{0}", aProduct.EventName);

                        break;
                    }
                    mainHeader.Header = productsString.ToString();
                    
                    //TextBlock.Text = json;
                }
                catch (Exception ex)
                {
                    mainHeader.Header = "Exception Thrown"; 
                }


            }
        }

    }
}