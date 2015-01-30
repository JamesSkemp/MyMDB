using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PawJershauge.IMDBFlatFiles
{
	public class DistributorListFile : FlatFile
	{
		bool fastForward = true;

		public DistributorListFile(string path)
			: base(path)
		{
			// todo
			ColumnHeaders.Add("RawData");
			ColumnHeaders.Add("Title");
			ColumnHeaders.Add("YearPlayed");
			ColumnHeaders.Add("EpisodeName");
			ColumnHeaders.Add("Distributor");
			ColumnHeaders.Add("CountryCode");
			ColumnHeaders.Add("YearDistributed");
			ColumnHeaders.Add("Country");
			ColumnHeaders.Add("Format");
		}

		private void FastForward()
		{
			if (fastForward)
			{
				while (!EndOfFile && ReadLine() != "DISTRIBUTORS LIST")
				{
				}
				ReadLine();
				ReadLine();
				fastForward = false;
			}
		}

		public override bool Read()
		{
			string line = string.Empty;
			bool read = true;
			bool readOn = false;
			FastForward();
			do
			{
				line = ReadLine();
				if (line != null)
				{
					read = !line.StartsWith("------------------") && !line.StartsWith("=========");
					if (read)
					{
						readOn = (line.Length == 0 || line.Contains("{{SUSPENDED}}"));
						if (!readOn)
						{
							string[] lineArray = line.Split('\t');



							SetObjectArray(new object[9] 
							{
								// RawData
								line,
								// Title
								lineArray[0],
								lineArray[1],
								lineArray[2],
								lineArray[3],
								lineArray[4],
								lineArray[5],
								lineArray[6],
								lineArray[7],
							});
						}
					}
					else
						readOn = false;
				}
				else
				{
					readOn = false;
					read = false;
				}
			} while (readOn);
			return read;
		}
	}
}
