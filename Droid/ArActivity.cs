
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Com.Wikitude.Architect;
using Com.Wikitude.Common.Camera;
using Android.Support.V7.Widget;
using Android.Support.V7.App;


namespace mARkIt.Droid
{

    [Activity(Label = "ArActivity", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.KeyboardHidden | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class ArActivity : AppCompatActivity, ILocationListener, ArchitectView.ISensorAccuracyChangeListener
    {
        protected ArchitectView architectView;
        private Location.LocationProvider locationProvider;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            WebView.SetWebContentsDebuggingEnabled(true);


            var config = new ArchitectStartupConfiguration
            {
                LicenseKey = mARkIt.Utils.Keys.WikitudeLicense,
                CameraPosition = CameraSettings.CameraPosition.Back,
                CameraResolution = CameraSettings.CameraResolution.FULLHD1920x1080,
                CameraFocusMode = CameraSettings.CameraFocusMode.Continuous,
                ArFeatures = ArchitectStartupConfiguration.Features.ImageTracking | ArchitectStartupConfiguration.Features.Geo
            };

            architectView = new ArchitectView(this);
            architectView.OnCreate(config);
            
            SetContentView(architectView);
            SetContentView(Resource.Layout.AR);
            var myToolbar = FindViewById<Toolbar>(Resource.Id.mainToolbar);
            SetSupportActionBar(myToolbar);
            SupportActionBar.Title = "mARk-it";
            locationProvider = new Location.LocationProvider(this, this);

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.toolbar_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            string textToShow;
            Intent i;
            if (item.ItemId == Resource.Id.menu_info)
            {
                i = new Intent(this, typeof(TabsActivity));
                StartActivity(i);

            }
            else
                textToShow = "Overfloooow";



            return base.OnOptionsItemSelected(item);
        }
        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            architectView.OnPostCreate();
            architectView.Load("ARPages/ServerPage/index.html");

        }
        protected override void OnResume()
        {
            base.OnResume();
            architectView.OnResume();
     
            /*
             * The SensorAccuracyChangeListener has to be registered to the Architect view after ArchitectView.OnCreate.
             * There may be more than one SensorAccuracyChangeListener.
             */
            architectView.RegisterSensorAccuracyChangeListener(this);
        }

        protected override void OnPause()
        {
            base.OnPause();
            architectView.OnPause();
            locationProvider.Stop();
            // The SensorAccuracyChangeListener has to be unregistered from the Architect view before ArchitectView.onDestroy.
            architectView.UnregisterSensorAccuracyChangeListener(this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            architectView.ClearCache();

            architectView.OnDestroy();
        }

        public virtual void OnLocationChanged(Android.Locations.Location location)
        {
            float accuracy = location.HasAccuracy ? location.Accuracy : 1000;
            if (location.HasAltitude)
            {
                architectView.SetLocation(location.Latitude, location.Longitude, location.Altitude, accuracy);
            }
            else
            {
                architectView.SetLocation(location.Latitude, location.Longitude, accuracy);
            }
        }

        /*
         * The very basic Activity setup of this sample app does not handle the following callbacks
         * to keep the sample app as small as possible. They should be used to handle changes in a production app.
         */
        public void OnProviderDisabled(string provider) { }

        public void OnProviderEnabled(string provider) { }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras) { }


        /*
         * The ArchitectView.ISensorAccuracyChangeListener notifies of changes in the accuracy of the compass.
         * This can be used to notify the user that the sensors need to be recalibrated.
         *
         * This listener has to be registered after OnCreate and unregistered before OnDestroy in the ArchitectView.
         */
        public void OnCompassAccuracyChanged(int accuracy)
        {
            
        }
    }
}
