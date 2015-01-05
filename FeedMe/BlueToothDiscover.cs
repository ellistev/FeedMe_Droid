using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Locations;
using Android.Text;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using Android.App.Admin;
using Android.Net.Wifi;
using Android.Text.Method;
using Android.Util;
using Java.Util;


namespace iBeacon_Indexer
{
    [Activity(Label = "BlueToothDiscover")]
	public class BlueToothDiscover : Activity, ILocationListener
    {
        private static int REQUEST_ENABLE_BT = 1;
		BluetoothAdapter btAdapter = BluetoothAdapter.DefaultAdapter;
        //private TextView blueToothTextView;
		private ListView blueToothListView;
        private List<BluetoothDevice> btDeviceList = new List<BluetoothDevice>();
        private ActionFoundReceiver receiver = null;
		private Location _currentLocation;
		private LocationManager _locationManager;
		public string _locationText;
		public string _addressText;
		private String _locationProvider;
		bool locationUpdated = false;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.BlueToothView);
			InitializeLocationManager();

            //blueToothTextView = FindViewById<TextView>(Resource.Id.BlueToothResults);
			blueToothListView = FindViewById<ListView>(Resource.Id.BlueToothResultsListView);

            //blueToothTextView.MovementMethod = new ScrollingMovementMethod();
            //Register the BroadcastReceiver
			receiver = new ActionFoundReceiver(this);
            IntentFilter filter = new IntentFilter(BluetoothDevice.ActionFound);
            filter.AddAction(BluetoothDevice.ActionUuid);
			filter.AddAction(BluetoothAdapter.ActionDiscoveryStarted);
			filter.AddAction(BluetoothAdapter.ActionDiscoveryFinished);

            RegisterReceiver(receiver, filter); // Don't forget to unregister during onDestroy
 
            // Getting the Bluetooth adapter
			btAdapter = BluetoothAdapter.DefaultAdapter;
            //blueToothTextView.Text += "\nAdapter: " + btAdapter;
     
            Button startButton = FindViewById<Button>(Resource.Id.startBlueToothButton);
            startButton.Click += (object sender, EventArgs e) =>
            {
				//blueToothTextView.SetHeight(1200);
				blueToothListView.SetMinimumHeight(0);
				CheckBTState();
				receiver.SortBlueToothList(this);
                //btAdapter.StartDiscovery();
            };

            Button stopButton = FindViewById<Button>(Resource.Id.stopBlueToothButton);
            stopButton.Click += (object sender, EventArgs e) =>
			{
				btAdapter.StopLeScan(receiver);
				receiver.SortBlueToothList(this);
            };

            Button sortButton = FindViewById<Button>(Resource.Id.sortBlueToothButton);
            sortButton.Click += (object sender, EventArgs e) =>
			{
				receiver.PrintFullBlueToothList();
				//blueToothTextView.SetHeight(0);
				blueToothListView.SetMinimumHeight(1200);
				//btAdapter.CancelDiscovery();
				btAdapter.StopLeScan(receiver);
                //receiver.SortBlueToothList(this);
            };


        }

		public bool LocationWasUpdated(){
			return _currentLocation != null;
		}

		protected override void OnResume()
		{
			base.OnResume();
			_locationManager.RequestLocationUpdates(_locationProvider, 0, 0, this);
		}

      protected void onActivityResult(int requestCode, Result resultCode, Intent data) {
        base.OnActivityResult(requestCode, resultCode, data);
        if (requestCode == REQUEST_ENABLE_BT) {
          //CheckBTState();
        }
      }

      protected void onDestroy() {
        base.OnDestroy();

		ActionFoundReceiver receiver = new ActionFoundReceiver(this);
        UnregisterReceiver(receiver);
        //blueToothTextView = FindViewById<TextView>(Resource.Id.BlueToothResults);
          //blueToothTextView.Text += btDeviceList;
      }



		protected override void OnPause()
		{
			base.OnPause();
			_locationManager.RemoveUpdates(this);
		}

		void InitializeLocationManager()
		{
			_locationManager = (LocationManager) this.BaseContext.GetSystemService(LocationService);
			Criteria criteriaForLocationService = new Criteria
			{
				Accuracy = Accuracy.NoRequirement
			};
			IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

			if (acceptableLocationProviders.Any())
			{
				_locationProvider = acceptableLocationProviders.First();
			}
			else
			{
				_locationProvider = String.Empty;
			}
		}

		public string CheckLocation(){
			UpdateLocation ();
			return _addressText;
		}

		public async void UpdateLocation()
		{
			if (_currentLocation == null)
			{
				_addressText = "Can't determine the current address.";
				return;
			}

			Geocoder geocoder = new Geocoder(this);
			IList<Address> addressList = await geocoder.GetFromLocationAsync(_currentLocation.Latitude, _currentLocation.Longitude, 10);

			Address address = addressList.FirstOrDefault();
			if (address != null)
			{
				string deviceAddress = "";
				for (int i = 0; i < address.MaxAddressLineIndex; i++)
				{
					deviceAddress += (address.GetAddressLine(i)) + ", ";
				}
				_addressText = deviceAddress;
			}
			else
			{
				_addressText = "Unable to determine the address.";
			}
		}



		public void OnLocationChanged(Location location)
		{
			_currentLocation = location;
			locationUpdated = true;
			if (_currentLocation == null)
			{
				_locationText = "Unable to determine your location.";
			}
			else
			{
				_locationText = String.Format("{0},{1}", _currentLocation.Latitude, _currentLocation.Longitude);
			}
		}

		public Location GetCurrentLocationObject(){
			return _currentLocation;
		}

		public void OnProviderDisabled(string provider) {}

		public void OnProviderEnabled(string provider) {}

		public void OnStatusChanged(string provider, Availability status, Bundle extras) {}

      private void CheckBTState() {
        // Check for Bluetooth support and then check to make sure it is turned on
        // If it isn't request to turn it on
        // List paired devices
        // Emulator doesn't support Bluetooth and will return null
        if(btAdapter==null) { 
          //blueToothTextView.Text += "\nBluetooth NOT supported. Aborting.";
          return;
        } else {
          if (btAdapter.IsEnabled) {
            	//blueToothTextView.Text += "\nBluetooth is enabled...";

				//btAdapter.StartDiscovery ();

				btAdapter.StartLeScan(receiver);
          } else {
			Intent enableBtIntent = new Intent(BluetoothAdapter.ActionRequestEnable);
            StartActivityForResult(enableBtIntent, REQUEST_ENABLE_BT);
          }
        }
      }

    }

}



   
