using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900.Containers
{
    public enum CommandCharacters : byte
    {
        StartText = 0x02,
        EndText = 0x03,
        EndOfTransmission = 0x04,
        Acknowledge = 0x06,
        CarriageReturn = 0x0d,
        Abort = 0x10,
        DeviceControl1 = 0x11,
        DeviceControl2 = 0x12,
        DeviceControl3 = 0x13,
        DeviceControl4 = 0x14,
        NegativeAcknowledge = 0x15,
        CommandAccepted = 0x1a
    }

    /// <summary>
    /// Constants for Serial Port Communication
    /// </summary>
    public static class TransmissionCharacters
    {
        public static char STX { get { return ((char)CommandCharacters.StartText); } }
        public static char ETX { get { return ((char)CommandCharacters.EndText); } }
        public static char EOT { get { return ((char)CommandCharacters.EndOfTransmission); } }
        public static char ACK { get { return ((char)CommandCharacters.Acknowledge); } }
        public static char CR  { get { return ((char)CommandCharacters.CarriageReturn); } }
        public static char ABO { get { return ((char)CommandCharacters.Abort); } }
        public static char DC1 { get { return ((char)CommandCharacters.DeviceControl1); } }
        public static char DC2 { get { return ((char)CommandCharacters.DeviceControl2); } }
        public static char DC3 { get { return ((char)CommandCharacters.DeviceControl3); } }
        public static char DC4 { get { return ((char)CommandCharacters.DeviceControl4); } }
        public static char NAK { get { return ((char)CommandCharacters.NegativeAcknowledge); } }
        public static char SUB { get { return ((char)CommandCharacters.CommandAccepted); } }
    }
}
