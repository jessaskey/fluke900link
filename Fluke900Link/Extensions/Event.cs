using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900Link.Extensions
{

    /// <summary>Extension methods for EventHandler-type delegates.</summary>
    public static class EventExtensions
    {
        /// <summary>
        /// Safely raises any EventHandler event asynchronously.
        /// </summary>
        /// <param name="sender">The object raising the event (usually this).</param>
        /// <param name="e">The EventArgs for this event.</param>
        public static void Raise(this MulticastDelegate thisEvent, object sender, EventArgs e)
        {
            EventHandler uiMethod;
            ISynchronizeInvoke target;
            AsyncCallback callback = new AsyncCallback(EndAsynchronousEvent);

            foreach (Delegate d in thisEvent.GetInvocationList())
            {
                uiMethod = d as EventHandler;
                if (uiMethod != null)
                {
                    target = d.Target as ISynchronizeInvoke;
                    if (target != null) target.BeginInvoke(uiMethod, new[] { sender, e });
                    else uiMethod.BeginInvoke(sender, e, callback, uiMethod);
                }
            }
        }

        private static void EndAsynchronousEvent(IAsyncResult result)
        {
            ((EventHandler)result.AsyncState).EndInvoke(result);
        }
    }
}
