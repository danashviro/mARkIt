﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using mARkIt.Droid.Helpers;
using mARkIt.Models;
using Newtonsoft.Json;

namespace mARkIt.Droid.Activities
{
    [Activity(Label = "MarkDetailsActivity")]
    public class MarkDetailsActivity : Activity,IOnMapReadyCallback
    {
        MapFragment m_MapFragment;
        TextView m_MessageTextView;
        TextView m_DateTextView;
        RatingBar m_MarkRatingBar;
        Mark m_Mark;
        GoogleMap m_GoogleMap;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MarkPresentaion);
            findComponents();
            Button button  = FindViewById<Button>(Resource.Id.DeleteButton);
            button.Click += deleteButton_Click;
            string markAsJson =Intent.GetStringExtra("markAsJson");
            m_Mark  = JsonConvert.DeserializeObject<Mark>(markAsJson);
            m_MessageTextView.Text = m_Mark.Message;
            m_DateTextView.Text= m_Mark.createdAt.ToLocalTime().ToLongDateString();
            m_MapFragment.GetMapAsync(this);
        }

        private void findComponents()
        {
            m_MessageTextView = FindViewById<TextView>(Resource.Id.MessageTextView1);
            m_DateTextView = FindViewById<TextView>(Resource.Id.DateTextView1);
            m_MarkRatingBar = FindViewById<RatingBar>(Resource.Id.MarkRatingBar);
            m_MapFragment = FragmentManager.FindFragmentById<MapFragment>(Resource.Id.MapFragment);
            m_MarkRatingBar.Rating = m_Mark.Rating;
        }

        private async void deleteButton_Click(object sender, EventArgs e)
        {
            bool deleted = await Mark.Delete(m_Mark);
            if(deleted)
            {
                Alert.Show("Success", "Mark deleted successfully", this, Finish);
            }
            else
            {
                Alert.Show("Failure", "Mark couldnt be deleted", this);
            }
        }      

        public void OnMapReady(GoogleMap googleMap)
        {
            m_GoogleMap = googleMap;
            mapToMarkLocation();
        }

        private void mapToMarkLocation()
        {
            MarkerOptions marker = new MarkerOptions();
            LatLng position = new LatLng(m_Mark.Latitude, m_Mark.Longitude);
            marker.SetPosition(position);
            marker.SetTitle(m_Mark.Message);
            m_GoogleMap.AddMarker(marker);
            var cameraPosition = new Android.Gms.Maps.Model.CameraPosition.Builder().Target(position).Zoom(16).Bearing(0).Build();
            m_GoogleMap.MoveCamera(CameraUpdateFactory.NewCameraPosition(cameraPosition));
        }
    }
}