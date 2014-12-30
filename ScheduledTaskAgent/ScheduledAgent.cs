using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Scheduler;
using Windows.Devices.Geolocation.Geofencing;
using Windows.Devices.Geolocation;
using System;
using Windows.ApplicationModel.Background;

namespace ScheduledTaskAgent1
{

    /// <summary>
    /// This ScheduledTask will download a list of new Events everyday and mark them for geofencing
    /// </summary>
    public class ScheduledAgent1 : ScheduledTaskAgent 
    {
        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        static ScheduledAgent1()
        {
            // Subscribe to the managed exception handler
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                Application.Current.UnhandledException += UnhandledException;
            });
        }

        /// Code to execute on Unhandled Exceptions
        private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        protected override void OnInvoke(ScheduledTask task)
        {
            //TODO: Add code to perform your task in background

            // call API here to download the list n subscribe to geofence
            CreateGeofence("sample", 28.6437958, 77.0704003, 10000);
            NotifyComplete();
        }

        /// <summary>
        /// Creates a Geofence, return if already created with same id
        /// </summary>
        /// <param name="id">String type identifier</param>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <param name="radius">Radius In meters</param>
        public static void CreateGeofence(string id, double lat, double lon, double radius)
        {
            foreach (Geofence g in  GeofenceMonitor.Current.Geofences)
            {
                if(g.Id==id)
                {
                    return;
                }
            }

            var position = new BasicGeoposition();
            position.Latitude = lat;
            position.Longitude = lon;

            //Radius in meters
            var geocircle = new Geocircle(position, radius); 

            MonitoredGeofenceStates mask = 0;
            mask |= MonitoredGeofenceStates.Entered;

            // Create Geofence with the supplied id, geocircle and mask, not for single use
            // and with a dwell time of 0 seconds and single use only
            var geofence = new Geofence(id, geocircle, mask, true, new TimeSpan(0, 0, 0));
            GeofenceMonitor.Current.Geofences.Add(geofence);
        }
        private void Init_BackgroundGeofence()
        {
            var backgroundAccessStatus =
                BackgroundExecutionManager.RequestAccessAsync();
            var geofenceTaskBuilder = new BackgroundTaskBuilder
            {
                Name = "GeofenceBackgroundTask",
                TaskEntryPoint = "BackgroundTask.GeofenceBackgroundTask"
            };

            var trigger = new LocationTrigger(LocationTriggerType.Geofence);
            geofenceTaskBuilder.SetTrigger(trigger);
            var geofenceTask = geofenceTaskBuilder.Register();
            
        }
    }
}