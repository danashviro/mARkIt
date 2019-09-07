﻿using System;
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
using Microsoft.AppCenter.Push;

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

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            m_View= inflater.Inflate(Resource.Layout.Settings, container, false);
            return m_View; 
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(m_View, savedInstanceState);
            findComponents();
            addButtonsEvents();
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

        public override void OnResume()
        {
            base.OnResume();
            if(IsHidden==false)
                fillComponents();
        }

        private async void SaveButton_Click(object sender, EventArgs e)
        {
            int catagories = getCaregories();
            Button button = sender as Button;
            button.Clickable = false;
            App.ConnectedUser.RelevantCategoriesCode = catagories;
            bool updated = await User.Update(App.ConnectedUser);
            string msg = updated ? "Upload successfull!" : "Upload failed!";
            Toast.MakeText(Context, msg, ToastLength.Long).Show();
            button.Clickable = true;
        }

        private int getCaregories()
        {
            int categories = 0;
            if (m_GeneralCheckBox.Checked)
            {
                categories |= (int)eCategories.General;
            }
            if (m_FoodCheckBox.Checked)
            {
                categories |= (int)eCategories.Food;
            }
            if (m_SportCheckBox.Checked)
            {
                categories |= (int)eCategories.Sport;
            }
            if (m_HistoryCheckBox.Checked)
            {
                categories |= (int)eCategories.History;
            }
            if (m_NatureCheckBox.Checked)
            {
                categories |= (int)eCategories.Nature;
            }
            return categories;
        }

        private void fillComponents()
        {
            int relevantCategoriesCode = App.ConnectedUser.RelevantCategoriesCode;

            m_GeneralCheckBox.Checked = (relevantCategoriesCode & (int)eCategories.General) != 0 ? true : false;
            m_FoodCheckBox.Checked = (relevantCategoriesCode & (int)eCategories.Food) != 0 ? true : false;
            m_HistoryCheckBox.Checked = (relevantCategoriesCode & (int)eCategories.History) != 0 ? true : false;
            m_SportCheckBox.Checked = (relevantCategoriesCode & (int)eCategories.Sport) != 0 ? true : false;
            m_NatureCheckBox.Checked = (relevantCategoriesCode & (int)eCategories.Nature) != 0 ? true : false;
        }

        private async void LogoutButton_Click(object sender, EventArgs e)
        {
            // remove account from device
            await LoginHelper.Logout();

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