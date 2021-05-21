using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900.Containers
{


    public class ClientCommandBase
    {
        private Dictionary<ClientCommands, string> _remoteCommandDictionary = new Dictionary<ClientCommands, string>()
        {
            { ClientCommands.DataString, "--" },
            { ClientCommands.Identify,"AIdentify" },
            { ClientCommands.SetSyncTime, "CEC" },
            { ClientCommands.GetSyncTime, "CED" },
            { ClientCommands.WritePinDefinition, "CF" },
            { ClientCommands.ReadResetDefinition, "DB" },
            { ClientCommands.SetClipCheck, "DC" },
            { ClientCommands.GetClipCheck, "DD" },
            { ClientCommands.SetRDTest, "DE" },
            { ClientCommands.GetRDTest, "DF" },
            { ClientCommands.SetTriggerGateWord, "DGA" },
            { ClientCommands.GetTriggerGateWord, "DGB" },
            { ClientCommands.SetTriggerConfiguration, "DGC" },
            { ClientCommands.SetTriggerEnable, "DGD" },
            { ClientCommands.GetTriggerEnable, "DGE" },
            { ClientCommands.SetSimulation, "DL" },
            { ClientCommands.GetSimulation, "DM" },
            { ClientCommands.DisplayText, "EA" },
            { ClientCommands.ReadKeystroke, "EB" },
            { ClientCommands.ReadKeystrokes, "EC" },
            { ClientCommands.GenerateSound, "EDA" },
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
            { ClientCommands.SetDateTime, "JC" },
            { ClientCommands.GetDateTime, "JD" }
        };

        protected List<string> _parameters = new List<string>();
        private ClientCommands _commandCode = ClientCommands.ExitRemoteMode;
        private string _commandString = "";

        public string CommandString { get { return _commandString; } }
        public ClientCommands CommandCode { get { return _commandCode; } }

        public ClientCommandBase(ClientCommands commandCode)
        {
            _commandCode = commandCode;
            _commandString = _remoteCommandDictionary[commandCode];
            this.FormatResult = this.FormatResultDefault;
        }

        public ClientCommandBase(string commandText)
        {
            bool found = false;
            foreach(var c in _remoteCommandDictionary)
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


        public Func<byte[], string> FormatResult { get; set; }

        protected virtual string FormatResultDefault(byte[] resultBytes)
        {
            if (resultBytes != null)
            {
                if (resultBytes.Length > 2)
                {
                    return Encoding.ASCII.GetString(resultBytes, 1, (resultBytes.Length - 2)).Replace("\r", "\r\n");
                }
            }
            return null;
        }

        public Func<byte[], object> GetResultObject { get; set; }

    }
}
