namespace FeedMe
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
		public byte[] Minor;
		public byte Tx;

		public long ScannedTime;

	}
}