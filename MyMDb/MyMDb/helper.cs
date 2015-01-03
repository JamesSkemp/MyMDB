using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MyMDb
{
    public static class helper
    {

        public static string[] SplitRegEx(this string value, string pattern)
        {
            return value.SplitRegEx(pattern, StringSplitOptions.None);
        }
        public static string[] SplitRegEx(this string value, string pattern, StringSplitOptions option)
        {
            if (value == null)
                return new string[0];
            if (option == StringSplitOptions.None)
                return Regex.Split(value, pattern);
            else
                return Regex.Split(value, pattern).ToList().FindAll(s => !String.IsNullOrEmpty(s)).ToArray();
        }

        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;
            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
    }


    public static class StreamExtensions
    {
        /// <summary>
        /// Copies the input stream to the output stream.
        /// </summary>
        /// <param name="input">Stream to be copied from.</param>
        /// <param name="output">Destination stream to copy to.</param>
        /// <remarks>Only useful before .NET 4</remarks>
        public static void CopyTo(this Stream input, Stream output)
        {
            byte[] buffer = new byte[4 * 1024];
            int bytesRead;
            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
                output.Write(buffer, 0, bytesRead);
        }
        /// <summary>
        /// Copies the input stream to the output stream.
        /// </summary>
        /// <param name="input">Stream to be copied from.</param>
        /// <param name="output">Destination stream to copy to.</param>
        /// <param name="buffersize">Size of the buffer to use.</param>
        /// <remarks>Only useful before .NET 4</remarks>
        public static void CopyTo(this Stream input, Stream output, int buffersize)
        {
            byte[] buffer = new byte[buffersize];
            int bytesRead;
            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
                output.Write(buffer, 0, bytesRead);
        }
        /// <summary>
        /// Copies the input stream to the output stream.
        /// </summary>
        /// <param name="input">Stream to be copied from.</param>
        /// <param name="output">Destination stream to copy to.</param>
        /// <param name="callback">callback method the show the progress of the CopyTo method.</param>
        /// <remarks>Only useful before .NET 4</remarks>
        public static void CopyTo(this Stream input, Stream output, CopyToHandle callback)
        {
            DateTime st = DateTime.Now;
            byte[] buffer = new byte[4 * 1024];
            int bytesRead;
            long currentpos = 0;
            long length = input.Length;
            if (input is GZipStream)
                length = GzLength((GZipStream)input);
            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                currentpos += bytesRead;
                output.Write(buffer, 0, bytesRead);
                if (callback != null)
                    callback(null, new CopyToArgs(length, currentpos, st));
            }
        }
        /// <summary>
        /// Copies the input stream to the output stream.
        /// </summary>
        /// <param name="input">Stream to be copied from.</param>
        /// <param name="output">Destination stream to copy to.</param>
        /// <param name="buffersize">Size of the buffer to use.</param>
        /// <param name="callback">callback method the show the progress of the CopyTo method.</param>
        /// <remarks>Only useful before .NET 4</remarks>
        public static void CopyTo(this Stream input, Stream output, int buffersize, CopyToHandle callback)
        {
            DateTime st = DateTime.Now;
            byte[] buffer = new byte[buffersize];
            int bytesRead;
            long currentpos = 0;
            long length = input.Length;
            if (input is GZipStream)
                length = GzLength((GZipStream)input);
            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                currentpos += bytesRead;
                output.Write(buffer, 0, bytesRead);
                if (callback != null)
                    callback(null, new CopyToArgs(length, currentpos, st));
            }
        }

        public static int GzLength(this GZipStream gs)
        {
            try
            {
                if (!gs.CanSeek)
                    return -1;
                byte[] ba = new byte[4];
                gs.Seek(-4, SeekOrigin.End);
                gs.Read(ba, 0, 4);
                gs.Seek(0, SeekOrigin.Begin);
                gs.Position = 0;
                return BitConverter.ToInt32(ba, 0);
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }

    public delegate void CopyToHandle(object sender, CopyToArgs e);
    public class CopyToArgs : EventArgs
    {
        private readonly long _TotalLength = 0;
        public long TotalLength
        {
            get { return _TotalLength; }
        }
        private readonly long _CurrentPosition = 0;
        public long CurrentPosition
        {
            get { return _CurrentPosition; }
        }
        private readonly DateTime _StartTime = DateTime.Now;
        public DateTime StartTime
        {
            get { return _StartTime; }
        }

        public double LengthPrSecond
        {
            get
            {
                return Math.Max(1L, _CurrentPosition) / Math.Max(1D, Duration.TotalSeconds);
            }
        }
        public DateTime ExpectedEndTime
        {
            get
            {
                return StartTime.AddSeconds(Math.Round(Math.Max(1L, _TotalLength) / Math.Max(1D, LengthPrSecond), 0));
            }
        }
        public TimeSpan Duration
        {
            get
            {
                return DateTime.Now - _StartTime;
            }
        }

        internal CopyToArgs(long tl, long cp, DateTime st)
        {
            _TotalLength = tl;
            _CurrentPosition = cp;
            _StartTime = st;
        }
    }
}
