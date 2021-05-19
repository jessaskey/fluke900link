using Fluke900.Containers;
using Fluke900.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fluke900Emu
{
    public partial class Form1 : Form
    {
        private bool _connected = false;
        private TestParameters _testParameters = new TestParameters();

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonConnectDisconnect_Click(object sender, EventArgs e)
        {
            if (!_connected)
            {
                //connect
                SerialPortController.Port = "COM6";
                SerialPortController.BaudRate = 9600;
                SerialPortController.Parity = RJCP.IO.Ports.Parity.None;
                SerialPortController.DataBits = 8;
                SerialPortController.StopBits = RJCP.IO.Ports.StopBits.One;

                _connected = SerialPortController.Connect();
                if (_connected) {
                    buttonConnectDisconnect.Text = "Disconnect";
                    Listen();
                }
            }
            else
            {
                _connected = false;
                SerialPortController.Disconnect();
                buttonConnectDisconnect.Text = "Connect";
            }
        }

        private void Listen()
        {
            List<string> textOutput = new List<string>();
            try
            {
                string lastTextString = "";
                int lastCounter = 0;
                while (_connected)
                {
                    List<byte> bytes = new List<byte>();
                    StringBuilder sb = new StringBuilder();
                    RemoteCommand command = SerialPortController.ReceiveCommand();
                    if (command != null)
                    {
                        switch (command.CommandCode)
                        {
                            case RemoteCommandCodes.Identify:
                                bytes.Add((byte)RemoteCommandChars.StartText);
                                bytes.AddRange(Encoding.ASCII.GetBytes("Fluke 900\rP\r6.00\rL3.00\rM0\rH1\rI0"));
                                bytes.Add((byte)RemoteCommandChars.Acknowledge);
                                break;
                            case RemoteCommandCodes.ExitRemoteMode:
                                bytes.Add((byte)RemoteCommandChars.Acknowledge);
                                break;
                            case RemoteCommandCodes.ReadPinDefinition:
                                bytes.Add((byte)RemoteCommandChars.StartText);
                                bytes.AddRange(Encoding.ASCII.GetBytes("A\rI"));
                                bytes.Add((byte)RemoteCommandChars.Acknowledge);
                                break;
                            case RemoteCommandCodes.ReadResetDefinition:
                                bytes.Add((byte)RemoteCommandChars.StartText);
                                bytes.AddRange(Encoding.ASCII.GetBytes("P\rI\r-200\r500"));
                                bytes.Add((byte)RemoteCommandChars.EndText);
                                break;
                            case RemoteCommandCodes.SetRDDrive:
                                _testParameters.ReferenceDeviceDrive = command.Parameters[0] == "H" ? true : false;
                                bytes.Add((byte)RemoteCommandChars.Acknowledge);
                                break;
                            case RemoteCommandCodes.GetRDDrive:
                                bytes.Add((byte)RemoteCommandChars.StartText);
                                bytes.AddRange(Encoding.ASCII.GetBytes(_testParameters.ReferenceDeviceDrive ? "H":"L"));
                                bytes.Add((byte)RemoteCommandChars.Acknowledge);
                                break;
                            default:
                                bytes.Add((byte)RemoteCommandChars.Acknowledge);
                                break;
                        }
                    }
                    if (bytes.Count > 0)
                    {
                        //send first
                        SerialPortController.SendBinary(bytes.ToArray());
                        //update UI
                        string currentText = command.CommandCode + "(" + command.CommandString + ")";
                        if (!String.IsNullOrEmpty(lastTextString) && lastTextString == currentText)
                        {
                            lastCounter++;
                            textOutput[0] = currentText + " " + lastCounter.ToString() + " times.";
                        }
                        else
                        {
                            textOutput.Insert(0, currentText);
                            lastCounter = 1;
                        }
                        lastTextString = currentText;
                        textBoxCommands.Text = String.Join("\r\n", textOutput.ToArray());
                    }
                    Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                _connected = false;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _connected = false;
            e.Cancel = false;
        }
    }
}
