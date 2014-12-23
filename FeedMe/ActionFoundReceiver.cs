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
    public class ActionFoundReceiver : BroadcastReceiver
    {
        private List<BtDevice> btDeviceList = new List<BtDevice>();
		private List<String> btTextList = new List<String> ();
		private Context context;
		private BtDeviceArrayAdapter adapter;
        public BlueToothDiscover mBlueToothDiscover;
		private List<BtDevice> newBtDeviceList;

        public ActionFoundReceiver(){}

        //public ActionFoundReceiver(System.IntPtr whatever, Android.Runtime.JniHandleOwnership stuff){}

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
            //int count = 0;
            //foreach (var btDevice in newBtDeviceList)
            //{
                //blueToothTextView.Text += "\n" + count + ": " + btDevice.Name + ", " + btDevice.Type + ", " + btDevice.MacAddress +
                                          //", " + btDevice.Strength;
                //count++;
            //}
            
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
    }


}