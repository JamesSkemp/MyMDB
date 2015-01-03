using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PawJershauge.IMDBFlatFiles
{
    public class MediaEntry 
    {
        private Guid _Id = Guid.Empty;
        public Guid Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        private Guid _ParentId = Guid.Empty;
        public Guid ParentId
        {
            get { return _ParentId; }
            set { _ParentId = value; }
        }

        
        public string Key { get; private set; }
        public string ParentKey { get; private set; }
        public string Title { get; private set; }
        public string SubTitle { get; private set; }
        public int Season { get; private set; }
        public int Episode { get; private set; }
        public int Year { get; private set; }
        public int YearEnd { get; private set; }
        public bool Show { get; private set; }
        public DateTime? Dated { get; private set; }
        public int TitleVersion { get; private set; }
        //public string TagLine { get; set; }

        internal MediaEntry(string line)
        {
            //_Id = Guid.NewGuid();
            //_ParentId = _Id;
            string[] linearray = line.Split('\t');
            string k = line.Replace("\t", "");
            string year = linearray[linearray.Length - 1];
            Key = linearray[0];
            Show = k.StartsWith("\"");
            if (Show && Key.Contains("{"))
                ParentKey = Key.Substring(0, Key.IndexOf(" {"));
            if (year.Contains("-"))
            {
                string[] ya = year.Split('-');
                if (!ya[0].Contains("?"))
                    Year = int.Parse(ya[0].Trim());
                if (!ya[1].Contains("?"))
                    YearEnd = int.Parse(ya[1].Trim());
            }
            else
            {
                if (year.Contains('?'))
                {
                    if (Key.Contains("????"))
                        Year = 0;
                    else
                        Year = util.GetYear(Key);
                }
                else
                    Year = int.Parse(year.Trim());
            }
            TitleVersion = util.GetYearTitleVersion(k);
            if (Show && k.Contains("{"))
            {
                Title = k.Substring(1, k.LastIndexOf("\"")-1);
                int st = k.IndexOf("{") + 1;
                string sub = k.Substring(st, k.LastIndexOf("}") - st);
                if (!sub.StartsWith("(#") && sub.StartsWith("(") && sub.EndsWith(")") && util.Dated.IsMatch(sub))
                {
                    SubTitle = null;
                    Dated = util.GetDated(sub);//Dated = sub.Substring(1, sub.Length - 2);
                }
                else if (sub.Contains("(#"))
                {

                    Dated = null;
                    KeyValuePair<int, int> i = util.GetSE(sub.Substring(sub.LastIndexOf("(#")));
                    Season = i.Key;
                    Episode = i.Value;
                    SubTitle = sub.Substring(0, sub.LastIndexOf("(#")).Trim();
                }
                else
                    SubTitle = sub;
            }
            else
            {
                Title = k.Substring(0, k.LastIndexOf("(")-1).Trim();
                if (Title.StartsWith("\"") && Title.EndsWith("\""))
                    Title = Title.Substring(1, Title.Length - 2);
            }
            //if (Key != null && Key.StartsWith("\"'Allo 'Allo!\""))
            //    System.Diagnostics.Debug.WriteLine(string.Format("{2}-{0}\t{1}", Key.Length, Key, string.IsNullOrEmpty(ParentKey) ? "M" : "S"));
            //if (ParentKey != null && ParentKey.StartsWith("\"'Allo 'Allo!\""))
            //    System.Diagnostics.Debug.WriteLine(string.Format("P-{0}\t{1}", ParentKey.Length, ParentKey));
            _Id = Key.ToGuid();
            _ParentId = string.IsNullOrEmpty(ParentKey) ? _Id : ParentKey.ToGuid();
        }

        public override string ToString()
        {
            return Key;
        }
    }
}
