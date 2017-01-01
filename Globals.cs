using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Fluke900Link.Containers;
using Fluke900Link.Controls;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Docking;

namespace Fluke900Link
{
    public static class Globals
    {
        public static string DOCK_CONFIGURATION_FILE = "dock.xml";
        public static string TEMPLATES_FOLDER = "_Templates";
        public static string DOCUMENTS_FOLDER = "_Documents";
        public static string EXAMPLES_FOLDER = "_Examples";
        public static string CARTRIDGE_TEST_FILENAME = "WRITETEST.TXT:CART";
        public static int    NEW_DOCUMENT_COUNTER = 1;
        public static string ADMIN_EMAIL = "jess@askey.org";
        public static string CLIENTMSG_URL = "http://apps.askey.org/fluke900/clientmessage.txt";

        public static List<AppException> Exceptions = new List<AppException>();
        public static string LastDirectoryBrowse = null;

        public static class UIElements
        {          
            //MainForm
            public static Splash Splash = null;
            public static MainForm MainForm = null;

            //toolstip areas for docking
            public static ToolTabStrip LeftSideStrip = null;
            public static ToolTabStrip RightSideStrip = null;
            public static ToolTabStrip BottomSideStrip = null;
            public static ToolTabStrip FillStrip = null;

            //toolboxes and documents
            public static DirectoryEditorControl DirectoryEditorLocal = null;
            public static DirectoryEditorControl DirectoryEditorCartridge = null;
            public static DirectoryEditorControl DirectoryEditorSystem = null;
            public static TextEditorControl TerminalFormattedWindow = null;
            public static TextEditorControl TerminalRawWindow = null;
            public static TerminalSend TerminalSendWindow = null;
            public static LibraryBrowser LibraryBrowser = null;
            public static SolutionExplorer SolutionExplorer = null;
            public static DeveloperConsole DeveloperConsole = null;

            //imageLists
            public static ImageList ImageList16x16 = null;
        }

        public static string GetClientMessage()
        {
            string clientMessage = "";
            try
            {
                WebRequest request = WebRequest.Create(CLIENTMSG_URL);
                request.Timeout = 3000;
                using (WebResponse response = request.GetResponse())
                {
                    HttpWebResponse httpWebResponse = response as HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        using (var stream = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding(1252)))
                        {
                            clientMessage = stream.ReadToEnd();
                        }
                    }
                }
            }
            catch { }
            return clientMessage;
        }
    }
}
