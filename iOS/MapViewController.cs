using Foundation;
using MapKit;
using System;
using UIKit;
using mARkIt.Services;
using mARkIt.Models;

namespace mARkIt.iOS
{
    public partial class MapViewController : UIViewController
    {
        public MapViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            getPins();
        }

        private async void getPins()
        {
            var locations = await LocationService.Instance().GetLocations();
            foreach (Location location in locations)
            {
                var pin = new MKPointAnnotation()
                {
                    Title = location.message,
                    Coordinate = new CoreLocation.CLLocationCoordinate2D(location.latitude, location.longitude)
                    //Type = Xamarin.Forms.Maps.PinType.Generic,
                    //Label = location.message
                };
                mapView.AddAnnotation(pin);
            }
        }
    }
}