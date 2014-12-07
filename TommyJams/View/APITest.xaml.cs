using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.MobileServices;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Text;



namespace TommyJams.View
{

    public class User
    {
        [DataMember(Name = "name")]
        public String name { get; set; }
        [DataMember(Name = "education")]
        public String education { get; set; }
        [DataMember(Name = "birtday")]
        public String birthday { get; set; }
    }

    public class FbID
    {
        public String fbid { get; set; }
    }

    public partial class APITest : PhoneApplicationPage
    {
        //public static MobileServiceClient client = new MobileServiceClient("https://testneo4j.azure-mobile.net", "");
        public APITest()
        {
            InitializeComponent();
            
        }


        public void MakeSomeHTTPCall(object sender, RoutedEventArgs e)
        {
            WebClient wc = new WebClient();
            
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            wc.DownloadStringAsync(new System.Uri("https://testneo4j.azure-mobile.net/api/getUserPresence?fbid=11111111111111111"));

            /*var request = (HttpWebRequest)WebRequest.Create("https://testneo4j.azure-mobile.net/api/api/getUserPresence?fbid=11111111111111111");
         request.Method = "GET";
         request.BeginGetResponse(new AsyncCallback(GetSomeResponse), request);*/
        }

        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                try
                {
                    String result = "{ \"name\" : \"1\",\"birthday\" : \"sfdhd\"}";
                    //User json = JsonConvert.DeserializeObject<User>(result) as User;
                    //StringBuilder productsString = new StringBuilder();
                    /*foreach (User aProduct in json)
                    {
                        productsString.AppendFormat("Name: {0}",aProduct.nname);
                        break;
                    }
                    */
                    /*String final="";
                    for (int i = 0; i < e.Result.Length; i++)
                    {
                        if (e.Result[i] == '.')
                        { }
                        else
                        {
                            final = final + e.Result[i];
                        }

                    }*/

                    User json = JsonConvert.DeserializeObject<User>(result) as User;
                    StringBuilder productsString = new StringBuilder();
                    //foreach (User aProduct in json)
                    //{
                        productsString.AppendFormat("Name: {0}",json.name);
                        productsString.AppendFormat("Birthday: {0}", json.birthday);
                        
                    //    break;
                    //}
                    TextBlock.Text = productsString.ToString() ;
                    
                    //TextBlock.Text = json;
                }
                catch (Exception ex)
                {
                    TextBlock.Text = "Exception: " + ex.Message;
                }
                
                
            }
        }



        private void GetSomeResponse(IAsyncResult MyResultAsync)
      {
          HttpWebRequest request = (HttpWebRequest)MyResultAsync.AsyncState;
          try
            {
              HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(MyResultAsync);
              if (response.StatusCode == HttpStatusCode.OK && response.ContentLength > 0)
              {
                  using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                  {
                      // This result string below is going to be whatever I get back,
                      //   be it JSON or XML or whatever
                      string result = sr.ReadToEnd();                        
                  }
              }
          }
          catch (WebException e)
          {
              if (e.Status == WebExceptionStatus.RequestCanceled)
                  MessageBox.Show("Looks like your request was interrupted by tombstoning");
              else
              {
                  using (HttpWebResponse response = (HttpWebResponse)e.Response)
                  {
                      MessageBox.Show("I got an http error of: " + response.StatusCode.ToString());
                  }
              }
          }
      }



        public async void testAPI(object sender, RoutedEventArgs e)
        {
            try
            {
                var client = new MobileServiceClient("https://testneo4j.azure-mobile.net/", "8MXv+0OPLj2Tg0cj6W8X5pOqWB2RdkTybv8kCc3AqG8P4VuIc0xjMfzV9TRDO1AdPggfIykoZ++x50H+B3XJrg==");
                //HttpWebRequest webRequest = (HttpWebRequest)asynchronousResult.AsyncState;
                //HttpWebResponse response;
                //String URI = "https://testneo4j.azure-mobile.net/api/getUserPresence?fbid=11111111111111111";
                
                //String parameters = "";
                //String result = wc.UploadString(URI, parameters);

                var test = new FbID { fbid = "56784957689798" };
                var result = await client.InvokeApiAsync<FbID, User>("getuserpresence", test);
                TextBlock.Text = result.name;
            }
            catch (Exception ex)
            {
                TextBlock.Text = ex.ToString();
            }


        }
        

    }
}