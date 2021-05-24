using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fluke900.Containers;
using RJCP.IO.Ports;

namespace Fluke900.Controllers
{
    public static class Fluke900Controller
    {
        private static SerialPortStream _serialPort = null;

        public static string Port = "COM1";
        public static int BaudRate = (int)BaudRates.Rate9600;
        public static Parity Parity = Parity.Even;
        public static int DataBits = 7;
        public static StopBits StopBits = StopBits.One;
        public static int Timeout = 10000;

        private const int READ_BUFFER_SIZE = 65536;

        public static IProgress<ConnectionStatus> ConnectionStatusProgress = null;
        public static IProgress<CommunicationDirection> DataStatusProgress = null;
        public static IProgress<ClientCommand> DataSendProgress = null;
        public static IProgress<ClientCommandResponse> DataReceiveProgress = null;

        public static List<AppException> Exceptions = new List<AppException>();

        public static bool Connect()
        {
            bool success = false;
            try
            {
                _serialPort = new SerialPortStream(Port, (int)BaudRate, (int)DataBits, Parity, StopBits);
                _serialPort.ReadTimeout = Timeout;
                _serialPort.ReadBufferSize = READ_BUFFER_SIZE;
                _serialPort.WriteBufferSize = READ_BUFFER_SIZE;
                _serialPort.Encoding = Encoding.ASCII;
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

        public static void Disconnect()
        {
            if (_serialPort != null)
            {
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

       
        public static void SendBinary(byte[] bytes)
        {
            _serialPort.Write(bytes, 0, bytes.Length);
        }

        public static ClientCommand ReceiveCommand()
        {
            ClientCommand command = null;
            bool receiveComplete = false;
            var readBuffer = new byte[READ_BUFFER_SIZE];
            var totalBytesRead = 0;
            try
            {
                // Read from the serial port
                while (!receiveComplete)
                {
                    Application.DoEvents();
                    if (_serialPort == null)
                    {
                        break;
                    }
                    byte b = (byte)_serialPort.ReadByte();

                    
                    if (b == (byte)CommandCharacters.StartText)
                    {
                        continue;
                    }

                    if (totalBytesRead == 0 && b == 255)
                    {
                        //if we haven't started, don't keep FF's
                        continue;
                    }

                    readBuffer[totalBytesRead] = b;
                    totalBytesRead++;

                    if (b == (byte)CommandCharacters.EndText)
                    {
                        receiveComplete = true;
                    }
                    else if (b == (byte)CommandCharacters.EndOfTransmission)
                    {
                        receiveComplete = true;
                    }
                    else if (b == (byte)CommandCharacters.NegativeAcknowledge)
                    {
                        receiveComplete = true;
                    }
                    else if (b == (byte)CommandCharacters.CommandAccepted)
                    {
                        receiveComplete = true;
                    }
                }         
                //we should have full response here...
                if (totalBytesRead > 0)
                {
                    string commandText = Encoding.ASCII.GetString(readBuffer.Take(totalBytesRead-1).ToArray());
                    command = new ClientCommand(commandText.Replace("\u0010",""));
                }
            }
            catch (Exception ex)
            {
                Exceptions.Add(new AppException(ex));
                var bytesRead = new byte[totalBytesRead];
                Array.Copy(readBuffer, 0, bytesRead, 0, totalBytesRead);
            }

            return command;
        }

    }
}
