using Foundation;
using MapKit;
using System;
using UIKit;
using mARkIt.Services;
using mARkIt.Models;
using CoreLocation;

namespace mARkIt.iOS
{
    public partial class MapViewController : UIViewController
    {
        private bool m_UserLocationInit = false;
        public User User { get; set; }

        public MapViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            getPins();
            mapView.DidUpdateUserLocation += MapView_DidUpdateUserLocation; 

        }

        private void MapView_DidUpdateUserLocation(object sender, MKUserLocationEventArgs e)
        {
            if(!m_UserLocationInit)
            {
                m_UserLocationInit = true;
                var coordinateSpan = new MKCoordinateSpan(0.01, 0.01); //this seems to be the maximum zoom out
                var coordinateRegion = new MKCoordinateRegion(mapView.UserLocation.Coordinate, coordinateSpan);
                mapView.SetRegion(coordinateRegion, false);
            }
        }

        private async void getPins()
        {
            var marks = await Mark.GetRelevantMarks(User.RelevantCategoriesCode);
            foreach (Mark mark in marks)
            {
                var pin = new MKPointAnnotation()
                {
                    Title = mark.Message,
                    Coordinate = new CLLocationCoordinate2D(mark.Latitude, mark.Longitude)
                };
                mapView.AddAnnotation(pin);
            }
        }
    }
}