using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Fluke900;
using Fluke900.Containers;
using Fluke900.Controllers;
using Fluke900.Helpers;
using RJCP.IO.Ports;

namespace Fluke900Link.Controllers
{
    public static class FlukeController
    {


        public static void SetConnectionProperties(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            SerialController.Port = portName;
            SerialController.BaudRate = baudRate;
            SerialController.Parity = parity;
            SerialController.DataBits = dataBits;
            SerialController.StopBits = stopBits;
        }


        public static async Task<bool> Connect()
        {
            if (SerialController.OpenPort())
            {
                ClientCommand command = new ClientCommand(ClientCommands.Identify);
                await SerialController.SendCommand(command);
                return command.Response.Status == CommandResponseStatus.Success;
            }
            return false;
        }

        public async static Task Disconnect()
        {
            if (SerialController.IsConnected)
            {
                ClientCommand command = ClientCommandFactory.GetCommand(ClientCommands.ExitRemoteMode);
                await SerialController.SendCommand(command);
            }
            SerialController.ClosePort();
        }

        public static void Initialize(IProgress<ConnectionStatus> connectionStatusProgress,
                                      IProgress<CommunicationDirection> dataStatusProgress,
                                      IProgress<byte[]> dataSendProgress,
                                      IProgress<byte[]> dataReceiveProgress,
                                      IProgress<ClientCommand> commandSendProgress,
                                      IProgress<ClientCommandResponse> commandResponseProgress)
        {
            SerialController.ConnectionStatusProgress = connectionStatusProgress;
            SerialController.DataStatusProgress = dataStatusProgress;
            SerialController.DataSendProgress = dataSendProgress;
            SerialController.DataReceiveProgress = dataReceiveProgress;
            SerialController.CommandSendProgress = commandSendProgress;
            SerialController.CommandResponseProgress = commandResponseProgress;
        }

        public static bool IsConnected
        {
            get
            {
                return SerialController.IsConnected;
            }
        }

        public static async Task<DateTime?> GetDateTime()
        {
            if (SerialController.IsConnected)
            {
                ClientCommand command = new ClientCommand(ClientCommands.GetDateTime);
                await SerialController.SendCommand(command);

                string[] resultParts = Encoding.ASCII.GetString(command.Response.RawBytes, 1, (command.Response.RawBytes.Length - 2)).Split('\r');
                IFormatProvider culture = new System.Globalization.CultureInfo("en-US", true);
                return DateTime.Parse(resultParts[1] + " " + resultParts[0], culture);
            }
            return null;
        }

        public static async Task<bool> SetDate(DateTime currentDateTime)
        {
            if (SerialController.IsConnected)
            {
                string flukeFormattedDate = currentDateTime.Day.ToString("00") + "/" + currentDateTime.Month.ToString("00") + "/" + currentDateTime.ToString("yy");
                ClientCommand command = new ClientCommand(ClientCommands.SetDateTime, flukeFormattedDate);
                await SerialController.SendCommand(command);
                return command.Response.Status == CommandResponseStatus.Success;
            }
            return false;
        }

        public static async Task<bool> SetTime(DateTime currentDateTime)
        {
            if (SerialController.IsConnected)
            {
                string flukeFormattedTime = currentDateTime.Hour.ToString("00") + ":" + currentDateTime.Minute.ToString("00");
                ClientCommand command = new ClientCommand(ClientCommands.SetDateTime, flukeFormattedTime);
                await SerialController.SendCommand(command);
                return command.Response.Status == CommandResponseStatus.Success;
            }
            return false;
        }

        public static async Task<bool> SoftReset()
        {
            if (SerialController.IsConnected)
            {
                ClientCommand command = ClientCommandFactory.GetCommand(ClientCommands.SoftReset);
                await SerialController.SendCommand(command);
                return command.Response.Status == CommandResponseStatus.Success;
            }
            return false;
        }

        public static async Task<bool> HardReset()
        {
            if (SerialController.IsConnected)
            {
                ClientCommand command = ClientCommandFactory.GetCommand(ClientCommands.HardReset);
                await SerialController.SendCommand(command);
                return command.Response.Status == CommandResponseStatus.Success;
            }
            return false;
        }

        public static async Task<bool> SendCommand(ClientCommand command)
        {
            if (SerialController.IsConnected)
            {
                return await SerialController.SendCommand(command);
            }
            return false;
        }

        public static async Task<DirectoryListingInfo> GetDirectoryListing(FileLocations fileLocation)
        {
            DirectoryListingInfo directoryInfo = null;
            ClientCommand command = null;

            switch (fileLocation)
            {
                case FileLocations.FlukeCartridge:
                    command = ClientCommandFactory.GetCommand(ClientCommands.GetDirectoryCartridge);
                    await SerialController.SendCommand(command);
                    break;
                case FileLocations.FlukeSystem:
                    command = ClientCommandFactory.GetCommand(ClientCommands.GetDirectorySystem);
                    await SerialController.SendCommand(command);
                    break;
                case FileLocations.LocalComputer:
                    throw new Exception("Cannot get PC Directory from Fluke, ask someone else.");
            }
            if (command != null && command.Response != null)
            {
                directoryInfo = await CommandResponseToDirectoryListing(command.Response);
            }
            return directoryInfo;
        }

        public static async Task<PerformanceEnvelopeSettings> GetPerformanceEnvelopeSettings()
        {
            PerformanceEnvelopeSettings settings = null;
            if (SerialController.IsConnected)
            {
                ClientCommand command = ClientCommandFactory.GetCommand(ClientCommands.PerformanceEnvelope);
                await SerialController.SendCommand(command);
                if (command.Response.Status != CommandResponseStatus.Executing)
                {
                    string resultString = Encoding.ASCII.GetString(command.Response.RawBytes.Take(command.Response.RawBytes.Length-1).ToArray());
                    string[] results = resultString.Split(' ');
                    if (results.Length >= 6)
                    {
                        settings = new PerformanceEnvelopeSettings();
                        settings.FaultMask = int.Parse(results[0]);
                        settings.FaultMaskStep = int.Parse(results[1]);
                        settings.FaultMaskTestCount = int.Parse(results[2]);
                        settings.Threshold = int.Parse(results[3]);
                        settings.ThresholdStep = int.Parse(results[4]);
                        settings.ThresholdTestCount = int.Parse(results[5]);
                    }
                }
            }
            return settings;
        }

        /// <summary>
        /// Transfers a file from one location to another. Each file specifier is made up of a Path (PC Only), the Filename and a Location Code suffix.
        /// </summary>
        /// <param name="source">The file that will be copied.</param>
        /// <param name="destination">The destination for the file copy. On the Fluke, files will 
        /// automatically overwrite any existing file with the same name. On the PC, the user is 
        /// prompted for overwriting of files and they may cancel the copy action.</param>
        /// <returns></returns>
        public static async Task<int> TransferFile(string source, string destination)
        {
            return await TransferFile(source, destination, true);
        }

        public static async Task<int> TransferFile(string source, string destination, bool overwriteExisting)
        {
            int filesCopied = 0;

            FileLocations? sourceLocation = await FileHelper.GetFileLocation(source);
            FileLocations? destLocation = await FileHelper.GetFileLocation(destination);

            if (!sourceLocation.HasValue)
            {
                throw new Exception("Source Location was not specified.");
            }

            if (!destLocation.HasValue)
            {
                throw new Exception("Destination Location was not specified.");
            }

            if (sourceLocation == FileLocations.LocalComputer)
            {
                //PC to FLUKE
                string sourceFile = FileHelper.RemoveFileLocation(source);

                if (File.Exists(sourceFile))
                {
                    ClientCommand command = new ClientCommand(ClientCommands.UploadFile, FileHelper.AdjustForTransfer(destination));
                    await SerialController.SendCommand(command);
                    if (command.Response.Status == CommandResponseStatus.Executing)
                    {
                        string fileContent = File.ReadAllText(sourceFile, Encoding.ASCII).Replace("\n", "").Replace("\t", new string(' ', Properties.Settings.Default.ConvertTabsToSpaces));

                        ClientCommand commandFile = ClientCommandFactory.GetCommand(ClientCommands.DataString, new string[] { fileContent });
                        await SerialController.SendCommand(commandFile);
                        Console.Write(Encoding.ASCII.GetString(commandFile.Response.RawBytes));

                        if (commandFile.Response.Status == CommandResponseStatus.Success)
                        {
                            filesCopied++;
                        }
                        else
                        {
                            //ProgressManager.Stop();
                            //MessageBox.Show("Error copying file '" + source + "': " + response.ErrorMessage, "Error Copying File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return filesCopied;
                        }
                    }
                    else
                    {
                        //ProgressManager.Stop();
                        //MessageBox.Show("Error copying file '" + destination + "': " + response.ErrorMessage, "Error Copying File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return filesCopied;
                    }
                }
            }
            else
            {
                //FLUKE to PC
                string sourceFileName = source;

                //string fileExtension = Path.GetExtension(sourceFileName);
                string localPathFilename = FileHelper.AdjustForTransfer(FileHelper.RemoveFileLocation(destination));

                if (!overwriteExisting && File.Exists(localPathFilename) )
                {
                    throw new Exception("Destination Location already exists and FileOverwrite not set.");
                }
                //conflicts resolved, do the copy now..
                ClientCommand command = new ClientCommand(ClientCommands.DownloadFile, sourceFileName);
                await SerialController.SendCommand(command);
                if (command.Response.Status == CommandResponseStatus.Success)
                {
                    StringBuilder sb = new StringBuilder();
                    //last byte is an ACK so we don't want to save that byte
                    for (int i = 1; i < command.Response.RawBytes.Length - 1; i++)
                    {
                        sb.Append(Encoding.ASCII.GetString(command.Response.RawBytes, i, 1));
                    }
                    File.WriteAllText(localPathFilename, sb.ToString());
                    filesCopied++;
                }
            }
            return filesCopied;
        }

        public static async Task<bool> DeleteFile(string fileName)
        {
            bool success = false;

            FileLocations? location = await FileHelper.GetFileLocation(fileName);

            switch (location)
            {
                case FileLocations.FlukeCartridge:
                case FileLocations.FlukeSystem:
                    ClientCommand command = new ClientCommand(ClientCommands.DeleteFile, fileName);
                    await SerialController.SendCommand(command);
                    success = command.Response.Status == CommandResponseStatus.Success;
                    break;
                default:
                    throw new Exception("Can only delete files located on Fluke.");
            }
            return success;
        }

        public static async Task<bool> FileExists(string pathFileName)
        {
            bool result = false;

            FileLocations? fileLocation = await FileHelper.GetFileLocation(pathFileName);
            DirectoryListingInfo directoryInfo = null;

            if (fileLocation.HasValue)
            {
                switch (fileLocation.Value)
                {
                    case FileLocations.FlukeCartridge:
                    case FileLocations.FlukeSystem:
                        directoryInfo = await GetDirectoryListing(fileLocation.Value);
                        break;
                    case FileLocations.LocalComputer:
                        throw new Exception("Request for PC location from Fluke cannot be executed.");
                }
            }
            else
            {
                throw new Exception("File location not specified in file: '" + pathFileName + "'");
            }

            if (directoryInfo != null)
            {
                result = directoryInfo.FileExists(FileHelper.GetFilenameOnly(pathFileName));
            }

            return result;
        }

        /// <summary>
        /// Test if the Cartridge is currently inserted into the Fluke. This method will still return true if the Cartridge is write-protected.
        /// </summary>
        /// <returns>True if the cartridge is inserted.</returns>
        public static async Task<bool> IsCartridgeAvailable()
        {
            ClientCommand command = ClientCommandFactory.GetCommand(ClientCommands.GetDirectoryCartridge);
            await SerialController.SendCommand(command);
            return command.Response.Status == CommandResponseStatus.Success;
        }

        /// <summary>
        /// Tests if the Cartridge is inserted and also writable by writing a small test file to the Cartridge. The test file is immediately deleted after creation.
        /// </summary>
        /// <returns>True if the cartridge is able to receive files.</returns>
        public static async Task<bool?> IsCartridgeWritable()
        {
            if (!IsConnected) return false;

            bool? isWritable = null;
            string testFile = ApplicationGlobals.CARTRIDGE_TEST_FILENAME;

            ClientCommand command = new ClientCommand(ClientCommands.UploadFile, testFile);
            await SerialController.SendCommand(command);

            if (command.Response.Status == CommandResponseStatus.Success)
            {
                ClientCommand fileCommand = new ClientCommand(ClientCommands.DataString, ";WRITETEST;");
                await SerialController.SendCommand(fileCommand);
                if (fileCommand.Response.Status == CommandResponseStatus.Success)
                {
                    isWritable = true;
                    await DeleteFile(testFile);
                }
            }
            return isWritable;
        }

        public static async Task<CompilationResult> CompileFile(string file)
        {
            ClientCommand command = ClientCommandFactory.GetCommand(ClientCommands.CompileFile, file);
            await SerialController.SendCommand(command);
            if (command.Response.Status == CommandResponseStatus.Success)
            {
                //ClientCommandResponse responseResult = await SerialController.ReceiveResponseAsync(command);
                return command.Response.AsCompilationResult();
            }
            return null;
        }

        public static async Task<ClientCommandResponse> FormatCartridge()
        {
            ClientCommand command = ClientCommandFactory.GetCommand(ClientCommands.FormatCartridge);
            await SerialController.SendCommand(command);
            return command.Response;
        }



















        /// <summary>
        /// Will do some formatting of the Directory info object with knowing the response info... this 
        /// *really* shoudln't be here and this all needs cleaned up
        /// </summary>
        /// <param name="cr">The command response object that produced this DirectoryListing</param>
        /// <returns>A pretty DirectoryListingInfo object</returns>
        private static async Task<DirectoryListingInfo> CommandResponseToDirectoryListing(ClientCommandResponse cr)
        {
            DirectoryListingInfo dl = cr.ResultObject as DirectoryListingInfo;
            switch (cr.Status)
            {
                case CommandResponseStatus.Error:
                    dl.ErrorMessage = cr.ErrorMessage;
                    dl.FontBold = true;
                    dl.TextColor = Color.Red;
                    break;
                case CommandResponseStatus.Success:
                    break;
                case CommandResponseStatus.Aborted:
                    dl.ErrorMessage = "COMMAND ABORTED";
                    dl.FontBold = true;
                    dl.TextColor = Color.Red;
                    break;
            }
            return dl;
        }
    }
}
