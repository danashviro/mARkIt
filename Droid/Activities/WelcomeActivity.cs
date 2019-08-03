﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class WelcomeActivity : Activity, IPermissionManagerPermissionManagerCallback
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Welcome);
            init();
        }

        private async Task init()
        {
            await autoConnect();
            askForARPermissions();
        }

        Account m_Account = null;

        private async Task autoConnect()
        {
            m_Account = await mARkIt.Authentication.SecureStorageAccountStore
                .GetAccountAsync("Facebook");
            if (m_Account == null)
            {
                m_Account = await mARkIt.Authentication.SecureStorageAccountStore
                    .GetAccountAsync("Google");
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
            ArchitectView.PermissionManager.CheckPermissions(this, permissions, PermissionManager.WikitudePermissionRequest, this);
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

        private void onOkClick(object sender, DialogClickEventArgs e)
        {
            Finish();
        }

        private void askForARPermissions()
        {
            string[] permissions = { Manifest.Permission.Camera, Manifest.Permission.AccessFineLocation };
            ArchitectView.PermissionManager.CheckPermissions(this, permissions, PermissionManager.WikitudePermissionRequest, this);
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
            //fbClient.GetEmailAddress();
            Intent tabsIntent = new Intent(this, typeof(TabsActivity));
            StartActivity(tabsIntent);
            Finish();
        }

        public void PermissionsDenied(string[] deniedPermissions)
        {
            Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
            dialog.SetTitle("Permissions Denied");
            dialog.SetMessage("You cannot proceed without granting permissions");
            dialog.SetPositiveButton("OK", onOkClick);
            dialog.Show();
        }

        public void PermissionsGranted(int responseCode)
        {
            m_PermissionsAllowed = true;
            loadApp();

        }

        private bool m_PermissionsAllowed = false;

        public void ShowPermissionRationale(int requestCode, string[] permissions)
        {
            m_PermissionsAllowed = false;
        }
    }
}