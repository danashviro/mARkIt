using CoreGraphics;
using Foundation;
using Syncfusion.iOS.Buttons;
using System;
using UIKit;


namespace mARkIt.iOS
{
    public partial class NewMarkViewController : UIViewController
    {
        public NewMarkViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            View.AddGestureRecognizer(new UITapGestureRecognizer(() => View.EndEditing(true)));
        }
    }
}