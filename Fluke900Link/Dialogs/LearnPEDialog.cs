using Fluke900.Containers;
using Fluke900.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fluke900Link.Dialogs
{
    public partial class LearnPEDialog : Form
    {
        private const int _xAxisOffset = 50;
        private const int _yAxisOffset = 30;
        private Brush _backBrush = null;
        private Brush _passBrush = Brushes.LimeGreen;
        private Brush _failBrush = Brushes.Red;
        private Font _drawFont = new System.Drawing.Font("Consolas", 12, FontStyle.Bold);

        
        private ClientCommand _currentCommand = null;
 
        public string LastError { get; set; }
        public PerformanceEnvelopeSettings Settings { get; set; }
        public PEResults Results { get; set; }

        public LearnPEDialog()
        {
            InitializeComponent();
            _backBrush = new SolidBrush(this.BackColor);
        }

        private bool Learn()
        {
            bool result = false;
            if (Settings != null)
            {
                try
                {
                    numericUpDownFaultMaskFrom.Value = Settings.FaultMask;
                    numericUpDownFaultMaskStep.Value = Settings.FaultMaskStep;
                    labelFaultMaskTo.Text = (Settings.FaultMask + (Settings.FaultMaskStep * (Settings.FaultMaskTestCount-1))).ToString();
                    numericUpDownThresholdFrom.Value = Settings.Threshold;
                    numericUpDownThresholdStep.Value = Settings.ThresholdStep;
                    labelThresholdTo.Text = (Settings.Threshold + (Settings.ThresholdStep * (Settings.ThresholdTestCount-1))).ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                StartTest();
            }
            return result;
        }

        private async void StartTest()
        {
            //send the PE test command
            _currentCommand = ClientCommand.GetCommand(ClientCommands.PerformanceEnvelope);
            _currentCommand.Parameters.Add(Settings.FaultMask.ToString());
            _currentCommand.Parameters.Add(Settings.FaultMaskStep.ToString());
            _currentCommand.Parameters.Add(Settings.Threshold.ToString());
            _currentCommand.Parameters.Add(Settings.ThresholdStep.ToString());
            await SerialController.SendCommand(_currentCommand);
            Results = new PEResults(_currentCommand.Response.RawBytes);
            panelTestResult.Invalidate();
            timerTest.Enabled = true;
        }

        private async void buttonUse_Click(object sender, EventArgs e)
        {

        }

        private void panelTestResult_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Panel panel = sender as Panel;

            g.FillRectangle(_backBrush, new Rectangle(0, 0, panel.Width, panel.Height));
            //horizontal Y Axis line
            g.DrawLine(Pens.Black, new Point(_yAxisOffset, panel.Height - _yAxisOffset), new Point(panel.Width, panel.Height - _yAxisOffset));
            //vertical X Axis line
            g.DrawLine(Pens.Black, new Point(_yAxisOffset, panel.Height - _yAxisOffset), new Point(_yAxisOffset, panel.Height - panel.Height));

            if (Settings != null)
            {
                for (int i = 0; i < Settings.FaultMaskTestCount; i++)
                {
                    int currentFaultMask = Settings.FaultMask + (Settings.FaultMaskStep * i);
                    int yPosition = panel.Height - _yAxisOffset - (_yAxisOffset + (i * 19));
                    //y axis
                    g.DrawString(currentFaultMask.ToString(), _drawFont, Brushes.Black, 0f, (float)yPosition);
                    for (int j = 0; j < Settings.ThresholdTestCount; j++)
                    {
                        int currentThreshold = Settings.Threshold + (Settings.ThresholdStep * j);
                        int xPosition = _xAxisOffset + (j * 55);
                        if (i == 0)
                        {
                            //x axis
                            g.DrawString(currentThreshold.ToString(), _drawFont, Brushes.Black, xPosition, panel.Height - 20f);
                        }
                        //results
                        if (Results != null) {
                            PEResult result = Results.Results.Where(r => r.FaultMask == currentFaultMask && r.Threshold == currentThreshold).FirstOrDefault();
                            if (result != null)
                            { 
                                //does this match the final suggestion?
                                if (result.FaultMask == Results.SuggestedFaultMask &&
                                    result.Threshold == Results.SuggestedThreshold)
                                {
                                    SizeF blockSize = g.MeasureString("PASS", _drawFont);
                                    g.FillRectangle(Brushes.White, new Rectangle(xPosition, yPosition, (int)blockSize.Width, (int)blockSize.Height));
                                }
                                //draw the result now..
                                if (result.PassFail)
                                {
                                    g.DrawString("PASS", _drawFont, _passBrush, xPosition, yPosition);
                                }
                                else
                                {
                                    g.DrawString("****", _drawFont, _failBrush, xPosition, yPosition);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void timerTest_Tick(object sender, EventArgs e)
        {
            timerTest.Enabled = false;
            if (_currentCommand != null)
            {
                if (_currentCommand.Response != null)
                {
                    Results = new PEResults(_currentCommand.Response.RawBytes);
                    if (Results != null)
                    {
                        if (_currentCommand.Response.Status == Fluke900.CommandResponseStatus.Executing)
                        {
                            timerTest.Enabled = true; 
                            panelTestResult.Invalidate();
                        }
                    }
                }
            }
        }

        private void LearnPEDialog_Shown(object sender, EventArgs e)
        {
            if (Settings != null)
            {
                Learn();
            }
        }
    }
}
