using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScintillaNET;

namespace Fluke900Link
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Scintilla.SetDestroyHandleBehavior(true);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            

            Splash splash = new Splash();
            splash.AutoClose = true;
            Application.Run(splash);
        }
    }
}
