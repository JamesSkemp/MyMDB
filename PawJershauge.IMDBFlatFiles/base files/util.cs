﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace PawJershauge.IMDBFlatFiles
{
    public static class util
    {
        //public static string ConnnectionString = "Data Source=WSW7ODPAW;Initial Catalog=MyMDb;Integrated Security=True";

        private static MD5 md5 = MD5.Create();
        public static Regex FindYear = new Regex("\\((\\d{4})", RegexOptions.Compiled);
        public static Regex FindSE = new Regex("\\(#.?(\\d{1,4}).?(\\d{1,4})?", RegexOptions.Compiled);
        public static Regex FindYearTitleVersion = new Regex("\\(\\d{4}/(\\w+)\\)", RegexOptions.Compiled);
        public static Regex Dated = new Regex(@"\((\d{4}-\d{2}-\d{2})\)", RegexOptions.Compiled);

        public static int GetYear(string value)
        {
            if (FindYear.IsMatch(value))
                return int.Parse(FindYear.Match(value).Groups[1].Value);
            else
                return 0;
        }
        public static KeyValuePair<int,int> GetSE(string value)
        {
            var x = FindSE.Match(value);
            return new KeyValuePair<int, int>(int.Parse(x.Groups[1].Value), string.IsNullOrEmpty(x.Groups[2].Value) ? 0 : int.Parse(x.Groups[2].Value));
        }
        public static int GetYearTitleVersion(string value)
        {
            if (FindYearTitleVersion.IsMatch(value))
                return (int)FromRoman(FindYearTitleVersion.Match(value).Groups[1].Value);
            else
                return 0;
        }
        public static DateTime? GetDated(string value)
        {
            if (Dated.IsMatch(value))
                return DateTime.ParseExact(Dated.Match(value).Groups[1].Value,"yyyy-MM-dd",null);
            else
                return null;
        }

        public static string ToRoman(long number)
        {
            if (number < 1)
                return null;
            StringBuilder sb = new StringBuilder();
            var values = Enum.GetValues(typeof(RomanNumber));
            for (int i = values.Length - 1; i >= 0; --i)
            {
                long value = (long)values.GetValue(i);
                while (number >= value)
                {
                    sb.Append(Enum.GetName(typeof(RomanNumber), value));
                    number -= value;
                }
            }
            return sb.ToString();
        }
        public static long FromRoman(string roman)
        {
            long rtn = 0;
            char[] chars = roman.Trim().ToUpper().ToCharArray();
            List<string> validchars = new List<string>(Enum.GetNames(typeof(RomanNumber)));
            for (int i = 0; i < chars.Length; i++)
			{
                if (!validchars.Contains(chars[i].ToString()))
                    throw new FormatException(string.Format("Incorrect roman format: {0}", roman));
                switch (chars[i])
                {
                    case 'I':
                        if (i < chars.Length - 1 && chars[i + 1] != 'I')
                            rtn--;
                        else 
                            rtn++;
                        break;
                    case 'V':
                        rtn += 5;
                        break;
                    case 'X':
                        if (i < chars.Length - 1 && (chars[i + 1] == 'L' || chars[i + 1] == 'C'))
                            rtn -= 10;
                        else 
                            rtn += 10;
                        break;
                    case 'L':
                        rtn += 50;
                        break;
                    case 'C':
                        if (i < chars.Length - 1 && (chars[i + 1] == 'D' || chars[i + 1] == 'M'))
                            rtn -= 100;
                        else
                            rtn += 100;
                        break;
                    case 'D':
                        rtn += 500;
                        break;
                    case 'M':
                        rtn += 1000;
                        break;
                    default:
                        throw new FormatException(string.Format("Incorrect roman format: {0}", roman));
                }
			}
            return rtn;
        }

        #region Extensions

        /// <summary>
        /// Converts an string into an Guid using the Default MD5 hash protocol and a Default Encoder.
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns>Guid representing the string.</returns>
        /// <remarks>Code Generated by Paw jershauge [Blog: pawjershauge.blogspot.com]</remarks>
        public static Guid ToGuid(this string value)
        {
            return new Guid(md5.ComputeHash(Encoding.Default.GetBytes(value)));
        }
        /// <summary>
        /// Converts an string into an Guid using the MD5 hash protocol and a specified Encoder.
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="encoder">Encoder to use when converting the string to bytes</param>
        /// <returns>Guid representing the string using the specified encoder.</returns>
        /// <remarks>Code Generated by Paw jershauge [Blog: pawjershauge.blogspot.com]</remarks>
        public static Guid ToGuid(this string value, Encoding encoder)
        {
            return new Guid(md5.ComputeHash(encoder.GetBytes(value)));
        }
        /// <summary>
        /// Converts an string into an Guid using the specified MD5 hash protocol and Encoder.
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="encoder">Encoder to use when converting the string to bytes</param>
        /// <param name="algName">Creates an instance of the specified implementation of the <see cref="System.Security.Cryptography.MD5"/> hash algorithm.</param>
        /// <returns>Guid representing the string using the specified algorithm and encoder.</returns>
        /// <remarks>Code Generated by Paw jershauge [Blog: pawjershauge.blogspot.com]</remarks>
        public static Guid ToGuid(this string value, Encoding encoder, string algName)
        {
            return new Guid(MD5.Create(algName).ComputeHash(encoder.GetBytes(value)));
        }
        /// <summary>
        /// Converts an string into an Guid using the specified MD5 hash protocol and Default Encoder.
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="algName">Creates an instance of the specified implementation of the <see cref="System.Security.Cryptography.MD5"/> hash algorithm.</param>
        /// <returns>Guid representing the string using the specified algorithm and default encoder.</returns>
        /// <remarks>Code Generated by Paw jershauge [Blog: pawjershauge.blogspot.com]</remarks>
        public static Guid ToGuid(this string value, string algName)
        {
            return new Guid(MD5.Create(algName).ComputeHash(Encoding.Default.GetBytes(value)));
        }

        /// <summary>
        /// Converts long into FileSize string. like [120,00 Kb]
        /// </summary>
        /// <param name="size">ValueType to convert into filesize string.</param>
        /// <returns>string that represents the filesize value. (120,00 KB)</returns>
        public static string ToFileSize(this double size)
        {
            return priToFileSize(size, 2);
        }
        /// <summary>
        /// Converts long into FileSize string. like [120,00 Kb]
        /// </summary>
        /// <param name="size">ValueType to convert into filesize string.</param>
        /// <param name="digits">The number of fractional digits (precision) in the return value.</param>
        /// <returns>string that represents the filesize value. (120,00 KB)</returns>
        public static string ToFileSize(this double size, int digits)
        {
            return priToFileSize(size, digits);
        }
        /// <summary>
        /// Converts long into FileSize string. like [120,00 Kb]
        /// </summary>
        /// <param name="size">ValueType to convert into filesize string.</param>
        /// <returns>string that represents the filesize value. (120,00 KB)</returns>
        public static string ToFileSize(this long size)
        {
            return priToFileSize((double)size, 2);
        }
        /// <summary>
        /// Converts long into FileSize string. like [120,00 Kb]
        /// </summary>
        /// <param name="size">ValueType to convert into filesize string.</param>
        /// <param name="digits">The number of fractional digits (precision) in the return value.</param>
        /// <returns>string that represents the filesize value. (120,00 KB)</returns>
        public static string ToFileSize(this long size, int digits)
        {
            return priToFileSize((double)size, digits);
        }

        /// <summary>
        /// Converts an valuetype into filesize string
        /// </summary>
        /// <param name="size">The Size to convert</param>
        /// <param name="digits">The number of fractional digits (precision) in the return value.</param>
        /// <returns>string that represents the filesize value. (120,00 KB)</returns>
        private static string priToFileSize(double size, int digits)
        {
            double mod = 1024;
            double sized = size;

            string[] sizeformat = new string[] { "B", "KB", "MB", "GB", "TB", "PB" };
            int i;
            for (i = 0; sized > mod; i++)
                sized /= mod;

            return string.Format("{0} {1}", System.Math.Round(sized, digits), sizeformat[i]);
        }


        #endregion
    }

    internal enum RomanNumber
    {
        I = 1,
        IV = 4,
        V = 5,
        IX = 9,
        X = 10,
        XL = 40,
        L = 50,
        XC = 90,
        C = 100,
        CD = 400,
        D = 500,
        CM = 900,
        M = 1000
    }

}
