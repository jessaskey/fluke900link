using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fluke900Link.Helpers;
using ScintillaNET;

namespace Fluke900Link
{
    public class ProgramAsync
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //ProgramAsync p = new ProgramAsync(args);
            //p.Start();
            ProgramAsync p = new ProgramAsync(args);
            p.ExitRequested += p_ExitRequested;
            Task programStart = p.StartAsync();
            HandleExceptions(programStart);
            Application.Run();
        }

        private MainForm2 _mainForm;
        private string[] _args = null;

        private ProgramAsync(string[] args)
        {

            Scintilla.SetDestroyHandleBehavior(true);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SynchronizationContext.SetSynchronizationContext(new WindowsFormsSynchronizationContext());

            //arg passing for clickonce is a little different, we need to look for args in both
            //the normal args parameter above in Main, plus the ClickOnce path..
            List<string> passedArgs = new List<string>();
            if (args != null) passedArgs.AddRange(args);
            if (AppDomain.CurrentDomain != null && AppDomain.CurrentDomain.SetupInformation != null && AppDomain.CurrentDomain.SetupInformation.ActivationArguments != null && AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData != null)
            {
                //seems that these are URI's, need to take off the 'file:///' prefix
                passedArgs.AddRange(AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData.Select(a => a.Replace("file:///", "")));
            }

            _args = args;

            //_mainForm = new MainForm2();
            //_mainForm.OpenArgs = passedArgs.ToArray();
        }

        public async Task StartAsync()
        {
            //await _mainForm.InitializeAsync();
            //_mainForm.Show();
            using (Splash splashScreen = new Splash())
            {
                splashScreen.HideButtons = true;
                // If user closes splash screen, quit; that would also
                // be a good opportunity to set a cancellation token
                splashScreen.FormClosed += mainForm_FormClosed;
                splashScreen.Show();

                _mainForm = new MainForm2();
                _mainForm.FormClosed += mainForm_FormClosed;

                splashScreen.LinkMessage =  "Im looking for Fluke 900 parts to buy/trade... click here to email me!";
                await Task.Delay(5000);
                await _mainForm.InitializeAsync();
                splashScreen.TextMessage = "Loading Device Libraries...";
                await LibraryHelper.LoadReferenceLibrary();

                ProgressManager.Stop();
                // This ensures the activation works so when the
                // splash screen goes away, the main form is activated
                splashScreen.Owner = _mainForm;
                _mainForm.Show();

                splashScreen.FormClosed -= mainForm_FormClosed;
                splashScreen.Close();
            }
        }

        public event EventHandler<EventArgs> ExitRequested;

        static void p_ExitRequested(object sender, EventArgs e)
        {
            Application.ExitThread();
        }

        private static async void HandleExceptions(Task task)
        {
            try
            {
                await Task.Yield();
                await task;
            }
            catch (Exception ex)
            {
                //...log the exception, show an error to the user, etc.
                MessageBox.Show("Error", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        void mainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            OnExitRequested(EventArgs.Empty);
        }

        protected virtual void OnExitRequested(EventArgs e)
        {
            if (ExitRequested != null)
                ExitRequested(this, e);
        }


    }


}
