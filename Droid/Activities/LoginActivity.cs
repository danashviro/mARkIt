using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using System;
using Xamarin.Auth;
using Android.Support.V7.App;
using mARkIt.Authentication;
using mARkIt.Droid.Services;

namespace mARkIt.Droid
{
    [Activity(Label = "Login")]
    public class LoginActivity: AppCompatActivity, IAuthenticationDelegate
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            SetContentView(Resource.Layout.Login);
            
            ImageButton facebookLoginButton = FindViewById<ImageButton>(Resource.Id.facebook_login_button);
            facebookLoginButton.Click += OnFacebookLoginButtonClicked;
            
            ImageButton googleLoginButton = FindViewById<ImageButton>(Resource.Id.google_login_button);
            googleLoginButton.Click += OnGoogleLoginButtonClicked;
        }

        private void OnFacebookLoginButtonClicked(object sender, EventArgs e)
        {
            var oauth2authenticator = LoginHelper.GetFacebook2Authenticator(this);
            Intent facebookIntent = oauth2authenticator.GetUI(this);
            StartActivity(facebookIntent);
        }

        private void OnGoogleLoginButtonClicked(object sender, EventArgs e)
        {
            OAuth2Authenticator oauth2authenticator = LoginHelper.GetGoogle2Authenticator(this);
            Intent googleIntent = oauth2authenticator.GetUI(this);
            CustomTabsConfiguration.CustomTabsClosingMessage = null;
            StartActivity(googleIntent);
        }

        public async void OnAuthenticationCompleted(Account i_Account)
        {
            await LoginHelper.CreateUserAndSaveToDevice(i_Account);
            PushNotificationsService.Register(context: this);
            startMainApp();
        }

        public void OnAuthenticationFailed(string i_Message, Exception i_Exception)
        {
            
            // if permission was not granted, we do not want to store any account - to be safe
            SecureStorageAccountStore.RemoveAllAccounts();
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

        private async void startMainApp()
        {
            Intent mainTabs = new Intent(this, typeof(TabsActivity));
            mainTabs.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
            StartActivity(mainTabs);
            Finish();
        }
    }
}

