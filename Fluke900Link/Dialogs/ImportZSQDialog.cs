using Fluke900;
using Fluke900.Containers;
using Fluke900Link.Containers;
using Fluke900Link.Factories;
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


namespace Fluke900Link.Dialogs
{
    public enum FileSegment : int
    {
        SQN = 0,
        SQX,
        LCN,
        LCX
    }

    public enum SequenceHeader : int
    {
        Title,
        Description,
        Author,
        StartMessage,
        Mode,
        Version,
        Software,
        SoftwareVersion
    }

    public partial class ImportZSQDialog : Form
    {
        public ProjectTest ImportedProjectTest { get; set; }

        public ImportZSQDialog()
        {
            InitializeComponent();
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            if (File.Exists(textBoxImportFileName.Text))
            {
                ImportedProjectTest = ImportFile(textBoxImportFileName.Text);
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("File not found: " + textBoxImportFileName.Text, "File Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private ProjectTest ImportFile(string fileName)
        {
            ProjectTest zsqTest = new ProjectTest();
            int i = 0;
            FileSegment currentSegment = 0;  //start at lowest, ordered by enum definition
            byte[] bytes = File.ReadAllBytes(fileName);
            List<CTreeRecord> locationRecords = new List<CTreeRecord>();
            List<CTreeRecord> sequenceRecords = new List<CTreeRecord>();
            //there will always be 4 sections in the main file, but 
            //they are of different 1K lengths.
            while (i < bytes.Length) {
                Int32 length = BitConverter.ToInt32(bytes, i);
                byte[] fileBytes = bytes.Skip(i + 4).Take(length).ToArray();
                switch (currentSegment)
                {
                    case FileSegment.LCN:
                    case FileSegment.SQN:
                        ImportData(currentSegment, zsqTest, locationRecords, sequenceRecords, fileBytes);
                        break;
                }
                //next please
                currentSegment += 1;
                i += length+4;
            }
            //parse the data into the project now..
            //sequence header record
            CTreeRecord sequenceHeader = sequenceRecords.Where(r => r.Name == "HEADER").First();
            if (sequenceHeader != null)
            {
                List<string> headerData = sequenceHeader.Columns.Where(c => c.Name == "ExtendedData").First().Value as List<string>;

                zsqTest.Title = headerData[(int)SequenceHeader.Title];
                zsqTest.Description = headerData[(int)SequenceHeader.Description];
                zsqTest.Author = headerData[(int)SequenceHeader.Author];
                zsqTest.StartMessage = headerData[(int)SequenceHeader.StartMessage];
                zsqTest.Mode = headerData[(int)SequenceHeader.Mode].ToLower() == "develop" ? ProjectTest.TestMode.Develop : ProjectTest.TestMode.ReadOnly;
                zsqTest.Version = decimal.Parse(headerData[(int)SequenceHeader.Version]);
                zsqTest.Software = headerData[(int)SequenceHeader.Software];
                zsqTest.SoftwareVersion = decimal.Parse(headerData[(int)SequenceHeader.SoftwareVersion]);
            }
            //set Title if it was not set before
            if (String.IsNullOrEmpty(zsqTest.Title))
            {
                zsqTest.Title = Path.GetFileNameWithoutExtension(fileName);
            }
            //DEFAULT location test parameters
            CTreeRecord locationDefault = locationRecords.Where(r => r.Name == "DEFAULT").First();
            if (locationDefault != null)
            {
                zsqTest.DefaultLocation = ImportLocation(locationDefault);
            }
            
            //load all the locations first
            foreach (var l in locationRecords.Where(r => r.Name != "DEFAULT"))
            {
                ProjectLocation pl = ImportLocation(l);
                zsqTest.Locations.Add(pl);
            }
            //actual sequence records now
            foreach (var seq in sequenceRecords.OrderBy(r => r.Ordinal))
            {
                //header record is ignored
                if (seq.Name.ToLower() != "header")
                {
                    if (zsqTest.Sequences.Where(s => s.Location.Name == seq.Name).Count() == 0)
                    {
                        ProjectLocation location = zsqTest.Locations.Where(l => l.Name == seq.Name).FirstOrDefault();
                        if (location != null)
                        {
                            zsqTest.Sequences.Add(new TestSequenceLocation(seq.Name, location));
                        }
                        
                    }
                }
            }
            return zsqTest;
        }

        private ProjectLocation ImportLocation(CTreeRecord record)
        {
            int defaultPinCount = int.Parse(record.Columns.Where(c => c.Name.ToLower() == "pins").FirstOrDefault().Value.ToString());
            List<byte[]> pinDefinitionBytes = record.Columns.Where(c => c.Name.ToLower() == "pindefinitions").First().Value as List<byte[]>;
            ProjectLocation projectLocation = new ProjectLocation();
            projectLocation.Name = record.Name;
            projectLocation.DeviceName = record.Columns.Where(c => c.Name == "DeviceName").First().Value.ToString();
            projectLocation.Pins = int.Parse(record.Columns.Where(c => c.Name.ToLower() == "pins").FirstOrDefault().Value.ToString());
            projectLocation.LoadPinDefinitions(pinDefinitionBytes);
            string simulationValue = record.Columns.Where(c => c.Name == "SimulationOption").FirstOrDefault().Value.ToString();
            if (simulationValue == "N/I" || simulationValue == "N/A")
            {
                projectLocation.Simulation = Fluke900.SimulationShadowDefinition.NotInstalled;
            }
            else
            {
                projectLocation.Simulation = simulationValue == "E" ? SimulationShadowDefinition.Enabled : SimulationShadowDefinition.Disabled;
            }
            projectLocation.ReferenceDeviceTest = record.Columns.Where(c => c.Name == "ReferenceDeviceTest").FirstOrDefault().Value.ToString() == "on";
            projectLocation.ClipCheck = record.Columns.Where(c => c.Name == "ClipCheck").FirstOrDefault().Value.ToString() == "on";
            string syncTimeValue = record.Columns.Where(c => c.Name == "SyncTime").FirstOrDefault().Value.ToString();
            if (syncTimeValue == "off")
            {
                projectLocation.SyncTime = null;
            }
            else
            {
                int syncTime = 0;
                if (int.TryParse(syncTimeValue, out syncTime))
                {
                    projectLocation.SyncTime = syncTime;
                }
            }
            //trigger
            projectLocation.TriggerEnabled = record.Columns.Where(c => c.Name == "Trigger").FirstOrDefault().Value.ToString() == "on";
            //RAM Shadow
            projectLocation.RAMShadow = SimulationShadowDefinition.NotInstalled;
            string ramShadowValue = record.Columns.Where(c => c.Name == "RAMShadow").FirstOrDefault().Value.ToString();
            if (ramShadowValue == "on")
            {
                projectLocation.RAMShadow = SimulationShadowDefinition.Enabled;
            }
            else
            {
                projectLocation.RAMShadow = SimulationShadowDefinition.Disabled;
            }
            projectLocation.FaultMask = int.Parse(record.Columns.Where(c => c.Name == "FaultMask").FirstOrDefault().Value.ToString());
            projectLocation.TestTime = record.Columns.Where(c => c.Name == "TestTime").FirstOrDefault().Value.ToString();
            projectLocation.Gate = new GateDefinition();
            projectLocation.Gate.Polarity = record.Columns.Where(c => c.Name == "GatePolarity").FirstOrDefault().Value.ToString() == "T" ? true : false;
            projectLocation.Gate.Delay = UnitTime.Parse(record.Columns.Where(c => c.Name == "GateDelay").FirstOrDefault().Value.ToString());
            string gateDurationValue = record.Columns.Where(c => c.Name == "GateDuration").FirstOrDefault().Value.ToString();
            if (gateDurationValue == "C")
            {
                projectLocation.Gate.Duration = null;
            }
            else
            {
                projectLocation.Gate.Duration = UnitTime.Parse(gateDurationValue);
            }
            projectLocation.Threshold = int.Parse(record.Columns.Where(c => c.Name == "Threshold").FirstOrDefault().Value.ToString());
            //reset
            projectLocation.Reset = new ResetDefinition();
            projectLocation.Reset.Polarity = record.Columns.Where(c => c.Name == "ResetPolarity").FirstOrDefault().Value.ToString().Substring(0, 1);
            projectLocation.Reset.Source = record.Columns.Where(c => c.Name == "ResetVcc").FirstOrDefault().Value.ToString().Substring(0, 1);
            projectLocation.Reset.NegativeOffset = int.Parse(record.Columns.Where(c => c.Name == "ResetNegativeOffset").FirstOrDefault().Value.ToString());
            projectLocation.Reset.Duration = int.Parse(record.Columns.Where(c => c.Name == "ResetDuration").FirstOrDefault().Value.ToString());
            return projectLocation;
        }

        private void ImportData(FileSegment segment, ProjectTest projectTest, List<CTreeRecord> locationRecords, List<CTreeRecord> sequenceRecords, byte[] bytes)
        {
            UInt16 dbIdentifier = BitConverter.ToUInt16(bytes, 0);
            if (dbIdentifier != 0x0061)
            {
                projectTest.ImportErrors.Add("Identifier for LCN file is incorrect. Expected 0x0061, found 0x" + dbIdentifier.ToString("X4"));
            }
            else
            {
                UInt16 rowBase = BitConverter.ToUInt16(bytes, 2);
                while (rowBase < bytes.Length)
                {
                    CTreeSchema schema = null;
                    byte objectType = bytes[rowBase];
                    switch (objectType) {
                        case 0xFA:
                            switch (segment)
                            {
                                //sequence file has a few different entity types
                                case FileSegment.SQN:
                                    schema = CTreeSchemaFactory.GetSchema(CTreeSchemaFactory.CTreeSchemaType.Sequence);
                                    CTreeRecord sequenceRecord = new CTreeRecord(schema, GetNextObjectBytes(bytes, rowBase));
                                    sequenceRecords.Add(sequenceRecord);
                                    rowBase += sequenceRecord.Length;
                                    break;
                                case FileSegment.LCN:
                                    schema = CTreeSchemaFactory.GetSchema(CTreeSchemaFactory.CTreeSchemaType.Location);
                                    CTreeRecord locationRecord = new CTreeRecord(schema, GetNextObjectBytes(bytes, rowBase));
                                    locationRecords.Add(locationRecord);
                                    rowBase += locationRecord.Length;
                                    break;
                            }
                            break;
                        case 0xFB:
                        case 0xFD:           
                            //deleted records, can ignore for now
                            rowBase += (UInt16)(GetNextObjectBytes(bytes, rowBase).Length);
                            break;
                        default:
                            rowBase += 2;
                            break;
                    }
                }
            }
        }

        private byte[] GetNextObjectBytes(byte[] bytes, UInt16 rowBase)
        {
            Int16 objectLength = BitConverter.ToInt16(bytes, rowBase + 2);
            return bytes.Skip(rowBase).Take(objectLength+6).ToArray();
        }

    }
}
