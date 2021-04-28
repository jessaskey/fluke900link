using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fluke900Link
{
    public partial class Splash : Form
    {
        //private Timer _timer = null;

        public bool HideButtons = false;
        //public string[] OpenArgs = null;

        public string ClientMessage
        {
            get { return linkLabelMessage.Text; }
            set { linkLabelMessage.Text = value; }
        }

        public Splash()
        {

            InitializeComponent();
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.BackColor = Color.Transparent;

            linkLabelMessage.Text = Globals.GetClientMessage();
            labelVersion.Text = "";
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                labelVersion.Text = "Version: " + ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            }
            else
            {
                labelVersion.Text = "Version: Debug";
            }
        }

        private void Splash_Shown(object sender, EventArgs e)
        {
            if (!HideButtons)
            {
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
                this.ControlBox = true;
                this.MinimizeBox = false;
                this.MaximizeBox = false;
            }

        }
        void timer_Tick(object sender, EventArgs e)
        {
            ////after 3 sec stop the timer
            //_timer.Stop();
            ////display mainform
            //MainForm mf = new MainForm();
            //mf.OpenArgs = OpenArgs;
            ////hide this form
            //this.Hide();
            ////show the mainform now, lets be happy
            //mf.Show();
            
        }

        private void Splash_Load(object sender, EventArgs e)
        {

        }

        private void linkLabelMessage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //yeah, Im being selfish about this for now... until I get a complete setup, I will keep this annoyance in here. :-)
            if (!String.IsNullOrEmpty(linkLabelMessage.Text))
            {
                System.Diagnostics.Process.Start("mailto:" + Globals.ADMIN_EMAIL + "?Subject=Fluke900");
            }
        }

 
    }
}
