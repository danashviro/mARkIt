using Foundation;
using mARkIt.Authentication;
using mARkIt.iOS.CoreServices;
using System;
using UIKit;
using WikitudeComponent.iOS;
using Xamarin.Auth;

namespace mARkIt.iOS
{
    public partial class NavigationController : UINavigationController
    {
        private WTAuthorizationRequestManager authorizationRequestManager = new WTAuthorizationRequestManager();
        User m_User;
        Account m_StoredAccount;

        public NavigationController (IntPtr handle) : base (handle)
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            getWikitudePermissions();
            autoConnect();
        }


        private void getWikitudePermissions()
        {
            WTFeatures requiredFeatures = WTFeatures.Geo | WTFeatures.WTFeature_InstantTracking;

            ArExperienceAuthorizationController.AuthorizeRestricedAPIAccess(authorizationRequestManager, requiredFeatures, () =>
            {

            }, (UIAlertController alertController) =>
            {
            });
        }

        private async void autoConnect()
        {
            // TODO - add Google
            m_StoredAccount = await MarkIt.Utils.SecureStorageAccountStore
                .GetAccountAsync("Facebook");
            if (m_StoredAccount == null)
                PerformSegue("loginSegue", this);
            else
                startMainApp(m_StoredAccount);

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

        private void startMainApp(Account i_Account)
        {
            m_User = new User();
            m_User.Email = "dedisidi@gmail.com";
            PerformSegue("launchAppSegue", this);
        }


    }
}
