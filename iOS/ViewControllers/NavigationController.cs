using Foundation;
using mARkIt.Authentication;
using mARkIt.iOS.CoreServices;
using mARkIt.Models;
using System;
using UIKit;
using WikitudeComponent.iOS;
using Xamarin.Auth;

namespace mARkIt.iOS
{
    public partial class NavigationController : UINavigationController
    {
        private WTAuthorizationRequestManager m_AuthorizationRequestManager = new WTAuthorizationRequestManager();
        User m_User;
        Account m_StoredAccount;

        public NavigationController(IntPtr handle) : base(handle)
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            getWikitudePermissions();
        }


        private void getWikitudePermissions()
        {
            WTFeatures requiredFeatures = WTFeatures.Geo | WTFeatures.WTFeature_InstantTracking;

            ArExperienceAuthorizationController.AuthorizeRestricedAPIAccess(m_AuthorizationRequestManager, requiredFeatures, () =>
            {
                autoConnect();
            }, (UIAlertController alertController) =>
            {
                UIAlertController alert = new UIAlertController();
                alertController.Title = "Permissions Denied";
                alertController.Message = "You cannot proceed without granting permissions";
                alertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, (r) => Environment.Exit(0)));
                PresentViewController(alert, true, null);
            });
        }

        private async void autoConnect()
        {
            // TODO - add Google
            m_StoredAccount = await SecureStorageAccountStore
                .GetAccountAsync("Facebook");
            if (m_StoredAccount == null)
            {
                m_StoredAccount = await SecureStorageAccountStore
                    .GetAccountAsync("Google");
            }
            if (m_StoredAccount == null)
                PerformSegue("loginSegue", this);
            else
                startMainApp();

        }

        public override bool ShouldPerformSegue(string segueIdentifier, NSObject sender)
        {
            if (segueIdentifier == "loginSegue") //after press login moveOnlyIfLoggedIn 
            {
                return m_StoredAccount == null;
            }
            else
            {
                return m_StoredAccount != null;
            }
        }

        private void startMainApp()
        {
            PerformSegue("launchAppSegue", this);
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            if (segue.Identifier == "launchAppSegue")
            {
                var destenationViewController = segue.DestinationViewController as MainTabBarViewController;
                destenationViewController.Account = m_StoredAccount;
            }
            base.PrepareForSegue(segue, sender);
        }


    }
}
