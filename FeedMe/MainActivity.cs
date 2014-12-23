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
using String = System.String;

namespace FeedMe
{
    [Activity(MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private static readonly List<string> phoneNumbers = new List<string>();
        private WifiManager wifi;
        private List<BluetoothDevice> btDeviceList = new List<BluetoothDevice>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            Button mainBlueToothButton = FindViewById<Button>(Resource.Id.MainBlueToothButton);
            mainBlueToothButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(BlueToothDiscover));
                StartActivity(intent);
            };

            Button mainSettingsButton = FindViewById<Button>(Resource.Id.MainSettingsButton);
            mainSettingsButton.Click += (sender, e) =>
            {
                ConnectBluetooth("Basis B1");
            };
        }

        private bool ConnectBluetooth(string sDeviceName)
        {

            try
            {
                BluetoothAdapter oBblue = BluetoothAdapter.DefaultAdapter;
                System.Boolean isEnabled = oBblue.IsEnabled;

                if (oBblue.State == Android.Bluetooth.State.Off)
                {
                    return false;
                }

                if (string.IsNullOrEmpty(oBblue.Name))
                {
                    return false;
                }
                ICollection<BluetoothDevice> devices = oBblue.BondedDevices;
                if (devices != null)
                {
                    if (devices.Count >= 1)
                    {
                        foreach (BluetoothDevice Dev in devices)
                        {
                            if (Dev.Name == sDeviceName)
                            {
                                try
                                {
                                    BluetoothSocket socket = Dev.CreateRfcommSocketToServiceRecord(Dev.GetUuids()[0].Uuid);

                                    if (!socket.IsConnected)
                                    {
                                        socket.Connect();
                                    }
                                    return socket.IsConnected;
                                }
                                catch (System.Exception ex)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }

                return false;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }
    }




}

