using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.App.Admin;
using Android.Bluetooth;
using Android.Net.Wifi;
using Android.Util;
using Java.Lang;
using Java.Util;
using RadiusNetworks.IBeaconAndroid;
using String = System.String;
using Color = Android.Graphics.Color;
using Android.Support.V4.App;
using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Views;
using Android.Widget;
using Android.OS;
using RadiusNetworks.IBeaconAndroid;
using Android.Support.V4.App;


namespace FeedMe
{
	[Activity(MainLauncher = true, Icon = "@drawable/icon", LaunchMode = LaunchMode.SingleTask)]
	public class MainActivity : Activity, IBeaconConsumer
    {
        private List<BluetoothDevice> btDeviceList = new List<BluetoothDevice>();

		private const string UUID = "f7826da64fa24e988024bc5b71e0893e";
		private const string monkeyId = "Monkey";

		bool _paused;
		View _view;
		IBeaconManager _iBeaconManager;
		MonitorNotifier _monitorNotifier;
		RangeNotifier _rangeNotifier;
		Region _monitoringRegion;
		Region _rangingRegion;
		TextView _text;

		int _previousProximity;

		public MainActivity()
		{
			_iBeaconManager = IBeaconManager.GetInstanceForApplication(this);

			_monitorNotifier = new MonitorNotifier();
			_rangeNotifier = new RangeNotifier();

			_monitoringRegion = new Region(monkeyId, UUID, null, null);
			_rangingRegion = new Region(monkeyId, UUID, null, null);
		}

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
			SetContentView(Resource.Layout.Monkey);

			_view = FindViewById<RelativeLayout>(Resource.Id.findTheMonkeyView);
			_text = FindViewById<TextView>(Resource.Id.monkeyStatusLabel);

			_iBeaconManager.Bind(this);

			_monitorNotifier.EnterRegionComplete += EnteredRegion;
			_monitorNotifier.ExitRegionComplete += ExitedRegion;

			_rangeNotifier.DidRangeBeaconsInRegionComplete += RangingBeaconsInRegion;
            //Button mainBlueToothButton = FindViewById<Button>(Resource.Id.MainBlueToothButton);
            //mainBlueToothButton.Click += (sender, e) =>
           // {
             //   var intent = new Intent(this, typeof(BlueToothDiscover));
            //    StartActivity(intent);
           // };

           // Button mainSettingsButton = FindViewById<Button>(Resource.Id.MainSettingsButton);
          //  mainSettingsButton.Click += (sender, e) =>
         //   {
		//		var intent = new Intent(this, typeof(SettingsActivity));
		//		StartActivity(intent);
           // };
        }

		protected override void OnResume()
		{
			base.OnResume();
			_paused = false;
		}

		protected override void OnPause()
		{
			base.OnPause();
			_paused = true;
		}

		void EnteredRegion(object sender, MonitorEventArgs e)
		{
			if(_paused)
			{
				ShowNotification();
			}	
		}

		void ExitedRegion(object sender, MonitorEventArgs e)
		{
		}

		void RangingBeaconsInRegion(object sender, RangeEventArgs e)
		{
			if (e.Beacons.Count > 0)
			{
				IBeacon beacon = (IBeacon)e.Beacons;
				var message = string.Empty;

				switch((ProximityType)beacon.Proximity)
				{
				case ProximityType.Immediate:
					UpdateDisplay("You found the monkey!", Color.Green);
					break;
				case ProximityType.Near:
					UpdateDisplay("You're getting warmer", Color.Yellow);
					break;
				case ProximityType.Far:
					UpdateDisplay("You're freezing cold", Color.Blue);
					break;
				case ProximityType.Unknown:
					UpdateDisplay("I'm not sure how close you are to the monkey", Color.Red);
					break;
				}

				_previousProximity = beacon.Proximity;
			}
		}

		private void UpdateDisplay(string message, Color color)
		{
			RunOnUiThread(() =>
				{
					_text.Text = message;
					_view.SetBackgroundColor(color);
				});
		}

		private void ShowNotification()
		{
			var resultIntent = new Intent(this, typeof(MainActivity));
			resultIntent.AddFlags(ActivityFlags.ReorderToFront);
			var pendingIntent = PendingIntent.GetActivity(this, 0, resultIntent, PendingIntentFlags.UpdateCurrent);
			var notificationId = Resource.String.monkey_notification;

			var builder = new NotificationCompat.Builder(this)
				//.SetSmallIcon(Resource.Drawable.Xamarin_Icon)
				//.SetContentTitle(this.GetText(Resource.String.app_label))
				.SetContentText(this.GetText(Resource.String.monkey_notification))
				.SetContentIntent(pendingIntent)
				.SetAutoCancel(true);

			var notification = builder.Build();

			var notificationManager = (NotificationManager)GetSystemService(NotificationService);
			notificationManager.Notify(notificationId, notification);
		}

		public void OnIBeaconServiceConnect()
		{
			_iBeaconManager.SetMonitorNotifier(_monitorNotifier);
			_iBeaconManager.SetRangeNotifier(_rangeNotifier);

			_iBeaconManager.StartMonitoringBeaconsInRegion(_monitoringRegion);
			_iBeaconManager.StartRangingBeaconsInRegion(_rangingRegion);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			_monitorNotifier.EnterRegionComplete -= EnteredRegion;
			_monitorNotifier.ExitRegionComplete -= ExitedRegion;

			_rangeNotifier.DidRangeBeaconsInRegionComplete -= RangingBeaconsInRegion;

			_iBeaconManager.StopMonitoringBeaconsInRegion(_monitoringRegion);
			_iBeaconManager.StopRangingBeaconsInRegion(_rangingRegion);
			_iBeaconManager.UnBind(this);
		}
    }




}

