using Foundation;
using mARkIt.Authentication;
using System;
using UIKit;
using static mARkIt.iOS.Notifications.LocationManager;

namespace mARkIt.iOS
{
    public partial class TabBarController : UITabBarController
    {
        public TabBarController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            MarksScanner.StartScanning();
            LoginHelper.LoggedOut += MarksScanner.StopScanning;
        }
    }
}