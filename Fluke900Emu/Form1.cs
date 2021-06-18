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
        private bool _ramShadowInstalled = true;
        private int _peFaultMask = 20;
        private int _peStepFM = 10;
        private int _peThreshold = 1400;
        private int _peStepT = 200;
        private int _peFaultMaskTestCount = 6;
        private int _peThresholdTestCount = 6;
        private List<int> _peFaultMaskCurve = new List<int>() { 4, 3, 2, 2, 3, 4 };

        //private TestParameters _testParameters = new TestParameters();
        private ProjectLocation _defaultTestParameters = new ProjectLocation();

        public Form1()
        {
            InitializeComponent();
            _defaultTestParameters.Simulation = SimulationShadowDefinition.Enabled;
            _defaultTestParameters.RAMShadow = SimulationShadowDefinition.Enabled;
        }

        private void buttonConnectDisconnect_Click(object sender, EventArgs e)
        {
            if (!_connected)
            {
                //connect
                Fluke900Emulator.Port = "COM6";
                Fluke900Emulator.BaudRate = 19200;
                Fluke900Emulator.Parity = RJCP.IO.Ports.Parity.Even;
                Fluke900Emulator.DataBits = 7;
                Fluke900Emulator.StopBits = RJCP.IO.Ports.StopBits.One;

                _connected = Fluke900Emulator.Connect();
                if (_connected) {
                    buttonConnectDisconnect.Text = "Disconnect";
                    Listen();
                }
            }
            else
            {
                _connected = false;
                Fluke900Emulator.Disconnect();
                buttonConnectDisconnect.Text = "Connect";
            }
        }

        private void Listen()
        {
            ClientCommand command = null;
            List<string> textOutput = new List<string>();
            try
            {
                string lastTextString = "";
                int lastCounter = 0;
                while (_connected)
                {
                    List<byte> bytes = new List<byte>();
                    StringBuilder sb = new StringBuilder();
                    string currentText = "";
                    try
                    {
                        command = Fluke900Emulator.ReceiveCommand();
                        if (command != null)
                        {
                            System.Threading.Thread.Sleep(500);
                            switch (command.CommandCode)
                            {
                                case ClientCommands.Identify:
                                    bytes.Add((byte)CommandCharacters.StartText);
                                    bytes.AddRange(Encoding.ASCII.GetBytes("Fluke 900\rP\r6.00\rL3.00\rM0\rH3ESD\rI0"));
                                    bytes.Add((byte)CommandCharacters.Acknowledge);
                                    break;
                                case ClientCommands.ExitRemoteMode:
                                    bytes.Add((byte)CommandCharacters.Acknowledge);
                                    break;
                                case ClientCommands.SetLocation:
                                    _defaultTestParameters.Name = command.Parameters[0];
                                    bytes.Add((byte)CommandCharacters.Substitute);
                                    bytes.Add((byte)CommandCharacters.Acknowledge);
                                    break;
                                case ClientCommands.SetDevice:
                                    _defaultTestParameters.DeviceName = command.Parameters[0];
                                    bytes.Add((byte)CommandCharacters.Substitute);
                                    bytes.Add((byte)CommandCharacters.Acknowledge);
                                    break;
                                case ClientCommands.GetDevice:
                                    bytes.Add((byte)CommandCharacters.StartText);
                                    bytes.AddRange(Encoding.ASCII.GetBytes(_defaultTestParameters.DeviceName));
                                    bytes.Add((byte)CommandCharacters.Acknowledge);
                                    break;
                                case ClientCommands.SetSizePower:
                                    _defaultTestParameters.Pins = int.Parse(command.Parameters[0]);
                                    int pinIterator = 2;    //we start looking at Vcc pins at parameter 3
                                    while (command.Parameters[pinIterator] != "G")
                                    {
                                        _defaultTestParameters.VccPins.Add(int.Parse(command.Parameters[pinIterator++]));
                                    }
                                    pinIterator++;
                                    while (pinIterator < command.Parameters.Count)
                                    {
                                        _defaultTestParameters.GndPins.Add(int.Parse(command.Parameters[pinIterator++]));
                                    }
                                    bytes.Add((byte)CommandCharacters.Acknowledge);
                                    break;
                                case ClientCommands.GetSizePower:
                                    bytes.Add((byte)CommandCharacters.StartText);
                                    bytes.AddRange(Encoding.ASCII.GetBytes(_defaultTestParameters.Pins.ToString()));
                                    bytes.AddRange(Encoding.ASCII.GetBytes("V"));
                                    foreach(int v in _defaultTestParameters.VccPins)
                                    {
                                        bytes.AddRange(Encoding.ASCII.GetBytes(v.ToString()));
                                    }
                                    bytes.AddRange(Encoding.ASCII.GetBytes("G"));
                                    foreach (int g in _defaultTestParameters.GndPins)
                                    {
                                        bytes.AddRange(Encoding.ASCII.GetBytes(g.ToString()));
                                    }
                                    bytes.Add((byte)CommandCharacters.Acknowledge);
                                    break;
                                case ClientCommands.ReadPinDefinition:
                                    bytes.Add((byte)CommandCharacters.StartText);
                                    bytes.AddRange(Encoding.ASCII.GetBytes("A\rI"));
                                    bytes.Add((byte)CommandCharacters.Acknowledge);
                                    break;
                                case ClientCommands.SetResetDefinition:
                                    //bytes.AddRange(Encoding.ASCII.GetBytes(_defaultTestParameters.Reset.ToString()));
                                    _defaultTestParameters.Reset = null;
                                    if (command.Parameters.Count > 0 && !String.IsNullOrEmpty(command.Parameters[0]))
                                    {
                                        _defaultTestParameters.Reset = new ResetDefinition(command.Parameters);
                                    }
                                    bytes.Add((byte)CommandCharacters.Acknowledge);
                                    break;
                                case ClientCommands.GetResetDefinition:
                                    bytes.Add((byte)CommandCharacters.StartText);
                                    bytes.AddRange(Encoding.ASCII.GetBytes(_defaultTestParameters.Reset.ToString()));
                                    bytes.Add((byte)CommandCharacters.Acknowledge);
                                    break;
                                case ClientCommands.SetRDDrive:
                                    _defaultTestParameters.ReferenceDeviceDrive = command.Parameters[0] == "H" ? true : false;
                                    bytes.Add((byte)CommandCharacters.Acknowledge);
                                    break;
                                case ClientCommands.GetRDDrive:
                                    bytes.Add((byte)CommandCharacters.StartText);
                                    bytes.AddRange(Encoding.ASCII.GetBytes(_defaultTestParameters.ReferenceDeviceDrive ? "H" : "L"));
                                    bytes.Add((byte)CommandCharacters.Acknowledge);
                                    break;
                                case ClientCommands.SetFmask:
                                    if (command.Parameters.Count > 0 && !String.IsNullOrEmpty(command.Parameters[0]))
                                    {
                                        _defaultTestParameters.FaultMask = int.Parse(command.Parameters[0]);
                                    }
                                    bytes.Add((byte)CommandCharacters.Acknowledge);
                                    break;
                                case ClientCommands.GetFmask:
                                    bytes.Add((byte)CommandCharacters.StartText);
                                    bytes.AddRange(Encoding.ASCII.GetBytes(_defaultTestParameters.FaultMask.ToString()));
                                    bytes.Add((byte)CommandCharacters.Acknowledge);
                                    break;
                                case ClientCommands.SetThreshold:
                                    _defaultTestParameters.Threshold = int.Parse(command.Parameters[0].Replace(".","")); 
                                    bytes.Add((byte)CommandCharacters.Acknowledge);
                                    break;
                                case ClientCommands.GetThreshold:
                                    bytes.Add((byte)CommandCharacters.StartText);
                                    bytes.AddRange(Encoding.ASCII.GetBytes(_defaultTestParameters.Threshold.ToString()));
                                    bytes.Add((byte)CommandCharacters.Acknowledge);
                                    break;
                                case ClientCommands.SetPinEnableDisable:


                                    bytes.Add((byte)CommandCharacters.Acknowledge);
                                    break;
                                case ClientCommands.SetTestTime:
                                    int testTime = -1;
                                    int.TryParse(command.Parameters[0], out testTime);
                                    _defaultTestParameters.TestTime = testTime;
                                    bytes.Add((byte)CommandCharacters.Acknowledge);
                                    break;
                                case ClientCommands.GetTestTime:
                                    bytes.Add((byte)CommandCharacters.StartText);
                                    if (_defaultTestParameters.TestTime >= 0)
                                    {
                                        bytes.AddRange(Encoding.ASCII.GetBytes(_defaultTestParameters.TestTime.ToString()));
                                    }
                                    else
                                    {
                                        bytes.AddRange(Encoding.ASCII.GetBytes("CONT"));
                                    }
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
                                case ClientCommands.SetCsum:
                                    _defaultTestParameters.Checksum = int.Parse(command.Parameters[0]);
                                    bytes.Add((byte)CommandCharacters.Acknowledge);
                                    break;
                                case ClientCommands.GetCsum:
                                    bytes.Add((byte)CommandCharacters.StartText);
                                    bytes.AddRange(Encoding.ASCII.GetBytes(_defaultTestParameters.Checksum.ToString()));
                                    bytes.Add((byte)CommandCharacters.Acknowledge);
                                    break;
                                case ClientCommands.GetUknown:
                                    bytes.Add((byte)CommandCharacters.StartText);
                                    bytes.AddRange(Encoding.ASCII.GetBytes(""));
                                    bytes.Add((byte)CommandCharacters.Acknowledge);
                                    break;
                                case ClientCommands.SetSimulation:
                                    string simulationCode = command.Parameters[0];
                                    switch (simulationCode)
                                    {
                                        case "N":
                                            _defaultTestParameters.Simulation = SimulationShadowDefinition.NotInstalled;
                                            break;
                                        case "E":
                                            _defaultTestParameters.Simulation = SimulationShadowDefinition.Enabled;
                                            break;
                                        default:
                                            _defaultTestParameters.Simulation = SimulationShadowDefinition.Disabled;
                                            break;
                                    }
                                    bytes.Add((byte)CommandCharacters.Acknowledge);
                                    break;
                                case ClientCommands.GetSimulation:
                                    bytes.Add((byte)CommandCharacters.StartText);
                                    if (_simulationInstalled)
                                    {
                                        bytes.AddRange(Encoding.ASCII.GetBytes(_defaultTestParameters.Simulation == SimulationShadowDefinition.Enabled ? "E" : "D"));
                                    }
                                    else
                                    {
                                        bytes.AddRange(Encoding.ASCII.GetBytes("N"));
                                    }
                                    bytes.Add((byte)CommandCharacters.Acknowledge);
                                    break;
                                case ClientCommands.PerformanceEnvelope:
                                    if (command.Parameters.Count > 1)
                                    {
                                        _peFaultMask = int.Parse(command.Parameters[0]);
                                        _peStepFM = int.Parse(command.Parameters[1]);
                                        _peThreshold = int.Parse(command.Parameters[2]);
                                        _peStepT = int.Parse(command.Parameters[3]);
                                    }
                                    bytes.Add((byte)CommandCharacters.StartText);
                                    bytes.AddRange(Encoding.ASCII.GetBytes(_peFaultMask.ToString()));
                                    bytes.AddRange(Encoding.ASCII.GetBytes(" "));
                                    bytes.AddRange(Encoding.ASCII.GetBytes(_peStepFM.ToString()));
                                    bytes.AddRange(Encoding.ASCII.GetBytes(" "));
                                    bytes.AddRange(Encoding.ASCII.GetBytes(_peFaultMaskTestCount.ToString()));
                                    bytes.AddRange(Encoding.ASCII.GetBytes(" "));
                                    bytes.AddRange(Encoding.ASCII.GetBytes(_peThreshold.ToString()));
                                    bytes.AddRange(Encoding.ASCII.GetBytes(" "));
                                    bytes.AddRange(Encoding.ASCII.GetBytes(_peStepT.ToString()));
                                    bytes.AddRange(Encoding.ASCII.GetBytes(" "));
                                    bytes.AddRange(Encoding.ASCII.GetBytes(_peThresholdTestCount.ToString()));
                                    bytes.Add((byte)CommandCharacters.Substitute);
                                    for (int i = 0; i < _peThresholdTestCount; i++)
                                    {
                                        for (int j = 0; j < _peFaultMaskTestCount; j++)
                                        {
                                            if (j <= _peFaultMaskCurve[i])
                                            {
                                                if (i != 0 || j != 0)
                                                {
                                                    bytes.AddRange(Encoding.ASCII.GetBytes("\r"));
                                                }
                                                bytes.AddRange(Encoding.ASCII.GetBytes((_peFaultMask + (j * _peStepFM)).ToString()));
                                                bytes.AddRange(Encoding.ASCII.GetBytes(" "));
                                                bytes.AddRange(Encoding.ASCII.GetBytes((_peThreshold + (_peStepT * i)).ToString()));
                                                bytes.AddRange(Encoding.ASCII.GetBytes(" "));
                                                bytes.AddRange(Encoding.ASCII.GetBytes("W"));
                                                bytes.Add((byte)CommandCharacters.Substitute);
                                                bytes.AddRange(Encoding.ASCII.GetBytes("T"));
                                                bytes.Add((byte)CommandCharacters.Substitute);
                                                if (j < _peFaultMaskCurve[i])
                                                {
                                                    bytes.AddRange(Encoding.ASCII.GetBytes("F"));
                                                }
                                                else
                                                {
                                                    bytes.AddRange(Encoding.ASCII.GetBytes("P"));
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    bytes.Add((byte)CommandCharacters.Substitute);
                                    bytes.AddRange(Encoding.ASCII.GetBytes("RESULT:" + (_peFaultMask + ((_peFaultMaskCurve[2] + 1) * _peStepFM)).ToString() + " " + (_peThreshold + (_peStepT * 2)).ToString() + "\r"));
                                    //optional HINT can be from Fluke900, but we wont
                                    //bytes.AddRange(Encoding.ASCII.GetBytes("HINTS:NOTE:HA HA HA HA!"));
                                    bytes.Add((byte)CommandCharacters.Acknowledge);
                                    break;
                                case ClientCommands.SetRAMShadow:
                                    string ramShadowCode = command.Parameters[0];
                                    switch (ramShadowCode)
                                    {
                                        case "N":
                                            _defaultTestParameters.RAMShadow = SimulationShadowDefinition.NotInstalled;
                                            break;
                                        case "E":
                                            _defaultTestParameters.RAMShadow = SimulationShadowDefinition.Enabled;
                                            break;
                                        default:
                                            _defaultTestParameters.RAMShadow = SimulationShadowDefinition.Disabled;
                                            break;
                                    }
                                    bytes.Add((byte)CommandCharacters.Acknowledge);
                                    break;
                                case ClientCommands.GetRAMShadow:
                                    bytes.Add((byte)CommandCharacters.StartText);
                                    if (_ramShadowInstalled)
                                    {
                                        bytes.AddRange(Encoding.ASCII.GetBytes(_defaultTestParameters.RAMShadow == SimulationShadowDefinition.Enabled ? "E" : "D"));
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
                                case ClientCommands.GetDirectoryCartridge:
                                    bytes.Add((byte)CommandCharacters.StartText);
                                    bytes.AddRange(Encoding.ASCII.GetBytes("TEST1.SEQ:CART 2857\rTEST1.LOC:CART 5326\r8183 Bytes used, 24561  Left\r"));
                                    bytes.Add((byte)CommandCharacters.Acknowledge);
                                    break;
                                case ClientCommands.GetDirectorySystem:
                                    bytes.Add((byte)CommandCharacters.StartText);
                                    bytes.AddRange(Encoding.ASCII.GetBytes("TEST1.SEQ:SYS 2857\rTEST1.LOC:SYS 5326\r8183 Bytes used, 24561  Left\r"));
                                    bytes.Add((byte)CommandCharacters.Acknowledge);
                                    break;
                                case ClientCommands.Stimulus:
                                    //bytes.Add((byte)CommandCharacters.StartText);
                                    ////bytes.AddRange(Encoding.ASCII.GetBytes(_defaultTestParameters.TriggerConfiguration));
                                    //_defaultTestParameters.TriggerConfiguration = command.Parameters[0];
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
                                        for (int i = 0; i < wordValues.Length; i++)
                                        {
                                            _defaultTestParameters.PinDefinitions.Add(new TestPinDefinition());
                                        }
                                    }
                                    switch (command.Parameters[0])
                                    {
                                        case "0":
                                            //gate
                                            for (int i = 0; i < wordValues.Length; i++)
                                            {
                                                switch (wordValues[i])
                                                {
                                                    case 'X':
                                                        _defaultTestParameters.PinDefinitions[i].GatePinDefinition = GatePinDefinition.DontCare;
                                                        break;
                                                    case '1':
                                                        _defaultTestParameters.PinDefinitions[i].GatePinDefinition = GatePinDefinition.True;
                                                        break;
                                                    default:
                                                        _defaultTestParameters.PinDefinitions[i].GatePinDefinition = GatePinDefinition.False;
                                                        break;
                                                }
                                            }
                                            switch (extValue)
                                            {
                                                case "X":
                                                    _defaultTestParameters.GateExt = GateExtDefinition.DontCare;
                                                    break;
                                                case "1":
                                                    _defaultTestParameters.GateExt = GateExtDefinition.True;
                                                    break;
                                                default:
                                                    _defaultTestParameters.GateExt = GateExtDefinition.False;
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
                                                        _defaultTestParameters.PinDefinitions[i].TriggerWord1 = TriggerWord1Definition.DontCare;
                                                        break;
                                                    case '1':
                                                        _defaultTestParameters.PinDefinitions[i].TriggerWord1 = TriggerWord1Definition.True;
                                                        break;
                                                    default:
                                                        _defaultTestParameters.PinDefinitions[i].TriggerWord1 = TriggerWord1Definition.False;
                                                        break;
                                                }
                                            }
                                            switch (extValue)
                                            {
                                                case "X":
                                                    _defaultTestParameters.TriggerExt1 = TriggerExt1Definition.DontCare;
                                                    break;
                                                case "1":
                                                    _defaultTestParameters.TriggerExt1 = TriggerExt1Definition.True;
                                                    break;
                                                default:
                                                    _defaultTestParameters.TriggerExt1 = TriggerExt1Definition.False;
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
                                                        _defaultTestParameters.PinDefinitions[i].TriggerWord2 = TriggerWord2Definition.DontCare;
                                                        break;
                                                    case '1':
                                                        _defaultTestParameters.PinDefinitions[i].TriggerWord2 = TriggerWord2Definition.True;
                                                        break;
                                                    default:
                                                        _defaultTestParameters.PinDefinitions[i].TriggerWord2 = TriggerWord2Definition.False;
                                                        break;
                                                }
                                            }
                                            switch (extValue)
                                            {
                                                case "X":
                                                    _defaultTestParameters.TriggerExt2 = TriggerExt2Definition.DontCare;
                                                    break;
                                                case "1":
                                                    _defaultTestParameters.TriggerExt2 = TriggerExt2Definition.True;
                                                    break;
                                                default:
                                                    _defaultTestParameters.TriggerExt2 = TriggerExt2Definition.False;
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
                                                switch (_defaultTestParameters.PinDefinitions[i].GatePinDefinition)
                                                {
                                                    case GatePinDefinition.DontCare:
                                                        triggerGateWordValues.Append("X");
                                                        break;
                                                    case GatePinDefinition.True:
                                                        triggerGateWordValues.Append("1");
                                                        break;
                                                    default:
                                                        triggerGateWordValues.Append("0");
                                                        break;
                                                }
                                            }
                                            switch (_defaultTestParameters.GateExt)
                                            {
                                                case GateExtDefinition.DontCare:
                                                    triggerGateExtValue = "X";
                                                    break;
                                                case GateExtDefinition.True:
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
                                                    case TriggerWord1Definition.DontCare:
                                                        triggerGateWordValues.Append("X");
                                                        break;
                                                    case TriggerWord1Definition.True:
                                                        triggerGateWordValues.Append("1");
                                                        break;
                                                    default:
                                                        triggerGateWordValues.Append("0");
                                                        break;
                                                }
                                            }
                                            switch (_defaultTestParameters.TriggerExt1)
                                            {
                                                case TriggerExt1Definition.DontCare:
                                                    triggerGateExtValue = "X";
                                                    break;
                                                case TriggerExt1Definition.True:
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
                                                    case TriggerWord2Definition.DontCare:
                                                        triggerGateWordValues.Append("X");
                                                        break;
                                                    case TriggerWord2Definition.True:
                                                        triggerGateWordValues.Append("1");
                                                        break;
                                                    default:
                                                        triggerGateWordValues.Append("0");
                                                        break;
                                                }
                                            }
                                            switch (_defaultTestParameters.TriggerExt2)
                                            {
                                                case TriggerExt2Definition.DontCare:
                                                    triggerGateExtValue = "X";
                                                    break;
                                                case TriggerExt2Definition.True:
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
                                case ClientCommands.SetGateEnable:
                                    _defaultTestParameters.GateEnabled = command.Parameters[0] == "E";
                                    bytes.Add((byte)CommandCharacters.Acknowledge);
                                    break;
                                case ClientCommands.SetGateConfiguration:
                                    _defaultTestParameters.Gate = new GateDefinition();
                                    _defaultTestParameters.Gate.Polarity = command.Parameters[1] == "T" ? true : false;
                                    _defaultTestParameters.Gate.Delay = UnitTime.Parse(command.Parameters[2]);
                                    //int gateDelayValue = 0;
                                    //if (int.TryParse(command.Parameters[2], out gateDelayValue))
                                    //{
                                    //    _defaultTestParameters.Gate.Delay = gateDelayValue;
                                    //}
                                    _defaultTestParameters.Gate.Duration = UnitTime.Parse(command.Parameters[3]);
                                    //if (command.Parameters[3] == "C")
                                    //{
                                    //    _defaultTestParameters.Gate.Duration = -1;
                                    //}
                                    //else
                                    //{
                                    //    int gateDurationValue = 0;
                                    //    if (int.TryParse(command.Parameters[3], out gateDurationValue))
                                    //    {
                                    //        _defaultTestParameters.Gate.Duration = gateDurationValue;
                                    //    }
                                    //}
                                    bytes.Add((byte)CommandCharacters.Acknowledge);
                                    break;
                                case ClientCommands.GetGateConfiguration:
                                    bytes.Add((byte)CommandCharacters.StartText);
                                    bytes.AddRange(Encoding.ASCII.GetBytes(_defaultTestParameters.GateEnabled ? "E\r" : "D\r"));
                                    bytes.AddRange(Encoding.ASCII.GetBytes("0\r"));
                                    bytes.AddRange(Encoding.ASCII.GetBytes(_defaultTestParameters.Gate.Polarity ? "T\r" : "I\r"));
                                    bytes.AddRange(Encoding.ASCII.GetBytes(_defaultTestParameters.Gate.Delay.ToString() + "\r"));
                                    if (_defaultTestParameters.Gate.Duration == null)
                                    {
                                        bytes.AddRange(Encoding.ASCII.GetBytes("CONT"));
                                    }
                                    else
                                    {
                                        bytes.AddRange(Encoding.ASCII.GetBytes(_defaultTestParameters.Gate.Duration.ToString()));
                                    }
                                    bytes.Add((byte)CommandCharacters.Acknowledge);
                                    break;
                                case ClientCommands.ResetAllParameters:
                                    bytes.Add((byte)CommandCharacters.Acknowledge);
                                    break;
                                default:
                                    bytes.Add((byte)CommandCharacters.Acknowledge);
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //internal catch for failed commands..
                        StringBuilder eb = new StringBuilder();
                        if (command != null)
                        {
                            eb.Append(command.CommandString);
                            eb.Append(":");
                            eb.Append(ex.Message);
                        }
                        else
                        {
                            eb.Append(ex.Message);
                        }
                        //clear bytes so message routine below does nothing
                        bytes.Clear();
                        currentText = "";
                        lastCounter = 0;
                        textOutput.Insert(0, eb.ToString());
                        textBoxCommands.Text = String.Join("\r\n", textOutput.ToArray());
                    }
                    if (bytes.Count > 0)
                    {
                        Fluke900Emulator.SendBinary(bytes.ToArray());
                        //update UI
                        if (command.CommandCode == ClientCommands.Unknown)
                        {
                            //special verbose
                            currentText = command.CommandCode + "(" + command.CommandString + ") " + String.Join(",", command.Parameters.ToArray());
                        }
                        else
                        {
                            currentText = command.CommandCode + "(" + command.CommandString + ") " + String.Join(",", command.Parameters.ToArray()); ;
                        }
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
