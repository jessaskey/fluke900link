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

namespace Fluke900Link.Controls
{
    public partial class TerminalOutputControl : DockContentEx
    {


        public TerminalOutputControl()
        {
            InitializeComponent();

            scintillaMain.Styles[ScintillaNET.Style.Default].Font = "Courier New";
            scintillaMain.Styles[ScintillaNET.Style.Default].Size = 10;
  
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
                File.WriteAllText(sd.FileName, scintillaMain.Text);
            }
        }

        private void toolStripButtonClearWindow_Click(object sender, EventArgs e)
        {
            //richTextBox.Text = "";
            scintillaMain.ClearAll();
        }

        public bool WordWrap
        {
            get { return scintillaMain.WrapMode == WrapMode.None ? false : true; }
            set { if (value == true) {
                scintillaMain.WrapMode = WrapMode.Whitespace;
                }
                else {scintillaMain.WrapMode = WrapMode.None; }
            }
        }

        public void WriteLine(string text)
        {
            if (scintillaMain != null && !scintillaMain.IsDisposed)
            {
                scintillaMain.AppendText(text);
                scintillaMain.AppendText(Environment.NewLine);
                scintillaMain.ExecuteCmd(ScintillaNET.Command.DocumentEnd);
            }
        }

        public void WriteLine(string text, Color color, bool fontBold)
        {

            if (scintillaMain != null && !scintillaMain.IsDisposed)
            {
                scintillaMain.AppendText(text);
                scintillaMain.AppendText(Environment.NewLine);
                scintillaMain.ExecuteCmd(ScintillaNET.Command.DocumentEnd);
            }

        }


    }


}
