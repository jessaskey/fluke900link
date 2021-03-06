﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Fluke900Link.Controllers;
using Fluke900Link.Containers;
using Fluke900.Containers;
using Fluke900;

namespace Fluke900Link.Controls
{
    public partial class TerminalSendControl : DockContentEx
    {
        private string _lastCommand = null;

        public TerminalSendControl()
        {
            InitializeComponent();

            scintillaEditor.Styles[ScintillaNET.Style.Default].Font = "Courier New";
            scintillaEditor.Styles[ScintillaNET.Style.Default].Size = 10;
        }


        private void toolStripButtonSend_Click(object sender, EventArgs e)
        {
            if (FlukeController.IsConnected)
            {
                _lastCommand = scintillaEditor.Text;
                ClientCommand commandFile = ClientCommand.GetCommand(ClientCommands.DataString, new string[] { scintillaEditor.Text });

                Task.Run(async () => { await FlukeController.SendCommand(commandFile); }).Wait();
                //FlukeController.SendCommandOnly(commandFile);

                scintillaEditor.ClearAll();

                //finaly have a result here?
                string res = Encoding.ASCII.GetString(commandFile.Response.RawBytes);

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
