using Foundation;
using MapKit;
using mARkIt.Models;
using System;
using UIKit;

namespace mARkIt.iOS
{
    public partial class MarkViewController : UIViewController
    {
        public Mark Mark { get; set; }

        public MarkViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            backBarButton.Clicked += BackBarButton_Clicked;
            prepareMap();
            messageTextView.Text = Mark.Message;
            ratingBar.Value = 3;
            ratingBar.UserInteractionEnabled = false;
            dateLabel.Text = Mark.createdAt.ToLocalTime().ToLongDateString();
            ratingBar.ItemSize = 17;
            
        }

        private void BackBarButton_Clicked(object sender, EventArgs e)
        {
            NavigationController.PopViewController(true);
        }

        private void prepareMap()
        {
            var markLocation = new CoreLocation.CLLocationCoordinate2D(Mark.Latitude, Mark.Longitude);
            var coordinateSpan = new MKCoordinateSpan(0.01, 0.01); //this seems to be the maximum zoom out
            var coordinateRegion = new MKCoordinateRegion(markLocation, coordinateSpan);
            mapView.SetRegion(coordinateRegion, false);
            var pin = new MKPointAnnotation()
            {
                Title = Mark.Message,
                Coordinate = markLocation
            };
            mapView.AddAnnotation(pin);

        }
    }
}