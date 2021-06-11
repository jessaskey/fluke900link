using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fluke900;
using Fluke900.Helpers;
using Fluke900Link.Containers;
using Fluke900Link.Helpers;

namespace Fluke900Link.Dialogs
{
    public partial class LibraryParserDialog : Form
    {
        private LibraryFile _currentLibraryFile = null;
        private List<DeviceLibrary> _deviceLibraries = new List<DeviceLibrary>();

        public LibraryParserDialog()
        {
            InitializeComponent();
        }

        public async Task Initialize()
        {
            if (await LibraryHelper.LoadReferenceLibrary())
            {
                foreach (string device in LibraryHelper.GetUniqueDevices())
                {
                    listViewDevices.Items.Add(device);
                }
            }
            string libFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Libraries", "F900LIB_2_06.LI!");
            if (File.Exists(libFile))
            {
                textBoxLibraryFile.Text = libFile;
            }
        }

private void buttonBrowseLibraryBinaryFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog od = new OpenFileDialog();
            if (Directory.Exists(Properties.Settings.Default.DefaultFilesDirectory))
            { 
                od.InitialDirectory = Properties.Settings.Default.DefaultFilesDirectory;
            }
            od.Filter = "";
            od.Multiselect = false;
            DialogResult dr = od.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                textBoxLibraryFile.Text = od.FileName;
            }
        }

        private void buttonLoadBinaryFile_Click(object sender, EventArgs e)
        {
            _currentLibraryFile = null;
            listViewDevices.Items.Clear();

            if (File.Exists(textBoxLibraryFile.Text))
            {
                _currentLibraryFile = new LibraryFile(textBoxLibraryFile.Text);
                if (_currentLibraryFile.LoadLibraryFile())
                {
                    LogText("Library Loaded: " + _currentLibraryFile.FileName);
                    LogText(" - Found " + _currentLibraryFile.DeviceLibraries.Count.ToString() + " items.");

                    if (_currentLibraryFile != null)
                    {
                        List<string> uniqueNames = GetUniqueDeviceNames();

                        foreach (string un in uniqueNames.OrderBy(s => s))
                        {
                            listViewDevices.Items.Add(un);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("File does not exist or cannot be opened: '" + textBoxLibraryFile.Text + "'", "File Open Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private List<string> GetUniqueDeviceNames()
        {
            List<string> uniqueNames = new List<string>();

            foreach (DeviceLibrary dl in _currentLibraryFile.DeviceLibraries)
            {
                foreach (DeviceLibraryItem item in dl.Items.Where(i => i.TypeDefinition == DeviceLibraryConfigurationItem.NAME))
                {
                    uniqueNames.Add(item.Data);
                }
            }
            return uniqueNames.OrderBy(s => s).ToList();
        }

        private void buttonSaveBinaryFileResults_Click(object sender, EventArgs e)
        {
            //serialize it..
            SaveFileDialog sd = new SaveFileDialog();
            sd.InitialDirectory = Properties.Settings.Default.DefaultFilesDirectory;
            DialogResult dr = sd.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(_deviceLibraries.GetType());
                System.IO.FileStream file = System.IO.File.Create(sd.FileName);
                x.Serialize(file, _deviceLibraries);
                file.Close();
            }
        }

        private void LogText(string text)
        {
            listViewBinaryFileLoadResults.Items.Add(new ListViewItem(text));
        }
        
        private void buttonSaveSimPointers_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> allDeviceSims = new Dictionary<string, string>();
            List<SimBinaryData> simLibrary = new List<SimBinaryData>();
           
            foreach (DeviceLibrary lib in _deviceLibraries)
            {
                DeviceLibraryItem simItem = lib.Items.Where(i => i.TypeDefinition == DeviceLibraryConfigurationItem.RDTEST).FirstOrDefault();
                if (simItem != null)
                {
                    if (simItem.DataBytes != null)
                    {
                        if (simItem.DataBytes.Length != 6)
                        {
                            string names = String.Join(",", lib.Items.Where(i => i.TypeDefinition == DeviceLibraryConfigurationItem.NAME).Select(i => i.Data).ToArray());
                            LogText(names + " has <> 2 Simulation Blocks");
                        }
                        foreach (string name in lib.Items.Where(i => i.TypeDefinition == DeviceLibraryConfigurationItem.NAME).Select(i => i.Data))
                        {
                            string sim1LowByte = simItem.DataBytes[1].ToString("x2").ToUpper();
                            string sim1HighByte = simItem.DataBytes[2].ToString("x2").ToUpper();

                            string sim2LowByte = simItem.DataBytes[4].ToString("x2").ToUpper();
                            string sim2HighByte = simItem.DataBytes[5].ToString("x2").ToUpper();

                            int sim1Address = Convert.ToInt32((sim1HighByte + sim1LowByte), 16);
                            int sim2Address = Convert.ToInt32((sim2HighByte + sim2LowByte), 16);

                            //offset from start location
                            sim1Address += 0x302;
                            sim2Address += 0x302;

                            SimBinaryData simAddressData = new SimBinaryData();
                            simAddressData.SimBlock1 = sim1Address;
                            simAddressData.SimBlock2 = sim2Address;
                            simAddressData.Device = name;

                            simLibrary.Add(simAddressData);
                            allDeviceSims.Add(name, ByteArrayToString(simItem.DataBytes));
                        }
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            //foreach (KeyValuePair<string, string> kvp in allDeviceSims.OrderBy(s => s.Key))
            //{
            //    sb.AppendLine(kvp.Key + ": " + kvp.Value);
            //}

            foreach(SimBinaryData simData in simLibrary.OrderBy(s=>s.SimBlock1))
            {
                sb.AppendLine(simData.SimBlock1.ToString("X2") + ":" + simData.SimBlock2.ToString("X2") + " - " + simData.Device);
            }

            SaveFileDialog sd = new SaveFileDialog();
            sd.InitialDirectory = Properties.Settings.Default.DefaultFilesDirectory;
            DialogResult dr = sd.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                File.WriteAllText(sd.FileName, sb.ToString());
            }
        }

        public string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        private void buttonPrintUsedEnums_Click(object sender, EventArgs e)
        {
            List<int> enums = new List<int>();

            foreach (DeviceLibrary library in _deviceLibraries)
            {
                foreach (DeviceLibraryItem item in library.Items)
                {
                    if (!enums.Contains((int)item.TypeDefinition))
                    {
                        enums.Add((int)item.TypeDefinition);
                    }
                }
            }

            //serialize it..
            SaveFileDialog sd = new SaveFileDialog();
            sd.InitialDirectory = Properties.Settings.Default.DefaultFilesDirectory;
            DialogResult dr = sd.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                File.WriteAllText(sd.FileName, String.Join(Environment.NewLine, enums.OrderBy(i => i).Select(i => i.ToString("X2")).ToArray()));
            }
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    double d01 = 0.009d;
        //    double d1 = 1.00d;
        //    double d2 = 2.00d;
        //    double d3 = 3.00d;
        //    double d4 = 4.00d;
        //    double d5 = 10.00d;
        //    double d6 = 100.00d;
        //    double d7 = 1000.00d;
        //    double d8 = 10000.00d;
        //    double d9 = 100000.00d;
        //    double d10 = 1000000.00d;
        //    listViewBinaryFileLoadResults.Items.Add(new ListViewItem(d01.ToString() + ": " + Compiler.DoubleToHex(d01)));
        //    listViewBinaryFileLoadResults.Items.Add(new ListViewItem(d1.ToString() + ": " + Compiler.DoubleToHex(d1)));
        //    listViewBinaryFileLoadResults.Items.Add(new ListViewItem(d2.ToString() + ": " + Compiler.DoubleToHex(d2)));
        //    listViewBinaryFileLoadResults.Items.Add(new ListViewItem(d3.ToString() + ": " + Compiler.DoubleToHex(d3)));
        //    listViewBinaryFileLoadResults.Items.Add(new ListViewItem(d4.ToString() + ": " + Compiler.DoubleToHex(d4)));
        //    listViewBinaryFileLoadResults.Items.Add(new ListViewItem(d5.ToString() + ": " + Compiler.DoubleToHex(d5)));
        //    listViewBinaryFileLoadResults.Items.Add(new ListViewItem(d6.ToString() + ": " + Compiler.DoubleToHex(d6)));
        //    listViewBinaryFileLoadResults.Items.Add(new ListViewItem(d7.ToString() + ": " + Compiler.DoubleToHex(d7)));
        //    listViewBinaryFileLoadResults.Items.Add(new ListViewItem(d8.ToString() + ": " + Compiler.DoubleToHex(d8)));
        //    listViewBinaryFileLoadResults.Items.Add(new ListViewItem(d9.ToString() + ": " + Compiler.DoubleToHex(d9)));
        //    listViewBinaryFileLoadResults.Items.Add(new ListViewItem(d10.ToString() + ": " + Compiler.DoubleToHex(d10)));
        //}

        private void buttonSaveFileTextLib_Click(object sender, EventArgs e)
        {
           
            string indentTabs = new String('\t', 2);

            //this routine will attemt to save a file that is compilable back into binary... lets see

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("");
            sb.AppendLine("");
            sb.AppendLine("");

            foreach (DeviceLibrary library in _deviceLibraries)
            {
                StringBuilder lb = new StringBuilder();
                //name is always first
                foreach (string name in library.Items.Where(i => i.TypeDefinition == DeviceLibraryConfigurationItem.NAME).Select(i => i.Data))
                {
                    lb.AppendLine(indentTabs + "NAME\t" + name);
                }
                //then size
                byte size = library.Items.Where(i => i.TypeDefinition == DeviceLibraryConfigurationItem.SIZE).Select(i => i.DataBytes[0]).FirstOrDefault();
                lb.AppendLine(indentTabs + "SIZE\t" + size.ToString());
                //do the rest now...
                foreach (DeviceLibraryItem item in library.Items.Where(i => i.TypeDefinition != DeviceLibraryConfigurationItem.NAME 
                                                                            && i.TypeDefinition != DeviceLibraryConfigurationItem.SIZE
                                                                            && i.TypeDefinition != DeviceLibraryConfigurationItem.SIMULATION_DATA))
                {
     
                    int pinGrouping = 7;

                    switch (item.TypeDefinition)
                    {
                        case DeviceLibraryConfigurationItem.BINARY:
                            Handler_RDTEST(indentTabs, pinGrouping, item, library, lb);  
                            break;
                        case DeviceLibraryConfigurationItem.ACTIVITY:
                            Handler_ACTIVITY(indentTabs, pinGrouping, item, library, lb);
                            break;
                        default:
                            lb.AppendLine(indentTabs + Enum.GetName(typeof(DeviceLibraryConfigurationItem), item.TypeDefinition) + "\t" + item.Data);
                            break;
                    }
                    
                }

                //then finally any simulation data
                DeviceLibraryItem libSim = library.Items.Where(i => i.TypeDefinition == DeviceLibraryConfigurationItem.SIMULATION_DATA).FirstOrDefault();
                lb.AppendLine(indentTabs + "SIM_DATA\t" + size.ToString());

                sb.AppendLine(lb.ToString().TrimEnd('\r').TrimEnd('\n') + ":");
                sb.AppendLine("");
                sb.AppendLine("");
            }

            SaveFileDialog sd = new SaveFileDialog();
            sd.InitialDirectory = Properties.Settings.Default.DefaultFilesDirectory;
            DialogResult dr = sd.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                File.WriteAllText(sd.FileName, sb.ToString());
            }
        }

        private void Handler_RDTEST(string indentTabs, int pinGrouping, DeviceLibraryItem item, DeviceLibrary library, StringBuilder lb)
        {
            //RDTEST is a messy messy thing
            string rdtest = indentTabs + "RDTEST";
            if (item.RDTestData.Count > 0)
            {
                StringBuilder vectorData = new StringBuilder();
                vectorData.AppendLine(rdtest += " VECTORS");
                int currentSize = library.Items.Where(i => i.TypeDefinition == DeviceLibraryConfigurationItem.SIZE).Select(i => i.DataBytes[0]).FirstOrDefault();
                pinGrouping = GetPinGrouping(currentSize);
                AddVectorHeader(indentTabs, pinGrouping, currentSize, vectorData);

                int pinSetSize = (int)(Math.Ceiling(((double)currentSize) / 8d) * 3);
                if (item.VectorType == 0)
                {
                    pinSetSize = (int)(Math.Ceiling(((double)currentSize) / 8d) * 4);
                }

                foreach (List<byte[]> byteList in item.RDTestData.Select(b=>b))
                {
                    foreach(byte[] bytes in byteList)
                    { 
                        int pinSetIndex = 0;

                        while (pinSetIndex < (bytes.Length / pinSetSize))
                        {
                            byte[] pinVectors = bytes.Skip(pinSetIndex * pinSetSize).Take(pinSetSize).ToArray();
                            vectorData.AppendLine(GetVector(indentTabs, pinGrouping, currentSize, pinVectors));
                            pinSetIndex++;
                        }
                    }
                }
                lb.Append(vectorData.ToString());
                lb.AppendLine(indentTabs + "END_VECTORS");
            }
            else
            {
                lb.AppendLine(rdtest);
            }
        }

        private void Handler_ACTIVITY(string indentTabs, int pinGrouping, DeviceLibraryItem item, DeviceLibrary library, StringBuilder lb)
        {
            StringBuilder ab = new StringBuilder();
            //P1=A P2=A P3=A P4=A P5=A P6=A P7=A P8=A P9=A P10=A P13=4.773MHz 1% P14=L P28=A P27=L P26=A P22=A P21=A P20=A P19=A P18=A P16=A P15=A
            //    2-Byte Pairs
            //XX YY
            //Where 
            //    XX is the pin number
            //    YY is the state 
            //        00=X 
            //        01=L 
            //        02=H 
            //        03=A 
            //        05=FREQUENCY @ 1%
            //        06=FREQUENCY @ 2%
            //        07=FREQUENCY @ 3%
            //        08=FREQUENCY @ 4%
            //        09=FREQUENCY @ 5%
            //        0A=FREQUENCY @ 6%
            //        0B=FREQUENCY @ 7%
            //        0C=FREQUENCY @ 8%
            //        0D=FREQUENCY @ 9%
            //        0E=FREQUENCY @ 10%
            int index = 0;
            ab.Append(indentTabs + "ACTIVITY\t");

            while (index < item.DataBytes.Length)
            {
                byte pinNumber = item.DataBytes[index];
                byte pinState = item.DataBytes[index + 1];

                int increment = 2;
                switch (pinState)
                {
                    case 0:
                        ab.Append("P" + pinNumber.ToString() + "=X ");
                        break;
                    case 1:
                        ab.Append("P" + pinNumber.ToString() + "=L ");
                        break;
                    case 2:
                        ab.Append("P" + pinNumber.ToString() + "=H ");
                        break;
                    case 3:
                        ab.Append("P" + pinNumber.ToString() + "=A ");
                        break;
                    default:
                        switch (pinState)
                        {
                            case 5:
                                ab.Append("P" + pinNumber.ToString() + "=" + GetFloatString() + " 1% ");
                  
                                break;
                            case 6:
                                ab.Append("P" + pinNumber.ToString() + "=" + GetFloatString() + " 2% ");
                                break;
                            case 7:
                                ab.Append("P" + pinNumber.ToString() + "=" + GetFloatString() + " 3% ");
                                break;
                            case 8:
                                ab.Append("P" + pinNumber.ToString() + "=" + GetFloatString() + " 4% ");
                                break;
                            case 9:
                                ab.Append("P" + pinNumber.ToString() + "=" + GetFloatString() + " 5% ");
                                break;
                            case 10:
                                ab.Append("P" + pinNumber.ToString() + "=" + GetFloatString() + " 6% ");
                                break;
                            case 11:
                                ab.Append("P" + pinNumber.ToString() + "=" + GetFloatString() + " 7% ");
                                break;
                            case 12:
                                ab.Append("P" + pinNumber.ToString() + "=" + GetFloatString() + " 8% ");
                                break;
                            case 13:
                                ab.Append("P" + pinNumber.ToString() + "=" + GetFloatString() + " 9% ");
                                break;
                            case 14:
                                ab.Append("P" + pinNumber.ToString() + "=" + GetFloatString() + " 10% ");
                                break;

                        }
                        break;
                }
                index += increment;
            }
            lb.AppendLine(ab.ToString());
        }

        private string GetFloatString()
        {
            return "XXXHz";
        }

        private int GetPinGrouping(int size)
        {
            int pinGrouping = size / 2;
            if (size == 24)
            {
                pinGrouping = 8;
            }
            else if (size == 28)
            {
                pinGrouping = 7;
            }
            return pinGrouping;
        }

        private void AddVectorHeader(string indent, int pinGrouping, int size, StringBuilder sb)
        {

            string vectorHeader1 = ";" + indent;
            string vectorHeader2 = ";" + indent;

            for (int i = 1; i <= size; i++)
            {
                vectorHeader1 += (i / 10).ToString();
                vectorHeader2 += (i % 10).ToString();
                if (i % pinGrouping == 0)
                {
                    vectorHeader1 += " ";
                    vectorHeader2 += " ";
                }
            }

            sb.AppendLine(vectorHeader1);
            sb.AppendLine(vectorHeader2);
        }

        private string GetVector(string indent, int pinGrouping, int size, byte[] thisVector)
        {
            int index = thisVector.Length / 3;

            StringBuilder sb = new StringBuilder();
            sb.Append(indent);

            for (int i = index - 1; i >= 0; i--)
            {
                for (int j = 7; j >= 0; j--)
                {
                    int position = ((index * 8) - ((i * 8) + j));
                    if (position <= size)
                    {
                        bool bitVal = ((thisVector[(index * 2) + i] >> (7 - j)) & 0x01) > 0;
                        bool bitZ = ((thisVector[(index * 1) + i] >> (7 - j)) & 0x01) > 0;
                        bool bitIO = ((thisVector[i] >> (7 - j)) & 0x01) > 0;
                        if (bitZ)
                        {
                            sb.Append("Z");
                        }
                        else
                        {
                            if (bitVal)
                            {
                                //high or 1
                                if (bitIO) sb.Append("H");
                                else sb.Append("1");
                            }
                            else
                            {
                                if (bitIO) sb.Append("L");
                                else sb.Append("0");
                            }
                        }
                        if (position % pinGrouping == 0)
                        {
                            sb.Append(" ");
                        }
                    }
                }
            }

            //add comment
            sb.Append("\t;");
            foreach (byte b in thisVector)
            {
                sb.Append(b.ToString("X2") + " ");
            }

            return sb.ToString();
        }

        private string Reverse(string input)
        {
            char[] inputarray = input.ToCharArray();
            Array.Reverse(inputarray);
            return new string(inputarray);
        }

        private void buttonSaveBinary_Click(object sender, EventArgs e)
        {
            if (_currentLibraryFile != null)
            {
                SaveFileDialog sd = new SaveFileDialog();
                sd.FileName = _currentLibraryFile.FileName;
                sd.OverwritePrompt = true;
                DialogResult dr = sd.ShowDialog();
                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    File.WriteAllBytes(sd.FileName, _currentLibraryFile.FileBytes);
                    LogText("Saved '" + sd.FileName + "'");
                }
                
            }
            else
            {
                MessageBox.Show("You must load a library first.", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void buttonCreateBinaryLibrary_Click(object sender, EventArgs e)
        {
            if (listViewDevices.CheckedItems.Count > 0)
            {
                //ask for the output 
                SaveFileDialog sd = new SaveFileDialog();
                sd.InitialDirectory = ApplicationGlobals.LastDirectoryBrowse;
                sd.CheckPathExists = true;
                DialogResult dr = sd.ShowDialog();
                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    List<string> checkedItems = GetSelectedDevices();
                    List<byte> outputBytes = LibraryHelper.GetDeviceLibraries(checkedItems, LibraryFileFormat.LibraryBinary);
                    if (outputBytes != null && outputBytes.Count > 0)
                    {
                        File.WriteAllBytes(sd.FileName, outputBytes.ToArray());
                        MessageBox.Show("Library file created sucessfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("There was nothing to write.");
                    }
                }
            }
            else
            {
                MessageBox.Show("You must select at least one item from the Device Listing.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private List<string> GetSelectedDevices()
        {
            List<string> checkedItems = new List<string>();
            foreach (ListViewItem i in listViewDevices.CheckedItems)
            {
                checkedItems.Add(i.Text);
            }
            return checkedItems.Distinct().ToList();
        }

        //private List<byte> GetSelectedLibrariesAsBinary()
        //{
        //    List<string> checkedItems = new List<string>();
        //    foreach (ListViewItem i in listViewDevices.CheckedItems)
        //    {
        //        checkedItems.Add(i.Text);
        //    }

        //    List<DeviceLibrary> deviceLibraries = _currentLibraryFile.DeviceLibraries.Where(dl => dl.Items.Where(i => i.TypeDefinition == DeviceLibraryConfigurationItem.NAME && checkedItems.Contains(i.Data)).Count() > 0).Distinct().ToList();

        //    List<byte> outputBytes = new List<byte>();

        //    foreach (DeviceLibrary deviceLibrary in deviceLibraries)
        //    {
        //        List<byte> libraryBytes = deviceLibrary.AsBinary(deviceLibrary == deviceLibraries[deviceLibraries.Count - 1]);
        //        if (libraryBytes != null)
        //        {
        //            outputBytes.AddRange(libraryBytes);
        //        }
        //    }


        //    return outputBytes;
        //}

        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            listViewDevices.Items.Clear();
            List<string> uniqueNames = GetUniqueDeviceNames();
            string re = System.Text.RegularExpressions.Regex.Escape(textBoxFilter.Text).Replace("%", ".*").Replace("_", ".?");
            foreach (String s in uniqueNames.Where(u => Regex.IsMatch(u, re, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)).OrderBy(s => s))
            {
                listViewDevices.Items.Add(s);
            }
        }

        private void buttonCreateASCIILibrary_Click(object sender, EventArgs e)
        {
            if (listViewDevices.CheckedItems.Count > 0)
            {
                //ask for the output 
                SaveFileDialog sd = new SaveFileDialog();
                sd.InitialDirectory = ApplicationGlobals.LastDirectoryBrowse;
                sd.CheckPathExists = true;
                DialogResult dr = sd.ShowDialog();
                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    List<string> checkedItems = GetSelectedDevices();
                    List<byte> outputBytes = LibraryHelper.GetDeviceLibraries(checkedItems, LibraryFileFormat.LibraryBinary);
                    if (outputBytes != null && outputBytes.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        int byteCounter = 0;
                        foreach(byte b in outputBytes)
                        {
                            //encode binary to ASCII
                            sb.Append(b.ToString("X2"));
                            if (byteCounter < 15)
                            {
                                byteCounter++;
                                sb.Append(" ");
                            }
                            else
                            {
                                byteCounter = 0;
                                sb.Append("\r\n");
                            }
                        }
                        File.WriteAllText(sd.FileName, sb.ToString());
                        MessageBox.Show("Library file created sucessfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("There was nothing to write.");
                    }
                }
            }
            else
            {
                MessageBox.Show("You must select at least one item from the Device Listing.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }



        private void linkLabelUpdateBinarySize_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            labelBinarySize.Text = "calculating...";

            if (listViewDevices.CheckedItems.Count > 0)
            {
                List<string> checkedItems = GetSelectedDevices();
                List<byte> outputBytes = LibraryHelper.GetDeviceLibraries(checkedItems, LibraryFileFormat.LibraryBinary);
                labelBinarySize.Text = outputBytes.Count.ToString("0,000") + " bytes";
            }
            else
            {
                MessageBox.Show("You must select at least one item from the Device Listing.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void buttonConvertASCIIToBinary_Click(object sender, EventArgs e)
        {
            //this will load an ASCII file and convert it to binary...
            OpenFileDialog od = new OpenFileDialog();
            od.InitialDirectory = ApplicationGlobals.LastDirectoryBrowse;
            DialogResult dr = od.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                SaveFileDialog sd = new SaveFileDialog();
                sd.InitialDirectory = ApplicationGlobals.LastDirectoryBrowse;
                sd.FileName = od.FileName + ".bin";
                dr = sd.ShowDialog();
                byte[] inBytes = File.ReadAllBytes(od.FileName);
                if (inBytes != null)
                {
                    List<byte> decodedBytes = FileHelper.ASCIIEncode(0, inBytes);
                    if (dr == System.Windows.Forms.DialogResult.OK)
                    {
                        File.WriteAllBytes(sd.FileName, decodedBytes.ToArray());
                    }
                }
            }
        }

        private void buttonOpenList_Click(object sender, EventArgs e)
        {
            OpenFileDialog od = new OpenFileDialog();
            od.Filter = "Fluke User List Files (*.ulf)|*.ulf|All Files (*.*)|*";
            od.Multiselect = true;
            od.CheckFileExists = true;
            od.InitialDirectory = Utilities.GetBrowseDirectory();
            DialogResult dr = od.ShowDialog();

            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                string[] listDevices = File.ReadAllLines(od.FileName);

                foreach (string listDevice in listDevices)
                {
                    ListViewItem item = listViewDevices.FindItemWithText(listDevice);
                    if (item != null)
                    {
                        item.Checked = true;
                    }
                }
            }
        }

        private void buttonCreateXMLLibrary_Click(object sender, EventArgs e)
        {
            if (listViewDevices.CheckedItems.Count > 0)
            {
                //ask for the output 
                SaveFileDialog sd = new SaveFileDialog();
                sd.InitialDirectory = ApplicationGlobals.LastDirectoryBrowse;
                sd.CheckPathExists = true;
                DialogResult dr = sd.ShowDialog();
                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    List<string> checkedItems = GetSelectedDevices();
                    List<DeviceLibrary> selectedLibraries = LibraryHelper.GetDeviceLibraries(checkedItems);
                    System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(selectedLibraries.GetType());
                    System.IO.FileStream file = System.IO.File.Create(sd.FileName);
                    x.Serialize(file, selectedLibraries);
                    file.Close();
                }
            }
            else
            {
                MessageBox.Show("You must select at least one item from the Device Listing.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
