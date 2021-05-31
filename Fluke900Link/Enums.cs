using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900Link
{
    public enum SerialCableType : int
    {
        StraightThrough = 0,
        NullModem = 1
    }


    public enum InitialTreeStatus
    {
        Collapsed,
        FirstNodeExpanded,
        AllNodesExpanded
    }



    public enum OtherErrors : int
    {
        CartridgeNotInserted = 0x14
    }



    public enum TerminalWindowTypes
    {
        Raw,
        Formatted
    }

    public enum SoundCode : int {
        PowerUp = 0,
        Acknowledge = 1,
        Keypress = 2,
        Error = 3,
        Pass = 4,
        Fail = 5
    }


    public enum DeviceLibraryConfigurationItem
    {
        ACTIVITY = 0x00,
        BINARY = 0x01,
        VECTORS = 0x02,
        UNKNOWN_03 = 0x03,
        UNKNOWN_04 = 0x04,
        UNKNOWN_05 = 0x05,
        END_COMMANDGROUP = 0x06,
        UNKNOWN_07 = 0x07,
        UNKNOWN_08 = 0x08,
        UNKNOWN_09 = 0x09,
        UNKNOWN_0a = 0x0a,
        RDTEST = 0x0b,
        UNKNOWN_0c = 0x0c,
        NAME = 0x0d,
        UNKNOWN_0e = 0x0e,
        UNKNOWN_0f = 0x0f,
        SIZE = 0x10,
        S_TIME = 0x11,
        UNKNOWN_12 = 0x12,
        UNKNOWN_13 = 0x13,
        UNKNOWN_14 = 0x14,
        UNKNOWN_15 = 0x15,
        UNKNOWN_16 = 0x16,
        UNKNOWN_17 = 0x17,
        UNKNOWN_18 = 0x18,
        UNKNOWN_19 = 0x19,
        UNKNOWN_1a = 0x1a,
        END_LIBRARY_ALL = 0x1b,
        UNKNOWN_1c = 0x1c,
        RDT_ENABLE = 0x1d,
        UNKNOWN_1e = 0x1e,
        UNKNOWN_1f = 0x1f,
        UNKNOWN_20 = 0x20,
        CLIP_CHK = 0x21,
        UNKNOWN_22 = 0x22,
        UNKNOWN_23 = 0x23,
        SYNC_COND = 0x24,
        SYNC_VECT = 0x25,
        UNKNOWN_26 = 0x26,
        UNKNOWN_27 = 0x27,
        SYNC_RND = 0x28,
        UNKNOWN_29 = 0x29,
        UNKNOWN_2A = 0x2a,
        SYNC_PINS = 0x2b,
        UNKNOWN_2c = 0x2c,
        UNKNOWN_2d = 0x2d,
        SHADOW_DATA = 0x2e,
        SIMULATION_DATA= 0x2f,
        UNKNOWN_30 = 0x30,
        UNKNOWN_31 = 0x31,
        UNKNOWN_32 = 0x32,
        UNKNOWN_33 = 0x33,
        UNKNOWN_34 = 0x34,
        UNKNOWN_35 = 0x35
    }


    public enum ProjectNodeType : int
    {
        Project = 0,
        LibraryFile = 1,
        LocationFile = 2,
        SequenceFile = 3,
        Location = 4,
        Test = 5
    }


}
