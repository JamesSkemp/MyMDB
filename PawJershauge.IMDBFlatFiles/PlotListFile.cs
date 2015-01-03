using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace PawJershauge.IMDBFlatFiles
{
    public class PlotListFile : FlatFile
    {
        bool fastforward = true;
        Guid? currentkey = null;
        byte currentkeyindex = 0;

        public PlotListFile(string path)
            : base(path)
        {
            ColumnHeaders.Add("Id", typeof(Guid));
            ColumnHeaders.Add("PlotIndex", typeof(byte));
            ColumnHeaders.Add("Text");
            ColumnHeaders.Add("By");
        }

        private void FastForward()
        {
            if (fastforward)
            {
                while (!EndOfFile && ReadLine() != "PLOT SUMMARIES LIST")
                { 
                }
                fastforward = false;
            }
        }

        public override bool Read()
        {
            FastForward();
            string line = string.Empty;
            StringBuilder plot = new StringBuilder();
            bool read = true;
            bool readOn = false;
            do
            {
                line = ReadLine();
                if (line != null)
                {
                    read = !EndOfFile;//line.StartsWith("------------------");
                    if (read)
                    {
                        if (currentkey.HasValue && line.StartsWith("BY: "))
                        {
                            currentkeyindex++;
                            SetObjectArray(new object[4] 
                            { 
                                currentkey.Value,
                                currentkeyindex,
                                plot.ToString(),
                                line.Substring(4)
                            });
                            //plot = new StringBuilder();
                            readOn = false;
                        }
                        else
                        {
                            if (line.StartsWith("------------")) // if line Starts With we search for a new key
                            {
                                currentkey = null;
                                currentkeyindex = 0;
                            }
                            else
                            {
                                if (line.StartsWith("MV: "))
                                {
                                    if (line.Contains("{{SUSPENDED}}"))
                                    {
                                        currentkey = null;
                                        currentkeyindex = 0;
                                    }
                                    else
                                    {
                                        currentkeyindex=0;
                                        currentkey = line.Substring(4).Trim().ToGuid();
                                        plot = new StringBuilder();
                                    }
                                }
                                else
                                {
                                    if(line.StartsWith("PL: "))
                                        plot.Append(line.Substring(3));
                                }
                            }
                            readOn = true;
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
