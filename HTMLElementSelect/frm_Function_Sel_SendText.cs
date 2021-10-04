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
    public partial class frm_Function_Sel_SendText : Form
    {
        public frm_Function_Sel_SendText()
        {
            InitializeComponent();
        }

        private void bt_SaveStep_Click(object sender, EventArgs e)
        {
            if (txt_XPath.Text == "" & CurrentisDynamicElementXPath == "False")
            {
                MessageBox.Show("Xpath value is empty");
                return;
            }
            if (txt_SendValue.Text == "")
            {
                MessageBox.Show("Send Value is empty,please fill in");
                txt_SendValue.Focus();
                return;
            }
            SaveStep();
        }
    }
}
