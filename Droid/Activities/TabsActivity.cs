using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using mARkIt.Authentication;
using mARkIt.Droid.Fragments;
using System;

namespace mARkIt.Droid
{
    [Activity(Label = "TabsActivity", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.KeyboardHidden | Android.Content.PM.ConfigChanges.ScreenSize)]

    public class TabsActivity : FragmentActivity
    {
        TabLayout m_TabLayout;
        private ARFragment m_ARFragment;
        private MapFragment m_MapFragment;
        private SettingsFragment m_SettingsFragment;
        
        string m_Email;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Tabs);

            // todo, send User object between activities and not only email
            //m_Email = Intent.GetStringExtra("Email");

            m_TabLayout = FindViewById<TabLayout>(Resource.Id.mainTabLayout);
            m_TabLayout.TabSelected += TabLayout_TabSelected;

            // create fragments
            m_ARFragment = new ARFragment();
            m_MapFragment = new MapFragment();
            m_SettingsFragment = new SettingsFragment();
            fragmentNavigate(m_ARFragment);
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
