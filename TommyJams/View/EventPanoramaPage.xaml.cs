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
using System.Net.Http.Headers;
using System.Net.Http;
using System.IO;
using TommyJams.Resources;

namespace TommyJams.View
{
    class Product
    {
        public String fbid { get; set; }
        public String eventID { get; set; }
    }

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

        private void Event_Accept(object sender, EventArgs e)
        {
            this.AcceptInvite();

        }

        public async void AcceptInvite()
        {

            Uri myUri = new Uri("https://testneo4j.azure-mobile.net/api/joinEvent");
            HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(myUri);
            myRequest.Method = "POST" ;
            myRequest.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), myRequest);
}

        void GetRequestStreamCallback(IAsyncResult callbackResult)
        {
            HttpWebRequest myRequest = (HttpWebRequest)callbackResult.AsyncState;

            // End the stream request operation
            Stream postStream = myRequest.EndGetRequestStream(callbackResult);

            // Create the post data
            string postData = "{\"fbid\":\"56784957689798\",\"eventID\":\"5\"}";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            // Add the post data to the web request
            postStream.Write(byteArray, 0, byteArray.Length);
            postStream.Close();

            // Start the web request
            myRequest.BeginGetResponse(new AsyncCallback(GetResponsetStreamCallback), myRequest);
        }

        void GetResponsetStreamCallback(IAsyncResult callbackResult)
        {
            //lib = new ApiLibrary();

            try
            {
                HttpWebRequest request = (HttpWebRequest)callbackResult.AsyncState;
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(callbackResult);
                string result = "";
                using (StreamReader httpWebStreamReader = new StreamReader(response.GetResponseStream()))
                {
                    result = httpWebStreamReader.ReadToEnd();
                }

                string APIResult = result;
                description.Text = "asdasda";
                String a = "";
            }
            catch (Exception e)
            {

            }
        }       


    }
}