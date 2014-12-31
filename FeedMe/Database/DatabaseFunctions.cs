using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using String = System.String;
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

	public class DatabaseFunctions
	{
		private SQLiteConnection conn;
		private static string dbName = "MajorMinorIndex.s3db";
		public DatabaseFunctions ()
		{
			string dbPath = Path.Combine (Android.OS.Environment.ExternalStorageDirectory.ToString (), dbName);
			conn = new SQLiteConnection (dbPath);
		}

		public DatabaseFunctions (Context context)
		{
			string dbPath = Path.Combine (Android.OS.Environment.ExternalStorageDirectory.ToString(), dbName);
			// Check if your DB has already been extracted.
			if (!File.Exists(dbPath))
			{
				using (BinaryReader br = new BinaryReader(context.Assets.Open(dbName)))
				{
					using (BinaryWriter bw = new BinaryWriter(new FileStream(dbPath, FileMode.Create)))
					{
						byte[] buffer = new byte[2048];
						int len = 0;
						while ((len = br.Read(buffer, 0, buffer.Length)) > 0)
						{
							bw.Write (buffer, 0, len);
						}
					}
				}
			}

			conn = new SQLiteConnection (dbPath);
		}

		public void RefreshDatabase (Context context)
		{
			//wipes out database with fresh copy from assets folder

			string dbPath = Path.Combine (Android.OS.Environment.ExternalStorageDirectory.ToString (), dbName);
			// Check if your DB has already been extracted.

			using (BinaryReader br = new BinaryReader(context.Assets.Open(dbName)))
			{
				using (BinaryWriter bw = new BinaryWriter(new FileStream(dbPath, FileMode.OpenOrCreate)))
				{
					byte[] buffer = new byte[2048];
					int len = 0;
					while ((len = br.Read(buffer, 0, buffer.Length)) > 0)
					{
						bw.Write (buffer, 0, len);
					}
				}
			}
		}

		public void CreateTables(){

			conn.CreateTable<MMLocation> ();


			conn.CreateTable<Locations> ();

		}

		public void PopulateData(){

			var s = new MMLocation { Id = 1,
				Major = 1854,
				Minor = 36039,
				LocationId = 1
			};
			conn.Insert (s);
			s = new MMLocation { Id = 2,
				Major = 43942,
				Minor = 1810,
				LocationId = 2
			};
			conn.Insert (s);
			s = new MMLocation { Id = 3,
				Major = 62614,
				Minor = 62288,
				LocationId = 3
			};
			conn.Insert (s);

			var l = new Locations { Id = 1,
				Name = "Bedroom"
			};
			conn.Insert (l);
			l = new Locations {
				Id = 2,
				Name = "Couch"
			};
			conn.Insert (l);
			l = new Locations {
				Id = 3,
				Name = "Table"
			};
			conn.Insert (l);
		}

		public SQLiteConnection GetConnection(){
			return conn;
		}

		public void ExportDatabaseToExternalStorage(){
			string sdcardpath = System.IO.Path.Combine (Android.OS.Environment.ExternalStorageDirectory.Path, "MajorMinorIndex.s3dm");

			string databaseLocation = System.IO.Path.Combine (System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal), "MajorMinorIndex.s3db");

			if(System.IO.File.Exists(sdcardpath)){
				File.Copy (databaseLocation, sdcardpath, true);
			}
		}

	}
}

