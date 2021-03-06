﻿using System;
using System.Diagnostics;
using System.Resources;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TommyJams.Resources;
using Microsoft.WindowsAzure.MobileServices;
using Facebook.Client;
using TommyJams.Model;
using TommyJams.View;
using TommyJams.ViewModel;
using Windows.Devices.Geolocation;
using Microsoft.Phone.Notification;
using System.Text;
using Microsoft.WindowsAzure.Messaging;
using System.Collections.Generic;
using System.Windows.Media;
using Windows.Data.Xml.Dom;
using Windows.ApplicationModel.Background;
using Coding4Fun.Toolkit.Controls;
using System.Windows.Media.Imaging;

namespace TommyJams
{
    public partial class App : Application
    {
        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        internal const string FACEBOOK_DEFAULT_ID = "100004896491644";
        internal const string MAP_APPLICATION_ID = "72f88dc6-44dd-40b4-9e13-24cef4fe5ea9";
        internal const string MAP_AUTHENTICATION_TOKEN = "BOVJwqXv9dbCEX-Fo7uyYg";

        public static PhoneApplicationFrame RootFrame { get; private set; }
        public static MobileServiceUser user;
        public static FacebookSession fbSession;
        //public static MainViewModel viewModel = null;
        public static EventViewModel viewModel= null;
        public static bool isAuthenticated = false;
        public static string EventID = "5";
        public static string PushChannel = "TommyJamsPushChannel";
        public static string NotificationHubPath = "testneo4jhub";
        public static string ConnectionString = "Endpoint=sb://testneo4jhub-ns.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=0E6XY/X2R9CBq9+5RmOTJBaEh+1Kuc4mYFHjJkvTtWc=";
        internal static string AccessToken = "CAACEdEose0cBANAiEbyjsoZAQRA4bwlmrcURKZBDMKZCgHk6FUE8kYabMZBT4eHxnuCKX2IOvEA5ZBEaUz1rN82zjdyYkBG4HyYe6eJbbxPa6bZB9N6KUyWzwYbvS65TXjZBRIJo49V9ZCFQ4RKvapdsqYxFh27CWbvNPoIEuFJkedxCRoqYNnGcTfZC7T5XnQKALWuI2bbgnFNSWSqWjnMuY";
        internal static string FacebookId = FACEBOOK_DEFAULT_ID;
        internal static string FacebookName;
        public static User fbUser;

        public delegate void cityChangedhandler();
        public static event cityChangedhandler cityChanged;
        internal static string _city = "Bangalore";
        internal static string city
        {
            get { return _city;}
            set 
            {
                if (value != _city)
                {
                    _city = value;
                    if (cityChanged != null)
                        cityChanged();
                }
            }
        }
        internal static string country = "India";
        public static Geolocator geoLocator;
        public static Geoposition myPosition;
        public static Geocoordinate myGeocoordinate;
        public static FacebookData friends= null;
        public static EventViewModel ViewModel
        {
            get
            {
                // Delay creation of the view model until necessary
                if (viewModel == null)
                {
                    viewModel = new EventViewModel();
                }

                return viewModel;
            }
        }

        public static FacebookData FBViewModel
        {
            get
            {
                // Delay creation of the view model until necessary
                if (friends == null)
                {
                    friends = new FacebookData();
                }

                return friends;
            }
        }
        private bool resetApp = false;
        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions.
            UnhandledException += Application_UnhandledException;

            //override light theme with dark
            if ((Visibility)Resources["PhoneLightThemeVisibility"] == Visibility.Visible)
            {
                DarkTheme();
            }

            // Standard XAML initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Language display initialization
            InitializeLanguage();

            // Show graphics profiling information while debugging.
            if (Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                //Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode,
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Prevent the screen from turning off while under the debugger by disabling
                // the application's idle detection.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

        }

        private void DarkTheme()
        {
            ((SolidColorBrush)Resources["PhoneRadioCheckBoxCheckBrush"]).Color = ((SolidColorBrush)Resources["PhoneRadioCheckBoxBorderBrush"]).Color = ((SolidColorBrush)Resources["PhoneForegroundBrush"]).Color = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
            ((SolidColorBrush)Resources["PhoneBackgroundBrush"]).Color = Color.FromArgb(0xFF, 0x00, 0x00, 0x00);
            ((SolidColorBrush)Resources["PhoneContrastForegroundBrush"]).Color = Color.FromArgb(0xFF, 0x00, 0x00, 0x00);
            ((SolidColorBrush)Resources["PhoneContrastBackgroundBrush"]).Color = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
            ((SolidColorBrush)Resources["PhoneDisabledBrush"]).Color = Color.FromArgb(0x66, 0xFF, 0xFF, 0xFF);
            ((SolidColorBrush)Resources["PhoneProgressBarBackgroundBrush"]).Color = Color.FromArgb(0x19, 0xFF, 0xFF, 0xFF);
            ((SolidColorBrush)Resources["PhoneTextCaretBrush"]).Color = Color.FromArgb(0xFF, 0x00, 0x00, 0x00);
            ((SolidColorBrush)Resources["PhoneTextBoxBrush"]).Color = Color.FromArgb(0xBF, 0xFF, 0xFF, 0xFF);
            ((SolidColorBrush)Resources["PhoneTextBoxForegroundBrush"]).Color = Color.FromArgb(0xFF, 0x00, 0x00, 0x00);
            ((SolidColorBrush)Resources["PhoneTextBoxEditBackgroundBrush"]).Color = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
            ((SolidColorBrush)Resources["PhoneTextBoxReadOnlyBrush"]).Color = Color.FromArgb(0x77, 0x00, 0x00, 0x00);
            ((SolidColorBrush)Resources["PhoneSubtleBrush"]).Color = Color.FromArgb(0x99, 0xFF, 0xFF, 0xFF);
            ((SolidColorBrush)Resources["PhoneTextBoxSelectionForegroundBrush"]).Color = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
            ((SolidColorBrush)Resources["PhoneButtonBasePressedForegroundBrush"]).Color = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
            ((SolidColorBrush)Resources["PhoneTextHighContrastBrush"]).Color = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
            ((SolidColorBrush)Resources["PhoneTextMidContrastBrush"]).Color = Color.FromArgb(0x99, 0xFF, 0xFF, 0xFF);
            ((SolidColorBrush)Resources["PhoneTextLowContrastBrush"]).Color = Color.FromArgb(0x73, 0xFF, 0xFF, 0xFF);
            ((SolidColorBrush)Resources["PhoneSemitransparentBrush"]).Color = Color.FromArgb(0xAA, 0x00, 0x00, 0x00);
            ((SolidColorBrush)Resources["PhoneChromeBrush"]).Color = Color.FromArgb(0xFF, 0x1F, 0x1F, 0x1F);

            ((SolidColorBrush)Resources["PhoneInactiveBrush"]).Color = Color.FromArgb(0x33, 0xFF, 0xFF, 0xFF);
            ((SolidColorBrush)Resources["PhoneInverseInactiveBrush"]).Color = Color.FromArgb(0xFF, 0xCC, 0xCC, 0xCC);
            ((SolidColorBrush)Resources["PhoneInverseBackgroundBrush"]).Color = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
            ((SolidColorBrush)Resources["PhoneBorderBrush"]).Color = Color.FromArgb(0xBF, 0xFF, 0xFF, 0xFF);
        }

        private async void UpdateLocation()
        {
            if (settings_extension.Location_setting_status())
            {
                try
                {
                    myPosition = await geoLocator.GetGeopositionAsync();
                    myGeocoordinate = myPosition.Coordinate;
                }
                catch (Exception ex) { }
            }
            
        }

        public static FacebookSessionClient FacebookSession = new FacebookSessionClient("526508354136712");

        public static MobileServiceClient MobileService = new MobileServiceClient(
            "https://testneo4j.azure-mobile.net/",
            "LxqAhRVmuUESACkYahJxCgfjOomzEP99"
            );

        //public static PushNotificationChannel channel { get; private set; }

        /// <summary>
        /// Request a channel for push notification Between MPNS and the device
        /// then register with Mobile Service with the URI from MPNS
        /// </summary>
        public static void InitNotifications()
        {
            // Request a push notification channel if Push Notification settings are enabled
            if (settings_extension.PushNotification_setting_status() == true)
            {
                var channel = HttpNotificationChannel.Find(PushChannel);
                if (channel == null)
                {
                    channel = new HttpNotificationChannel(PushChannel);
                    channel.Open();
                    channel.BindToShellToast();
                    channel.BindToShellTile();
                    channel.ShellToastNotificationReceived += CurrentChannel_ShellToastNotificationReceived;
                }

                channel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(async (o, args) =>
                {
                    //Register to Mobile Service 
                    try
                    {
                        //List<string> tags = new List<string>();
                        if (MobileService.CurrentUser != null)
                        {
                            //tags.Add(MobileService.CurrentUser.UserId);
                            await MobileService.GetPush()
                            .RegisterNativeAsync(args.ChannelUri.ToString());
                        }
                    }
                    catch { }
                });
            }
        }

        static string relativeUri = string.Empty; 
        /// <summary>
        /// Handles the Toast Push Notification when app is in Foreground
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void CurrentChannel_ShellToastNotificationReceived(object sender, NotificationEventArgs e)
        {
            
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                ToastPrompt toast = new ToastPrompt();
                toast.ImageSource = new BitmapImage(new Uri("/Resources/Image/Toast_tj_icon.png", UriKind.RelativeOrAbsolute));
                if(e.Collection.Keys.Contains("wp:Text1"))
                {
                    toast.Title = e.Collection["wp:Text1"] ;
                }
                if(e.Collection.Keys.Contains("wp:Text2"))
                {
                    toast.Message = e.Collection["wp:Text2"];
                }
                 if(e.Collection.Keys.Contains("wp:Param"))
                    relativeUri = e.Collection["wp:Param"];

                 toast.Tap += toast_Tap;
                // Display a dialog of all the fields in the toast.                
                toast.Show();               
            });
        }

        static void toast_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if(!string.IsNullOrEmpty(relativeUri))
            {
                try{
                    Uri uri = new Uri(relativeUri, UriKind.Relative);
                    (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(uri);
                }
                catch{
                    return;
                }
            }
        }
        
        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            //InitNotifications();
            //Register_ScheduledTask();

            //check for saved city setting
            string c = settings_extension.City_setting_status();
            //no city saved
            if(c=="")
            {
                //if no city saved then get the current location and save if found in supported city
                if(settings_extension.Location_setting_status())
                    settings_extension.GetLocation();
            }
            else
            {
                city = c;
            }
        }
        

        public string taskName = "ScheduledTask";
        private async void Register_ScheduledTask()
        {
            //needs to be async because of the BackgroundExecutionManager
            try
            {
        // calling the BackgroundExecutionManager
        //this performs the message prompt to the user that allows the permissions entry
                var backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();
 
        //checking if we have access to set up our live tile
        if (backgroundAccessStatus == BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity ||
                    backgroundAccessStatus == BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity)
                {
            //unregistering our old task, if there is one                  
            foreach (var task in BackgroundTaskRegistration.AllTasks)
                    {
                        if (task.Value.Name == taskName)
                        {
                            task.Value.Unregister(true);
                        }
                    }
            //building up our new task and registering it
                    BackgroundTaskBuilder taskBuilder = new BackgroundTaskBuilder();
                    taskBuilder.Name = taskName;
                    taskBuilder.TaskEntryPoint = typeof(ScheduledTask.ScheduledTaskClass).FullName;
                    taskBuilder.SetTrigger(new TimeTrigger(60*24, false));
                    taskBuilder.AddCondition(new SystemCondition(SystemConditionType.InternetAvailable));
                    var registration = taskBuilder.Register();
                }
            }
        //catching all exceptions that can happen
            catch (Exception ex)
            {
        //async method used, but wil be marked by VS to be executed synchronously
                Debug.WriteLine("Couldn't register Background ScheduledTask");                
            }
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new TransitionFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Handle reset requests for clearing the backstack
            RootFrame.Navigated += CheckForResetNavigation;
            
            // Used to cancel navigation on reset
            RootFrame.Navigating += RootFrame_Navigating;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }
        private void RootFrame_Navigating(object sender, NavigatingCancelEventArgs e)
      {
          if (resetApp && e.IsCancelable && e.Uri.OriginalString == "/MainPage.xaml")
          {
              e.Cancel = true;
              resetApp = false;
          }
    }
        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        private void CheckForResetNavigation(object sender, NavigationEventArgs e)
        {
            resetApp = e.NavigationMode == NavigationMode.Reset;
            // If the app has received a 'reset' navigation, then we need to check
            // on the next navigation to see if the page stack should be reset
            //if (e.NavigationMode == NavigationMode.Reset)
            //    RootFrame.Navigated += ClearBackStackAfterReset;
        }

        private void ClearBackStackAfterReset(object sender, NavigationEventArgs e)
        {
            // Unregister the event so it doesn't get called again
            RootFrame.Navigated -= ClearBackStackAfterReset;

            // Only clear the stack for 'new' (forward) and 'refresh' navigations
            if (e.NavigationMode != NavigationMode.New && e.NavigationMode != NavigationMode.Refresh)
                return;

            // For UI consistency, clear the entire page stack
            while (RootFrame.RemoveBackEntry() != null)
            {
                ; // do nothing
            }
        }

        #endregion

        // Initialize the app's font and flow direction as defined in its localized resource strings.
        //
        // To ensure that the font of your application is aligned with its supported languages and that the
        // FlowDirection for each of those languages follows its traditional direction, ResourceLanguage
        // and ResourceFlowDirection should be initialized in each resx file to match these values with that
        // file's culture. For example:
        //
        // AppResources.es-ES.resx
        //    ResourceLanguage's value should be "es-ES"
        //    ResourceFlowDirection's value should be "LeftToRight"
        //
        // AppResources.ar-SA.resx
        //     ResourceLanguage's value should be "ar-SA"
        //     ResourceFlowDirection's value should be "RightToLeft"
        //
        // For more info on localizing Windows Phone apps see http://go.microsoft.com/fwlink/?LinkId=262072.
        //
        private void InitializeLanguage()
        {
            try
            {
                // Set the font to match the display language defined by the
                // ResourceLanguage resource string for each supported language.
                //
                // Fall back to the font of the neutral language if the Display
                // language of the phone is not supported.
                //
                // If a compiler error is hit then ResourceLanguage is missing from
                // the resource file.
                RootFrame.Language = XmlLanguage.GetLanguage(AppResources.ResourceLanguage);

                // Set the FlowDirection of all elements under the root frame based
                // on the ResourceFlowDirection resource string for each
                // supported language.
                //
                // If a compiler error is hit then ResourceFlowDirection is missing from
                // the resource file.
                FlowDirection flow = (FlowDirection)Enum.Parse(typeof(FlowDirection), AppResources.ResourceFlowDirection);
                RootFrame.FlowDirection = flow;
            }
            catch
            {
                // If an exception is caught here it is most likely due to either
                // ResourceLangauge not being correctly set to a supported language
                // code or ResourceFlowDirection is set to a value other than LeftToRight
                // or RightToLeft.

                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }

                throw;
            }
        }

    }
}