using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Fluke900Link.Containers;
using Fluke900Link.Dialogs;
using Telerik.WinControls.UI;

namespace Fluke900Link
{
    public static class ProgressManager
    {

        private static bool _useDialog = true;
        private static bool _useStatus = true;


        private static RadLabelElement _statusLabel = null;
        private static ToolStripStatusLabel _statusLabel2 = null;

        //private static RadWaitingBarElement _statusWaiting = null;
        //private static IProgress<ProgressAction> _updateProgressPopup = new Progress<ProgressAction>(data => ApplyProgressActionPopup(data));
        //private static IProgress<ProgressAction> _updateProgressBackground = new Progress<ProgressAction>(data => ApplyProgressActionBackground(data));

        public static void SetUIComponents(RadLabelElement labelElement, RadWaitingBarElement waitingElement)
        {
            _statusLabel = labelElement;
            //_statusWaiting = waitingElement;
        }

        public static void SetUIComponents2(ToolStripStatusLabel labelElement, ToolStripProgressBar waitingElement)
        {
            _statusLabel2 = labelElement;
            //_statusWaiting = waitingElement;
        }

        //public static void ShowProgressBackground(string message)
        //{
        //    ProgressAction pa = new ProgressAction();
        //    pa.TopMessage = message;
        //    pa.StartProgress = true;
        //    _updateProgressBackground.Report(pa);
        //}

        public static void Start(string topMessage)
        {
            Start("Working...", topMessage);
        }

        public static void Start(string title, string topMessage)
        {
            Start(title, topMessage, null);
        }

        public static void Start(string title, string topMessage, string bottomMessage)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (_useDialog)
            {
                ProgressDialog.Instance.StartProgress(title, topMessage);
            }

            if (_useStatus)
            {
                if (_statusLabel != null)
                {
                    _statusLabel.Text = topMessage;
                }
                if (_statusLabel2 != null)
                {
                    _statusLabel2.Text = topMessage;
                }
            }
        }

        public static void ProgressChanged (object sender, ProgressChangedEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ProgressDialog.Instance.StartProgress(e.UserState.ToString());
        }

        //public static void UpdateStatus(string message)
        //{
        //    if (_statusLabel != null)
        //    {
        //        _statusLabel.Text = message;
        //    }
        //}

        public static void Stop(string message)
        {
            if (_useStatus)
            {
                if (message == null)
                {
                    if (_statusLabel != null)
                    {
                        _statusLabel.Text = "Ready...";
                    }
                    if (_statusLabel2 != null)
                    {
                        _statusLabel2.Text = "Ready...";
                    }
                }
                else
                {
                    if (_statusLabel != null)
                    {
                        _statusLabel.Text = message;
                    }
                    if (_statusLabel2 != null)
                    {
                        _statusLabel2.Text = message;
                    }
                }
            }
            if (_useDialog)
            {
                ProgressDialog.Instance.StopProgress();
            }
            Cursor.Current = Cursors.Default;
        }

        public static void Stop()
        {
            Stop(null);
        }



        //private static void ApplyProgressActionPopup(ProgressAction summary)
        //{
        //    if (_useDialog)
        //    {
        //        if (summary.StartProgress)
        //        {
        //            ProgressDialog.Instance.StartProgress(summary.Title, summary.TopMessage);
        //        }
        //        else
        //        {
        //            ProgressDialog.Instance.StopProgress();

        //        }
        //    }

        //    if (_useStatus)
        //    {
        //        if (_statusLabel != null)
        //        {
        //            if (summary.TopMessage == null)
        //            {
        //                _statusLabel.Text = "Ready...";
        //            }
        //            else
        //            {
        //                _statusLabel.Text = summary.TopMessage;
        //            }
        //        }
        //    }
        //}

        //private static void ApplyProgressActionBackground(ProgressAction summary)
        //{
        //    if (summary.StartProgress)
        //    {
        //        _statusWaiting.Enabled = true;
        //        _statusWaiting.Visibility = Telerik.WinControls.ElementVisibility.Visible;
        //        _statusWaiting.StartWaiting();
        //    }
        //    else
        //    {
        //        if (_statusWaiting != null)
        //        {
        //            _statusWaiting.Visibility  = Telerik.WinControls.ElementVisibility.Hidden;
        //            _statusWaiting.StopWaiting();
        //        }
        //        if (_statusLabel != null)
        //        {
        //            if (summary.CompletionText == null)
        //            {
        //                _statusLabel.Text = "Ready...";
        //            }
        //            else
        //            {
        //                _statusLabel.Text = summary.CompletionText;
        //            }
        //        }
        //    }
        //}
    }
}
