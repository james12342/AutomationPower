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
    public partial class frm_Step : Form
    {
        public frm_Step()
        {
            InitializeComponent();
        }

        public  void LoadFunctionNames()
        {
            int f_count = frm_Power_Automation.List_Functions.Count();
            for (int i = 0; i <= f_count - 1; i++)
            {
                toolStripCmb_StepFunctions.Items.Add(frm_Power_Automation.List_Functions[i].ToString());
            }
        }
        private void frm_Step_Load(object sender, EventArgs e)
        {
            LoadFunctionNames();
        }
    }
}
