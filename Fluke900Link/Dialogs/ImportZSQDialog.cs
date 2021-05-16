﻿using Fluke900Link.Containers;
using Fluke900Link.Factories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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
        public ImportZSQDialog()
        {
            InitializeComponent();
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            if (File.Exists(textBoxImportFileName.Text))
            {
                Project project = ImportFile(textBoxImportFileName.Text);
            }
            else
            {
                MessageBox.Show("File not found: " + textBoxImportFileName.Text, "File Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private Project ImportFile(string fileName)
        {
            Project project = new Project();
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
                        ImportData(currentSegment, project, locationRecords, sequenceRecords, fileBytes);
                        break;
                }
                //next please
                currentSegment += 1;
                i += length+4;
            }
            //parse the data into the project now..
            //sequence header record
            ProjectTest zsqTest = new ProjectTest();
            project.Tests.Add(zsqTest);
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
            //DEFAULT location test parameters
            CTreeRecord locationDefault = locationRecords.Where(r => r.Name == "DEFAULT").First();
            if (sequenceHeader != null)
            {
                List<string> headerData = sequenceHeader.Columns.Where(c => c.Name == "ExtendedData").First().Value as List<string>;

            }
            //load all the locations first
            foreach (var l in locationRecords)
            {
                ProjectLocation pl = new ProjectLocation();
                zsqTest.Locations.Add(pl);
            }
            //actual sequence records now
            foreach (var seq in sequenceRecords.OrderBy(r => r.Ordinal))
            {
                //header record is ignored
                if (seq.Name.ToLower() != "header")
                {
                    if (zsqTest.Sequences.Where(s => s.LocationName == seq.Name).Count() == 0)
                    {
                        ProjectLocation location = zsqTest.Locations.Where(l => l.Name == seq.Name).FirstOrDefault();
                        if (location != null)
                        {
                            zsqTest.Sequences.Add(new TestSequenceLocation(location));
                        }
                        
                    }
                }
            }
            return project;
        }

        private void ImportData(FileSegment segment, Project project, List<CTreeRecord> locationRecords, List<CTreeRecord> sequenceRecords, byte[] bytes)
        {
            Int16 dbIdentifier = BitConverter.ToInt16(bytes, 0);
            if (dbIdentifier != 0x0061)
            {
                project.ImportErrors.Add("Identifier for LCN file is incorrect. Expected 0x0061, found 0x" + dbIdentifier.ToString("X4"));
            }
            else
            {
                Int16 rowBase = BitConverter.ToInt16(bytes, 2);
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
                            rowBase += (Int16)(GetNextObjectBytes(bytes, rowBase).Length);
                            break;
                        default:
                            rowBase += 2;
                            break;
                    }
                }
            }
        }

        private byte[] GetNextObjectBytes(byte[] bytes, Int16 rowBase)
        {
            Int16 objectLength = BitConverter.ToInt16(bytes, rowBase + 2);
            return bytes.Skip(rowBase).Take(objectLength+6).ToArray();
        }

    }
}
