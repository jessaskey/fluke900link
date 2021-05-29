using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900
{
    public enum UnitDefinition
    {
        Ns,
        Ms
    }
    public enum GatePinDefinition
    {
        DontCare = -1,
        False = 0,
        True = 1
    }

    public enum TriggerExt1Definition {
        DontCare = -1,
        False = 0,
        True = 1
    }

    public enum TriggerExt2Definition
    {
        DontCare = -1,
        False = 0,
        True = 1
    }

    public enum GateExtDefinition
    {
        DontCare = -1,
        False = 0,
        True = 1
    }

    public enum GateIgnoreCompareDefinition
    {
        Ignore = 0,
        Compare = 1
    }

    public enum TriggerWord1Definition
    {
        DontCare = -1,
        False = 0,
        True = 1
    }

    public enum TriggerWord2Definition
    {
        DontCare = -1,
        False = 0,
        True = 1
    }

    public enum FileLocationCopyBehavior : int
    {
        System,
        Cartridge,
        SystemCartridgeDefault,
        Optimized
    }
    public enum PinActivityDefinition : int
    {
        Frequency = 0,
        DontCare = 1,
        High = 2,
        Low = 4,
        Activity = 8
    }

    public enum IgnoreDefinition
    {
        Ignore,
        Compare
    }

    public enum FloatCheckDefinition : int
    {
        NotChecked = 0,
        Inactive
    }

    public enum SimulationShadowDefinition
    {
        NotInstalled = -1,
        Disabled = 0,
        Enabled = 1
    }

    public enum FrequencyUnitDefinition
    {
        Hz,
        kHz,
        MHz
    }
    public enum LibraryFileFormat
    {
        LibraryBinary,
        ROMBinary,
        ASCIIEncodedBinary,
        PlainText
    }

    public enum KnownFileType
    {
        Lib,
        Loc,
        Seq
    }

    public enum FileLocations
    {
        LocalComputer,
        FlukeSystem,
        FlukeCartridge
    }

    public enum CompilationError
    {
        None,
        Unknown,
        NotSpecified,
        DuplicateLabel,
        LabelNotFound,
        DefaultsMissing,
        LocationMissingInSequence

    }
    public enum ConnectionStatus
    {
        Connected,
        Disconnected,
        Unknown
    }
    public enum BaudRates : int
    {
        Rate4800 = 4800,
        Rate9600 = 9600,
        Rate19200 = 19200
    }

    public enum DataBits : int
    {
        Bits5 = 5,
        Bits6 = 6,
        Bits7 = 7,
        Bits8 = 8
    }

    public enum CommandResponseStatus
    {
        Success,
        Error,
        Aborted,
        Accepted
    }

    public enum CommunicationDirection
    {
        Idle,
        Send,
        Receive
    }

    public enum RemoteCommandError : byte
    {
        ParityError = ConsoleKey.A,
        BreakDetected = ConsoleKey.B,
        FramingError = ConsoleKey.C,
        OverrunError = ConsoleKey.D,
        OtherComError = ConsoleKey.E,
        InvalidCommand = ConsoleKey.F,
        SyntaxError = ConsoleKey.G,
        CommandAborted = ConsoleKey.H,
        DeviceNotFound = ConsoleKey.I,
        ParameterOutOfRange = ConsoleKey.J,
        FileNotFound = ConsoleKey.K,
        InvalidFileType = ConsoleKey.L,
        LocationNotFound = ConsoleKey.M,
        ExecutionError = ConsoleKey.N,
        CommandSpecificError = ConsoleKey.Y,
        OtherError = ConsoleKey.Z
    }

    public enum FileCommand : byte
    {
        ACTIVITY = 0x00,
        BINARY = 0x01,
        VECTORS = 0x02,
        DISPLAY = 0x03,
        END = 0x04,
        COMPARE = 0x05,
        END_COMMANDGROUP = 0x06,
        F_MASK = 0x07,
        GATE = 0x08,
        GLOBAL = 0x09,
        LOAD = 0x0a,
        RDTEST = 0x0b,
        LOC_FILE = 0x0c,
        NAME = 0x0d,
        RESET = 0x0e,
        RD_DRV = 0x0f,
        SIZE = 0x10,
        S_TIME = 0x11,
        TEST = 0x12,
        THRSLD = 0x13,
        TRIGGER = 0x14,
        T_TIME = 0x15,
        W_TIME = 0x16,
        FUNCTION = 0x17,
        END_FUNCTION = 0x18,
        JUMP = 0x19,
        IF = 0x1a,
        IGNORE = 0x1b,
        C_SUM = 0x1c,
        RDT_ENABLE = 0x1d,
        SOUND = 0x1e,
        DO_TEST = 0x1f,
        UNKNOWN_20 = 0x20,
        CLIP_CHK = 0x21,
        UNKNOWN_22 = 0x22,
        COMMENT = 0x23,
        SYNC_COND = 0x24,
        SYNC_VECT = 0x25,
        SYNC_PAT = 0x26,
        SYNC_GATE = 0x27,
        SYNC_RND = 0x28,
        SYNC_GR_END = 0x29,
        SYNC_IGNORE = 0x2a,
        SYNC_PINS = 0x2b,
        SYNC_RESET_OFF = 0x2c,
        GATE_DELAY = 0x2d,
        SIM_DATA = 0x2e,
        SHADOW_DATA = 0x2f,
        SHADOW = 0x30,
        RDSIM = 0x31,
        FLOAT_TST = 0x32,
        LEVEL_CHK = 0x33,
        UNKNOWN_34 = 0x34,
        UNKNOWN_35 = 0x35
    }

    public enum CommandFileErrorType
    {
        Warning,
        Error
    }
}
