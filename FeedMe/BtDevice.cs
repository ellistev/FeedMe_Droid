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
    public class BtDevice
    {
        public string Name;
        public string Type;
        public string MacAddress;
        public int Strength;
        public string Uuid;
		public string UuidString;
		public string Major;
		public int MajorInt;
		public string Minor;
		public int MinorInt;
        
        public BtDevice(){}

		public BtDevice(BluetoothDevice device){
			Name = device.Name;
		}

        public BtDevice(string name, string type, string macAddress, int strength, string uuid)
        {
            this.Name = name;
            this.Type = type;
            this.MacAddress = macAddress;
            this.Strength = strength;
            this.Uuid = uuid;
        }

		public BtDevice(ScannedBleDevice device)
		{
			this.Name = !string.IsNullOrEmpty(device.DeviceName) ? device.DeviceName : "";
			this.Type = device.GetType().ToString();
			this.MacAddress = device.MacAddress;
			this.Strength = int.Parse(device.RSSI.ToString());
			this.Uuid = device.IbeaconProximityUUID.ToString();
			this.UuidString = device.IbeaconProximityUUIDString;
			this.Major = device.Major.ToString();
			this.MajorInt = device.MajorInt;
			this.Minor = device.Minor.ToString();
			this.MinorInt = device.MinorInt;

		}


    }
}