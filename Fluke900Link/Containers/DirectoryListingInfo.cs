using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900Link.Containers
{
    public class DirectoryListingInfo
    {


        private List<Tuple<string, string>> _files = new List<Tuple<string, string>>();
        private long _bytesFree = 0;
        private long _bytesUsed = 0;
        private string _errorMessage = "";
        private Color _textColor = Color.Black;
        private bool _fontBold = false;
        private string _directory = "";
        private bool _readOnly = false;

        public List<Tuple<string, string>> Files { get { return _files; } }
        public long BytesFree { get { return _bytesFree;}  set { _bytesFree = value;} }
        public long BytesUsed { get { return _bytesUsed; } set { _bytesUsed = value; } }
        public string ErrorMessage { get { return _errorMessage; } set { _errorMessage = value; } }
        public Color TextColor { get { return _textColor; } set { _textColor = value; } }
        public bool FontBold { get { return _fontBold; } set { _fontBold = value; } }
        public string Directory { get { return _directory; } set { _directory = value; } }
        public bool ReadOnly { get { return _readOnly; } set { _readOnly = value; } }

        public bool FileExists(string filename)
        {
            if (_files.Where(t => t.Item1 == filename).FirstOrDefault() != null)
            {
                return true;
            }
            return false;
        }
    }
}
