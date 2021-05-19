using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900.Containers
{
    public enum RemoteCommandCodes
    {
        Unknown,
        Identify,
        ExitRemoteMode,
        SoftReset,
        HardReset,
        GetDateTime,
        SetDateTime,
        GetDirectorySystem,
        GetDirectoryCartridge,
        FormatCartridge, 
        DownloadFile,
        UploadFile,
        DeleteFile,
        DisplayText,
        ReadKeystroke,
        ReadKeystrokes,
        GenerateSound,
        CompileFile,
        DataString,
        SendFileLine,
        WritePinDefinition,
        ReadPinDefinition,
        ReadResetDefinition,
        SetRDDrive,
        GetRDDrive
    }
}
