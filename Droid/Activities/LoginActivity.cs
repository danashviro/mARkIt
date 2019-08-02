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
using System.Threading.Tasks;

namespace mARkIt.Droid
{
    [Activity(Label = "Login")]
    public class LoginActivity: AppCompatActivity, IPermissionManagerPermissionManagerCallback, IFacebookAuthenticationDelegate
    {      
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // create buttons
            Button facebookLoginButton = FindViewById<Button>(Resource.Id.facebookLoginButton);
            facebookLoginButton.Click += OnFacebookLoginButtonClicked;

            // auto connect
            autoConnect("Facebook");
            //autoConnect("Google");

            SetContentView(Resource.Layout.Login);
        }

        private static async Task<bool> autoConnect(string i_ServiceType)
        {
            Account account = null;
            // TODO - add Google
            account = await mARkIt.Authentication.SecureStorageAccountStore
                .GetAccountAsync(i_ServiceType);
            if (account != null)
            {
                //startMainApp(account);
            }
            return false;
        }

        private void askForARPermissions()
        {
            string[] permissions = { Manifest.Permission.Camera, Manifest.Permission.AccessFineLocation };
            ArchitectView.PermissionManager.CheckPermissions(this, permissions, PermissionManager.WikitudePermissionRequest, this);
        }

        private async void OnFacebookLoginButtonClicked(object sender, EventArgs e)
        {
            bool canAutoConnect = await autoConnect("Facebook");
            if (canAutoConnect == false)
            {
                FacebookAuthenticator facebookAuthenticator = new FacebookAuthenticator(Keys.FacebookAppId,
                    Configuration.FacebookAuthScope,
                    this);
                OAuth2Authenticator oauth2authenticator = facebookAuthenticator.GetOAuth2();
                Intent FBIntent = oauth2authenticator.GetUI(this);
                StartActivity(FBIntent);
            }
            else
            {
                askForARPermissions();
            }
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
               .SetMessage("You cannot proceed without granting permissions").Show();
            // if permission was not granted, we do not want to store any account
            mARkIt.Authentication.SecureStorageAccountStore.RemoveAllAccounts();
        }

        public void PermissionsGranted(int responseCode)
        {
            permissionsGranted = true;
        }

        public void ShowPermissionRationale(int requestCode, string[] permissions)
        {
            new Android.App.AlertDialog.Builder(this)
               .SetTitle("Permissions Denied")
               .SetMessage("You cannot proceed without granting permissions").Show();
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
            User user = new User();
            user.Email = "dedisidi@gmail.com";

            // go to main page (tabs) if we got all the permissions
            Intent mainTabs = new Intent(this, typeof(TabsActivity));
            mainTabs.PutExtra("Email", user.Email);
            StartActivity(mainTabs);
            Finish();

            //if (!permissionsGranted)
            //{
            //    askForARPermissions();
            //}

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

