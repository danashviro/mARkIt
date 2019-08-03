using System;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace mARkIt.Droid.Fragments
{
    public class SettingsFragment : Android.Support.V4.App.Fragment
    {
        public event EventHandler Logout;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
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
            // this tells the TabsActivity to kill itself and go back to login activity
            this.Logout?.Invoke(null, EventArgs.Empty);
        }
    }
}