using Foundation;
using MapKit;
using mARkIt.Models;
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
            prepareMap();
            messageTextView.Text = ViewMark.Message;
            ratingBar.Value = 3;
            ratingBar.UserInteractionEnabled = false;
            dateLabel.Text = ViewMark.createdAt.ToLocalTime().ToLongDateString();
            ratingBar.ItemSize = 17;
            deleteMarkButton.Clicked += DeleteMarkButton_Clicked;       
        }

        private async void DeleteMarkButton_Clicked(object sender, EventArgs e)
        {
            bool deleted = await Mark.Delete(ViewMark);
            if(deleted)
            {
                Helpers.Alert.DisplayAnAlert("Ok", "The mARk was deleted", new Action<UIAlertAction>((a) => NavigationController.PopViewController(true)), this);
            }
            else
            {
                Helpers.Alert.DisplayAnAlert("Error", "There was a problem deleting this mARk, Please try later ", null, this);                
            }
        }
        

        private void BackBarButton_Clicked(object sender, EventArgs e)
        {
            NavigationController.PopViewController(true);
        }

        private void prepareMap()
        {
            var markLocation = new CoreLocation.CLLocationCoordinate2D(ViewMark.Latitude, ViewMark.Longitude);
            var coordinateSpan = new MKCoordinateSpan(0.01, 0.01); //this seems to be the maximum zoom out
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