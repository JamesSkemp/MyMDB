using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace PawJershauge.IMDBFlatFiles
{
    public class GenreListFile : FlatFile
    {
        Dictionary<string, short> _GenreList = null;
        public Dictionary<string, short> GenreList
        {
            get { return _GenreList; }
        }
        bool fastforward = true;

        public GenreListFile(string path,Dictionary<string, short> genreList)
            : base(path)
        {
            ColumnHeaders.Add("MediaEntry_Id", typeof(Guid));
            ColumnHeaders.Add("Genre_Id", typeof(short));
            _GenreList = genreList;
        }

        private void FastForward()
        {
            if (fastforward)
            {
                while (!EndOfFile && !ReadLine().Contains("THE GENRES LIST"))
                {
                }
                ReadLine();
                fastforward = false;
            }
        }

        private short GetGenre(string key)
        { 
            short gk = 0;
            if (GenreList.TryGetValue(key, out gk))
                return gk;
            else
            {
                short newk = (short)(GenreList.Values.Max() + 1);
                GenreList.Add(key, newk);
                return newk;
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
                            string g = linearray[linearray.Length - 1];
                            SetObjectArray(new object[2] 
                            { 
                                k.ToGuid(), 
                                GetGenre(g)
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
