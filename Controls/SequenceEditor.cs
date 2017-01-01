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

using Fluke900Link.Containers;
using Telerik.WinControls.UI.Docking;

namespace Fluke900Link.Controls
{
    public partial class SequenceEditor : DocumentEditor
    {
        ProjectSequenceFile _sequence = null;

        public SequenceEditor()
        {
            InitializeComponent();
        }

        public bool LoadSequence(ProjectSequenceFile sequence)
        {
            _sequence = sequence;
            this.Text = _sequence.FileNameOnly;
            this.ToolTipText = _sequence.PathFileName;
            this.Name = _sequence.PathFileName;

            this.OpenDocumentForEditing(_sequence.PathFileName);

            return true;
        }
    }
}
