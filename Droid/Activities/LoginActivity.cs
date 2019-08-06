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

namespace mARkIt.Droid
{
    [Activity(Label = "Login")]
    public class LoginActivity: AppCompatActivity, IAuthenticationDelegate
    {      
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Login);

            // create buttons

            // facebook button
            ImageButton facebookLoginButton = FindViewById<ImageButton>(Resource.Id.facebook_login_button);
            facebookLoginButton.Click += OnFacebookLoginButtonClicked;

            // google button
            ImageButton googleLoginButton = FindViewById<ImageButton>(Resource.Id.google_login_button);
            googleLoginButton.Click += OnGoogleLoginButtonClicked;
        }

        private void OnFacebookLoginButtonClicked(object sender, EventArgs e)
        {
            FacebookAuthenticator facebookAuthenticator = new FacebookAuthenticator(
                Keys.FacebookAppId,
                Configuration.FacebookAuthScope,
                this);
            OAuth2Authenticator oauth2authenticator = facebookAuthenticator.GetOAuth2();
            Intent FBIntent = oauth2authenticator.GetUI(this);
            StartActivity(FBIntent);
        }

        private void OnGoogleLoginButtonClicked(object sender, EventArgs e)
        {
            GoogleAuthenticator googleAuthenticator = new GoogleAuthenticator(
                Keys.GoogleClientId,
                Configuration.GoogleAuthScope,
                this);
            OAuth2Authenticator oauth2authenticator = googleAuthenticator.GetOAuth2();
            Intent FBIntent = oauth2authenticator.GetUI(this);
            StartActivity(FBIntent);
        }

        public async void OnAuthenticationCompleted(Account i_Account)
        {
            // save account to device
            await mARkIt.Authentication.SecureStorageAccountStore.SaveAccountAsync(i_Account, "Facebook");

            // serialize it so we move it to another activity
            string accountAsJson = JsonConvert.SerializeObject(i_Account);

            // start main app
            startMainApp(accountAsJson);
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

        private void startMainApp(string i_AccountAsJson)
        {
            Intent mainTabs = new Intent(this, typeof(TabsActivity));
            mainTabs.PutExtra("account", i_AccountAsJson);
            StartActivity(mainTabs);
            Finish();
        }
    }
}

