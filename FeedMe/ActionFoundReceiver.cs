using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Widget;
using Java.Lang;
using Java.Util;
using String = System.String;
using Resource = Android.Resource;
using SQLite;
using Android.Locations;

namespace iBeacon_Indexer
{
    [BroadcastReceiver]
    [IntentFilter(new string[] { BluetoothDevice.ActionFound, BluetoothDevice.ActionUuid, BluetoothAdapter.ActionDiscoveryFinished, BluetoothAdapter.ActionDiscoveryFinished },
        Priority = Int32.MaxValue)]
	public class ActionFoundReceiver : BroadcastReceiver, BluetoothAdapter.ILeScanCallback
    {
        private List<BtDevice> btDeviceList = new List<BtDevice>();
		private List<String> btTextList = new List<String> ();
		private Context context;
		private BtDeviceArrayAdapter adapter;
        public BlueToothDiscover mBlueToothDiscover;
		private List<BtDevice> newBtDeviceList;
		public TextView blueToothTextView;
		private DatabaseFunctions database;
        public ActionFoundReceiver(){}

		public ActionFoundReceiver(BlueToothDiscover activity)
        {
            mBlueToothDiscover = activity;
			context = context;
			database = new DatabaseFunctions(activity.BaseContext);
        }

        public void ClearBlueToothList()
        {
            btDeviceList.Clear();
			btTextList.Clear ();
        }

        public void PrintFullBlueToothList()
        {
            TextView blueToothTextView = mBlueToothDiscover != null ? mBlueToothDiscover.FindViewById<TextView>(Resource.Id.BlueToothResults) : null;
            blueToothTextView.Text = "";
            int count = 0;
            foreach (var btDevice in btDeviceList)
            {
                blueToothTextView.Text += "\n" + count + ": " + btDevice.Name + ", " + btDevice.Type + ", " + btDevice.MacAddress +
                                          ", " + btDevice.Strength;
                count++;
            }
        }

		public void SortBlueToothList(Activity activity)
        {
            TextView blueToothTextView = mBlueToothDiscover != null ? mBlueToothDiscover.FindViewById<TextView>(Resource.Id.BlueToothResults) : null;


			ListView blueToothListView = mBlueToothDiscover.FindViewById<ListView>(Resource.Id.BlueToothResultsListView);

			blueToothListView.ItemClick += blueToothListView_ItemClick;

			newBtDeviceList = btDeviceList.OrderByDescending(o => o.Strength).GroupBy(i => i.MacAddress).Select(g => g.First()).ToList();

			adapter = new BtDeviceArrayAdapter(activity, context, Android.Resource.Layout.SimpleListItem1);
			adapter.AddList (newBtDeviceList);
			blueToothListView.Adapter = adapter; 

            blueToothTextView.Text = "";

        }

		void blueToothListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			BtDevice device= adapter.GetBlueToothListItem(e.Position);

			device.MacAddress = "";

			adapter.NotifyDataSetChanged ();

		}

		public List<String> getPopulatedList(List<BtDevice> newBtDeviceList) {
			List<String> myList = new List<String>();
			foreach(var btDevice in newBtDeviceList){
				myList.Add(btDevice.Name + ", " + btDevice.Type + ", " + btDevice.MacAddress +	", " + btDevice.Strength);
			}

			return myList; 
		}

        public override void OnReceive(Context context, Intent intent)
        {
            String action = intent.Action;
			this.context = context;
            blueToothTextView = mBlueToothDiscover != null ? mBlueToothDiscover.FindViewById<TextView>(Resource.Id.BlueToothResults) : null;

            if (BluetoothDevice.ActionFound == action && blueToothTextView != null)
            {
                BluetoothDevice device = (BluetoothDevice) intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);

				int rssi = intent.GetShortExtra(BluetoothDevice.ExtraRssi, Short.MinValue);
				string uuid = (string)intent.GetParcelableExtra(BluetoothDevice.ExtraUuid);

                blueToothTextView.Text += "\n  Device: " + device.Name + ", " + device.Type + ", " + rssi + ", " + device;
                BtDevice btDevice = new BtDevice();
                btDevice.Name = device.Name;
                btDevice.Type = device.Type.ToString();
                btDevice.Strength = rssi;
                btDevice.MacAddress = device.ToString();
                btDevice.Uuid = uuid;
                btDeviceList.Add(btDevice);
            }
            else
            {
                if (BluetoothDevice.ActionPairingRequest == action)
                {
                    BluetoothDevice device = (BluetoothDevice) intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);
                    IParcelable[] uuidExtra = intent.GetParcelableArrayExtra(BluetoothDevice.ExtraUuid);
                    for (int i = 0; i < uuidExtra.Length; i++)
                    {
                        blueToothTextView.Text += "\n  Device: " + device.Name + ", " + device + ", Service: " +
                                                  uuidExtra[i].ToString();
                    }
                }
                else
                {
                    if (BluetoothAdapter.ActionDiscoveryStarted == action)
                    {
                        blueToothTextView.Text += "\nDiscovery Started...";
                    }
                }
            }

            if (BluetoothAdapter.ActionDiscoveryFinished == action && blueToothTextView != null)
            {
                blueToothTextView.Text += "\nDiscovery Stopped For Some Reason...";
                
            }
        }

		public void OnLeScan (BluetoothDevice device, int rssi, byte[] scanRecord)
		{
			ScannedBleDevice parsedLEDevice = ParseRawScanRecord (device, rssi, scanRecord, null);

			if (parsedLEDevice != null) {
				blueToothTextView = mBlueToothDiscover != null ? mBlueToothDiscover.FindViewById<TextView>(Resource.Id.BlueToothResults) : null;
				BtDevice btDevice = new BtDevice (parsedLEDevice);
				int newBtDeviceId;

				//get gps location and 
				string location = mBlueToothDiscover.CheckLocation ();
				Locations locationName = database.GetLocationName (btDevice.MajorInt, btDevice.MinorInt);

				//if (location != "Can't determine the current address." && location != "Unable to determine your location.") {
				blueToothTextView.Text += "\n Device found: " + btDevice.MajorInt + ":" + btDevice.MinorInt + " You are at " + locationName.Name + "(" + location + ")";


				btDeviceList.Add (btDevice);

				//only add if new, update if changed
				Location currentLocation = mBlueToothDiscover.GetCurrentLocationObject ();
				BtDevices deviceAlreadyExists = database.GetBtDevice (btDevice.UuidString, btDevice.MajorInt, btDevice.MinorInt, btDevice.MacAddress);
				if (deviceAlreadyExists == null) {
					GPSLocation newGpsLocation = new GPSLocation();
					newGpsLocation.LatitudeLongitude = String.Format ("{0},{1}", currentLocation != null ? currentLocation.Latitude.ToString() : "", currentLocation != null ? currentLocation.Longitude.ToString() : "");
					newGpsLocation.Address = location;
					newGpsLocation.Altitude = currentLocation != null ? currentLocation.Altitude.ToString() : "";
					newBtDeviceId = database.AddNewBtDevice (btDevice, newGpsLocation);
				} else {
					GPSLocation updatedGpsLocation = new GPSLocation();
					updatedGpsLocation.LatitudeLongitude = String.Format ("{0},{1}", currentLocation != null ? currentLocation.Latitude.ToString() : "", currentLocation != null ? currentLocation.Longitude.ToString() : "");
					updatedGpsLocation.Address = location;
					updatedGpsLocation.Altitude = currentLocation != null ? currentLocation.Altitude.ToString() : "";
					newBtDeviceId = database.UpdateBtDevice (btDevice, updatedGpsLocation);
				}

				//}

			}

		}

		static char[] hexArray = "0123456789ABCDEF".ToCharArray();
		private static String bytesToHex(byte[] bytes) {
			char[] hexChars = new char[bytes.Length * 2];
			for ( int j = 0; j < bytes.Length; j++ ) {
				int v = bytes[j] & 0xFF;
				hexChars[j * 2] = hexArray[v >> 4];
				hexChars[j * 2 + 1] = hexArray[v & 0x0F];
			}
			return new String(hexChars);
		}






		// use this method to parse those bytes and turn to an object which defined proceeding.
		// the uuidMatcher works as a UUID filter, put null if you want parse any BLE advertising data around.
		private ScannedBleDevice ParseRawScanRecord(BluetoothDevice device, int rssi, byte[] advertisedData, byte[] uuidMatcher) {

			int startByte = 2;
			try {
				ScannedBleDevice parsedObj = new ScannedBleDevice();
				// parsedObj.BLEDevice = device;
				parsedObj.DeviceName = device.Name;
				parsedObj.MacAddress = device.Address;
				parsedObj.RSSI = rssi;
				List<UUID> uuids = new List<UUID>();
				int skippedByteCount = advertisedData[0];
				int magicStartIndex = skippedByteCount + 1;
				int magicEndIndex = magicStartIndex
					+ advertisedData[magicStartIndex] + 1;
				List<byte> magic = new List<byte>();
				for (int i = magicStartIndex; i < magicEndIndex; i++) {
					magic.Add(advertisedData[i]);
				}

				byte[] companyId = new byte[2];
				companyId[0] = magic[2];
				companyId[1] = magic[3];
				parsedObj.CompanyId = companyId;

				byte[] ibeaconProximityUUID = new byte[16];
				for (int i = 0; i < 16; i++) {
					if(magic.Count > (i+6)){
						ibeaconProximityUUID[i] = magic[i + 6];
					}
				}

				parsedObj.IbeaconProximityUUID = ibeaconProximityUUID;

				String hexString = bytesToHex(ibeaconProximityUUID);

				//Here is your UUID
				parsedObj.IbeaconProximityUUIDString =  hexString.Substring(0,8) + "-" + 
					hexString.Substring(8,4) + "-" + 
					hexString.Substring(12,4) + "-" + 
					hexString.Substring(16,4) + "-" + 
					hexString.Substring(20,12);


				byte[] major = new byte[2];
				major[0] = magic[22];
				major[1] = magic[23];
				parsedObj.Major = major;
				parsedObj.MajorInt  = (advertisedData[startByte+23] & 0xff) * 0x100 + (advertisedData[startByte+24] & 0xff);

				byte[] minor = new byte[2];
				minor[0] = magic[24];
				minor[1] = magic[25];
				parsedObj.Minor = minor;
				parsedObj.MinorInt =  (advertisedData[startByte+25] & 0xff) * 0x100 + (advertisedData[startByte+26] & 0xff);


				//Here is your Minor value
				//parsedObj.MinorInt = (advertisedData[startByte+22] & 0xff) * 0x100 + (advertisedData[startByte+23] & 0xff);

				byte tx = 0;
				tx = magic[26];
				parsedObj.Tx = tx;

				parsedObj.ScannedTime = new Date().Time;
				return parsedObj;
			} catch (System.Exception ex) {

				// Log.e(LOG_TAG,
				// "Exception in ParseRawScanRecord with advertisedData: "
				// + Util.BytesToHexString(advertisedData, " ")
				// + ", detail: " + ex.getMessage());
				return null;
			}
		}
			
    }

}