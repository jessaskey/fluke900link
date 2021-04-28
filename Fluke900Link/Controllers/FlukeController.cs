using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Fluke900Link.Containers;
using Fluke900Link.Helpers;
using RJCP.IO.Ports;

namespace Fluke900Link.Controllers
{
    public static class FlukeController
    {

        public static void SetConnectionProperties(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            SerialPortController.Port = portName;
            SerialPortController.BaudRate = baudRate;
            SerialPortController.Parity = parity;
            SerialPortController.DataBits = dataBits;
            SerialPortController.StopBits = stopBits;
        }

        #region Progress Callbacks

        public static void SetConnectionStatusProgress(IProgress<ConnectionStatus> connectionStatusProgress)
        {
            SerialPortController.ConnectionStatusProgress = connectionStatusProgress;
        }

        public static void SetDataStatusProgress(IProgress<CommunicationDirection> dataStatusProgress)
        {
            SerialPortController.DataStatusProgress = dataStatusProgress;
        }

        public static void SetDataSendProgress(IProgress<RemoteCommand> dataSendProgress)
        {
            SerialPortController.DataSendProgress = dataSendProgress;
        }

        public static void SetDataReceiveProgress(IProgress<RemoteCommandResponse> dataReceiveProgress)
        {
            SerialPortController.DataReceiveProgress = dataReceiveProgress;
        }

        #endregion

        public static async Task<bool> Connect()
        {
            SerialPortController.Connect();
            RemoteCommandResponse response = await SerialPortController.SendCommandAsync(RemoteCommandCodes.Identify, null);
            return response.Status == CommandResponseStatus.Success;
        }

        public async static Task Disconnect()
        {
            if (SerialPortController.IsConnected)
            {
                await SerialPortController.SendCommandAsync(RemoteCommandCodes.ExitRemoteMode);
            }
            SerialPortController.Disconnect();
        }

        public static bool IsConnected
        {
            get
            {
                return SerialPortController.IsConnected;
            }
        }

        public static async Task<DateTime?> GetDateTime()
        {
            if (SerialPortController.IsConnected)
            {
                RemoteCommandResponse response = await SerialPortController.SendCommandAsync(RemoteCommandCodes.GetDateTime, null);

                string[] resultParts = Encoding.ASCII.GetString(response.RawBytes, 1, (response.RawBytes.Length - 2)).Split('\r');
                IFormatProvider culture = new System.Globalization.CultureInfo("en-US", true);
                return DateTime.Parse(resultParts[1] + " " + resultParts[0], culture);
            }
            return null;
        }

        public static async Task<bool> SetDate(DateTime currentDateTime)
        {
            if (SerialPortController.IsConnected)
            {
                string flukeFormattedDate = currentDateTime.Day.ToString("00") + "/" + currentDateTime.Month.ToString("00") + "/" + currentDateTime.ToString("yy");
                RemoteCommandResponse response = await SerialPortController.SendCommandAsync(RemoteCommandCodes.SetDateTime, flukeFormattedDate);
                return response.Status == CommandResponseStatus.Success;
            }
            return false;
        }

        public static async Task<bool> SetTime(DateTime currentDateTime)
        {
            if (SerialPortController.IsConnected)
            {
                string flukeFormattedTime = currentDateTime.Hour.ToString("00") + ":" + currentDateTime.Minute.ToString("00");
                RemoteCommandResponse response = await SerialPortController.SendCommandAsync(RemoteCommandCodes.SetDateTime, flukeFormattedTime);
                return response.Status == CommandResponseStatus.Success;
            }
            return false;
        }

        public static async Task<bool> SoftReset()
        {
            if (SerialPortController.IsConnected)
            {
                RemoteCommandResponse response = await SerialPortController.SendCommandAsync(RemoteCommandCodes.SoftReset);
                return response.Status == CommandResponseStatus.Success;
            }
            return false;
        }

        public static async Task<bool> HardReset()
        {
            if (SerialPortController.IsConnected)
            {
                RemoteCommandResponse response = await SerialPortController.SendCommandAsync(RemoteCommandCodes.HardReset);
                return response.Status == CommandResponseStatus.Success;
            }
            return false;
        }

        public static async Task<RemoteCommandResponse> SendCommand(RemoteCommand command)
        {
            if (SerialPortController.IsConnected)
            {
                return await SerialPortController.SendCommandAsync(command);
            }
            return null;
        }

        public static async Task<DirectoryListingInfo> GetDirectoryListing(FileLocations fileLocation)
        {
            DirectoryListingInfo directoryInfo = null;
            RemoteCommandResponse response = new RemoteCommandResponse();

            switch (fileLocation)
            {
                case FileLocations.FlukeCartridge:
                    response = await SerialPortController.SendCommandAsync(RemoteCommandCodes.GetDirectoryCartridge);
                    break;
                case FileLocations.FlukeSystem:
                    response = await SerialPortController.SendCommandAsync(RemoteCommandCodes.GetDirectorySystem);
                    break;
                case FileLocations.LocalComputer:
                    throw new Exception("Cannot get PC Directory from Fluke, ask someone else.");
            }
            if (response != null)
            {
                directoryInfo = await CommandResponseToDirectoryListing(response);
            }
            return directoryInfo;
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

            if (sourceLocation == Fluke900Link.FileLocations.LocalComputer)
            {
                //PC to FLUKE
                string sourceFile = FileHelper.RemoveFileLocation(source);

                if (File.Exists(sourceFile))
                {
                    RemoteCommandResponse response = await SerialPortController.SendCommandAsync(RemoteCommandCodes.UploadFile, FileHelper.AdjustForTransfer(destination));
                    if (response.Status == CommandResponseStatus.Accepted)
                    {
                        string fileContent = File.ReadAllText(sourceFile, Encoding.ASCII).Replace("\n", "").Replace("\t", new string(' ', Properties.Settings.Default.ConvertTabsToSpaces));

                        RemoteCommand commandFile = RemoteCommandFactory.GetCommand(RemoteCommandCodes.DataString, new string[] { fileContent });
                        RemoteCommandResponse responseFile = await SerialPortController.SendCommandAsync(commandFile);
                        Console.Write(Encoding.ASCII.GetString(responseFile.RawBytes));

                        if (responseFile.Status == CommandResponseStatus.Success)
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
                RemoteCommandResponse cr = await SerialPortController.SendCommandAsync(RemoteCommandCodes.DownloadFile, sourceFileName );
                if (cr.Status == CommandResponseStatus.Success)
                {
                    StringBuilder sb = new StringBuilder();
                    //last byte is an ACK so we don't want to save that byte
                    for (int i = 1; i < cr.RawBytes.Length - 1; i++)
                    {
                        sb.Append(Encoding.ASCII.GetString(cr.RawBytes, i, 1));
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
                    RemoteCommandResponse response = await SerialPortController.SendCommandAsync(RemoteCommandCodes.DeleteFile, fileName);
                    if (response.Status == CommandResponseStatus.Success)
                    {
                        success = false;
                    }
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
            RemoteCommandResponse response = await SerialPortController.SendCommandAsync(RemoteCommandCodes.GetDirectoryCartridge);
            return response.Status == CommandResponseStatus.Success;
        }

        /// <summary>
        /// Tests if the Cartridge is inserted and also writable by writing a small test file to the Cartridge. The test file is immediately deleted after creation.
        /// </summary>
        /// <returns>True if the cartridge is able to receive files.</returns>
        public static async Task<bool?> IsCartridgeWritable()
        {
            if (!IsConnected) return false;

            bool? isWritable = null;
            string testFile = Globals.CARTRIDGE_TEST_FILENAME;

            RemoteCommandResponse response = await SerialPortController.SendCommandAsync(RemoteCommandCodes.UploadFile, testFile);

            if (response.Status == CommandResponseStatus.Accepted)
            {
                RemoteCommandResponse responseFile = await SerialPortController.SendCommandAsync(RemoteCommandCodes.DataString, ";WRITETEST;" );
                if (responseFile.Status == CommandResponseStatus.Success)
                {
                    isWritable = true;
                    await DeleteFile(testFile);
                }
            }
            return isWritable;
        }

        public static async Task<CompilationResult> CompileFile(string file)
        {
            RemoteCommand command = RemoteCommandFactory.GetCommand(RemoteCommandCodes.CompileFile, file);
            RemoteCommandResponse response1 = await SerialPortController.SendCommandAsync(command);
            if (response1.Status == CommandResponseStatus.Accepted)
            {
                RemoteCommandResponse responseResult = await SerialPortController.ReceiveResponseAsync(command);
                return responseResult.AsCompilationResult();
            }
            return null;
        }

        public static async Task<RemoteCommandResponse> FormatCartridge()
        {
            return await SerialPortController.SendCommandAsync(RemoteCommandCodes.FormatCartridge);
        }



















        /// <summary>
        /// Will do some formatting of the Directory info object with knowing the response info... this 
        /// *really* shoudln't be here and this all needs cleaned up
        /// </summary>
        /// <param name="cr">The command response object that produced this DirectoryListing</param>
        /// <returns>A pretty DirectoryListingInfo object</returns>
        private static async Task<DirectoryListingInfo> CommandResponseToDirectoryListing(RemoteCommandResponse cr)
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
