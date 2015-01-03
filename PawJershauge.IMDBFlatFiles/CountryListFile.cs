using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace PawJershauge.IMDBFlatFiles
{
    public class CountryListFile : FlatFile
    {
        Dictionary<string, string> _CorrectionList = null;
        public Dictionary<string, string> CorrectionList
        {
            get { return _CorrectionList; }
        }
        bool fastforward = true;

        public CountryListFile(string path, Dictionary<string, string> correctionList)
            : base(path)
        {
            ColumnHeaders.Add("MediaEntry_Id", typeof(Guid));
            ColumnHeaders.Add("ISO3166_Alpha3", typeof(string));
            _CorrectionList = correctionList;
        }

        private void FastForward()
        {
            if (fastforward)
            {
                while (!EndOfFile && !ReadLine().Contains("COUNTRIES LIST"))
                {
                }
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
                    read = !EndOfFile;
                    if (read)
                    {
                        readOn = (line.Length == 0 || line.Contains("{{SUSPENDED}}"));
                        if (!readOn)
                        {
                            string[] linearray = line.Split('\t');
                            string k = linearray[0];
                            string g = linearray[linearray.Length - 1].ToLower();
                            SetObjectArray(new object[2] 
                            { 
                                k.ToGuid(), 
                                CorrectionList[g]
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
