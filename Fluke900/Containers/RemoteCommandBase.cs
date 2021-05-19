using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900.Containers
{


    public class RemoteCommandBase
    {
        private Dictionary<RemoteCommandCodes, string> _remoteCommandDictionary = new Dictionary<RemoteCommandCodes, string>()
        {
            { RemoteCommandCodes.DataString, "--" },
            { RemoteCommandCodes.Identify,"AIdentify" },
            { RemoteCommandCodes.WritePinDefinition, "CF" },
            { RemoteCommandCodes.ReadResetDefinition, "DB" },
            { RemoteCommandCodes.DisplayText, "EA" },
            { RemoteCommandCodes.ReadKeystroke, "EB" },
            { RemoteCommandCodes.ReadKeystrokes, "EC" },
            { RemoteCommandCodes.GenerateSound, "EDA" },
            { RemoteCommandCodes.ExitRemoteMode, "GD" },
            { RemoteCommandCodes.SoftReset, "GC" },
            { RemoteCommandCodes.HardReset, "GB" },
            { RemoteCommandCodes.ReadPinDefinition, "GG" },
            { RemoteCommandCodes.UploadFile, "HA" },
            { RemoteCommandCodes.DownloadFile, "HB" },
            { RemoteCommandCodes.CompileFile, "HC" },
            { RemoteCommandCodes.DeleteFile, "HD" },
            { RemoteCommandCodes.GetDirectoryCartridge, "HE" },
            { RemoteCommandCodes.GetDirectorySystem, "HES" },
            { RemoteCommandCodes.FormatCartridge, "HH" },
            { RemoteCommandCodes.SetRDDrive, "IB" },
            { RemoteCommandCodes.GetRDDrive, "IC" },
            { RemoteCommandCodes.SetDateTime, "JC" },
            { RemoteCommandCodes.GetDateTime, "JD" }
        };

        protected List<string> _parameters = new List<string>();
        private RemoteCommandCodes _commandCode = RemoteCommandCodes.ExitRemoteMode;
        private string _commandString = "";

        public string CommandString { get { return _commandString; } }
        public RemoteCommandCodes CommandCode { get { return _commandCode; } }

        public RemoteCommandBase(RemoteCommandCodes commandCode)
        {
            _commandCode = commandCode;
            _commandString = _remoteCommandDictionary[commandCode];
            this.FormatResult = this.FormatResultDefault;
        }

        public RemoteCommandBase(string commandText)
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
                _commandCode = RemoteCommandCodes.Unknown;
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
