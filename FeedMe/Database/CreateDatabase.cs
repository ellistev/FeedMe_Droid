using System;
using SQLite;

namespace FeedMe
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

	public class CreateDatabase
	{
		private SQLiteConnection conn;

		public CreateDatabase ()
		{

			string folder = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			conn = new SQLiteConnection (System.IO.Path.Combine (folder, "MajorMinorIndex.s3db"));



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

	}
}

