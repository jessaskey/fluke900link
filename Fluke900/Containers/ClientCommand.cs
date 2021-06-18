using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO.Ports;
using Fluke900.Helpers;

namespace Fluke900.Containers
{
    public class ClientCommand
    {
        public static ClientCommand GetCommand(ClientCommands clientCommand, string parameter)
        {
            ClientCommand command = GetCommand(clientCommand);
            command.Parameters.Add(parameter);
            return command;
        }

        public static ClientCommand GetCommand(ClientCommands clientCommand, string[] parameters)
        {
            ClientCommand command = GetCommand(clientCommand);
            command.Parameters.AddRange(parameters);
            return command;
        }

        public static ClientCommand GetCommand(string commandText)
        {
            ClientCommand command = new ClientCommand(commandText);
            return command;
        }

        public static ClientCommand GetCommand(ClientCommands clientCommand)
        {
            ClientCommand command = new ClientCommand(clientCommand);
            switch (clientCommand)
            {
                case ClientCommands.Identify:
                    command.FormatResult = delegate (byte[] resultBytes)
                    {
                        List<string> results = new List<string>();
                        string rawResultString = Encoding.ASCII.GetString(resultBytes, 1, (resultBytes.Length - 2));
                        string[] resultParts = rawResultString.Split('\r');
                        results.Add("Unit: " + resultParts[0]);
                        results.Add("Test Results: " + resultParts[1]);
                        results.Add("Software Version: " + resultParts[2]);
                        results.Add("Library Version: " + resultParts[3]);
                        results.Add("MicroBoard Revision: " + resultParts[4]);
                        results.Add("HighSpeedBoard Revision: " + resultParts[5]);
                        results.Add("InputBuffer Revision: " + resultParts[6]);
                        return results.ToArray();
                    };
                    break;
                case ClientCommands.GetDateTime:
                    command.FormatResult = delegate (byte[] resultBytes)
                    {
                        string rawResultString = Encoding.ASCII.GetString(resultBytes, 1, (resultBytes.Length - 2));
                        string[] resultParts = rawResultString.Split('\r');
                        return new string[1] { "Date on Device: " + resultParts[1] + " " + resultParts[0] };
                    };
                    break;
                case ClientCommands.SetDateTime:
                    command.FormatResult = delegate (byte[] resultBytes)
                    {
                        if (resultBytes != null && resultBytes.Length > 0 && resultBytes[0] == (byte)CommandCharacters.Acknowledge)
                        {
                            return new string[1] { "Time/Date sucessfully set!" };
                        }
                        return new string[1] { "Error Setting Time/Date" };
                    };
                    break;
                case ClientCommands.GetDirectorySystem:
                    command.FormatResult = delegate (byte[] resultBytes)
                    {
                        List<string> results = new List<string>();
                        if (resultBytes.Length > 3)
                        {
                            string rawResultString = Encoding.ASCII.GetString(resultBytes, 2, (resultBytes.Length - 3));
                            string[] resultParts = rawResultString.Split(new string[] { "\r" }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string fileinfos in resultParts)
                            {
                                string[] fileInfo = fileinfos.Split(' ');
                                if (fileInfo.Length == 2)
                                {
                                    results.Add(fileInfo[0].PadRight(15) + fileInfo[1] + " bytes");
                                }
                                else
                                {
                                    results.Add(fileinfos);
                                }
                            }
                        }
                        return results.ToArray();
                    };
                    command.GetResultObject = delegate (byte[] resultBytes)
                    {
                        DirectoryListingInfo dl = new DirectoryListingInfo();
                        if (resultBytes.Length > 2)
                        {
                            string rawResultString = Encoding.ASCII.GetString(resultBytes, 1, (resultBytes.Length - 2));
                            rawResultString = rawResultString.TrimStart(TransmissionCharacters.STX);
                            string[] resultParts = rawResultString.Split(new string[] { "\r" }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string fileinfos in resultParts)
                            {
                                string[] fileInfo = fileinfos.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                if (fileInfo.Length == 2)
                                {
                                    dl.Files.Add(new Tuple<string, string>(FileHelper.GetFilenameOnly(fileInfo[0].Trim()), fileInfo[1].Trim()));
                                }
                                else
                                {
                                    dl.BytesUsed = long.Parse(fileInfo[0]);
                                    dl.BytesFree = long.Parse(fileInfo[3]);
                                }
                            }
                        }
                        return dl;
                    };
                    break;
                case ClientCommands.GetDirectoryCartridge:
                    command.FormatResult = delegate (byte[] resultBytes)
                    {
                        List<string> results = new List<string>();
                        string rawResultString = Encoding.ASCII.GetString(resultBytes, 2, (resultBytes.Length - 3));
                        string[] resultParts = rawResultString.Split(new string[] { "\r" }, StringSplitOptions.RemoveEmptyEntries);
                        if (resultParts.Length > 1)
                        {
                            foreach (string fileinfos in resultParts)
                            {
                                string[] fileInfo = fileinfos.Split(' ');
                                if (fileInfo.Length == 2)
                                {
                                    results.Add(fileInfo[0].PadRight(15) + fileInfo[1] + " bytes");
                                }
                                else
                                {
                                    results.Add(fileinfos);
                                }
                            }
                        }
                        return results.ToArray();
                    };
                    command.GetResultObject = delegate (byte[] resultBytes)
                    {
                        DirectoryListingInfo dl = new DirectoryListingInfo();
                        if (resultBytes.Length >= 2)
                        {
                            string rawResultString = Encoding.ASCII.GetString(resultBytes, 1, (resultBytes.Length - 2));
                            string[] resultParts = rawResultString.Split(new string[] { "\r" }, StringSplitOptions.RemoveEmptyEntries);
                            if (resultParts.Length > 0)
                            {
                                foreach (string fileinfos in resultParts)
                                {
                                    string[] fileInfo = fileinfos.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                    if (fileInfo.Length == 2)
                                    {
                                        //when there are 2 parts, then this is a valid file listing
                                        dl.Files.Add(new Tuple<string, string>(FileHelper.GetFilenameOnly(fileInfo[0]), fileInfo[1]));
                                    }
                                    else
                                    {
                                        if (fileinfos.ToLower().Contains("* corrupted *"))
                                        {
                                            dl.Files.Add(new Tuple<string, string>(FileHelper.GetFilenameOnly(fileInfo[0]), "*Corrupted*"));
                                        }
                                        else
                                        {
                                            long bytesUsed = 0;
                                            long bytesFree = 0;
                                            if (long.TryParse(fileInfo[0], out bytesUsed))
                                            {
                                                dl.BytesUsed = bytesUsed;
                                            }
                                            if (fileInfo.Length > 3)
                                            {
                                                if (long.TryParse(fileInfo[3], out bytesFree))
                                                {
                                                    dl.BytesFree = bytesFree;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        return dl;
                    };
                    break;
                case ClientCommands.FormatCartridge:
                    command.FormatResult = delegate (byte[] resultBytes)
                    {
                        if (resultBytes != null && resultBytes.Length > 0 && resultBytes[0] == (byte)CommandCharacters.Acknowledge)
                        {
                            return new string[1] { "Cartridge Formatted Sucessfully" };
                        }
                        return new string[1] { "Error Formatting Cartridge" };
                    };
                    break;
                case ClientCommands.DownloadFile:
                    command.FormatResult = delegate (byte[] resultBytes)
                    {
                        if (resultBytes != null && resultBytes.Length > 0 && resultBytes[0] == (byte)CommandCharacters.Acknowledge)
                        {
                            return new string[1] { "File Transferred sucessfully!" };
                        }
                        return new string[1] { "Error Downloading File" };
                    };
                    break;
                case ClientCommands.PerformanceEnvelope:
                    command.FormatResult = delegate (byte[] resultBytes)
                    {
                        List<string> results = new List<string>();
                        string rawResultString = Encoding.UTF8.GetString(resultBytes);
                        string[] resultParts = rawResultString.Split('\r');
                        if (resultParts.Length > 0)
                        {
                            results.Add("PEFaultMask: " + resultParts[0]);
                            results.Add("PEFaultMaskStep: " + resultParts[1]);
                            results.Add("PEFaultMaskCount: " + resultParts[2]);
                            results.Add("PEThreshold: " + resultParts[3]);
                            results.Add("PEThresholdStep: " + resultParts[4]);
                            results.Add("PEThresholdCount: " + resultParts[5]);
                        }
                        return results.ToArray();
                    };
                    break;
            }
            return command;
        }

        private ClientCommand(ClientCommands commandCode) 
        {
            _commandCode = commandCode;
            _commandString = _remoteCommandDictionary[commandCode];
        }

        private ClientCommand(ClientCommands commandCode, string parameter)
        {
            _commandCode = commandCode;
            _commandString = _remoteCommandDictionary[commandCode];
            Parameters.Add(parameter);
        }

        private ClientCommand(string commandText)
        {
            bool found = false;
            foreach (var c in _remoteCommandDictionary)
            {
                if (commandText.ToLower().StartsWith(c.Value.ToLower()))
                {
                    _commandCode = c.Key;
                    _commandString = c.Value;
                    //define command parameters
                    _parameters.AddRange(commandText.Split('\r'));
                    if (_parameters.Count > 0)
                    {
                        _parameters[0] = _parameters[0].Substring(_commandString.Length);
                    }
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                _commandString = commandText;
                _commandCode = ClientCommands.Unknown;
            }
        }

        public byte[] BytesToSend
        {
            get
            {
                Encoding encoder = Encoding.ASCII;
                StringBuilder sb = new StringBuilder();
                sb.Append(TransmissionCharacters.STX);
                sb.Append(_commandString);
                if (_parameters != null)
                {
                    sb.Append(String.Join("\r", _parameters));
                }
                sb.Append(TransmissionCharacters.ETX);

                byte[] commandBytes = encoder.GetBytes(sb.ToString());

                //int arrayLength = commandBytes.Length + 2;
                //byte[] finalBytes = new byte[arrayLength];

                //finalBytes[0] = (byte)CommandChars.StartCommand;
                //commandBytes.CopyTo(finalBytes, 1);
                //finalBytes[commandBytes.Length + 1] = (byte)CommandChars.EndCommand;
                return commandBytes;
            }
        }

        public Func<byte[], string[]> FormatResult { get; set; }
        public virtual string[] FormatResultDefault(byte[] resultBytes)
        {
            if (resultBytes != null)
            {
                if (resultBytes.Length > 2)
                {
                    //trim leading <stx> and trailing <ack>
                    return Encoding.ASCII.GetString(resultBytes, 1, (resultBytes.Length - 2)).Split('\r');
                }
            }
            return null;
        }

        public Func<byte[], object> GetResultObject { get; set; }


        public string CommandString { get { return _commandString; } }
        public ClientCommands CommandCode { get { return _commandCode; } }
        public List<string> Parameters { get { return _parameters; } set { _parameters = value; } }
        public ClientCommandResponse Response { get; set; }

        private List<string> _parameters = new List<string>();
        private ClientCommands _commandCode = ClientCommands.ExitRemoteMode;
        private string _commandString = "";

        private Dictionary<ClientCommands, string> _remoteCommandDictionary = new Dictionary<ClientCommands, string>()
        {
            { ClientCommands.DataString, "--" },
            { ClientCommands.Identify,"AIdentify" },
            { ClientCommands.SetLocation, "BA" },
            { ClientCommands.SetDevice, "BB" },
            { ClientCommands.SetSizePower, "BC" },
            { ClientCommands.GetSizePower, "BD" },
            { ClientCommands.GetDevice, "BE" },
            { ClientCommands.SetFmask, "CA" },
            { ClientCommands.GetFmask, "CB" },
            { ClientCommands.SetThreshold, "CC" },
            { ClientCommands.GetThreshold, "CD" },
            { ClientCommands.SetTestTime, "CEA" },
            { ClientCommands.GetTestTime, "CEB" },
            { ClientCommands.SetSyncTime, "CEC" },
            { ClientCommands.GetSyncTime, "CED" },
            { ClientCommands.WritePinDefinition, "CF" },
            { ClientCommands.SetPinEnableDisable, "CH" },
            { ClientCommands.GetUknown, "CI" },
            { ClientCommands.SetResetDefinition, "DA" },
            { ClientCommands.GetResetDefinition, "DB" },
            { ClientCommands.SetClipCheck, "DC" },
            { ClientCommands.GetClipCheck, "DD" },
            { ClientCommands.SetRDTest, "DE" },
            { ClientCommands.GetRDTest, "DF" },
            { ClientCommands.SetTriggerGateWord, "DGA" },
            { ClientCommands.GetTriggerGateWord, "DGB" },
            { ClientCommands.SetTriggerConfiguration, "DGC" },
            { ClientCommands.SetTriggerEnable, "DGD" },
            { ClientCommands.GetTriggerEnable, "DGE" },
            { ClientCommands.SetGateConfiguration, "DGF" },
            { ClientCommands.SetGateEnable, "DGG" },
            { ClientCommands.GetGateConfiguration, "DGH" },
            { ClientCommands.SetRAMShadow,"DJ" },
            { ClientCommands.GetRAMShadow,"DK" },
            { ClientCommands.SetSimulation, "DL" },
            { ClientCommands.GetSimulation, "DM" },
            { ClientCommands.PerformanceEnvelope, "DN" },
            { ClientCommands.Stimulus, "DQ" },
            { ClientCommands.DisplayText, "EA" },
            { ClientCommands.ReadKeystroke, "EB" },
            { ClientCommands.ReadKeystrokes, "EC" },
            { ClientCommands.GenerateSound, "EDA" },
            { ClientCommands.ResetAllParameters, "GA" },
            { ClientCommands.ExitRemoteMode, "GD" },
            { ClientCommands.SoftReset, "GC" },
            { ClientCommands.HardReset, "GB" },
            { ClientCommands.ReadPinDefinition, "GG" },
            { ClientCommands.UploadFile, "HA" },
            { ClientCommands.DownloadFile, "HB" },
            { ClientCommands.CompileFile, "HC" },
            { ClientCommands.DeleteFile, "HD" },
            { ClientCommands.GetDirectoryCartridge, "HE" },
            { ClientCommands.GetDirectorySystem, "HES" },
            { ClientCommands.FormatCartridge, "HH" },
            { ClientCommands.SetRDDrive, "IB" },
            { ClientCommands.GetRDDrive, "IC" },
            { ClientCommands.SetCsum, "ID" },
            { ClientCommands.GetCsum, "IE" },
            { ClientCommands.SetDateTime, "JC" },
            { ClientCommands.GetDateTime, "JD" }
        };
    }
}
