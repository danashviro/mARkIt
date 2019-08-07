using System;
using Foundation;
using UIKit;
using mARkIt.Utils;
using Xamarin.Auth;
using mARkIt.Authentication;
using mARkIt.Models;

namespace mARkIt.iOS
{
    public partial class SignInViewController : UIViewController, IFacebookAuthenticationDelegate
    {
        bool hasLoggedIn = false;
        private Account m_Account;

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
                return hasLoggedIn;
            }
            return true;
        }


        public async void OnAuthenticationCompleted(Account i_Account)
        {
            DismissViewController(true, null);
            hasLoggedIn = true;
            await SecureStorageAccountStore.SaveAccountAsync(i_Account, "Facebook");
            m_Account = i_Account;
            PerformSegue("launchAppSegue", this);

        }


        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            if (segue.Identifier == "launchAppSegue")
            {
                var destenationViewController = segue.DestinationViewController as MainTabBarViewController;
                destenationViewController.Account = m_Account;
            }
            base.PrepareForSegue(segue, sender);
        }

        public void OnAuthenticationFailed(string i_Message, Exception i_Exception)
        {
            SecureStorageAccountStore.RemoveAllAccounts();
        }

        public void OnAuthenticationCanceled()
        {
            //throw new NotImplementedException();
        }

    }
}
