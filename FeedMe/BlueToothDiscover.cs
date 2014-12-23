using System.Collections.Generic;
using System.Linq;
using System.Text;

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
using Java.Lang;
using Java.Util;


namespace FeedMe
{
    [Activity(Label = "BlueToothDiscover")]
	public class BlueToothDiscover : Activity
    {
        private static int REQUEST_ENABLE_BT = 1;
		BluetoothAdapter btAdapter = BluetoothAdapter.DefaultAdapter;
        private TextView blueToothTextView;
		private ListView blueToothListView;
        private List<BluetoothDevice> btDeviceList = new List<BluetoothDevice>();
        private ActionFoundReceiver receiver = null;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.BlueToothView);

            blueToothTextView = FindViewById<TextView>(Resource.Id.BlueToothResults);
			blueToothListView = FindViewById<ListView>(Resource.Id.BlueToothResultsListView);

            blueToothTextView.MovementMethod = new ScrollingMovementMethod();
            //Register the BroadcastReceiver
            receiver = new ActionFoundReceiver(this);
            IntentFilter filter = new IntentFilter(BluetoothDevice.ActionFound);
            filter.AddAction(BluetoothDevice.ActionUuid);
			filter.AddAction(BluetoothAdapter.ActionDiscoveryStarted);
			filter.AddAction(BluetoothAdapter.ActionDiscoveryFinished);

            RegisterReceiver(receiver, filter); // Don't forget to unregister during onDestroy
 
            // Getting the Bluetooth adapter
			btAdapter = BluetoothAdapter.DefaultAdapter;
            blueToothTextView.Text += "\nAdapter: " + btAdapter;
     
            Button startButton = FindViewById<Button>(Resource.Id.startBlueToothButton);
            startButton.Click += (object sender, EventArgs e) =>
            {
				blueToothTextView.SetHeight(1200);
				blueToothListView.SetMinimumHeight(0);
				CheckBTState();
                //btAdapter.StartDiscovery();
            };

            Button stopButton = FindViewById<Button>(Resource.Id.stopBlueToothButton);
            stopButton.Click += (object sender, EventArgs e) =>
            {
                btAdapter.CancelDiscovery();
            };

            Button sortButton = FindViewById<Button>(Resource.Id.sortBlueToothButton);
            sortButton.Click += (object sender, EventArgs e) =>
            {
				blueToothTextView.SetHeight(0);
				blueToothListView.SetMinimumHeight(1200);
                receiver.SortBlueToothList(this);
            };

            Button fullListButton = FindViewById<Button>(Resource.Id.fulllistBlueToothButton);
            fullListButton.Click += (object sender, EventArgs e) =>
            {
                receiver.PrintFullBlueToothList();
            };

            Button clearButton = FindViewById<Button>(Resource.Id.clearBlueToothListButton);
            clearButton.Click += (object sender, EventArgs e) =>
            {
                btAdapter.CancelDiscovery();
                blueToothTextView.Text = "";
                receiver.ClearBlueToothList();
            };
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
        blueToothTextView = FindViewById<TextView>(Resource.Id.BlueToothResults);
          blueToothTextView.Text += btDeviceList;
      }

      private void CheckBTState() {
        // Check for Bluetooth support and then check to make sure it is turned on
        // If it isn't request to turn it on
        // List paired devices
        // Emulator doesn't support Bluetooth and will return null
        if(btAdapter==null) { 
          blueToothTextView.Text += "\nBluetooth NOT supported. Aborting.";
          return;
        } else {
          if (btAdapter.IsEnabled) {
            blueToothTextView.Text += "\nBluetooth is enabled...";

			btAdapter.StartDiscovery ();
			btAdapter.StartLeScan(receiver);
          } else {
			Intent enableBtIntent = new Intent(BluetoothAdapter.ActionRequestEnable);
            StartActivityForResult(enableBtIntent, REQUEST_ENABLE_BT);
          }
        }
      }

    }

}



   
