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
    public partial class LibraryEditor : DocumentWindow
    {
        ProjectLibraryFile _library = null;
        private bool _modified = false;

        public LibraryEditor()
        {
            InitializeComponent();
        }

        public bool LoadLibrary(ProjectLibraryFile library)
        {
            _library = library;
            this.Text = _library.FileNameOnly;
            this.ToolTipText = _library.PathFileName;
            this.Name = _library.PathFileName;

            return true;
        }
    }
}
