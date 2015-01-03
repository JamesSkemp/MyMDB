using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace MyMDb
{
    public static class ZipFile
    {
        public static void Compress(string fi, string fo)
        {
            Compress(new FileInfo(fi), new FileInfo(fo));
        }
        public static void Compress(FileInfo fi, FileInfo fo)
        {
            // Get the stream of the source file. 
            using (FileStream inFile = fi.OpenRead())
            {
                // Create the compressed file. 
                using (FileStream outFile = File.Create(fo.FullName))
                {
                    using (DeflateStream Compress = new DeflateStream(outFile, CompressionMode.Compress))
                    {
                        // Copy the source file into the compression stream.
                        byte[] buffer = new byte[4096];
                        int numRead;
                        while ((numRead = inFile.Read(buffer, 0, buffer.Length)) != 0)
                            Compress.Write(buffer, 0, numRead);
                    }
                }
            }
        }

        public static void Decompress(string fi, string fo)
        {
            Decompress(new FileInfo(fi), new FileInfo(fo));
        }
        public static void Decompress(FileInfo fi, FileInfo fo)
        {
            // Get the stream of the source file. 
            using (FileStream inFile = fi.OpenRead())
            {
                //Create the decompressed file. 
                using (FileStream outFile = File.Create(fo.FullName))
                {
                    using (DeflateStream Decompress = new DeflateStream(inFile, CompressionMode.Decompress))
                    {
                        byte[] buffer = new byte[4096];
                        int numRead;
                        while ((numRead = Decompress.Read(buffer, 0, buffer.Length)) != 0)
                            outFile.Write(buffer, 0, numRead);
                    }
                }
            }
        }

        public static void DecompressGz(string fi, string fo, CopyToHandle callback)
        {
            DecompressGz(new FileInfo(fi), new FileInfo(fo), callback);
        }
        public static void DecompressGz(FileInfo fi, FileInfo fo, CopyToHandle callback)
        {
            DateTime st = DateTime.Now;
            long currentpos = 0;
            long CompressedOriginalFileSize = GetGzOriginalFileSize(fi);
            // Get the stream of the source file. 
            using (FileStream inFile = fi.OpenRead())
            {
                //Create the decompressed file. 
                using (FileStream outFile = File.Create(fo.FullName))
                {
                    using (GZipStream Decompress = new GZipStream(inFile, CompressionMode.Decompress))
                    {
                        byte[] buffer = new byte[4096];
                        int numRead;
                        while ((numRead = Decompress.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            currentpos += numRead;
                            outFile.Write(buffer, 0, numRead);
                            if (callback != null)
                                callback(new object(), new CopyToArgs(CompressedOriginalFileSize, currentpos, st));
                        }
                    }
                }
            }
        }

        public static int GetGzOriginalFileSize(FileInfo fi)
        {
            try
            {
                using (FileStream fs = fi.OpenRead())
                {
                    try
                    {
                        byte[] fh = new byte[3];
                        fs.Read(fh, 0, 3);
                        if (fh[0] == 31 && fh[1] == 139 && fh[2] == 8) //If magic numbers are 31 and 139 and the deflation id is 8 then...
                        {
                            byte[] ba = new byte[4];
                            fs.Seek(-4, SeekOrigin.End);
                            fs.Read(ba, 0, 4);
                            return BitConverter.ToInt32(ba, 0);
                        }
                        else
                            return -1;
                    }
                    finally
                    {
                        fs.Close();
                    }
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public static string DecompressTextFile(string fi)
        {
            return DecompressTextFile(new FileInfo(fi));
        }
        public static string DecompressTextFile(FileInfo fi)
        {
            try
            {
                // Get the stream of the source file. 
                using (FileStream inFile = fi.OpenRead())
                {
                    //Create the decompressed memorystream. 
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (DeflateStream Decompress = new DeflateStream(inFile, CompressionMode.Decompress))
                        {
                            byte[] buffer = new byte[4096];
                            int numRead;
                            while ((numRead = Decompress.Read(buffer, 0, buffer.Length)) != 0)
                                ms.Write(buffer, 0, numRead);
                        }
                        ms.Position = 0;
                        using (StreamReader sr = new StreamReader(ms))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
