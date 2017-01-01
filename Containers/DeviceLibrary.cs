using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900Link.Containers
{
    public class DeviceLibrary
    {
        public List<DeviceLibraryItem> Items = new List<DeviceLibraryItem>();

        public override string ToString()
        {
            string[] names = Items.Where(i => i.TypeDefinition == DeviceLibraryConfigurationItem.NAME).Select(i => i.Data).ToArray();
            if (names.Length > 0)
            {
                return String.Join(",", names);
            }
            return base.ToString();
        }

        /// <summary>
        /// Returns an equivalent byte array of data to represent this device library
        /// </summary>
        /// <returns>An ordered list of bytes.</returns>
        public List<byte> AsBinary(bool isLastLib)
        {
            List<byte> deviceBytes = new List<byte>();

            List<Tuple<int, byte[]>> pointers = new List<Tuple<int, byte[]>>();

            foreach(DeviceLibraryItem item in Items)
            {
                switch (item.TypeDefinition)
                {
                    case DeviceLibraryConfigurationItem.RDTEST:
                        int totalLength = 2; //start here
                        int itemIndexBase = deviceBytes.Count; //this is what we will use to overwrite for the data length
                        deviceBytes.Add(0x0); //empty length, we will overwrite this later
                        deviceBytes.Add((byte)item.TypeDefinition);

                        foreach(List<byte[]> byteList in item.RDTestData)
                        {
                            int chunkLength = (byteList.Count * 2) + 1;
                            totalLength += chunkLength;
                            deviceBytes.Add((byte)chunkLength); //length of all chunk data
                            foreach (byte[] bytes in byteList)
                            {
                                pointers.Add(new Tuple<int, byte[]>(deviceBytes.Count, bytes));
                                deviceBytes.AddRange(new byte[] { 0x0, 0x0 }); //dummy pointer, will be populated later
                            }
                        }
                        deviceBytes[itemIndexBase] = (byte)totalLength;
                        //pointers will be fixed later

                        break;
                    case DeviceLibraryConfigurationItem.SHADOW_DATA:
                        deviceBytes.Add(0x04); //length of all pointer data
                        deviceBytes.Add((byte)item.TypeDefinition);
                        pointers.Add(new Tuple<int, byte[]>(deviceBytes.Count, item.ShadowData.ToArray()));
                        deviceBytes.AddRange(new byte[] { 0x0, 0x0 }); //dummy pointer, will be populated later
                        break;
                    case DeviceLibraryConfigurationItem.SIM_DATA:
                        deviceBytes.Add(0x04); //length of all pointer data
                        deviceBytes.Add((byte)item.TypeDefinition);
                        pointers.Add(new Tuple<int, byte[]>(deviceBytes.Count, item.SimulationData.ToArray()));
                        deviceBytes.AddRange(new byte[] { 0x0, 0x0 }); //dummy pointer, will be populated later
                        break;
                    default:
                        if (item.DataBytes == null)
                        {
                            deviceBytes.Add((byte)2);
                            deviceBytes.Add((byte)item.TypeDefinition);
                        }
                        else
                        {
                            deviceBytes.Add((byte)(item.DataBytes.Length + 2));
                            deviceBytes.Add((byte)item.TypeDefinition);
                            deviceBytes.AddRange(item.DataBytes);
                        }
                        break;
                }
            }

            if (isLastLib)
            {
                deviceBytes.AddRange(new byte[] { 0x02, (byte)DeviceLibraryConfigurationItem.END_LIBRARY_ALL });
            }

            //now do binary pointers, always 2-byte
            foreach(Tuple<int,byte[]> pointer in pointers)
            {
                int binaryStart = deviceBytes.Count;
                int binaryLength = pointer.Item2.Length + 2;
                //push length first
                deviceBytes.Add((byte)(binaryLength & 0xff));
                deviceBytes.Add((byte)((binaryLength >> 8) & 0xff));
                deviceBytes.AddRange(pointer.Item2);
                //fix the pointer
                deviceBytes[pointer.Item1] = (byte)(binaryStart & 0xff);
                deviceBytes[pointer.Item1 + 1] = (byte)((binaryStart >> 8) & 0xff);
            }

            return deviceBytes;
        }
    }
}
