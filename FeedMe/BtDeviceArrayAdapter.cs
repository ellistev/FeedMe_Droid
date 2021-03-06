﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Bluetooth;
using Android.Content;
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
	public class BtDeviceArrayAdapter : ArrayAdapter<BtDevice>
	{

		public static List<BtDevices> btDeviceList;
		private Activity _activity;


		public BtDeviceArrayAdapter(Activity activity, Context context,int resourceId):base(context,resourceId)//,assets)
		{
			_activity = activity;
			btDeviceList = new List<BtDevices>();
		}

		public static List<BtDevices> getBtDeviceList()
		{
			return btDeviceList;
		}

		public void Add(BtDevices item)
		{
			btDeviceList.Add(item);
		}

		public void AddList(List<BtDevices> list)
		{
			btDeviceList = list;
		}

		public List<BtDevices> GetBlueToothList()
		{
			return btDeviceList;
		}

		public override int Count
		{
			get { return btDeviceList.Count; }
		}

		public BtDevices GetBlueToothListItem(int position)
		{
			return btDeviceList[position];
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var view = convertView ?? _activity.LayoutInflater.Inflate(Resource.Layout.BlueToothListItemView, parent, false);
			var deviceName = view.FindViewById<TextView>(Resource.Id.btListItemName);
			var deviceType = view.FindViewById<TextView>(Resource.Id.btListItemType);
			var deviceStrength = view.FindViewById<TextView>(Resource.Id.btListItemStength);
			var deviceMacAddress = view.FindViewById<TextView>(Resource.Id.btListItemMacAddress);
			var deviceRawData = view.FindViewById<TextView>(Resource.Id.btListItemRaw);
			deviceName.Text = btDeviceList[position].Name;
			deviceType.Text = "";
			deviceStrength.Text = "";
			deviceMacAddress.Text = "";
			BtDevices deviceInQuestion = btDeviceList [position];

			var dump = ObjectDumper.Dump(deviceInQuestion);

			deviceRawData.Text = dump;


			return view;
		}
						
		public override void NotifyDataSetChanged()
		{
			base.NotifyDataSetChanged();
		}

		public override void NotifyDataSetInvalidated()
		{
			base.NotifyDataSetInvalidated();
		}
	}
}

