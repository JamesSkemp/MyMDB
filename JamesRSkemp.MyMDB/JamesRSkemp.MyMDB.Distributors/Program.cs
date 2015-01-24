using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
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
			var noQuoteDisplayed = false;

			while (!reader.EndOfStream && (lineData = reader.ReadLine()) != null)
			{
				if (string.IsNullOrWhiteSpace(lineData) || lineData.StartsWith("========="))
				{
					continue;
				}

				string[] lineElements = lineData.Split('\t');

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
					Console.WriteLine(lineArrayLength);
					Console.WriteLine(lineData);
					tempColumnCounts2.Add(lineArrayLength);
				}
				if (!noQuoteDisplayed && !lineData.StartsWith("\""))
				{
					Console.WriteLine("No quote.");
					Console.WriteLine(lineData);
					noQuoteDisplayed = true;
				}

				//var distributor = parseLineData(lineData);

				using (var db = new DatabaseModels.DistributorsEntities())
				{
					try
					{
						//db.Distributors.Add(distributor);
						//db.SaveChanges();


					}
					catch (Exception)
					{
						throw;
					}
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

		private static DatabaseModels.Distributor parseLineData(string lineData)
		{
			var distributor = new DatabaseModels.Distributor();
			distributor.RawData = lineData;
			distributor.Title = null;
			distributor.YearPlayed = null;
			distributor.EpisodeName = null;
			distributor.Distributor1 = null;
			distributor.CountryCode = null;
			distributor.YearDistributed = null;
			distributor.Country = null;
			distributor.Format = null;
			


			return distributor;
		}
	}
}
