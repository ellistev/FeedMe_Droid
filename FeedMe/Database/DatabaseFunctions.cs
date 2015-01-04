using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using String = System.String;
using Exception = System.Exception;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Widget;
using SQLite;
using Java.IO;
using Java.Lang;

namespace iBeacon_Indexer
{


	public class DatabaseFunctions
	{
		private SQLiteConnection conn;
		private static string dbName = "MajorMinorIndex.s3db";
		private static string dbExternalFolder = "iBeacon_Indexer_Droid";
		private Context context;
		public DatabaseFunctions ()
		{
			//create new database connection

			string dbPath = Path.Combine (Android.OS.Environment.ExternalStorageDirectory.ToString (), dbName);
			conn = new SQLiteConnection (dbPath);
		}

		public DatabaseFunctions (Context icontext)
		{
			//creates and updates database to most recent version
			//create new database connection, and create database location if doesn't already exist.
			context = icontext;
			string dbPath = Path.Combine (Android.OS.Environment.ExternalStorageDirectory.ToString(), dbName);
			// Check if your DB has already been extracted.

			conn = new SQLiteConnection (dbPath);

			UpdateDatabaseToCurrentVersion ();
		}

		public void UpdateDatabaseToCurrentVersion(){
			int lastSchemaVersion = 0;
			int lastDataVersion = 0;
			try{
				var lastSchemaVersionEntry = conn.Query<DBSchemaVersion> ("select * from DbSchemaVersion");
				lastSchemaVersion = lastSchemaVersionEntry.OrderBy(x => x.Id).First().LastDBSchemaVersion;
			}
			catch(Exception e){
				// doesn't exist
			}
			try{
				var lastDbDataEntry = conn.Query<DbDataVersion> ("select * from DBDataVersion");
				lastDataVersion = lastDbDataEntry.FirstOrDefault().LastDBDataVersion;
			}
			catch(Exception e){
				// doesn't exist
			}
		
			UpdateSchema (lastSchemaVersion);

			UpdateData (lastDataVersion);
		}

		private void UpdateSchema(int lastSchemaVersion)
		{
			//get list of files in schema folder
			var schemaFolderFiles = context.Assets.List ("database/schema");
			//List<String> schemaUpdateFiles = FileUtils.getFileNames (schemaFolderName);
			foreach(string dataSqlFileName in schemaFolderFiles){
				if(Integer.ParseInt(dataSqlFileName.Split('.')[0]) > lastSchemaVersion){
					//if this file is greater than last updated, execute to update database schema
					StringBuilder buf= new StringBuilder();
					Stream sqlstreamin = context.Assets.Open ("database/schema/" + dataSqlFileName);
					BufferedReader infile =	new BufferedReader(new Java.IO.InputStreamReader(sqlstreamin, "UTF-8"));
					String str;

					while ((str=infile.ReadLine()) != null) {
						buf.Append(str);
					}

					infile.Close();
					//execute all files not executed already
					ExecuteScript (buf.ToString());
				}
			}

		}

		private void UpdateData(int lastDataVersion)
		{
			//get list of files in schema folder
			var dataFolderFiles = context.Assets.List ("database/data");
			//List<String> schemaUpdateFiles = FileUtils.getFileNames (schemaFolderName);
			foreach(string dataSqlFileName in dataFolderFiles){
				if(Integer.ParseInt(dataSqlFileName.Split('.')[0]) > lastDataVersion){
					//if this file is greater than last updated, execute to update database schema
					StringBuilder buf= new StringBuilder();
					Stream sqlstreamin = context.Assets.Open ("database/data/" + dataSqlFileName);
					BufferedReader infile =	new BufferedReader(new Java.IO.InputStreamReader(sqlstreamin, "UTF-8"));
					String str;

					while ((str=infile.ReadLine()) != null) {
						buf.Append(str);
					}

					infile.Close();
					//execute all files not executed already
					ExecuteScript (buf.ToString());
				}
			}
		}

		protected void ExecuteScript(string script)
		{
			string[] commandTextArray = script.Split(new string[] { "GO" }, StringSplitOptions.RemoveEmptyEntries); // See EDIT below!

			foreach (string commandText in commandTextArray)
			{
				if (commandText.Trim() == string.Empty) continue;
				conn.Execute (commandText);
		    }
		}

		public void RefreshDatabase (Context context)
		{
			//wipes out database with fresh copy from assets folder
			//todo find out how to update database with deltas if there are changes between versions

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

			conn.CreateTable<BtDevices> ();

		}

		public int AddNewBtDevice(BtDevice btdevice){

			BtDevices btDevicesIndividual = new BtDevices (btdevice);

			return conn.Insert(btDevicesIndividual);
		}

		public Locations GetLocationName(int major, int minor){

			var results = conn.Table<MMLocation>().Where (x => x.Major == major && x.Minor == minor);
			MMLocation resultMMLocation = results.FirstOrDefault ();
			TableQuery<Locations> locationNameResult = conn.Table<Locations>().Where (x => x.Id == resultMMLocation.Id);
			string path = conn.DatabasePath;

			return locationNameResult.FirstOrDefault();

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
				System.IO.File.Copy (databaseLocation, sdcardpath, true);
			}
		}

	}
}

