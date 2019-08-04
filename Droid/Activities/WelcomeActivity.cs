using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Com.Wikitude.Architect;
using Com.Wikitude.Common.Permission;
using Xamarin.Auth;

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
            Task.Run(() => autoConnect()).ContinueWith(
                task => askForARPermissions(),
                TaskScheduler.FromCurrentSynchronizationContext());            
        }


        private async void autoConnect()
        {
            m_Account = await mARkIt.Authentication.SecureStorageAccountStore
                .GetAccountAsync("Facebook");
            if (m_Account == null)
            {
                m_Account = await mARkIt.Authentication.SecureStorageAccountStore
                    .GetAccountAsync("Google");
            }
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
            loadApp();
        }

        public void PermissionsDenied(string[] deniedPermissions)
        {
            showPermissionsDeniedDialog();
        }

        public void ShowPermissionRationale(int requestCode, string[] permissions)
        {
            showPermissionsDeniedDialog();
        }

        private async void loadApp()
        {
            await Task.Delay(TimeSpan.FromSeconds(1));

            // Go straight to main tabs page
            if (m_Account != null)
            {
                startMainApp(m_Account);
            }
            else // go to login page
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

        private void startMainApp(Account i_Account)
        {
            // TODO add facebook client to retrieve email / id with the account

            //mARkIt.Authentication.FacebookClient fbClient = new Authentication.FacebookClient(i_Account);
            string email = string.Empty; // = fbClient.GetEmailAddress();
            Intent mainTabs = new Intent(this, typeof(TabsActivity));
            mainTabs.PutExtra("Email", email);
            StartActivity(mainTabs);
            Finish();
        }

        private void showPermissionsDeniedDialog()
        {
            Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
            dialog.SetTitle("Permissions Denied");
            dialog.SetMessage("You cannot proceed without granting permissions");
            dialog.SetPositiveButton("OK", (sender, eventArgs) => Finish());
            dialog.Show();
        }
    }
}