﻿using Fluke900Link.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Fluke900Link.Containers.CTreeColumn;

namespace Fluke900Link.Factories
{
    public class CTreeSchemaFactory
    {
        public enum CTreeSchemaType
        {
            Sequence,
            Location,
        }



        public static CTreeSchema GetSchema(CTreeSchemaType schemaType)
        {
            CTreeSchema schema = null;
            switch (schemaType)
            {
                case CTreeSchemaType.Location:
                    schema = new CTreeSchema();
                    schema.Columns.Add(new CTreeColumn("Ordinal", CTreeColumnType.Number, 4));
                    schema.Columns.Add(new CTreeColumn("Pins", CTreeColumnType.String, 0));
                    schema.Columns.Add(new CTreeColumn("Name", CTreeColumnType.String, 0));
                    schema.Columns.Add(new CTreeColumn("DeviceName", CTreeColumnType.String, 0));
                    schema.Columns.Add(new CTreeColumn("Device", CTreeColumnType.String, 0)); //F900
                    schema.Columns.Add(new CTreeColumn("Group", CTreeColumnType.String, 0)); //2 MAIN
                    schema.Columns.Add(new CTreeColumn("VccPin", CTreeColumnType.String, 0)); //20
                    schema.Columns.Add(new CTreeColumn("GroundPin", CTreeColumnType.String, 0)); //10
                    schema.Columns.Add(new CTreeColumn("ReferenceDeviceDrive", CTreeColumnType.String, 0)); //high
                    schema.Columns.Add(new CTreeColumn("Ny", CTreeColumnType.String, 0)); //ny
                    //schema.Columns.Add(new CTreeColumn("High", CTreeColumnType.String, 0)); ?
                    //schema.Columns.Add(new CTreeColumn("Ny", CTreeColumnType.String, 0));
                    //Pin Definition Array is 6 properties per pin
                    // A0  Flags 
                    // 00  
                    // F0  Frequency String
                    // 00
                    // X   Pin Gate Defintion
                    // 00
                    // P   Pin Ignore Flag (P?/I/C?)
                    // 00
                    // SW1 Sync Word 1
                    // 00
                    // SW2 Sync Word 2
                    // 00
                    //After the pin definitions, there are some global pin definitions
                    // X.X...X..1.1.
                    // Last two are the Software Trigger Word 1 and Word 2 external trigger values
                    //schema.Columns.Add(new CTreeColumn("PinDefinitions", CTreeColumnType.PinDefinitions, 0x17A));
                    schema.Columns.Add(new CTreeColumn("PinDefinitions", CTreeColumnType.ByteArray, (6*28)+6));
                    schema.Columns.Add(new CTreeColumn("SimulationOption", CTreeColumnType.String, 0));
                    schema.Columns.Add(new CTreeColumn("ReferenceDeviceTest", CTreeColumnType.String, 0));
                    schema.Columns.Add(new CTreeColumn("ClipCheck", CTreeColumnType.String, 0));
                    schema.Columns.Add(new CTreeColumn("Unknown2", CTreeColumnType.String, 0));
                    schema.Columns.Add(new CTreeColumn("SyncOnOff", CTreeColumnType.String, 0));
                    schema.Columns.Add(new CTreeColumn("SyncTime", CTreeColumnType.String, 0));
                    schema.Columns.Add(new CTreeColumn("Trigger", CTreeColumnType.String, 0));
                    schema.Columns.Add(new CTreeColumn("ResetNegativeOffset", CTreeColumnType.String, 0));
                    schema.Columns.Add(new CTreeColumn("RAMShadow", CTreeColumnType.String, 0));
                    schema.Columns.Add(new CTreeColumn("FaultMask", CTreeColumnType.String, 0));
                    schema.Columns.Add(new CTreeColumn("TestTime", CTreeColumnType.String, 0));
                    schema.Columns.Add(new CTreeColumn("GateEnable", CTreeColumnType.String, 0));
                    schema.Columns.Add(new CTreeColumn("GateDelay", CTreeColumnType.String, 0));
                    schema.Columns.Add(new CTreeColumn("GateDuration", CTreeColumnType.String, 0));
                    schema.Columns.Add(new CTreeColumn("GatePolarity", CTreeColumnType.String, 0));
                    schema.Columns.Add(new CTreeColumn("Unknown3", CTreeColumnType.String, 0));
                    schema.Columns.Add(new CTreeColumn("Threshold", CTreeColumnType.String, 0));
                    schema.Columns.Add(new CTreeColumn("ResetVcc", CTreeColumnType.String, 0));
                    schema.Columns.Add(new CTreeColumn("ResetPolarity", CTreeColumnType.String, 0));
                    schema.Columns.Add(new CTreeColumn("ResetDuration", CTreeColumnType.String, 0));
                    schema.Columns.Add(new CTreeColumn("X21", CTreeColumnType.String, 0));
                    schema.Columns.Add(new CTreeColumn("X22", CTreeColumnType.String, 0));
                    schema.Columns.Add(new CTreeColumn("X23", CTreeColumnType.String, 0));
                    break;
                case CTreeSchemaType.Sequence:
                    schema = new CTreeSchema();
                    schema.Columns.Add(new CTreeColumn("Ordinal", CTreeColumnType.Number, 4));
                    schema.Columns.Add(new CTreeColumn("Unknown3", CTreeColumnType.Number, 4));
                    schema.Columns.Add(new CTreeColumn("Unknown4", CTreeColumnType.Number, 4));
                    schema.Columns.Add(new CTreeColumn("Name", CTreeColumnType.String, 0));
                    schema.Columns.Add(new CTreeColumn("ExtendedData", CTreeColumnType.StringArray, 0));
                    break;
                //case CTreeSchemaType.SequenceD:
                //    schema = new CTreeSchema();
                //    schema.Columns.Add(new CTreeColumn("Unknown1", CTreeColumnType.Number, 2));
                //    schema.Columns.Add(new CTreeColumn("Unknown2", CTreeColumnType.Number, 4));
                //    schema.Columns.Add(new CTreeColumn("Unknown3", CTreeColumnType.Number, 4));
                //    schema.Columns.Add(new CTreeColumn("Unknown4", CTreeColumnType.Number, 4));
                //    schema.Columns.Add(new CTreeColumn("Name", CTreeColumnType.String, 0));
                //    break;
            }
            return schema;
        }
    }
}
