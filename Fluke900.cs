using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Fluke900Link.Containers;
using Fluke900Link.Dialogs;
using Fluke900Link.Helpers;

namespace Fluke900Link
{

    //public delegate void DataActivityEventHandler(EventArgs e, bool status);
    public delegate void SerialDataStatusHandler(bool sending, bool receiving);
    public delegate void DataConnectionEventHander(EventArgs e, ConnectionStatus previousStatus, ConnectionStatus currentStatus);


    public static class Fluke900
    {

        public static string Port = "COM1";
        public static BaudRates BaudRate = BaudRates.Rate9600;
        public static Parity Parity = Parity.Even;
        public static DataBits DataBits = DataBits.Bits7;
        public static StopBits StopBits = StopBits.One;

        public static IProgress<byte[]> SendTerminalRaw = new Progress<byte[]>(data => LogSendTerminalRaw(data));
        public static IProgress<RemoteCommand> SendTerminalFormatted = new Progress<RemoteCommand>(data => LogSendTerminalFormatted(data));
        public static IProgress<byte[]> ReceiveTerminalRaw = new Progress<byte[]>(data => LogReceiveTerminalRaw(data));
        public static IProgress<string> ReceiveTerminalFormatted = new Progress<string>(data => LogReceiveTerminalFormatted(data));
        //public static IProgress<CommunicationStatus> UpdateCommunicationIndicators = new Progress<CommunicationStatus> (data => LogCommunicationStatus(data));
        public static SerialDataStatusHandler OnDataStatusChanged = null;


        private const int READ_BUFFER_SIZE = 65536;

        private static Stack<RemoteCommandSet> _commandStack = new Stack<RemoteCommandSet>();
        private static string _lastError = null;
        private static SerialPort _serialPort = null;

        /// <summary>
        /// Open the Serial Port Communication with the default connection timeout
        /// </summary>
        /// <returns></returns>
        public static bool Connect()
        {
            return Connect(10000);
        }

        /// <summary>
        /// Open the Serial Port Communication with the specified connection timeout
        /// </summary>
        /// <param name="timeoutOverride"></param>
        /// <returns></returns>
        public static bool Connect(int timeoutOverride)
        {
            bool success = false;
            try
            {
                _serialPort = new SerialPort(Port, (int)BaudRate, Parity, (int)DataBits, StopBits);
                _serialPort.ReadTimeout = timeoutOverride;
                _serialPort.ReadBufferSize = READ_BUFFER_SIZE;
                _serialPort.WriteBufferSize = READ_BUFFER_SIZE;
                _serialPort.DtrEnable = true;
                _serialPort.RtsEnable = true;
                _serialPort.Handshake = Handshake.RequestToSendXOnXOff;
                _serialPort.Open();
                success = true;
            }
            catch (Exception ex)
            {
                Globals.Exceptions.Add(new AppException(ex));
                _lastError = ex.Message;
            }
            return success;
        }


        /// <summary>
        /// Close the Serial Port connection and Disconnect
        /// </summary>
        public static void Disconnect()
        {
            if (_serialPort != null)
            {
                _serialPort.Close();
                _serialPort.Dispose();
                _serialPort = null;
            }
        }

        /// <summary>
        /// Checks the current Connection status and shows a default message to the user if not connected.
        /// </summary>
        /// <returns>The value of the IsConnected flag.</returns>
        public static bool VerifyConnected()
        {
            if (!IsConnected())
            {
                MessageBox.Show("You must be connected to perform this action.", "Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Send a Command to the Fluke via the passed command code and parameters
        /// </summary>
        /// <param name="commandCode">The CommandCode enumeration</param>
        /// <param name="parameters">A string array of parameters to be passed with the Array</param>
        /// <returns></returns>
        public static RemoteCommandResponse SendCommand(RemoteCommandCodes commandCode, string[] parameters)
        {
            RemoteCommand command = RemoteCommandFactory.GetCommand(commandCode, parameters);
            RemoteCommandResponse response = new RemoteCommandResponse(command);

            if (SendCommandOnly(command))
            {
                GetResponse(response);
            }
            return response;
        }

        /// <summary>
        /// Send a command to the Fluke with a Prebuilt command
        /// </summary>
        /// <param name="command">The Command object to send</param>
        /// <returns></returns>
        public static bool SendCommandOnly(RemoteCommand command)
        {
            bool success = false;
            if(_serialPort.IsOpen)
            {
                SetSerialStatus(CommunicationDirection.Send);
                //Thread.Sleep(50);
                try
                {
                    RemoteCommandSet set = new RemoteCommandSet(command);
                    _commandStack.Push(set);
                    
                    //all general commands come here
                    System.Diagnostics.Debug.WriteLine("Writing Bytes: " + Encoding.ASCII.GetString(command.BytesToSend));
                    //int pointer = 0;

                    byte[] bytesToSend = command.BytesToSend;
                    _serialPort.Write(bytesToSend, 0, bytesToSend.Length);

                    //while (pointer < bytesToSend.Length)
                    //{
                        //int s = Math.Min(bytesToSend.Length - pointer, 16);
                        //for some reason, we can overflow the input buffer
                        //of the fluke if we don't wait a little bit every
                        //256 bytes.
                        //if (pointer > 0 && (pointer) % 0x100 == 0)
                        //{
                        //    Thread.Sleep(1000);
                        //}
                        //while (_serialPort.CtsHolding)
                        //{
                        //    Application.DoEvents();
                        //}
                        //_serialPort.Write(bytesToSend, pointer, s);
                        //pointer += s;
                    //}

                    //_serialPort.Write(new byte[] { 0x04 }, 0, 1);

                    System.Diagnostics.Debug.WriteLine("Complete!");
                    //log send
                    SendTerminalFormatted.Report(command);
                    SendTerminalRaw.Report(command.BytesToSend);
                    success = true;
                }
                catch (Exception ex)
                {
                    Globals.Exceptions.Add(new AppException(ex));
                }
                SetSerialStatus(CommunicationDirection.Idle);
            }          
            return success;
        }

        /// <summary>
        /// Wait for a valid response from the Fluke via the serial port. This method is blocking.
        /// </summary>
        /// <param name="response">A prebuilt CommandResponse to fill.</param>
        public static void GetResponse(RemoteCommandResponse response)
        {
            if (_serialPort.IsOpen)
            {
                SetSerialStatus(CommunicationDirection.Receive);
                bool receiveComplete = false;
                var readBuffer = new byte[READ_BUFFER_SIZE];
                var totalBytesRead = 0;
                try
                {
                    // Read from the serial port
                    while (!receiveComplete)
                    {
                        byte b = (byte)_serialPort.ReadByte();

                        if (b == (byte)RemoteCommandChars.DeviceControl1
                            || b == (byte)RemoteCommandChars.DeviceControl2
                            || b == (byte)RemoteCommandChars.DeviceControl3
                            || b == (byte)RemoteCommandChars.DeviceControl4
                        )
                        {
                            continue;
                        }

                        readBuffer[totalBytesRead] = b;
                        totalBytesRead++;

                        if (b == (byte)RemoteCommandChars.Acknowledge)
                        {
                            response.Status = CommandResponseStatus.Success;
                            receiveComplete = true;
                        }
                        else if (b == (byte)RemoteCommandChars.NegativeAcknowledge)
                        {
                            response.Status = CommandResponseStatus.Error;
                            receiveComplete = true;
                        }
                        else if (b == (byte)RemoteCommandChars.CommandAccepted)
                        {
                            response.Status = CommandResponseStatus.Accepted;
                            receiveComplete = true;
                        }
                    }

                    //we should have full response here...
                    if (totalBytesRead > 0)
                    {
                        var bytesRead = new byte[totalBytesRead];
                        Array.Copy(readBuffer, 0, bytesRead, 0, totalBytesRead);
                        response.RawBytes = bytesRead;

                        //error checking now...
                        if (response.Status == CommandResponseStatus.Error)
                        {
                            response.ErrorMessage = ParseError(bytesRead);
                        }

                        ReceiveTerminalFormatted.Report(response.FormattedResult);
                        ReceiveTerminalRaw.Report(response.RawBytes);
                    }
                }
                catch (Exception ex)
                {
                    Globals.Exceptions.Add(new AppException(ex));
                    response.ErrorMessage = ex.Message;
                    var bytesRead = new byte[totalBytesRead];
                    Array.Copy(readBuffer, 0, bytesRead, 0, totalBytesRead);
                    response.RawBytes = bytesRead;
                    response.Status = CommandResponseStatus.Aborted;
                }
                SetSerialStatus(CommunicationDirection.Idle);
            }
        }

        //public static void SendAbort()
        //{
        //    //SendString("\0x10");
        //}

        /// <summary>
        /// Test if the Cartridge is currently inserted into the Fluke. This method will still return true if the Cartridge is write-protected.
        /// </summary>
        /// <returns>True if the cartridge is inserted.</returns>
        public static bool IsCartridgeAvailable()
        {
            RemoteCommandResponse cr = Fluke900.SendCommand(RemoteCommandCodes.GetDirectoryCartridge, null);
            return cr.Status == CommandResponseStatus.Success;
        }

        /// <summary>
        /// Tests if the Cartridge is inserted and also writable by writing a small test file to the Cartridge. The test file is immediately deleted after creation.
        /// </summary>
        /// <returns>True if the cartridge is able to receive files.</returns>
        public static bool? IsCartridgeWritable()
        {
            if (!IsConnected()) return false; 

            bool? isWritable = null;
            string testFile = Globals.CARTRIDGE_TEST_FILENAME;

            RemoteCommand command = RemoteCommandFactory.GetCommand(RemoteCommandCodes.UploadFile, new string[] { testFile });
            RemoteCommandResponse response = new RemoteCommandResponse();

            Fluke900.SendCommandOnly(command);
            Fluke900.GetResponse(response);
            if (response.Status == CommandResponseStatus.Accepted)
            {
                RemoteCommand commandFile = RemoteCommandFactory.GetCommand(RemoteCommandCodes.DataString, new string[] { ";WRITETEST;" });
                RemoteCommandResponse responseFile = new RemoteCommandResponse();

                Fluke900.SendCommandOnly(commandFile);
                Fluke900.GetResponse(responseFile);

                if (responseFile.Status == CommandResponseStatus.Success)
                {
                    isWritable = true;
                    DeleteFile(testFile);
                }
            }
            return isWritable;
        }

        /// <summary>
        /// Returns the current connection state of the SerialPort.
        /// </summary>
        /// <returns>True if the SerialPort is instantiated and Open.</returns>
        public static bool IsConnected()
        {
            if (_serialPort != null)
            {
                return _serialPort.IsOpen;
            }
            return false;
        }

        public static bool SoftReset()
        {
            if (IsConnected())
            {
                RemoteCommandResponse cr = Fluke900.SendCommand(RemoteCommandCodes.SoftReset, null);
                return cr.Status == CommandResponseStatus.Success;
            }
            return false;
        }

        public static bool HardReset()
        {
            if (IsConnected())
            {
                RemoteCommandResponse cr = Fluke900.SendCommand(RemoteCommandCodes.SoftReset, null);
                return cr.Status == CommandResponseStatus.Success;
            }
            return false;
        }


        public static CompilationResult CompileFile(string file)
        {
            RemoteCommandResponse response1 = SendCommand(RemoteCommandCodes.CompileFile, new string[] { file });
            if (response1.Status == CommandResponseStatus.Accepted)
            {
                RemoteCommandResponse response2 = new RemoteCommandResponse();
                GetResponse(response2);
                return response2.AsCompilationResult();

            }
            return null;
        }

        /// <summary>
        /// Transfers a file from one location to another. Each file specifier is made up of a Path (PC Only), the Filename and a Location Code suffix.
        /// </summary>
        /// <param name="source">The file that will be copied.</param>
        /// <param name="destination">The destination for the file copy. On the Fluke, files will 
        /// automatically overwrite any existing file with the same name. On the PC, the user is 
        /// prompted for overwriting of files and they may cancel the copy action.</param>
        /// <returns></returns>
        public static int TransferFile(string source, string destination)
        {
            return TransferFile(source, destination, true);
        }

        public static int TransferFile(string source, string destination, bool checkDestinationExists)
        {
            int filesCopied = 0;

            FileLocations? sourceLocation = FileHelper.GetFileLocation(source);
            FileLocations? destLocation = FileHelper.GetFileLocation(destination);

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
                    RemoteCommandResponse response = Fluke900.SendCommand(RemoteCommandCodes.UploadFile, new string[] { FileHelper.AdjustForTransfer(destination) });
                    if (response.Status == CommandResponseStatus.Accepted)
                    {
                        string fileContent = File.ReadAllText(sourceFile, Encoding.ASCII).Replace("\n","").Replace("\t", new string(' ', Properties.Settings.Default.ConvertTabsToSpaces));

                        RemoteCommand commandFile = RemoteCommandFactory.GetCommand(RemoteCommandCodes.DataString, new string[] { fileContent });
                        Fluke900.SendCommandOnly(commandFile);
                        RemoteCommandResponse responseFile = new RemoteCommandResponse();

                        Fluke900.GetResponse(responseFile);
                        Console.Write(Encoding.ASCII.GetString(responseFile.RawBytes));

                        if (responseFile.Status == CommandResponseStatus.Success)
                        {
                            filesCopied++;
                        }
                        else
                        {
                            ProgressManager.Stop();
                            //PortManager.SendAbort();
                            MessageBox.Show("Error copying file '" + source + "': " + response.ErrorMessage, "Error Copying File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return filesCopied;
                        }
                    }
                    else
                    {
                        ProgressManager.Stop();
                        //PortManager.SendAbort();
                        MessageBox.Show("Error copying file '" + destination + "': " + response.ErrorMessage, "Error Copying File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                if (checkDestinationExists && File.Exists(localPathFilename))
                {
                    DialogResult dr = MessageBox.Show("File already exists: '" + sourceFileName + "', are you sure you want to overwrite it?", "Confirm Overwrite", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                    if (dr == DialogResult.Cancel)
                    {
                        ProgressManager.Stop();
                        return filesCopied;
                    }
                    else if (dr == DialogResult.No)
                    {
                        return -1;
                    }
                }
                //conflicts resolved, do the copy now..
                RemoteCommandResponse cr = Fluke900.SendCommand(RemoteCommandCodes.DownloadFile, new string[] { sourceFileName });
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


        public static bool ReceiveCommandAccepted()
        {
            if (_serialPort.IsOpen)
            {
                // Read from the serial port
                //DataReceived(EventArgs.Empty, true);

                byte b = (byte)_serialPort.ReadByte();
                return b == (byte)RemoteCommandChars.CommandAccepted;               
            }
            return false;
        }

        //public static bool SendString(string dataString)
        //{
        //    if (_serialPort.IsOpen)
        //    {
        //        try
        //        {

        //            //all general commands come here
        //            byte[] bytes = Encoding.ASCII.GetBytes(dataString);
        //            System.Diagnostics.Debug.WriteLine("Writing Bytes: " + dataString);
        //            _serialPort.Write(bytes, 0, bytes.Length);
        //            System.Diagnostics.Debug.WriteLine("Complete!");
                    
        //            //SendTerminalFormatted.Report(dataString);
        //            LogSendTerminalFormatted(dataString);
        //            //SendTerminalRaw.Report(bytes);
        //            LogSendTerminalRaw(bytes);
        //            //DataSent(EventArgs.Empty, false);
        //            return true;
        //        }
        //        catch (Exception ex)
        //        {
        //            Globals.Exceptions.Add(new AppException(ex));
        //        }
        //    }
        //    return false;
        //}

        public static byte[] ReceiveSelfTestData()
        {
            var readBuffer = new byte[READ_BUFFER_SIZE];
            bool receiveComplete = false;
            var totalBytesRead = 0;

            try
            {
                // Read from the serial port
                while (!receiveComplete)
                {
                    byte b = (byte)_serialPort.ReadByte();
                    readBuffer[totalBytesRead] = b;
                    totalBytesRead++;

                    if (b == 0x0d && readBuffer[totalBytesRead - 2] == 0x0c)
                    {
                        receiveComplete = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Globals.Exceptions.Add(new AppException(ex));
            }

            //we should have full response here...
            var bytesRead = new byte[totalBytesRead];
            if (totalBytesRead > 0)
            {

                Array.Copy(readBuffer, 0, bytesRead, 0, totalBytesRead);
            }
            return bytesRead;
        }

        public static string ParseError(byte[] responseBytes)
        {
            StringBuilder sb = new StringBuilder();

            int startIndex = 0;
            //incrementIndex to skip any control characters, up to a certain number of trims... say 10
            int counter = 0;
            while (counter < 10 && startIndex < responseBytes.Length - 1)
            {
                if (responseBytes[startIndex] < 0x04)
                {
                    startIndex++;
                    counter++;
                }
                else
                {
                    break;
                }
            }



            byte responseChar = responseBytes.ToArray()[startIndex];
            sb.Append(Enum.GetName(typeof(RemoteCommandError), responseChar));
            if (Enum.IsDefined(typeof(RemoteCommandError), responseChar))
            {
                RemoteCommandError enumeratedError = (RemoteCommandError)responseChar;

                if (enumeratedError == RemoteCommandError.OtherError)
                {
                    //this is the character after the 'OtherError' byte
                    sb.Clear();
                    StringBuilder otherCode = new StringBuilder();
                    for (int i = startIndex + 1; i < responseBytes.Length - 1; i++)
                    {
                        otherCode.Append(Encoding.ASCII.GetString(new byte[] { responseBytes[i] }));
                    }
                    if (ExtendedErrors.CodeValues.ContainsKey(otherCode.ToString()))
                    {
                        sb.Append(ExtendedErrors.CodeValues[otherCode.ToString()]);
                    }
                    else
                    {
                        sb.Append(otherCode.ToString());
                    }
                }
                else
                {
                    sb.Append(Enum.GetName(typeof(RemoteCommandError), enumeratedError));
                }
            }
            else
            {
                sb.Append("UNKNOWN ERROR RESPONSE!!!!!");
            }
            return sb.ToString().ToUpper();
        }

        public static bool SyncDateToUnit()
        {
            bool result = false;

            try
            {
                DateTime currentDateTime = DateTime.Now;
                string flukeFormattedDate = currentDateTime.Day.ToString("00") + "/" + currentDateTime.Month.ToString("00") + "/" + currentDateTime.ToString("yy");
                RemoteCommandResponse stcr = SendCommand(RemoteCommandCodes.SetDateTime, new string[] { flukeFormattedDate });
                result = true;
            }
            catch (Exception ex)
            {
                Globals.Exceptions.Add(new AppException(ex));
            }
            return result;
        }

        public static bool SyncTimeToUnit()
        {
            bool result = false;

            try
            {
                DateTime currentDateTime = DateTime.Now;
                string flukeFormattedTime = currentDateTime.Hour.ToString("00") + ":" + currentDateTime.Minute.ToString("00");
                RemoteCommandResponse stcr = SendCommand(RemoteCommandCodes.SetDateTime, new string[] { flukeFormattedTime });
                result = true;
            }
            catch (Exception ex)
            {
                Globals.Exceptions.Add(new AppException(ex));
            }
            return result;
        }

        #region File Methods

        public static bool FileExists(string pathFileName)
        {
            bool result = false;

            FileLocations? fileLocation = FileHelper.GetFileLocation(pathFileName);
            DirectoryListingInfo directoryInfo = null;

            if (fileLocation.HasValue)
            {
                switch (fileLocation.Value)
                {
                    case FileLocations.FlukeCartridge:
                    case FileLocations.FlukeSystem:
                        directoryInfo = GetDirectoryListing(fileLocation.Value);
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

        public static DirectoryListingInfo GetDirectoryListing(FileLocations fileLocation)
        {
            DirectoryListingInfo directoryInfo = null;
            RemoteCommandResponse response = new RemoteCommandResponse();

            switch (fileLocation)
            {
                case FileLocations.FlukeCartridge:
                    response = Fluke900.SendCommand(RemoteCommandCodes.GetDirectoryCartridge, null);
                    break;
                case FileLocations.FlukeSystem:
                    response = Fluke900.SendCommand(RemoteCommandCodes.GetDirectorySystem, null);
                    break;
                case FileLocations.LocalComputer:
                    throw new Exception("Cannot get PC Directory from Fluke, ask someone else.");
            }
            if (response != null)
            {
                directoryInfo = CommandResponseToDirectoryListing(response);
            }
            return directoryInfo;
        }

        public static bool DeleteFile(string fileName)
        {
            bool success = false;

            FileLocations? location = FileHelper.GetFileLocation(fileName);

            switch (location)
            {
                case FileLocations.FlukeCartridge:
                case FileLocations.FlukeSystem:
                    RemoteCommandResponse response = Fluke900.SendCommand(RemoteCommandCodes.DeleteFile, new string[] { fileName });
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

        private static DirectoryListingInfo CommandResponseToDirectoryListing(RemoteCommandResponse cr)
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

        #endregion




        #region UI Progress Actions



        private static void LogSendTerminalFormatted(RemoteCommand c)
        {
            //when data is sent, we will always log it to these windows if they are open..
            if (Globals.UIElements.TerminalFormattedWindow != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("//-----------------------------//");
                sb.AppendLine("// BEGIN SEND                  //");
                sb.AppendLine("//-----------------------------//");
                sb.Append(c.CommandString);
                if (c.Parameters != null)
                {
                    sb.Append(String.Join("\r", c.Parameters));
                }
                //sb.Append(Environment.NewLine);
                Globals.UIElements.TerminalFormattedWindow.WriteLine(sb.ToString());
            }
        }

        private static void LogReceiveTerminalFormatted(string str)
        {
            if (Globals.UIElements.TerminalFormattedWindow != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("//--- WAITING FOR RESPONSE ----//");
                sb.AppendLine(str);
                sb.AppendLine("//-----------------------------//");
                sb.AppendLine("// END RECEIVE                 //");
                sb.AppendLine("//-----------------------------//");
                //sb.AppendLine("");
                Globals.UIElements.TerminalFormattedWindow.WriteLine(sb.ToString());
            }
        }

        private static void LogSendTerminalRaw(byte[] bytesToSend)
        {
            //when data is sent, we will always log it to these windows if they are open..
            if (Globals.UIElements.TerminalRawWindow != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SEND--> ");
                sb.Append(Encoding.ASCII.GetString(bytesToSend));
                Globals.UIElements.TerminalRawWindow.WriteLine(sb.ToString(), Color.Green, false);
            }
        }

        private static void LogReceiveTerminalRaw(byte[] bytes)
        {
            if (Globals.UIElements.TerminalRawWindow != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("RECV<-- ");
                //foreach (byte b in bytes)
                //{
                    //sb.Append(b.ToString("X").PadLeft(2).Replace(" ", "0"));
                    //sb.Append(" ");
                //}
                sb.Append(Encoding.ASCII.GetString(bytes));
                Globals.UIElements.TerminalRawWindow.WriteLine(sb.ToString(), Color.Red, false);
            }
        }

#endregion

        #region Events

        //public static event DataActivityEventHandler OnDataSent;
        //public static event DataActivityEventHandler OnDataReceived;
        public static event DataConnectionEventHander OnConnectionStatusChanged;

        public static void SetSerialStatus(CommunicationDirection direction)
        {
            if (OnDataStatusChanged != null)
            {
                bool sending = false;
                bool receiving = false;

                switch (direction)
                {
                    case CommunicationDirection.Receive:
                        receiving = true;
                        break;
                    case CommunicationDirection.Send:
                        sending = true;
                        break;
                }
                OnDataStatusChanged(sending, receiving);
                Application.DoEvents();
            }
        }
        //public static void DataSent(EventArgs e,  bool status)
        //{
        //    if (OnDataSent != null)
        //    {
        //        OnDataSent(e, status);
        //    }
        //}

        //public static void DataReceived(EventArgs e, bool status)
        //{
        //    if (OnDataReceived != null)
        //    {
        //        OnDataReceived(e, status);
        //    }
        //}

        public static void ConnectionStatusChanged(EventArgs e, ConnectionStatus previousStatus, ConnectionStatus currentStatus)
        {
            if (OnConnectionStatusChanged != null)
            {
                OnConnectionStatusChanged(e, previousStatus, currentStatus);
                Application.DoEvents();
            }
        }

        #endregion
    }
}
