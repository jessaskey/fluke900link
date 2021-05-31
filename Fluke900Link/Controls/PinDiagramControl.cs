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
        private ProjectLocation _projectLocation = null;

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
        private Font _legendFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25f);
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
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
        }

        public void SetProjectLocation(ProjectLocation projectLocation)
        {
            _projectLocation = projectLocation;
            Invalidate();
            //DataUpdated();
        }
        //public void DataUpdated()
        //{
        //    if (_projectLocation != null)
        //    {
        //        int lastXPosition = (_chipWidth / 2) + _valuePadding;
        //        labelFloatCheck1.Location = new Point((Width / 2) - lastXPosition - (_valuePadding * 2), _legendY);
        //        labelFloatCheck2.Location = new Point((Width / 2) + lastXPosition, _legendY);
        //        lastXPosition += _valuePadding * 4;
        //        labelActivity1.Location = new Point((Width / 2) - lastXPosition - (_valuePadding * 3), _legendY);
        //        labelActivity2.Location = new Point((Width / 2) + lastXPosition, _legendY);

        //        labelTriggerWord1a.Visible = _projectLocation.TriggerEnabled;
        //        labelTriggerWord1b.Visible = _projectLocation.TriggerEnabled;
        //        labelTriggerWord2a.Visible = _projectLocation.TriggerEnabled;
        //        labelTriggerWord2b.Visible = _projectLocation.TriggerEnabled;
        //        if (_projectLocation.TriggerEnabled)
        //        {
        //            lastXPosition += _valuePadding * 5;
        //            labelTriggerWord1a.Location = new Point((Width / 2) - lastXPosition - (_valuePadding * 3), _legendY);
        //            labelTriggerWord1b.Location = new Point((Width / 2) + lastXPosition, _legendY);
        //            lastXPosition += _valuePadding * 4;
        //            labelTriggerWord2a.Location = new Point((Width / 2) - lastXPosition - (_valuePadding * 3), _legendY);
        //            labelTriggerWord2b.Location = new Point((Width / 2) + lastXPosition, _legendY);
        //        }

        //        labelGate1.Visible = _projectLocation.GateEnabled;
        //        labelGate2.Visible = _projectLocation.GateEnabled;
        //        if (_projectLocation.GateEnabled)
        //        {
        //            lastXPosition += _valuePadding * 4;
        //            labelGate1.Location = new Point((Width / 2) - lastXPosition - (_valuePadding * 3), _legendY);
        //            labelGate2.Location = new Point((Width / 2) + lastXPosition, _legendY);
        //        }
        //    }
        //}

        private void panelMain_Paint(object sender, PaintEventArgs e)
        {
            int xCenter = Width / 2;
            if (_projectLocation != null)
            {
                using (Graphics g = e.Graphics)
                {
                    int halfPinCount = _projectLocation.PinDefinitions.Count / 2;

                    //draw the chip rectangle
                    int chipY= _chipTopOffset;
                    g.FillRectangle(_blackBrush, new Rectangle(xCenter - (_chipWidth/2), chipY, _chipWidth, (halfPinCount * _verticalPinSpacing) + (_chipTextPadding *2)));

                    //name
                    float textY = chipY + (((halfPinCount - _projectLocation.DeviceName.Length) * _textVerticalSpacing)/2);
                    for (int i = 0; i < _projectLocation.DeviceName.Length; i++)
                    {
                        float textSize = g.MeasureString((i + 1).ToString(), _drawFontDevice).Width;
                        g.DrawString(_projectLocation.DeviceName.Substring(i,1), _drawFontDevice, _whiteBrush, xCenter - (textSize/2), textY+(i* _textVerticalSpacing));
                    }
                    //legend
                    int legendXPosition = (_chipWidth / 2) + _valuePadding;
                    g.DrawString("Float", _legendFont, _blackBrush, new Point((Width / 2) - legendXPosition - (_valuePadding * 2), _legendY));
                    g.DrawString("Float", _legendFont, _blackBrush, new Point((Width / 2) + legendXPosition, _legendY));
                    legendXPosition += _valuePadding * 4;
                    g.DrawString("Activity", _legendFont, _blackBrush, new Point((Width / 2) - legendXPosition - (_valuePadding * 3), _legendY));
                    g.DrawString("Activity", _legendFont, _blackBrush, new Point((Width / 2) + legendXPosition, _legendY));
                    if (_projectLocation.TriggerEnabled)
                    {
                        legendXPosition += _valuePadding * 5;
                        g.DrawString("Word1", _legendFont, _blackBrush, new Point((Width / 2) - legendXPosition - (_valuePadding * 3), _legendY));
                        g.DrawString("Word1", _legendFont, _blackBrush, new Point((Width / 2) + legendXPosition, _legendY));
                        legendXPosition += _valuePadding * 4;
                        g.DrawString("Word2", _legendFont, _blackBrush, new Point((Width / 2) - legendXPosition - (_valuePadding * 3), _legendY));
                        g.DrawString("Word2", _legendFont, _blackBrush, new Point((Width / 2) + legendXPosition, _legendY));
                    }
                    if (_projectLocation.GateEnabled)
                    {
                        legendXPosition += _valuePadding * 4;
                        g.DrawString("Gate", _legendFont, _blackBrush, new Point((Width / 2) - legendXPosition - (_valuePadding * 3), _legendY));
                        g.DrawString("Gate", _legendFont, _blackBrush, new Point((Width / 2) + legendXPosition, _legendY));
                    }
                    //pin numbers
                    int pinY = chipY + _chipTextPadding;
                    for(int i = 0; i < halfPinCount; i++)
                    {
                        g.DrawString((i+1).ToString(), _drawFont, _greyBrush, xCenter - (_chipWidth/2) + _chipTextPadding, pinY);
                        float textSize = g.MeasureString((i + 1).ToString(), _drawFont).Width;
                        g.DrawString((i + 1).ToString(), _drawFont, _greyBrush, xCenter + (_chipWidth / 2) - _chipTextPadding - textSize, pinY);
                        pinY += _verticalPinSpacing;
                    }

                    pinY = chipY + _chipTextPadding;
                    for (int i = 0; i < halfPinCount; i++)
                    {
                        //float check is first 
                        int currentX = (_chipWidth / 2) + _valuePadding;
                        currentX += RenderEnum(g, typeof(FloatCheckDefinition), xCenter, currentX, pinY, halfPinCount,i, new List<string>() { "X", "Z" });
                        currentX += _valuePadding;
                        //pin activity is next
                        currentX += RenderEnum(g, typeof(PinActivityDefinition), xCenter, currentX, pinY, halfPinCount,i, new List<string>() { "F", "X", "H", "L", "A" });
                        currentX += _valuePadding;
                        //trigger def
                        if (_projectLocation.TriggerEnabled)
                        {
                            currentX += RenderEnum(g, typeof(TriggerWord1Definition), xCenter, currentX, pinY, halfPinCount,i, new List<string>() { "1", "0", "X" });
                            currentX += _valuePadding;
                            currentX += RenderEnum(g, typeof(TriggerWord2Definition), xCenter, currentX, pinY, halfPinCount,i, new List<string>() { "1", "0", "X" });
                            currentX += _valuePadding;
                        }
                        //gate def
                        if (_projectLocation.GateEnabled)
                        {
                            currentX += RenderEnum(g, typeof(GatePinDefinition), xCenter, currentX, pinY, halfPinCount,i, new List<string>() { "1", "0", "X" });
                        }
                        pinY += _verticalPinSpacing;
                    }
                }
            }
        }

        private int RenderEnum(Graphics g, Type enumType, int xCenter, int xOffset, int chipY, int halfPinCount, int i, List<string> letters)
        {
            int valueCount = Enum.GetValues(enumType).Length;
            int minEnumValue = Enum.GetValues(enumType).Cast<int>().Min();
            int pinY = chipY + _chipTextPadding;
            List<int> values = _projectLocation.GetPinValues(enumType);
            //for (int i = 0; i < halfPinCount; i++)
            //{
                //each enum value
                for (int j = minEnumValue; j < valueCount; j++)
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
            //}
            return valueCount * _valuePadding;
        }

        private void panelMain_Click(object sender, EventArgs e)
        {
            Point point = panelMain.PointToClient(Cursor.Position);
            int halfPinCount = _projectLocation.PinDefinitions.Count / 2;
            int chipY = _chipTopOffset;
            int xCenter = Width / 2;
            Size stringSize = TextRenderer.MeasureText("X", _drawFont);
            //pin numbers
            int pinY = chipY + _chipTextPadding;

            for (int i = 0; i < halfPinCount; i++)
            {
                //float check is first 
                int currentX = (_chipWidth / 2) + _valuePadding;
                currentX += ClickCheckEnum(point, i, typeof(FloatCheckDefinition), xCenter, currentX, pinY, halfPinCount, stringSize);
                currentX += _valuePadding;
                //pin activity is next
                currentX += ClickCheckEnum(point, i, typeof(PinActivityDefinition), xCenter, currentX, pinY, halfPinCount, stringSize);
                currentX += _valuePadding;
                //trigger def
                if (_projectLocation.TriggerEnabled)
                {
                    currentX += ClickCheckEnum(point, i, typeof(TriggerWord1Definition), xCenter, currentX, pinY, halfPinCount, stringSize);
                    currentX += _valuePadding;
                    currentX += ClickCheckEnum(point, i, typeof(TriggerWord2Definition), xCenter, currentX, pinY, halfPinCount, stringSize);
                    currentX += _valuePadding;
                }
                //gate def
                if (_projectLocation.GateEnabled)
                {
                    currentX += ClickCheckEnum(point, i, typeof(GatePinDefinition), xCenter, currentX, pinY, halfPinCount, stringSize);
                }
                pinY += _verticalPinSpacing;
            }
        }

        private int ClickCheckEnum(Point hitPoint, int i, Type enumType, int xCenter, int xOffset, int chipY, int halfPinCount, Size hitSize)
        {
            bool hit = false;
            bool dataChanged = false;
            int valueCount = Enum.GetValues(enumType).Length;
            int minEnumValue = Enum.GetValues(enumType).Cast<int>().Min();
            int pinY = chipY + _chipTextPadding;
            //List<int> values = _projectLocation.GetPinValues(enumType);

                //each enum value
                for (int j = minEnumValue; j < valueCount; j++)
                {
                    //left side is funky, needs to inverse so math is more complex
                    int pinX1 = xCenter - (valueCount * _valuePadding) - xOffset + (j * _valuePadding);
                    int pinX2 = xCenter + xOffset + (j * _valuePadding);
                    Rectangle hitRect1 = new Rectangle(new Point(pinX1, pinY), hitSize);
                    Rectangle hitRect2 = new Rectangle(new Point(pinX2, pinY), hitSize);

                    if (hitRect1.Contains(hitPoint))
                    {
                        //hit on left
                        dataChanged =_projectLocation.SetPinValue(enumType, i, j);
                        hit = true;
                    }
                    else if (hitRect2.Contains(hitPoint))
                    {
                        //hit on right
                        dataChanged = _projectLocation.SetPinValue(enumType, i+halfPinCount, j);
                        hit = true;
                    }
                    if (hit)
                    {
                        break;
                    }
                }

            if (dataChanged)
            {
                Refresh();
            }
            return valueCount * _valuePadding;
        }

        private void panelMain_Resize(object sender, EventArgs e)
        {
            Refresh();
        }

        private void PinDiagramControl_Load(object sender, EventArgs e)
        {
//            System.Reflection.PropertyInfo aProp = typeof(System.Windows.Forms.Control).GetProperty(
//"DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
//            aProp.SetValue(panelMain, true, null);
        }
    }
}
