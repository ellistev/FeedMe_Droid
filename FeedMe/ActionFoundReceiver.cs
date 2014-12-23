using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Widget;
using Java.Lang;
using Java.Util;
using String = System.String;

namespace FeedMe
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

        public ActionFoundReceiver(){}

        public ActionFoundReceiver(BlueToothDiscover activity)
        {
            mBlueToothDiscover = activity;
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
            TextView blueToothTextView = mBlueToothDiscover != null ? mBlueToothDiscover.FindViewById<TextView>(Resource.Id.BlueToothResults) : null;

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

			BtDevice btDevice = new BtDevice(parsedLEDevice);
			btDeviceList.Add(btDevice);

			Console.WriteLine ("LeScanCallback: " + device.Name);
		}

		// use this method to parse those bytes and turn to an object which defined proceeding.
		// the uuidMatcher works as a UUID filter, put null if you want parse any BLE advertising data around.
		private ScannedBleDevice ParseRawScanRecord(BluetoothDevice device,
			int rssi, byte[] advertisedData, byte[] uuidMatcher) {
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

				byte[] major = new byte[2];
				major[0] = magic[22];
				major[1] = magic[23];
				parsedObj.Major = major;

				byte[] minor = new byte[2];
				minor[0] = magic[24];
				minor[1] = magic[25];
				parsedObj.Minor = minor;

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