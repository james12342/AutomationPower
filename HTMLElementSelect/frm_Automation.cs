using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HTMLElementSelect
{
    public partial class frm_Automation : Form
    {
        public frm_Automation()
        {
            InitializeComponent();
        }

        private void bt_Go_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate(txt_URL.Text); //! Navigate to the CodeProject
            webBrowser1.ContextMenuStrip = contextMenuStrip1;    //! Set our ContextMenuStrip
            webBrowser1.IsWebBrowserContextMenuEnabled = false;  //! Disable the default IE ContextMenu
            webBrowser1.ScriptErrorsSuppressed = true;
        }
    }
}
