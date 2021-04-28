﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900Link.Containers
{
    public class LibraryFile
    {
        private int _baseIndex = 0;
        private int _maxIndex = -1;
        private string _filename;
        private byte[] _filebytes;
        private LibraryFileFormat _filetype;
        private List<DeviceLibrary> _deviceLibraries = new List<DeviceLibrary>();


        public LibraryFile()
        {

        }

        public LibraryFile(string filename)
        {
            _filename = filename;

            if (_filename.ToLower().EndsWith(".lib"))
            {
                //.lib files are normal text human readable USER library files 
                _filetype = LibraryFileFormat.PlainText;
            }
            else if (_filename.ToLower().EndsWith(".li@"))
            {
                //.li@ files are generated by LibLoad.exe for download into the fluke
                _filetype = LibraryFileFormat.ASCIIEncodedBinary;
            }
            else if (_filename.ToLower().EndsWith(".li!"))
            {
                //.li! files are libload reference library files in pure binary with a different format
                _filetype = LibraryFileFormat.LibraryBinary;
            }
            else if (_filename.ToLower().EndsWith(".rom"))
            {
                _filetype = LibraryFileFormat.ROMBinary;
            }
            else
            {
                //unknown format, bad doggie
                throw new Exception("Unknown library format.");
            }
        }

        public int BaseIndex
        {
            get { return _baseIndex; }
        }

        public LibraryFileFormat FileType
        {
            get { return _filetype; }
        }

        public byte[] FileBytes
        {
            get { return _filebytes; }
        }

        public string FileName
        {
            get { return _filename; }
        }

        public List<DeviceLibrary> DeviceLibraries { get { return _deviceLibraries; } }


        public bool LoadLibraryFile()
        {
            if (!File.Exists(_filename))
            {
                throw new Exception("File does not exist or is not accessible: '" + _filename + "'");
            }

            _deviceLibraries.Clear();
            _filebytes = File.ReadAllBytes(_filename);

            //determine the type of library file, test first 3 bytes for now
            if (_filetype == LibraryFileFormat.ASCIIEncodedBinary)
            {
                //we will first change all the ASCII into straight binary
                _baseIndex = 0; //indeed
                List<byte> convertedBytes = Helpers.FileHelper.ASCIIEncode(_baseIndex, _filebytes);
               _filebytes = convertedBytes.ToArray();
                //now it is decoded, it is just a regular binary file
               _filetype = LibraryFileFormat.ROMBinary;

            }
            else if (_filetype == LibraryFileFormat.LibraryBinary)
            {
                //in library binaries, the ROM header is always 0x3F, so the first device definition is at 0x40
                _baseIndex = 0x40;

                int libListIndex = _baseIndex;
                List<LibraryListItem> listItems = new List<LibraryListItem>();

                while (_filebytes[libListIndex] != 0)
                {
                    //library items are a set of bytes of fixed length (0x1e)
                    LibraryListItem libItem = new LibraryListItem(_filebytes.Skip(libListIndex).Take(0x1e).ToArray());
                    listItems.Add(libItem);
                    libListIndex += 0x1e;
                }

                //we need to append and end library command here since the actual binary does not contain this
                byte[] endLib = new byte[] { 0x02, 0x06 };

                //now we have all our devices, load each library up
                //List<DeviceLibrary> deviceLibraries = new List<DeviceLibrary>();

                for (int i = 0; i < listItems.Count; i++ )
                {
                    LibraryListItem libitem = listItems[i];
                    //turn the item into an actual device library
                    DeviceLibrary deviceLibrary = GetLibrary(libitem.StartOffset);
                    _deviceLibraries.Add(deviceLibrary);
                }
            }
            else if (_filetype == LibraryFileFormat.PlainText)
            {
                throw new NotSupportedException("This file format is not yet implemented.");
            }

            //this test must be standalone from the main if/elseif because the ASCII format will 
            //convert to ROMBinary and then this has to run.
            if (_filetype == LibraryFileFormat.ROMBinary)
            {
                //in ROM's, the start and length are defined starting at 0x002d
                _baseIndex = _filebytes[0x002e] << 8 + _filebytes[0x002d];
                int liblength = _filebytes[0x0030] << 8 + _filebytes[0x002f];
                _maxIndex = _baseIndex + liblength;
            }

            return true;
        }

        private DeviceLibrary GetLibrary(int libraryBaseIndex)
        {
            DeviceLibrary deviceLibrary = new DeviceLibrary();
            int libraryIndex = libraryBaseIndex;
            bool deviceFinished = false;

            while (libraryIndex < _filebytes.Length && libraryIndex+1 < _filebytes.Length && !deviceFinished)
            {
                //iterate through the items
                DeviceLibraryItem item = new DeviceLibraryItem();
                int itemLength = _filebytes[libraryIndex];
                item.TypeDefinition = (DeviceLibraryConfigurationItem) _filebytes[libraryIndex+1];
                int itemIndex = 0;

                if (item.TypeDefinition == DeviceLibraryConfigurationItem.END_COMMANDGROUP)
                {
                    deviceFinished = true;
                }
                else if (item.TypeDefinition == DeviceLibraryConfigurationItem.END_LIBRARY_ALL)
                {
                    deviceFinished = true;
                }
                else if (item.TypeDefinition == DeviceLibraryConfigurationItem.SIMULATION_DATA)
                {
                    //these have a 2-byte address because they can be much longer arrays of data
                    if (itemLength == 4)
                    {
                        int binaryOffset = Helpers.Z80Helper.GetBigEndianNumber(_filebytes.Skip(libraryIndex + itemIndex  + 2).Take(2).ToArray());
                        //two byte binary lengths for SIM
                        int binaryLength = Helpers.Z80Helper.GetBigEndianNumber(_filebytes.Skip(libraryBaseIndex + binaryOffset).Take(2).ToArray());
                        item.SimulationData = _filebytes.Skip(libraryBaseIndex + binaryOffset + 2).Take(binaryLength - 2).ToList();
                    }
                    else
                    {
                        //just testing to see if we have more than a single array of data for these types
                    }
                }
                else if (item.TypeDefinition == DeviceLibraryConfigurationItem.SHADOW_DATA)
                {
                    //these have a 2-byte address because they can be much longer arrays of data
                    if (itemLength == 4)
                    {
                        int binaryOffset = Helpers.Z80Helper.GetBigEndianNumber(_filebytes.Skip(libraryIndex + itemIndex + 2).Take(2).ToArray());
                        //two byte binary lengths for SHADOW
                        int binaryLength = Helpers.Z80Helper.GetBigEndianNumber(_filebytes.Skip(libraryBaseIndex + binaryOffset).Take(2).ToArray());
                        item.ShadowData = _filebytes.Skip(libraryBaseIndex + binaryOffset + 2).Take(binaryLength - 2).ToList();
                    }
                    else
                    {
                        //just testing to see if we have more than a single array of data for these types
                    }
                }
                else if (item.TypeDefinition == DeviceLibraryConfigurationItem.RDTEST)
                {
                    itemIndex = 2;
                    while (itemIndex < itemLength)
                    {
                        //int binaryLength = _filebytes[libraryIndex + itemIndex];
                        //CommandBinaryObject binObj = null;
                        //this is the total length, it may contain multiple pointers of variable length
                        int chunkIndex = 0; //offset to first pointer length
                        while ((chunkIndex + itemIndex) < itemLength)
                        {
                            int chunkLength = _filebytes[libraryIndex + itemIndex + chunkIndex];

                            //binary pointers in libraries are always 16 bits... however they can be a single pointer or multiple
                            //pointers,
                            if (chunkLength == 3) //this is a single pointer
                            {
                                int binaryAddress = Helpers.Z80Helper.GetBigEndianNumber(_filebytes.Skip(libraryIndex + itemIndex + chunkIndex + 1).Take(2).ToArray());
                                //single byte binary lengths for RDTEST
                                int addressLength = _filebytes[libraryBaseIndex + binaryAddress];
                                List<byte[]> blist = new List<byte[]>();
                                blist.Add(_filebytes.Skip(libraryBaseIndex + binaryAddress).Take(addressLength).ToArray());
                                item.RDTestData.Add(blist);
                                chunkIndex += chunkLength;
                            }
                            else
                            {
                                List<byte[]> blist = new List<byte[]>();
                                for (int b = 0; b < (chunkLength - 1) / 2; b++)
                                {
                                    int binaryAddress = Helpers.Z80Helper.GetBigEndianNumber(_filebytes.Skip(libraryIndex + itemIndex + chunkIndex + (b*2) + 1 ).Take(2).ToArray());
                                    //single byte binary lengths for RDTEST
                                    int addressLength = _filebytes[libraryBaseIndex + binaryAddress];
                                    blist.Add(_filebytes.Skip(libraryBaseIndex + binaryAddress).Take(addressLength).ToArray());
                                }
                                item.RDTestData.Add(blist);
                                chunkIndex += chunkLength;
                            }
                        }
                        itemIndex += chunkIndex;
                    }
                }
                else
                {
                    if (itemLength > 2)
                    {
                        item.DataBytes = _filebytes.Skip(libraryIndex + 2).Take(itemLength - 2).ToArray();
                    }
                }

                deviceLibrary.Items.Add(item);
                libraryIndex += itemLength;
                //LogText("Added " + deviceLibrary.ToString());                
            }

            //LogText("Loaded '" + libfile.FileName + "'");
            return deviceLibrary;
        }

        //private BinaryObject GetBinaryObject(int binaryIndex, byte[] libraryBytes)
        //{
        //    BinaryObject binObj = null;
        //    if (binaryIndex < libraryBytes.Length)
        //    {
        //        binObj = new BinaryObject(libraryBytes.Skip(binaryIndex).Take(libraryBytes[binaryIndex]).ToArray());
        //    }
        //    else
        //    {
        //        int externalAddress = _baseIndex + binaryIndex;
        //        byte[] externalBytes = _filebytes.Skip(externalAddress).Take(_filebytes[externalAddress]).ToArray();
        //        binObj = new BinaryObject(externalBytes);
        //    }
        //    return binObj;
        //}

        private int GetBinaryFromVariablePointer(int index, byte[] bytes, List<byte> data, int addressOffset)
        {
            int addressLength = bytes[index]-1; //this will be 2-4 bytes, actual length is one less
            int dataAddress = Helpers.Z80Helper.GetBigEndianNumber(bytes.Skip(index + 1).Take(addressLength).ToArray()); //Convert.ToInt32(dataAddressString, 16);

            //adjust for any baseAddress value
            dataAddress += addressOffset;
            data.AddRange(GetRawBytes(bytes, dataAddress).ToList());
            return addressLength;
        }

        private byte[] GetRawBytes(byte[] bytes, int indexPointer)
        {
            int length = bytes[indexPointer];
            return bytes.Skip(indexPointer + 1).Take(length - 1).ToArray();
        }
    }
}
