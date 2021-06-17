using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Fluke900Link.Containers
{
    public class CTreeRecord
    {
        private byte[] _internalBytes = new byte[0];

        public byte RecordTypeId { get; set; }
        public UInt16 Length {
            get
            {
                return (UInt16)_internalBytes.Length;
            }
        }
        public List<CTreeColumn> Columns { get; set; } = new List<CTreeColumn>();
        public string Name
        {
            get { return Columns.Where(c => c.Name == "Name").First().Value.ToString(); }
        }

        public Int32 Ordinal
        {
            get
            {
                Int32 ordinalValue = -1;
                CTreeColumn ordinalColumn = Columns.Where(c => c.Name.ToLower() == "ordinal").FirstOrDefault();
                if (ordinalColumn != null)
                {
                    ordinalValue = Convert.ToInt32(ordinalColumn.Value);
                }
                return ordinalValue;
            }
        }

        public CTreeRecord() { }

        public CTreeRecord(CTreeSchema schema, byte[] bytes)
        {
            _internalBytes = bytes;
            RecordTypeId = bytes[0];
            int byteIndex = 6;
            foreach(var i in schema.Columns)
            {
                int endIndex = 0;
                switch (i.Type)
                {
                    case CTreeColumn.CTreeColumnType.String:
                        if (i.Length == 0)
                        {
                            //null terminated
                            endIndex = Array.IndexOf<byte>(bytes, 0x00, byteIndex);
                            i.Value = Encoding.ASCII.GetString(bytes.Skip(byteIndex).Take(endIndex - byteIndex).ToArray());
                            byteIndex += (1+ endIndex - byteIndex);
                        }
                        if (i.Length > 0)
                        {
                            //specific length
                            throw new NotSupportedException("Static Length String columns not supported.");
                            //byteIndex += i.Length;
                        }
                        break;
                    case CTreeColumn.CTreeColumnType.StringArray:
                        List<string> headerStrings = new List<string>();
                        endIndex = Array.IndexOf<byte>(bytes, 0x00, byteIndex);
                        byte[] arrayBytes = bytes.Skip(byteIndex).Take(endIndex - byteIndex).ToArray();
                        byteIndex += (1 + endIndex - byteIndex);
                        List<byte[]> headerBytes = SplitByteString(arrayBytes, 0x01, false);
                        foreach (byte[] b in headerBytes)
                        {
                            headerStrings.Add(Encoding.ASCII.GetString(b));
                        }
                        i.Value = headerStrings;
                        break;
                    case CTreeColumn.CTreeColumnType.ByteArray:
                        List<byte[]> byteArray = new List<byte[]>();
                        for (int a = 0; a < i.Length; a++)
                        {
                            endIndex = Array.IndexOf<byte>(bytes, 0x00, byteIndex);
                            byteArray.Add(bytes.Skip(byteIndex).Take(endIndex - byteIndex).ToArray());
                            byteIndex += (1 + endIndex - byteIndex);
                        }
                        i.Value = byteArray;
                        break;
                    case CTreeColumn.CTreeColumnType.Number:
                        switch (i.Length)
                        {
                            case 2:
                                i.Value = BitConverter.ToUInt16(bytes, byteIndex);
                                break;
                            case 4:
                                i.Value = BitConverter.ToUInt32(bytes, byteIndex);
                                break;
                        }
                        byteIndex += i.Length;
                        break;
                    case CTreeColumn.CTreeColumnType.Date:
                        throw new NotSupportedException("Date columns not supported.");
                        break;
                }
                Columns.Add(i);
            }
        }

        private List<byte[]> SplitByteString(byte[] bytes, byte splitValue, bool removeEmptyEntries)
        {
            List<byte[]> splitBytes = new List<byte[]>();
            int currentIndex = 0;
            while (currentIndex < bytes.Length)
            {
                int splitIndex = Array.IndexOf<byte>(bytes, splitValue, currentIndex);
                if (splitIndex >= 0)
                {
                    //found one
                    int currentLength = splitIndex - currentIndex;
                    if (!(currentLength == 0 && removeEmptyEntries))
                    {
                        splitBytes.Add(bytes.Skip(currentIndex).Take(currentLength).ToArray());
                        currentIndex += (1 + currentLength);
                    }
                    else
                    {
                        currentIndex++;
                    }
                }
                else
                {
                    //not found, just take until end now..
                    splitBytes.Add(bytes.Skip(currentIndex).Take(bytes.Length - currentIndex).ToArray());
                    currentIndex += (bytes.Length - currentIndex);
                }
                
            }
            return splitBytes;
        }
    }
}
