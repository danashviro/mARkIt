using mARkIt.Authentication;
using mARkIt.iOS.CoreServices;
using mARkIt.iOS.Helpers;
using mARkIt.Utils;
using System;
using System.Threading.Tasks;
using UIKit;
using WikitudeComponent.iOS;
using Xamarin.Auth;

namespace mARkIt.iOS
{
    public partial class InitViewController : UIViewController
    {
        private WTAuthorizationRequestManager m_AuthorizationRequestManager = new WTAuthorizationRequestManager();
        private Account m_Account;

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
                Alert.DisplayAnAlert("Permissions Denied", "You cannot proceed without granting permissions", this, (r) => Environment.Exit(0));
            });
        }

        private async void autoConnect()
        {
            try
            {
                await LoginHelper.AutoConnect(refreshGoogleAccessToken);
            }
            catch
            {

            }

            if (App.ConnectedUser == null)
            {
                PerformSegue("loginSegue", this);
            }
            else
            {
                PerformSegue("launchAppSegue", this);
            }

        }


        private async Task refreshGoogleAccessToken(Account i_Account)
        {
            GoogleAuthenticator glAuth = new GoogleAuthenticator(Keys.GoogleClientId, Configuration.GoogleAuthScope);
            OAuth2Authenticator oauth2 = glAuth.GetOAuth2();
            m_Account = i_Account;
            oauth2.Completed += OnAuthenticationCompleted_RefreshedToken;
            int refreshTokenExpireTime = await oauth2.RequestRefreshTokenAsync(i_Account.Properties["refresh_token"]);
        }

        private void OnAuthenticationCompleted_RefreshedToken(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                m_Account.Properties["access_token"] = e.Account.Properties["access_token"];
            }
        }

    }
}