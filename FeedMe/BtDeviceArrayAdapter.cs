using System.Collections.Generic;
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

namespace FeedMe
{
	public class BtDeviceArrayAdapter : ArrayAdapter<BtDevice>
	{

		public static List<BtDevice> btDeviceList;
		private Activity _activity;


		public BtDeviceArrayAdapter(Activity activity, Context context,int resourceId):base(context,resourceId)//,assets)
		{
			_activity = activity;
			btDeviceList = new List<BtDevice>();
		}

		public static List<BtDevice> getBtDeviceList()
		{
			return btDeviceList;
		}

		public void Add(BtDevice item)
		{
			btDeviceList.Add(item);
		}

		public void AddList(List<BtDevice> list)
		{
			btDeviceList = list;
		}

		public List<BtDevice> GetBlueToothList()
		{
			return btDeviceList;
		}

		public override int Count
		{
			get { return btDeviceList.Count; }
		}

		public BtDevice GetBlueToothListItem(int position)
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
			deviceName.Text = btDeviceList[position].Name;
			deviceType.Text = btDeviceList[position].Type;
			deviceStrength.Text = btDeviceList[position].Strength.ToString();
			deviceMacAddress.Text = btDeviceList[position].MacAddress;

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

