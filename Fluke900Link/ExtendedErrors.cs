using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900Link
{
    public static class ExtendedErrors
    {
        public static Dictionary<string,string> CodeValues = new Dictionary<string,string>() 
                {
                    { "0", "No fault."},
                    { "1", "File is open."},
                    { "2", "File not found/Device already formatted."},
                    { "3", "File not found."},
                    { "4", "File already exists."},
                    { "5", "Insufficient space on device."},
                    { "6", "Error writing to device."},
                    { "7", "Device is read-only."},
                    { "8", "File is modify protected."},
                    { "9", "File cannot be deleted."},
                    { "10", "Non-existent device."},
                    { "11", "Device not open."},
                    { "12", "Error setting page."},
                    { "13", "Error setting chapter."},
                    { "14", "Cartridge not inserted."},
                    { "15", "Password invalid."},
                    { "16", "Insufficient stack space for transfer."},
                    { "17", "Checksum error on file."},
                    { "18", "File is copy protected."},
                    { "19", "Unformatted cartridge."},
                    { "20", "File unrecoverable."},
                    { "21`", ""},
                    { "22", ""},
                    { "23", ""},
                    { "24", ""},
                    { "25", ""},
                    { "26", ""},
                    { "27", ""},
                    { "28", "File too large for destination."},
                    { "29", ""},
                    { "30", ""},
                    { "31", ""},
                    { "32", ""},
                    { "33", ""},
                    { "34", "Invalid file type."},
                    { "35", ""},
                    { "36", "Corrupt file."},
                };
    }
}
