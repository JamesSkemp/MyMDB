using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace PawJershauge.IMDBFlatFiles
{
    public class TagLineListFile : FlatFile
    {
        bool fastforward = true;
        Guid? currentkey = null;
        byte currentkeyindex = 0;

        public TagLineListFile(string path)
            : base(path)
        {
            ColumnHeaders.Add("Id", typeof(Guid));
            ColumnHeaders.Add("TagIndex", typeof(byte));
            ColumnHeaders.Add("Text");
        }

        private void FastForward()
        {
            if (fastforward)
            {
                while (!EndOfFile && ReadLine() != "TAG LINES LIST")
                { 
                }
                fastforward = false;
            }
        }

        public override bool Read()
        {
            FastForward();
            string line = string.Empty;
            bool read = true;
            bool readOn = false;
            do
            {
                line = ReadLine();
                if (line != null)
                {
                    read = !line.StartsWith("------------------");
                    if (read)
                    {
                        if (currentkey.HasValue && line.StartsWith("\t"))
                        {
                            currentkeyindex++;
                            SetObjectArray(new object[3] 
                            { 
                                currentkey.Value,
                                currentkeyindex,
                                line.Substring(1)
                            });
                            readOn = false;
                        }
                        else
                        {
                            if (line.Length < 1) // if line is empty we search for a new key
                            {
                                currentkey = null;
                                currentkeyindex = 0;
                                readOn = true;
                            }
                            else
                            {
                                if (line.StartsWith("#") & !line.Contains("{{SUSPENDED}}"))
                                {
                                    currentkeyindex++;
                                    Guid g = line.Substring(2).Trim().ToGuid();
                                    currentkey = g;
                                    string tag = ReadLine();
                                    SetObjectArray(new object[3] 
                                        { 
                                            currentkey.Value,
                                            currentkeyindex,
                                            tag.Substring(1)
                                        });
                                    readOn = false;
                                }
                                else
                                    readOn = true;
                            }
                        }
                    }
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
