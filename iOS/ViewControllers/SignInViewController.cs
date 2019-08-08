using System;
using Foundation;
using UIKit;
using mARkIt.Utils;
using Xamarin.Auth;
using mARkIt.Authentication;
using mARkIt.Models;

namespace mARkIt.iOS
{
    public partial class SignInViewController : UIViewController, IAuthenticationDelegate
    {
        private bool m_HasLoggedIn = false;
        private User m_User;
        public static GoogleAuthenticator s_GoogleAuthenticator;
        public static FacebookAuthenticator s_FacebookAuthenticator;



        public SignInViewController(IntPtr handle) : base(handle)
        {
        }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            FacebookButton.TouchUpInside += FacebookButton_TouchUpInside;
            GoogleButton.TouchUpInside += GoogleButton_TouchUpInside;
            NavigationItem.SetHidesBackButton(true, false);

        }

        private void GoogleButton_TouchUpInside(object sender, EventArgs e)
        {
            s_GoogleAuthenticator = new GoogleAuthenticator(
                Keys.GoogleClientId,
                Configuration.GoogleAuthScope,
                this);
            OAuth2Authenticator oauth2authenticator = s_GoogleAuthenticator.GetOAuth2();
            PresentViewController(oauth2authenticator.GetUI(), true, null);
        }

        private void FacebookButton_TouchUpInside(object sender, EventArgs e)
        {
            s_FacebookAuthenticator = new FacebookAuthenticator(Keys.FacebookAppId,
                        Configuration.FacebookAuthScope,
                        this);
            OAuth2Authenticator oauth2authenticator = s_FacebookAuthenticator.GetOAuth2();

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

            m_User = await LoginHelper.GetUser(s_FacebookAuthenticator, s_GoogleAuthenticator, i_Account);
            DismissViewController(true, null);
            m_HasLoggedIn = true;
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
