using HTMLElementSelect.Functions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HTMLElementSelect
{
    public partial class frm_DiagramAction : Form
    {
        public static string currentStepID = string.Empty;

        public frm_DiagramAction()
        {
            InitializeComponent();
        }

        private void frm_DiagramAction_Load(object sender, EventArgs e)
        {
            //this.Text = "Work flow:" + frm_Power_Automation.CurrentAutoName + " | Step:" + frm_Power_Automation.CurrentDiagram_SelectStepID;
            this.lbl_Message.Text = this.lbl_Message.Text + ":" + frm_Power_Automation.CurrentAutoName + " | Step:" + frm_Power_Automation.CurrentDiagram_SelectStepID+ " ?";
            currentStepID = frm_Power_Automation.CurrentDiagram_SelectStepID;
        }

     
        private void bt_DeleteFunction_Click(object sender, EventArgs e)
        {
           // string currentFunctionStepFile = frm_Power_Automation.CurrentFunctionsListExecuteJSONFile;

           // string targetFileSteps1 = currentFunctionStepFile.Replace(".json","1.json"); 

           // string targetFileStepsText = File.ReadAllText(currentFunctionStepFile);
           // JObject JOSteps = JObject.Parse(targetFileStepsText);
           // //get the current delete step id and replace it
           // //object►StepList►1►FunctionDetail►
           // string FunctionID = JOSteps.SelectToken("StepList")[int.Parse(currentStepID)-1].SelectToken("FunctionDetail").SelectToken("FunctionID").ToString();
           // string FunctionName = JOSteps.SelectToken("StepList")[int.Parse(currentStepID) - 1].SelectToken("FunctionDetail").SelectToken("FunctionName").ToString();
           // string FunctionExecuteJsonLocation = JOSteps.SelectToken("StepList")[int.Parse(currentStepID) - 1].SelectToken("FunctionDetail").SelectToken("FunctionExecuteJsonLocation").ToString();
           
           // //    "FunctionID": "FID_3_N_amazon_scrap_F_Click_Action",
           // //"FunctionName": "F_Click_Action_N_amazon_scrap_FID_3",
           // //"FunctionCategoryID": "",
           // //"FunctionCategoryName": "",
           // //"FunctionExecuteJsonLocation": "amazon scrap_STEP_3_Click Action_Function.json"


           // string needDeleteFunctionFile= frm_Power_Automation.App_Path+ "\\Functions\\"+ frm_Power_Automation.CurrentAutoName+"\\"+ JOSteps.SelectToken("StepList")[int.Parse(currentStepID)-1].SelectToken("FunctionDetail").SelectToken("FunctionExecuteJsonLocation").ToString(); 
           //// string toReplaceContnt =  "\"FunctionDetail\": \r\n\t  {\r\n        \"FunctionID\": \"FunctionID_SampleXXX_StepID"+ currentStepID + "\",\r\n        \"FunctionName\": \"FunctionName_SampleXXX_StepID"+ currentStepID + "\",\r\n        \"FunctionCategoryID\": \"FunctionCategoryID_SampleXXX_StepID"+ currentStepID + "\",\r\n        \"FunctionCategoryName\": \"FunctionCategoryName_SampleXXX_StepID"+ currentStepID + "\",\r\n        \"FunctionExecuteJsonLocation\": \"FunctionExecuteJsonLocation_SampleXXX_StepID"+ currentStepID + "\"\r\n      }";
           // targetFileStepsText = targetFileStepsText.Replace(FunctionID, "FunctionID_SampleXXX_StepID"+currentStepID);
            
           // targetFileStepsText = targetFileStepsText.Replace(FunctionExecuteJsonLocation, "FunctionExecuteJsonLocation_SampleXXX_StepID" + currentStepID);
           // targetFileStepsText = targetFileStepsText.Replace(FunctionName, "FunctionName_SampleXXX_StepID" + currentStepID);


           // File.WriteAllText(targetFileSteps1, targetFileStepsText);
           // File.Delete(currentFunctionStepFile);
           // //rename the steps file 
           // File.Move(targetFileSteps1, currentFunctionStepFile);
           // //remove the function file
           // File.Delete(needDeleteFunctionFile);

           // //update the Maxstep deduct one step
           // string text = File.ReadAllText(frm_Power_Automation.CurrentMax_StepFile);
           // string MaxStepValue = text.Split(':')[1];
           // int MaxStepValue_Update = int.Parse(MaxStepValue)-1;

           // text = text.Replace(MaxStepValue, MaxStepValue_Update.ToString());
           // File.WriteAllText(frm_Power_Automation.CurrentMax_StepFile, text);
            //end update maxstep
            LiteDB_Action.DeleteFuntionStepInAuto_Lite(frm_Power_Automation.CurrentAutoName, frm_Power_Automation.CurrentDiagram_SelectStepID);
            MessageBox.Show("Delete "+frm_Power_Automation.CurrentAutoName + " Step:" + frm_Power_Automation.CurrentDiagram_SelectStepID + " Sucessfully","Delete Function",MessageBoxButtons.OK);
            frm_Power_Automation a = new frm_Power_Automation();
            Application.Run(a);
        }

        private void bt_EditFunction_Click(object sender, EventArgs e)
        {

        }
    }
}
