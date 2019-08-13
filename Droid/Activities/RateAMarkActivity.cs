﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using mARkIt.Droid.Helpers;
using mARkIt.Models;

namespace mARkIt.Droid.Activities
{
    [Activity(Label = "RateAMarkActivity")]
    public class RateAMarkActivity : Activity
    {
        private RatingBar m_MarkRatingBar;
        private RatingBar m_YourRatingBar;
        private string m_MarkId;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.RateAMark);
            m_MarkRatingBar = FindViewById<RatingBar>(Resource.Id.MarkRatingBar);
            m_YourRatingBar = FindViewById<RatingBar>(Resource.Id.YourRatingBar);
            m_MarkId = Intent.GetStringExtra("markId");
            Button saveButton = FindViewById<Button>(Resource.Id.SaveButton);
            saveButton.Click += SaveButton_Click;
        }

        private async void SaveButton_Click(object sender, EventArgs e)
        {
            bool succeded = await User.RateMark(App.ConnectedUser.Email, m_MarkId, m_YourRatingBar.Rating);
            
            if(succeded)
            {
                Alert.Show("Success", "Your rating has been submited.",this);
            }
            else
            {
                Alert.Show("Faliure", "Something went wrong.", this);
            }
        }
    }
}