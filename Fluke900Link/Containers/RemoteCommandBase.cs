using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900Link
{


    public class RemoteCommandBase
    {
        private Dictionary<RemoteCommandCodes, string> _remoteCommandDictionary = new Dictionary<RemoteCommandCodes, string>()
        {
            { RemoteCommandCodes.Identify,"AIdentify" },
            { RemoteCommandCodes.ExitRemoteMode, "GD" },
            { RemoteCommandCodes.SoftReset, "GC" },
            { RemoteCommandCodes.HardReset, "GB" },
            { RemoteCommandCodes.GetDateTime, "JD" },
            { RemoteCommandCodes.SetDateTime, "JC" },
            { RemoteCommandCodes.GetDirectorySystem, "HES" },
            { RemoteCommandCodes.GetDirectoryCartridge, "HE" },
            { RemoteCommandCodes.FormatCartridge, "HH" },
            { RemoteCommandCodes.DownloadFile, "HB" },
            { RemoteCommandCodes.UploadFile, "HA" },
            { RemoteCommandCodes.DeleteFile, "HD" },
            { RemoteCommandCodes.DisplayText, "EA" },
            { RemoteCommandCodes.ReadKeystroke, "EB" },
            { RemoteCommandCodes.ReadKeystrokes, "EC" },
            { RemoteCommandCodes.GenerateSound, "EDA" },
            { RemoteCommandCodes.CompileFile, "HC" },
            { RemoteCommandCodes.DataString, "--" },
            { RemoteCommandCodes.WritePinDefinition, "CF" },
            { RemoteCommandCodes.ReadPinDefinition, "GG" }
        };

        protected string[] _parameters = null;
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
                    found = true;
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
