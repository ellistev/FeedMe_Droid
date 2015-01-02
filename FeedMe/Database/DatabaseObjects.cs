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

		public BtDevices(BtDevice device){
			Name = device.Name;
			Type = device.Type;
			MacAddress = device.MacAddress;
			Strength = device.Strength;
			Uuid = device.UuidString;
			Major = device.MajorInt;
			Minor = device.MinorInt;
		}
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

