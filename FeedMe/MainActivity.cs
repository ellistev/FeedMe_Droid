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

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            //var intent = new Intent(this, typeof(BlueToothDiscover));
            //StartActivity(intent);

            // Our code will go here

            // Get our UI controls from the loaded layout
            //EditText phoneNumberText = FindViewById<EditText>(Resource.Id.PhoneNumberText);
            //
            //Button callButton = FindViewById<Button>(Resource.Id.CallButton);
            //Button callHistoryButton = FindViewById<Button>(Resource.Id.CallHistoryButton);
            //TextView ssidList = FindViewById<TextView>(Resource.Id.SSIDList);





            // Disable the "Call" button
            //callButton.Enabled = false;

            // Add code to translate number
            //string translatedNumber = string.Empty;

            //ConnectBluetooth("Basis B1");



            //translateButton.Click += (object sender, EventArgs e) =>
            //{

            //    var intent = new Intent(this, typeof(BlueToothDiscover));
            //    intent.PutStringArrayListExtra("phone_numbers", phoneNumbers);
            //    StartActivity(intent);

            //    //// Translate user’s alphanumeric phone number to numeric
            //    //translatedNumber = Core.FeedMeTranslator.ToNumber(phoneNumberText.Text);
            //    //if (String.IsNullOrWhiteSpace(translatedNumber))
            //    //{
            //    //    callButton.Text = "Call";
            //    //    callButton.Enabled = false;
            //    //}
            //    //else
            //    //{
            //    //    callButton.Text = "Call " + translatedNumber;
            //    //    callButton.Enabled = true;
            //    //}
            //};

            //callButton.Click += (object sender, EventArgs e) =>
            //{
            //    // On "Call" button click, try to dial phone number.
            //    var callDialog = new AlertDialog.Builder(this);
            //    callDialog.SetMessage("Call " + translatedNumber + "?");
            //    callDialog.SetNeutralButton("Call", delegate
            //    {
            //        // add dialed number to list of called numbers.
            //        phoneNumbers.Add(translatedNumber);
            //        // enable the Call History button
            //        callHistoryButton.Enabled = true;

            //        // Create intent to dial phone
            //        var callIntent = new Intent(Intent.ActionCall);
            //        callIntent.SetData(Android.Net.Uri.Parse("tel:" + translatedNumber));
            //        StartActivity(callIntent);
            //    });
            //    callDialog.SetNegativeButton("Cancel", delegate { });

            //    // Show the alert dialog to the user and wait for response.
            //    callDialog.Show();
            //};

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
                                    //BluetoothSocket socket =Dev.CreateInsecureRfcommSocketToServiceRecord (applicationUUID);

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

