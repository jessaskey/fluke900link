using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Fluke900Link.Containers;

namespace Fluke900Link.Helpers
{
    public static class LibraryHelper
    {
        private static string _referenceLibraryFile = "Libraries/F900LIB_2_06.LI!";
        private static List<DeviceLibrary> _referenceLibraries = null;

        public static bool LoadReferenceLibrary()
        {
            bool errors = false;

            if (_referenceLibraries == null)
            {
                _referenceLibraries = new List<DeviceLibrary>();
            }
            else{
                _referenceLibraries.Clear();
            }

            string localReferenceLibraryFile = Path.Combine(Utilities.GetExecutablePath(), _referenceLibraryFile);

            if (File.Exists(localReferenceLibraryFile))
            {
                LibraryFile referenceLibrary = new LibraryFile(localReferenceLibraryFile);
                if (referenceLibrary.LoadLibraryFile())
                {
                    _referenceLibraries.AddRange(referenceLibrary.DeviceLibraries);
                }
            }
            else
            {
                return false;
            }

            return errors;
        }

        public static bool HasDevice(string deviceName)
        {
            return GetDeviceLibrary(deviceName, false) != null;
        }

        private static DeviceLibrary GetDeviceLibrary(string deviceName)
        {
            return _referenceLibraries.Where(l=>l.Items.Where(i=>i.TypeDefinition == DeviceLibraryConfigurationItem.NAME && i.Data.ToLower() == deviceName.ToLower()).Count() > 0).FirstOrDefault();
        }

        public static DeviceLibrary GetDeviceLibrary(string deviceName, bool forceReload)
        {
            if (_referenceLibraries == null || forceReload)
            {
                LoadReferenceLibrary();
            }

            return GetDeviceLibrary(deviceName);
        }

        public static List<byte> GetDeviceLibraries(List<string> deviceList, LibraryFileFormat format)
        {
            List<DeviceLibrary> deviceLibraries = _referenceLibraries.Where(dl => dl.Items.Where(i => i.TypeDefinition == DeviceLibraryConfigurationItem.NAME && deviceList.Contains(i.Data)).Count() > 0).Distinct().ToList();

            List<byte> outputBytes = new List<byte>();

            foreach (DeviceLibrary deviceLibrary in deviceLibraries)
            {
                List<byte> libraryBytes = deviceLibrary.AsBinary(deviceLibrary == deviceLibraries[deviceLibraries.Count - 1]);
                if (libraryBytes != null)
                {
                    outputBytes.AddRange(libraryBytes);
                }
            }

            switch (format)
            {
                case LibraryFileFormat.LibraryBinary:
                    //nothing to do here
                    break;
                case LibraryFileFormat.ASCIIEncodedBinary:
                    if (outputBytes != null && outputBytes.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        int byteCounter = 0;
                        foreach (byte b in outputBytes)
                        {
                            //encode binary to ASCII
                            sb.Append(b.ToString("X2"));
                            if (byteCounter < 15)
                            {
                                byteCounter++;
                                sb.Append(" ");
                            }
                            else
                            {
                                byteCounter = 0;
                                sb.Append("\r\n");
                            }
                        }
                        //rewrite outputBytes now in our new ASCII
                        outputBytes.Clear();
                        outputBytes.AddRange(Encoding.ASCII.GetBytes(sb.ToString()));
                    }
                    break;
                default:
                    //this is bad, just return normal binary for now
                    break;
            }
            return outputBytes;
        }


    }
}
