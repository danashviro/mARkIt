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
using mARkIt.Droid.Adapters;
using mARkIt.Services;

namespace mARkIt.Droid.Fragments
{
    public class MyMarksFragment : Android.Support.V4.App.ListFragment
    {
        public override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here 
            var locations = await LocationService.Instance().GetLocations();
            List<Models.Location> locationList = new List<Models.Location>(locations);

            ListAdapter = new LocationAdapter(Context,locationList);

         }
    }
}