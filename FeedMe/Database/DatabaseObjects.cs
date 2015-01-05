using System;
using SQLite;

namespace iBeacon_Indexer
{

	public class MMLocation
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		[Indexed]
		public int Major { get; set; }
		public int Minor { get; set; }
		public int LocationId { get; set; }

	}

	public class Locations
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		[Indexed]
		public string Name { get; set; }
	}

	public class GPSLocation
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set;}
		[Indexed]
		public int BtDevicesId { get; set;}
		public string Address { get; set;}
		public string LatitudeLongitude { get; set;}
		public string Altitude {get; set;}
	}

	public class BtDevices{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		[Indexed]
		public string Name{ get; set; }
		public string Type{ get; set; }
		public string MacAddress{ get; set; }
		public int Strength{ get; set; }
		public string Uuid{ get; set; }
		public int Major{ get; set; }
		public int Minor{ get; set; }


	}

	public class DBSchemaVersion
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		[Indexed]
		public int LastDBSchemaVersion { get; set; }
	}

	public class DbDataVersion
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		[Indexed]
		public int LastDBDataVersion { get; set; }
	}
}

