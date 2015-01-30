using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PawJershauge.IMDBFlatFiles
{
	public class Distributor
	{
		public string RawData { get; set; }
		public string Title { get; set; }
		public string YearPlayed { get; set; }
		public string EpisodeName { get; set; }
		public string Distributor { get; set; }
		public string CountryCode { get; set; }
		public string YearDistributed { get; set; }
		public string Country { get; set; }
		public string Format { get; set; }

		public Distributor()
		{
		}

		public Distributor(string listFileLine)
			: this()
		{
			if (!string.IsNullOrWhiteSpace(listFileLine))
			{
				this.RawData = listFileLine;

				var lineArray = listFileLine.Split('\t');
				// todo this needs to be split up
				this.Title = lineArray[0];

				if (lineArray.Length - 1 > 0)
				{
					// the last column seems to have the year, country, and format, with an optional seasons listing
				}
			}
		}
	}
}