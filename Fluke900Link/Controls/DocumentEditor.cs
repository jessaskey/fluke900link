using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

using ScintillaNET;
using Fluke900Link.Lexers;
using Telerik.WinControls.UI.Docking;
using Fluke900Link.Containers;

namespace Fluke900Link.Controls
{
    public partial class DocumentEditor : DockContentEx
    {
        protected bool _disableModifiedFlagUpdates = false;
        protected bool _modified = false;
        protected string _pathFileName = "";
        protected string _fileName = "";
        protected long _lastTextLength = 0;

        private Fluke900Lexer _flukeLexer = null;

        /// <summary>
        /// 
        /// </summary>
        public DocumentEditor()
        {
            InitializeComponent();
            scintilla.TextChanged += scintilla_TextChanged;
            scintilla.StyleNeeded += scintilla_StyleNeeded;

            scintilla.StyleResetDefault();
            scintilla.Styles[Style.Default].Font = "Consolas";
            scintilla.Styles[Style.Default].Size = 10;
            scintilla.StyleClearAll();

            scintilla.Styles[Fluke900Lexer.StyleDefault].ForeColor = Color.Black;
            scintilla.Styles[Fluke900Lexer.StyleKeyword].ForeColor = Color.Blue;
            scintilla.Styles[Fluke900Lexer.StyleNumber].ForeColor = Color.Purple;
            scintilla.Styles[Fluke900Lexer.StyleString].ForeColor = Color.DarkGreen;
            scintilla.Styles[Fluke900Lexer.StyleComment].ForeColor = Color.Gray;
            scintilla.Styles[Fluke900Lexer.StyleInvalid].ForeColor = Color.Red;

            scintilla.Lexer = Lexer.Container;
        }

        private void scintilla_StyleNeeded(object sender, StyleNeededEventArgs e)
        {
            if (_flukeLexer != null)
            {
                var startPos = scintilla.GetEndStyled();
                var endPos = e.Position;
                _flukeLexer.Style(scintilla, startPos, endPos);
            }
        }

        public bool IsModified { get { return _modified; } }
        public string Filename { get { return _fileName; } }
        public string PathFilename { get { return _pathFileName; } }

        public bool SaveDocument()
        {
            //show a warning if the file is an 'example' file
            if (_pathFileName.Contains("_Examples"))
            {
                MessageBox.Show("It looks like you are trying to save a file that is in the supplied '_Examples' folder. You can save these with the same filename but the file will be overwritten next the time application starts. If you save in the '_Examples' folder using a different filename, the file may still be removed. I highly suggest that you save the file somewhere else. :-)", "File Save Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (File.Exists(_pathFileName))
            {
                try
                {
                    File.WriteAllText(_pathFileName, scintilla.Text);
                    Text = Text.Replace("*", "");
                    _modified = false;
                    return true;
                }
                catch (Exception ex)
                {
                    Globals.Exceptions.Add(new Containers.AppException(ex));
                    MessageBox.Show(ex.Message, "File Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                return SaveDocumentAs();
            }
            return false;
        }

        public bool SaveDocumentAs()
        {
            SaveFileDialog sd = new SaveFileDialog();
            sd.FileName = _fileName;
            sd.InitialDirectory = Utilities.GetBrowseDirectory();
            DialogResult dr = sd.ShowDialog();
            if (dr == DialogResult.Cancel)
            {
                return false;
            }
            try
            {
                File.WriteAllText(sd.FileName, scintilla.Text);
                _pathFileName = sd.FileName;
                Globals.LastDirectoryBrowse = Path.GetDirectoryName(sd.FileName);
                _fileName = Path.GetFileName(sd.FileName);
                _modified = false;
                Text = _fileName;
                SetLexer();
                return true;
            }
            catch (Exception ex)
            {
                Globals.Exceptions.Add(new Containers.AppException(ex));
            }
            return false;
        }

        public bool CreateNewDocument(string filename, string content)
        {
            _pathFileName = "";
            _fileName = filename;
            Text = filename + "*";
            ToolTipText = "";
            _modified = false;
            if (!string.IsNullOrEmpty(content))
            {
                scintilla.Text = content;
            }

            SetLexer();

            return true;
        }

        private void scintilla_TextChanged(object sender, EventArgs e)
        {
            if (!_disableModifiedFlagUpdates)
            {
                _modified = true;
                if (!Text.Contains("*"))
                {
                    Text += "*";
                }
            }

            if (Properties.Settings.Default.Editor_LineNumbers)
            {
                // Did the number of characters in the line number display change?
                // i.e. nnn VS nn, or nnnn VS nn, etc...
                var currentLineCount = scintilla.Lines.Count.ToString().Length;
                if (currentLineCount != _lastTextLength)
                {
                    // Calculate the width required to display the last line number
                    // and include some padding for good measure.
                    const int padding = 2;
                    scintilla.Margins[0].Width = scintilla.TextWidth(ScintillaNET.Style.LineNumber, new string('9', currentLineCount)) + padding;
                    _lastTextLength = currentLineCount;
                }
            }
        }


        public bool OpenDocumentForEditing(string pathFileName)
        {
            try {
                _pathFileName = pathFileName;
                _fileName = Path.GetFileName(pathFileName);
                this.Text = _fileName;
                this.ToolTipText = pathFileName;
                this.Name = pathFileName;
                _disableModifiedFlagUpdates = true;
                scintilla.AppendText(File.ReadAllText(pathFileName));
                _disableModifiedFlagUpdates = false;
                SetLexer();
                return true;
            }
            catch{}
            return false;
        }

        private void SetLexer()
        {
            switch (Path.GetExtension(_fileName).ToLower())
            {
                case ".lib":
                    _flukeLexer = new Fluke900Lexer(KnownFileType.Lib);
                    break;
                case ".loc":
                    _flukeLexer = new Fluke900Lexer(KnownFileType.Loc);
                    break;
                case ".seq":
                    _flukeLexer = new Fluke900Lexer(KnownFileType.Seq);
                    break;
                default:
                    _flukeLexer = null;
                    break;
            }
        }

        private void scintilla_InsertCheck(object sender, InsertCheckEventArgs e)
        {
            if ((e.Text.EndsWith("\r") || e.Text.EndsWith("\n")))
            {
                int startPos = scintilla.Lines[scintilla.LineFromPosition(scintilla.CurrentPosition)].Position;
                int endPos = e.Position;
                string curLineText = scintilla.GetTextRange(startPos, (endPos - startPos)); //Text until the caret so that the whitespace is always equal in every line.

                Match indent = Regex.Match(curLineText, "^[ \\t]*");
                e.Text = (e.Text + indent.Value);
                if (Regex.IsMatch(curLineText, "{\\s*$"))
                {
                    e.Text = (e.Text + "\t");
                }
            }
        }
    }
}
