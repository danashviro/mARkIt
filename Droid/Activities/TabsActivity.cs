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
        private TabLayout m_TabLayout;
        private ARFragment m_ARFragment;
        private MapFragment m_MapFragment;
        private SettingsFragment m_SettingsFragment;
        private MyMarksFragment m_MyMarksFragment;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Tabs);

            // create user object from the account

            m_TabLayout = FindViewById<TabLayout>(Resource.Id.mainTabLayout);
            m_TabLayout.TabSelected += TabLayout_TabSelected;

            // create fragments
            m_ARFragment = new ARFragment();
            m_MapFragment = new MapFragment();
            m_SettingsFragment = new SettingsFragment();
            m_MyMarksFragment = new MyMarksFragment();
            navigateToFragment(m_ARFragment);
        }

        private void TabLayout_TabSelected(object sender, TabLayout.TabSelectedEventArgs e)
        {
            switch(e.Tab.Position)
            {
                case 0:
                    navigateToFragment(m_ARFragment);
                    break;
                case 1:
                    navigateToFragment(m_MapFragment);
                    break;
                case 2:
                    navigateToFragment(m_MyMarksFragment);
                    break;
                case 3:
                    navigateToFragment(m_SettingsFragment);
                    break;
            }
        }

        Android.Support.V4.App.Fragment m_CurrentFragment;

        private void navigateToFragment(Android.Support.V4.App.Fragment fragment)
        {            
            // If the fragment hasn't been added yet - add it
            if (SupportFragmentManager.FindFragmentById(fragment.Id) == null)
            {
                SupportFragmentManager.BeginTransaction().Add(Resource.Id.contentFrame, fragment).Commit();
            }

            // Show the new fragment
            SupportFragmentManager.BeginTransaction().Show(fragment).Commit();

            // Hide the previous fragment
            if (m_CurrentFragment != null && m_CurrentFragment != fragment)
            {
                SupportFragmentManager.BeginTransaction().Hide(m_CurrentFragment).Commit();
            }

            m_CurrentFragment = fragment;
        }

        public override void OnBackPressed()
        {
            //base.OnBackPressed();
        }
    }
}
