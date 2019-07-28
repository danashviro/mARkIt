using System;
using Foundation;
using mARkIt.iOS.CoreServices;
using UIKit;
using WikitudeComponent.iOS;
using mARkIt.Utils;
using Xamarin.Auth;
using mARkIt.Authentication;
using MarkIt.Utils;

namespace mARkIt.iOS
{
    public partial class SignInViewController : UIViewController, IFacebookAuthenticationDelegate
    {
        bool hasLoggedIn = false;
        User m_User;

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


        public void OnAuthenticationCompleted(Account i_Account)
        {
            DismissViewController(true, null);
            hasLoggedIn = true;
            m_User = new User();
            m_User.Email = "dedisidi@gmail.com";
            PerformSegue("launchAppSegue", this);

        }


        public void OnAuthenticationFailed(string i_Message, Exception i_Exception)
        {
            //throw new NotImplementedException();
        }

        public void OnAuthenticationCanceled()
        {
            //throw new NotImplementedException();
        }
    }
}
