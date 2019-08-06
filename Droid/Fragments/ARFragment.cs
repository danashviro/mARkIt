
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
using Android.Locations;



using Android.Webkit;
using Com.Wikitude.Architect;
using Com.Wikitude.Common.Camera;
using Android.Support.Design.Widget;
using Org.Json;

namespace mARkIt.Droid.Fragments
{
    public class ARFragment : Android.Support.V4.App.Fragment, ILocationListener, ArchitectView.ISensorAccuracyChangeListener,IArchitectJavaScriptInterfaceListener
    {
        public readonly static string IntentExtrasKeyExperienceData = "ExperienceData";
        private Location.LocationProvider locationProvider;

        protected ArchitectView architectView;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            locationProvider = new Location.LocationProvider(Context, this);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            WebView.SetWebContentsDebuggingEnabled(true);

            architectView = new ArchitectView(Context);
            return architectView;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            //var experience = ArExperience.Deserialize(Arguments.GetByteArray(IntentExtrasKeyExperienceData));

            var arExperiencePath = "ARPages/ServerPage/index.html";

            var config = new ArchitectStartupConfiguration
            {
                LicenseKey = mARkIt.Utils.Keys.WikitudeLicense,
                CameraPosition = CameraSettings.CameraPosition.Back,
                CameraResolution = CameraSettings.CameraResolution.FULLHD1920x1080,
                CameraFocusMode = CameraSettings.CameraFocusMode.Continuous,
                ArFeatures = ArchitectStartupConfiguration.Features.ImageTracking | ArchitectStartupConfiguration.Features.Geo
            };

            //var config = new ArchitectStartupConfiguration
            //{
            //    LicenseKey = mARkIt.Utils.Keys.WikitudeLicense,
            //    CameraPosition = Util.PlatformConverter.ConvertSharedToPlatformPosition(experience.CameraPosition),
            //    CameraResolution = Util.PlatformConverter.ConvertSharedToPlatformResolution(experience.CameraResolution),
            //    CameraFocusMode = Util.PlatformConverter.ConvertSharedToPlatformFocusMode(experience.CameraFocusMode),
            //    Camera2Enabled = experience.Camera2Enabled,
            //    ArFeatures = (int)experience.FeaturesMask
            //};

            architectView.OnCreate(config);
            architectView.OnPostCreate();
            architectView.Load(arExperiencePath);
            architectView.AddArchitectJavaScriptInterfaceListener(this);
            //FloatingActionButton floatingActionButton = new FloatingActionButton(Context);
            //architectView.AddView(floatingActionButton);
            //floatingActionButton.Bottom += 300;
        }

        public override void OnResume()
        {
            base.OnResume();
            architectView.OnResume();
            if (!locationProvider.Start())
            {
                Toast.MakeText(Context, "Could not start Location updates. Make sure that locations and location providers are enabled and Runtime Permissions are granted.", ToastLength.Long).Show();
            }
            architectView.RegisterSensorAccuracyChangeListener(this);
        }

        public override void OnPause()
        {
            base.OnPause();
            architectView.OnPause();
            locationProvider.Stop();
            architectView.UnregisterSensorAccuracyChangeListener(this);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            architectView.ClearCache();
            architectView.OnDestroy();
        }

        public void OnLocationChanged(Android.Locations.Location location)
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

        public void OnProviderDisabled(string provider) { }

        public void OnProviderEnabled(string provider) { }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras) { }

        public void OnCompassAccuracyChanged(int accuracy)
        {
            
        }

        public void OnJSONObjectReceived(JSONObject p0)
        {
            Intent C = new Intent(Activity, typeof(LoginActivity));
            StartActivity(C);
        }
    }
}
