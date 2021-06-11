using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fluke900Link.Dialogs
{
    public partial class ProgressDialog : Form
    {
        private static ProgressDialog _instance = new ProgressDialog();
        public static ProgressDialog Instance { get { return _instance; } }


        public ProgressDialog()
        {
            InitializeComponent();
            this.TopMost = true;
        }

        public void StartProgress(string title, string topMessage)
        {
            this.Text = title;
            if (IsHandleCreated)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    labelTop.Text = topMessage;
                    radWaitingBar1.StartWaiting();
                    this.Show();
                });
            }
        }

        public void StartProgress(string topMessage)
        {
            StartProgress("Fluke 900 Link", topMessage);
        }

        public void StopProgress()
        {
            radWaitingBar1.StopWaiting();
            this.Hide();
        }


    }
}
