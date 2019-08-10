using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using System;
using Xamarin.Auth;
using Android.Support.V7.App;
using mARkIt.Authentication;
using Newtonsoft.Json;
using mARkIt.Utils;
using System.Threading.Tasks;
using mARkIt.Models;

namespace mARkIt.Droid
{
    [Activity(Label = "Login")]
    public class LoginActivity: AppCompatActivity, IAuthenticationDelegate
    {
        public static FacebookAuthenticator s_FacebookAuthenticator;
        public static GoogleAuthenticator s_GoogleAuthenticator;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Login);
            
            ImageButton facebookLoginButton = FindViewById<ImageButton>(Resource.Id.facebook_login_button);
            facebookLoginButton.Click += OnFacebookLoginButtonClicked;
            
            ImageButton googleLoginButton = FindViewById<ImageButton>(Resource.Id.google_login_button);
            googleLoginButton.Click += OnGoogleLoginButtonClicked;
        }

        private void OnFacebookLoginButtonClicked(object sender, EventArgs e)
        {
            s_FacebookAuthenticator = new FacebookAuthenticator(
                Keys.FacebookAppId,
                Configuration.FacebookAuthScope,
                this);
            OAuth2Authenticator oauth2authenticator = s_FacebookAuthenticator.GetOAuth2();
            Intent facebookIntent = oauth2authenticator.GetUI(this);
            StartActivity(facebookIntent);
        }

        private void OnGoogleLoginButtonClicked(object sender, EventArgs e)
        {
            s_GoogleAuthenticator = new GoogleAuthenticator(
                Keys.GoogleClientId,
                Configuration.GoogleAuthScope,
                this);
            OAuth2Authenticator oauth2authenticator = s_GoogleAuthenticator.GetOAuth2();
            Intent googleIntent = oauth2authenticator.GetUI(this);
            Xamarin.Auth.CustomTabsConfiguration.CustomTabsClosingMessage = null;
            StartActivity(googleIntent);
        }

        public async void OnAuthenticationCompleted(Account i_Account)
        {
            mARkIt.Authentication.Authentication.e_SupportedAuthentications authType = mARkIt.Authentication.Authentication.e_SupportedAuthentications.Facebook;

            // save account to device
            if (s_FacebookAuthenticator != null)
            {
                await mARkIt.Authentication.SecureStorageAccountStore.SaveAccountAsync(i_Account, "Facebook");
                authType = mARkIt.Authentication.Authentication.e_SupportedAuthentications.Facebook;
            }
            else if (s_GoogleAuthenticator != null)
            {
                await mARkIt.Authentication.SecureStorageAccountStore.SaveAccountAsync(i_Account, "Google");
                authType = mARkIt.Authentication.Authentication.e_SupportedAuthentications.Google;
            }

            startMainApp(i_Account, authType);
        }

        public void OnAuthenticationFailed(string i_Message, Exception i_Exception)
        {
            
            // if permission was not granted, we do not want to store any account - to be safe
            mARkIt.Authentication.SecureStorageAccountStore.RemoveAllAccounts();

            /*
            // show error message
            new Android.App.AlertDialog.Builder(this)
                           .SetTitle(i_Message)
                           .SetMessage(i_Exception?.ToString())
                           .Show();
                           */
        }

        public void OnAuthenticationCanceled()
        {
            // user canceled (pressed back) during auth process
            new Android.App.AlertDialog.Builder(this)
                           .SetTitle("Authentication canceled")
                           .SetMessage("You didn't complete the authentication process")
                           .Show();
        }

        private async void startMainApp(Account i_Account, mARkIt.Authentication.Authentication.e_SupportedAuthentications i_AuthType)
        {
            Intent mainTabs = new Intent(this, typeof(TabsActivity));
            mainTabs.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
            await createUserObjectAsync(i_Account, i_AuthType);

            StartActivity(mainTabs);
            Finish();
        }

        private async Task createUserObjectAsync(Account i_Account, mARkIt.Authentication.Authentication.e_SupportedAuthentications i_AuthType)
        {
            User user = null;
            switch (i_AuthType)
            {
                case mARkIt.Authentication.Authentication.e_SupportedAuthentications.Facebook:
                    FacebookClient fbClient = new FacebookClient(i_Account);
                    user = await fbClient.GetUserAsync();
                    break;
                case mARkIt.Authentication.Authentication.e_SupportedAuthentications.Google:
                    GoogleClient glClient = new GoogleClient(i_Account);
                    user = await glClient.GetUserAsync();
                    break;
                default:
                    break;
            }

            App.ConnectedUser = await User.GetUserByEmail(user.Email);
        }
    }
}

