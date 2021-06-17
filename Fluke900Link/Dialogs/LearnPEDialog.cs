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

        private PerformanceEnvelopeSettings _settings = null;
        private PEResults _results = null;

        public string LastError { get; set; }


        public LearnPEDialog()
        {
            InitializeComponent();
            _backBrush = new SolidBrush(this.BackColor);
        }

        public bool Learn(PerformanceEnvelopeSettings settings)
        {
            bool result = false;
            if (settings != null)
            {
                _settings = settings;
                try
                {
                    numericUpDownFaultMaskFrom.Value = _settings.FaultMask;
                    numericUpDownFaultMaskStep.Value = _settings.FaultMaskStep;
                    labelFaultMaskTo.Text = (_settings.FaultMask + (_settings.FaultMaskStep * (_settings.FaultMaskTestCount-1))).ToString();
                    numericUpDownThresholdFrom.Value = _settings.Threshold;
                    numericUpDownThresholdStep.Value = _settings.ThresholdStep;
                    labelThresholdTo.Text = (_settings.Threshold + (_settings.ThresholdStep * (_settings.ThresholdTestCount-1))).ToString();
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
            _results = null;
            //send the PE test command
            ClientCommand peCommand = new ClientCommand(ClientCommands.PerformanceEnvelope);
            peCommand.Parameters.Add(_settings.FaultMask.ToString());
            peCommand.Parameters.Add(_settings.FaultMaskStep.ToString());
            peCommand.Parameters.Add(_settings.Threshold.ToString());
            peCommand.Parameters.Add(_settings.ThresholdStep.ToString());
            ClientCommandResponse response = await ClientController.SendCommand(peCommand);
            _results = new PEResults(response.RawBytes);
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

            if (_settings != null)
            {
                for (int i = 0; i < _settings.FaultMaskTestCount; i++)
                {
                    int currentFaultMask = _settings.FaultMask + (_settings.FaultMaskStep * i);
                    int yPosition = panel.Height - _yAxisOffset - (_yAxisOffset + (i * 19));
                    //y axis
                    g.DrawString(currentFaultMask.ToString(), _drawFont, Brushes.Black, 0f, (float)yPosition);
                    for (int j = 0; j < _settings.ThresholdTestCount; j++)
                    {
                        int currentThreshold = _settings.Threshold + (_settings.ThresholdStep * j);
                        int xPosition = _xAxisOffset + (j * 55);
                        if (i == 0)
                        {
                            //x axis
                            g.DrawString(currentThreshold.ToString(), _drawFont, Brushes.Black, xPosition, panel.Height - 20f);
                        }
                        //results
                        if (_results != null) {
                            int resultIndex = (i * _settings.FaultMaskTestCount) + j;
                            if (_results.Results.Count > resultIndex) 
                            {
                                //draw the result now..
                                if (_results.Results[i].PassFail)
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

        }
    }
}
