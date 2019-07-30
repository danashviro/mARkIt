using CoreGraphics;
using CoreLocation;
using Foundation;
using mARkIt.Models;
using mARkIt.Services;
using Syncfusion.iOS.Buttons;
using System;
using System.Linq;
using UIKit;


namespace mARkIt.iOS
{
    public partial class NewMarkViewController : UIViewController, ICLLocationManagerDelegate
    {
        private readonly CLLocationManager locationManager = new CLLocationManager();
        private SfRadioButton m_WoodMarkStyleRadioButton, m_MetalMarkStyleRadioButton, m_SchoolMarkStyleRadioButton;
        public NewMarkViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            View.AddGestureRecognizer(new UITapGestureRecognizer(() => View.EndEditing(true)));

            // you can set the update threshold and accuracy if you want:
            //locationManager.DistanceFilter = 10d; // move ten meters before updating
            //locationManager.HeadingFilter = 3d; // move 3 degrees before updating

            // you can also set the desired accuracy:
            locationManager.DesiredAccuracy = 10; // 1000 meters/1 kilometer
            // you can also use presets, which simply evalute to a double value:
            //locationManager.DesiredAccuracy = CLLocation.AccuracyNearestTenMeters;

            locationManager.Delegate = this;
            locationManager.RequestWhenInUseAuthorization();

            if (CLLocationManager.LocationServicesEnabled)
            {
                locationManager.StartUpdatingLocation();
            }

            saveMarkButton.Clicked += SaveMarkButton_Clicked;
            doneBarButton.Clicked += DoneBarButton_Clicked;

            m_WoodMarkStyleRadioButton = new SfRadioButton();
            m_MetalMarkStyleRadioButton = new SfRadioButton();
            m_SchoolMarkStyleRadioButton = new SfRadioButton();
            m_WoodMarkStyleRadioButton.IsChecked = true;
            markStyleRadioGroup.AddArrangedSubview(m_WoodMarkStyleRadioButton);
            markStyleRadioGroup.AddArrangedSubview(m_MetalMarkStyleRadioButton);
            markStyleRadioGroup.AddArrangedSubview(m_SchoolMarkStyleRadioButton);
        }

        private async void SaveMarkButton_Clicked(object sender, EventArgs e)
        {
            CLLocation lastLocation = locationManager.Location;
            if (lastLocation != null || markTextView.Text != string.Empty)
            {
                Location location = new Location()
                {
                    latitude = lastLocation.Coordinate.Latitude,
                    longitude = lastLocation.Coordinate.Longitude,
                    message = markTextView.Text
                };
                await LocationService.Instance().AddLocation(location);               
                NavigationController.PopViewController(true);
            }
            else
            {
                //alert
            }
        }

        

        private async void DoneBarButton_Clicked(object sender, EventArgs e)
        {
            CLLocation lastLocation = locationManager.Location;
            if (lastLocation != null || markTextView.Text != string.Empty)
            {
                Location location = new Location()
                {
                    latitude = lastLocation.Coordinate.Latitude,
                    longitude = lastLocation.Coordinate.Longitude,
                    message = markTextView.Text
                };
                await LocationService.Instance().AddLocation(location);
                NavigationController.PopViewController(true);
            }
            else
            {
                //alert
            }
        }

        partial void CancleBarButton_Activated(UIBarButtonItem sender)
        {
            NavigationController.PopViewController(true);
        }
    }
}