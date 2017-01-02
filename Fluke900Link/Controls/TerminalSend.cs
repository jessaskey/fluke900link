﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fluke900Link.Controls
{
    public partial class TerminalSend : UserControl
    {
        private string _lastCommand = null;

        public TerminalSend()
        {
            InitializeComponent();

            scintillaEditor.Styles[ScintillaNET.Style.Default].Font = "Courier New";
            scintillaEditor.Styles[ScintillaNET.Style.Default].Size = 10;
        }


        private void toolStripButtonSend_Click(object sender, EventArgs e)
        {
            if (Fluke900.IsConnected())
            {
                _lastCommand = scintillaEditor.Text;
                RemoteCommand commandFile = RemoteCommandFactory.GetCommand(RemoteCommandCodes.DataString, new string[] { scintillaEditor.Text });
                Fluke900.SendCommandOnly(commandFile);

                //PortManager.SendString(scintillaEditor.Text);
                scintillaEditor.ClearAll();

                RemoteCommandResponse cr = new RemoteCommandResponse();
                Fluke900.GetResponse(cr);
                while (cr.Status == CommandResponseStatus.Accepted)
                {
                    Fluke900.GetResponse(cr);
                }
                //finaly have a result here?
                string res = Encoding.ASCII.GetString(cr.RawBytes);

            }
            else
            {
                MessageBox.Show("You must be connected in order to send commands.", "Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void toolStripButtonRedo_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(_lastCommand))
            {
                scintillaEditor.AppendText(_lastCommand);
            }
        }
    }
}