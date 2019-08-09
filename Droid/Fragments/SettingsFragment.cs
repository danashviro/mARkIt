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
using mARkIt.Utils;

namespace mARkIt.Droid.Fragments
{
    public class SettingsFragment : Android.Support.V4.App.Fragment
    {
        View m_View;
        private CheckBox m_GeneralCheckBox;
        private CheckBox m_FoodCheckBox;
        private CheckBox m_SportCheckBox;
        private CheckBox m_HistoryCheckBox;
        private CheckBox m_NatureCheckBox;

        //private RatingBar m_RatingBar;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            m_View= inflater.Inflate(Resource.Layout.Settings, container, false);
            return m_View; 
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(m_View, savedInstanceState);
            addButtonsEvents();
            findComponents();
            //OnHiddenChanged is not called when view is created so fillcomponents must be called manually
            fillComponents();
        }

        private void addButtonsEvents()
        {
            Button logoutButton = m_View.FindViewById<Button>(Resource.Id.logout_button);
            Button saveButton = m_View.FindViewById<Button>(Resource.Id.SaveButton);
            logoutButton.Click += LogoutButton_Click;
            saveButton.Click += SaveButton_Click;
        }

        public override void OnHiddenChanged(bool hidden)
        {
            base.OnHiddenChanged(hidden);
            if(hidden==false)
                fillComponents();

        }

        private async void SaveButton_Click(object sender, EventArgs e)
        {
            int catagories = 0;
            if(m_GeneralCheckBox.Checked)
            {
                catagories |= (int)eCategories.General;
            }
            if (m_FoodCheckBox.Checked)
            {
                catagories |= (int)eCategories.Food;
            }
            if (m_SportCheckBox.Checked)
            {
                catagories |= (int)eCategories.Sport;
            }
            if (m_HistoryCheckBox.Checked)
            {
                catagories |= (int)eCategories.History;
            }
            if (m_NatureCheckBox.Checked)
            {
                catagories |= (int)eCategories.Nature;
            }
            App.User.RelevantCategoriesCode = catagories;
            await User.Update(App.User);
            Toast.MakeText(Context, "Upload successfull.", ToastLength.Long).Show();
        }

        private void fillComponents()
        {
            int relevantCategoriesCode = App.User.RelevantCategoriesCode;

            m_GeneralCheckBox.Checked = (relevantCategoriesCode & (int)eCategories.General) != 0 ? true : false;
            m_FoodCheckBox.Checked = (relevantCategoriesCode & (int)eCategories.Food) != 0 ? true : false;
            m_HistoryCheckBox.Checked = (relevantCategoriesCode & (int)eCategories.History) != 0 ? true : false;
            m_SportCheckBox.Checked = (relevantCategoriesCode & (int)eCategories.Sport) != 0 ? true : false;
            m_NatureCheckBox.Checked = (relevantCategoriesCode & (int)eCategories.Nature) != 0 ? true : false;
            //m_RatingBar.Rating=m_User.Rating
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

        private void findComponents()
        {
            m_GeneralCheckBox = m_View.FindViewById<CheckBox>(Resource.Id.GeneralCheckBox);
            m_FoodCheckBox = m_View.FindViewById<CheckBox>(Resource.Id.FoodCheckBox);
            m_HistoryCheckBox = m_View.FindViewById<CheckBox>(Resource.Id.HistoryCheckBox);
            m_SportCheckBox = m_View.FindViewById<CheckBox>(Resource.Id.SportCheckBox);
            m_NatureCheckBox = m_View.FindViewById<CheckBox>(Resource.Id.NatureCheckBox);
        }

    }
}