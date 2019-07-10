using Foundation;
using System;
using UIKit;
using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using WikitudeComponent.iOS;
using mARkIt.iOS.CoreServices;

namespace mARkIt.iOS
{
    public partial class MainTabBarViewController : UITabBarController
    {


        public MainTabBarViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            NavigationItem.SetHidesBackButton(true, false);
        }

    }
}