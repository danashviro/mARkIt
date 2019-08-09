using Foundation;
using mARkIt.Authentication;
using mARkIt.iOS.CoreServices;
using mARkIt.iOS.Helpers;
using mARkIt.Models;
using System;
using UIKit;
using WikitudeComponent.iOS;
using Xamarin.Auth;

namespace mARkIt.iOS
{
    public partial class InitViewController : UIViewController
    {
        private WTAuthorizationRequestManager m_AuthorizationRequestManager = new WTAuthorizationRequestManager();
        private User m_User;
        private Account m_StoredAccount;
        private Authentication.Authentication.e_SupportedAuthentications m_AuthType;

        public InitViewController(IntPtr handle) : base(handle)
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
                Alert.DisplayAnAlert("Permissions Denied", "You cannot proceed without granting permissions", (r) => Environment.Exit(0), null);
            });
        }

        private async void autoConnect()
        {
            // TODO - add Google
            string authType;
            m_StoredAccount = await LoginHelper.AutoConnect();

            if (m_StoredAccount == null)
                PerformSegue("loginSegue", this);
            else
            {
                m_StoredAccount.Properties.TryGetValue("AuthType", out authType);
                m_AuthType = authType == "Facebook" ? Authentication.Authentication.e_SupportedAuthentications.Facebook : Authentication.Authentication.e_SupportedAuthentications.Google;
                startMainApp();
            }

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

        private async void startMainApp()
        {
            m_User = await LoginHelper.CreateUserObject(m_StoredAccount, m_AuthType);
            PerformSegue("launchAppSegue", this);
        }



        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            if (segue.Identifier == "launchAppSegue")
            {
                var destenationViewController = segue.DestinationViewController as MainTabBarViewController;
                destenationViewController.ConnectedUser = m_User;
            }
            base.PrepareForSegue(segue, sender);
        }

    }
}