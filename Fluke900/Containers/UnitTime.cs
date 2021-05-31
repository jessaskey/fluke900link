using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Fluke900.Containers
{
    public class UnitTime
    {

        public int Time { get; set; }
        public UnitDefinition Unit { get; set; }

        public UnitTime() { }

        public override string ToString()
        {
            return Time.ToString() + Enum.GetName(typeof(UnitDefinition), Unit).ToLower();
        }

        public static UnitTime Parse(string value)
        {
            Match match = Regex.Match(value, @"(\d+)([a-zA-Z]+)");
            if (match.Success)
            {
                UnitTime ut = new UnitTime();
                ut.Time = int.Parse(match.Groups[1].Value);
                string unit = match.Groups[2].Value.ToLower();
                if (unit == "ns")
                {
                    ut.Unit = UnitDefinition.Ns;
                }
                else if (unit == "ms")
                {
                    ut.Unit = UnitDefinition.Ms;
                }
                return ut;
            }
            return null;
        }
    }
}
