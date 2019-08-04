using Foundation;
using MapKit;
using System;
using UIKit;

namespace mARkIt.iOS
{
    public partial class MarkViewController : UIViewController
    {
        public mARkIt.Models.Location Mark { get; set; }

        public MarkViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            backBarButton.Clicked += BackBarButton_Clicked;
            prepareMap();
            messageTextView.Text = Mark.message;
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
            var markLocation = new CoreLocation.CLLocationCoordinate2D(Mark.latitude, Mark.longitude);
            var coordinateSpan = new MKCoordinateSpan(0.01, 0.01); //this seems to be the maximum zoom out
            var coordinateRegion = new MKCoordinateRegion(markLocation, coordinateSpan);
            mapView.SetRegion(coordinateRegion, false);
            var pin = new MKPointAnnotation()
            {
                Title = Mark.message,
                Coordinate = markLocation
            };
            mapView.AddAnnotation(pin);

        }
    }
}