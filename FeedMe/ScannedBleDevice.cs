namespace iBeacon_Indexer
{
	public class ScannedBleDevice{

		public string MacAddress;

		public string DeviceName;
		public double RSSI;
		public double Distance;

		public byte[] CompanyId;
		public byte[] IbeaconProximityUUID;
		public string IbeaconProximityUUIDString;
		public byte[] Major;
		public int MajorInt;
		public byte[] Minor;
		public int MinorInt;
		public byte Tx;

		public long ScannedTime;

	}
}