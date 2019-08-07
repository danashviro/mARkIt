using CoreGraphics;
using Foundation;
using mARkIt.Models;
using Syncfusion.SfRating.iOS;
using System;
using UIKit;

namespace mARkIt.iOS
{
    public partial class SettingsViewController : UIViewController
    {
        public User User { get; set; }

        public SettingsViewController (IntPtr handle) : base (handle)
        {
        }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

        }

        partial void LogoutButton_TouchUpInside(UIButton sender)
        {
            Xamarin.Essentials.SecureStorage.RemoveAll();
        }

       
    }
}