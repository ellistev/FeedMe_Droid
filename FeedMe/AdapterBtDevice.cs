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

	public class AdapterBtDevice : ArrayAdapter<BtDevice> {
		private Activity activity;
		private List<BtDevice> lBtDevices;
		private static LayoutInflater inflater = null;

		public AdapterBtDevice (Activity activity, int textViewResourceId,List<BtDevice> _lBtDevices) {
			super(activity, textViewResourceId, _lBtDevices);
			try {
				this.activity = activity;
				this.lBtDevices = _lBtDevices;

				inflater = (LayoutInflater) activity.getSystemService(Context.LAYOUT_INFLATER_SERVICE);

			} catch (Exception e) {

			}
		}

		public int getCount() {
			return lBtDevice.size();
		}

		public BtDevice getItem(BtDevice position) {
			return position;
		}

		public long getItemId(int position) {
			return position;
		}

		public class ViewHolder {
			public TextView display_name;
			public TextView display_number;             

		}

		public View getView(int position, View convertView, ViewGroup parent) {
			View vi = convertView;
			final ViewHolder holder;
			try {
				if (convertView == null) {
					vi = inflater.inflate(R.layout.yourlayout, null);
					holder = new ViewHolder();

					holder.display_name = (TextView) vi.findViewById(R.id.display_name);
					holder.display_number = (TextView) vi.findViewById(R.id.display_number);


					vi.setTag(holder);
				} else {
					holder = (ViewHolder) vi.getTag();
				}



				holder.display_name.setText(lProducts.get(position).name);
				holder.display_number.setText(lProducts.get(position).number);


			} catch (Exception e) {


			}
			return vi;
		}
	}
}

