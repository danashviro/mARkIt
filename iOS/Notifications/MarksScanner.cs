using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using CoreLocation;
using Foundation;
using mARkIt.Models;
using mARkIt.Services;
using UIKit;

namespace mARkIt.iOS.Notifications
{
    public static class LocationManager
    {
        private static DateTime lastServiceRun;

        private static CLLocationManager locMgr;

         static LocationManager()
        {
            locMgr = new CLLocationManager();
            locMgr.AllowsBackgroundLocationUpdates = true;
            locMgr.Failed += (object sender, NSErrorEventArgs e) =>
            {
                Console.WriteLine("didFailWithError " + e.Error);
                Console.WriteLine("didFailWithError coe " + e.Error.Code);
            };
        }

        public static TimeSpan TimeDiff { get; set; }

        public static CLLocationManager LocMgr
        {
            get
            {
                return locMgr;
            }
        }

        public static void StartLocationUpdates()
        {

            if (CLLocationManager.LocationServicesEnabled)
            {
                // sets the accuracy that we want in meters
                LocMgr.DesiredAccuracy = 1;

                
                LocMgr.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) =>
                {
                    CLLocation newLocation = e.Locations[e.Locations.Length - 1];
                    PrintLocation(newLocation);
                };
                

                //// Start our location updates
                LocMgr.StartUpdatingLocation();

                lastServiceRun = DateTime.Now;

                // Get some output from our manager in case of failure
                LocMgr.Failed += (object sender, NSErrorEventArgs e) =>
                {
                    Console.WriteLine(e.Error);
                };
            }
            else
            {
                //// Let the user know that they need to enable LocationServices
                Console.WriteLine("Location services not enabled, please enable this in your Settings");
            }
        }

        /// <summary>
        /// The stop updating location.
        /// </summary>
        public static void StopUpdatingLocation()
        {
            locMgr.StopUpdatingLocation();
        }


        public static async void PrintLocation(CLLocation location)
        {
            Console.WriteLine("Longitude: " + location.Coordinate.Longitude);
            Console.WriteLine("Latitude: " + location.Coordinate.Latitude);

            var diff = DateTime.Now - lastServiceRun;
            TimeDiff = diff;
            if (TimeDiff.Seconds >= 10)
            {
                lastServiceRun = DateTime.Now;
                string firstMarkId = "6e3d7472aa134f03ad682075b0ad0a59";
                Mark stamMark = await AzureWebApi.MobileService.GetTable<Mark>().LookupAsync(firstMarkId);
                new LocalNotification().Show("Location changed", stamMark.Message);
            }
        }

        public static class MarksScanner
        {
            public static void StartScanning()
            {
                LocationManager.StartLocationUpdates();
            }

            public static void StopScanning()
            {
                LocationManager.StopUpdatingLocation();
            }

            private async static void checkForClosestNewMark()
            {
                try
                {
                    CLLocation lastKnownPosition = getCurrentLocation();

                    if (lastKnownPosition != null)
                    {
                        Dictionary<string, string> parameters = new Dictionary<string, string>
                    {
                        {"latitude", lastKnownPosition.Coordinate.Latitude.ToString() },
                        {"longitude", lastKnownPosition.Coordinate.Longitude.ToString() }
                    };

                        Mark closestMark = await AzureWebApi.MobileService.InvokeApiAsync<Mark>("ClosestMark", HttpMethod.Get, parameters);

                        if (closestMark != null)
                        {
                            new LocalNotification().Show("mARkIt", "A new mark is closeby!");
                        }
                    }
                }

                catch (Exception e)
                {
                    // Log
                }
            }

            private static CLLocation getCurrentLocation()
            {
                throw new NotImplementedException();
            }
        }


    }
    
}