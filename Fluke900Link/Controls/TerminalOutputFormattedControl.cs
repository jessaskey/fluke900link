using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ScintillaNET;
using WeifenLuo.WinFormsUI.Docking;
using Fluke900Link.Containers;
using Fluke900.Containers;

namespace Fluke900Link.Controls
{
    public partial class TerminalOutputFormattedControl : DockContentEx
    {
        private List<ClientCommand> _commands = new List<ClientCommand>();

        public TerminalOutputFormattedControl()
        {
            InitializeComponent();
        }

        public void CommandSendProgress(ClientCommand command)
        {
            if (command != null)
            {
                StringBuilder sb = new StringBuilder();
                //sb.AppendLine("//-----------------------------//");
                //sb.AppendLine("// BEGIN SEND                  //");
                //sb.AppendLine("//-----------------------------//");
                //sb.Append("-->");
                sb.Append(command.CommandString);
                //if (command.Parameters != null)
                //{
                //    for(int i = 0; i < command.Parameters.Count; i++)
                //    {
                //        sb.Append("Parameter" + i.ToString() + ": ");
                //        sb.Append(command.Parameters[i]);
                //        sb.Append("\r");
                //    }
                //}
                WriteText(sb.ToString(), Color.Green);
                //WriteText("<-- WAITING FOR RESPONSE -->", Color.Black);
            }
        }
        public void CommandResponseProgress(ClientCommandResponse response)
        {
            //WriteText("<-- RECEIVED RESPONSE -->",Color.Black);
            if (response != null)
            {
                string[] formattedResult = response.FormattedResult;
                if (formattedResult != null)
                {
                    foreach (string s in formattedResult)
                    {
                        WriteText(s, Color.Red);
                    }
                }
            }
            //WriteText("<-- END RECEIVE -->", Color.Black);
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sd = new SaveFileDialog();
            sd.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            sd.CheckFileExists = false;
            DialogResult dr = sd.ShowDialog();

            if (dr == DialogResult.OK)
            {
                if (File.Exists(sd.FileName))
                {
                    DialogResult dr2 = MessageBox.Show("File already exists, are you sure you want to overwrite it?", "File Exists", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dr2 == DialogResult.No || dr2 == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                File.WriteAllText(sd.FileName, richTextBox1.Text);
            }
        }

        private void toolStripButtonClearWindow_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }

        public bool WordWrap
        {
            get { return richTextBox1.WordWrap; }
            set { richTextBox1.WordWrap = value; }
        }


        public void WriteText(string text, Color color)
        {
            if (!String.IsNullOrEmpty(text)) {
                if (!richTextBox1.IsDisposed)
                {
                    richTextBox1.Select(richTextBox1.TextLength, 0);
                    richTextBox1.SelectionColor = color;
                    richTextBox1.AppendText(text);
                    richTextBox1.AppendText("\r");
                }
            }
        }
    }
}