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
        public Tuple<List<byte>,List<CommandBinaryObject>> GetBinary(bool isLastLib)
        {
            List<byte> deviceBytes = new List<byte>();

            List<CommandBinaryObject> pointers = new List<CommandBinaryObject>();

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
                                pointers.Add(new CommandBinaryObject(item.TypeDefinition, deviceBytes.Count, bytes));
                                deviceBytes.AddRange(new byte[] { 0x0, 0x0 }); //dummy pointer, will be populated later
                            }
                        }
                        deviceBytes[itemIndexBase] = (byte)totalLength;
                        //pointers will be fixed later

                        break;
                    case DeviceLibraryConfigurationItem.SHADOW_DATA:
                        deviceBytes.Add(0x04); //length of all pointer data
                        deviceBytes.Add((byte)item.TypeDefinition);
                        //pointers.Add(new Tuple<int, byte[]>(deviceBytes.Count, item.ShadowData.ToArray()));
                        pointers.Add(new CommandBinaryObject(item.TypeDefinition, deviceBytes.Count, item.ShadowData.ToArray()));
                        deviceBytes.AddRange(new byte[] { 0x0, 0x0 }); //dummy pointer, will be populated later
                        break;
                    case DeviceLibraryConfigurationItem.SIMULATION_DATA:
                        deviceBytes.Add(0x04); //length of all pointer data
                        deviceBytes.Add((byte)item.TypeDefinition);
                        pointers.Add(new CommandBinaryObject(item.TypeDefinition, deviceBytes.Count, item.SimulationData.ToArray()));
                        //pointers.Add(new Tuple<int, byte[]>(deviceBytes.Count, item.SimulationData.ToArray()));
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

            return new Tuple<List<byte>, List<CommandBinaryObject>>(deviceBytes, pointers);
        }


        #region LibraryItem declarations

        public CmdSize Size
        {
            get
            {
                DeviceLibraryItem sizeItem = Items.Where(i => i.TypeDefinition == DeviceLibraryConfigurationItem.SIZE).FirstOrDefault();
                if (sizeItem != null)
                {
                    return new CmdSize(sizeItem.DataBytes);
                }
                return null;
            }
        }

        #endregion
    }
}
