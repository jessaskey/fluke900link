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
        private Timer _timer = null;

        public bool AutoClose = false;
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
            if (AutoClose)
            {
                _timer = new Timer();
                _timer.Interval = 1500;
                _timer.Tick += timer_Tick;
                _timer.Start();
            }
            else
            {
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
                this.ControlBox = true;
                this.MinimizeBox = false;
                this.MaximizeBox = false;
            }

        }
        void timer_Tick(object sender, EventArgs e)
        {
            //after 3 sec stop the timer
            _timer.Stop();
            //display mainform
            MainForm mf = new MainForm();
            //hide this form
            this.Hide();
            mf.Show();
            
        }

        private void Splash_Load(object sender, EventArgs e)
        {

        }

        private void linkLabelMessage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!String.IsNullOrEmpty(linkLabelMessage.Text))
            {
                System.Diagnostics.Process.Start("mailto:" + Globals.ADMIN_EMAIL + "?Subject=Fluke900");
            }
        }

 
    }
}
