using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fluke900.Containers;
using Fluke900Link.Containers;

namespace Fluke900Link
{
    public partial class ShowExceptionDialog : Form
    {
        public ShowExceptionDialog()
        {
            InitializeComponent();

            foreach (AppException ae in ApplicationGlobals.Exceptions.OrderByDescending(a => a.ThrownDateTime))
            {
                ListViewItem item = new ListViewItem(ae.ThrownDateTime.ToString());
                item.SubItems.Add(ae.Message);
                item.Tag = ae;
                listViewException.Items.Add(item);
            }
        }

        private void listViewException_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewException.SelectedItems.Count == 1)
            {
                Exception ex = listViewException.SelectedItems[0].Tag as Exception;
                propertyGrid1.SelectedObject = ex;
            }
        }


    }
}
