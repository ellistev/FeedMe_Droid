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


namespace iBeacon_Indexer
{
	[Activity(Label = "Settings")]
	public class SettingsActivity : Activity
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
			SetContentView(Resource.Layout.SettingsView);

			Button refreshDatabaseButton = FindViewById<Button>(Resource.Id.SettingsRefreshDatabaseButton);
			refreshDatabaseButton.Click += (object sender, EventArgs e) =>
			{
				DatabaseFunctions database = new DatabaseFunctions();
				database.RefreshDatabase(this.BaseContext);
			};

		}

		protected void onDestroy() {
			base.OnDestroy();

		}

	}

}




