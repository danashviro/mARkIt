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
        private User m_User;
        public Account Account { get; set; }

        public MainTabBarViewController(IntPtr handle) : base(handle)
        {
        }

        public override async void ViewDidLoad()
        {
            base.ViewDidLoad();
            NavigationItem.SetHidesBackButton(true, false);
            await createUserObject();
            (this.ViewControllers[0] as ARViewController).User = m_User;
            (this.ViewControllers[1] as MapViewController).User = m_User;
            (this.ViewControllers[2] as MyMarksViewController).User = m_User;
            (this.ViewControllers[3] as SettingsViewController).User = m_User;
        }
        
        private async Task createUserObject()
        {
            FacebookClient fbClient = new FacebookClient(Account);
            User user = await fbClient.GetUserAsync();
            m_User = await User.GetUserByEmail(user.Email);
        }

        

    }
}