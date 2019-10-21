using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CoreLocation;
using Foundation;
using mARkIt.Models;
using mARkIt.Notifications;
using mARkIt.Services;
using UIKit;

namespace mARkIt.iOS.Notifications
{
    /// <remarks>
    ///     - Since iOS is very restrictive of most continues long-running background tasks,
    ///       this implementation uses the iOS-approved location-updates task to keep polling the backend consistently.
    /// </remarks>
    public sealed class IOSMarksScanner : MarksScanner
    {
        private static IOSMarksScanner s_Instance = null;
        private static object s_LockObj = new Object();

        private CLLocationManager locMng;
        private DateTime lastScanTime;

        public static IOSMarksScanner Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    lock (s_LockObj)
                    {
                        if (s_Instance == null)
                        {
                            s_Instance = new IOSMarksScanner();
                        }
                    }
                }

                return s_Instance;
            }
        }

        private IOSMarksScanner()
        {
            locMng = new CLLocationManager();
            locMng.AllowsBackgroundLocationUpdates = true;
            locMng.PausesLocationUpdatesAutomatically = false;
            locMng.DesiredAccuracy = 1;
            locMng.Failed += locMgr_OnFailor;
            locMng.LocationsUpdated += locMgr_OnLocationsUpdated;
        }

        public override void StartScanning()
        {
            if (CLLocationManager.LocationServicesEnabled)
            {
                lastScanTime = DateTime.Now;

                locMng.StartUpdatingLocation();
            }
        }

        public override void StopScanning()
        {
            locMng.StopUpdatingLocation();
        }

        private async void locMgr_OnLocationsUpdated(object sender, CLLocationsUpdatedEventArgs e)
        {
            CLLocation lastKnownPosition = e.Locations[e.Locations.Length - 1];
            TimeSpan timeSinceLastScan = DateTime.Now - lastScanTime;

            if (timeSinceLastScan.Minutes >= 1)
            {
                lastScanTime = DateTime.Now;

                /// base.CheckForClosestNewMark(lastKnownPosition.Coordinate.Latitude, lastKnownPosition.Coordinate.Longitude);

                await dummyTest();
            }
        }

        private async Task dummyTest()
        {
            string firstMarkId = "6e3d7472aa134f03ad682075b0ad0a59";
            Mark stamMark = await AzureWebApi.MobileService.GetTable<Mark>().LookupAsync(firstMarkId);
            new IOSLocalNotification().Show("Location changed", stamMark.Message);
        }

        private void locMgr_OnFailor(object sender, NSErrorEventArgs e)
        {
            Console.WriteLine("didFailWithError " + e.Error);
            Console.WriteLine("didFailWithError coe " + e.Error.Code);
        }  

        protected override ILocalNotification CreateLocalNotification()
        {
            return new IOSLocalNotification();
        }

        protected override void LogScanningException(Exception e)
        {
            Console.WriteLine("ClosestMarkScanFail: " + e.Message);
        }
    }
}