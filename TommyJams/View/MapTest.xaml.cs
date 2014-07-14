﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Maps;
using Microsoft.Phone.Net.NetworkInformation;
using Windows.Devices.Geolocation;
using System.Device.Location;
using Microsoft.Phone.Maps.Services;
using Microsoft.Phone.Maps.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Json; 

namespace TommyJams.View
{
    public partial class MapTest : PhoneApplicationPage
    {
        IsolatedStorageFile Settings = IsolatedStorageFile.GetUserStoreForApplication(); 
        LocationList LocationListobj = new LocationList(); 
        const int MIN_ZOOM_LEVEL = 2; 
        const int MAX_ZOOM_LEVEL = 20; 
        const int MIN_ZOOMLEVEL_FOR_LANDMARKS = 16; 
        public List<GeoCoordinate> MyCoordinates = new List<GeoCoordinate>(); 
        public MapTest() 
        { 
            InitializeComponent(); 
 
            this.Loaded += MapView_Loaded; 
        } 
 
        private void MapView_Loaded(object sender, RoutedEventArgs e) 
        { 
            BuildLocalizedApplicationBar(); 
            LoadAvalableParks(); 
        } 
        private void BuildLocalizedApplicationBar() 
        { 
            ApplicationBar = new ApplicationBar(); 
            ApplicationBar.Opacity = 0.5; 
            ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/zoomin.png", UriKind.Relative)); 
            appBarButton.Text = "ZoomIn"; 
            appBarButton.Click += ZoomIn; 
            ApplicationBar.Buttons.Add(appBarButton); 
            // Zoom Out button. 
            appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/zoomout.png", UriKind.Relative)); 
            appBarButton.Text = "ZoomOut"; 
            appBarButton.Click += ZoomOut; 
            ApplicationBar.Buttons.Add(appBarButton); 
            ApplicationBar.IsVisible = true; 
 
        } 
        void ZoomIn(object sender, EventArgs e) 
        { 
            if (MapVieMode.ZoomLevel < MAX_ZOOM_LEVEL) 
            { 
                MapVieMode.ZoomLevel++; 
            } 
        } 
 
        void ZoomOut(object sender, EventArgs e) 
        { 
            if (MapVieMode.ZoomLevel >= MIN_ZOOM_LEVEL) 
            { 
                MapVieMode.ZoomLevel--; 
            } 
        } 
        
        private void LoadAvalableParks() 
        { 
            try 
            { 
                /************Add diff locations to list**************/ 
                LocationListobj.Add(new LocationDetail { id = "1", D_name = "West Bank Office Building Ramp", Lat = "44.976288", Long = "-93.248582", Distance = "2" }); 
                LocationListobj.Add(new LocationDetail { id = "2", D_name = "19th Avenue Ramp", Lat = "44.970541", Long = "-93.246075", Distance = "3" }); 
                LocationListobj.Add(new LocationDetail { id = "4", D_name = "Washington Avenue Ramp", Lat = "44.973919", Long = "-93.231372", Distance = "5" }); 
                LocationListobj.Add(new LocationDetail { id = "5", D_name = "University Avenue Ramp", Lat = "44.976140", Long = "-93.228981", Distance = "6" }); 
                //LocationListobj.Add(new LocationDetail { id = "3", D_name = "Clemson House Lot", Lat = "34.681158", Long = "-82.834368", Distance = "4" }); 
                //LocationListobj.Add(new LocationDetail { id = "6", D_name = "West Bank Office Building Ramp", Lat = "44.976288", Long = "-93.248582", Distance = "7" }); 
                /************Add diff locations to Mapview**************/ 
                 
                MapVieMode.Layers.Clear(); 
                MapLayer mapLayer = new MapLayer(); 
                MyCoordinates.Clear(); 
                for (int i = 0; i < LocationListobj.Count; i++) 
                { 
                    MyCoordinates.Add(new GeoCoordinate { Latitude = double.Parse("" + LocationListobj[i].Lat), Longitude = double.Parse("" + LocationListobj[i].Long) }); 
 
                } 
                DrawMapMarkers(); 
                // Thickness ss=new Thickness(10,0,0,10); 
 
                // LocationRectangle boundingRectangle = new LocationRectangle( ); 
                MapVieMode.Center = MyCoordinates[MyCoordinates.Count - 1]; 
                //  MapVieMode.ZoomLevel = 14; 
                Dispatcher.BeginInvoke(() => 
                { 
                    MapVieMode.SetView(LocationRectangle.CreateBoundingRectangle(MyCoordinates)); 
                }); 
                // MapVieMode.SetView(LocationRectangle.CreateBoundingRectangle(from 1 in MyCoordinates); 
                MapVieMode.SetView(MyCoordinates[MyCoordinates.Count - 1], 10, MapAnimationKind.Linear); 
            } 
            catch 
            { 
            } 
 
        } 
        
 
        private void Map_Loaded(object sender, RoutedEventArgs e) 
        { 
            MapsSettings.ApplicationContext.ApplicationId = "<applicationid>"; 
            MapsSettings.ApplicationContext.AuthenticationToken = "<authenticationtoken>"; 
        } 
         
       private void DrawMapMarkers() 
        { 
            //MapVieMode.Layers.Clear(); 
            MapLayer mapLayer = new MapLayer(); 
            // Draw marker for current position        
 
            // Draw markers for location(s) / destination(s) 
            for (int i = 0; i < MyCoordinates.Count; i++) 
            { 
 
                //DrawMapMarker(MyCoordinates[i], Colors.Red, mapLayer, parklist.parking_details[i].DestinationName); 
                /*UCCustomToolTip _tooltip = new UCCustomToolTip(); 
               _tooltip.Description = LocationListobj[i].D_name.ToString() + "\n" + LocationListobj[i].Distance; 
                _tooltip.DataContext = LocationListobj[i]; 
                 _tooltip.Menuitem.Click += Menuitem_Click; 
                _tooltip.imgmarker.Tap += _tooltip_Tapimg;*/ 
                MapOverlay overlay = new MapOverlay(); 
                //overlay.Content = _tooltip; 
                overlay.GeoCoordinate = MyCoordinates[i]; 
                overlay.PositionOrigin = new Point(0.0, 1.0); 
                mapLayer.Add(overlay); 
            } 
            MapVieMode.Layers.Add(mapLayer); 
        } 
 
        private void Menuitem_Click(object sender, RoutedEventArgs e) 
        { 
            try 
            { 
                MenuItem item = (MenuItem)sender; 
                string selecteditem = item.Tag.ToString(); 
                var selectedparkdata = LocationListobj.Where(s => s.id == selecteditem).ToList(); 
                if (selectedparkdata.Count > 0) 
                { 
                    foreach (var items in selectedparkdata) 
                    { 
                         
                        if (Settings.FileExists("LocationDetailItem")) 
                        { 
                            Settings.DeleteFile("LocationDetailItem"); 
                        } 
                        using (IsolatedStorageFileStream fileStream = Settings.OpenFile("LocationDetailItem", FileMode.Create)) 
                        { 
                            DataContractSerializer serializer = new DataContractSerializer(typeof(LocationDetail)); 
                            serializer.WriteObject(fileStream, items); 
 
                        } 
                        NavigationService.Navigate(new Uri("/MapViewDetailsPage.xaml", UriKind.Relative)); 
                        break; 
                    } 
                } 
            } 
            catch 
            { 
            } 
        } 
 
        private void _tooltip_Tapimg(object sender, System.Windows.Input.GestureEventArgs e) 
        { 
            try 
            { 
                Image item = (Image)sender; 
                string selecteditem = item.Tag.ToString(); 
                var selectedparkdata = LocationListobj.Where(s => s.id == selecteditem).ToList(); 
 
 
                if (selectedparkdata.Count > 0) 
                { 
                    foreach (var items in selectedparkdata) 
                    { 
                        ContextMenu contextMenu = 
                    ContextMenuService.GetContextMenu(item); 
                        contextMenu.DataContext = items; 
                        if (contextMenu.Parent == null) 
                        { 
                            contextMenu.IsOpen = true; 
 
                        } 
                        break; 
                    } 
                } 
            } 
            catch 
            { 
            } 
        } 
        
 
    } 
    public class LocationDetail 
    { 
        public string id { get; set; } 
        public string D_name { get; set; } 
        public string Lat { get; set; } 
        public string Long { get; set; } 
        public string _number;  
        public string Distance 
        { 
            get 
            { 
                return this._number; 
            } 
            set 
            { 
                if (value != null) 
                { 
                     
                    this._number = value + " " + "miles"; 
                } 
                else 
                { 
                    this._number = value; 
                } 
            } 
        } 
         
    } 
 
    public class LocationList : List<LocationDetail> 
    { 
    } 
}