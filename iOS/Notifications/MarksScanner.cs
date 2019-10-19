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
    public class LocationManager
    {
        private static DateTime lastServiceRun;

        private CLLocationManager locMgr;

        public LocationManager()
        {
            this.locMgr = new CLLocationManager();
            this.locMgr.AllowsBackgroundLocationUpdates = true;
            this.LocationUpdated += this.PrintLocation;
            this.locMgr.Failed += (object sender, NSErrorEventArgs e) =>
            {
                Console.WriteLine("didFailWithError " + e.Error);
                Console.WriteLine("didFailWithError coe " + e.Error.Code);
            };
        }

        public event EventHandler<LocationUpdatedEventArgs> LocationUpdated = delegate { };

        public static TimeSpan TimeDiff { get; set; }

        public CLLocationManager LocMgr
        {
            get
            {
                return this.locMgr;
            }
        }

        public void StartLocationUpdates()
        {

            if (CLLocationManager.LocationServicesEnabled)
            {
                // sets the accuracy that we want in meters
                this.LocMgr.DesiredAccuracy = 1;

                //// Location updates are handled differently pre-iOS 6. If we want to support older versions of iOS,
                //// we want to do perform this check and let our LocationManager know how to handle location updates.

                if (UIDevice.CurrentDevice.CheckSystemVersion(6, 0))
                {
                    this.LocMgr.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) =>
                    {
                        //// fire our custom Location Updated event
                        this.LocationUpdated(this, new LocationUpdatedEventArgs(e.Locations[e.Locations.Length - 1]));
                    };
                }
                else
                {
                    //// this won't be called on iOS 6 (deprecated). We will get a warning here when we build.
                    this.LocMgr.UpdatedLocation += (object sender, CLLocationUpdatedEventArgs e) =>
                    {
                        this.LocationUpdated(this, new LocationUpdatedEventArgs(e.NewLocation));
                    };
                }

                //// Start our location updates
                this.LocMgr.StartUpdatingLocation();

                lastServiceRun = DateTime.Now;

                // Get some output from our manager in case of failure
                this.LocMgr.Failed += (object sender, NSErrorEventArgs e) =>
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
        public void StopUpdatingLocation()
        {
            this.locMgr.StopUpdatingLocation();
        }

        /// <summary>
        /// The print location. (This will keep going in the background)
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> Location updated event argument </param>
        public void PrintLocation(object sender, LocationUpdatedEventArgs e)
        {
            CLLocation location = e.Location;

            Console.WriteLine("Longitude: " + location.Coordinate.Longitude);
            Console.WriteLine("Latitude: " + location.Coordinate.Latitude);

            var diff = DateTime.Now - lastServiceRun;
            TimeDiff = diff;
            if (TimeDiff.Seconds >= 60)
            {
                lastServiceRun = DateTime.Now;
                new LocalNotification().Show("Location changed", "!!");
            }
        }

        public static class MarksScanner
        {
            public static void StartScanning()
            {

            }

            public static void StopScanning()
            {

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

        public class LocationUpdatedEventArgs : EventArgs
        {
            private CLLocation location;

            public LocationUpdatedEventArgs(CLLocation location)
            {
                this.location = location;
            }

            public CLLocation Location
            {
                get { return this.location; }
            }
        }
    }
    
}