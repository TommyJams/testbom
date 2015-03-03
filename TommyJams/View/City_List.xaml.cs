using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;

namespace TommyJams.View
{
    public partial class City_List : UserControl
    {
        public ObservableCollection<Cities> cities = new ObservableCollection<Cities>();
        public City_List()
        {
            InitializeComponent();
            cities.Add(new Cities("Current Location"));
            cities.Add(new Cities("Bangalore"));
            cities.Add(new Cities("Chennai"));
            cities.Add(new Cities("Delhi"));
            cities.Add(new Cities("Hyderabad"));
            cities.Add(new Cities("Kolkata"));

            City_LongListSelector.ItemsSource = cities;
        }

    }
    public class Cities
    {
        Visibility isCurrentLocation = Visibility.Collapsed;
        public Visibility IsCurrentLocation
        {
            get { return isCurrentLocation; }
            set { isCurrentLocation = value; }
        }
        public Visibility IsNotCurrentLocation
        {
            get
            {
                if (isCurrentLocation == Visibility.Collapsed)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }
        string city_name = "";
        public string City_Name
        {
            get {
                return city_name;
            }
            set
            {
                city_name = value;
            }
        }

        public Cities(string city)
        {
            city_name = city;
            if (city_name == "Current Location")
                isCurrentLocation = Visibility.Visible;
        }
    }
}
