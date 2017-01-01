using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900Link
{
    public enum RemoteCommandChars : byte
    {
        StartCommand = 0x02,
        EndCommand = 0x03,
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
        public static char STX { get { return ((char)RemoteCommandChars.StartCommand); } }
        public static char ETX { get { return ((char)RemoteCommandChars.EndCommand); } }
        public static char EOT { get { return ((char)RemoteCommandChars.EndOfTransmission); } }
        public static char ACK { get { return ((char)RemoteCommandChars.Acknowledge); } }
        public static char CR  { get { return ((char)RemoteCommandChars.CarriageReturn); } }
        public static char ABO { get { return ((char)RemoteCommandChars.Abort); } }
        public static char DC1 { get { return ((char)RemoteCommandChars.DeviceControl1); } }
        public static char DC2 { get { return ((char)RemoteCommandChars.DeviceControl2); } }
        public static char DC3 { get { return ((char)RemoteCommandChars.DeviceControl3); } }
        public static char DC4 { get { return ((char)RemoteCommandChars.DeviceControl4); } }
        public static char NAK { get { return ((char)RemoteCommandChars.NegativeAcknowledge); } }
        public static char SUB { get { return ((char)RemoteCommandChars.CommandAccepted); } }
    }
}
