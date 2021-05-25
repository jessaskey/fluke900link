using System;
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
    public partial class PinDiagramControl : UserControl
    {
       // private int _titlePadding { get; set; } = 20;
        private int _edgePadding { get; set; } = 20;
        private int _valuePadding { get; set; } = 10;
        private int _verticalSpacing { get; set; } = 20;
        private int _textVerticalSpacing { get; set; } = 18;
        private int _chipWidth { get; set; } = 90;

        private Pen _blackPen = new Pen(Color.FromArgb(255, 0, 0, 0), 1);
        private Font _drawFontDevice = new System.Drawing.Font("Microsoft Sans Serif", 14, FontStyle.Bold);
        private Font _drawFont = new System.Drawing.Font("Microsoft Sans Serif", 14);
        private SolidBrush _blackBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);

        public int PinCount { get; set; }
        public Type ValueType { get; set; }
        //public string Title { get; set; }
        public string Device { get; set; }

        public List<string> Values { get; set; }

        public Font DiagramFont { get; set; }

        public PinDiagramControl()
        {
            InitializeComponent();
        }

        private void panelMain_Paint(object sender, PaintEventArgs e)
        {

            if (ValueType != null)
            {
                int valueCount = Enum.GetValues(ValueType).Length;

                using (Graphics g = e.Graphics)
                {
                    int halfPinCount = PinCount / 2;
                    //float titleWidth = g.MeasureString(Title, DiagramFont).Width;
                    //g.DrawString(Title, DiagramFont, Brushes.Black, new PointF((Width - titleWidth) / 2f, _titlePadding));

                    //draw the chip shape
                    int chipX = _edgePadding + (valueCount * _valuePadding);
                    int chipY= _verticalSpacing;
                    g.DrawRectangle(_blackPen, new Rectangle(chipX, chipY, _chipWidth, PinCount * _verticalSpacing));

                    //name
                    float textX = chipX + (_chipWidth / 2);
                    float textY = (_textVerticalSpacing * 2f) + (((halfPinCount - Device.Length) * _textVerticalSpacing)/2);
                    for (int i = 0; i < Device.Length; i++)
                    {
                        float textSize = g.MeasureString((i + 1).ToString(), _drawFontDevice).Width;
                        g.DrawString(Device.Substring(i,1), _drawFontDevice, _blackBrush, textX-(textSize/2), textY+(i* _textVerticalSpacing));
                    }
                    //pin numbers
                    int pinY = chipY + _verticalSpacing;
                    for(int i = 0; i < PinCount/2; i++)
                    {
                        g.DrawString((i+1).ToString(), _drawFont, _blackBrush, chipX + 5, pinY);
                        float textSize = g.MeasureString((i + 1).ToString(), _drawFont).Width;
                        g.DrawString((i + 1).ToString(), _drawFont, _blackBrush, chipX + _chipWidth - 5 - textSize, pinY);
                        pinY += _verticalSpacing;
                    }
                }
            }
        }

        private void panelMain_Click(object sender, EventArgs e)
        {
            

           
        }
    }
}
