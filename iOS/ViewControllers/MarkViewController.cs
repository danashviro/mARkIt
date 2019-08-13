using Foundation;
using MapKit;
using mARkIt.Models;
using Syncfusion.SfRating.iOS;
using System;
using UIKit;

namespace mARkIt.iOS
{
    public partial class MarkViewController : UIViewController
    {
        public Mark ViewMark { get; set; }

        public MarkViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            backBarButton.Clicked += BackBarButton_Clicked;
            ratingBar.Value = 3;
            ratingBar.UserInteractionEnabled = false;
            ratingBar.ItemSize = 17;
            ratingBar.Precision = SFRatingPrecision.Exact;
            deleteMarkButton.Clicked += DeleteMarkButton_Clicked;
            initMarkDetails();
        }

        private void initMarkDetails()
        {
            messageTextView.Text = ViewMark.Message;
            dateLabel.Text = ViewMark.createdAt.ToLocalTime().ToLongDateString();
            ratingBar.Value = (float)ViewMark.Rating;
            prepareMap();
        }

        private async void DeleteMarkButton_Clicked(object sender, EventArgs e)
        {
            bool deleted = await Mark.Delete(ViewMark);
            if(deleted)
            {
                Helpers.Alert.DisplayAnAlert("Ok", "The mARk was deleted", this, new Action<UIAlertAction>((a) => NavigationController.PopViewController(true)));
            }
            else
            {
                Helpers.Alert.DisplayAnAlert("Error", "There was a problem deleting this mARk, Please try later", this);                
            }
        }
        

        private void BackBarButton_Clicked(object sender, EventArgs e)
        {
            NavigationController.PopViewController(true);
        }

        private void prepareMap()
        {
            var markLocation = new CoreLocation.CLLocationCoordinate2D(ViewMark.Latitude, ViewMark.Longitude);
            var coordinateSpan = new MKCoordinateSpan(0.01, 0.01);
            var coordinateRegion = new MKCoordinateRegion(markLocation, coordinateSpan);
            mapView.SetRegion(coordinateRegion, false);
            var pin = new MKPointAnnotation()
            {
                Title = ViewMark.Message,
                Coordinate = markLocation
            };
            mapView.AddAnnotation(pin);

        }
    }
}