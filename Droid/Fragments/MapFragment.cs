
using mARkIt.Services;
using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Locations;
using Android.Gms.Maps.Model;

namespace mARkIt.Droid.Fragments
{
    public class MapFragment : Fragment, IOnMapReadyCallback,ILocationListener
    {
        MapView m_MapView;
        GoogleMap m_GoogleMap;
        View m_View;
        double m_Latitude, m_Longitude;
        LocationManager m_LocationManager;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            m_MapView = m_View.FindViewById<MapView>(Resource.Id.mapView);
            if(m_MapView!=null)
            {
                m_MapView.OnCreate(null);
                m_MapView.OnResume();
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false); we need this
            //  setUpMap();
            m_View = inflater.Inflate(Resource.Layout.Map, container, false);
            return m_View;
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            MapsInitializer.Initialize(Context);
            m_GoogleMap = googleMap;
            googleMap.MapType = GoogleMap.MapTypeNormal;
            mapToMyLocation();
            addMarksFromServer();
        }

        private async void addMarksFromServer()
        {
            var locations = await LocationService.Instance().GetLocations();
            foreach (mARkIt.Models.Location location in locations)
            {
                MarkerOptions marker = new MarkerOptions();
                marker.SetPosition(new LatLng(location.latitude, location.longitude));
                marker.SetTitle(location.message);
                m_GoogleMap.AddMarker(marker);
            }
        }

        private void mapToMyLocation()
        {
            MarkerOptions marker = new MarkerOptions();
            LatLng position = new LatLng(m_Latitude, m_Longitude);
            marker.SetPosition(position);
            marker.SetTitle("Your location");
            marker.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueBlue));
            m_GoogleMap.AddMarker(marker);
            var cameraPosition = new Android.Gms.Maps.Model.CameraPosition.Builder().Target(position).Zoom(16).Bearing(0).Build();
            m_GoogleMap.MoveCamera(CameraUpdateFactory.NewCameraPosition(cameraPosition));
        }

        public void OnLocationChanged(Android.Locations.Location location)
        {
            m_Latitude = location.Latitude;
            m_Longitude = location.Longitude;
            m_MapView.GetMapAsync(this);
        }

        public void OnProviderDisabled(string provider)
        {
        }

        public void OnProviderEnabled(string provider)
        {
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
        }

        public override void OnPause()
        {
            base.OnPause();
            m_LocationManager.RemoveUpdates(this);
        }

        public override void OnResume()
        {
            base.OnResume();
            m_LocationManager = Activity.GetSystemService(Context.LocationService) as LocationManager;
            string provider = LocationManager.GpsProvider;
            if (m_LocationManager.IsProviderEnabled(provider))
            {
                m_LocationManager.RequestLocationUpdates(provider, 5000, 100, this);
            }
        }
    }
}
