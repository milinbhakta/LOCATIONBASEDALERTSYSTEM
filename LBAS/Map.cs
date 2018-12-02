
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using Android.Nfc;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace LBAS
{
    [Activity(Label = "Map", Theme = "@style/MyTheme")]
    public class Map : AppCompatActivity, IOnMapReadyCallback, ILocationListener
    {

        LinearLayout act_map;
        private GoogleMap GMap;
        Location currentLocation;
        LocationManager locationManager;
        string LocationProvider;
        public void OnLocationChanged(Location location)
        {
            currentLocation = location;
            if (currentLocation == null)
            {
                //Error Message  
            }
            else
            {
                GMap.UiSettings.ZoomControlsEnabled = true;
                LatLng latlng = new LatLng(Convert.ToDouble(currentLocation.Latitude), Convert.ToDouble(currentLocation.Longitude));
                CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(latlng, 5);
                GMap.MoveCamera(camera);
                MarkerOptions options = new MarkerOptions().SetPosition(latlng).SetTitle("MyLocation");
                GMap.Clear();
                GMap.AddMarker(options);
            }
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            this.GMap = googleMap;
            //var location = GetLocation(address);
            try
            {
                var geo = new Geocoder(this);
                var addresses = geo.GetFromLocation(currentLocation.Latitude, currentLocation.Longitude, 1);
                GMap.UiSettings.ZoomControlsEnabled = true;
                LatLng latlng = new LatLng(Convert.ToDouble(addresses[0].Latitude), Convert.ToDouble(addresses[0].Longitude));
                CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(latlng, 5);
                GMap.MoveCamera(camera);
                MarkerOptions options = new MarkerOptions().SetPosition(latlng).SetTitle("MyLocation");
                GMap.Clear();
                GMap.AddMarker(options);
            }
            catch (Exception e)
            {
                Console.WriteLine("Map:OnMapReadyExecption" + e.Message);
            }

        }

        public void OnProviderDisabled(string provider)
        {
            throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {

            if (currentLocation == null)
            {
                //Error Message  
            }
            else
            {
                GMap.UiSettings.ZoomControlsEnabled = true;
                LatLng latlng = new LatLng(Convert.ToDouble(currentLocation.Latitude), Convert.ToDouble(currentLocation.Longitude));
                CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(latlng, 5);
                GMap.MoveCamera(camera);
                MarkerOptions options = new MarkerOptions().SetPosition(latlng).SetTitle("MyLocation");
                GMap.Clear();
                GMap.AddMarker(options);
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Map);

            Android.Support.V7.App.ActionBar actionBar = this.SupportActionBar;
            actionBar.SetHomeButtonEnabled(true);
            actionBar.SetDisplayHomeAsUpEnabled(true);
            act_map = FindViewById<LinearLayout>(Resource.Id.linearLayoutMap);
            InitializeLocationManager();
            // Create your application here
            if (GMap == null)
            {
                MapView mMapView = (MapView)FindViewById(Resource.Id.mapView1);
                MapsInitializer.Initialize(this);

                mMapView.OnCreate(savedInstanceState);
                mMapView.OnResume();
                mMapView.GetMapAsync(this);




            }

        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {

            MenuInflater.Inflate(Resource.Menu.menu_alert, menu);
            return base.OnCreateOptionsMenu(menu);
        }


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    StartActivity(new Android.Content.Intent(this, typeof(Dashboard)));
                    Finish();
                    return true;
                case Resource.Id.action_insert:
                    StartActivity(new Android.Content.Intent(this, typeof(ReportAlert)));
                    Finish();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
        private void InitializeLocationManager()
        {
            locationManager = (LocationManager)GetSystemService(LocationService);
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine
            };
            IList<string> acceptableLocationProviders = locationManager.GetProviders(criteriaForLocationService, true);
            if (acceptableLocationProviders.Any())
            {
                LocationProvider = acceptableLocationProviders.First();
            }
            else
            {
                LocationProvider = string.Empty;
            }

        }

        protected override void OnResume()
        {
            base.OnResume();
            locationManager.RequestLocationUpdates(LocationProvider, 0, 0, this);
        }
        protected override void OnPause()
        {
            base.OnPause();
            locationManager.RemoveUpdates(this);
        }
    }
}

