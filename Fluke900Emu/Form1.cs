using Fluke900;
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
        private bool _simulationInstalled = true;
        //private TestParameters _testParameters = new TestParameters();
        private ProjectLocation _defaultTestParameters = new ProjectLocation();

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonConnectDisconnect_Click(object sender, EventArgs e)
        {
            if (!_connected)
            {
                //connect
                Fluke900Controller.Port = "COM6";
                Fluke900Controller.BaudRate = 9600;
                Fluke900Controller.Parity = RJCP.IO.Ports.Parity.None;
                Fluke900Controller.DataBits = 8;
                Fluke900Controller.StopBits = RJCP.IO.Ports.StopBits.One;

                _connected = Fluke900Controller.Connect();
                if (_connected) {
                    buttonConnectDisconnect.Text = "Disconnect";
                    Listen();
                }
            }
            else
            {
                _connected = false;
                Fluke900Controller.Disconnect();
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
                    ClientCommand command = Fluke900Controller.ReceiveCommand();
                    if (command != null)
                    {
                        switch (command.CommandCode)
                        {
                            case ClientCommands.Identify:
                                bytes.Add((byte)CommandCharacters.StartText);
                                bytes.AddRange(Encoding.ASCII.GetBytes("Fluke 900\rP\r6.00\rL3.00\rM0\rH1\rI0\rE\rS\rD"));
                                bytes.Add((byte)CommandCharacters.Acknowledge);
                                break;
                            case ClientCommands.ExitRemoteMode:
                                bytes.Add((byte)CommandCharacters.Acknowledge);
                                break;
                            case ClientCommands.ReadPinDefinition:
                                bytes.Add((byte)CommandCharacters.StartText);
                                bytes.AddRange(Encoding.ASCII.GetBytes("A\rI"));
                                bytes.Add((byte)CommandCharacters.Acknowledge);
                                break;
                            case ClientCommands.ReadResetDefinition:
                                bytes.Add((byte)CommandCharacters.StartText);
                                bytes.AddRange(Encoding.ASCII.GetBytes("P\rI\r-200\r500"));
                                bytes.Add((byte)CommandCharacters.EndText);
                                break;
                            case ClientCommands.SetRDDrive:
                                _defaultTestParameters.ReferenceDeviceDrive = command.Parameters[0] == "H" ? true : false;
                                bytes.Add((byte)CommandCharacters.Acknowledge);
                                break;
                            case ClientCommands.GetRDDrive:
                                bytes.Add((byte)CommandCharacters.StartText);
                                bytes.AddRange(Encoding.ASCII.GetBytes(_defaultTestParameters.ReferenceDeviceDrive ? "H":"L"));
                                bytes.Add((byte)CommandCharacters.Acknowledge);
                                break;
                            case ClientCommands.SetRDTest:
                                _defaultTestParameters.ReferenceDeviceTest = command.Parameters[0] == "E" ? true : false;
                                bytes.Add((byte)CommandCharacters.Acknowledge);
                                break;
                            case ClientCommands.GetRDTest:
                                bytes.Add((byte)CommandCharacters.StartText);
                                bytes.AddRange(Encoding.ASCII.GetBytes(_defaultTestParameters.ReferenceDeviceTest ? "E" : "D"));
                                bytes.Add((byte)CommandCharacters.Acknowledge);
                                break;
                            case ClientCommands.SetClipCheck:
                                _defaultTestParameters.ClipCheck = command.Parameters[0] == "E" ? true : false;
                                bytes.Add((byte)CommandCharacters.Acknowledge);
                                break;
                            case ClientCommands.GetClipCheck:
                                bytes.Add((byte)CommandCharacters.StartText);
                                bytes.AddRange(Encoding.ASCII.GetBytes(_defaultTestParameters.ClipCheck ? "E" : "D"));
                                bytes.Add((byte)CommandCharacters.Acknowledge);
                                break;
                            case ClientCommands.SetSimulation:
                                string simulationCode = command.Parameters[0];
                                switch (simulationCode)
                                {
                                    case "N":
                                        _defaultTestParameters.Simulation = SimulationDefinition.NotInstalled;
                                        break;
                                    case "E":
                                        _defaultTestParameters.Simulation = SimulationDefinition.Enabled;
                                        break;
                                    default:
                                        _defaultTestParameters.Simulation = SimulationDefinition.Disabled;
                                        break;
                                }
                                bytes.Add((byte)CommandCharacters.Acknowledge);
                                break;
                            case ClientCommands.GetSimulation:
                                bytes.Add((byte)CommandCharacters.StartText);
                                if (_simulationInstalled)
                                {
                                    bytes.AddRange(Encoding.ASCII.GetBytes(_defaultTestParameters.Simulation == SimulationDefinition.Enabled ? "E" : "D"));
                                }
                                else
                                {
                                    bytes.AddRange(Encoding.ASCII.GetBytes("N"));
                                }
                                bytes.Add((byte)CommandCharacters.Acknowledge);
                                break;
                            case ClientCommands.SetSyncTime:
                                if (String.IsNullOrEmpty(command.Parameters[0]))
                                {
                                    _defaultTestParameters.SyncTime = null;
                                }
                                else
                                {
                                    _defaultTestParameters.SyncTime = int.Parse(command.Parameters[0]);
                                }
                                bytes.Add((byte)CommandCharacters.Acknowledge);
                                break;
                            case ClientCommands.GetSyncTime:
                                bytes.Add((byte)CommandCharacters.StartText);
                                if (_defaultTestParameters.SyncTime.HasValue)
                                {
                                    bytes.AddRange(Encoding.ASCII.GetBytes(_defaultTestParameters.SyncTime.Value.ToString()));
                                }
                                bytes.Add((byte)CommandCharacters.Acknowledge);
                                break;
                            case ClientCommands.SetTriggerConfiguration:
                                bytes.Add((byte)CommandCharacters.StartText);
                                //bytes.AddRange(Encoding.ASCII.GetBytes(_defaultTestParameters.TriggerConfiguration));
                                _defaultTestParameters.TriggerConfiguration = command.Parameters[0];
                                bytes.Add((byte)CommandCharacters.Acknowledge);
                                break;
                            case ClientCommands.SetTriggerEnable:
                                bytes.Add((byte)CommandCharacters.StartText);
                                _defaultTestParameters.TriggerEnabled = command.Parameters[0] == "E" ? true : false;
                                bytes.Add((byte)CommandCharacters.Acknowledge);
                                break;
                            case ClientCommands.GetTriggerEnable:
                                bytes.Add((byte)CommandCharacters.StartText);
                                bytes.AddRange(Encoding.ASCII.GetBytes(_defaultTestParameters.TriggerEnabled ? "E" : "D"));
                                bytes.Add((byte)CommandCharacters.Acknowledge);
                                break;
                            case ClientCommands.SetTriggerGateWord:
                                string wordValues = command.Parameters[1];
                                string extValue = command.Parameters[2];
                                if (_defaultTestParameters.PinDefinitions.Count < wordValues.Length)
                                {
                                    _defaultTestParameters.PinDefinitions.Clear();
                                    for(int i =0; i < wordValues.Length; i++)
                                    {
                                        _defaultTestParameters.PinDefinitions.Add(new TestPinDefinition());
                                    }
                                }
                                switch (command.Parameters[0])
                                {
                                    case "0":
                                        //gate
                                        for(int i = 0; i < wordValues.Length; i++)
                                        {
                                            switch (wordValues[i])
                                            {
                                                case 'X':
                                                    _defaultTestParameters.PinDefinitions[i].Gate = TriggerGateDefinition.DontCare;
                                                    break;
                                                case '1':
                                                    _defaultTestParameters.PinDefinitions[i].Gate = TriggerGateDefinition.True;
                                                    break;
                                                default:
                                                    _defaultTestParameters.PinDefinitions[i].Gate = TriggerGateDefinition.False;
                                                    break;
                                            }
                                        }
                                        switch (extValue)
                                        {
                                            case "X":
                                                _defaultTestParameters.GateExt = TriggerGateDefinition.DontCare;
                                                break;
                                            case "1":
                                                _defaultTestParameters.GateExt = TriggerGateDefinition.True;
                                                break;
                                            default:
                                                _defaultTestParameters.GateExt = TriggerGateDefinition.False;
                                                break;
                                        }
                                        break;
                                    case "1":
                                        //trigger 1
                                        for (int i = 0; i < wordValues.Length; i++)
                                        {
                                            switch (wordValues[i])
                                            {
                                                case 'X':
                                                    _defaultTestParameters.PinDefinitions[i].TriggerWord1 = TriggerGateDefinition.DontCare;
                                                    break;
                                                case '1':
                                                    _defaultTestParameters.PinDefinitions[i].TriggerWord1 = TriggerGateDefinition.True;
                                                    break;
                                                default:
                                                    _defaultTestParameters.PinDefinitions[i].TriggerWord1 = TriggerGateDefinition.False;
                                                    break;
                                            }
                                        }
                                        switch (extValue)
                                        {
                                            case "X":
                                                _defaultTestParameters.TriggerExt1 = TriggerGateDefinition.DontCare;
                                                break;
                                            case "1":
                                                _defaultTestParameters.TriggerExt1 = TriggerGateDefinition.True;
                                                break;
                                            default:
                                                _defaultTestParameters.TriggerExt1 = TriggerGateDefinition.False;
                                                break;
                                        }
                                        break;
                                    case "2":
                                        //trigger 2
                                        for (int i = 0; i < wordValues.Length; i++)
                                        {
                                            switch (wordValues[i])
                                            {
                                                case 'X':
                                                    _defaultTestParameters.PinDefinitions[i].TriggerWord2 = TriggerGateDefinition.DontCare;
                                                    break;
                                                case '1':
                                                    _defaultTestParameters.PinDefinitions[i].TriggerWord2 = TriggerGateDefinition.True;
                                                    break;
                                                default:
                                                    _defaultTestParameters.PinDefinitions[i].TriggerWord2 = TriggerGateDefinition.False;
                                                    break;
                                            }
                                        }
                                        switch (extValue)
                                        {
                                            case "X":
                                                _defaultTestParameters.TriggerExt2 = TriggerGateDefinition.DontCare;
                                                break;
                                            case "1":
                                                _defaultTestParameters.TriggerExt2 = TriggerGateDefinition.True;
                                                break;
                                            default:
                                                _defaultTestParameters.TriggerExt2 = TriggerGateDefinition.False;
                                                break;
                                        }
                                        break;
                                }
                                bytes.Add((byte)CommandCharacters.Acknowledge);
                                break;
                            case ClientCommands.GetTriggerGateWord:
                                StringBuilder triggerGateWordValues = new StringBuilder();
                                string triggerGateExtValue = "";
                                switch (command.Parameters[0])
                                {
                                    case "0":
                                        //gate
                                        for (int i = 0; i < _defaultTestParameters.PinDefinitions.Count; i++)
                                        {
                                            switch (_defaultTestParameters.PinDefinitions[i].Gate)
                                            {
                                                case TriggerGateDefinition.DontCare:
                                                    triggerGateWordValues.Append("X");
                                                    break;
                                                case TriggerGateDefinition.True:
                                                    triggerGateWordValues.Append("1");
                                                    break;
                                                default:
                                                    triggerGateWordValues.Append("0");
                                                    break;
                                            }
                                        }
                                        switch (_defaultTestParameters.GateExt)
                                        {
                                            case TriggerGateDefinition.DontCare:
                                                triggerGateExtValue = "X";
                                                break;
                                            case TriggerGateDefinition.True:
                                                triggerGateExtValue = "1";
                                                break;
                                            default:
                                                triggerGateExtValue = "0";
                                                break;
                                        }
                                        break;
                                    case "1":
                                        //trigger 1
                                        for (int i = 0; i < _defaultTestParameters.PinDefinitions.Count; i++)
                                        {
                                            switch (_defaultTestParameters.PinDefinitions[i].TriggerWord1)
                                            {
                                                case TriggerGateDefinition.DontCare:
                                                    triggerGateWordValues.Append("X");
                                                    break;
                                                case TriggerGateDefinition.True:
                                                    triggerGateWordValues.Append("1");
                                                    break;
                                                default:
                                                    triggerGateWordValues.Append("0");
                                                    break;
                                            }
                                        }
                                        switch (_defaultTestParameters.TriggerExt1)
                                        {
                                            case TriggerGateDefinition.DontCare:
                                                triggerGateExtValue = "X";
                                                break;
                                            case TriggerGateDefinition.True:
                                                triggerGateExtValue = "1";
                                                break;
                                            default:
                                                triggerGateExtValue = "0";
                                                break;
                                        }
                                        break;
                                    case "2":
                                        //trigger 2
                                        for (int i = 0; i < _defaultTestParameters.PinDefinitions.Count; i++)
                                        {
                                            switch (_defaultTestParameters.PinDefinitions[i].TriggerWord2)
                                            {
                                                case TriggerGateDefinition.DontCare:
                                                    triggerGateWordValues.Append("X");
                                                    break;
                                                case TriggerGateDefinition.True:
                                                    triggerGateWordValues.Append("1");
                                                    break;
                                                default:
                                                    triggerGateWordValues.Append("0");
                                                    break;
                                            }
                                        }
                                        switch (_defaultTestParameters.TriggerExt2)
                                        {
                                            case TriggerGateDefinition.DontCare:
                                                triggerGateExtValue = "X";
                                                break;
                                            case TriggerGateDefinition.True:
                                                triggerGateExtValue = "1";
                                                break;
                                            default:
                                                triggerGateExtValue = "0";
                                                break;
                                        }
                                        break;
                                }
                                bytes.Add((byte)CommandCharacters.StartText);
                                bytes.AddRange(Encoding.ASCII.GetBytes(triggerGateWordValues.ToString()));
                                bytes.AddRange(Encoding.ASCII.GetBytes("\r"));
                                bytes.AddRange(Encoding.ASCII.GetBytes(triggerGateExtValue));
                                bytes.Add((byte)CommandCharacters.Acknowledge);
                                break;
                            default:
                                bytes.Add((byte)CommandCharacters.Acknowledge);
                                break;
                        }
                    }
                    if (bytes.Count > 0)
                    {
                        Fluke900Controller.SendBinary(bytes.ToArray());
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
