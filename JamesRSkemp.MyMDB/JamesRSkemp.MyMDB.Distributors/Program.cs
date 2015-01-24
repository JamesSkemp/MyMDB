using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JamesRSkemp.MyMDB.Distributors
{
	class Program
	{
		static void Main(string[] args)
		{
			var distributorFilePath = System.Configuration.ConfigurationManager.AppSettings["DistributorFilePath"];

			if (!File.Exists(distributorFilePath))
			{
				Console.WriteLine("Distributors file cannot be found.");
				Console.WriteLine("Please update the application configuration with the location of the uncompressed file.");
				Console.WriteLine("Press any key to end.");
				Console.ReadKey();
				return;
			}

			var maximumDistributors = 0;
			int.TryParse(ConfigurationManager.AppSettings["MaximumDistributors"].ToString(), out maximumDistributors);

			var reader = new StreamReader(distributorFilePath);

			while (!reader.EndOfStream && reader.ReadLine() != "DISTRIBUTORS LIST")
			{
				// Cycle through the lines, skipping the header information.
			}

			int distributorsParsed = 0;
			string lineData = null;

			List<int> tempColumnCounts = new List<int>();
			List<int> tempColumnCounts2 = new List<int>();

			while (!reader.EndOfStream && (lineData = reader.ReadLine()) != null)
			{
				if (string.IsNullOrWhiteSpace(lineData) || lineData.StartsWith("========="))
				{
					continue;
				}

				string[] lineElements = lineData.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);

				if (lineElements.Length == 1)
				{
					continue;
				}

				var lineArrayLength = lineElements.Length;
				if (!tempColumnCounts.Contains(lineArrayLength))
				{
					Console.WriteLine(lineArrayLength);
					Console.WriteLine(lineData);
					for (int i = 0; i < lineArrayLength; i++)
					{
						Console.WriteLine("	" + lineElements[i]);
					}
					tempColumnCounts.Add(lineArrayLength);
				}
				else if (!tempColumnCounts2.Contains(lineArrayLength))
				{
					//Console.WriteLine(lineArrayLength);
					//Console.WriteLine(lineData);
					tempColumnCounts2.Add(lineArrayLength);
				}

				var distributor = parseLineData(lineData);
				if (distributor == null)
				{
					Console.WriteLine(lineData);
					Console.WriteLine("Bad line found. Press any key to continue.");
					Console.ReadKey();
					break;
				}
				else
				{
					// TODO uncomment
					saveData(distributor);
				}

				// If there's a maximum number of distributors to parse, see if we've hit that limit.
				if (maximumDistributors > 0)
				{
					distributorsParsed++;
					if (distributorsParsed >= maximumDistributors)
					{
						break;
					}
				}
			}
			Console.WriteLine(string.Format("Parsed {0} distributors.", distributorsParsed));
			Console.WriteLine("Press any key to end.");
			Console.ReadKey();
		}

		/// <summary>
		/// Given a line, parse it and return a new distributor for saving into the database.
		/// </summary>
		/// <param name="lineData">Raw line from the data dump.</param>
		/// <returns>Database distributor ready to be saved, or null if the data can't be parsed.</returns>
		private static DatabaseModels.Distributor parseLineData(string lineData)
		{
			var distributor = new DatabaseModels.Distributor();
			// Save the raw data in case we need to fix our parser.
			distributor.RawData = lineData;

			string[] lineElements = lineData.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);

			if (lineElements.Length < 2 || lineElements.Length > 3)
			{
				// TODO log the line
				return null;
			}

			// The first element should contain the title and year. It may also include episode information.
			// First check to see if it's formatted like an episode.
			var episodeMatch = Regex.Match(lineElements[0], @"^(.*) \(([^)]*)\) {(.*)}$");
			if (episodeMatch.Success && episodeMatch.Groups.Count == 4)
			{
				distributor.Title = episodeMatch.Groups[1].Value;
				distributor.YearPlayed = episodeMatch.Groups[2].Value;
				distributor.EpisodeName = episodeMatch.Groups[3].Value;
			}
			else
			{
				// We have one other format we can check against.
				var otherMatch = Regex.Match(lineElements[0], @"^(.*) \(([^)]*)\).*$");
				if (otherMatch.Success && otherMatch.Groups.Count == 3)
				{
					distributor.Title = otherMatch.Groups[1].Value;
					distributor.YearPlayed = otherMatch.Groups[2].Value;
					distributor.EpisodeName = null;
				}
			}

			// If our title is still empty, the data can't be trusted.
			if (string.IsNullOrWhiteSpace(distributor.Title))
			{
				// TODO log the raw data
				return null;
			}

			// Next we have either the distributor information, or year/country/format data.
			var distributionMatch = Regex.Match(lineElements[1], @"^(.*) \[(.*)\]$");
			if (distributionMatch.Success && distributionMatch.Groups.Count == 3)
			{
				distributor.Distributor1 = distributionMatch.Groups[1].Value;
				distributor.CountryCode = distributionMatch.Groups[2].Value;
			}
			else
			{
				distributor.Distributor1 = null;
				distributor.CountryCode = null;
			}

			var lastElement = lineElements[lineElements.Length - 1];
			var yearCountryFormatMatch = Regex.Match(lastElement, @"^\(([^)]*)\) \(([^)]*)\) \(([^)]*)\)$");
			if (yearCountryFormatMatch.Success && yearCountryFormatMatch.Groups.Count == 4)
			{
				distributor.YearDistributed = yearCountryFormatMatch.Groups[1].Value;
				distributor.Country = yearCountryFormatMatch.Groups[2].Value;
				distributor.Format = yearCountryFormatMatch.Groups[3].Value;
			}
			else
			{
				distributor.YearDistributed = null;
				distributor.Country = null;
				distributor.Format = null;
			}

			if (string.IsNullOrWhiteSpace(distributor.Distributor1) && string.IsNullOrWhiteSpace(distributor.YearDistributed))
			{
				// TODO log this
			}

			return distributor;
		}

		/// <summary>
		/// Saves a distributor as a new record in the database.
		/// </summary>
		/// <param name="distributor">Data to add to the database.</param>
		private static void saveData(DatabaseModels.Distributor distributor)
		{
			using (var db = new DatabaseModels.DistributorsEntities())
			{
				try
				{
					db.Distributors.Add(distributor);
					db.SaveChanges();
				}
				catch (Exception)
				{
					// TODO log the exception
					throw;
				}
			}
		}
	}
}
