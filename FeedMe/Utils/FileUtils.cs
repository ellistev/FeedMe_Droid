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
namespace iBeacon_Indexer
{

 
	public class FileUtils
	{

		public static List<FileSystemInfo> getFileNames(String folder)		 
		{
			List<FileSystemInfo> visibleThings = new List<FileSystemInfo>();
			var dir = new DirectoryInfo(folder);
			foreach (var item in dir.GetFileSystemInfos().OrderBy(x=> x.Name))
			{
				visibleThings.Add(item);
			}
			
			return visibleThings;
		}
	}

}

