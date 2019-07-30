using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Com.Wikitude.Architect;
using Android;
using Com.Wikitude.Common.Permission;
using System;
using Xamarin.Auth;
using Android.Support.V7.App;
using mARkIt.Authentication;
using mARkIt.Utils;

namespace mARkIt.Droid
{
    [Activity(Label = "mARk-It", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IPermissionManagerPermissionManagerCallback, IFacebookAuthenticationDelegate
    {
        User m_User;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            // create buttons
            Button facebookLoginButton = FindViewById<Button>(Resource.Id.facebookLoginButton);
            facebookLoginButton.Click += OnFacebookLoginButtonClicked;

            // auto connect
            autoConnect();
        }

        Account m_StoredAccount;

        private async void autoConnect()
        {
            // TODO - add Google
            m_StoredAccount = await MarkIt.Utils.SecureStorageAccountStore
                .GetAccountAsync("Facebook");
            if (m_StoredAccount != null)
            {
                startMainApp(m_StoredAccount);
            }
        }

        private void askForARPermissions()
        {
            string[] permissions = { Manifest.Permission.Camera, Manifest.Permission.AccessFineLocation };
            ArchitectView.PermissionManager.CheckPermissions(this, permissions, PermissionManager.WikitudePermissionRequest, this);
        }

        private void OnFacebookLoginButtonClicked(object sender, EventArgs e)
        {
            FacebookAuthenticator facebookAuthenticator = new FacebookAuthenticator(Keys.FacebookAppId,
                         Configuration.FacebookAuthScope,
                         this);
            OAuth2Authenticator oauth2authenticator = facebookAuthenticator.GetOAuth2();
            Intent FBIntent = oauth2authenticator.GetUI(this);
            StartActivity(FBIntent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            int[] results = new int[grantResults.Length];
            for (int i = 0; i < grantResults.Length; i++)
            {
                results[i] = (int)grantResults[i];
            }
            ArchitectView.PermissionManager.OnRequestPermissionsResult(requestCode, permissions, results);
        }

        bool permissionsGranted = false;
        public void PermissionsDenied(string[] deniedPermissions)
        {
            new Android.App.AlertDialog.Builder(this)
               .SetTitle("Permissions Denied")
               .SetMessage("You cannot proceed without granting permissions");
        }

        public void PermissionsGranted(int responseCode)
        {
            permissionsGranted = true;
            // go to main page (tabs) if we got all the permissions
            Intent mainTabs = new Intent(this, typeof(TabsActivity));
            mainTabs.PutExtra("Email", m_User.Email);
            StartActivity(mainTabs);
            Finish();
        }

        public void ShowPermissionRationale(int requestCode, string[] permissions)
        {

        }

        public void OnAuthenticationCompleted(Account i_Account)
        {
            startMainApp(i_Account);
        }

        public void OnAuthenticationFailed(string i_Message, Exception i_Exception)
        {
            new Android.App.AlertDialog.Builder(this)
                           .SetTitle(i_Message)
                           .SetMessage(i_Exception?.ToString())
                           .Show();
        }

        public void OnAuthenticationCanceled()
        {
            new Android.App.AlertDialog.Builder(this)
                           .SetTitle("Authentication canceled")
                           .SetMessage("You didn't completed the authentication process")
                           .Show();
        }

        private void startMainApp(Account i_Account)
        {
            // TODO add facebook client to retrieve email / id with the account

            //new Android.App.AlertDialog.Builder(this)
            //   .SetTitle("Authentication sucess")
            //   .SetMessage("SUCCESS")
            //   .Show();
            m_User = new User();
            m_User.Email = "dedisidi@gmail.com";

            if (!permissionsGranted)
            {
                askForARPermissions();
            }

            //startMainApp(user);
            //// Retrieve the user's email address
            //var facebookService = new FacebookService();
            //var email = await facebookService.GetEmailAsync(token.AccessToken);

            //// Display it on the UI
            //var facebookButton = FindViewById<Button>(Resource.Id.facebookLoginButton);
            //facebookButton.Text = $"Connected with {email}";
        }
    }
}

