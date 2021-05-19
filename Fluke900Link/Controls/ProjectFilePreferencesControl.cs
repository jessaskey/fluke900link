using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fluke900;

namespace Fluke900Link.Controls
{

    public partial class ProjectFilePreferencesControl : UserControl
    {

        private FileLocationCopyBehavior _fileLocationCopyBehavior = FileLocationCopyBehavior.System;

        public ProjectFilePreferencesControl()
        {
            InitializeComponent();
        }

        public FileLocationCopyBehavior ProjectFileCopyBehavior 
        {
            get
            {
                return _fileLocationCopyBehavior;
            }
            set
            {
                _fileLocationCopyBehavior = value;
                radioButtonFileCopySystem.Checked = true;
                switch (_fileLocationCopyBehavior)
                {
                    case FileLocationCopyBehavior.System:
                        radioButtonFileCopySystem.Checked = true;
                        break;
                    case FileLocationCopyBehavior.Cartridge:
                        radioButtonFileCopyCartridge.Checked = true;
                        break;
                    case FileLocationCopyBehavior.SystemCartridgeDefault:
                        radioButtonFileCopyCartridgeThenSystem.Checked = true;
                        break;
                    case FileLocationCopyBehavior.Optimized:
                        radioButtonFileCopySplit.Checked = true;
                        break;
                }
            }
        }

        private void radioButton_CheckChanged(object sender, EventArgs e)
        {
            if (radioButtonFileCopyCartridge.Checked)
            {
                _fileLocationCopyBehavior = FileLocationCopyBehavior.Cartridge;
            }
            else if (radioButtonFileCopySystem.Checked)
            {
                _fileLocationCopyBehavior = FileLocationCopyBehavior.System;
            }
            else if (radioButtonFileCopyCartridgeThenSystem.Checked)
            {
                _fileLocationCopyBehavior = FileLocationCopyBehavior.SystemCartridgeDefault;
            }
            else
            {
                _fileLocationCopyBehavior = FileLocationCopyBehavior.Optimized;
            }
        }

    }
}
