package ibeacon_indexer;


public class ActionFoundReceiver
	extends android.content.BroadcastReceiver
	implements
		mono.android.IGCUserPeer,
		android.bluetooth.BluetoothAdapter.LeScanCallback
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onReceive:(Landroid/content/Context;Landroid/content/Intent;)V:GetOnReceive_Landroid_content_Context_Landroid_content_Intent_Handler\n" +
			"n_onLeScan:(Landroid/bluetooth/BluetoothDevice;I[B)V:GetOnLeScan_Landroid_bluetooth_BluetoothDevice_IarrayBHandler:Android.Bluetooth.BluetoothAdapter/ILeScanCallbackInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("iBeacon_Indexer.ActionFoundReceiver, iBeacon_Indexer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ActionFoundReceiver.class, __md_methods);
	}


	public ActionFoundReceiver () throws java.lang.Throwable
	{
		super ();
		if (getClass () == ActionFoundReceiver.class)
			mono.android.TypeManager.Activate ("iBeacon_Indexer.ActionFoundReceiver, iBeacon_Indexer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public ActionFoundReceiver (ibeacon_indexer.BlueToothDiscover p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == ActionFoundReceiver.class)
			mono.android.TypeManager.Activate ("iBeacon_Indexer.ActionFoundReceiver, iBeacon_Indexer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "iBeacon_Indexer.BlueToothDiscover, iBeacon_Indexer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
	}


	public void onReceive (android.content.Context p0, android.content.Intent p1)
	{
		n_onReceive (p0, p1);
	}

	private native void n_onReceive (android.content.Context p0, android.content.Intent p1);


	public void onLeScan (android.bluetooth.BluetoothDevice p0, int p1, byte[] p2)
	{
		n_onLeScan (p0, p1, p2);
	}

	private native void n_onLeScan (android.bluetooth.BluetoothDevice p0, int p1, byte[] p2);

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
