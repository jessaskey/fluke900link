using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900Link.Containers
{
    public class CTreeColumn
    {
        public enum CTreeColumnType
        {
            String,
            Number,
            Date,
            StringArray,
            ByteArray
        }

        public string Name { get; set; }
        public int Length { get; set; }
        public CTreeColumnType Type { get; set; }
        public object Value { get; set; }

        public CTreeColumn()
        {

        }

        public CTreeColumn(string name, CTreeColumnType type, int length)
        {
            Name = name;
            Type = type;
            Length = length;
        }
    }
}
