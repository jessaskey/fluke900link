using System;
using System.Collections.Generic;
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

        private static bool _useDialog = false;
        private static bool _useStatus = true;


        private static RadLabelElement _statusLabel = null;
        //private static RadWaitingBarElement _statusWaiting = null;

        private static IProgress<ProgressAction> _updateProgressPopup = new Progress<ProgressAction>(data => ApplyProgressActionPopup(data));
        //private static IProgress<ProgressAction> _updateProgressBackground = new Progress<ProgressAction>(data => ApplyProgressActionBackground(data));

        public static void SetUIComponents(RadLabelElement labelElement, RadWaitingBarElement waitingElement)
        {
            _statusLabel = labelElement;
            //_statusWaiting = waitingElement;
        }

        //public static void ShowProgressBackground(string message)
        //{
        //    ProgressAction pa = new ProgressAction();
        //    pa.TopMessage = message;
        //    pa.StartProgress = true;
        //    _updateProgressBackground.Report(pa);
        //}

        public static void Start(string message)
        {
            Cursor.Current = Cursors.WaitCursor;

            ProgressAction pa = new ProgressAction();
            pa.TopMessage = message;
            pa.StartProgress = true;
            _updateProgressPopup.Report(pa);
            
            Application.DoEvents();
        }

        public static void Start(string title, string message)
        {
            Cursor.Current = Cursors.WaitCursor;

            ProgressAction pa = new ProgressAction();
            pa.Title = title;
            pa.TopMessage = message;
            pa.StartProgress = true;
            _updateProgressPopup.Report(pa);
            Application.DoEvents();
        }

        public static void Start(string title, string topMessage, string bottomMessage)
        {
            Cursor.Current = Cursors.WaitCursor;

            ProgressAction pa = new ProgressAction();
            pa.Title = title;
            pa.TopMessage = topMessage;
            pa.BottomMessage = bottomMessage;
            pa.StartProgress = true;
            _updateProgressPopup.Report(pa);

            Application.DoEvents();
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
            ProgressAction pa = new ProgressAction();
            pa.TopMessage = message;
            pa.StartProgress = false;
            _updateProgressPopup.Report(pa);
            //_updateProgressBackground.Report(pa);
            Cursor.Current = Cursors.Default;
            Application.DoEvents();
        }

        public static void Stop()
        {
            ProgressAction pa = new ProgressAction();
            pa.StartProgress = false;
            _updateProgressPopup.Report(pa);
            //_updateProgressBackground.Report(pa);
            Cursor.Current = Cursors.Default;
            Application.DoEvents();
        }



        private static void ApplyProgressActionPopup(ProgressAction summary)
        {
            if (_useDialog)
            {
                if (summary.StartProgress)
                {
                    ProgressDialog.Instance.StartProgress(summary.Title, summary.TopMessage);
                }
                else
                {
                    ProgressDialog.Instance.StopProgress();

                }
            }

            if (_useStatus)
            {
                if (_statusLabel != null)
                {
                    if (summary.TopMessage == null)
                    {
                        _statusLabel.Text = "Ready...";
                    }
                    else
                    {
                        _statusLabel.Text = summary.TopMessage;
                    }
                }
            }
        }

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
