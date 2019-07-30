
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using mARkIt.Authentication;
using mARkIt.Droid.Fragments;

namespace mARkIt.Droid
{
    [Activity(Label = "TabsActivity",ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.KeyboardHidden | Android.Content.PM.ConfigChanges.ScreenSize)]

    public class TabsActivity : Activity
    {
        TabLayout m_TabLayout;
        ARFragment m_ARFragment;
        MapFragment m_MapFragment;
        SettingsFragment m_SettingsFragment;
        
        string m_Email;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // todo, send User object between activities and not only email
            m_Email = Intent.GetStringExtra("Email");

            // Create your application here
            SetContentView(Resource.Layout.Tabs);
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

        private void fragmentNavigate(Fragment fragment)
        {
            var transaction = FragmentManager.BeginTransaction();
            transaction.Replace(Resource.Id.contentFrame, fragment);
            transaction.Commit();
        }

        public override void OnBackPressed()
        {
            // disabling the backbutton original functionality
            //base.OnBackPressed();
        }
    }
}
