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
        static void Main(string[] args)
        {
            Scintilla.SetDestroyHandleBehavior(true);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //arg passing for clickonce is a little different, we need to look for args in both
            //the normal args parameter above in Main, plus the ClickOnce path..
            List<string> passedArgs = new List<string>();
            if (args!= null) passedArgs.AddRange(args);
            if (AppDomain.CurrentDomain != null && AppDomain.CurrentDomain.SetupInformation != null && AppDomain.CurrentDomain.SetupInformation.ActivationArguments != null && AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData != null)
            {
                //seems that these are URI's, need to take off the 'file:///' prefix
                passedArgs.AddRange(AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData.Select(a=>a.Replace("file:///","")));
            }

            //Splash splash = new Splash();
            //splash.AutoClose = true;
            //splash.OpenArgs = passedArgs.ToArray();
            MainForm2 mainForm = new MainForm2();
            mainForm.OpenArgs = passedArgs.ToArray();
            Application.Run(mainForm);
        }
    }
}
