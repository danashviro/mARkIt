using System;
using UIKit;
using System.Threading.Tasks;
using Xamarin.Auth;
using mARkIt.Authentication;
using mARkIt.Models;

namespace mARkIt.iOS
{
    public partial class MainTabBarViewController : UITabBarController
    {
        public User ConnectedUser { get; set; }

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