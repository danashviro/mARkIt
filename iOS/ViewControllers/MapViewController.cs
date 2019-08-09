using Foundation;
using MapKit;
using System;
using UIKit;
using mARkIt.Services;
using mARkIt.Models;
using CoreLocation;
using System.Threading.Tasks;

namespace mARkIt.iOS
{
    public partial class MapViewController : UIViewController
    {
        private bool m_UserLocationInit = false;
        private bool m_ViewLoaded = false;
        private MapDelegate m_MapDelegate;

        public MapViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            mapView.DidUpdateUserLocation += MapView_DidUpdateUserLocation;
            m_ViewLoaded = true;
            //m_MapDelegate = new MapDelegate();
            //mapView.Delegate = m_MapDelegate;
        }

        public override async void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if(m_ViewLoaded)
            {
                await getPins();
            }
        }

        private void MapView_DidUpdateUserLocation(object sender, MKUserLocationEventArgs e)
        {
            if(!m_UserLocationInit)
            {
                m_UserLocationInit = true;
                var coordinateSpan = new MKCoordinateSpan(0.01, 0.01);
                var coordinateRegion = new MKCoordinateRegion(mapView.UserLocation.Coordinate, coordinateSpan);
                mapView.SetRegion(coordinateRegion, false);
            }
        }

        private async Task getPins()
        {
            var marks = await Mark.GetRelevantMarks(App.User.RelevantCategoriesCode);
            mapView.RemoveAnnotations(mapView.Annotations);
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