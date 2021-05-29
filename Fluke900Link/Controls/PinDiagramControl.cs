using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fluke900.Containers;
using Fluke900;

namespace Fluke900Link.Controls
{
    public partial class PinDiagramControl : UserControl
    {
        public ProjectLocation ProjectLocation { get; set; }

        // private int _titlePadding { get; set; } = 20;
        //private int _edgePadding { get; set; } = 20;

        private const int _legendY = 30;
        private const int _chipTopOffset = 50;
        private int _valuePadding { get; set; } = 18;
        private int _verticalPinSpacing { get; set; } = 20;
        private int _textVerticalSpacing { get; set; } = 18;
        private int _chipWidth { get; set; } = 90;
        private int _chipTextPadding { get; set; } = 4;

        private Pen _blackPen = new Pen(Color.FromArgb(255, 0, 0, 0), 1);
        private Font _drawFontDevice = new System.Drawing.Font("Consolas", 14, FontStyle.Bold);
        private Font _drawFont = new System.Drawing.Font("Consolas", 14);
        private SolidBrush _blackBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
        private SolidBrush _whiteBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);
        private SolidBrush _greyBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Gray);
        private SolidBrush _selectedBrush = new System.Drawing.SolidBrush(System.Drawing.Color.LimeGreen);

        //public List<int> Values { get; set; }
        //public Type ValueType { get; set; }
        //public string Title { get; set; }
        //public string Device { get; set; }
        //public List<string> EnumValues { get; set; }

        public Font DiagramFont { get; set; }

        public PinDiagramControl()
        {
            InitializeComponent();
            PositionLabels();
        }

        private void PositionLabels()
        {
            int currentX = (_chipWidth/2) + _valuePadding;
            labelFloatCheck1.Location = new Point((Width / 2) - currentX - (_valuePadding*2),_legendY);
            labelFloatCheck2.Location = new Point((Width / 2) + currentX,_legendY);
            currentX += _valuePadding * 4;
            labelActivity1.Location = new Point((Width / 2) - currentX - (_valuePadding * 3), _legendY);
            labelActivity2.Location = new Point((Width / 2) + currentX, _legendY);
            currentX += _valuePadding * 5;
            labelTriggerWord1a.Location = new Point((Width / 2) - currentX - (_valuePadding * 3), _legendY);
            labelTriggerWord1b.Location = new Point((Width / 2) + currentX, _legendY);
            currentX += _valuePadding * 4;
            labelTriggerWord2a.Location = new Point((Width / 2) - currentX - (_valuePadding * 3), _legendY);
            labelTriggerWord2b.Location = new Point((Width / 2) + currentX, _legendY);
            currentX += _valuePadding * 4;
            labelGate1.Location = new Point((Width / 2) - currentX - (_valuePadding * 3), _legendY);
            labelGate2.Location = new Point((Width / 2) + currentX, _legendY);

        }

        private void panelMain_Paint(object sender, PaintEventArgs e)
        {
            int xCenter = Width / 2;
            if (ProjectLocation != null)
            {
                using (Graphics g = e.Graphics)
                {
                    int halfPinCount = ProjectLocation.PinDefinitions.Count / 2;

                    //draw the chip rectangle
                    int chipY= _chipTopOffset;
                    g.FillRectangle(_blackBrush, new Rectangle(xCenter - (_chipWidth/2), chipY, _chipWidth, (halfPinCount * _verticalPinSpacing) + (_chipTextPadding *2)));

                    //name
                    float textY = chipY + (((halfPinCount - ProjectLocation.DeviceName.Length) * _textVerticalSpacing)/2);
                    for (int i = 0; i < ProjectLocation.DeviceName.Length; i++)
                    {
                        float textSize = g.MeasureString((i + 1).ToString(), _drawFontDevice).Width;
                        g.DrawString(ProjectLocation.DeviceName.Substring(i,1), _drawFontDevice, _whiteBrush, xCenter - (textSize/2), textY+(i* _textVerticalSpacing));
                    }
                    //pin numbers
                    int pinY = chipY + _chipTextPadding;
                    for(int i = 0; i < halfPinCount; i++)
                    {
                        g.DrawString((i+1).ToString(), _drawFont, _greyBrush, xCenter - (_chipWidth/2) + _chipTextPadding, pinY);
                        float textSize = g.MeasureString((i + 1).ToString(), _drawFont).Width;
                        g.DrawString((i + 1).ToString(), _drawFont, _greyBrush, xCenter + (_chipWidth / 2) - _chipTextPadding - textSize, pinY);

                        //float check is first 
                        int currentX = (_chipWidth / 2) + _valuePadding;
                        currentX += RenderEnum(g, typeof(FloatCheckDefinition), xCenter, currentX, pinY, halfPinCount, new List<string>() { "X", "Z" });
                        currentX += _valuePadding;
                        //pin activity is next
                        currentX += RenderEnum(g, typeof(PinActivityDefinition), xCenter, currentX, pinY, halfPinCount, new List<string>() { "F", "X", "H", "L", "A" });
                        currentX += _valuePadding;
                        //trigger def
                        currentX += RenderEnum(g, typeof(TriggerWord1Definition), xCenter, currentX, pinY, halfPinCount, new List<string>() { "1", "0", "X" });
                        currentX += _valuePadding;
                        currentX += RenderEnum(g, typeof(TriggerWord2Definition), xCenter, currentX, pinY, halfPinCount, new List<string>() { "1", "0", "X" });
                        currentX += _valuePadding;
                        //gate def
                        currentX += RenderEnum(g, typeof(GatePinDefinition), xCenter, currentX, pinY, halfPinCount, new List<string>() { "1", "0", "X" });

                        pinY += _verticalPinSpacing;
                    }

                }
            }
        }

        private int RenderEnum(Graphics g, Type enumType, int xCenter, int xOffset, int chipY, int halfPinCount, List<string> letters)
        {
            int valueCount = Enum.GetValues(enumType).Length;
            int pinY = chipY + _chipTextPadding;
            List<int> values = ProjectLocation.GetPinValues(enumType);
            for (int i = 0; i < halfPinCount; i++)
            {
                //each enum value
                for (int j = 0; j < valueCount; j++)
                {
                    //left side is funky, needs to inverse so math is more complex
                    float pinX1 = xCenter - (valueCount * _valuePadding) - xOffset + (j * _valuePadding);
                    float pinX2 = xCenter + xOffset + (j * _valuePadding);
                    SizeF stringSize = g.MeasureString(letters[j], _drawFont);
                    if (values[i] == j)
                    {
                        //this is a selected value, highlight it green
                        g.FillRectangle(_selectedBrush, new RectangleF(pinX1, pinY, stringSize.Width, stringSize.Height));
                    }
                    if (values[i + halfPinCount] == j)
                    {
                        //this is a selected value, highlight it green
                        g.FillRectangle(_selectedBrush, new RectangleF(pinX2, pinY, stringSize.Width, stringSize.Height));
                    }
                    g.DrawString(letters[j], _drawFont, _blackBrush, pinX1, pinY);
                    g.DrawString(letters[j], _drawFont, _blackBrush, pinX2, pinY);
                }
            }
            return valueCount * _valuePadding;
        }

        private void panelMain_Click(object sender, EventArgs e)
        {
            

           
        }

        private void panelMain_Resize(object sender, EventArgs e)
        {
            PositionLabels();
            Invalidate();
        }
    }
}
