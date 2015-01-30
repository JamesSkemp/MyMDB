<Query Kind="Statements">
  <Namespace>System.IO.Compression</Namespace>
</Query>

var baseDirectory = new DirectoryInfo(@"C:\Users\James\Projects\MyMDB\MyMDb\MyMDb\DB Files\");

foreach (var directory in baseDirectory.EnumerateDirectories())
{
	foreach (var file in directory.GetFiles())
	{
		try
		{
			// Get the stream of the source file. 
			using (FileStream inFile = file.OpenRead())
			{
				//Create the decompressed memorystream. 
				using (MemoryStream ms = new MemoryStream())
				{
					using (DeflateStream Decompress = new DeflateStream(inFile, CompressionMode.Decompress))
					{
						byte[] buffer = new byte[4096];
						int numRead;
						while ((numRead = Decompress.Read(buffer, 0, buffer.Length)) != 0)
							ms.Write(buffer, 0, numRead);
					}
					ms.Position = 0;
					using (StreamReader sr = new StreamReader(ms))
					{
						//file.FullName.Dump();
						File.WriteAllText(file.FullName + ".sql", sr.ReadToEnd());
						//sr.ReadToEnd().Dump();
						//break;
					}
				}
			}
		}
		catch (Exception ex)
		{
			ex.Dump();
		}
	}
}

FileInfo fi;
fi = new FileInfo(@"C:\Users\James\Projects\MyMDB\MyMDb\MyMDb\DB Files\Create\00_CREATE Db.zip");
fi = new FileInfo(@"C:\Users\James\Projects\MyMDB\MyMDb\MyMDb\DB Files\Bulk\Prepare Bulk Operations.zip");

try
{
	// Get the stream of the source file. 
	using (FileStream inFile = fi.OpenRead())
	{
		//Create the decompressed memorystream. 
		using (MemoryStream ms = new MemoryStream())
		{
			using (DeflateStream Decompress = new DeflateStream(inFile, CompressionMode.Decompress))
			{
				byte[] buffer = new byte[4096];
				int numRead;
				while ((numRead = Decompress.Read(buffer, 0, buffer.Length)) != 0)
					ms.Write(buffer, 0, numRead);
			}
			ms.Position = 0;
			using (StreamReader sr = new StreamReader(ms))
			{
				sr.ReadToEnd().Dump();
			}
		}
	}
}
catch (Exception ex)
{
	ex.Dump();
}
