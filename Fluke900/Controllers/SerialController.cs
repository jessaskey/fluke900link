﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fluke900.Containers;
using RJCP.IO.Ports;

namespace Fluke900.Controllers
{
    public static class SerialController
    {
        private const int READ_BUFFER_SIZE = 65536;

        private static SerialPortStream _serialPort = null;
        private static ClientCommand _currentCommand = null;
        private static ClientCommandResponse _currentResponse = null;

        public static string Port = "COM1";
        public static int BaudRate = (int)BaudRates.Rate9600;
        public static Parity Parity = Parity.Even;
        public static int DataBits = 7;
        public static StopBits StopBits = StopBits.One;
        public static int Timeout = 10000;

        public static IProgress<ConnectionStatus> ConnectionStatusProgress = null;
        public static IProgress<CommunicationDirection> DataStatusProgress = null;
        public static IProgress<byte[]> DataSendProgress = null;
        public static IProgress<byte[]> DataReceiveProgress = null;
        public static IProgress<ClientCommand> CommandSendProgress = null;
        public static IProgress<ClientCommandResponse> CommandResponseProgress = null;

        public static List<AppException> Exceptions = new List<AppException>();

        public static bool OpenPort()
        {
            bool success = false;
            try
            {
                _serialPort = new SerialPortStream(Port, (int)BaudRate, (int)DataBits, Parity, StopBits);
                _serialPort.ReadTimeout = Timeout;
                _serialPort.ReadBufferSize = READ_BUFFER_SIZE;
                _serialPort.WriteBufferSize = READ_BUFFER_SIZE;
                _serialPort.Encoding = Encoding.ASCII;
                _serialPort.DataReceived += SerialPort_DataReceived;
                // _serialPort.DtrEnable = true;
                // _serialPort.RtsEnable = true;
                // _serialPort.Handshake = Handshake.RtsXOn; // Handshake.RequestToSendXOnXOff;
                _serialPort.Open();
                if (ConnectionStatusProgress != null) ConnectionStatusProgress.Report(ConnectionStatus.Connected);
                success = true;
            }
            catch (Exception ex)
            {
                Exceptions.Add(new AppException(ex));
            }
            return success;
        }

        public static void ClosePort()
        {
            if (_serialPort != null)
            {
                _serialPort.DataReceived -= SerialPort_DataReceived;
                _serialPort.Close();
                _serialPort.Dispose();
                _serialPort = null;
                if (ConnectionStatusProgress != null) ConnectionStatusProgress.Report(ConnectionStatus.Disconnected);
            }
        }

        public static bool IsConnected
        {
            get
            {
                if (_serialPort != null)
                {
                    return _serialPort.IsOpen;
                }
                return false;
            }
        }

        /// <summary>
        /// Write a line to the ICommunicationPortAdaptor asynchronously followed
        /// immediately by attempting to read a line from the same port. Useful
        /// for COMMAND --> RESPONSE type communication.
        /// </summary>
        /// <param name="serialPort">The port to process commands through</param>
        /// <param name="command">The command to send through the port</param>
        /// <returns>The response from the port</returns>
        public static async Task<bool> SendCommand(ClientCommand command)
        {
            _currentResponse = null;

            if (_currentCommand != null)
            {
                throw new Exception("There is already a command in process: " + _currentCommand.CommandString);
            }
            else
            {
                if (_serialPort != null)
                {
                    _currentCommand = command;
                    if (DataStatusProgress != null) DataStatusProgress.Report(CommunicationDirection.Send);
                    if (CommandSendProgress != null) CommandSendProgress.Report(command);
                    if (DataSendProgress != null) DataSendProgress.Report(command.BytesToSend);

                    await _serialPort.WriteAsync(command.BytesToSend, 0, command.BytesToSend.Length);
                    await _serialPort.FlushAsync();
                    //log it 
                    
                    if (DataStatusProgress != null) DataStatusProgress.Report(CommunicationDirection.Idle);
                    
                    //get response or at least the first chunk of it
                    while (_currentResponse == null || _currentResponse.Status == CommandResponseStatus.Accepted)
                    {
                        //await System.Threading.Thread.Sleep(100);
                    }
                    //command has responded intially or finally, push into history and clear current
                    _currentCommand.Response = _currentResponse;
                    _currentCommand = null;
                    return true;
                }
            }
            return false;
        }

        private static void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (_serialPort.BytesToRead > 0)
            {
                var bytes = new byte[_serialPort.BytesToRead];
                _serialPort.Read(bytes, 0, bytes.Length);

                if (DataReceiveProgress != null)
                {
                    DataReceiveProgress.Report(bytes);
                }

                if (_currentCommand == null)
                {

                }
                else
                {
                    if (_currentResponse == null)
                    {
                        _currentResponse = new ClientCommandResponse(_currentCommand);
                        var combined = new byte[_currentResponse.RawBytes.Length + bytes.Length];
                        if (_currentResponse.RawBytes.Length > 0)
                        {
                            _currentResponse.RawBytes.CopyTo(combined, 0);
                        }
                        bytes.CopyTo(combined, _currentResponse.RawBytes.Length);
                        _currentResponse.RawBytes = combined;
                        byte lastByte = _currentResponse.RawBytes[_currentResponse.RawBytes.Length - 1];
                        if (lastByte == (byte)CommandCharacters.Acknowledge)
                        {
                            _currentResponse.Status = CommandResponseStatus.Success;
                            if (CommandResponseProgress != null)
                            {
                                CommandResponseProgress.Report(_currentResponse);
                            }
                        }
                        else if (lastByte == (byte)CommandCharacters.NegativeAcknowledge)
                        {
                            _currentResponse.Status = CommandResponseStatus.Error;
                            if (CommandResponseProgress != null)
                            {
                                CommandResponseProgress.Report(_currentResponse);
                            }
                        }
                        else if (lastByte == (byte)CommandCharacters.Substitute)
                        {
                            _currentResponse.Status = CommandResponseStatus.Executing;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Wait for a valid response from the Fluke via the serial port. This method is blocking.
        /// </summary>
        /// <param name="response">A prebuilt CommandResponse to fill.</param>
        //public static async Task<ClientCommandResponse> ReceiveResponseAsync(ClientCommand sentCommand)
        //{
        //    ClientCommandResponse response = new ClientCommandResponse(sentCommand);

        //    if (DataStatusProgress != null) DataStatusProgress.Report(CommunicationDirection.Receive);

        //    bool receiveComplete = false;
        //    var readBuffer = new byte[READ_BUFFER_SIZE];
        //    var totalBytesRead = 0;
        //    try
        //    {
        //        // Read from the serial port
        //        while (!receiveComplete)
        //        {
        //            byte b = (byte)_serialPort.ReadByte();

        //            if (b == (byte)CommandCharacters.StartText
        //                || b == (byte)CommandCharacters.DeviceControl2
        //                || b == (byte)CommandCharacters.DeviceControl3
        //                || b == (byte)CommandCharacters.DeviceControl4
        //            )
        //            {
        //                continue;
        //            }

        //            readBuffer[totalBytesRead] = b;
        //            totalBytesRead++;

        //            if (b == (byte)CommandCharacters.Acknowledge)
        //            {
        //                response.Status = CommandResponseStatus.Success;
        //                receiveComplete = true;
        //            }
        //            else if (b == (byte)CommandCharacters.NegativeAcknowledge)
        //            {
        //                response.Status = CommandResponseStatus.Error;
        //                receiveComplete = true;
        //            }
        //            else if (b == (byte)CommandCharacters.Substitute)
        //            {
        //                response.Status = CommandResponseStatus.Executing;
        //                receiveComplete = true;
        //            }
        //        }

        //        //we should have full response here...
        //        if (totalBytesRead > 0)
        //        {
        //            response.RawBytes = new byte[totalBytesRead];
        //            Array.Copy(readBuffer, 0, response.RawBytes, 0, totalBytesRead);

        //            //error checking now...
        //            if (response.Status == CommandResponseStatus.Error)
        //            {
        //                response.ErrorMessage = ParseError(response.RawBytes);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Exceptions.Add(new AppException(ex));
        //        response.ErrorMessage = ex.Message;
        //        var bytesRead = new byte[totalBytesRead];
        //        Array.Copy(readBuffer, 0, bytesRead, 0, totalBytesRead);
        //        response.RawBytes = bytesRead;
        //        response.Status = CommandResponseStatus.Aborted;
        //    }

        //    //logging here, these need to call on the UI thread, so report them via IProgress
        //    if (DataStatusProgress != null) DataStatusProgress.Report(CommunicationDirection.Idle);
        //    if (DataReceiveProgress != null) DataReceiveProgress.Report(response.RawBytes);

        //    return response;
        //}

        //public static void SendBinary(byte[] bytes)
        //{
        //    _serialPort.Write(bytes, 0, bytes.Length);
        //}

        private static string ParseError(byte[] responseBytes)
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

        #region UI Progress Actions



        //private static void LogSendTerminalFormatted(RemoteCommand c)
        //{
        //    //when data is sent, we will always log it to these windows if they are open..
        //    if (ControlFactory.UIElements.TerminalFormattedWindow != null)
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        sb.AppendLine("//-----------------------------//");
        //        sb.AppendLine("// BEGIN SEND                  //");
        //        sb.AppendLine("//-----------------------------//");
        //        sb.Append(c.CommandString);
        //        if (c.Parameters != null)
        //        {
        //            sb.Append(String.Join("\r", c.Parameters));
        //        }
        //        //sb.Append(Environment.NewLine);
        //        ControlFactor.UIElements.TerminalFormattedWindow.WriteLine(sb.ToString());
        //    }
        //}

        //private static void LogReceiveTerminalFormatted(string str)
        //{
        //    if (ControlFactory.UIElements.TerminalFormattedWindow != null)
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        sb.AppendLine("//--- WAITING FOR RESPONSE ----//");
        //        sb.AppendLine(str);
        //        sb.AppendLine("//-----------------------------//");
        //        sb.AppendLine("// END RECEIVE                 //");
        //        sb.AppendLine("//-----------------------------//");
        //        //sb.AppendLine("");
        //        ControlFactory.UIElements.TerminalFormattedWindow.WriteLine(sb.ToString());
        //    }
        //}

        //private static void LogSendTerminalRaw(byte[] bytesToSend)
        //{
        //    if (SendTerminalRaw != null)
        //    {
        //        SendTerminalRaw.Report(
        //    }
        //    //when data is sent, we will always log it to these windows if they are open..
        //    if (ControlFactory.UIElements.TerminalRawWindow != null)
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        sb.Append("SEND--> ");
        //        sb.Append(Encoding.ASCII.GetString(bytesToSend));
        //        ControlFactory.UIElements.TerminalRawWindow.WriteLine(sb.ToString(), Color.Green, false);
        //    }
        //}

        //private static void LogReceiveTerminalRaw(byte[] bytes)
        //{
        //    if (ControlFactory.UIElements.TerminalRawWindow != null)
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        sb.Append("RECV<-- ");
        //        //foreach (byte b in bytes)
        //        //{
        //        //sb.Append(b.ToString("X").PadLeft(2).Replace(" ", "0"));
        //        //sb.Append(" ");
        //        //}
        //        sb.Append(Encoding.ASCII.GetString(bytes));
        //        ControlFactory.UIElements.TerminalRawWindow.WriteLine(sb.ToString(), Color.Red, false);
        //    }
        //}

        #endregion

        #region Events

        //public static void SetSerialStatus(CommunicationDirection direction)
        //{
        //    if (OnDataStatusChanged != null)
        //    {
        //        bool sending = false;
        //        bool receiving = false;

        //        switch (direction)
        //        {
        //            case CommunicationDirection.Receive:
        //                receiving = true;
        //                break;
        //            case CommunicationDirection.Send:
        //                sending = true;
        //                break;
        //        }
        //        OnDataStatusChanged(sending, receiving);
        //    }
        //}

        //public static void ConnectionStatusChanged(EventArgs e, ConnectionStatus previousStatus, ConnectionStatus currentStatus)
        //{
        //    if (OnConnectionStatusChanged != null)
        //    {
        //        OnConnectionStatusChanged(e, previousStatus, currentStatus);
        //    }
        //}

        #endregion



        /// <summary>
        /// Send a Command to the Fluke via the passed command code without parameters
        /// </summary>
        /// <param name="commandCode">The CommandCode enumeration</param>
        /// <returns></returns>
        //public static async Task<ClientCommandResponse> SendCommand(ClientCommands commandCode)
        //{
        //    return await SendCommand(commandCode, null);
        //}

        /// <summary>
        /// Send a Command to the Fluke via the passed command code and parameters
        /// </summary>
        /// <param name="commandCode">The CommandCode enumeration</param>
        /// <param name="parameters">A string array of parameters to be passed with the Array</param>
        /// <returns></returns>
        //public static async Task<ClientCommandResponse> SendCommand(ClientCommands commandCode, string parameters)
        //{
        //    ClientCommand command = ClientCommandFactory.GetCommand(commandCode, parameters);
        //    //ClientCommandResponse response = new ClientCommandResponse(command);
        //    await SendCommand(command);
        //    return command.Response;
        //}

        /// <summary>
        /// Send a command to the Fluke with a Prebuilt command
        /// </summary>
        /// <param name="command">The Command object to send</param>
        /// <returns></returns>
        //public static void SendCommand(RemoteCommand command, AsyncCallback callback)
        //{
        //    bool success = false;

        //    if (_serialPort.IsOpen)
        //    {
        //        SetSerialStatus(CommunicationDirection.Send);

        //        _serialPort.BeginWrite(command.BytesToSend, 0, command.BytesToSend.Length, SendCallback, command);


        //        //_serialPort.BeginWrite(command.BytesToSend,
        //        //                        0,
        //        //                        command.BytesToSend.Length,
        //        //                        (AsyncCallback)((ar) =>
        //        //                        {
        //        //                            //log it 
        //        //                            SendTerminalFormatted.Report(command);
        //        //                            SendTerminalRaw.Report(command.BytesToSend);
        //        //                            //send is complete.... get a response

        //        //                            RemoteCommandResponse response = new RemoteCommandResponse(command);
        //        //                            GetResponse(response);

        //        //                            callback(ar);
        //        //                        }),
        //        //                        null);

        //    }
        //}

        /// <summary>
        /// Write a line to the SerialPort asynchronously
        /// </summary>
        /// <param name="serialPort">The port to send text to</param>
        /// <param name="str">The text to send</param>
        /// <returns></returns>
        //public static async Task WriteBytesAsync(byte[] bytes)
        //{
        //    await _serialPort.WriteAsync(bytes, 0, bytes.Length);
        //    await _serialPort.FlushAsync();
        //}
    }
}
