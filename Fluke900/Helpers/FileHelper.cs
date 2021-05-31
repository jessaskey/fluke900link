using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace Fluke900.Helpers
{
    /// <summary>
    /// Contains methods to support file operations between the PC and Fluke
    /// </summary>
    public static class FileHelper
    {

        /// <summary>
        /// Determines the location of the file based upon the suffix of the file. The comparison is case insensitive. If no known Location
        /// is found, the result is null.
        /// </summary>
        /// <param name="fileName">A full or partial path, filename and location string.</param>
        /// <returns>The encoded Location of the File.</returns>
        public static async Task<FileLocations?> GetFileLocation(string fileName)
        {
            if (fileName.ToUpper().EndsWith(":CART")) return FileLocations.FlukeCartridge;
            else if (fileName.ToUpper().EndsWith(":SYST")) return FileLocations.FlukeSystem;
            else if (fileName.ToUpper().EndsWith(":PC")) return FileLocations.LocalComputer;
            return null;
        }

        /// <summary>
        /// Appends the proper location suffix to the filename. If there is already a suffix on the source filename as denoted by the 
        /// presence of a colon ':' character, an exception is thrown.
        /// </summary>
        /// <param name="filename">The bare filename that will have the location appended.</param>
        /// <param name="location">The FileLocation enum of the location to append.</param>
        /// <returns></returns>
        public static string AppendLocation(string filename, FileLocations location)
        {
            int colonIndex = Path.GetFileName(filename).IndexOf(':');
            if (colonIndex >= 0)
            {
                throw new Exception("passed filename '" + filename + "' already has a location suffix.");
            }

            switch (location)
            {
                case FileLocations.FlukeCartridge:
                    return filename + ":CART";
                case FileLocations.FlukeSystem:
                    return filename + ":SYST";
                case FileLocations.LocalComputer:
                    return filename + ":PC";
            }
            return filename;
        }

        /// <summary>
        /// Trims any prefix path info and any suffix file location info off the passed filename;
        /// </summary>
        /// <param name="fileName">A full or partial path, filename and location string.</param>
        /// <returns>The bare filename with any path and location information removed.</returns>
        public static string GetFilenameOnly(string fileName)
        {
            string trimmedFileName = fileName;
            //trims off the Path if it exists
            int lastSlash = trimmedFileName.LastIndexOf("\\");
            if (lastSlash > 0)
            {
                trimmedFileName = trimmedFileName.Substring(lastSlash + 1, trimmedFileName.Length - lastSlash - 1);
            }
            //returns all characters BEFORE the delimiting colon
            int colonPos = trimmedFileName.LastIndexOf(":");
            if (colonPos >= 0)
            {
                trimmedFileName = trimmedFileName.Substring(0, colonPos);
            }

            return trimmedFileName.Trim();
        }

        /// <summary>
        /// Removes any file location suffix from the path, file and location string.
        /// </summary>
        /// <param name="fileName">A full or partial path, filename and location string.</param>
        /// <returns>Only path and filename data is returned.</returns>
        public static string RemoveFileLocation(string fileName)
        {
            string trimmedFileName = fileName;
            //returns all characters BEFORE the delimiting colon
            int colonPos = fileName.LastIndexOf(":");
            if (colonPos > 0)
            {
                trimmedFileName = fileName.Substring(0, colonPos);
            }
            return trimmedFileName;
        }

        /// <summary>
        /// Method will attempt to convert a passed filename into the KnownFileType enumeration. If no match is found, then null is returned.
        /// </summary>
        /// <param name="filename">The filename to enumerate</param>
        /// <returns>The matched KnownFileType</returns>
        public static KnownFileType? FilenameToKnownFileType(string filename)
        {
            string extension = Path.GetExtension(filename).ToLower().Replace(".","");
            KnownFileType fileType = KnownFileType.Lib; //this is a placeholder only
            if (Enum.TryParse<KnownFileType>(extension, true, out fileType))
            {
                return fileType;
            }
            if (extension == "li@")
            {
                return KnownFileType.Lib;
            }
            return null;
        }

        /// <summary>
        /// Changes file extensions to the proper text based upon the target device (case + @ symbols).
        /// </summary>
        /// <param name="sourceFilename">The filename that is being transferred</param>
        /// <returns>The modified filename for the target device.</returns>
        public static string AdjustForTransfer(string sourceFilename)
        {
            if (sourceFilename.Contains(".lib")
                || sourceFilename.Contains(".loc")
                || sourceFilename.Contains(".seq")
                || sourceFilename.Contains(".nsq")
                || sourceFilename.Contains(".LI@")
                || sourceFilename.Contains(".LO@")
                || sourceFilename.Contains(".SE@")
                  || sourceFilename.Contains(".NSQ")
                )
            {
                if (sourceFilename.Contains("@"))
                {
                    //we are going from PC to Fluke... remove @ and make extension lower-case
                    return sourceFilename.Replace(".LI@", ".lib").Replace(".LO@", ".loc").Replace(".SE@", ".seq");
                }
                else
                {
                    //Fluke to PC
                    return sourceFilename.Replace(".lib", ".LI@").Replace(".loc", ".LO@").Replace(".seq", ".SE@").Replace(".nsq",".NSQ");
                }
            }
            return sourceFilename;
        }

        /// <summary>
        /// Method to simply encode a binary array into an ASCII formatted file which is suitable for transmission over the RS-232 port.
        /// </summary>
        /// <param name="baseIndex">The offset to start decoding from. An offset of zero will decode the entire passed array.</param>
        /// <param name="sourceBytes">The binary data to convert.</param>
        /// <returns></returns>
        public static List<byte> ASCIIEncode(int baseIndex, byte[] sourceBytes)
        {
            //need to convert the ASCII into binary..
            int i = 0;

            List<byte> convertedBytes = new List<byte>();

            while (i < sourceBytes.Length)
            {
                byte byte1 = sourceBytes[i];
                byte byte2 = sourceBytes[i + 1];

                //and back to binary
                string bytecode = System.Text.Encoding.ASCII.GetString(new[] { byte1, byte2 });
                if (bytecode != "\r\n")
                {
                    convertedBytes.Add((byte)Convert.ToInt16(bytecode, 16));
                }

                //advance to next block
                if ((i + 2) < sourceBytes.Length)
                {
                    if (sourceBytes[i + 2] == 0x20)
                    {
                        i += 3;
                    }
                    else if (sourceBytes[i + 2] == 0x0d)
                    {
                        i += 4;
                    }
                    else
                    {
                        i++;
                    }
                }
                else
                {
                    i += 2;
                }
            }

            return convertedBytes;
        }

        /// <summary>
        /// Method that should be called prior to any transfer to ensure it meets the requirements to exist on the Fluke 900. This method
        /// will show MessageBoxes for any issues found.
        /// </summary>
        /// <param name="filename">The filename to check</param>
        /// <returns>A boolean value that is true if the file passes all requirements.</returns>
        public static bool ValidateFilename(string filename, List<string> errors)
        {
            bool isValid = true;

            if (filename.Length > 12)
            {
                errors.Add("Filename must be less than 12 characters.");
                isValid = false;
            }
            //must start with a alpha character
            if (!Regex.Match(filename, @"^[a-zA-Z].*").Success)
            {
                errors.Add("Filename must start with a letter.");
                isValid = false;
            }
            return isValid;
        }

        public static string GetTemplate(string extension, string defaultFilesDirectory)
        {
            string templateFilename = Path.Combine(defaultFilesDirectory, "_Templates", extension.Replace(".", "") + ".template");
            if (File.Exists(templateFilename))
            {
                return File.ReadAllText(templateFilename);
            }
            return null;
        }

    }
}
