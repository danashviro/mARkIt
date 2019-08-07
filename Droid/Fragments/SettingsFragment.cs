using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using mARkIt.Authentication;
using mARkIt.Models;

namespace mARkIt.Droid.Fragments
{
    public class SettingsFragment : Android.Support.V4.App.Fragment
    {
        private User m_User;

        public SettingsFragment(User i_User)
        {
            m_User = i_User;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(Resource.Layout.Settings, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            Button logoutButton = view.FindViewById<Button>(Resource.Id.logout_button);
            logoutButton.Click += LogoutButton_Click;
        }

        private void LogoutButton_Click(object sender, EventArgs e)
        {
            // remove account from device
            SecureStorageAccountStore.RemoveAllAccounts();

            //  go back to login activity
            Intent loginIntent = new Intent(Activity, typeof(LoginActivity));
            StartActivity(loginIntent);
            Activity.Finish();
        }
    }
}