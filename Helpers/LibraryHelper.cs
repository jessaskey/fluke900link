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
            bool success = false;

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
                    success = true;
                }
            }
            return success;
        }

        public static bool HasDevice(string deviceName)
        {
            return GetDeviceLibrary(deviceName, false) != null;
        }

        private static DeviceLibrary GetDeviceLibrary(string deviceName)
        {
            return _referenceLibraries.Where(l=>l.Items.Where(i=>i.TypeDefinition == DeviceLibraryConfigurationItem.NAME && i.Data.ToLower() == deviceName.ToLower()).Count() > 0).FirstOrDefault();
        }

        public static List<string> GetUniqueDevices()
        {
            List<string> deviceList = new List<string>();

            foreach (DeviceLibrary lib in _referenceLibraries)
            {
                foreach (string name in lib.Items.Where(i => i.TypeDefinition == DeviceLibraryConfigurationItem.NAME).Select(i=>i.Data))
                {
                    if (!deviceList.Contains(name))
                    {
                        deviceList.Add(name);
                    }
                }
            }

            return deviceList.OrderBy(i => i).ToList();
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
            List<CommandBinaryObject> adjustedPointers = new List<CommandBinaryObject>();
            //List<Tuple<List<byte>,List<Tuple<int, byte[]>>>> libraries = new List<Tuple<List<byte>,List<Tuple<int,byte[]>>>>();

            foreach (DeviceLibrary deviceLibrary in deviceLibraries)
            {
                Tuple<List<byte>,List<CommandBinaryObject>> library = deviceLibrary.GetBinary(deviceLibrary == deviceLibraries[deviceLibraries.Count - 1]);
                if (library != null)
                {
                    int libraryBase = outputBytes.Count;
                    outputBytes.AddRange(library.Item1);
                    //need to adjust pointer data here because the pointer offset is relative to the library start
                    foreach (CommandBinaryObject pointer in library.Item2)
                    {
                        CommandBinaryObject adjustedPointer = new CommandBinaryObject(pointer.Item, pointer.Offset + libraryBase, pointer.Bytes);
                        adjustedPointers.Add(adjustedPointer);
                    }
                }
                //libraries.Add(library);
            }
            outputBytes.AddRange(new byte[] { 0x02, (byte)DeviceLibraryConfigurationItem.END_LIBRARY_ALL });
            
            //we need to append the binary data next, and keep track of pointer changes
            //we do this in order by type
            foreach (CommandBinaryObject pointer in adjustedPointers.Where(p=>p.Item == DeviceLibraryConfigurationItem.RDTEST))
            {
                //fix the pointer
                outputBytes[pointer.Offset] = (byte)(outputBytes.Count & 0xff);
                outputBytes[pointer.Offset + 1] = (byte)((outputBytes.Count >> 8) & 0xff);
                //and data
                outputBytes.AddRange(pointer.Bytes);
            }
            foreach (CommandBinaryObject pointer in adjustedPointers.Where(p => p.Item == DeviceLibraryConfigurationItem.SHADOW_DATA))
            {
                //fix the pointer
                outputBytes[pointer.Offset] = (byte)(outputBytes.Count & 0xff);
                outputBytes[pointer.Offset + 1] = (byte)((outputBytes.Count >> 8) & 0xff);
                //prepend the length (two bytes)
                outputBytes.Add((byte)((pointer.Bytes.Length + 2) & 0xff));
                outputBytes.Add((byte)(((pointer.Bytes.Length + 2) >> 8) & 0xff));
                //and data
                outputBytes.AddRange(pointer.Bytes);
            }
            foreach (CommandBinaryObject pointer in adjustedPointers.Where(p => p.Item == DeviceLibraryConfigurationItem.SIMULATION_DATA))
            {
                //fix the pointer
                outputBytes[pointer.Offset] = (byte)(outputBytes.Count & 0xff);
                outputBytes[pointer.Offset + 1] = (byte)((outputBytes.Count >> 8) & 0xff);
                //prepend the lengthh (two bytes)
                outputBytes.Add((byte)((pointer.Bytes.Length + 2) & 0xff));
                outputBytes.Add((byte)(((pointer.Bytes.Length + 2) >> 8) & 0xff));
                //and data
                outputBytes.AddRange(pointer.Bytes);
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
