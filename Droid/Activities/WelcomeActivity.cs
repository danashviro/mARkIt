using System;
using System.Linq;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using mARkIt.Authentication;
using Com.Wikitude.Architect;
using Com.Wikitude.Common.Permission;
using Xamarin.Auth;
using mARkIt.Utils;
using mARkIt.Droid.Helpers;
using mARkIt.Droid.Services;

namespace mARkIt.Droid.Activities
{
    [Activity(Label = "mARk-It", MainLauncher = true)]
    public class WelcomeActivity : AppCompatActivity, IPermissionManagerPermissionManagerCallback
    {
        Account m_Account = null;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Welcome);
            askForARPermissions();
        }

        private async void autoConnect()
        {
            try
            {
                await LoginHelper.AutoConnect(refreshGoogleAccessToken);
            }
            catch (Exception)
            {

            }
            loadApp();
        }

        private void askForARPermissions()
        {
            string[] permissions = { Manifest.Permission.Camera,
                                     Manifest.Permission.AccessFineLocation,
                                     Manifest.Permission.ReadExternalStorage,
                                     Manifest.Permission.WriteExternalStorage,
                                     };

            ArchitectView.PermissionManager.CheckPermissions(this, permissions, PermissionManager.WikitudePermissionRequest, this);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            int []results = grantResults.Cast<int>().ToArray();
            ArchitectView.PermissionManager.OnRequestPermissionsResult(requestCode, permissions, results);
            ArchitectView.PermissionManager.CheckPermissions(this, permissions, PermissionManager.WikitudePermissionRequest, this);
        }

        public void PermissionsGranted(int responseCode)
        {
            autoConnect();
        }

        public void PermissionsDenied(string[] deniedPermissions)
        {
            showPermissionsDeniedDialog();
        }

        public void ShowPermissionRationale(int requestCode, string[] permissions)
        {
            showPermissionsDeniedDialog();
        }

        private void loadApp()
        {
            if (App.ConnectedUser != null)
            {
                PushNotificationsService.Register(context: this);
                startMainApp();
            }
            else
            {
                startLoginPage();
            }
        }

        private void startLoginPage()
        {
            Intent loginIntent = new Intent(this, typeof(LoginActivity));
            StartActivity(loginIntent);
            Finish();
        }

        private void startMainApp()
        {
            Intent mainTabs = new Intent(this, typeof(TabsActivity));
            mainTabs.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
            StartActivity(mainTabs);
            Finish();
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

        private void showPermissionsDeniedDialog()
        {
            Alert.Show("Permissions Denied", "You cannot proceed without granting permissions", this, Finish);
        }
    }
}