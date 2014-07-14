using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Microsoft.Phone.Maps.Services;
using System.Net.Http;
using System.Xml.Linq;
using System.Globalization;
using Windows.Devices.Geolocation;
using System.Device.Location;
namespace TommyJams.Controller
{
    public partial class SelectCity : PhoneApplicationPage
    {
        public SelectCity()
        {
            InitializeComponent();
        }

        private async void OneShotLocation_Click(object sender, RoutedEventArgs e)
        {
            Geolocator geolocator = new Geolocator();
            //geolocator.DesiredAccuracyInMeters = 50;
            ReverseGeocodeQuery geoQ;
            try
            {
                //String city = "";
                Geoposition geoposition = await geolocator.GetGeopositionAsync(
                    maximumAge: TimeSpan.FromMinutes(5),
                    timeout: TimeSpan.FromSeconds(10)
                    );


                geoQ = new ReverseGeocodeQuery();
                //LongitudeTextBlock.Text = City;
                //geoQ.GeoCoordinate.Latitude = geoposition.Coordinate.Latitude;
                geoQ.GeoCoordinate = new GeoCoordinate(geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude);
                geoQ.QueryCompleted += reverseGeocode_QueryCompleted;
                geoQ.QueryAsync();
                //add = geoposition.CivicAddress;
                /*while (true)
                { city = geoposition.CivicAddress.Country;
                if (city != "")
                { break; }
                }
                LatitudeTextBlock.Text = city;*/
                //LatitudeTextBlock.Text = geoQ.GeoCoordinate.;
            }
            catch (Exception ex)
            {
                if ((uint)ex.HResult == 0x80004004)
                {
                    // the application does not have the right capability or the location master switch is off
                    StatusTextBlock.Text = "location  is disabled in phone settings.";
                }
                //else
                {
                    // something else happened acquring the location
                }
            }
        }

        public static GeoCoordinate ConvertGeocoordinate(Geocoordinate geocoordinate)
        {
            return new GeoCoordinate
                (
                geocoordinate.Latitude,
                geocoordinate.Longitude,
                geocoordinate.Altitude ?? Double.NaN,
                geocoordinate.Accuracy,
                geocoordinate.AltitudeAccuracy ?? Double.NaN,
                geocoordinate.Speed ?? Double.NaN,
                geocoordinate.Heading ?? Double.NaN
                );
        }


        void reverseGeocode_QueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
        {
            String addressString1, addressString2, addressString3;
            try
            {
                if (e.Cancelled)
                {
                    LatitudeTextBlock.Text = "operation was cancelled";
                }
                else if (e.Error != null)
                {
                    LatitudeTextBlock.Text = "Error: " + e.Error.Message;
                }
                else if (e.Result != null)
                {
                    if (e.Result.Count > 0)
                    {
                        MapAddress geoAddress = e.Result[0].Information.Address;
                        addressString1 = geoAddress.HouseNumber + " " + geoAddress.Street;
                        addressString2 = geoAddress.District + ", " + geoAddress.City;
                        addressString3 = geoAddress.Country;
                        if (addressString1 != " ")
                            addressString1 = addressString1 + "\n";
                        else
                            addressString1 = "";

                        if (addressString2 != ",  ")
                            addressString2 = addressString2 + "\n";
                        else
                            addressString2 = "";

                        LatitudeTextBlock.Text = addressString1 + addressString2 + addressString3;
                    }
                    else
                    {
                        LatitudeTextBlock.Text = "no address found at that location";
                    }
                }
            }
            catch
            {
                MessageBox.Show("Some error occured in converting location geo Coordinates to location address, please try again later");
            }
        }
    }
}