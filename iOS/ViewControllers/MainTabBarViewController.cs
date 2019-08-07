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

        public override async void ViewDidLoad()
        {
            (this.ViewControllers[0] as ARViewController).ConnectedUser = ConnectedUser;
            (this.ViewControllers[1] as MapViewController).ConnectedUser = ConnectedUser;
            (this.ViewControllers[2] as MyMarksViewController).ConnectedUser = ConnectedUser;
            (this.ViewControllers[3] as SettingsViewController).ConnectedUser = ConnectedUser;
            base.ViewDidLoad();
            NavigationItem.SetHidesBackButton(true, false);
        }
                

    }
}