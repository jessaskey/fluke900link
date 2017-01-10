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

using Fluke900Link.Controllers;

namespace Fluke900Link.Dialogs
{
    public partial class SelfTestDialog : Form
    {
        private byte[] _loadedBytes = null;

        public SelfTestDialog()
        {
            InitializeComponent();
        }

        private void buttonListen_Click(object sender, EventArgs e)
        {
            if (FlukeController.IsConnected)
            {
                MessageBox.Show("You are currently connected to the Fluke in the main interface. You need to be 'DISCONNECTED' in the software so that you can manually send the Self Test data to this Wizard.", "Connection Issue", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                Cursor.Current = Cursors.WaitCursor;
                RemoteCommandResponse cr = new RemoteCommandResponse();
                //_loadedBytes = Fluke900.ReceivePrinterData();
                textBoxResults.Text = Encoding.ASCII.GetString(_loadedBytes);

                Cursor.Current = Cursors.Default;
            }
        }

        private void buttonSaveResults_Click(object sender, EventArgs e)
        {
            if (_loadedBytes != null)
            {
                SaveFileDialog sd = new SaveFileDialog();
                sd.InitialDirectory = Properties.Settings.Default.DefaultFilesDirectory;
                DialogResult dr = sd.ShowDialog();
                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    if (File.Exists(sd.FileName))
                    {
                        DialogResult dr2 = MessageBox.Show("File '" + sd.FileName + "' already exists. Are you sure you want to overwrite this file?", "Confirm File Overwrite", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dr2 == System.Windows.Forms.DialogResult.No)
                        {
                            return;
                        }
                        File.Delete(sd.FileName);
                    }
                    File.WriteAllBytes(sd.FileName, _loadedBytes);
                    MessageBox.Show("Data Saved", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog od = new OpenFileDialog();
            od.InitialDirectory = Globals.DOCUMENTS_FOLDER;

            DialogResult dr = od.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                byte[] bytes = File.ReadAllBytes(od.FileName);
                List<bool> bits = new List<bool>();
                List<byte> outbytes = new List<byte>();

                //we need to decode these into a big long binary stream
                foreach (byte b in bytes)
                {
                    byte mask = (byte)0x80;

                    for (int i = 0; i < 8; i++)
                    {
                        if ((b & mask) > 0)
                        {
                            bits.Add(true);
                        }
                        else
                        {
                            bits.Add(false);
                        }
                        mask = (byte)(mask >> 1);
                    }
                }

                //now rebuild, eliminiating stop bit and parity 

                int j = 1;  //skip first start byte

                byte currentByte = 0;
                int byteindex = 0;

                while (j < bits.Count)
                {
                    
                    
                    byte shiftIndex = (byte)(7 - byteindex);
                    if (bits[j])
                    {
                        currentByte |= (byte)(1 << shiftIndex);
                    }

                    if ((j + 1) % 8 == 0)
                    {
                        j += 3; //start bit, parity bit, stop bit
                        byteindex++;
                    }
                    else if ((byteindex + 1) % 8 == 0)
                    {
                        outbytes.Add(currentByte);
                        currentByte = 0;
                        byteindex = 0;
                    }
                    else
                    {
                        j++;
                        byteindex++;
                    }
                }

                textBoxResults.Text = "";


            }
        }
    }
}
