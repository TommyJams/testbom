﻿#pragma checksum "C:\Users\Kumar\Documents\Visual Studio 2012\Projects\TommyJams\TommyJams\View\Settings.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "3576261EB8AE72642A5D6B2C2C45FB33"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace TommyJams.View {
    
    
    public partial class Settings : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBlock Title;
        
        internal System.Windows.Controls.Image MyImage;
        
        internal System.Windows.Controls.TextBlock MyName;
        
        internal System.Windows.Controls.TextBlock LocationTitle;
        
        internal System.Windows.Controls.TextBlock CurrentLocation;
        
        internal Microsoft.Phone.Controls.ToggleSwitch toggle1;
        
        internal Microsoft.Phone.Controls.ToggleSwitch toggle2;
        
        internal System.Windows.Controls.Button ShareApp;
        
        internal System.Windows.Controls.Button Logout;
        
        internal System.Windows.Controls.Button About;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/TommyJams;component/View/Settings.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.Title = ((System.Windows.Controls.TextBlock)(this.FindName("Title")));
            this.MyImage = ((System.Windows.Controls.Image)(this.FindName("MyImage")));
            this.MyName = ((System.Windows.Controls.TextBlock)(this.FindName("MyName")));
            this.LocationTitle = ((System.Windows.Controls.TextBlock)(this.FindName("LocationTitle")));
            this.CurrentLocation = ((System.Windows.Controls.TextBlock)(this.FindName("CurrentLocation")));
            this.toggle1 = ((Microsoft.Phone.Controls.ToggleSwitch)(this.FindName("toggle1")));
            this.toggle2 = ((Microsoft.Phone.Controls.ToggleSwitch)(this.FindName("toggle2")));
            this.ShareApp = ((System.Windows.Controls.Button)(this.FindName("ShareApp")));
            this.Logout = ((System.Windows.Controls.Button)(this.FindName("Logout")));
            this.About = ((System.Windows.Controls.Button)(this.FindName("About")));
        }
    }
}
