using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace HTMLElementSelect
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("http://www.codeproject.com/"); //! Navigate to the CodeProject
            webBrowser1.ContextMenuStrip = contextMenuStrip1;    //! Set our ContextMenuStrip
            webBrowser1.IsWebBrowserContextMenuEnabled = false;  //! Disable the default IE ContextMenu
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            //! Screen coordinates
            Point ScreenCoord = new Point(MousePosition.X, MousePosition.Y);
            //! Browser coordinates
            Point BrowserCoord = webBrowser1.PointToClient(ScreenCoord);
            HtmlElement elem = webBrowser1.Document.GetElementFromPoint(BrowserCoord);

            //! Hide all menu items
            for (int i = 0; i < contextMenuStrip1.Items.Count; i++)
            {
                contextMenuStrip1.Items[i].Visible = false;
            }

            //! Show what we want to see.
            switch (elem.TagName)
            {
                case "A":
                    //! This is a link.. display the appropriate menu items
                    openToolStripMenuItem.Visible = true;
                    openInNewTabToolStripMenuItem.Visible = true;
                    openInNewWindowToolStripMenuItem.Visible = true;
                    break;
                case "IMG":
                    //! This is an image.. show our image menu items
                    saveImageToolStripMenuItem.Visible = true;
                    setAsDesktopWallpaperToolStripMenuItem.Visible = true;
                    break;
                default:
                    //! This is anywhere else
                    refreshToolStripMenuItem.Visible = true;
                    viewSourceToolStripMenuItem.Visible = true;
                    break;
            }
        }
    }
}
