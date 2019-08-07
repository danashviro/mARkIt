using System;
using Foundation;
using UIKit;
using mARkIt.Utils;
using Xamarin.Auth;
using mARkIt.Authentication;
using mARkIt.Models;
using mARkIt.iOS.Helpers;

namespace mARkIt.iOS
{
    public partial class SignInViewController : UIViewController, IFacebookAuthenticationDelegate
    {
        private bool m_HasLoggedIn = false;
        private User m_User;

        public SignInViewController(IntPtr handle) : base(handle)
        {
        }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            FacebookButton.TouchUpInside += FacebookButton_TouchUpInside;
            NavigationItem.SetHidesBackButton(true, false);

        }

        private void FacebookButton_TouchUpInside(object sender, EventArgs e)
        {
            FacebookAuthenticator facebookAuthenticator = new FacebookAuthenticator(Keys.FacebookAppId,
                        Configuration.FacebookAuthScope,
                        this);
            OAuth2Authenticator oauth2authenticator = facebookAuthenticator.GetOAuth2();

            PresentViewController(oauth2authenticator.GetUI(), true, null);
        }


        public override bool ShouldPerformSegue(string segueIdentifier, NSObject sender)
        {
            if (segueIdentifier == "launchAppSegue") //after press login moveOnlyIfLoggedIn 
            {
                return m_HasLoggedIn;
            }
            return true;
        }


        public async void OnAuthenticationCompleted(Account i_Account)
        {
            DismissViewController(true, null);
            m_HasLoggedIn = true;
            await SecureStorageAccountStore.SaveAccountAsync(i_Account, "Facebook");
            m_User = await LoginHelper.CreateUserObject(i_Account);
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

        public void OnAuthenticationFailed(string i_Message, Exception i_Exception)
        {
            SecureStorageAccountStore.RemoveAllAccounts();
        }

        public void OnAuthenticationCanceled()
        {
        }

    }
}
