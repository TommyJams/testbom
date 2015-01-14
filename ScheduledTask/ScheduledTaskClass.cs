using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Devices.Geolocation;
using Windows.Devices.Geolocation.Geofencing;

namespace ScheduledTask
{
    public sealed class ScheduledTaskClass : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            // call API here to download the list n subscribe to geofence
            Init_BackgroundGeofence();
            CreateGeofence("sample", 28.6437958, 77.0704003, 10000);
        }

        public static void CreateGeofence(string id, double lat, double lon, double radius)
        {
            foreach (Geofence g in GeofenceMonitor.Current.Geofences)
            {
                if (g.Id == id)
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
                Name = "GeofenceTask",
                TaskEntryPoint = "GeofenceTask.Task"
            };

            var trigger = new LocationTrigger(LocationTriggerType.Geofence);
            geofenceTaskBuilder.SetTrigger(trigger);
            var geofenceTask = geofenceTaskBuilder.Register();
        }
    }
}
