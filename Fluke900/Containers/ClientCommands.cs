﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900.Containers
{
    public enum ClientCommands
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
        SetResetDefinition,
        GetResetDefinition,
        SetRDDrive,
        GetRDDrive,
        SetRDTest,
        GetRDTest,
        SetClipCheck,
        GetClipCheck,
        SetSimulation,
        GetSimulation,
        SetSyncTime,
        GetSyncTime,
        SetTriggerConfiguration,
        SetTriggerEnable,
        GetTriggerEnable,
        SetTriggerGateWord,
        GetTriggerGateWord,
        SetRAMShadow,
        GetRAMShadow,
        SetFmask,
        GetFmask,
        SetTestTime,
        GetTestTime,
        SetGateEnable,
        SetGateConfiguration,
        GetGateConfiguration,
        SetThreshold,
        GetThreshold,
        ResetAllParameters,
        SetSizePower,
        GetSizePower,
        SetPinEnableDisable
    }
}