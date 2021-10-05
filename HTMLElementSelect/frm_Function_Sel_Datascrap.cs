using HTMLElementSelect.Functions;
using HTMLElementSelect.Objects;
using LiteDB;
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
using static HTMLElementSelect.Objects.Object_Lite_FunctionSteps;

namespace HTMLElementSelect
{
    public partial class frm_Function_Sel_Datascrap : Form
    {
        //       "FunctionID": "FunctionID_SampleXXX_StepID1",
        //"": "FunctionName_SampleXXX_StepID1",
        //"": "AutoCate_SEL_01",
        //"": "SEL",
        //"": "FunctionExecuteJsonLocation_SampleXXX_StepID1",
        //"": "FileNamewitoutExtension",
        //"": "FileName",
        //"": "CSV",
        //"": "Single",
        //" ": ",",

        //"FunctionDetail": 
        //    "isNavigateURL":"FunctionDetail_isNavigateURL_SampleXXX",
        //"NavigateURL":"FunctionDetail_NavigateURL_SampleXXX",	 
        //"Action":"Data Scrap",
        //   "ElementXPath":"FunctionDetail_ElementXPath_SampleXXX",
        //"ElementToColumnName":"FunctionDetail_ElementXPath_SampleXXX",
        //"ElementToColumnIndex":"FunctionDetail_ElementXPath_SampleXXX",
        //"ConditionCompareSymbol":"equal",  
        //"ConditionCompareValue":"FunctionDetail_AfterDoneWaitSeconds_SampleXXX",  
        //"isElementMedia":"FunctionDetail_isElementMedia_SampleXXX",
        //"MediaDownloadSaveName":"FunctionDetail_MediaDownloadSaveName_SampleXXX",
        //"MediaDownloadSaveLocation":"FunctionDetail_MediaDownloadSaveLocation_SampleXXX"

        public static string FunctionID = string.Empty;
        public static string FunctionName = string.Empty;
        public static string FunctionCategoryID = string.Empty;
        public static string FunctionCategoryName = string.Empty;
        public static string FunctionExecuteJsonLocation = string.Empty;
        public static string SaveFileFolder = string.Empty;
        public static string SaveFileName = string.Empty;
        public static string SaveFileFormat = string.Empty;
        public static string ScrapPages = string.Empty;
        public static string SaveFileColumnDelimiter = string.Empty;

        public static string App_Path = string.Empty;
        public static string CurrentFuntionFileToWrite = string.Empty;
        public static string CurrentFuntionFileFromWrite = string.Empty;
        public static string CurrentStepID = string.Empty;

        //details
        public static string isNavigateURL = string.Empty;
        public static string NavigateURL = string.Empty;
        public static string ElementXPath = string.Empty;
        public static string ElementToColumnName = string.Empty;
        public static string ElementToColumnIndex = string.Empty;
        public static string ConditionCompareSymbol = string.Empty;
        public static string ConditionCompareValue = string.Empty;
        public static string isElementMedia = string.Empty;
        public static string MediaDownloadSaveName = string.Empty;
        public static string MediaDownloadSaveLocation = string.Empty;


        public static string CurrentFunctionExecuteJsonShortName = string.Empty;
        public static string CurrentFunctionExecuteJsonLocation = string.Empty;
        public static string CurrentToCopyFunctionExecuteJsonLocation = string.Empty;

        //pages/multiple page
        public static string isMutiplePage = string.Empty;
        public static string PageElementXPath = string.Empty;

        public static int CurrentFunctionDetail_Index = 1;







        public frm_Function_Sel_Datascrap()
        {
            InitializeComponent();
        }

        private void txt_DataS_ScraperName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_DataS_ScraperName_Click(object sender, EventArgs e)
        {
            txt_DataS_ScraperName.Text = "";
        }

        private void txt_DataS_FileLocation_Click(object sender, EventArgs e)
        {
            txt_DataS_FileLocation.Text = "";
        }

        private void txt_DataS_ColumnName_Click(object sender, EventArgs e)
        {
            txt_DataS_ColumnName.Text = "";
        }

        private void txt_DataS_ConditionValue_Click(object sender, EventArgs e)
        {
            txt_DataS_ConditionValue.Text = "";
        }

        public string GetLatestJOSNFile(string DirPath, string spec)
        {
            string[] folders = Directory.GetDirectories(DirPath, "*", SearchOption.AllDirectories);
            var directory = Directory.GetDirectories(DirPath, "*", SearchOption.AllDirectories).OrderByDescending(d => new DirectoryInfo(d).CreationTime).Select(Path.GetFullPath);
            var file = Directory.GetFiles(DirPath, spec, SearchOption.AllDirectories).OrderByDescending(d => new FileInfo(d).CreationTime).Select(Path.GetFullPath);
            var selector = from i in file
                           orderby new FileInfo(i).CreationTime descending
                           select i;
            var last = selector.FirstOrDefault();
            return last;
        }

        public int GetStepMaxFromFile()

        {
            try
            {
                if (frm_Power_Automation.CurrentMax_StepFile == "")
                {
                    frm_Power_Automation.CurrentMax_StepFile = App_Path + "\\Functions\\" + frm_Power_Automation.CurrentAutoName + "\\" + frm_Power_Automation.CurrentAutoName + "_MaxStep.ini";
                }
                string oldValue = File.ReadAllText(frm_Power_Automation.CurrentMax_StepFile);
                string oldMax = oldValue.Split(':')[1];
                return int.Parse(oldMax);
            }
            catch (Exception e1) { return 0; }

        }

        private void frm_DataScraper_Load(object sender, EventArgs e)
        {
            cmb_DataS_Format.SelectedIndex = 0;
            cmb_DataS_Pages.SelectedIndex = 0;
            cmb_ElementType.SelectedIndex = 0;

            cmb_DataS_Condition.SelectedIndex = 0;
            App_Path = System.IO.Directory.GetCurrentDirectory();

            CurrentStepID = frm_Power_Automation.CurrentStepID;
            if (CurrentStepID == "")
            {
                int intCurrentStepID = GetStepMaxFromFile() + 1;
                CurrentStepID = intCurrentStepID.ToString();
            }
            CurrentFuntionFileFromWrite = GetLatestJOSNFile(App_Path + "\\Functions\\" + frm_Power_Automation.CurrentAutoName, "*STEP_" + CurrentStepID + "_Data Scrap*.json");
            CurrentFuntionFileToWrite = CurrentFuntionFileFromWrite;
            //check if the file exist, if not, copy
            bool isFileExist = File.Exists(CurrentFuntionFileFromWrite);
            if (!isFileExist)
            {
                File.Copy(App_Path + "\\settings\\FunctionTemplates\\FunctionTemplate_DataScaper_SEL.json", CurrentFuntionFileFromWrite);
            }
            LoadMappingsDG();
            txt_DataS_ScraperName.Text = frm_Power_Automation.CurrentAutoName + "_Scrapper";
            //set the current endpoint url and isenpointurl

            //now, update the function json file 
            string CurrentToCopyFunctionExecuteJsonLocation1 = CurrentFuntionFileFromWrite.Replace(".json", "1.json");
            string targetFileFunctionText = File.ReadAllText(CurrentFuntionFileFromWrite);
            JObject J_fullText = JObject.Parse(targetFileFunctionText);

            //          "FunctionID": "FunctionID_SampleXXX_StepID1",
            //"FunctionName": "FunctionName_SampleXXX_StepID1",
            //"FunctionCategoryID": "AutoCate_SEL_01",
            //"FunctionCategoryName": "SEL",
            //"FunctionExecuteJsonLocation": "FunctionExecuteJsonLocation_SampleXXX_StepID1",
            //"SaveFileFolder": "C:|Users|james|Downloads|HTMLElementSelect|HTMLElementSelect|HTMLElementSelect|bin|Debug|Functions|majessss",
            //"SaveFileName": "majessss_Scrapper.csv",
            //"SaveFileFormat": "CSV_SampleXXX_StepID1",
            //"ScrapPages": "Single_SampleXXX_StepID1",
            //"SaveFileColumnDelimiter": "SaveFileColumnDelimiter_SampleXXX_StepID1",
            //"WholeFunctionisNavigateURL": true,
            //"WholeFunctionNavigateURL": "https://www.majesticpet.com/bagel-bed/",


            J_fullText["FunctionName"] = frm_Power_Automation.CurrentFunctionName;
            J_fullText["WholeFunctionisNavigateURL"] = frm_Power_Automation.CurrentIsNavigateURL;

            J_fullText["WholeFunctionNavigateURL"] = frm_Power_Automation.CurrentStartEndPointURL;

            File.WriteAllText(CurrentToCopyFunctionExecuteJsonLocation1, J_fullText.ToString());
            //rename the file 
            File.Delete(CurrentFuntionFileToWrite);
            File.Move(CurrentToCopyFunctionExecuteJsonLocation1, CurrentFuntionFileToWrite);


            //end setting
        }

        private void bt_DataS_SetLocation_Click(object sender, EventArgs e)
        {

            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    //string[] files = Directory.GetFiles(fbd.SelectedPath);
                    txt_DataS_FileLocation.Text = fbd.SelectedPath.ToString();
                    txt_DataS_FileLocation.ForeColor = Color.Black;
                    //System.Windows.Forms.MessageBox.Show("Files found: " + files.Length.ToString(), "Message");
                }
            }


            //######################################################  end check the function file is exist ################################################
            string CurrentToCopyFunctionExecuteJsonLocation1 = CurrentFuntionFileFromWrite.Replace(".json", "1.json");
            string targetFileFunctionText = File.ReadAllText(CurrentFuntionFileFromWrite);

            if (!txt_DataS_FileLocation.Text.Contains("Leave blank"))
            {
                //save the Location to the function json file

                //replace the isNavigateURL
                //check PreviousIsNavigateURL is null or have value
                if (frm_Power_Automation.PreviousIsNavigateURL == "" | frm_Power_Automation.PreviousIsNavigateURL == "False")
                {
                    targetFileFunctionText = targetFileFunctionText.Replace("FunctionDetail_isNavigateURL_SampleXXX", frm_Power_Automation.CurrentIsNavigateURL.ToString());
                    frm_Power_Automation.PreviousIsNavigateURL = frm_Power_Automation.CurrentIsNavigateURL.ToString();
                }
                else
                {
                    //need to check if previous is already True
                    if (frm_Power_Automation.CurrentIsNavigateURL && frm_Power_Automation.PreviousIsNavigateURL == "True")
                    {
                        MessageBox.Show("Prevoius Step has Navigation URL, are you sure to make this again? this will create a new Brower", "Step Setup Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        targetFileFunctionText = targetFileFunctionText.Replace("FunctionDetail_isNavigateURL_SampleXXX", "False");
                    }
                }

                //replace the SaveFileFolder
                string folder = txt_DataS_FileLocation.Text;
                folder = folder.Replace("\\", "|");
                targetFileFunctionText = targetFileFunctionText.Replace("SaveFileFolder_SampleXXX_StepID1", folder);
                File.WriteAllText(CurrentToCopyFunctionExecuteJsonLocation1, targetFileFunctionText);
            }
            if (txt_DataS_FileLocation.Text.Contains("Leave blank"))
            {
                string folder = App_Path + "\\Functions\\" + frm_Power_Automation.CurrentAutoName + "";
                folder = folder.Replace("\\", "|");
                targetFileFunctionText = targetFileFunctionText.Replace("SaveFileFolder_SampleXXX_StepID1", folder);
                File.WriteAllText(CurrentToCopyFunctionExecuteJsonLocation1, targetFileFunctionText);
            }
            if (txt_DataS_ScraperName.Text.Contains("Leave blank"))
            {
                //replace the SaveFileFolder
                targetFileFunctionText = targetFileFunctionText.Replace("FileName_SampleXXX_StepID1", App_Path + "\\Data\\" + frm_Power_Automation.CurrentAutoName + "_STEP_" + CurrentStepID + "_DataScrap" + ".csv");
                File.WriteAllText(CurrentToCopyFunctionExecuteJsonLocation1, targetFileFunctionText);
            }
            if (!txt_DataS_ScraperName.Text.Contains("Leave blank"))
            {
                //replace the SaveFileFolder
                targetFileFunctionText = targetFileFunctionText.Replace("FileName_SampleXXX_StepID1", txt_DataS_ScraperName.Text + ".csv");
                File.WriteAllText(CurrentToCopyFunctionExecuteJsonLocation1, targetFileFunctionText);
            }

            //rename the file 
            File.Delete(CurrentFuntionFileToWrite);
            File.Move(CurrentToCopyFunctionExecuteJsonLocation1, CurrentFuntionFileToWrite);
        }

        private void bt_DataS_AddCondition_Click(object sender, EventArgs e)
        {
            if (txt_DataS_ColumnName.Text == "")
            {
                MessageBox.Show("Please enter column Name");
                txt_DataS_ColumnName.Focus();
                return;
            }
            string condiSymbol = cmb_DataS_Condition.Text;
            string colName = txt_DataS_ColumnName.Text;
            string conValue = txt_DataS_ConditionValue.Text;
            string xp1 = txt_XPath1.Text;
            string xp2 = txt_XPath2.Text;

            int index_xp12diff = 0;
            int xLen1 = xp1.Length;
            int xLen2 = xp2.Length;
            string xpa = string.Empty;
            if (xLen1 - xLen2 > 1 | xLen2 - xLen1 > 1)
            {
                MessageBox.Show("Collect wrong element, they are not the same data type");
                return;
            }
            for (int i = 0; i <= xLen1 - 1; i++)
            {
                if (xp1.ToCharArray()[i] != xp2.ToCharArray()[i])
                {
                    index_xp12diff = i;
                    break;
                }
            }
            if (index_xp12diff > 0)
            {
                var aStringBuilder = new StringBuilder(xp1);
                aStringBuilder.Remove(index_xp12diff, 1);
                aStringBuilder.Insert(index_xp12diff, "xxx");
                xpa = aStringBuilder.ToString();
            }

            if (xpa != "")
            {
                //now, update the function json file 
                string CurrentToCopyFunctionExecuteJsonLocation1 = CurrentFuntionFileFromWrite.Replace(".json", "1.json");
                string targetFileFunctionText = File.ReadAllText(CurrentFuntionFileFromWrite);
                JObject J_fullText = JObject.Parse(targetFileFunctionText);
                int FunDetailCount = J_fullText["FunctionDetail"].Count();
                // object►FunctionDetail►0►ElementToColumnName
                int columnIndex = 0;
                for (int i = 0; i <= FunDetailCount - 1; i++)
                {
                    //get the top first elementtoColumnName, if it is something Samplexxx, then it is the first column need to update
                    string CoumnName = J_fullText["FunctionDetail"][i].SelectToken("ElementToColumnName").ToString();
                    if (CoumnName.Contains("ElementToColumnName_SampleXXX"))
                    {
                        columnIndex = i + 1;
                        //now update the json value

                        //                 {
                        //                     "isNavigateURL":"FunctionDetail_isNavigateURL_SampleXXX",
                        //"NavigateURL":"FunctionDetail_NavigateURL_SampleXXX",	 
                        //"Action":"Data Scrap",
                        //   "ElementXPath":"FunctionDetail_ElementXPath_SampleXXX",
                        //"ElementToColumnName":"FunctionDetail_ElementToColumnName_SampleXXX",
                        //"ElementToColumnIndex":"FunctionDetail_ElementToColumnIndex_SampleXXX",
                        //"ConditionCompareSymbol":"None",  
                        //"ConditionCompareValue":"FunctionDetail_ConditionCompareValue_SampleXXX",  
                        //"isElementMedia":"FunctionDetail_isElementMedia_SampleXXX",
                        //"MediaDownloadSaveName":"FunctionDetail_MediaDownloadSaveName_SampleXXX",
                        //"MediaDownloadSaveLocation":"FunctionDetail_MediaDownloadSaveLocation_SampleXXX"
                        // }

                        JObject j_FunctionDetail = (JObject)J_fullText["FunctionDetail"][i];

                        j_FunctionDetail["ElementToColumnName"] = txt_DataS_ColumnName.Text;

                        j_FunctionDetail["ElementToColumnIndex"] = i;

                        j_FunctionDetail["ConditionCompareSymbol"] = cmb_DataS_Condition.Text;

                        j_FunctionDetail["ConditionCompareValue"] = txt_DataS_ConditionValue.Text;

                        j_FunctionDetail["ElementXPath"] = xpa;

                        File.WriteAllText(CurrentToCopyFunctionExecuteJsonLocation1, J_fullText.ToString());

                        //end update the json value
                        break;
                    }
                }

                //rename the file 
                File.Delete(CurrentFuntionFileToWrite);
                File.Move(CurrentToCopyFunctionExecuteJsonLocation1, CurrentFuntionFileToWrite);
            }
            else
            {
                MessageBox.Show("Please fill the column detail");
            }



        }

        private void cmb_DataS_Condition_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public void LoadMappingsDG()
        {
            //load the mappings into the datagridview
            dg_DataS_Mappings.Rows.Clear();
            DataGridViewTextBoxColumn c_ColumnIndex = new DataGridViewTextBoxColumn();
            c_ColumnIndex.Name = "Column Index";
            c_ColumnIndex.HeaderText = "Column Index";
            c_ColumnIndex.Width = 120;
            dg_DataS_Mappings.Columns.Add(c_ColumnIndex);

            DataGridViewTextBoxColumn c_ColumnName = new DataGridViewTextBoxColumn();
            c_ColumnName.Name = "ColumnName";
            c_ColumnName.HeaderText = "Column Name";
            c_ColumnName.Width = 250;
            dg_DataS_Mappings.Columns.Add(c_ColumnName);

            DataGridViewTextBoxColumn c_ConditionSymbol = new DataGridViewTextBoxColumn();
            c_ConditionSymbol.Name = "Condition Symbol";
            c_ConditionSymbol.HeaderText = "ConditionSymbol";
            c_ConditionSymbol.Width = 120;
            dg_DataS_Mappings.Columns.Add(c_ConditionSymbol);

            DataGridViewTextBoxColumn c_ConditionValue = new DataGridViewTextBoxColumn();
            c_ConditionValue.Name = "Condition Value";
            c_ConditionValue.HeaderText = "Condition Value";
            c_ConditionValue.Width = 200;
            dg_DataS_Mappings.Columns.Add(c_ConditionValue);
            string targetFileFunctionText = File.ReadAllText(CurrentFuntionFileFromWrite);
            JObject J_fullText = JObject.Parse(targetFileFunctionText);
            int FunDetailCount = J_fullText["FunctionDetail"].Count();
            // object►FunctionDetail►0►ElementToColumnName
            int columnIndex = 0;
            for (int i = 0; i <= FunDetailCount - 1; i++)
            {
                //          "isNavigateURL": "FunctionDetail_isNavigateURL_SampleXXX",
                //"NavigateURL": "FunctionDetail_NavigateURL_SampleXXX",
                //"Action": "Data Scrap",
                //"ElementXPath": "FunctionDetail_ElementXPath_SampleXXX",
                //"ElementToColumnName": "FunctionDetail_ElementToColumnName_SampleXXX",
                //"ElementToColumnIndex": "FunctionDetail_ElementToColumnIndex_SampleXXX",
                //"ConditionCompareSymbol": "None",
                //"ConditionCompareValue": "FunctionDetail_ConditionCompareValue_SampleXXX",
                //"isElementMedia": "FunctionDetail_isElementMedia_SampleXXX",
                //"MediaDownloadSaveName": "FunctionDetail_MediaDownloadSaveName_SampleXXX",
                //"MediaDownloadSaveLocation": "FunctionDetail_MediaDownloadSaveLocation_SampleXXX"
                string ElementToColumnIndex = J_fullText["FunctionDetail"][i].SelectToken("ElementToColumnIndex").ToString();
                string ElementToColumnName = J_fullText["FunctionDetail"][i].SelectToken("ElementToColumnName").ToString();
                string ConditionCompareSymbol = J_fullText["FunctionDetail"][i].SelectToken("ConditionCompareSymbol").ToString();
                string ConditionCompareValue = J_fullText["FunctionDetail"][i].SelectToken("ConditionCompareValue").ToString();
                if (!ElementToColumnName.Contains("ElementToColumnName_SampleXXX"))
                {
                    dg_DataS_Mappings.Rows.Add(ElementToColumnIndex, ElementToColumnName, ConditionCompareSymbol, ConditionCompareValue);
                }

            }

        }

        public void checkBaseInfo()
        {
            string CurrentToCopyFunctionExecuteJsonLocation1 = CurrentFuntionFileFromWrite.Replace(".json", "1.json");
            string targetFileFunctionText = File.ReadAllText(CurrentFuntionFileFromWrite);

            if (!txt_DataS_FileLocation.Text.Contains("Leave blank"))
            {
                //save the Location to the function json file

                //replace the isNavigateURL
                //check PreviousIsNavigateURL is null or have value
                if (frm_Power_Automation.PreviousIsNavigateURL == "" | frm_Power_Automation.PreviousIsNavigateURL == "False")
                {
                    targetFileFunctionText = targetFileFunctionText.Replace("FunctionDetail_isNavigateURL_SampleXXX", frm_Power_Automation.CurrentIsNavigateURL.ToString());
                    frm_Power_Automation.PreviousIsNavigateURL = frm_Power_Automation.CurrentIsNavigateURL.ToString();
                }
                else
                {
                    //need to check if previous is already True
                    if (frm_Power_Automation.CurrentIsNavigateURL && frm_Power_Automation.PreviousIsNavigateURL == "True")
                    {
                        MessageBox.Show("Prevoius Step has Navigation URL, are you sure to make this again? this will create a new Brower", "Step Setup Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        targetFileFunctionText = targetFileFunctionText.Replace("FunctionDetail_isNavigateURL_SampleXXX", "False");
                    }
                }

                //replace the SaveFileFolder
                string folder = txt_DataS_FileLocation.Text;
                folder = folder.Replace("\\", "|");
                targetFileFunctionText = targetFileFunctionText.Replace("SaveFileFolder_SampleXXX_StepID1", folder);
                File.WriteAllText(CurrentToCopyFunctionExecuteJsonLocation1, targetFileFunctionText);
            }
            if (txt_DataS_FileLocation.Text.Contains("Leave blank"))
            {
                string folder = App_Path + "\\Functions\\" + frm_Power_Automation.CurrentAutoName + "";
                folder = folder.Replace("\\", "|");
                targetFileFunctionText = targetFileFunctionText.Replace("SaveFileFolder_SampleXXX_StepID1", folder);
                File.WriteAllText(CurrentToCopyFunctionExecuteJsonLocation1, targetFileFunctionText);
            }
            if (txt_DataS_ScraperName.Text.Contains("Leave blank"))
            {
                //replace the SaveFileFolder
                targetFileFunctionText = targetFileFunctionText.Replace("FileName_SampleXXX_StepID1", App_Path + "\\Data\\" + frm_Power_Automation.CurrentAutoName + "_STEP_" + CurrentStepID + "_DataScrap" + ".csv");
                File.WriteAllText(CurrentToCopyFunctionExecuteJsonLocation1, targetFileFunctionText);
            }
            if (!txt_DataS_ScraperName.Text.Contains("Leave blank"))
            {
                //replace the SaveFileFolder
                targetFileFunctionText = targetFileFunctionText.Replace("FileName_SampleXXX_StepID1", txt_DataS_ScraperName.Text + ".csv");
                File.WriteAllText(CurrentToCopyFunctionExecuteJsonLocation1, targetFileFunctionText);
            }

            //rename the file 
            File.Delete(CurrentFuntionFileToWrite);
            File.Move(CurrentToCopyFunctionExecuteJsonLocation1, CurrentFuntionFileToWrite);
        }
        private void bt_DataS_AddColumn_Click(object sender, EventArgs e)
        {
            if (txt_DataS_ColumnName.Text == "")
            {
                MessageBox.Show("Please enter column Name");
                txt_DataS_ColumnName.Focus();
                return;
            }
            //##############################################
            //######################################################  end check the function file is exist ################################################
            checkBaseInfo();
            //##############################################

            string condiSymbol = cmb_DataS_Condition.Text;
            string colName = txt_DataS_ColumnName.Text;
            string conValue = txt_DataS_ConditionValue.Text;
            string xp1 = txt_XPath1.Text;
            string xp2 = txt_XPath2.Text;

            int index_xp12diff = 0;
            int xLen1 = xp1.Length;
            int xLen2 = xp2.Length;
            string xpa = string.Empty;
            if (xLen1 - xLen2 > 1 | xLen2 - xLen1 > 1)
            {
                MessageBox.Show("Collect wrong element, they are not the same data type");
                return;
            }
            for (int i = 0; i <= xLen1 - 1; i++)
            {
                if (xp1.ToCharArray()[i] != xp2.ToCharArray()[i])
                {
                    index_xp12diff = i;
                    break;
                }
            }
            if (index_xp12diff > 0)
            {
                var aStringBuilder = new StringBuilder(xp1);
                aStringBuilder.Remove(index_xp12diff, 1);
                aStringBuilder.Insert(index_xp12diff, "xxx");
                xpa = aStringBuilder.ToString();
            }

            if (xpa != "")
            {
                xpa = xpa.Replace("\"", "'");
                //now, update the function json file 
                string CurrentToCopyFunctionExecuteJsonLocation1 = CurrentFuntionFileFromWrite.Replace(".json", "1.json");
                string targetFileFunctionText = File.ReadAllText(CurrentFuntionFileFromWrite);
                JObject J_fullText = JObject.Parse(targetFileFunctionText);
                int FunDetailCount = J_fullText["FunctionDetail"].Count();
                // object►FunctionDetail►0►ElementToColumnName
                int columnIndex = 0;
                for (int i = 0; i <= FunDetailCount - 1; i++)
                {
                    //get the top first elementtoColumnName, if it is something Samplexxx, then it is the first column need to update
                    string CoumnName = J_fullText["FunctionDetail"][i].SelectToken("ElementToColumnName").ToString();
                    if (CoumnName.Contains("ElementToColumnName_SampleXXX"))
                    {
                        columnIndex = i + 1;
                        //now update the json value
                        //                 {
                        //                     "isNavigateURL":"FunctionDetail_isNavigateURL_SampleXXX",
                        //"NavigateURL":"FunctionDetail_NavigateURL_SampleXXX",	 
                        //"Action":"Data Scrap",
                        //   "ElementXPath":"FunctionDetail_ElementXPath_SampleXXX",
                        //"ElementToColumnName":"FunctionDetail_ElementToColumnName_SampleXXX",
                        //"ElementToColumnIndex":"FunctionDetail_ElementToColumnIndex_SampleXXX",
                        //"ConditionCompareSymbol":"None",  
                        //"ConditionCompareValue":"FunctionDetail_ConditionCompareValue_SampleXXX",  
                        //"isElementMedia":"FunctionDetail_isElementMedia_SampleXXX",
                        //"MediaDownloadSaveName":"FunctionDetail_MediaDownloadSaveName_SampleXXX",
                        //"MediaDownloadSaveLocation":"FunctionDetail_MediaDownloadSaveLocation_SampleXXX"
                        // }

                        JObject j_FunctionDetail = (JObject)J_fullText["FunctionDetail"][i];
                        j_FunctionDetail["ElementToColumnName"] = txt_DataS_ColumnName.Text;
                        j_FunctionDetail["ElementToColumnIndex"] = i;
                        j_FunctionDetail["ConditionCompareSymbol"] = cmb_DataS_Condition.Text;
                        j_FunctionDetail["NavigateURL"] = frm_Power_Automation.CurrentIsNavigateURL;

                        string DataS_ConditionValue = txt_DataS_ConditionValue.Text;
                        if (DataS_ConditionValue == "A number or string want to compare")
                        {
                            DataS_ConditionValue = "";
                        }
                        j_FunctionDetail["ConditionCompareValue"] = DataS_ConditionValue;
                        j_FunctionDetail["ElementXPath"] = xpa;
                        File.WriteAllText(CurrentToCopyFunctionExecuteJsonLocation1, J_fullText.ToString());

                        //###############################  liteDB insert function to T_ TABLE                     
                        using (var db = new LiteDatabase(frm_Power_Automation.App_Path + "\\PowerAutoLiteDB.db"))
                        {

                            //                          "FunctionID": "FunctionID_SampleXXX_StepID1",
                            //"FunctionName": "Data Scraper",
                            //"FunctionCategoryID": "AutoCate_SEL_01",
                            //"FunctionCategoryName": "SEL",
                            //"FunctionExecuteJsonLocation": "FunctionExecuteJsonLocation_SampleXXX_StepID1",
                            //"SaveFileFolder": "C:|Users|james|Downloads|HTMLElementSelect|HTMLElementSelect|HTMLElementSelect|bin|Debug|Functions|Coinbase Price",
                            //"SaveFileName": "Coinbase Price_Scrapper.csv",
                            //"SaveFileFormat": "CSV_SampleXXX_StepID1",
                            //"ScrapPages": "Single_SampleXXX_StepID1",
                            //"SaveFileColumnDelimiter": "SaveFileColumnDelimiter_SampleXXX_StepID1",
                            //"WholeFunctionisNavigateURL": true,
                            //"WholeFunctionNavigateURL": "https://www.coinbase.com/price",
                            //"isMutiplePage": "False",
                            //"PageElementXPath": "PageElement_SampleXXX_StepID1",
                            //"FunctionDetail": [
                            //  {
                            //                              "isNavigateURL": "FunctionDetail_isNavigateURL_SampleXXX",
                            //    "NavigateURL": true,
                            //    "Action": "Data Scrap",
                            //    "isDynamicElementXPath": "False",
                            //    "DynamicElementTagName": "FunctionDetail_DynamicElementTagName_SampleXXX",
                            //    "DynamicElementAttributeName": "FunctionDetail_DynamicElementAttributeName_SampleXXX",
                            //    "DynamicElementAttributeValue1": "FunctionDetail_DynamicElementAttributeValue1_SampleXXX",
                            //    "DynamicElementAttributeValue2": "FunctionDetail_DynamicElementAttributeValue2_SampleXXX",
                            //    "ElementXPath": "//*[@id='main']/div[1]/table/tbody/tr[xxx]/td[1]/a/div/h4[2]",
                            //    "ElementToColumnName": "CoinName",
                            //    "ElementToColumnIndex": 0,
                            //    "ConditionCompareSymbol": "None",
                            //    "ConditionCompareValue": "",
                            //    "isElementMedia": "FunctionDetail_isElementMedia_SampleXXX",
                            //    "MediaDownloadSaveName": "FunctionDetail_MediaDownloadSaveName_SampleXXX",
                            //    "MediaDownloadSaveLocation": "FunctionDetail_MediaDownloadSaveLocation_SampleXXX",
                            //    "AfterDoneWaitSeconds": "0",
                            //    "AfterDoneRedirectURL": "FunctionDetail_AfterDoneRedirectURL_SampleXXX"
                            //  },

                            //string InsertLiteSQL = "insert into " + frm_Power_Automation.CurrentFunctionCategoryName + " VALUES { " +
                            //    "AutoName: " + "\"" + frm_Power_Automation.CurrentAutoName + "\"" + ", " +
                            //    "StepID: " + CurrentStepID + ", " +
                            //    "FunctionID:" + "\"" + "ID_" + frm_Power_Automation.CurrentFunctionCategoryName + "\"" + ", " +
                            //    "FunctionCategoryID:" + "\"" + "ID_" + frm_Power_Automation.CurrentFunctionCategoryName + "\"" + ", " +
                            //    "FunDetail_DetailIndex:" + CurrentFunctionDetail_Index + ", " +
                            //   "FunDetail_isNavigateURL:" + frm_Power_Automation.CurrentIsNavigateURL + ", " +
                            //    "FunDetail_NavigateURL:" + "\"" + frm_Power_Automation.CurrentStartEndPointURL + "\"" + ", " +
                            //    "FunDetail_Action:" + "\"" + "Data Scrap" + "\"" + ", " +
                            //    "FunDetail_isDynamicElementXPath:" + "\"" + CurrentisDynamicElementXPath + "\"" + ", " +
                            //    "FunDetail_DynamicElementTagName:" + "\"" + CurrentDynamicElementTagName + "\"" + ", " +
                            //    "FunDetail_DynamicElementAttributeName:" + "\"" + CurrentDynamicElementAttributeName + "\"" + ", " +
                            //    "FunDetail_DynamicElementAttributeValue1:" + "\"" + CurrentDynamicElementAttributeValue1 + "\"" + ", " +
                            //    "FunDetail_DynamicElementAttributeValue2:" + "\"" + CurrentDynamicElementAttributeValue2 + "\"" + ", " +
                            //    "FunDetail_ElementXPath:" + "\"" + xpathValue + "\"" + ", " +
                            //    "FunDetail_SendValue:" + "\"" + "" + "\"" + ", " +
                            //    "FunDetail_AfterDoneWaitSeconds:" + "\"" + txt_Tab2_WaitT.Text + "\"" + ", " +
                            //    "FunDetail_AfterDoneRedirectURL:" + "\"" + txt_Tab2_rediURL.Text + "\"" + " " +
                            //    "} ";
                            string InsertValues = "AutoName: " + "\"" + frm_Power_Automation.CurrentAutoName + "\"" + ", " +
                             "StepID: " + CurrentStepID + ", " +
                             "FunctionID:" + "\"" + "ID_" + frm_Power_Automation.CurrentFunctionCategoryName + "\"" + ", " +
                             "FunctionCategoryID:" + "\"" + "ID_" + frm_Power_Automation.CurrentFunctionCategoryName + "\"" + ", " +
                             "FunDetail_DetailIndex:" + CurrentFunctionDetail_Index + ", " +
                             "SaveFileFolder: " + "\"" + frm_Power_Automation.CurrentAutoExecuteFolder + "\"" + ", " +
                             "SaveFileName: " + "\"" + frm_Power_Automation.Current_Scaper_SaveDataFileName + "\"" + ", " +
                             "SaveFileFormat: " + "\"" + cmb_DataS_Format.SelectedText + "\"" + ", " +
                             "ScrapPages: " + "\"" + "Single_SampleXXX_StepID1" + "\"" + ", " +
                             "SaveFileColumnDelimiter: " + "\"" + "SaveFileColumnDelimiter_SampleXXX_StepID1" + "\"" + ", " +
                             "WholeFunctionisNavigateURL: " + "\"" + "true" + ", " +
                             "WholeFunctionNavigateURL: " + "\"" + "https:www.coinbase.com/price" + "\"" + ", " +
                             "isMutiplePage: " + "\"" + "False" + "\"" + ", " +
                             "PageElementXPath: " + "\"" + "PageElement_SampleXXX_StepID1" + "\"" + ", " +

                             "FunDetail_isNavigateURL: " + "\"" + "FunctionDetail_isNavigateURL_SampleXXX" + "\"" + ", " +
                             "FunDetail_NavigateURL: " + "\"" + "true" + ", " +
                             "FunDetail_Action: " + "\"" + "Data Scrap" + "\"" + ", " +
                             "FunDetail_isDynamicElementXPath: " + "\"" + "False" + "\"" + ", " +
                             "FunDetail_DynamicElementTagName: " + "\"" + "FunctionDetail_DynamicElementTagName_SampleXXX" + "\"" + ", " +
                             "FunDetail_DynamicElementAttributeName: " + "\"" + "FunctionDetail_DynamicElementAttributeName_SampleXXX" + "\"" + ", " +
                             "FunDetail_DynamicElementAttributeValue1: " + "\"" + "FunctionDetail_DynamicElementAttributeValue1_SampleXXX" + "\"" + ", " +
                             "FunDetail_DynamicElementAttributeValue2: " + "\"" + "FunctionDetail_DynamicElementAttributeValue2_SampleXXX" + "\"" + ", " +
                             "FunDetail_ElementXPath: " + "\"" + "*[@id='main']/div[1]/table/tbody/tr[xxx]/td[1]/a/div/h4[2]" + "\"" + ", " +
                             "FunDetail_ElementToColumnName: " + "\"" + "CoinName" + "\"" + ", " +
                             "FunDetail_ElementToColumnIndex: " + "\"" + "0" + "\"" + ", " +
                             "FunDetail_ConditionCompareSymbol: " + "\"" + "None" + "\"" + ", " +
                             "FunDetail_ConditionCompareValue:" + "\"" + "CoinName" + "\"" + ", " +
                             "FunDetail_isElementMedia: " + "\"" + "FunctionDetail_isElementMedia_SampleXXX" + "\"" + ", " +
                             "FunDetail_MediaDownloadSaveName: " + "\"" + "FunctionDetail_MediaDownloadSaveName_SampleXXX" + "\"" + ", " +
                             "FunDetail_MediaDownloadSaveLocation: " + "\"" + "FunctionDetail_MediaDownloadSaveLocation_SampleXXX" + "\"" + ", " +
                             "FunDetail_AfterDoneWaitSeconds:" + "\"" + "CoinName" + "\"" + ", " +
                             "FunDetail_AfterDoneRedirectURL: " + "\"" + "FunctionDetail_AfterDoneRedirectURL_SampleXXX";

                            string InsertLiteSQL = "insert into " + frm_Power_Automation.CurrentFunctionCategoryName + " VALUES { " +InsertValues+
                             
                             

                              "} ";
                            db.Execute(InsertLiteSQL);
                        }
                        //###############################/ end liteDB insert

                        //end update the json value
                        break;
                    }
                }

                //rename the file 
                File.Delete(CurrentFuntionFileToWrite);
                File.Move(CurrentToCopyFunctionExecuteJsonLocation1, CurrentFuntionFileToWrite);


                MessageBox.Show("New column " + txt_DataS_ColumnName.Text + " added!");
            }
            else
            {
                MessageBox.Show("Please fill the column detail");
            }

            LoadMappingsDG();
            txt_XPath1.Text = "";
            txt_XPath2.Text = "";

        }

        private void ck_XPath1_CheckedChanged(object sender, EventArgs e)
        {
            txt_XPath1.Text = frm_Power_Automation.CurrentElementXPathValue;
        }

        private void ck_XPath2_CheckedChanged(object sender, EventArgs e)
        {
            txt_XPath2.Text = frm_Power_Automation.CurrentElementXPathValue;
        }

        public void SaveStep()
        {
            string targetFileBase = App_Path + "\\Functions\\" + frm_Power_Automation.CurrentAutoName;
            string targetFileSteps = targetFileBase + "\\" + frm_Power_Automation.CurrentAutoName + "_FunctionSteps.json";
            string targetFileSteps1 = targetFileBase + "\\" + frm_Power_Automation.CurrentAutoName + "_FunctionSteps1.json";
            string AutoName = frm_Power_Automation.CurrentAutoName;
            string xpathValue = frm_Power_Automation.CurrentElementXPathValue.Replace("\"", "'");
            int inCurrentStepID = GetStepMaxFromFile() + 1;
            CurrentStepID = inCurrentStepID.ToString();
            //################### create the folder if not exist
            System.IO.Directory.CreateDirectory(targetFileBase);
            //check if the target file is exist, means it may have some step setup before
            if (File.Exists(targetFileSteps))
            {
                //edit the step belong to this step , only update the funtcion detail

                Object_AutoStepsJson.FunctionDetail ItemFunctionDetail = new Object_AutoStepsJson.FunctionDetail();
                ItemFunctionDetail.FunctionID = "FID_" + CurrentStepID + "_N_" + AutoName + "_F_" + frm_Power_Automation.CurrentFunctionName;
                ItemFunctionDetail.FunctionID = ItemFunctionDetail.FunctionID.Replace(" ", "_").Replace("|", "_");
                ItemFunctionDetail.FunctionName = "F_" + frm_Power_Automation.CurrentFunctionName + "_N_" + AutoName + "_FID_" + CurrentStepID;
                ItemFunctionDetail.FunctionName = ItemFunctionDetail.FunctionName.Replace(" ", "_").Replace("|", "_");
                ItemFunctionDetail.FunctionCategoryID = frm_Power_Automation.CurrentFunctionCategoryID;
                ItemFunctionDetail.FunctionCategoryName = frm_Power_Automation.CurrentFunctionCategoryName;
                string targetFileStep = targetFileBase + "\\" + ItemFunctionDetail.FunctionName.Replace("|", "_") + ".json";
                CurrentFunctionExecuteJsonShortName = AutoName + "_STEP_" + CurrentStepID + "_" + frm_Power_Automation.CurrentFunctionName + "_Function.json";
                ItemFunctionDetail.FunctionExecuteJsonLocation = CurrentFunctionExecuteJsonShortName;
                CurrentFunctionExecuteJsonLocation = CurrentToCopyFunctionExecuteJsonLocation;

                //replace the steplist value base on CurrentStepID
                if (int.Parse(CurrentStepID) < 10)
                {
                    //             "FunctionDetail": 
                    //{
                    //                 "FunctionID": "FunctionID_SampleXXX_StepID2",
                    //     "FunctionName": "FunctionName_SampleXXX_StepID2",
                    //     "FunctionCategoryID": "FunctionCategoryID_SampleXXX_StepID2",
                    //     "FunctionCategoryName": "FunctionCategoryName_SampleXXX_StepID2",
                    //     "FunctionExecuteJsonLocation": "FunctionExecuteJsonLocation_SampleXXX_StepID2"
                    //   }

                    string targetFileStepsText = File.ReadAllText(targetFileSteps);
                    //replace the AutoName
                    targetFileStepsText = targetFileStepsText.Replace("Power_Automation_SampleXXX", AutoName);
                    //replace the DESC
                    targetFileStepsText = targetFileStepsText.Replace("Automation_SampleXXX_Description", "");

                    //replace the "StartEndpointURL": "Automation_SampleXXX_StartEndpointURL",
                    targetFileStepsText = targetFileStepsText.Replace("Automation_SampleXXX_StartEndpointURL", frm_Power_Automation.CurrentStartEndPointURL);


                    //replace the FunctionID
                    targetFileStepsText = targetFileStepsText.Replace("FunctionID_SampleXXX_StepID" + CurrentStepID, ItemFunctionDetail.FunctionID);

                    //replace the FunctionName
                    targetFileStepsText = targetFileStepsText.Replace("FunctionName_SampleXXX_StepID" + CurrentStepID, ItemFunctionDetail.FunctionName);


                    //replace the FunctionCategoryID
                    targetFileStepsText = targetFileStepsText.Replace("FunctionCategoryID_SampleXXX_StepID" + CurrentStepID, ItemFunctionDetail.FunctionCategoryID);

                    //replace the FunctionCategoryName
                    targetFileStepsText = targetFileStepsText.Replace("FunctionCategoryName_SampleXXX_StepID" + CurrentStepID, ItemFunctionDetail.FunctionCategoryName);

                    //replace the FunctionExecuteJsonLocation

                    targetFileStepsText = targetFileStepsText.Replace("FunctionExecuteJsonLocation_SampleXXX_StepID" + CurrentStepID, ItemFunctionDetail.FunctionExecuteJsonLocation);

                    File.WriteAllText(targetFileSteps1, targetFileStepsText);
                    File.Delete(targetFileSteps);
                    //rename the file 
                    File.Move(targetFileSteps1, targetFileSteps);

                    //###############################  liteDB insert function steps
                    FunctionSteps FunStep = new FunctionSteps();
                    using (var db = new LiteDatabase(App_Path + "\\PowerAutoLiteDB.db"))
                    {
                        // Get a collection (or create, if doesn't exist)
                        var col = db.GetCollection<FunctionSteps>("AutoFunctionSteps");
                        // Create your new customer instance
                        var AutomationFunctionStep = new FunctionSteps
                        {
                            AutoName = AutoName,
                            //Description = txt_Description.Text,
                            FunctionName = ItemFunctionDetail.FunctionName,
                            FunctionTableName = "T_" + ItemFunctionDetail.FunctionCategoryName
                        };
                        col.Insert(AutomationFunctionStep);
                    }
                    //###############################/ end liteDB insert
                }
                else
                {

                    //                 {
                    //                     "StepID": 10,
                    //   "FunctionDetail": 
                    //{
                    //                         "FunctionID": "10FunctionID_SampleXXX_StepID",
                    //     "FunctionName": "10FunctionName_SampleXXX_StepID",
                    //     "FunctionCategoryID": "10FunctionCategoryID_SampleXXX_StepID",
                    //     "FunctionCategoryName": "10FunctionCategoryName_SampleXXX_StepID",
                    //     "FunctionExecuteJsonLocation": "10FunctionExecuteJsonLocation_SampleXXX_StepID"
                    //   }
                    //                 },

                    string targetFileStepsText = File.ReadAllText(targetFileSteps);

                    //replace the AutoName
                    targetFileStepsText = targetFileStepsText.Replace("Power_Automation_SampleXXX", AutoName);
                    //replace the DESC
                    targetFileStepsText = targetFileStepsText.Replace("Automation_SampleXXX_Description", "");

                    //replace the FunctionID
                    targetFileStepsText = targetFileStepsText.Replace(CurrentStepID + "FunctionID_SampleXXX_StepID", ItemFunctionDetail.FunctionID);

                    //replace the FunctionName
                    targetFileStepsText = targetFileStepsText.Replace(CurrentStepID + "FunctionName_SampleXXX_StepID", ItemFunctionDetail.FunctionName);


                    //replace the FunctionCategoryID
                    targetFileStepsText = targetFileStepsText.Replace(CurrentStepID + "FunctionCategoryID_SampleXXX_StepID", ItemFunctionDetail.FunctionCategoryID);

                    //replace the FunctionCategoryName
                    targetFileStepsText = targetFileStepsText.Replace(CurrentStepID + "FunctionCategoryName_SampleXXX_StepID", ItemFunctionDetail.FunctionCategoryName);

                    //replace the FunctionExecuteJsonLocation

                    targetFileStepsText = targetFileStepsText.Replace(CurrentStepID + "FunctionExecuteJsonLocation_SampleXXX_StepID", ItemFunctionDetail.FunctionExecuteJsonLocation);

                    File.WriteAllText(targetFileSteps1, targetFileStepsText);
                    File.Delete(targetFileSteps);
                    //rename the file 
                    File.Move(targetFileSteps1, targetFileSteps);

                    //###############################  liteDB insert function steps
                    FunctionSteps FunStep = new FunctionSteps();
                    using (var db = new LiteDatabase(App_Path + "\\PowerAutoLiteDB.db"))
                    {
                        // Get a collection (or create, if doesn't exist)
                        var col = db.GetCollection<FunctionSteps>("AutoFunctionSteps");
                        // Create your new customer instance
                        var AutomationFunctionStep = new FunctionSteps
                        {
                            AutoName = AutoName,
                            //Description = txt_Description.Text,
                            FunctionName = ItemFunctionDetail.FunctionName,
                            FunctionTableName = "T_" + ItemFunctionDetail.FunctionCategoryName

                        };

                        col.Insert(AutomationFunctionStep);
                    }
                    //###############################/ end liteDB insert


                    frm_Power_Automation.CurrentFunctionExecuteJsonLocation = CurrentFunctionExecuteJsonLocation;
                }
            }
            else
            {
                // it is new setup, copy the template file only
                try { File.Copy(App_Path + "\\Functions\\FunctionStepsList_Template.json", targetFileSteps); }
                catch (Exception e1)
                {
                    DialogResult resulterror = MessageBox.Show("Step " + CurrentStepID + " Setup Fail, please re-select this step in the combox", "Step Setup Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }
            //################################ Save the mutiple page ######################
            string xp1 = txt_XPath_Page1.Text;
            string xp2 = txt_XPath_Page2.Text;

            int index_xp12diff = 0;
            int xLen1 = xp1.Length;
            int xLen2 = xp2.Length;
            string xpa = string.Empty;
            if (xLen1 - xLen2 > 1 | xLen2 - xLen1 > 1)
            {
                MessageBox.Show("Collect wrong element, they are not the same data type");
                return;
            }
            for (int i = 0; i <= xLen1 - 1; i++)
            {
                if (xp1.ToCharArray()[i] != xp2.ToCharArray()[i])
                {
                    index_xp12diff = i;
                    break;
                }
            }
            if (index_xp12diff > 0)
            {
                var aStringBuilder = new StringBuilder(xp1);
                aStringBuilder.Remove(index_xp12diff, 1);
                aStringBuilder.Insert(index_xp12diff, "xxx");
                xpa = aStringBuilder.ToString();
            }

            if (xpa != "")
            {
                xpa = xpa.Replace("\"", "'");
                //now, update the function json file 
                string CurrentToCopyFunctionExecuteJsonLocation1 = CurrentFuntionFileFromWrite.Replace(".json", "1.json");
                string targetFileFunctionText = File.ReadAllText(CurrentFuntionFileFromWrite);
                JObject J_fullText = JObject.Parse(targetFileFunctionText);
                // "isMutiplePage":"False",	
                //"PageElementXPath":"PageElement_SampleXXX_StepID1",	
                J_fullText["isMutiplePage"] = isMutiplePage;
                J_fullText["ElementToColumnIndex"] = xpa;
               
                File.WriteAllText(CurrentToCopyFunctionExecuteJsonLocation1, J_fullText.ToString());
                //end update the json value
                //rename the file 
                File.Delete(CurrentFuntionFileToWrite);
                File.Move(CurrentToCopyFunctionExecuteJsonLocation1, CurrentFuntionFileToWrite);
            }
                //################################ end save the multiple page #################

                DialogResult result = MessageBox.Show("Step " + CurrentStepID + " Saved", "Step Setup Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //save the max_step to the ini file
                string oldValue = File.ReadAllText(frm_Power_Automation.CurrentMax_StepFile);
                string oldMax = oldValue.Split(':')[1];
                int newMax = int.Parse(oldMax) + 1;
                CurrentStepID = newMax.ToString();
                //MaxStep:0
                oldValue = oldValue.Replace(oldValue, "MaxStep:" + newMax);
                File.WriteAllText(frm_Power_Automation.CurrentMax_StepFile, oldValue);

            //###########################  lite db to UPDATE THE MAX STEP
            LiteDB_Action.UpdateAuto_Lite_MaxStep(AutoName, newMax);
            //###########################  end litedb insert

        }
        private void bt_SaveFuntion_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("A new Scraper function will be save, are you sure you finish? \r\nClick YES to save.\r\nClick NO to review and continue edit", "Save Function Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result== DialogResult.Yes)
            {
                SaveStep();
                try
                {
                    frm_Power_Automation a = new frm_Power_Automation();
                    Application.Run(a);
                } 
                catch (Exception e2) 
                { }
               
            }
            else 
            {
                return;
            }

        }

        private void cmb_DataS_Pages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_DataS_Pages.SelectedIndex == 0)
            {
                ck_Page1.Visible = false;
                ck_Page2.Visible = false;
                txt_XPath_Page1.Visible = false;
                txt_XPath_Page2.Visible = false;
                isMutiplePage = "False";
            }
            if (cmb_DataS_Pages.SelectedIndex == 1)
            {
                ck_Page1.Visible = true;
                ck_Page2.Visible = true;
                txt_XPath_Page1.Visible = true;
                txt_XPath_Page2.Visible = true;
                isMutiplePage = "True";
            }
        }

        private void cmb_ElementType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_ElementType.SelectedIndex == 0)
            {
                panel1.Visible = true;
                panel2.Visible = false;
            }
            if (cmb_ElementType.SelectedIndex == 1)
            {
                panel1.Visible = false;
                panel2.Visible = true;
            }
        }
    }
}
