using System;
using System.Collections.Generic;
using Android.Bluetooth;
using Java.Util;

namespace iBeacon_Indexer
{
	public class BLEParser
	{
		public BLEParser ()
		{
		}

		static char[] hexArray = "0123456789ABCDEF".ToCharArray();

		private static String bytesToHex(byte[] bytes) {
			char[] hexChars = new char[bytes.Length * 2];
			for ( int j = 0; j < bytes.Length; j++ ) {
				int v = bytes[j] & 0xFF;
				hexChars[j * 2] = hexArray[v >> 4];
				hexChars[j * 2 + 1] = hexArray[v & 0x0F];
			}
			return new String(hexChars);
		}

		// use this method to parse those bytes and turn to an object which defined proceeding.
		// the uuidMatcher works as a UUID filter, put null if you want parse any BLE advertising data around.
		public ScannedBleDevice ParseRawScanRecord(BluetoothDevice device, int rssi, byte[] advertisedData, byte[] uuidMatcher) {

			int startByte = 2;
			try {
				ScannedBleDevice parsedObj = new ScannedBleDevice();
				// parsedObj.BLEDevice = device;
				parsedObj.DeviceName = device.Name;
				parsedObj.MacAddress = device.Address;
				parsedObj.RSSI = rssi;
				List<UUID> uuids = new List<UUID>();
				int skippedByteCount = advertisedData[0];
				int magicStartIndex = skippedByteCount + 1;
				int magicEndIndex = magicStartIndex
					+ advertisedData[magicStartIndex] + 1;
				List<byte> magic = new List<byte>();
				for (int i = magicStartIndex; i < magicEndIndex; i++) {
					magic.Add(advertisedData[i]);
				}

				byte[] companyId = new byte[2];
				companyId[0] = magic[2];
				companyId[1] = magic[3];
				parsedObj.CompanyId = companyId;

				byte[] ibeaconProximityUUID = new byte[16];
				for (int i = 0; i < 16; i++) {
					if(magic.Count > (i+6)){
						ibeaconProximityUUID[i] = magic[i + 6];
					}
				}

				parsedObj.IbeaconProximityUUID = ibeaconProximityUUID;

				String hexString = bytesToHex(ibeaconProximityUUID);

				//Here is your UUID
				parsedObj.IbeaconProximityUUIDString =  hexString.Substring(0,8) + "-" + 
					hexString.Substring(8,4) + "-" + 
					hexString.Substring(12,4) + "-" + 
					hexString.Substring(16,4) + "-" + 
					hexString.Substring(20,12);

				byte[] major = new byte[2];
				byte[] minor = new byte[2];
				byte tx = 0;
				//try{

				major[0] = magic[22];
				major[1] = magic[23];
				parsedObj.Major = major;
				parsedObj.MajorInt  = (advertisedData[startByte+23] & 0xff) * 0x100 + (advertisedData[startByte+24] & 0xff);

				minor[0] = magic[24];
				minor[1] = magic[25];

				parsedObj.Minor = minor;
				parsedObj.MinorInt =  (advertisedData[startByte+25] & 0xff) * 0x100 + (advertisedData[startByte+26] & 0xff);

				tx = magic[26];
				parsedObj.Tx = tx;

				//} catch (System.Exception ex) {
				//	parsedObj.Major = major;
				//	parsedObj.MajorInt  = 0;
				//	parsedObj.Minor = minor;
				//	parsedObj.MinorInt =  0;
				//	parsedObj.Tx = 0;
				//}

				parsedObj.ScannedTime = new Date().Time;
				return parsedObj;
			} catch (System.Exception ex) {

				// Log.e(LOG_TAG,
				// "Exception in ParseRawScanRecord with advertisedData: "
				// + Util.BytesToHexString(advertisedData, " ")
				// + ", detail: " + ex.getMessage());
				return null;
			}
		}
	}
}

