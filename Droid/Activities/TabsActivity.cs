using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using mARkIt.Authentication;
using mARkIt.Droid.Fragments;
using mARkIt.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace mARkIt.Droid
{
    [Activity(Label = "TabsActivity", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.KeyboardHidden | Android.Content.PM.ConfigChanges.ScreenSize)]

    public class TabsActivity : FragmentActivity
    {
        TabLayout m_TabLayout;
        private ARFragment m_ARFragment;
        private MapFragment m_MapFragment;
        private SettingsFragment m_SettingsFragment;
        private MyMarksFragment m_MyMarksFragment;
        public User m_User;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Tabs);

            // create user object from the
            string accountAsJson = Intent.GetStringExtra("account");
            Task.Run(() => createUserObjectAsync(accountAsJson));

            m_TabLayout = FindViewById<TabLayout>(Resource.Id.mainTabLayout);
            m_TabLayout.TabSelected += TabLayout_TabSelected;

            // create fragments
            m_ARFragment = new ARFragment();
            m_MapFragment = new MapFragment();
            m_SettingsFragment = new SettingsFragment();
            m_MyMarksFragment = new MyMarksFragment();
            fragmentNavigate(m_ARFragment);
        }

        private async void createUserObjectAsync(string i_AccountAsJson)
        {
            Account account = JsonConvert.DeserializeObject<Account>(i_AccountAsJson);
            FacebookClient fbClient = new Authentication.FacebookClient(account);
            m_User = await fbClient.GetUserAsync();
        }

        private void TabLayout_TabSelected(object sender, TabLayout.TabSelectedEventArgs e)
        {
            switch(e.Tab.Position)
            {
                case 0:
                    fragmentNavigate(m_ARFragment);
                    break;
                case 1:
                    fragmentNavigate(m_MapFragment);
                    break;
                case 2:
                    fragmentNavigate(m_MyMarksFragment);
                    break;
                case 3:
                    fragmentNavigate(m_SettingsFragment);
                    break;
            }
        }

        private void fragmentNavigate(Android.Support.V4.App.Fragment fragment)
        {
            var transaction = SupportFragmentManager.BeginTransaction();
            transaction.Replace(Resource.Id.contentFrame, fragment);
            transaction.Commit();
        }
    }
}
