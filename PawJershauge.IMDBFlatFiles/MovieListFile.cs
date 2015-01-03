using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace PawJershauge.IMDBFlatFiles
{
    public class MovieListFile : FlatFile
    {
        bool fastforward = true;

        public MovieListFile(string path) : base(path)
        {
            ColumnHeaders.Add("Id", typeof(Guid));
            ColumnHeaders.Add("Parent_Id", typeof(Guid));
            ColumnHeaders.Add("Series", typeof(bool)); //must change !! TODO!!
            ColumnHeaders.Add("Title");
            ColumnHeaders.Add("Year", typeof(short));
            ColumnHeaders.Add("TitleVersionOfTheYear", typeof(byte));
            ColumnHeaders.Add("Season", typeof(short));
            ColumnHeaders.Add("Episode", typeof(short));
            ColumnHeaders.Add("SubTitle");
            ColumnHeaders.Add("Dated", typeof(DateTime));
            ColumnHeaders.Add("IMDbUrlId");
        }

        private void FastForward()
        {
            if (fastforward)
            {
                while (!EndOfFile && ReadLine() != "MOVIES LIST")
                {
                }
                ReadLine();
                ReadLine();
                fastforward = false;
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
                    read = !line.StartsWith("------------------");
                    if (read)
                    {
                        readOn = (line.Length == 0 || line.Contains("{{SUSPENDED}}"));
                        if (!readOn)
                        {
                            MediaEntry m = new MediaEntry(line);
                            SetObjectArray(new object[11] 
                            { 
                                m.Id, 
                                m.ParentId,
                                m.Show, 
                                m.Title, 
                                m.Year, 
                                m.TitleVersion, 
                                m.Season, 
                                m.Episode,
                                m.SubTitle, 
                                m.Dated.HasValue ? (object)m.Dated.Value : null,
                                null
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
