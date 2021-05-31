using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Fluke900.Containers;
using Fluke900.Helpers;

namespace Fluke900.Containers
{
    public static class ClientCommandFactory
    {

        private static Dictionary<ClientCommands, ClientCommand> _commands = new Dictionary<ClientCommands, ClientCommand>();

        public static void Initialize()
        {
            //load em up...

            //=========================================================================
            // Identify - Puts the Fluke into remote mode and gaters summary information
            //            from the device. The Fluke must be on the home screen for 
            //            this command to succeed.
            //=========================================================================
            ClientCommand commandIdentify = new ClientCommand(ClientCommands.Identify)
            {
                FormatResult = delegate(byte[] resultBytes)
                {
                    StringBuilder sb = new StringBuilder();
                    string rawResultString = Encoding.ASCII.GetString(resultBytes, 1, (resultBytes.Length - 2));
                    string[] resultParts = rawResultString.Split('\r');
                    sb.AppendLine("Unit: " + resultParts[0]);
                    sb.AppendLine("Test Results: " + resultParts[1]);
                    sb.AppendLine("Software Version: " + resultParts[2]);
                    sb.AppendLine("Library Version: " + resultParts[3]);
                    sb.AppendLine("MicroBoard Revision: " + resultParts[4]);
                    sb.AppendLine("HighSpeedBoard Revision: " + resultParts[5]);
                    sb.AppendLine("InputBuffer Revision: " + resultParts[6]);
                    return sb.ToString();
                }
            };
            _commands.Add(commandIdentify.CommandCode, commandIdentify);

            //=========================================================================
            // Exit Remote Mode - Exits remote control mode and goes back to the Fluke
            //                    home screen.
            //=========================================================================
            ClientCommand commandExitRemoteMode = new ClientCommand(ClientCommands.ExitRemoteMode);
            _commands.Add(commandExitRemoteMode.CommandCode, commandExitRemoteMode);

            //=========================================================================
            // Soft Reset - Just a quick restart with testing
            //=========================================================================
            ClientCommand commandResetUnitSoft = new ClientCommand(ClientCommands.SoftReset);
            _commands.Add(commandResetUnitSoft.CommandCode, commandResetUnitSoft);

            //=========================================================================
            // Full Reset - Basically reboots the Fluke
            //=========================================================================
            ClientCommand commandResetUnitFull = new ClientCommand(ClientCommands.HardReset);
            _commands.Add(commandResetUnitFull.CommandCode, commandResetUnitFull);

            //=========================================================================
            // Get Date Time - Gets both the DATE and TIME from the Fluke RTC
            //=========================================================================
            ClientCommand commandGetDateTime = new ClientCommand(ClientCommands.GetDateTime)
            {
                FormatResult = delegate(byte[] resultBytes)
                {
                    string rawResultString = Encoding.ASCII.GetString(resultBytes, 1, (resultBytes.Length - 2));
                    string[] resultParts = rawResultString.Split('\r');
                    return "Date on Device: " + resultParts[1] + " " + resultParts[0];
                }
            };
            _commands.Add(commandGetDateTime.CommandCode, commandGetDateTime);

            //=========================================================================
            // Set Date Time - Sets either the DATE or TIME or BOTH on the Fluke
            //=========================================================================
            ClientCommand commandSetDateTime = new ClientCommand(ClientCommands.SetDateTime)
            {
                FormatResult = delegate(byte[] resultBytes)
                {
                    if (resultBytes != null && resultBytes.Length > 0 && resultBytes[0] == (byte)CommandCharacters.Acknowledge)
                    {
                        return "Time/Date sucessfully set!";
                    }
                    return "Error Setting Time/Date";
                }
            };
            _commands.Add(commandSetDateTime.CommandCode, commandSetDateTime);

            //=========================================================================
            // Get Directory System - Gets the directory listing of the files residing
            //                        in the Fluke System Memory
            //=========================================================================
            ClientCommand cGetDirectorySystem = new ClientCommand(ClientCommands.GetDirectorySystem)
            {
                FormatResult = delegate(byte[] resultBytes)
                {
                    StringBuilder sb = new StringBuilder();
                    if (resultBytes.Length > 3)
                    {
                        string rawResultString = Encoding.ASCII.GetString(resultBytes, 2, (resultBytes.Length - 3));
                        string[] resultParts = rawResultString.Split(new string[] { "\r" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string fileinfos in resultParts)
                        {
                            string[] fileInfo = fileinfos.Split(' ');
                            if (fileInfo.Length == 2)
                            {
                                sb.AppendLine(fileInfo[0].PadRight(15) + fileInfo[1] + " bytes");
                            }
                            else
                            {
                                sb.AppendLine(fileinfos);
                            }
                        }
                    }
                    return sb.ToString();
                }
                ,
                GetResultObject = delegate(byte[] resultBytes)
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
                }
            };
            _commands.Add(cGetDirectorySystem.CommandCode, cGetDirectorySystem);

            //=========================================================================
            // Get Directory Cartridge - Gets the directory listing of the files residing
            //                           in the Fluke Memory Cartridge
            //=========================================================================
            ClientCommand cGetDirectoryCartridge = new ClientCommand(ClientCommands.GetDirectoryCartridge)
            {
                FormatResult = delegate(byte[] resultBytes)
                {
                    StringBuilder sb = new StringBuilder();
                    string rawResultString = Encoding.ASCII.GetString(resultBytes, 2, (resultBytes.Length - 3));
                    string[] resultParts = rawResultString.Split(new string[] { "\r" }, StringSplitOptions.RemoveEmptyEntries);
                    if (resultParts.Length > 1)
                    {
                        foreach (string fileinfos in resultParts)
                        {
                            string[] fileInfo = fileinfos.Split(' ');
                            if (fileInfo.Length == 2)
                            {
                                sb.AppendLine(fileInfo[0].PadRight(15) + fileInfo[1] + " bytes");
                            }
                            else
                            {
                                sb.AppendLine(fileinfos);
                            }
                        }
                    }
                    return sb.ToString();
                }
                ,
                GetResultObject = delegate(byte[] resultBytes)
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
                }
            };
            _commands.Add(cGetDirectoryCartridge.CommandCode, cGetDirectoryCartridge);

            //=========================================================================
            // Format Cartridge - Formats Cartridge and erases all files
            //=========================================================================
            ClientCommand cFormatCartridge = new ClientCommand(ClientCommands.FormatCartridge)
            {
                FormatResult = delegate(byte[] resultBytes)
                {
                    if (resultBytes != null && resultBytes.Length > 0 && resultBytes[0] == (byte)CommandCharacters.Acknowledge)
                    {
                        return "Cartridge Formatted Sucessfully";
                    }
                    return "Error Formatting Cartridge";
                }
            };
            _commands.Add(cFormatCartridge.CommandCode, cFormatCartridge);

            ////=========================================================================
            // Download File from Unit - Transfers a file from the Fluke to the PC
            //=========================================================================
            ClientCommand cDownloadFile = new ClientCommand(ClientCommands.DownloadFile)
            {
                FormatResult = delegate(byte[] resultBytes)
                {
                    if (resultBytes != null && resultBytes.Length > 0 && resultBytes[0] == (byte)CommandCharacters.Acknowledge)
                    {
                        return "File Transferred sucessfully!";
                    }
                    return "Error Downloading File";
                }
            };
            _commands.Add(cDownloadFile.CommandCode, cDownloadFile);

            ////=========================================================================
            // Upload File to Unit - Transfers a file from the PC to the Fluke
            //=========================================================================
            ClientCommand cUploadFile = new ClientCommand(ClientCommands.UploadFile);
            _commands.Add(cUploadFile.CommandCode, cUploadFile);

            ////=========================================================================
            // Delete File - Deletes a file on the Fluke
            //=========================================================================
            ClientCommand cDeleteFile = new ClientCommand(ClientCommands.DeleteFile);
            _commands.Add(cDeleteFile.CommandCode, cDeleteFile);

            ////=========================================================================
            // Display Text - Shows text on the Fluke Screen
            //=========================================================================
            ClientCommand cDisplayText = new ClientCommand(ClientCommands.DisplayText);
            _commands.Add(cDisplayText.CommandCode, cDisplayText);

            ////=========================================================================
            // Read Keystroke - Reads a key press from the Fluke
            //=========================================================================
            ClientCommand cReadKeystroke = new ClientCommand(ClientCommands.ReadKeystroke);
            _commands.Add(cReadKeystroke.CommandCode, cReadKeystroke);

            ////=========================================================================
            // Read Keystroke - Reads a key press from the Fluke
            //=========================================================================
            ClientCommand cReadKeystrokes = new ClientCommand(ClientCommands.ReadKeystrokes);
            _commands.Add(cReadKeystrokes.CommandCode, cReadKeystrokes);

            ////=========================================================================
            // Generate Sound - Creates a sound on the Fluke
            //=========================================================================
            ClientCommand cGenerateSound = new ClientCommand(ClientCommands.GenerateSound);
            _commands.Add(cGenerateSound.CommandCode, cGenerateSound);

            ////=========================================================================
            // Compile File - Compile a file on the Fluke
            //=========================================================================
            ClientCommand cCompileFile = new ClientCommand(ClientCommands.CompileFile);
            _commands.Add(cCompileFile.CommandCode, cCompileFile);

            ////=========================================================================
            // Send String - For sending files as strings
            //=========================================================================
            ClientCommand cSendString = new ClientCommand(ClientCommands.DataString);
            _commands.Add(cSendString.CommandCode, cSendString);


        }

        public static ClientCommand GetCommand(ClientCommands commandCode, string parameter)
        {
            ClientCommand command = _commands[commandCode];
            if (parameter != null)
            {
                command.Parameters = new List<string>() { parameter};
            }
            return command;
        }

        public static ClientCommand GetCommand(ClientCommands commandCode, string[] parameters)
        {
            ClientCommand command = _commands[commandCode];
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }
            return command;
        }

        public static ClientCommand GetCommandByStringCommand(string commandString, string[] parameters)
        {
            List<ClientCommand> allCommands = _commands.Select(c => c.Value).ToList();
            ClientCommand command = allCommands.Where(c => c.CommandString.ToUpper() == commandString.ToUpper()).FirstOrDefault();
            if (command != null)
            { 
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }      
            }
            return command;
        }

        #region Public Commands




        #endregion
    }
}
