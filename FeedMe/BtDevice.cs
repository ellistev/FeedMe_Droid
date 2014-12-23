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


    }
}