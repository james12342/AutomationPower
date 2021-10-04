using Sikuli4Net.sikuli_REST;
using Sikuli4Net.sikuli_UTIL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoGetRedPackages
{
    public partial class Form1 : Form
    {

        public static string App_Path = System.IO.Directory.GetCurrentDirectory();
     
          
        public Form1()
        {
            InitializeComponent();
        }

        private void bt_Start_Click(object sender, EventArgs e)
        {
            APILauncher launch = new APILauncher(true);
            launch.Start();

            string hongbao_coming_Img = System.IO.Directory.GetCurrentDirectory() + "\\img\\wechat\\hongbao_coming.jpg";
            Sikuli4Net.sikuli_REST.Pattern P_hongbao_coming_Img = new Pattern(hongbao_coming_Img);

            string hongbao_someoneopened_Img = System.IO.Directory.GetCurrentDirectory() + "\\img\\wechat\\hongbao_someoneopened.jpg";
            Sikuli4Net.sikuli_REST.Pattern P_hongbao_someoneopened_Img = new Pattern(hongbao_someoneopened_Img);

         
        
            Sikuli4Net.sikuli_REST.Screen scr = new Sikuli4Net.sikuli_REST.Screen();
            if (!scr.Exists(P_hongbao_coming_Img)) 
            {
                listBox1.Items.Add("nOT FOUND");
            }
   

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
