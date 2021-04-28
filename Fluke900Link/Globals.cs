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
        public static string WIKIPAGE_URL = "https://github.com/jessaskey/fluke900link/wiki";

        public static List<AppException> Exceptions = new List<AppException>();
        public static string LastDirectoryBrowse = null;

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
