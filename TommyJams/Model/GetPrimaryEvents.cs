using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TommyJams.ViewModel;
using TommyJams;
using System.Net;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.Globalization;

namespace TommyJams.Model
{
    class GetPrimaryEvents
    {
        EventViewModel viewModel = App.viewModel;
        WebClient wc = new WebClient();

        public GetPrimaryEvents()
        {
            
        }

        public void LoadData()
        {

            String defaultUri = "https://testneo4j.azure-mobile.net/api/getPrimaryEvents?";
            String completeUri = defaultUri + "fbid=" + App.FacebookId + "&city=" + App.city + "&country=" + App.country;
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
                    //this.Priority1Items = json;
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

                        /*String[] location = aProduct.EventCoordinates.Split(' ');
                        String a = "9.12";
                        Double s = Convert.ToDouble(a);
                        aProduct.EventDistance = (int) CalcDistance(App.myGeocoordinate.Latitude,App.myGeocoordinate.Longitude,Convert.ToDouble(location[0]),Convert.ToDouble(location[1]));
                        */
                        //break;
                    }

                }
                catch (Exception ex)
                {
                    int a = 0;
                    //gigsHeader.Header = "Exception Thrown";
                }


            }
        }
    }
}
