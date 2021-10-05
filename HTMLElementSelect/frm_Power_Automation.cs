using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using HtmlAgilityPack;
using mshtml;
using Newtonsoft.Json.Linq;
using System.IO;
using HTMLElementSelect.Objects;
using Newtonsoft.Json;
using Microsoft.Win32;
using Crainiate.Diagramming;
using Image = System.Drawing.Image;
using System.Text.RegularExpressions;
using System.Diagnostics;
using LiteDB;
using HTMLElementSelect.Functions;
using static HTMLElementSelect.Objects.Object_Lite_FunctionSteps;
using LiteDB;

namespace HTMLElementSelect
{
    public partial class frm_Power_Automation : Form
    {

        #region "Parameters"
        public static List<String> List_AutoNamesandTabs = new List<string>();
        public static JObject JO_AutoNames = new JObject();
        public static JObject JO_ExecuteFunctionJson = new JObject();
        public static JObject JO_ExecuteFunctionListJson = new JObject();

        public static string CurrentAutoName = string.Empty;
        public static string CurrentStepID = string.Empty;

        public static string CurrentFunctionName = string.Empty;
        public static string CurrentFunctionCategoryID = string.Empty;
        public static string CurrentFunctionsListExecuteJSONFile = string.Empty;
        public static string CurrentFunctionCategoryName = string.Empty;
        public static string CurrentFromCopyFunctionExecuteJsonLocation = string.Empty;
        public static string CurrentToCopyFunctionExecuteJsonLocation = string.Empty;
        public static string CurrentFunctionExecuteJsonLocation = string.Empty;
        public static string CurrentFunctionExecuteJsonShortName = string.Empty;


        public static string CurrentElementXPathValue = string.Empty;
        public static string CurrentAutoExecuteFolder = string.Empty;
        public static string PreviousIsNavigateURL = string.Empty;
        public static bool CurrentIsNavigateURL = false;
        public static int CurrentTabIndex = 0;
        public static string App_Path = string.Empty;
        public static bool isLoadCompleted = false;
        public static List<string> List_Functions = new List<string>();

        public static string LatestFunctionListExecuteJsonFile = string.Empty;

        public static string CurrentMax_StepFile = string.Empty;

        public static string CurrentTestRunningFunctionStepID = string.Empty;

        public static string CurrentStartEndPointURL = string.Empty;
        public static string CurrentDiagram_SelectStepID = string.Empty;

        //#################  scraper ##########################
        public static string Current_Scaper_SaveDataFileName = string.Empty;
        public static string CurrentisDynamicElementXPath = string.Empty;
        public static string CurrentDynamicElementTagName = string.Empty;
        public static string CurrentDynamicElementAttributeName = string.Empty;
        public static string CurrentDynamicElementAttributeValue1 = string.Empty;
        public static string CurrentDynamicElementAttributeValue2 = string.Empty;


        //#################  end scraper  #####################

        //##############  Functions ##########
        public static List<string> CurrentAutomationList = new List<string>();

        public static string CurrentRuningAutoFile = string.Empty;

        public static int CurrentFunctionDetail_Index = 1;

        public static List<FunctionSteps> CurrentAutoFunctionSteps_Lite = new List<FunctionSteps>();
        //############# end function list ######
        #endregion

        public frm_Power_Automation()
        {
            InitializeComponent();
           
        }

        private  void Form1_Load(object sender, EventArgs e)
        {
            InitFormLoad();
           
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string url = "";
            if (webBrowser1.Url != null)
            {
                url = webBrowser1.Url.AbsoluteUri;
                txt_URL.Text = url;
            }
            if (url == "www.google.com")
            {
                //label9.Text = url;
            }
        }

      
        private void bt_Go_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel2.Visible = false;
            DialogResult dialogResult = MessageBox.Show("Is this Start Endpoint URL for your automation?", "Confirm", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                CurrentStartEndPointURL = txt_URL.Text;
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
            webBrowser1.Navigate(txt_URL.Text); //! Navigate to the CodeProject
            webBrowser1.ContextMenuStrip = contextMenuStrip1;    //! Set our ContextMenuStrip
            webBrowser1.IsWebBrowserContextMenuEnabled = false;  //! Disable the default IE ContextMenu
            webBrowser1.ScriptErrorsSuppressed = true;

           // txt_CurrentURL.Text = webBrowser1.Url.AbsoluteUri;
        }



        #region"UI Functions"

        public string GetLatestStepListFile(string DirPath, string spec)
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

        public void GetAllAutomationsList( DataGridView dg)
        {
            dg.Rows.Clear();
            dg.Columns.Clear();

            // dg.ColumnCount = 9;

            DataGridViewCheckBoxColumn c_Select = new DataGridViewCheckBoxColumn();
            c_Select.Name = "Select";
            c_Select.HeaderText = "Select";
            c_Select.Width = 40;
            dg.Columns.Add(c_Select);


            DataGridViewTextBoxColumn c_AutoName = new DataGridViewTextBoxColumn();
            c_AutoName.Name = "AutoName";
            c_AutoName.HeaderText = "AutoName";
            c_AutoName.Width = 130;
            dg.Columns.Add(c_AutoName);


            DataGridViewTextBoxColumn c_AutoDesc = new DataGridViewTextBoxColumn();
            c_AutoDesc.Name = "Description";
            c_AutoDesc.HeaderText = "Description";
            c_AutoDesc.Width = 120;
            dg.Columns.Add(c_AutoDesc);

            DataGridViewTextBoxColumn c_AutoLocation = new DataGridViewTextBoxColumn();
            c_AutoLocation.Name = "Auto Location";
            c_AutoLocation.HeaderText = "Auto Location";
            c_AutoLocation.Width = 130;
            dg.Columns.Add(c_AutoLocation);

            DataGridViewTextBoxColumn c_Status = new DataGridViewTextBoxColumn();
            c_Status.Name = "isActive";
            c_Status.HeaderText = "isActive";
            c_Status.Width = 50;
            dg.Columns.Add(c_Status);

            DataGridViewTextBoxColumn c_TotalSteps = new DataGridViewTextBoxColumn();
            c_TotalSteps.Name = "Total Steps";
            c_TotalSteps.HeaderText = "Total Steps";
            c_TotalSteps.Width = 40;
            dg.Columns.Add(c_TotalSteps);

            DataGridViewTextBoxColumn c_LastRunTime = new DataGridViewTextBoxColumn();
            c_LastRunTime.Name = "Lastest RunTime";
            c_LastRunTime.HeaderText = "Lastest RunTime";
            c_LastRunTime.Width = 95;
            dg.Columns.Add(c_LastRunTime);

            DataGridViewButtonColumn c_Run = new DataGridViewButtonColumn();
            c_Run.Name = "Run";
            c_Run.HeaderText = "Run";
            c_Run.Width = 55;

            c_Run.Text = "Run";
            c_Run.UseColumnTextForButtonValue = true;
            c_Run.FlatStyle = FlatStyle.Standard;
            c_Run.CellTemplate.Style.BackColor = Color.Green;
            dg.Columns.Add(c_Run);

            DataGridViewButtonColumn c_Stop = new DataGridViewButtonColumn();
            c_Stop.Name = "Stop";
            c_Stop.HeaderText = "Stop";
            c_Stop.Text = "Stop";
            c_Stop.UseColumnTextForButtonValue = true;
            c_Stop.Width = 55;
            c_Stop.FlatStyle = FlatStyle.Standard;
            c_Stop.CellTemplate.Style.BackColor = Color.Yellow;
            dg.Columns.Add(c_Stop);

            DataGridViewButtonColumn c_Edit = new DataGridViewButtonColumn();
            c_Edit.Name = "Edit";
            c_Edit.HeaderText = "Edit";
            c_Edit.Text = "Edit";
            c_Edit.UseColumnTextForButtonValue = true;
            c_Edit.Width = 55;
            c_Edit.FlatStyle = FlatStyle.Standard;
            c_Edit.CellTemplate.Style.BackColor = Color.Blue;
            dg.Columns.Add(c_Edit);

            DataGridViewButtonColumn c_Del = new DataGridViewButtonColumn();
            c_Del.Name = "Delete";
            c_Del.HeaderText = "Delete";
            c_Del.Text = "Delete";
            c_Del.UseColumnTextForButtonValue = true;
            c_Del.Width = 55;
            c_Del.FlatStyle = FlatStyle.Standard;
            c_Del.CellTemplate.Style.BackColor = Color.Red;
            dg.Columns.Add(c_Del);

            DataGridViewButtonColumn c_DataResult = new DataGridViewButtonColumn();
            c_DataResult.Name = "Open Result";
            c_DataResult.HeaderText = "Open Result";
            c_DataResult.Width = 130;
            dg.Columns.Add(c_DataResult);

            //progressbar


            DataGridViewProgressColumn column = new DataGridViewProgressColumn();
            dg.Columns.Add(column);
            column.Width = 100;
            // dg.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            column.HeaderText = "Progress";

            using (var db = new LiteDatabase(App_Path + "\\PowerAutoLiteDB.db"))
            {
                using (var bsonReader = db.Execute("SELECT $ FROM Automations order by _id desc"))
                {
                    var output = new List<BsonValue>();
                    int i = 0;
                    while (bsonReader.Read())
                    {
                        Object_AutomationInfo itemAuto = new Object_AutomationInfo();
                        string r_AutoName = string.Empty;
                        string r_Description = string.Empty;
                        string r_AutoStepFunctionLocation = string.Empty;
                        string r_IsActive = string.Empty;
                        string r_DataResult = string.Empty;
                        string r_MaxSteps = string.Empty;
                        string r_LastRunTime = string.Empty;


                        output.Add(bsonReader.Current);

                        //if (output[i]["AutoName"].IsString) 
                        //{
                        r_AutoName = output[i]["AutoName"].ToString().TrimStart('"').TrimEnd('"');
                        r_Description = output[i]["Description"].ToString().TrimStart('"').TrimEnd('"');
                        r_AutoStepFunctionLocation = output[i]["AutoStepFunctionLocation"].ToString().TrimStart('"').TrimEnd('"');
                        r_IsActive = output[i]["IsActive"].ToString();
                        r_DataResult = output[i]["Run_ResultDataFileName"].ToString().TrimStart('"').TrimEnd('"');
                        r_MaxSteps = output[i]["MaxSteps"].ToString();
                        r_LastRunTime = output[i]["Run_LastRunTime"].ToString();

                        Random rand = new Random();
                        Random rand1 = new Random();
                        int randomNumber = 0;
                        randomNumber=rand.Next(0, 50)+ rand1.Next(0, 50);
                        dg.Rows.Add(false, r_AutoName, r_Description, r_AutoStepFunctionLocation, r_IsActive, r_MaxSteps, r_LastRunTime, "Run", "Stop", "Edit", "Delete","Open Result", randomNumber);
                        //}
                        i++;
                    }

                    // return output;
                }
            }


        }
        public void LoadAutoNames()
        {
            App_Path = System.IO.Directory.GetCurrentDirectory();
            string SettingFile = App_Path + "\\settings\\Power_AutomationNames.json";
            //object►Connections►0►ID
            JO_AutoNames = JObject.Parse(File.ReadAllText(SettingFile));
            // get all the orders first, then go inside get the items
            int count = JO_AutoNames["AutoNames"].ToList().Count;
            for (int i = 0; i <= count - 1; i++)
            {

                string AutoName = JO_AutoNames["AutoNames"][i].SelectToken("FunctionName").ToString();
                string TabName = JO_AutoNames["AutoNames"][i].SelectToken("TabName").ToString();
                List_Functions.Add(AutoName);

            }
        }
        public void InitEverything()
        {

            tab_AutoConfigs.SelectedIndex = 0;
            txt_AutoName.Text = "";
            txt_URL.Text = "";
            txt_XPath.Text = "";


            txt_Tab1_SendValue.Text = "";
            ck_Tab1_isStartURL.Checked = false;
            ck_Tab2_isStartURL.Checked = false;
            ck_Tab3_isStartURL.Checked = false;
            webBrowser1.Navigate("google.com");
            PreviousIsNavigateURL = "";
            txt_URL.Text = "google.com";

            //    List_AutoNamesandTabs = new List<string>();
            // JO_AutoNames= new JObject();
            //JO_ExecuteFunctionJson = new JObject();
            //  JO_ExecuteFunctionListJson = new JObject();

            CurrentAutoName = string.Empty;
            CurrentStepID = string.Empty;

            CurrentFunctionName = string.Empty;
            CurrentFunctionCategoryID = string.Empty;
            CurrentFunctionsListExecuteJSONFile = string.Empty;
            CurrentFunctionCategoryName = string.Empty;
            CurrentFromCopyFunctionExecuteJsonLocation = string.Empty;
            CurrentToCopyFunctionExecuteJsonLocation = string.Empty;
            CurrentFunctionExecuteJsonLocation = string.Empty;
            CurrentFunctionExecuteJsonShortName = string.Empty;


            CurrentElementXPathValue = string.Empty;
            CurrentAutoExecuteFolder = string.Empty;
            PreviousIsNavigateURL = string.Empty;
            CurrentIsNavigateURL = false;
            CurrentTabIndex = 0;

            isLoadCompleted = false;
            List_Functions = new List<string>();

            LatestFunctionListExecuteJsonFile = string.Empty;

            CurrentMax_StepFile = string.Empty;

            CurrentTestRunningFunctionStepID = string.Empty;

            diagram1.Clear();

        }
        public Model LoadDiagramModel(string CurrentAutoName)
        {

            //LatestFunctionListExecuteJsonFile = GetLatestStepListFile(App_Path + "\\Functions\\", "*FunctionSteps.json");
            //JO_ExecuteFunctionListJson = JObject.Parse(File.ReadAllText(LatestFunctionListExecuteJsonFile));


            //string FunctionFolder = JO_ExecuteFunctionListJson["AutoName"].ToString();
            //CurrentAutoName = JO_ExecuteFunctionListJson["AutoName"].ToString();
            ////int StepCounts = JO_ExecuteFunctionListJson["StepList"].Count();

            //get the currentAutoName steps
            List<FunctionSteps> CurrentAutoFunctionList = LiteDB_Action.GetFunctionSteps_Lite_ByAutoName(CurrentAutoName);
            int StepCounts = CurrentAutoFunctionList.Count();

            Model model = new Model();

            for (int i = 0; i <= StepCounts - 1; i++)
            {
                //string StepFunctionJSONFileFullName = App_Path + "\\Functions\\" + FunctionFolder + "\\" + JO_ExecuteFunctionListJson["StepList"][i].SelectToken("FunctionDetail").SelectToken("FunctionExecuteJsonLocation").ToString();

               // try { JO_ExecuteFunctionJson = JObject.Parse(File.ReadAllText(LatestFunctionListExecuteJsonFile)); } catch (Exception e2) { break; }

                //string StepID = JO_ExecuteFunctionListJson["StepList"][i].SelectToken("StepID").ToString();
                //string FunctionName = JO_ExecuteFunctionListJson["StepList"][i].SelectToken("FunctionDetail").SelectToken("FunctionName").ToString();

                string StepID = CurrentAutoFunctionList[i].StepID.ToString();
                string FunctionName = CurrentAutoFunctionList[i].FunctionName.ToString();

                //check if the function is actually created
                Color BorderColor = Color.Blue;
                Color BackColor = Color.White;

                if (!FunctionName.Contains("FunctionName_SampleXXX_StepID"))
                {
                    if (FunctionName.Contains("F_Click_Action"))
                    {
                        BackColor = Color.Yellow;
                    }
                    if (FunctionName.Contains("F_Send_Text"))
                    {
                        BackColor = Color.Orange;
                    }
                    if (FunctionName.Contains("F_Send_Key"))
                    {
                        BackColor = Color.Blue;
                    }
                    if (FunctionName.Contains("F_Data_Scra"))
                    {
                        BackColor = Color.Gold;
                    }
                    Table table = new Table();

                    //Set Element properties
                    table.Location = new PointF(10, 3 + i * 65);
                    table.Width = 220;
                    table.Height = 10;
                    table.Indent = 2;
                    table.Heading = "Customized Function Step:" + StepID;
                    table.SubHeading = FunctionName;
                    table.DrawExpand = true;


                    ////Add the fields group
                    TableGroup fieldGroup = new TableGroup();
                    //fieldGroup.Text = "Fields";
                    //table.Groups.Add(fieldGroup);

                    ////Add the fields rows
                    ////Layer
                    TableRow row = new TableRow();
                    //row.Text = "Layer";
                    ////Crainiate.Diagramming.Forms
                    ////row.Image = new Crainiate.Diagramming.Image("Resource.publicfield.gif", "frm_Power_Automation");
                    fieldGroup.Rows.Add(row);

                    ////SuspendEvents
                    //row = new TableRow();
                    //row.Text = "SuspendEvents";
                    //// row.Image = new Crainiate.Diagramming.Image("Resource.protectedfield.gif", "frm_Power_Automation");
                    //fieldGroup.Rows.Add(row);

                    ////Add the methods group
                    TableGroup methodGroup = new TableGroup();
                    //methodGroup.Text = "Methods";
                    //table.Groups.Add(methodGroup);

                    ////Add the methods rows
                    ////AddPath
                    //row = new TableRow();
                    //row.Text = "AddPath";
                    ////row.Image = new Crainiate.Diagramming.Image("Resource.publicmethod.gif", "Crainiate.Diagramming.Examples.Forms.frmClassDiagram");
                    //methodGroup.Rows.Add(row);

                    //row = new TableRow();
                    //// row.Image = new Crainiate.Diagramming.Image("Resource.protectedmethod.gif", "Crainiate.Diagramming.Examples.Forms.frmClassDiagram");
                    //row.Text = "SetLayer";
                    //methodGroup.Rows.Add(row);

                    //Add Element to model

                    model.Shapes.Add(StepID, table);
                    if (StepID == CurrentTestRunningFunctionStepID)
                    {
                        table.Heading = "Function Step:" + StepID + "--Test Completed";
                        model.Shapes[StepID].BorderWidth = 8;
                        model.Shapes[StepID].BorderColor = Color.Green;
                    }

                    model.Shapes[StepID].BackColor = BackColor;

                    //table = new Table();

                    ////Set SolidElement properties
                    //table.Location = new PointF(100, 250);
                    //table.Width = 140;
                    //table.Height = 500;
                    //table.Indent = 10;
                    //table.Heading = StepID;
                    //// table.SubHeading = "Class";
                    //// table.DrawExpand = true;

                    ////Add the fields group
                    //// fieldGroup = new TableGroup();
                    //// fieldGroup.Text = "Fields";
                    //// table.Groups.Add(fieldGroup);

                    ////Add the fields rows
                    ////BackColor
                    //row = new TableRow();
                    //row.Text = "BackColor";
                    ////row.Image = new Crainiate.Diagramming.Image("Resource.publicfield.gif", "Crainiate.Diagramming.Examples.Forms.frmClassDiagram");
                    //fieldGroup.Rows.Add(row);

                    ////Add the methods group
                    //methodGroup = new TableGroup();
                    //methodGroup.Text = "Functions";
                    //table.Groups.Add(methodGroup);

                    ////Add the methods rows
                    ////ScalePath
                    //row = new TableRow();
                    //row.Text = FunctionName;
                    //// row.Image = new Crainiate.Diagramming.Image("Resource.publicmethod.gif", "Crainiate.Diagramming.Examples.Forms.frmClassDiagram");
                    //methodGroup.Rows.Add(row);

                    ////Add Element to model
                    //model.Shapes.Add(StepID, table);

                    //table = new Table();

                    ////Set Shape properties
                    //table.Location = new PointF(100, 410);
                    //table.Width = 140;
                    //table.Height = 500;
                    //table.Indent = 10;
                    //table.Heading = "Shape";
                    //table.SubHeading = "Class";
                    //table.DrawExpand = true;

                    ////Add the fields group
                    //fieldGroup = new TableGroup();
                    //fieldGroup.Text = "Fields";
                    //table.Groups.Add(fieldGroup);

                    ////Add the fields rows
                    ////AllowMove
                    //row = new TableRow();
                    //row.Text = "AllowMove";
                    //// row.Image = new Crainiate.Diagramming.Image("Resource.publicfield.gif", "Crainiate.Diagramming.Examples.Forms.frmClassDiagram");
                    //fieldGroup.Rows.Add(row);

                    ////Add the methods group
                    //methodGroup = new TableGroup();
                    //methodGroup.Text = "Methods";
                    //table.Groups.Add(methodGroup);

                    ////Add the methods rows
                    ////Rotate
                    //row = new TableRow();
                    //row.Text = "Rotate";
                    //// row.Image = new Crainiate.Diagramming.Image("Resource.publicmethod.gif", "Crainiate.Diagramming.Examples.Forms.frmClassDiagram");
                    //methodGroup.Rows.Add(row);

                    ////Add Element to model
                    //model.Shapes.Add(StepID, table);

                    ////Add a Layer class
                    //table = new Table();

                    ////Set Shape properties
                    //table.Location = new PointF(400, 100);
                    //table.Width = 140;
                    //table.Height = 500;
                    //table.Indent = 10;
                    //table.Heading = "Layer";
                    //table.SubHeading = "Class";
                    //table.DrawExpand = true;

                    ////Add the fields group
                    //fieldGroup = new TableGroup();
                    //fieldGroup.Text = "Fields";
                    //table.Groups.Add(fieldGroup);

                    ////Add the fields rows
                    ////DrawShadows
                    //row = new TableRow();
                    //row.Text = "DrawShadows";
                    //// row.Image = new Crainiate.Diagramming.Image("Resource.publicfield.gif", "Crainiate.Diagramming.Examples.Forms.frmClassDiagram");
                    //fieldGroup.Rows.Add(row);

                    ////Elements
                    //row = new TableRow();
                    //row.Text = "Elements";
                    ////row.Image = new Crainiate.Diagramming.Image("Resource.publicfield.gif", "Crainiate.Diagramming.Examples.Forms.frmClassDiagram");
                    //fieldGroup.Rows.Add(row);

                    ////Opacity
                    //row = new TableRow();
                    //row.Text = "Opacity";
                    ////row.Image = new Crainiate.Diagramming.Image("Resource.publicfield.gif", "Crainiate.Diagramming.Examples.Forms.frmClassDiagram");
                    //fieldGroup.Rows.Add(row);

                    ////SuspendEvents
                    //row = new TableRow();
                    //row.Text = "SuspendEvents";
                    //// row.Image = new Crainiate.Diagramming.Image("Resource.protectedfield.gif", "Crainiate.Diagramming.Examples.Forms.frmClassDiagram");
                    //fieldGroup.Rows.Add(row);

                    ////Add Layer to model
                    //model.Shapes.Add("Layer", table);

                    ////Add GradientMode enumeration shape
                    //table = new Table();

                    ////Set Shape properties
                    //table.BackColor = Color.White;
                    //table.Location = new PointF(400, 300);
                    //table.Width = 140;
                    //table.Height = 500;
                    //table.Indent = 10;
                    //table.Heading = "GradientMode";
                    //table.SubHeading = "Enum";
                    //table.DrawExpand = true;

                    ////Add the fields rows
                    ////BackwardDiagonal
                    //row = new TableRow();
                    //row.Text = "BackwardDiagonal";
                    //table.Rows.Add(row);

                    ////ForwardDiagonal
                    //row = new TableRow();
                    //row.Text = "ForwardDiagonal";
                    //table.Rows.Add(row);

                    ////Vertical
                    //row = new TableRow();
                    //row.Text = "Vertical";
                    //table.Rows.Add(row);

                    ////Horizontal
                    //row = new TableRow();
                    //row.Text = "Horizontal";
                    //table.Rows.Add(row);

                    ////Add GradientMode to model
                    //model.Shapes.Add("GradientMode", table);

                    ////Connect
                    //Create an arrow line marker
                    Arrow arrow = new Arrow();
                    arrow.DrawBackground = false;
                    arrow.Inset = 5;

                    //Add link between shape and solid
                    int nextStep = int.Parse(StepID) - 1;

                    try
                    {
                        Connector line = new Connector(model.Shapes[nextStep.ToString()], model.Shapes[StepID]);
                        line.End.Marker = arrow;
                        model.Lines.Add(model.Lines.CreateKey(), line);
                    }
                    catch (Exception e2)
                    {
                    }

                }
            }

            return model;
        }
        private void DisplayTreeView(JToken root, string rootName)
        {
            treeView1.BeginUpdate();
            try
            {
                treeView1.Nodes.Clear();
                var tNode = treeView1.Nodes[treeView1.Nodes.Add(new TreeNode(rootName))];
                tNode.Tag = root;

                AddNode(root, tNode);

                treeView1.ExpandAll();
            }
            finally
            {
                treeView1.EndUpdate();
            }
        }
        private void AddNode(JToken token, TreeNode inTreeNode)
        {
            if (token == null)
                return;
            if (token is JValue)
            {
                var childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode(token.ToString()))];
                childNode.Tag = token;
            }
            else if (token is JObject)
            {
                var obj = (JObject)token;
                foreach (var property in obj.Properties())
                {
                    var childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode(property.Name))];
                    childNode.Tag = property;

                    string NodeText = property.Name.ToString();

                    childNode.ImageKey = NodeText + ".bmp";


                    AddNode(property.Value, childNode);
                }
            }
            else if (token is JArray)
            {
                var array = (JArray)token;
                for (int i = 0; i < array.Count; i++)
                {
                    //var childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode(i.ToString()))];
                    var childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode(array[i].ToString()))];
                    // if (array[i].ToString().Contains("Web"))
                    // {
                    string NodeText = array[i].ToString();
                    childNode.ImageKey = NodeText + ".bmp";

                }
            }
            else
            {
                Console.WriteLine(string.Format("{0} not implemented", token.Type)); // JConstructor, JRaw
            }
        }
        public void LoadFunctionsTreeview()
        {
            using (var reader = new StreamReader(App_Path + "\\settings\\FunctionTemplates\\Click to Create Functions Step.json"))
            using (var jsonReader = new JsonTextReader(reader))
            {
                var root = JToken.Load(jsonReader);
                DisplayTreeView(root, Path.GetFileNameWithoutExtension(App_Path + "\\settings\\FunctionTemplates\\Click to Create Functions Step.json"));
            }
        }
        public void InitFormLoad()
        {
            webBrowser1.ContextMenuStrip = contextMenuStrip1;    //! Set our ContextMenuStrip
            webBrowser1.IsWebBrowserContextMenuEnabled = false;  //! Disable the default IE ContextMenu
            webBrowser1.ScriptErrorsSuppressed = true;
            LoadAutoNames();
            cmb_InEvery.SelectedIndex = 0;

            tab_AutoConfigs.SelectedIndex = 0;
            isLoadCompleted = true;

            try
            {
                LatestFunctionListExecuteJsonFile = GetLatestStepListFile(App_Path + "\\Functions\\", "*FunctionSteps.json");
                JO_ExecuteFunctionListJson = JObject.Parse(File.ReadAllText(LatestFunctionListExecuteJsonFile));


                diagram1.Controller.Model = LoadDiagramModel(CurrentAutoName);

                diagram1.Controller.Refresh();
                txt_AutoName.Text = CurrentAutoName;
                CurrentFunctionsListExecuteJSONFile = App_Path + "\\Functions\\" + CurrentAutoName + "\\" + CurrentAutoName + "_FunctionSteps.json";
            }
            catch (Exception e2)
            {

            }
            LoadFunctionsTreeview();
            GetAllAutomationsList( dg_AutoList);
            tab_Main.TabPages[0].Select();


        }
        public void checkAutoNameEmpty()
        {
            if (txt_AutoName.Text == "" & isLoadCompleted)
            {
                DialogResult dialogResult = MessageBox.Show("Please Enter Auto Name First", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

        }
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

            //! Screen coordinates
            Point ScreenCoord = new Point(MousePosition.X, MousePosition.Y);
            //! Browser coordinates
            Point BrowserCoord = webBrowser1.PointToClient(ScreenCoord);

            HtmlElement element = this.webBrowser1.Document.GetElementFromPoint(BrowserCoord);

            var savedId = element.Id;
            var uniqueId = Guid.NewGuid().ToString();
            element.Id = uniqueId;

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(element.Document.GetElementsByTagName("html")[0].OuterHtml);

            element.Id = savedId;
            //element.Focus();

            var node = doc.GetElementbyId(uniqueId);
            var xpath = node.XPath;
            txt_XPath.Text = xpath;
            CurrentElementXPathValue = xpath;
            hightLightElement("input");
            hightLightElement("td");
            hightLightElement("option");
            hightLightElement("tr");

        }
        public void TreeviewSelectFunction(string selectedText)
        {
            ck_Tab1_isStartURL.Checked = false;
            ck_Tab2_isStartURL.Checked = false;
            ck_Tab3_isStartURL.Checked = false;
            txt_Tab1_SendValue.Text = "";

            // JObject match = JO_AutoNames["AutoNames"].Values<JObject>()
            //.Where(m => m["AutoName"].Value<string>() == selectedText)
            //.FirstOrDefault();
            JObject match = JO_AutoNames["AutoNames"].Values<JObject>()
         .Where(m => m["FunctionName"].Value<string>().Contains(selectedText))
         .FirstOrDefault();

            for (int i = 0; i <= 20; i++)
            {
                try
                {
                    tab_AutoConfigs.TabPages[i].BackColor = Color.Transparent;
                }
                catch (Exception e2)
                {
                }
            }

            if (match != null)
            {
                int TabIndex = int.Parse(match["ID"].ToString()) - 1;
                tab_AutoConfigs.SelectedIndex = TabIndex;
                CurrentTabIndex = int.Parse(match["ID"].ToString());
                try { tab_AutoConfigs.TabPages[TabIndex].BackColor = Color.LightYellow; } catch (Exception e1) { }
                CurrentFunctionName = match["FunctionName"].ToString();
                string FunctionTemplateFile = match["FunctionTemplateFile"].ToString();
                int intCurrentStepID = GetStepMaxFromFile() + 1;
                CurrentStepID = intCurrentStepID.ToString();
                if (CurrentStepID == "0")
                {
                    CurrentStepID = "1";
                }
                CurrentAutoExecuteFolder = App_Path + "\\Functions\\" + txt_AutoName.Text;
                // create the function step json file if the AppName is not empty
                string AppName = txt_AutoName.Text;
                // match ID CASE
                if (match["ID"].ToString() == "11")
                {
                    frm_PageTopBottom pagetopb = new frm_PageTopBottom();
                    pagetopb.Show();
                    pagetopb.TopMost = true;
                }
                //end id case

                if (AppName != "")
                {

                    string targetFileBase = App_Path + "\\Functions\\" + txt_AutoName.Text;
                    string targetFileSteps = targetFileBase + "\\" + txt_AutoName.Text + "_FunctionSteps.json";
                    string targetFileSteps1 = targetFileBase + "\\" + txt_AutoName.Text + "_FunctionSteps1.json";
                    string targetFileStepFunction = targetFileBase + "\\" + txt_AutoName.Text + "_STEP_" + CurrentStepID + "_" + CurrentFunctionName + "_Function.json";
                    CurrentFunctionExecuteJsonShortName = txt_AutoName.Text + "_STEP_" + CurrentStepID + "_" + CurrentFunctionName + "_Function.json";
                    string targetFileMaxStep = targetFileBase + "\\" + txt_AutoName.Text + "_MaxStep.ini";
                    string AutoName = txt_AutoName.Text;
                    CurrentFunctionsListExecuteJSONFile = targetFileSteps;
                    CurrentAutoName = AppName;
                    //################### create the folder if not exist
                    System.IO.Directory.CreateDirectory(targetFileBase);
                    //check if the target file is exist, means it may have some step setup before
                    if (!File.Exists(targetFileSteps))
                    {
                        // it is new setup, copy the template file only
                        File.Copy(App_Path + "\\settings\\FunctionTemplates\\FunctionStepsList_Template.json", targetFileSteps);

                    }
                    if (!File.Exists(targetFileMaxStep))
                    {
                        // it is new setup, copy the template file only
                        File.Copy(App_Path + "\\settings\\FunctionTemplates\\Power_MaxStep.ini", targetFileMaxStep);
                        CurrentMax_StepFile = targetFileMaxStep;


                    }
                    if (!File.Exists(targetFileStepFunction))
                    {

                        CurrentFromCopyFunctionExecuteJsonLocation = App_Path + "\\settings\\FunctionTemplates\\" + FunctionTemplateFile;
                        File.Copy(CurrentFromCopyFunctionExecuteJsonLocation, targetFileStepFunction);
                        CurrentToCopyFunctionExecuteJsonLocation = targetFileStepFunction;
                    }

                    //###########################  lite db to insert the automation detail
                    LiteDB_Action.AddNewAuto_Lite(txt_AutoName.Text, txt_Description.Text, targetFileSteps);

                    //###########################  end litedb insert


                }
                //end 
            }
            else
            {
                Console.WriteLine("match not found");
            }

        }

        public void TreeviewSelectOpenFunctionForm(string selectedText)
        {
            switch (selectedText)
            {
                case "T_Web_Actions_SendText":
                    frm_Function_Sel_SendText frmSelSendText = new frm_Function_Sel_SendText();
                    frmSelSendText.ShowDialog();
                    break;
               
                default:
                    // code block
                    break;
            }

            //###########################  lite db to insert the automation detail
            //LiteDB_Action.AddNewAuto_Lite(txt_AutoName.Text, txt_Description.Text, "Actually no need this");

                    //###########################  end litedb insert


              

        }

        public void hightLightElement(string elementDomType)
        {
            HtmlElement head = webBrowser1.Document.GetElementsByTagName("head")[0];
            HtmlElement script = webBrowser1.Document.CreateElement("script");
            HtmlElement JQueryscript = webBrowser1.Document.CreateElement("script");
            IHTMLScriptElement jQueryElement = (IHTMLScriptElement)JQueryscript.DomElement;
            jQueryElement.src = "http://ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js";

            IHTMLScriptElement domElement = (IHTMLScriptElement)script.DomElement;

            //  webBrowser1.ShowSaveAsDialog();
            domElement.type = "text/javascript";
            domElement.text = "function doIt(){$(document).ready( function(){$('" + elementDomType + "').mouseover( function(event){event.stopPropagation(); $(this).css('border-color','red'); $(this).css('border-width','4px'); $(this).css('border-style','solid'); }).mouseout(function (event) { event.stopPropagation();$(this).css('border-color',''); $(this).css('border-width','');$(this).css('border-style','');}); $('div').click(function(event){event.stopPropagation();alert(getElementPath(this));})}); function getElementPath(element){ return'//'+ $(element).parents().andSelf().map(function(){ var $this=$(this);var tagName=this.nodeName;if($this.siblings(tagName).length>0){ tagName +='['+$this.prevAll(tagName).length+']';}return tagName;}).get().join('/').toUpperCase();}}";
            //domElement.text = "function doIt(){$(document).ready( function(){$('div').mouseover( function(event){event.stopPropagation(); $(this).css('border-color','red'); $(this).css('border-width','4px'); $(this).css('border-style','solid'); }).mouseout(function (event) { event.stopPropagation();$(this).css('border-color',''); $(this).css('border-width','');$(this).css('border-style','');}); $('div').click(function(event){event.stopPropagation(););})}); function getElementPath(element){ return'//'+ $(element).parents().andSelf().map(function(){ var $this=$(this);var tagName=this.nodeName;if($this.siblings(tagName).length>0){ tagName +='['+$this.prevAll(tagName).length+']';}return tagName;}).get().join('/').toUpperCase();}}";
            // domElement.text = "function doIt(){$(document).ready( function(){$('select').mouseover( function(event){event.stopPropagation(); $(this).css('border-color','red'); $(this).css('border-width','4px'); $(this).css('border-style','solid'); }).mouseout(function (event) { event.stopPropagation();$(this).css('border-color',''); $(this).css('border-width','');$(this).css('border-style','');}); $('div').click(function(event){event.stopPropagation();alert(getElementPath(this));})}); function getElementPath(element){ return'//'+ $(element).parents().andSelf().map(function(){ var $this=$(this);var tagName=this.nodeName;if($this.siblings(tagName).length>0){ tagName +='['+$this.prevAll(tagName).length+']';}return tagName;}).get().join('/').toUpperCase();}}";
            head.AppendChild(JQueryscript);
            head.AppendChild(script);
            webBrowser1.Document.InvokeScript("doIt");
        }
        #endregion

      

        #region "Steps Fnnctions"
        public async void RunAutomation_Async(string _CurrentFunctionsListExecuteJSONFile)
        {

            // first, get the function list step file
            try
            {
                JO_ExecuteFunctionListJson = JObject.Parse(File.ReadAllText(_CurrentFunctionsListExecuteJSONFile));
            }
            catch (Exception e1)
            {
                //it must be just init, get the latest steplist json file from the functions folder
                string latestStepListFileName = GetLatestStepListFile(App_Path + "\\Functions\\", "*FunctionSteps.json");
                JO_ExecuteFunctionListJson = JObject.Parse(File.ReadAllText(latestStepListFileName));
            }
            //now goto each step and pick up the sub function json file and excute them one by one
            //object►AutoName
            string FunctionFolder = JO_ExecuteFunctionListJson["AutoName"].ToString();
            var chromeOptions = new ChromeOptions();
            //hide the debug window
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            //var driver = new ChromeDriver(driverService, new ChromeOptions());
            //*[@id="drpSelectedCustomer"]/option[14]
            //end hide the debug window
            string downloadDirectory = @"";

            chromeOptions.AddUserProfilePreference("download.default_directory", downloadDirectory);
            chromeOptions.AddUserProfilePreference("download.prompt_for_download", false);
            chromeOptions.AddUserProfilePreference("disable-popup-blocking", "true");
            chromeOptions.AddArgument("--start-maximized");

            IWebDriver driver = new ChromeDriver(driverService, chromeOptions);

            //object►StepList►0►FunctionDetail►FunctionExecuteJsonLocation
            int StepCounts = JO_ExecuteFunctionListJson["StepList"].Count();
            for (int i = 0; i <= StepCounts - 1; i++)
            {
            Nexti:
                string StepFunctionJSONFileFullName = string.Empty;
                try
                {
                    StepFunctionJSONFileFullName = App_Path + "\\Functions\\" + FunctionFolder + "\\" + JO_ExecuteFunctionListJson["StepList"][i].SelectToken("FunctionDetail").SelectToken("FunctionExecuteJsonLocation").ToString();
                }
                catch (Exception e3)
                {
                    //it must runout of the steps
                    break;
                }

                string RuuningSetpID = JO_ExecuteFunctionListJson["StepList"][i].SelectToken("StepID").ToString();
                CurrentTestRunningFunctionStepID = RuuningSetpID;

                try
                {
                    JO_ExecuteFunctionJson = JObject.Parse(File.ReadAllText(StepFunctionJSONFileFullName));
                }
                catch (Exception e2)
                {
                    i = i + 1;
                    goto Nexti;
                    //break; 
                } //if exception, then no json file to excute
                //object►FunctionCategoryID
                //check if the ID belong to selenium
                string FunctionCategoryID = JO_ExecuteFunctionJson.SelectToken("FunctionCategoryID").ToString();
                if (FunctionCategoryID == "AutoCate_SEL_01")
                {
                    //object►FunctionDetail►0►isStartPoint
                    //             "isNavigateURL":"True",
                    //"NavigateURL":"https://covtxhou01-22.covalentworks.com/MyB2B_Ver1/weblogin.aspx?ReturnUrl=%2fMyB2B_Ver1%2fPO%2fSearch.aspx%3fSenderID%3d08925485USSM%26ReceiverID%3d01933339053",	 
                    //   "ElementXPath":"/html[1]/body[1]/table[1]/tbody[1]/tr[1]/td[1]/form[1]/table[1]/tbody[1]/tr[7]/td[3]/table[1]/tbody[1]/tr[2]/td[2]/input[1]",
                    //"SendValue":"5435345435",
                    //"AfterDoneWaitSeconds":"0"

                    string isNavigateURL = JO_ExecuteFunctionJson.SelectToken("FunctionDetail")[0].SelectToken("isNavigateURL").ToString();
                    string NavigateURL = JO_ExecuteFunctionJson.SelectToken("FunctionDetail")[0].SelectToken("NavigateURL").ToString();
                    string Action = JO_ExecuteFunctionJson.SelectToken("FunctionDetail")[0].SelectToken("Action").ToString();
                    string ElementXPath = JO_ExecuteFunctionJson.SelectToken("FunctionDetail")[0].SelectToken("ElementXPath").ToString().Replace("'", "\"");
                    string AfterDoneWaitSeconds = JO_ExecuteFunctionJson.SelectToken("FunctionDetail")[0].SelectToken("AfterDoneWaitSeconds").ToString();
                    int waitMiseconds = 0;
                    try { waitMiseconds = int.Parse(AfterDoneWaitSeconds) * 1000; } catch (Exception e1) { waitMiseconds = 3000; }

                    string isDynamicElementXPath = string.Empty;
                    string DynamicElementTagName = string.Empty;
                    string DynamicElementAttributeName = string.Empty;
                    string DynamicElementAttributeValue1 = string.Empty;
                    string DynamicElementAttributeValue2 = string.Empty;
                    ////////////////////////////////////////
                    try
                    {
                        JToken j_isDynamicElementXPath = JO_ExecuteFunctionJson.SelectToken("FunctionDetail")[0].SelectToken("isDynamicElementXPath");
                        if (j_isDynamicElementXPath != null)
                        {
                            isDynamicElementXPath = JO_ExecuteFunctionJson.SelectToken("FunctionDetail")[0].SelectToken("isDynamicElementXPath").ToString();
                        }

                    }
                    catch (Exception e1)
                    {
                        isDynamicElementXPath = "False";
                    }
                    ////////////////////////////////////////
                    try
                    {
                        JToken j_DynamicElementTagName = JO_ExecuteFunctionJson.SelectToken("FunctionDetail")[0].SelectToken("DynamicElementTagName");
                        if (j_DynamicElementTagName != null)
                        {
                            DynamicElementTagName = JO_ExecuteFunctionJson.SelectToken("FunctionDetail")[0].SelectToken("DynamicElementTagName").ToString();
                        }

                    }
                    catch (Exception e1)
                    {
                        DynamicElementTagName = "";
                    }

                    ////////////////////////////////////////
                    try
                    {
                        JToken j_DynamicElementAttributeName = JO_ExecuteFunctionJson.SelectToken("FunctionDetail")[0].SelectToken("DynamicElementAttributeName");
                        if (j_DynamicElementAttributeName != null)
                        {
                            DynamicElementAttributeName = JO_ExecuteFunctionJson.SelectToken("FunctionDetail")[0].SelectToken("DynamicElementAttributeName").ToString();
                        }

                    }
                    catch (Exception e1)
                    {
                        DynamicElementAttributeName = "";
                    }

                    ////////////////////////////////////////
                    try
                    {
                        JToken j_DynamicElementAttributeValue1 = JO_ExecuteFunctionJson.SelectToken("FunctionDetail")[0].SelectToken("DynamicElementAttributeValue1");
                        if (j_DynamicElementAttributeValue1 != null)
                        {
                            DynamicElementAttributeValue1 = JO_ExecuteFunctionJson.SelectToken("FunctionDetail")[0].SelectToken("DynamicElementAttributeValue1").ToString();
                        }

                    }
                    catch (Exception e1)
                    {
                        DynamicElementAttributeValue1 = "";
                    }

                    ////////////////////////////////////////
                    try
                    {
                        JToken j_DynamicElementAttributeValue2 = JO_ExecuteFunctionJson.SelectToken("FunctionDetail")[0].SelectToken("DynamicElementAttributeValue2");
                        if (j_DynamicElementAttributeValue2 != null)
                        {
                            DynamicElementAttributeValue2 = JO_ExecuteFunctionJson.SelectToken("FunctionDetail")[0].SelectToken("DynamicElementAttributeValue2").ToString();
                        }

                    }
                    catch (Exception e1)
                    {
                        DynamicElementAttributeValue2 = "";
                    }



                    if (isNavigateURL == "True")
                    {
                        driver.Url = NavigateURL; //starting point 
                    }
                    //########################### send text ##################################
                    if (Action == "Send Value")
                    {
                        string SendValue = JO_ExecuteFunctionJson.SelectToken("FunctionDetail")[0].SelectToken("SendValue").ToString();

                        //solid element case
                        if (isDynamicElementXPath != "True")
                        {
                            try
                            {
                                driver.FindElement(By.XPath(ElementXPath)).SendKeys(SendValue);
                            }
                            catch (Exception e2)
                            {
                                Thread.Sleep(3000); //try first time
                                try
                                {
                                    driver.FindElement(By.XPath(ElementXPath)).SendKeys(SendValue);
                                }
                                catch (Exception e3)
                                {
                                    Thread.Sleep(3000);  //try 2nd time
                                    try
                                    {
                                        driver.FindElement(By.XPath(ElementXPath)).SendKeys(SendValue);
                                    }
                                    catch (Exception e4)
                                    {
                                        Thread.Sleep(3000);  //try 3rd time
                                        try
                                        {
                                            driver.FindElement(By.XPath(ElementXPath)).SendKeys(SendValue);
                                        }
                                        catch (Exception e5)
                                        {
                                        }
                                    }
                                }

                            }
                        }
                        //dynamic case
                        if (isDynamicElementXPath == "True")
                        {
                            string DynamicXPathValue = string.Empty;
                            IList<IWebElement> all_Elements = null;
                            all_Elements = driver.FindElements(By.XPath("//" + DynamicElementTagName + "[contains(@" + DynamicElementAttributeName + ",'" + DynamicElementAttributeValue1 + "') and contains(@" + DynamicElementAttributeName + ",'" + DynamicElementAttributeValue2 + "') ]"));



                            foreach (IWebElement item in all_Elements)
                            {
                                // DynamicXPathValue = item.GetAttribute("id");
                                //string skuvalue_href = item.Text;
                                try { item.SendKeys(SendValue); } catch (Exception e4) { }

                            }

                            try
                            {
                                driver.FindElement(By.XPath(ElementXPath)).SendKeys(SendValue);
                            }
                            catch (Exception e2)
                            {
                                Thread.Sleep(3000); //try first time
                                try
                                {
                                    driver.FindElement(By.XPath(ElementXPath)).SendKeys(SendValue);
                                }
                                catch (Exception e3)
                                {
                                    Thread.Sleep(3000);  //try 2nd time
                                    try
                                    {
                                        driver.FindElement(By.XPath(ElementXPath)).SendKeys(SendValue);
                                    }
                                    catch (Exception e4)
                                    {
                                        Thread.Sleep(3000);  //try 3rd time
                                        try
                                        {
                                            driver.FindElement(By.XPath(ElementXPath)).SendKeys(SendValue);
                                        }
                                        catch (Exception e5)
                                        {
                                        }
                                    }
                                }

                            }
                        }

                    }
                    //########################### click something ##################################
                    if (Action == "Click Button")
                    {
                        if (isDynamicElementXPath != "True")
                        {
                            try
                            {
                                driver.FindElement(By.XPath(ElementXPath)).Click();
                            }
                            catch (Exception e2)
                            {
                                Thread.Sleep(3000); //try first time
                                try
                                {
                                    driver.FindElement(By.XPath(ElementXPath)).Click();
                                }
                                catch (Exception e3)
                                {
                                    Thread.Sleep(3000);  //try 2nd time
                                    try
                                    {
                                        driver.FindElement(By.XPath(ElementXPath)).Click();
                                    }
                                    catch (Exception e4)
                                    {
                                        Thread.Sleep(3000);  //try 3rd time
                                        try
                                        {
                                            driver.FindElement(By.XPath(ElementXPath)).Click();
                                        }
                                        catch (Exception e5)
                                        {
                                        }
                                    }
                                }

                            }
                        }
                        //dynamic case
                        if (isDynamicElementXPath == "True")
                        {
                            string DynamicXPathValue = string.Empty;
                            IList<IWebElement> all_Elements = null;
                            all_Elements = driver.FindElements(By.XPath("//" + DynamicElementTagName + "[contains(@" + DynamicElementAttributeName + ",'" + DynamicElementAttributeValue1 + "')]"));



                            foreach (IWebElement item in all_Elements)
                            {
                                DynamicXPathValue = item.GetAttribute("id");
                                //*[@id="u_0_d_Op"]
                                DynamicXPathValue = "//*[@id=\"" + DynamicXPathValue + "\"]";
                            }

                            try
                            {
                                driver.FindElement(By.XPath(DynamicXPathValue)).Click();
                            }
                            catch (Exception e2)
                            {
                                Thread.Sleep(3000); //try first time
                                try
                                {
                                    driver.FindElement(By.XPath(DynamicXPathValue)).Click();
                                }
                                catch (Exception e3)
                                {
                                    Thread.Sleep(3000);  //try 2nd time
                                    try
                                    {
                                        driver.FindElement(By.XPath(DynamicXPathValue)).Click();
                                    }
                                    catch (Exception e4)
                                    {
                                        Thread.Sleep(3000);  //try 3rd time
                                        try
                                        {
                                            driver.FindElement(By.XPath(DynamicXPathValue)).Click();
                                        }
                                        catch (Exception e5)
                                        {
                                        }
                                    }
                                }

                            }

                        }
                    }
                    //########################### scrap data ##################################
                    if (Action == "Data Scrap")
                    {
                        //get the current function json and loop each step
                        string FunContent = File.ReadAllText(StepFunctionJSONFileFullName);
                        JObject J_Funs = JObject.Parse(FunContent);
                        //get the startendpoint url and isStartendpoint url
                        //                      "WholeFunctionisNavigateURL": true,
                        //"WholeFunctionNavigateURL": "https://www.majesticpet.com/bagel-bed/",
                        isNavigateURL = J_Funs["WholeFunctionisNavigateURL"].ToString();
                        NavigateURL = J_Funs["WholeFunctionNavigateURL"].ToString();
                        if (isNavigateURL == "True")
                        {
                            driver.Url = NavigateURL; //starting point 
                        }

                        //                      "FunctionID": "FunctionID_SampleXXX_StepID1",
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
                        //"isMutiplePage": "True",
                        // "PageElement": "//*[@id='dgItems']/tbody/tr[102]/td/a[xxx]",


                        Current_Scaper_SaveDataFileName = J_Funs["SaveFileFolder"].ToString().Replace("|", "\\") + "\\" + J_Funs["SaveFileName"].ToString();

                        //create the scraped data file
                        using (StreamWriter sw = File.CreateText(Current_Scaper_SaveDataFileName))
                        {
                            string isMutiplePage = J_Funs["isMutiplePage"].ToString();
                            ////////////////////////////////////////
                            try
                            {
                                JToken j_isMutiplePage = J_Funs["isMutiplePage"];
                                if (j_isMutiplePage != null)
                                {
                                    isMutiplePage = J_Funs["isMutiplePage"].ToString();
                                }

                            }
                            catch (Exception e1)
                            {
                                isMutiplePage = "False";
                            }


                            //##################### Single page case #######################################
                            if (isMutiplePage != "True")
                            {
                                int colCount = J_Funs["FunctionDetail"].Count();
                                string FileHeader = string.Empty;
                                string delimeter = ",";
                                for (int i_c = 0; i_c <= colCount - 1; i_c++)
                                {
                                    string ElementToColumnName = J_Funs["FunctionDetail"][i_c].SelectToken("ElementToColumnName").ToString();
                                    if (!ElementToColumnName.Contains("FunctionDetail_ElementToColumnName_SampleXXX"))
                                    {
                                        if (FileHeader == "")
                                        {
                                            FileHeader = ElementToColumnName;
                                        }
                                        else
                                        {
                                            FileHeader = FileHeader + delimeter + ElementToColumnName;
                                        }

                                    }
                                }
                                sw.WriteLine(FileHeader);
                                //after write the header, collect the data
                                //let's scrap max 1000 lines for each page
                                int errCount = 0;
                                for (int i_l = 0; i_l <= 1000; i_l++)
                                {
                                    //errCount>3 it means i is large and can't get value any more
                                    if (errCount > 30)
                                    {
                                        // MessageBox.Show("Your data scraper XPath is incorrect, please adjust it");
                                        break;
                                    }
                                    try
                                    {
                                        string DataLine = string.Empty;
                                        for (int i_c = 0; i_c <= colCount - 1; i_c++)
                                        {
                                            string ElementToColumnName = J_Funs["FunctionDetail"][i_c].SelectToken("ElementToColumnName").ToString();
                                            if (!ElementToColumnName.Contains("FunctionDetail_ElementToColumnName_SampleXXX"))
                                            {
                                                string theElementXPath = J_Funs["FunctionDetail"][i_c].SelectToken("ElementXPath").ToString();
                                                //string theElementXPath = "//*[@id=\"Fin-Stream\"]/ul/li[xxx]/div/div/div[2]/h3/a/u";
                                                //*[@id="Fin-Stream"]/ul/li[1]/div/div/div[2]/h3/a/u
                                                theElementXPath = theElementXPath.Replace("'", "\"");
                                                theElementXPath = theElementXPath.Replace("xxx", i_l.ToString());
                                                string colSrapData = string.Empty;
                                                try
                                                {
                                                    colSrapData = driver.FindElement(By.XPath(theElementXPath)).Text;
                                                    colSrapData = colSrapData.Replace(",", "\t");
                                                    if (DataLine == "")
                                                    {
                                                        DataLine = colSrapData;
                                                    }
                                                    else
                                                    {
                                                        DataLine = DataLine + delimeter + colSrapData;

                                                    }
                                                    //try
                                                    //{
                                                    //    DataGridViewTextBoxColumn c_ShowIndex = new DataGridViewTextBoxColumn();
                                                    //    c_ShowIndex.Name = colSrapData;
                                                    //    c_ShowIndex.HeaderText = colSrapData;
                                                    //    c_ShowIndex.Width = 60;
                                                    //    dg_ScrapData.Columns.Add(c_ShowIndex);
                                                    //} catch (Exception e3) { }


                                                }
                                                catch (Exception e3)
                                                {
                                                    errCount++; // i maybe 1,maybe0,maby2, let's try 3 times
                                                }

                                            }
                                        }
                                        sw.WriteLine(DataLine);
                                    }
                                    catch (Exception e2)
                                    {
                                    }
                                }
                                sw.Dispose();
                                sw.Close();
                            }
                            //##################### multiple pages case #######################################
                            if (isMutiplePage == "True")
                            {
                                int errPagesClickCount = 0;
                                string PageElement = J_Funs["PageElement"].ToString();
                                //going to loop 100 pages
                                for (int i_p = 0; i_p <= 100; i_p++)
                                {
                                    string PageElement_1 = PageElement.Replace("xxx", (i_p + 1).ToString());
                                    string PageElement_2 = PageElement.Replace("xxx", (i_p + 2).ToString());
                                    string PageElement_3 = PageElement.Replace("xxx", (i_p + 3).ToString());
                                    // PageElement = PageElement.Replace("xxx", i_p+1.ToString());
                                    try
                                    {
                                        int colCount = J_Funs["FunctionDetail"].Count();
                                        string FileHeader = string.Empty;
                                        string delimeter = ",";
                                        for (int i_c = 0; i_c <= colCount - 1; i_c++)
                                        {
                                            string ElementToColumnName = J_Funs["FunctionDetail"][i_c].SelectToken("ElementToColumnName").ToString();
                                            if (!ElementToColumnName.Contains("FunctionDetail_ElementToColumnName_SampleXXX"))
                                            {
                                                if (FileHeader == "")
                                                {
                                                    FileHeader = ElementToColumnName;
                                                }
                                                else
                                                {
                                                    FileHeader = FileHeader + delimeter + ElementToColumnName;
                                                }

                                            }
                                        }
                                        sw.WriteLine(FileHeader);
                                        //after write the header, collect the data
                                        //let's scrap max 1000 lines for each page
                                        int errCount = 0;
                                        for (int i_l = 0; i_l <= 1000; i_l++)
                                        {
                                            //errCount>3 it means i is large and can't get value any more
                                            if (errCount > 30)
                                            {
                                                // MessageBox.Show("Your data scraper XPath is incorrect, please adjust it");
                                                break;
                                            }
                                            try
                                            {
                                                string DataLine = string.Empty;
                                                for (int i_c = 0; i_c <= colCount - 1; i_c++)
                                                {
                                                    string ElementToColumnName = J_Funs["FunctionDetail"][i_c].SelectToken("ElementToColumnName").ToString();
                                                    if (!ElementToColumnName.Contains("FunctionDetail_ElementToColumnName_SampleXXX"))
                                                    {
                                                        string theElementXPath = J_Funs["FunctionDetail"][i_c].SelectToken("ElementXPath").ToString();
                                                        //string theElementXPath = "//*[@id=\"Fin-Stream\"]/ul/li[xxx]/div/div/div[2]/h3/a/u";
                                                        //*[@id="Fin-Stream"]/ul/li[1]/div/div/div[2]/h3/a/u
                                                        theElementXPath = theElementXPath.Replace("'", "\"");
                                                        theElementXPath = theElementXPath.Replace("xxx", i_l.ToString());
                                                        string colSrapData = string.Empty;
                                                        try
                                                        {
                                                            colSrapData = driver.FindElement(By.XPath(theElementXPath)).Text;
                                                            colSrapData = colSrapData.Replace(",", "\t");
                                                            if (DataLine == "")
                                                            {
                                                                DataLine = colSrapData;
                                                            }
                                                            else
                                                            {
                                                                DataLine = DataLine + delimeter + colSrapData;

                                                            }

                                                        }
                                                        catch (Exception e3)
                                                        {
                                                            errCount++; // i maybe 1,maybe0,maby2, let's try 3 times
                                                        }

                                                    }
                                                }
                                                sw.WriteLine(DataLine);
                                            }
                                            catch (Exception e2)
                                            {
                                            }
                                        }

                                    }
                                    catch (Exception e2)
                                    {
                                        sw.Dispose();
                                        sw.Close();
                                        break;
                                    }
                                    // after collect this page, click next page and continue to collect
                                    try
                                    {
                                        driver.FindElement(By.XPath(PageElement_1)).Click();
                                    }
                                    catch (Exception e2)
                                    {
                                        errPagesClickCount++;
                                        try
                                        {
                                            driver.FindElement(By.XPath(PageElement_2)).Click();
                                        }
                                        catch (Exception e3)
                                        {
                                            errPagesClickCount++;
                                            if (errPagesClickCount > 1)
                                            {
                                                break; //try 3 times, not click able then quit
                                            }
                                        }
                                    }

                                    Thread.Sleep(2000);
                                }
                                sw.Dispose();
                                sw.Close();
                            }
                        }
                        try
                        {
                            ReadCSV csv = new ReadCSV(Current_Scaper_SaveDataFileName);

                            try
                            {
                                dg_ScrapData.DataSource = csv.readCSV;
                                dg_ScrapData.AutoResizeColumns();

                                // Configure the details DataGridView so that its columns automatically
                                // adjust their widths when the data changes.
                                dg_ScrapData.AutoSizeColumnsMode =
                                    DataGridViewAutoSizeColumnsMode.AllCells;
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.Message);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }

                        MessageBox.Show(new Form { TopMost = true }, CurrentFunctionName + " Data Collection Finish", "Data Sraper", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //System.Diagnostics.Process.Start(Current_Scaper_SaveDataFileName);
                        //end get function
                    }

                    // diagram1.Controller.Model = LoadDiagramModel();
                    // diagram1.Controller.Refresh();

                    Thread.Sleep(waitMiseconds);

                }
                if (FunctionCategoryID == "AutoCate_KEY_07")
                {
                    //object►FunctionDetail►0►Action
                    bool isExist = false;
                    int SendKeyCount = JO_ExecuteFunctionJson.SelectToken("FunctionDetail").Count();
                    for (int i_key = 0; i_key <= SendKeyCount - 1; i_key++)
                    {
                        if (!isExist)
                        {
                            try
                            {
                                //                         "Action":"Send KEY",
                                //   "KEYName":"FunctionDetail_KEYName1_SampleXXX",
                                //"KEYSendTimes":"FunctionDetail_KEYSendTimes1_SampleXXX",
                                //"KEYSendWaitSeconds":"FunctionDetail_KEYSendWaitSeconds1_SampleXXX"
                                int eachKeyCount = int.Parse(JO_ExecuteFunctionJson.SelectToken("FunctionDetail")[i_key].SelectToken("KEYSendTimes").ToString());
                                for (int i_eachKey = 0; i_eachKey <= eachKeyCount - 1; i_eachKey++)
                                {
                                    try

                                    {
                                        SendKeys.SendWait(JO_ExecuteFunctionJson.SelectToken("FunctionDetail")[i_key].SelectToken("KEYName").ToString());
                                        Thread.Sleep(500); //default each key send after sleep 0.5 second
                                    }
                                    catch (Exception e2)
                                    {
                                        isExist = true;
                                        break; //it maybe donothing, so end it
                                    }

                                }
                                int sleepseconds = int.Parse(JO_ExecuteFunctionJson.SelectToken("FunctionDetail")[i_key].SelectToken("KEYSendWaitSeconds").ToString()) * 1000;
                                Thread.Sleep(sleepseconds);
                            }
                            catch (Exception e2)
                            {
                                break; //it maybe donothing, so end it
                            }
                        }

                    }

                }

            }
        }

        public void RunAutomation(string _CurrentFunctionsListExecuteJSONFile)
        {
            panel1.Visible = false;
            panel2.Visible = true;

            // first, get the function list step file
            try
            {
                JO_ExecuteFunctionListJson = JObject.Parse(File.ReadAllText(_CurrentFunctionsListExecuteJSONFile));
            }
            catch (Exception e1)
            {
                //it must be just init, get the latest steplist json file from the functions folder
                string latestStepListFileName = GetLatestStepListFile(App_Path + "\\Functions\\", "*FunctionSteps.json");
                JO_ExecuteFunctionListJson = JObject.Parse(File.ReadAllText(latestStepListFileName));
            }
            //now goto each step and pick up the sub function json file and excute them one by one
            //object►AutoName
            string FunctionFolder = JO_ExecuteFunctionListJson["AutoName"].ToString();
            var chromeOptions = new ChromeOptions();
            //hide the debug window
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            //var driver = new ChromeDriver(driverService, new ChromeOptions());
            //*[@id="drpSelectedCustomer"]/option[14]
            //end hide the debug window
            string downloadDirectory = @"";

            chromeOptions.AddUserProfilePreference("download.default_directory", downloadDirectory);
            chromeOptions.AddUserProfilePreference("download.prompt_for_download", false);
            chromeOptions.AddUserProfilePreference("disable-popup-blocking", "true");
            chromeOptions.AddArgument("--start-maximized");

            IWebDriver driver = new ChromeDriver(driverService, chromeOptions);

            //object►StepList►0►FunctionDetail►FunctionExecuteJsonLocation
            int StepCounts = JO_ExecuteFunctionListJson["StepList"].Count();
            for (int i = 0; i <= StepCounts - 1; i++)
            {
            Nexti:
                string StepFunctionJSONFileFullName = string.Empty;
                try
                {
                    StepFunctionJSONFileFullName = App_Path + "\\Functions\\" + FunctionFolder + "\\" + JO_ExecuteFunctionListJson["StepList"][i].SelectToken("FunctionDetail").SelectToken("FunctionExecuteJsonLocation").ToString();
                }
                catch (Exception e3)
                {
                    //it must runout of the steps
                    break;
                }

                string RuuningSetpID = JO_ExecuteFunctionListJson["StepList"][i].SelectToken("StepID").ToString();
                CurrentTestRunningFunctionStepID = RuuningSetpID;

                try
                {
                    JO_ExecuteFunctionJson = JObject.Parse(File.ReadAllText(StepFunctionJSONFileFullName));
                }
                catch (Exception e2)
                {
                    i = i + 1;
                    goto Nexti;
                    //break; 
                } //if exception, then no json file to excute
                //object►FunctionCategoryID
                //check if the ID belong to selenium
                string FunctionCategoryID = JO_ExecuteFunctionJson.SelectToken("FunctionCategoryID").ToString();
                if (FunctionCategoryID == "AutoCate_SEL_01")
                {
                    //object►FunctionDetail►0►isStartPoint
                    //             "isNavigateURL":"True",
                    //"NavigateURL":"https://covtxhou01-22.covalentworks.com/MyB2B_Ver1/weblogin.aspx?ReturnUrl=%2fMyB2B_Ver1%2fPO%2fSearch.aspx%3fSenderID%3d08925485USSM%26ReceiverID%3d01933339053",	 
                    //   "ElementXPath":"/html[1]/body[1]/table[1]/tbody[1]/tr[1]/td[1]/form[1]/table[1]/tbody[1]/tr[7]/td[3]/table[1]/tbody[1]/tr[2]/td[2]/input[1]",
                    //"SendValue":"5435345435",
                    //"AfterDoneWaitSeconds":"0"

                    string isNavigateURL = JO_ExecuteFunctionJson.SelectToken("FunctionDetail")[0].SelectToken("isNavigateURL").ToString();
                    string NavigateURL = JO_ExecuteFunctionJson.SelectToken("FunctionDetail")[0].SelectToken("NavigateURL").ToString();
                    string Action = JO_ExecuteFunctionJson.SelectToken("FunctionDetail")[0].SelectToken("Action").ToString();
                    string ElementXPath = JO_ExecuteFunctionJson.SelectToken("FunctionDetail")[0].SelectToken("ElementXPath").ToString().Replace("'", "\"");
                    string AfterDoneWaitSeconds = JO_ExecuteFunctionJson.SelectToken("FunctionDetail")[0].SelectToken("AfterDoneWaitSeconds").ToString();
                    int waitMiseconds = 0;
                    try { waitMiseconds = int.Parse(AfterDoneWaitSeconds) * 1000; } catch (Exception e1) { waitMiseconds = 3000; }

                    string isDynamicElementXPath = JO_ExecuteFunctionJson.SelectToken("FunctionDetail")[0].SelectToken("isDynamicElementXPath").ToString();
                    string DynamicElementTagName = JO_ExecuteFunctionJson.SelectToken("FunctionDetail")[0].SelectToken("DynamicElementTagName").ToString();
                    string DynamicElementAttributeName = JO_ExecuteFunctionJson.SelectToken("FunctionDetail")[0].SelectToken("DynamicElementAttributeName").ToString();
                    string DynamicElementAttributeValue1 = JO_ExecuteFunctionJson.SelectToken("FunctionDetail")[0].SelectToken("DynamicElementAttributeValue1").ToString();
                    string DynamicElementAttributeValue2 = JO_ExecuteFunctionJson.SelectToken("FunctionDetail")[0].SelectToken("DynamicElementAttributeValue2").ToString();

                    if (isNavigateURL == "True")
                    {
                        driver.Url = NavigateURL; //starting point 
                    }
                    //########################### send text ##################################
                    if (Action == "Send Value")
                    {
                        string SendValue = JO_ExecuteFunctionJson.SelectToken("FunctionDetail")[0].SelectToken("SendValue").ToString();

                        //solid element case
                        if (isDynamicElementXPath != "True")
                        {
                            try
                            {
                                driver.FindElement(By.XPath(ElementXPath)).SendKeys(SendValue);
                            }
                            catch (Exception e2)
                            {
                                Thread.Sleep(3000); //try first time
                                try
                                {
                                    driver.FindElement(By.XPath(ElementXPath)).SendKeys(SendValue);
                                }
                                catch (Exception e3)
                                {
                                    Thread.Sleep(3000);  //try 2nd time
                                    try
                                    {
                                        driver.FindElement(By.XPath(ElementXPath)).SendKeys(SendValue);
                                    }
                                    catch (Exception e4)
                                    {
                                        Thread.Sleep(3000);  //try 3rd time
                                        try
                                        {
                                            driver.FindElement(By.XPath(ElementXPath)).SendKeys(SendValue);
                                        }
                                        catch (Exception e5)
                                        {
                                        }
                                    }
                                }

                            }
                        }
                        //dynamic case
                        if (isDynamicElementXPath == "True")
                        {
                            string DynamicXPathValue = string.Empty;
                            IList<IWebElement> all_Elements = null;
                            all_Elements = driver.FindElements(By.XPath("//" + DynamicElementTagName + "[contains(@" + DynamicElementAttributeName + ",'" + DynamicElementAttributeValue1 + "') and contains(@" + DynamicElementAttributeName + ",'" + DynamicElementAttributeValue2 + "') ]"));
                            foreach (IWebElement item in all_Elements)
                            {
                                // DynamicXPathValue = item.GetAttribute("id");
                                //string skuvalue_href = item.Text;
                                try { item.SendKeys(SendValue); } catch (Exception e4) { }
                            }

                            try
                            {
                                driver.FindElement(By.XPath(ElementXPath)).SendKeys(SendValue);
                            }
                            catch (Exception e2)
                            {
                                Thread.Sleep(3000); //try first time
                                try
                                {
                                    driver.FindElement(By.XPath(ElementXPath)).SendKeys(SendValue);
                                }
                                catch (Exception e3)
                                {
                                    Thread.Sleep(3000);  //try 2nd time
                                    try
                                    {
                                        driver.FindElement(By.XPath(ElementXPath)).SendKeys(SendValue);
                                    }
                                    catch (Exception e4)
                                    {
                                        Thread.Sleep(3000);  //try 3rd time
                                        try
                                        {
                                            driver.FindElement(By.XPath(ElementXPath)).SendKeys(SendValue);
                                        }
                                        catch (Exception e5)
                                        {
                                        }
                                    }
                                }

                            }
                        }

                    }
                    //########################### click something ##################################
                    if (Action == "Click Button")
                    {
                        if (isDynamicElementXPath != "True")
                        {
                            try
                            {
                                driver.FindElement(By.XPath(ElementXPath)).Click();
                            }
                            catch (Exception e2)
                            {
                                Thread.Sleep(3000); //try first time
                                try
                                {
                                    driver.FindElement(By.XPath(ElementXPath)).Click();
                                }
                                catch (Exception e3)
                                {
                                    Thread.Sleep(3000);  //try 2nd time
                                    try
                                    {
                                        driver.FindElement(By.XPath(ElementXPath)).Click();
                                    }
                                    catch (Exception e4)
                                    {
                                        Thread.Sleep(3000);  //try 3rd time
                                        try
                                        {
                                            driver.FindElement(By.XPath(ElementXPath)).Click();
                                        }
                                        catch (Exception e5)
                                        {
                                        }
                                    }
                                }

                            }
                        }
                        //dynamic case
                        if (isDynamicElementXPath == "True")
                        {
                            string DynamicXPathValue = string.Empty;
                            IList<IWebElement> all_Elements = null;
                            all_Elements = driver.FindElements(By.XPath("//" + DynamicElementTagName + "[contains(@" + DynamicElementAttributeName + ",'" + DynamicElementAttributeValue1 + "') and contains(@" + DynamicElementAttributeName + ",'" + DynamicElementAttributeValue2 + "') ]"));
                            if (all_Elements.Count == 0)
                            {
                                all_Elements = driver.FindElements(By.XPath("//*[text()='Groups']"));
                            }
                            foreach (IWebElement item in all_Elements)
                            {
                                // DynamicXPathValue = item.GetAttribute("id");
                                //string skuvalue_href = item.Text;
                                try
                                {
                                    item.Click();
                                }
                                catch (Exception e4)
                                { }
                            }
                            //try
                            //{
                            //    driver.FindElement(By.XPath(DynamicXPathValue)).Click();
                            //}
                            //catch (Exception e2)
                            //{
                            //    Thread.Sleep(3000); //try first time
                            //    try
                            //    {
                            //        driver.FindElement(By.XPath(DynamicXPathValue)).Click();
                            //    }
                            //    catch (Exception e3)
                            //    {
                            //        Thread.Sleep(3000);  //try 2nd time
                            //        try
                            //        {
                            //            driver.FindElement(By.XPath(DynamicXPathValue)).Click();
                            //        }
                            //        catch (Exception e4)
                            //        {
                            //            Thread.Sleep(3000);  //try 3rd time
                            //            try
                            //            {
                            //                driver.FindElement(By.XPath(DynamicXPathValue)).Click();
                            //            }
                            //            catch (Exception e5)
                            //            {
                            //            }
                            //        }
                            //    }

                            //}

                        }

                    }
                    //########################### scrap data ##################################
                    if (Action == "Data Scrap")
                    {
                        //get the current function json and loop each step
                        string FunContent = File.ReadAllText(StepFunctionJSONFileFullName);
                        JObject J_Funs = JObject.Parse(FunContent);
                        //get the startendpoint url and isStartendpoint url
                        //                      "WholeFunctionisNavigateURL": true,
                        //"WholeFunctionNavigateURL": "https://www.majesticpet.com/bagel-bed/",
                        isNavigateURL = J_Funs["WholeFunctionisNavigateURL"].ToString();
                        NavigateURL = J_Funs["WholeFunctionNavigateURL"].ToString();
                        if (isNavigateURL == "True")
                        {
                            driver.Url = NavigateURL; //starting point 
                        }

                        //                      "FunctionID": "FunctionID_SampleXXX_StepID1",
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
                        //"isMutiplePage": "True",
                        // "PageElement": "//*[@id='dgItems']/tbody/tr[102]/td/a[xxx]",


                        Current_Scaper_SaveDataFileName = J_Funs["SaveFileFolder"].ToString().Replace("|", "\\") + "\\" + J_Funs["SaveFileName"].ToString();

                        //create the scraped data file
                        using (StreamWriter sw = File.CreateText(Current_Scaper_SaveDataFileName))
                        {
                            string isMutiplePage = J_Funs["isMutiplePage"].ToString();
                            //##################### Single page case #######################################
                            if (isMutiplePage != "True")
                            {
                                int colCount = J_Funs["FunctionDetail"].Count();
                                string FileHeader = string.Empty;
                                string delimeter = ",";
                                for (int i_c = 0; i_c <= colCount - 1; i_c++)
                                {
                                    string ElementToColumnName = J_Funs["FunctionDetail"][i_c].SelectToken("ElementToColumnName").ToString();
                                    if (!ElementToColumnName.Contains("FunctionDetail_ElementToColumnName_SampleXXX"))
                                    {
                                        if (FileHeader == "")
                                        {
                                            FileHeader = ElementToColumnName;
                                        }
                                        else
                                        {
                                            FileHeader = FileHeader + delimeter + ElementToColumnName;
                                        }

                                    }
                                }
                                sw.WriteLine(FileHeader);
                                //after write the header, collect the data
                                //let's scrap max 1000 lines for each page
                                int errCount = 0;
                                for (int i_l = 0; i_l <= 1000; i_l++)
                                {
                                    //errCount>3 it means i is large and can't get value any more
                                    if (errCount > 30)
                                    {
                                        // MessageBox.Show("Your data scraper XPath is incorrect, please adjust it");
                                        break;
                                    }
                                    try
                                    {
                                        string DataLine = string.Empty;
                                        for (int i_c = 0; i_c <= colCount - 1; i_c++)
                                        {
                                            string ElementToColumnName = J_Funs["FunctionDetail"][i_c].SelectToken("ElementToColumnName").ToString();
                                            if (!ElementToColumnName.Contains("FunctionDetail_ElementToColumnName_SampleXXX"))
                                            {
                                                string theElementXPath = J_Funs["FunctionDetail"][i_c].SelectToken("ElementXPath").ToString();
                                                //string theElementXPath = "//*[@id=\"Fin-Stream\"]/ul/li[xxx]/div/div/div[2]/h3/a/u";
                                                //*[@id="Fin-Stream"]/ul/li[1]/div/div/div[2]/h3/a/u
                                                theElementXPath = theElementXPath.Replace("'", "\"");
                                                theElementXPath = theElementXPath.Replace("xxx", i_l.ToString());
                                                string colSrapData = string.Empty;
                                                try
                                                {
                                                    colSrapData = driver.FindElement(By.XPath(theElementXPath)).Text;
                                                    colSrapData = colSrapData.Replace(",", "\t");
                                                    if (DataLine == "")
                                                    {
                                                        DataLine = colSrapData;
                                                    }
                                                    else
                                                    {
                                                        DataLine = DataLine + delimeter + colSrapData;

                                                    }
                                                    //try
                                                    //{
                                                    //    DataGridViewTextBoxColumn c_ShowIndex = new DataGridViewTextBoxColumn();
                                                    //    c_ShowIndex.Name = colSrapData;
                                                    //    c_ShowIndex.HeaderText = colSrapData;
                                                    //    c_ShowIndex.Width = 60;
                                                    //    dg_ScrapData.Columns.Add(c_ShowIndex);
                                                    //} catch (Exception e3) { }


                                                }
                                                catch (Exception e3)
                                                {
                                                    errCount++; // i maybe 1,maybe0,maby2, let's try 3 times
                                                }

                                            }
                                        }
                                        sw.WriteLine(DataLine);
                                    }
                                    catch (Exception e2)
                                    {
                                    }
                                }
                                sw.Dispose();
                                sw.Close();
                            }
                            //##################### multiple pages case #######################################
                            if (isMutiplePage == "True")
                            {
                                int errPagesClickCount = 0;
                                string PageElement = J_Funs["PageElementXPath"].ToString();
                                //going to loop 100 pages
                                for (int i_p = 0; i_p <= 100; i_p++)
                                {
                                    string PageElement_1 = PageElement.Replace("xxx", (i_p + 1).ToString());
                                    string PageElement_2 = PageElement.Replace("xxx", (i_p + 2).ToString());
                                    string PageElement_3 = PageElement.Replace("xxx", (i_p + 3).ToString());
                                    // PageElement = PageElement.Replace("xxx", i_p+1.ToString());
                                    try
                                    {
                                        int colCount = J_Funs["FunctionDetail"].Count();
                                        string FileHeader = string.Empty;
                                        string delimeter = ",";
                                        for (int i_c = 0; i_c <= colCount - 1; i_c++)
                                        {
                                            string ElementToColumnName = J_Funs["FunctionDetail"][i_c].SelectToken("ElementToColumnName").ToString();
                                            if (!ElementToColumnName.Contains("FunctionDetail_ElementToColumnName_SampleXXX"))
                                            {
                                                if (FileHeader == "")
                                                {
                                                    FileHeader = ElementToColumnName;
                                                }
                                                else
                                                {
                                                    FileHeader = FileHeader + delimeter + ElementToColumnName;
                                                }

                                            }
                                        }
                                        sw.WriteLine(FileHeader);
                                        //after write the header, collect the data
                                        //let's scrap max 1000 lines for each page
                                        int errCount = 0;
                                        for (int i_l = 0; i_l <= 1000; i_l++)
                                        {
                                            //errCount>3 it means i is large and can't get value any more
                                            if (errCount > 30)
                                            {
                                                // MessageBox.Show("Your data scraper XPath is incorrect, please adjust it");
                                                break;
                                            }
                                            try
                                            {
                                                string DataLine = string.Empty;
                                                for (int i_c = 0; i_c <= colCount - 1; i_c++)
                                                {
                                                    string ElementToColumnName = J_Funs["FunctionDetail"][i_c].SelectToken("ElementToColumnName").ToString();
                                                    if (!ElementToColumnName.Contains("FunctionDetail_ElementToColumnName_SampleXXX"))
                                                    {
                                                        string theElementXPath = J_Funs["FunctionDetail"][i_c].SelectToken("ElementXPath").ToString();
                                                        //string theElementXPath = "//*[@id=\"Fin-Stream\"]/ul/li[xxx]/div/div/div[2]/h3/a/u";
                                                        //*[@id="Fin-Stream"]/ul/li[1]/div/div/div[2]/h3/a/u
                                                        theElementXPath = theElementXPath.Replace("'", "\"");
                                                        theElementXPath = theElementXPath.Replace("xxx", i_l.ToString());
                                                        string colSrapData = string.Empty;
                                                        try
                                                        {
                                                            colSrapData = driver.FindElement(By.XPath(theElementXPath)).Text;
                                                            colSrapData = colSrapData.Replace(",", "\t");
                                                            if (DataLine == "")
                                                            {
                                                                DataLine = colSrapData;
                                                            }
                                                            else
                                                            {
                                                                DataLine = DataLine + delimeter + colSrapData;

                                                            }

                                                        }
                                                        catch (Exception e3)
                                                        {
                                                            errCount++; // i maybe 1,maybe0,maby2, let's try 3 times
                                                        }

                                                    }
                                                }
                                                sw.WriteLine(DataLine);
                                            }
                                            catch (Exception e2)
                                            {
                                            }
                                        }

                                    }
                                    catch (Exception e2)
                                    {
                                        sw.Dispose();
                                        sw.Close();
                                        break;
                                    }
                                    // after collect this page, click next page and continue to collect
                                    try
                                    {
                                        driver.FindElement(By.XPath(PageElement_1)).Click();
                                    }
                                    catch (Exception e2)
                                    {
                                        errPagesClickCount++;
                                        try
                                        {
                                            driver.FindElement(By.XPath(PageElement_2)).Click();
                                        }
                                        catch (Exception e3)
                                        {
                                            errPagesClickCount++;
                                            if (errPagesClickCount > 1)
                                            {
                                                break; //try 3 times, not click able then quit
                                            }
                                        }
                                    }

                                    Thread.Sleep(2000);
                                }
                                sw.Dispose();
                                sw.Close();
                            }

                        }
                        try
                        {
                            ReadCSV csv = new ReadCSV(Current_Scaper_SaveDataFileName);

                            try
                            {
                                dg_ScrapData.DataSource = csv.readCSV;
                                dg_ScrapData.AutoResizeColumns();

                                // Configure the details DataGridView so that its columns automatically
                                // adjust their widths when the data changes.
                                dg_ScrapData.AutoSizeColumnsMode =
                                    DataGridViewAutoSizeColumnsMode.AllCells;
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.Message);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }

                        MessageBox.Show(new Form { TopMost = true }, CurrentFunctionName + " Data Collection Finish", "Data Sraper", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //System.Diagnostics.Process.Start(Current_Scaper_SaveDataFileName);
                        //end get function
                    }

                    diagram1.Controller.Model = LoadDiagramModel(CurrentAutoName);
                    diagram1.Controller.Refresh();

                    Thread.Sleep(waitMiseconds);

                }
                if (FunctionCategoryID == "AutoCate_KEY_07")
                {
                    //object►FunctionDetail►0►Action
                    bool isExist = false;
                    int SendKeyCount = JO_ExecuteFunctionJson.SelectToken("FunctionDetail").Count();
                    for (int i_key = 0; i_key <= SendKeyCount - 1; i_key++)
                    {
                        if (!isExist)
                        {
                            try
                            {
                                //                         "Action":"Send KEY",
                                //   "KEYName":"FunctionDetail_KEYName1_SampleXXX",
                                //"KEYSendTimes":"FunctionDetail_KEYSendTimes1_SampleXXX",
                                //"KEYSendWaitSeconds":"FunctionDetail_KEYSendWaitSeconds1_SampleXXX"
                                int eachKeyCount = int.Parse(JO_ExecuteFunctionJson.SelectToken("FunctionDetail")[i_key].SelectToken("KEYSendTimes").ToString());
                                for (int i_eachKey = 0; i_eachKey <= eachKeyCount - 1; i_eachKey++)
                                {
                                    try

                                    {
                                        SendKeys.SendWait(JO_ExecuteFunctionJson.SelectToken("FunctionDetail")[i_key].SelectToken("KEYName").ToString());
                                        Thread.Sleep(500); //default each key send after sleep 0.5 second
                                    }
                                    catch (Exception e2)
                                    {
                                        isExist = true;
                                        break; //it maybe donothing, so end it
                                    }

                                }
                                int sleepseconds = int.Parse(JO_ExecuteFunctionJson.SelectToken("FunctionDetail")[i_key].SelectToken("KEYSendWaitSeconds").ToString()) * 1000;
                                Thread.Sleep(sleepseconds);
                            }
                            catch (Exception e2)
                            {
                                break; //it maybe donothing, so end it
                            }
                        }

                    }

                }

            }
        }

        public void RunAutomation_Lite(string AutoName)
        {
            panel1.Visible = false;
            panel2.Visible = true;
            // first, get the function list step file
            try
            {
                CurrentAutoFunctionSteps_Lite = LiteDB_Action.GetFunctionSteps_Lite_ByAutoName(AutoName);
            }
            catch (Exception e1)
            {
            }
            //now goto each step and pick up the sub function json file and excute them one by one
            //object►AutoName
            string FunctionFolder = JO_ExecuteFunctionListJson["AutoName"].ToString();
            var chromeOptions = new ChromeOptions();
            //hide the debug window
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            //var driver = new ChromeDriver(driverService, new ChromeOptions());
            //*[@id="drpSelectedCustomer"]/option[14]
            //end hide the debug window
            string downloadDirectory = @"";

            chromeOptions.AddUserProfilePreference("download.default_directory", downloadDirectory);
            chromeOptions.AddUserProfilePreference("download.prompt_for_download", false);
            chromeOptions.AddUserProfilePreference("disable-popup-blocking", "true");
            chromeOptions.AddArgument("--start-maximized");

            IWebDriver driver = new ChromeDriver(driverService, chromeOptions);

            //object►StepList►0►FunctionDetail►FunctionExecuteJsonLocation
            int StepCounts = CurrentAutoFunctionSteps_Lite.Count();
            for (int i = 0; i <= StepCounts - 1; i++)
            {
            Nexti:
                string FunctionTableName = string.Empty;
                string FunctionName = string.Empty;
                int StepID = 0;
                try
                {
                    FunctionTableName = CurrentAutoFunctionSteps_Lite[i].FunctionTableName.ToString().TrimStart('\"').TrimEnd('\"');
                    FunctionName = CurrentAutoFunctionSteps_Lite[i].FunctionName;
                    StepID = CurrentAutoFunctionSteps_Lite[i].StepID;
                }
                catch (Exception e3)
                {
                    //it must runout of the steps
                    break;
                }

                string RuuningSetpID = CurrentAutoFunctionSteps_Lite[i].StepID.ToString();
                CurrentTestRunningFunctionStepID = RuuningSetpID;
                //############ SendText Function ################
                if (FunctionTableName == "T_Web_Actions_SendText")
                {
                   //goto the table and search the auto
                    string isNavigateURL = string.Empty;
                    string NavigateURL = string.Empty;
                    string Action = string.Empty;
                    string ElementXPath = string.Empty;
                    string AfterDoneWaitSeconds = string.Empty;
                    int waitMiseconds = 0;                 
                    string isDynamicElementXPath = string.Empty;
                    string DynamicElementTagName = string.Empty;
                    string DynamicElementAttributeName = string.Empty;
                    string DynamicElementAttributeValue1 = string.Empty;
                    string DynamicElementAttributeValue2 = string.Empty;
                    string SendValue = string.Empty;

                    using (var db = new LiteDatabase(frm_Power_Automation.App_Path + "\\PowerAutoLiteDB.db"))
                    {
                        string sql = "SELECT $ FROM "+ FunctionTableName + " where AutoName=" + "\"" + AutoName + "\"" + " and StepID="+ StepID + "";
                        using (var bsonReader = db.Execute(sql))
                        {
                            var output = new List<BsonValue>();                          
                            while (bsonReader.Read())
                            {
                                output.Add(bsonReader.Current);    
                                isNavigateURL = output[0]["FunDetail_isNavigateURL"].ToString().TrimStart('"').TrimEnd('"');
                                NavigateURL = output[0]["FunDetail_NavigateURL"].ToString().TrimStart('"').TrimEnd('"');
                                Action = output[0]["FunDetail_Action"].ToString().TrimStart('"').TrimEnd('"');
                                ElementXPath= output[0]["FunDetail_ElementXPath"].ToString().TrimStart('"').TrimEnd('"');;
                                AfterDoneWaitSeconds = output[0]["FunDetail_AfterDoneWaitSeconds"].ToString();
                                isDynamicElementXPath = output[0]["FunDetail_isDynamicElementXPath"].ToString();
                                DynamicElementTagName = output[0]["FunDetail_DynamicElementTagName"].ToString();
                                DynamicElementAttributeName = output[0]["FunDetail_DynamicElementAttributeName"].ToString();
                                DynamicElementAttributeValue1 = output[0]["FunDetail_DynamicElementAttributeValue1"].ToString();
                                DynamicElementAttributeValue2 = output[0]["FunDetail_DynamicElementAttributeValue2"].ToString();
                                DynamicElementAttributeValue2 = output[0]["FunDetail_DynamicElementAttributeValue2"].ToString();
                                SendValue = output[0]["FunDetail_SendValue"].ToString().TrimStart('"').TrimEnd('"');
                                AfterDoneWaitSeconds= output[0]["FunDetail_AfterDoneWaitSeconds"].ToString().TrimStart('"').TrimEnd('"');
                                try { waitMiseconds = int.Parse(AfterDoneWaitSeconds) * 1000; } catch (Exception e1) { waitMiseconds = 3000; }
                            }
                        }
                        // return output;
                    }

                    if (isNavigateURL == "true")
                    {
                        driver.Url = NavigateURL; //starting point 
                    }
                    //########################### send text ##################################
                    if (Action == "Send Value")
                    {
                        //solid element case
                        if (isDynamicElementXPath != "True")
                        {
                            try
                            {
                                driver.FindElement(By.XPath(ElementXPath)).SendKeys(SendValue);
                            }
                            catch (Exception e2)
                            {
                                //Thread.Sleep(3000); //try first time
                                //try
                                //{
                                //    driver.FindElement(By.XPath(ElementXPath)).SendKeys(SendValue);
                                //}
                                //catch (Exception e3)
                                //{
                                //    Thread.Sleep(3000);  //try 2nd time
                                //    try
                                //    {
                                //        driver.FindElement(By.XPath(ElementXPath)).SendKeys(SendValue);
                                //    }
                                //    catch (Exception e4)
                                //    {
                                //        Thread.Sleep(3000);  //try 3rd time
                                //        try
                                //        {
                                //            driver.FindElement(By.XPath(ElementXPath)).SendKeys(SendValue);
                                //        }
                                //        catch (Exception e5)
                                //        {
                                //        }
                                //    }
                                //}

                            }
                        }
                        //dynamic case
                        if (isDynamicElementXPath == "True")
                        {
                            string DynamicXPathValue = string.Empty;
                            IList<IWebElement> all_Elements = null;
                            all_Elements = driver.FindElements(By.XPath("//" + DynamicElementTagName + "[contains(@" + DynamicElementAttributeName + ",'" + DynamicElementAttributeValue1 + "') and contains(@" + DynamicElementAttributeName + ",'" + DynamicElementAttributeValue2 + "') ]"));
                            foreach (IWebElement item in all_Elements)
                            {
                                // DynamicXPathValue = item.GetAttribute("id");
                                //string skuvalue_href = item.Text;
                                try { item.SendKeys(SendValue); } catch (Exception e4) { }
                            }

                            try
                            {
                                driver.FindElement(By.XPath(ElementXPath)).SendKeys(SendValue);
                            }
                            catch (Exception e2)
                            {
                                Thread.Sleep(3000); //try first time
                                try
                                {
                                    driver.FindElement(By.XPath(ElementXPath)).SendKeys(SendValue);
                                }
                                catch (Exception e3)
                                {
                                    Thread.Sleep(3000);  //try 2nd time
                                    try
                                    {
                                        driver.FindElement(By.XPath(ElementXPath)).SendKeys(SendValue);
                                    }
                                    catch (Exception e4)
                                    {
                                        Thread.Sleep(3000);  //try 3rd time
                                        try
                                        {
                                            driver.FindElement(By.XPath(ElementXPath)).SendKeys(SendValue);
                                        }
                                        catch (Exception e5)
                                        {
                                        }
                                    }
                                }

                            }
                        }

                    }

                    diagram1.Controller.Model = LoadDiagramModel(CurrentAutoName);
                    diagram1.Controller.Refresh();
                    Thread.Sleep(waitMiseconds);
                }
                //############ ClickAction Function ################
                if (FunctionTableName == "T_Web_Actions_ClickAction")
                {
                    //goto the table and search the auto
                    string isNavigateURL = string.Empty;
                    string NavigateURL = string.Empty;
                    string Action = string.Empty;
                    string ElementXPath = string.Empty;
                    string AfterDoneWaitSeconds = string.Empty;
                    int waitMiseconds = 0;
                    string isDynamicElementXPath = string.Empty;
                    string DynamicElementTagName = string.Empty;
                    string DynamicElementAttributeName = string.Empty;
                    string DynamicElementAttributeValue1 = string.Empty;
                    string DynamicElementAttributeValue2 = string.Empty;
                    string SendValue = string.Empty;

                    using (var db = new LiteDatabase(frm_Power_Automation.App_Path + "\\PowerAutoLiteDB.db"))
                    {
                        string sql = "SELECT $ FROM " + FunctionTableName + " where AutoName=" + "\"" + AutoName + "\"" + " and StepID=" + StepID + "";
                        using (var bsonReader = db.Execute(sql))
                        {
                            var output = new List<BsonValue>();

                            while (bsonReader.Read())
                            {
                                output.Add(bsonReader.Current);
                                isNavigateURL = output[0]["FunDetail_isNavigateURL"].ToString().TrimStart('"').TrimEnd('"');
                                NavigateURL = output[0]["FunDetail_NavigateURL"].ToString().TrimStart('"').TrimEnd('"');
                                Action = output[0]["FunDetail_Action"].ToString().TrimStart('"').TrimEnd('"');
                                ElementXPath = output[0]["FunDetail_ElementXPath"].ToString().TrimStart('"').TrimEnd('"'); ;
                                AfterDoneWaitSeconds = output[0]["FunDetail_AfterDoneWaitSeconds"].ToString();
                                isDynamicElementXPath = output[0]["FunDetail_isDynamicElementXPath"].ToString();
                                DynamicElementTagName = output[0]["FunDetail_DynamicElementTagName"].ToString();
                                DynamicElementAttributeName = output[0]["FunDetail_DynamicElementAttributeName"].ToString();
                                DynamicElementAttributeValue1 = output[0]["FunDetail_DynamicElementAttributeValue1"].ToString();
                                DynamicElementAttributeValue2 = output[0]["FunDetail_DynamicElementAttributeValue2"].ToString();
                                DynamicElementAttributeValue2 = output[0]["FunDetail_DynamicElementAttributeValue2"].ToString();
                                SendValue = output[0]["FunDetail_SendValue"].ToString().TrimStart('"').TrimEnd('"');
                                AfterDoneWaitSeconds = output[0]["FunDetail_AfterDoneWaitSeconds"].ToString().TrimStart('"').TrimEnd('"');
                                try { waitMiseconds = int.Parse(AfterDoneWaitSeconds) * 1000; } catch (Exception e1) { waitMiseconds = 3000; }
                            }
                        }
                        // return output;
                    }

                    if (isNavigateURL == "true")
                    {
                        driver.Url = NavigateURL; //starting point 
                    }
                    //########################### Click Button ##################################
                    if (Action == "Click Button")
                    {
                        if (isDynamicElementXPath != "True")
                        {
                            try
                            {
                                driver.FindElement(By.XPath(ElementXPath)).Click();
                            }
                            catch (Exception e2)
                            {
                                Thread.Sleep(3000); //try first time
                                try
                                {
                                    driver.FindElement(By.XPath(ElementXPath)).Click();
                                }
                                catch (Exception e3)
                                {
                                    Thread.Sleep(3000);  //try 2nd time
                                    try
                                    {
                                        driver.FindElement(By.XPath(ElementXPath)).Click();
                                    }
                                    catch (Exception e4)
                                    {
                                        Thread.Sleep(3000);  //try 3rd time
                                        try
                                        {
                                            driver.FindElement(By.XPath(ElementXPath)).Click();
                                        }
                                        catch (Exception e5)
                                        {
                                        }
                                    }
                                }

                            }
                        }
                        //dynamic case
                        if (isDynamicElementXPath == "True")
                        {
                            string DynamicXPathValue = string.Empty;
                            IList<IWebElement> all_Elements = null;
                            all_Elements = driver.FindElements(By.XPath("//" + DynamicElementTagName + "[contains(@" + DynamicElementAttributeName + ",'" + DynamicElementAttributeValue1 + "') and contains(@" + DynamicElementAttributeName + ",'" + DynamicElementAttributeValue2 + "') ]"));
                            if (all_Elements.Count == 0)
                            {
                                all_Elements = driver.FindElements(By.XPath("//*[text()='Groups']"));
                            }
                            foreach (IWebElement item in all_Elements)
                            {
                                // DynamicXPathValue = item.GetAttribute("id");
                                //string skuvalue_href = item.Text;
                                try
                                {
                                    item.Click();
                                }
                                catch (Exception e4)
                                { }
                            }
                           

                        }

                    }

                    diagram1.Controller.Model = LoadDiagramModel(CurrentAutoName);
                    diagram1.Controller.Refresh();
                    Thread.Sleep(waitMiseconds);
                }
                //############ SendKey Function ################
                if (FunctionTableName == "T_OperationSystem_KeyAction_SendKey")
                {
                    //goto the table and search the auto
                    string isNavigateURL = string.Empty;
                    string NavigateURL = string.Empty;
                    string Action = string.Empty;
                    int FunDetail_SendKeyTimes =0;
                    string AfterDoneWaitSeconds = string.Empty;
                    int waitMiseconds = 0;
                    string FunDetail_SendValue = string.Empty;
                    string FunDetail_AfterDoneRedirectURL = string.Empty;
                   
                    string SendValue = string.Empty;

                    using (var db = new LiteDatabase(frm_Power_Automation.App_Path + "\\PowerAutoLiteDB.db"))
                    {
                        string sql = "SELECT $ FROM " + FunctionTableName + " where AutoName=" + "\"" + AutoName + "\"" + " and StepID=" + StepID + "";
                        using (var bsonReader = db.Execute(sql))
                        {
                            var output = new List<BsonValue>();

                            while (bsonReader.Read())
                            {
                                output.Add(bsonReader.Current);
                                isNavigateURL = output[0]["FunDetail_isNavigateURL"].ToString().TrimStart('"').TrimEnd('"');
                                NavigateURL = output[0]["FunDetail_NavigateURL"].ToString().TrimStart('"').TrimEnd('"');
                                Action = output[0]["FunDetail_Action"].ToString().TrimStart('"').TrimEnd('"');
                                FunDetail_SendKeyTimes = int.Parse(output[0]["FunDetail_SendKeyTimes"].ToString().TrimStart('"').TrimEnd('"')) ;
                                AfterDoneWaitSeconds = output[0]["FunDetail_AfterDoneWaitSeconds"].ToString();
                                FunDetail_SendValue = output[0]["FunDetail_SendValue"].ToString().TrimStart('"').TrimEnd('"') ; 
                                FunDetail_AfterDoneRedirectURL = output[0]["FunDetail_AfterDoneRedirectURL"].ToString();
          
                                AfterDoneWaitSeconds = output[0]["FunDetail_AfterDoneWaitSeconds"].ToString().TrimStart('"').TrimEnd('"');
                                try { waitMiseconds = int.Parse(AfterDoneWaitSeconds) * 1000; } catch (Exception e1) { waitMiseconds = 3000; }
                            }
                        }
                        // return output;
                    }

                    if (isNavigateURL == "true")
                    {
                        driver.Url = NavigateURL; //starting point 
                    }
                    //########################### Send Key ##################################
                    if (Action == "Send Key")
                    {
                        for (int i_k = 0; i_k <= FunDetail_SendKeyTimes - 1; i_k++)
                        {
                            try
                            {
                                SendKeys.SendWait(FunDetail_SendValue);
                                Thread.Sleep(500); //default each key send after sleep 0.5 second
                            }
                            catch (Exception e2)
                            {
                               
                                break; //it maybe donothing, so end it
                            }
                        }
                      

                    }
                    Thread.Sleep(waitMiseconds);
                    diagram1.Controller.Model = LoadDiagramModel(CurrentAutoName);
                    diagram1.Controller.Refresh();
                    Thread.Sleep(waitMiseconds);
                }

            }
        }


        public int GetStepMaxFromFile()

        {
            try
            {
                if (CurrentMax_StepFile == "")
                {
                    CurrentMax_StepFile = App_Path + "\\Functions\\" + CurrentAutoName + "\\" + CurrentAutoName + "_MaxStep.ini";
                }
                string oldValue = File.ReadAllText(CurrentMax_StepFile);
                string oldMax = oldValue.Split(':')[1];
                return int.Parse(oldMax);
            }
            catch (Exception e1) { return 0; }

        }

        public void SaveStep()
        {
            if (txt_AutoName.Text == "")
            {
                MessageBox.Show("Automation Name is empty,please fill in");
                return;
            }
            else
            {

                string targetFileBase = App_Path + "\\Functions\\" + txt_AutoName.Text;
                string targetFileSteps = targetFileBase + "\\" + txt_AutoName.Text + "_FunctionSteps.json";
                string targetFileSteps1 = targetFileBase + "\\" + txt_AutoName.Text + "_FunctionSteps1.json";
                string AutoName = txt_AutoName.Text;
                string xpathValue = txt_XPath.Text.Replace("\"", "'");
                int inCurrentStepID = GetStepMaxFromFile() + 1;
                CurrentStepID = inCurrentStepID.ToString();

                //################### create the folder if not exist
                System.IO.Directory.CreateDirectory(targetFileBase);
                //check if the target file is exist, means it may have some step setup before
                if (File.Exists(targetFileSteps))
                {
                    //edit the step belong to this step , only update the funtcion detail
                    Object_AutoStepsJson.FunctionDetail ItemFunctionDetail = new Object_AutoStepsJson.FunctionDetail();
                    ItemFunctionDetail.FunctionID = "FID_" + CurrentStepID + "_N_" + AutoName + "_F_" + CurrentFunctionName;
                    ItemFunctionDetail.FunctionID = ItemFunctionDetail.FunctionID.Replace(" ", "_").Replace("|", "_");
                    ItemFunctionDetail.FunctionName = "F_" + CurrentFunctionName + "_N_" + AutoName + "_FID_" + CurrentStepID;
                    ItemFunctionDetail.FunctionName = ItemFunctionDetail.FunctionName.Replace(" ", "_").Replace("|", "_");
                    ItemFunctionDetail.FunctionCategoryID = CurrentFunctionCategoryID;
                    ItemFunctionDetail.FunctionCategoryName = CurrentFunctionCategoryName;
                    string targetFileStep = targetFileBase + "\\" + ItemFunctionDetail.FunctionName.Replace("|", "_") + ".json";
                    CurrentFunctionExecuteJsonShortName = txt_AutoName.Text + "_STEP_" + CurrentStepID + "_" + CurrentFunctionName + "_Function.json";
                    ItemFunctionDetail.FunctionExecuteJsonLocation = CurrentFunctionExecuteJsonShortName;
                    CurrentFunctionExecuteJsonLocation = CurrentToCopyFunctionExecuteJsonLocation;

                    //replace the steplist value base on CurrentStepID
                    if (int.Parse(CurrentStepID) < 10)
                    {

                        string targetFileStepsText = File.ReadAllText(targetFileSteps);
                        //replace the AutoName
                        targetFileStepsText = targetFileStepsText.Replace("Power_Automation_SampleXXX", AutoName);
                        //replace the DESC
                        targetFileStepsText = targetFileStepsText.Replace("Automation_SampleXXX_Description", txt_Description.Text);
                        //replace the "StartEndpointURL": "Automation_SampleXXX_StartEndpointURL",
                        targetFileStepsText = targetFileStepsText.Replace("Automation_SampleXXX_StartEndpointURL", CurrentStartEndPointURL);
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
                        LiteDB_Action.AddNewAuto_Lite_FunctionStep(AutoName);
                        //###############################/ end liteDB insert
                    }
                    else
                    {
                        string targetFileStepsText = File.ReadAllText(targetFileSteps);
                        //replace the AutoName
                        targetFileStepsText = targetFileStepsText.Replace("Power_Automation_SampleXXX", AutoName);
                        //replace the DESC
                        targetFileStepsText = targetFileStepsText.Replace("Automation_SampleXXX_Description", txt_Description.Text);

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
                        LiteDB_Action.AddNewAuto_Lite_FunctionStep(AutoName);
                        //###############################/ end liteDB insert
        
                    }

                    // isDynamic element case
                    if (txt_Dynamic_EleTagName.Text != "")
                    {
                        if (txt_DynamicEleAttName.Text == "")
                        {
                            MessageBox.Show("Please enter the Element Value,like style, id,name(classname)...");
                            txt_DynamicEleAttName.Focus();
                            return;
                        }
                        CurrentisDynamicElementXPath = "True";
                        CurrentDynamicElementTagName = txt_Dynamic_EleTagName.Text;
                        CurrentDynamicElementAttributeName = txt_DynamicEleAttName.Text;
                        CurrentDynamicElementAttributeValue1 = txt_DynamicEleAttValue1.Text;
                        CurrentDynamicElementAttributeValue2 = txt_DynamicEleAttValue2.Text;

                    }
                    if (txt_DynamicEleAttName.Text != "")
                    {
                        if (txt_Dynamic_EleTagName.Text == "")
                        {
                            MessageBox.Show("Please select the Tag Value,like input,tr,td...");
                            txt_Dynamic_EleTagName.Focus();
                            return;
                        }
                        CurrentisDynamicElementXPath = "True";
                        CurrentDynamicElementTagName = txt_Dynamic_EleTagName.Text;
                        CurrentDynamicElementAttributeName = txt_DynamicEleAttName.Text;
                        CurrentDynamicElementAttributeValue1 = txt_DynamicEleAttValue1.Text;
                        CurrentDynamicElementAttributeValue2 = txt_DynamicEleAttValue2.Text;


                    }
                    //end isDynamic element case


                    ////######################################################## 1.CurrentFunctionCategoryName Case:T_Web_Actions_SendText #########################################################
                    if (CurrentFunctionCategoryName == "T_Web_Actions_SendText")
                    {
                        //######################################################  make sure the function json file is exist ###########################################
                        if (!File.Exists(targetFileBase + "\\" + txt_AutoName.Text + "_STEP_" + CurrentStepID + "_" + CurrentFunctionName + "_Function.json"))
                        {
                            CurrentFromCopyFunctionExecuteJsonLocation = App_Path + "\\settings\\FunctionTemplates\\FunctionTemplate_SendValue_SEL.json";
                            CurrentToCopyFunctionExecuteJsonLocation = targetFileBase + "\\" + txt_AutoName.Text + "_STEP_" + CurrentStepID + "_" + CurrentFunctionName + "_Function.json";
                            File.Copy(CurrentFromCopyFunctionExecuteJsonLocation, CurrentToCopyFunctionExecuteJsonLocation);
                        }
                        //######################################################  end check the function file is exist ################################################
                        string CurrentToCopyFunctionExecuteJsonLocation1 = string.Empty;
                        string targetFileFunctionText = string.Empty;
                        try
                        {
                            targetFileFunctionText = File.ReadAllText(CurrentToCopyFunctionExecuteJsonLocation); //sometimes CurrentToCopyFunctionExecuteJsonLocation is empty
                            CurrentToCopyFunctionExecuteJsonLocation1 = CurrentToCopyFunctionExecuteJsonLocation.Replace(".json", "1.json");

                        }
                        catch (Exception e2)
                        {
                            CurrentToCopyFunctionExecuteJsonLocation = targetFileBase + "\\" + txt_AutoName.Text + "_STEP_" + CurrentStepID + "_" + CurrentFunctionName + "_Function.json";
                            targetFileFunctionText = File.ReadAllText(CurrentToCopyFunctionExecuteJsonLocation);
                            CurrentToCopyFunctionExecuteJsonLocation1 = CurrentToCopyFunctionExecuteJsonLocation.Replace(".json", "1.json");
                        }

                        //replace the isNavigateURL
                        //check PreviousIsNavigateURL is null or have value
                        if (PreviousIsNavigateURL == "" | PreviousIsNavigateURL == "False")
                        {
                            targetFileFunctionText = targetFileFunctionText.Replace("FunctionDetail_isNavigateURL_SampleXXX", CurrentIsNavigateURL.ToString());
                            PreviousIsNavigateURL = CurrentIsNavigateURL.ToString();
                        }
                        else
                        {
                            //need to check if previous is already True
                            if (CurrentIsNavigateURL && PreviousIsNavigateURL == "True")
                            {
                                MessageBox.Show("Prevoius Step has Navigation URL, are you sure to make this again? this will create a new Brower", "Step Setup Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                targetFileFunctionText = targetFileFunctionText.Replace("FunctionDetail_isNavigateURL_SampleXXX", "False");
                            }
                        }

                        // //replace the actual function value base on CurrentToCopyFunctionExecuteJsonLocation, open the file and update the values
                        //                 "isNavigateURL":"FunctionDetail_isNavigateURL_SampleXXX",
                        //"NavigateURL"":"FunctionDetail_NavigateURL_SampleXXX",	 
                        //   "ElementXPath":"FunctionDetail_ElementXPath_SampleXXX",
                        //"SendValue":"FunctionDetail_SendValue_SampleXXX",
                        //"AfterDoneWaitSeconds":"FunctionDetail_AfterDoneWaitSeconds_SampleXXX"

                        //replace the NavigateURL
                        targetFileFunctionText = targetFileFunctionText.Replace("FunctionDetail_NavigateURL_SampleXXX", txt_URL.Text);
                        //replace the ElementXPath
                        targetFileFunctionText = targetFileFunctionText.Replace("FunctionDetail_ElementXPath_SampleXXX", xpathValue);
                        //replace the SendValue
                        targetFileFunctionText = targetFileFunctionText.Replace("FunctionDetail_SendValue_SampleXXX", txt_Tab1_SendValue.Text);
                        //replace the AfterDoneWaitSeconds
                        targetFileFunctionText = targetFileFunctionText.Replace("FunctionDetail_AfterDoneWaitSeconds_SampleXXX", txt_Tab1_WaitT.Text);
                        // replace the "isDynamicElementXPath":"False",
                        targetFileFunctionText = targetFileFunctionText.Replace("\"isDynamicElementXPath\":\"False\",", "\"isDynamicElementXPath\":\"" + CurrentisDynamicElementXPath + "\",");
                        //replace "TagName":"FunctionDetail_DynamicElementTypeName_SampleXXX",
                        targetFileFunctionText = targetFileFunctionText.Replace("FunctionDetail_DynamicElementTagName_SampleXXX", CurrentDynamicElementTagName);
                        //replace DynamicElementAttributeName,
                        targetFileFunctionText = targetFileFunctionText.Replace("FunctionDetail_DynamicElementAttributeName_SampleXXX", CurrentDynamicElementAttributeName);
                        //"DynamicElementAttributeValue1":"FunctionDetail_DynamicElementAttributeValue_SampleXXX",
                        targetFileFunctionText = targetFileFunctionText.Replace("FunctionDetail_DynamicElementAttributeValue1_SampleXXX", CurrentDynamicElementAttributeValue1);
                        targetFileFunctionText = targetFileFunctionText.Replace("FunctionDetail_DynamicElementAttributeValue2_SampleXXX", CurrentDynamicElementAttributeValue2);
                        //replace the redirect url "AfterDoneRedirectURL":"FunctionDetail_AfterDoneRedirectURL_SampleXXX"
                        targetFileFunctionText = targetFileFunctionText.Replace("FunctionDetail_AfterDoneRedirectURL_SampleXXX", txt_Tab1_rediURL.Text);

                        File.WriteAllText(CurrentToCopyFunctionExecuteJsonLocation1, targetFileFunctionText);
                        File.Delete(CurrentToCopyFunctionExecuteJsonLocation);
                        //rename the file 
                        File.Move(CurrentToCopyFunctionExecuteJsonLocation1, CurrentToCopyFunctionExecuteJsonLocation);
                        //end replace the acutla function values
                        //###############################  liteDB insert function to T_ TABLE                     
                        using (var db = new LiteDatabase(frm_Power_Automation.App_Path + "\\PowerAutoLiteDB.db"))
                        {

                            string InsertLiteSQL = "insert into " + CurrentFunctionCategoryName + " VALUES { " +
                                "AutoName: " + "\"" + AutoName + "\"" + ", " +
                                "StepID: " + CurrentStepID  + ", " +
                                "FunctionID:" + "\"" + "ID_" + CurrentFunctionCategoryName + "\"" + ", " +
                                "FunctionCategoryID:" + "\"" + "ID_" + CurrentFunctionCategoryName + "\"" + ", " +
                                "FunDetail_DetailIndex:" +  CurrentFunctionDetail_Index +  ", " +
                               "FunDetail_isNavigateURL:" + CurrentIsNavigateURL + ", " +
                                "FunDetail_NavigateURL:" + "\"" + txt_URL.Text + "\"" + ", " +
                                "FunDetail_Action:" + "\"" + "Send Value" + "\"" + ", " +
                                "FunDetail_isDynamicElementXPath:" + "\"" + CurrentisDynamicElementXPath + "\"" + ", " +
                                "FunDetail_DynamicElementTagName:" + "\"" + CurrentDynamicElementTagName + "\"" + ", " +
                                "FunDetail_DynamicElementAttributeName:" + "\"" + CurrentDynamicElementAttributeName + "\"" + ", " +
                                "FunDetail_DynamicElementAttributeValue1:" + "\"" + CurrentDynamicElementAttributeValue1 + "\"" + ", " +
                                "FunDetail_DynamicElementAttributeValue2:" + "\"" + CurrentDynamicElementAttributeValue2 + "\"" + ", " +
                                "FunDetail_ElementXPath:" + "\"" + xpathValue + "\"" + ", " +
                                "FunDetail_SendValue:" + "\"" + txt_Tab1_SendValue.Text + "\"" + ", " +
                                "FunDetail_AfterDoneWaitSeconds:" + "\"" + txt_Tab1_WaitT.Text + "\"" + ", " +
                                "FunDetail_AfterDoneRedirectURL:" + "\"" + txt_Tab1_rediURL.Text + "\"" + " " +
                                "} ";
                            db.Execute(InsertLiteSQL);
                        }
                        //###############################/ end liteDB insert
                    }

                    ////######################################################## 2.CurrentFunctionCategoryName Case:T_Web_Actions_ClickAction #########################################################
                    if (CurrentFunctionCategoryName == "T_Web_Actions_ClickAction")
                    {
                        //######################################################  make sure the function json file is exist ###########################################
                        if (!File.Exists(targetFileBase + "\\" + txt_AutoName.Text + "_STEP_" + CurrentStepID + "_" + CurrentFunctionName + "_Function.json"))
                        {
                            CurrentFromCopyFunctionExecuteJsonLocation = App_Path + "\\settings\\FunctionTemplates\\FunctionTemplate_ClickButton_SEL.json";
                            CurrentToCopyFunctionExecuteJsonLocation = targetFileBase + "\\" + txt_AutoName.Text + "_STEP_" + CurrentStepID + "_" + CurrentFunctionName + "_Function.json";
                            File.Copy(CurrentFromCopyFunctionExecuteJsonLocation, CurrentToCopyFunctionExecuteJsonLocation);
                        }

                        //######################################################  end check the function file is exist ################################################
                        string CurrentToCopyFunctionExecuteJsonLocation1 = CurrentToCopyFunctionExecuteJsonLocation.Replace(".json", "1.json");
                        string targetFileFunctionText = File.ReadAllText(CurrentToCopyFunctionExecuteJsonLocation);
                        //replace the isNavigateURL
                        //check PreviousIsNavigateURL is null or have value
                        if (PreviousIsNavigateURL == "" | PreviousIsNavigateURL == "False")
                        {
                            targetFileFunctionText = targetFileFunctionText.Replace("FunctionDetail_isNavigateURL_SampleXXX", CurrentIsNavigateURL.ToString());
                            PreviousIsNavigateURL = CurrentIsNavigateURL.ToString();
                        }
                        else
                        {
                            //need to check if previous is already True
                            if (CurrentIsNavigateURL && PreviousIsNavigateURL == "True")
                            {
                                MessageBox.Show("Prevoius Step has Navigation URL, are you sure to make this again? this will create a new Brower", "Step Setup Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                targetFileFunctionText = targetFileFunctionText.Replace("FunctionDetail_isNavigateURL_SampleXXX", "False");
                            }
                        }

                        //                     "isNavigateURL":"FunctionDetail_isNavigateURL_SampleXXX",
                        //"NavigateURL":"FunctionDetail_NavigateURL_SampleXXX",	 
                        //   "ElementXPath":"FunctionDetail_ElementXPath_SampleXXX",
                        //"SendValue":"FunctionDetail_SendValue_SampleXXX",
                        //"AfterDoneWaitSeconds":"FunctionDetail_AfterDoneWaitSeconds_SampleXXX"

                        //replace the NavigateURL
                        targetFileFunctionText = targetFileFunctionText.Replace("FunctionDetail_NavigateURL_SampleXXX", txt_URL.Text);
                        //replace the ElementXPath
                        targetFileFunctionText = targetFileFunctionText.Replace("FunctionDetail_ElementXPath_SampleXXX", xpathValue);

                        //replace the AfterDoneWaitSeconds
                        targetFileFunctionText = targetFileFunctionText.Replace("FunctionDetail_AfterDoneWaitSeconds_SampleXXX", txt_Tab2_WaitT.Text);

                        // replace the "isDynamicElementXPath":"False",
                        targetFileFunctionText = targetFileFunctionText.Replace("\"isDynamicElementXPath\":\"False\",", "\"isDynamicElementXPath\":\"" + CurrentisDynamicElementXPath + "\",");
                        //replace "TagName":"FunctionDetail_DynamicElementTypeName_SampleXXX",
                        targetFileFunctionText = targetFileFunctionText.Replace("FunctionDetail_DynamicElementTagName_SampleXXX", CurrentDynamicElementTagName);
                        //replace DynamicElementAttributeName,
                        targetFileFunctionText = targetFileFunctionText.Replace("FunctionDetail_DynamicElementAttributeName_SampleXXX", CurrentDynamicElementAttributeName);
                        //"DynamicElementAttributeValue":"FunctionDetail_DynamicElementAttributeValue_SampleXXX",
                        targetFileFunctionText = targetFileFunctionText.Replace("FunctionDetail_DynamicElementAttributeValue1_SampleXXX", CurrentDynamicElementAttributeValue1);
                        targetFileFunctionText = targetFileFunctionText.Replace("FunctionDetail_DynamicElementAttributeValue2_SampleXXX", CurrentDynamicElementAttributeValue2);
                        //replace the redirect url "AfterDoneRedirectURL":"FunctionDetail_AfterDoneRedirectURL_SampleXXX"
                        targetFileFunctionText = targetFileFunctionText.Replace("FunctionDetail_AfterDoneRedirectURL_SampleXXX", txt_Tab2_rediURL.Text);
                        File.WriteAllText(CurrentToCopyFunctionExecuteJsonLocation1, targetFileFunctionText);
                        File.Delete(CurrentToCopyFunctionExecuteJsonLocation);
                        //rename the file 
                        File.Move(CurrentToCopyFunctionExecuteJsonLocation1, CurrentToCopyFunctionExecuteJsonLocation);

                        //end replace the acutla function values

                        //###############################  liteDB insert function to T_ TABLE                     
                        using (var db = new LiteDatabase(frm_Power_Automation.App_Path + "\\PowerAutoLiteDB.db"))
                        {

                            string InsertLiteSQL = "insert into " + CurrentFunctionCategoryName + " VALUES { " +
                                "AutoName: " + "\"" + AutoName + "\"" + ", " +
                                "StepID: " + CurrentStepID + ", " +
                                "FunctionID:" + "\"" + "ID_" + CurrentFunctionCategoryName + "\"" + ", " +
                                "FunctionCategoryID:" + "\"" + "ID_" + CurrentFunctionCategoryName + "\"" + ", " +
                                "FunDetail_DetailIndex:" + CurrentFunctionDetail_Index + ", " +
                               "FunDetail_isNavigateURL:" + CurrentIsNavigateURL + ", " +
                                "FunDetail_NavigateURL:" + "\"" + txt_URL.Text + "\"" + ", " +
                                "FunDetail_Action:" + "\"" + "Click Button" + "\"" + ", " +
                                "FunDetail_isDynamicElementXPath:" + "\"" + CurrentisDynamicElementXPath + "\"" + ", " +
                                "FunDetail_DynamicElementTagName:" + "\"" + CurrentDynamicElementTagName + "\"" + ", " +
                                "FunDetail_DynamicElementAttributeName:" + "\"" + CurrentDynamicElementAttributeName + "\"" + ", " +
                                "FunDetail_DynamicElementAttributeValue1:" + "\"" + CurrentDynamicElementAttributeValue1 + "\"" + ", " +
                                "FunDetail_DynamicElementAttributeValue2:" + "\"" + CurrentDynamicElementAttributeValue2 + "\"" + ", " +
                                "FunDetail_ElementXPath:" + "\"" + xpathValue + "\"" + ", " +
                                "FunDetail_SendValue:" + "\"" + "" + "\"" + ", " +
                                "FunDetail_AfterDoneWaitSeconds:" + "\"" + txt_Tab2_WaitT.Text + "\"" + ", " +
                                "FunDetail_AfterDoneRedirectURL:" + "\"" + txt_Tab2_rediURL.Text + "\"" + " " +
                                "} ";
                            db.Execute(InsertLiteSQL);
                        }
                        //###############################/ end liteDB insert


                    }

                    ////######################################################## 3.CurrentFunctionCategoryName Case:T_OperationSystem_KeyAction_SendKey #########################################################
                    if (CurrentFunctionCategoryName == "T_OperationSystem_KeyAction_SendKey")
                    {
                        //######################################################  make sure the function json file is exist ###########################################
                        if (!File.Exists(targetFileBase + "\\" + txt_AutoName.Text + "_STEP_" + CurrentStepID + "_" + CurrentFunctionName + "_Function.json"))
                        {
                            CurrentFromCopyFunctionExecuteJsonLocation = App_Path + "\\settings\\FunctionTemplates\\FunctionTemplate_SendKeys_KEY.json";
                            CurrentToCopyFunctionExecuteJsonLocation = targetFileBase + "\\" + txt_AutoName.Text + "_STEP_" + CurrentStepID + "_" + CurrentFunctionName + "_Function.json";
                            File.Copy(CurrentFromCopyFunctionExecuteJsonLocation, CurrentToCopyFunctionExecuteJsonLocation);
                        }

                        //######################################################  end check the function file is exist ################################################
                        string CurrentToCopyFunctionExecuteJsonLocation1 = CurrentToCopyFunctionExecuteJsonLocation.Replace(".json", "1.json");
                        string targetFileFunctionText = File.ReadAllText(CurrentToCopyFunctionExecuteJsonLocation);
                       
                        string Key1 = string.Empty;
                        string Key2 = string.Empty;
                        string Key3 = string.Empty;

                        switch (cmb_Tab7_Key1.Text)
                        {
                           
                            case "ENTER":
                                Key1 = "{ENTER}";
                                break;
                            case "TAB":
                                Key1 = "{TAB}";
                                break;
                            case "PAGE UP":
                                Key1 = "{PGUP}";
                                break;
                            case "PAGE DOWN":
                                Key1 = "{PGDN}";
                                break;
                            case "BACKSPACE":
                                Key1 = "{BACKSPACE}";
                                break;
                            case "BREAK":
                                Key1 = "{BREAK}";
                                break;
                            case "CAPS LOCK":
                                Key1 = "{CAPSLOCK}";
                                break;
                            case "DELETE":
                                Key1 = "{DELETE}";
                                break;
                            default:
                                Key1 = "{DONOTHING}";
                                break;

                        }
                        switch (cmb_Tab7_Key2.Text)
                        {
                         

                            case "ENTER":
                                Key2 = "{ENTER}";
                                break;
                            case "TAB":
                                Key2 = "{TAB}";
                                break;
                            case "PAGE UP":
                                Key2 = "{PGUP}";
                                break;
                            case "PAGE DOWN":
                                Key2 = "{PGDN}";
                                break;
                            case "BACKSPACE":
                                Key2 = "{BACKSPACE}";
                                break;
                            case "BREAK":
                                Key2 = "{BREAK}";
                                break;
                            case "CAPS LOCK":
                                Key2 = "{CAPSLOCK}";
                                break;
                            case "DELETE":
                                Key2 = "{DELETE}";
                                break;
                            default:
                                Key2 = "{DONOTHING}";
                                break;

                        }
                        switch (cmb_Tab7_Key3.Text)
                        {
                           

                            case "ENTER":
                                Key3 = "{ENTER}";
                                break;
                            case "TAB":
                                Key3 = "{TAB}";
                                break;
                            case "PAGE UP":
                                Key3 = "{PGUP}";
                                break;
                            case "PAGE DOWN":
                                Key3 = "{PGDN}";
                                break;
                            case "BACKSPACE":
                                Key3 = "{BACKSPACE}";
                                break;
                            case "BREAK":
                                Key3 = "{BREAK}";
                                break;
                            case "CAPS LOCK":
                                Key3 = "{CAPSLOCK}";
                                break;
                            case "DELETE":
                                Key3 = "{DELETE}";
                                break;
                            default:
                                Key3 = "{DONOTHING}";
                                break;

                        }
                        //replace the KEYName
                        targetFileFunctionText = targetFileFunctionText.Replace("FunctionDetail_KEYName1_SampleXXX", Key1);
                        //replace the KEYSendTimes
                        targetFileFunctionText = targetFileFunctionText.Replace("FunctionDetail_KEYSendTimes1_SampleXXX", txt_Key1_Times.Text);
                        //replace the KEYSendWaitSeconds
                        targetFileFunctionText = targetFileFunctionText.Replace("FunctionDetail_KEYSendWaitSeconds1_SampleXXX", txt_Key1_WaitSeconds.Text);

                        //replace the KEYName
                        targetFileFunctionText = targetFileFunctionText.Replace("FunctionDetail_KEYName2_SampleXXX", Key2);
                        //replace the KEYSendTimes
                        targetFileFunctionText = targetFileFunctionText.Replace("FunctionDetail_KEYSendTimes2_SampleXXX", txt_Key2_Times.Text);
                        //replace the KEYSendWaitSeconds
                        targetFileFunctionText = targetFileFunctionText.Replace("FunctionDetail_KEYSendWaitSeconds2_SampleXXX", txt_Key2_WaitSeconds.Text);

                        //replace the KEYName
                        targetFileFunctionText = targetFileFunctionText.Replace("FunctionDetail_KEYName3_SampleXXX", Key3);
                        //replace the KEYSendTimes
                        targetFileFunctionText = targetFileFunctionText.Replace("FunctionDetail_KEYSendTimes3_SampleXXX", txt_Key3_Times.Text);
                        //replace the KEYSendWaitSeconds
                        targetFileFunctionText = targetFileFunctionText.Replace("FunctionDetail_KEYSendWaitSeconds3_SampleXXX", txt_Key3_WaitSeconds.Text);
                        //replace the redirect url "AfterDoneRedirectURL":"FunctionDetail_AfterDoneRedirectURL_SampleXXX"
                        targetFileFunctionText = targetFileFunctionText.Replace("FunctionDetail_AfterDoneRedirectURL_SampleXXX", txt_Tab4_rediURL.Text);

                        File.WriteAllText(CurrentToCopyFunctionExecuteJsonLocation1, targetFileFunctionText);
                        File.Delete(CurrentToCopyFunctionExecuteJsonLocation);
                        //rename the file 
                        File.Move(CurrentToCopyFunctionExecuteJsonLocation1, CurrentToCopyFunctionExecuteJsonLocation);

                        //end replace the acutla function values

                        //###############################  liteDB insert function to T_ TABLE                     
                        using (var db = new LiteDatabase(frm_Power_Automation.App_Path + "\\PowerAutoLiteDB.db"))
                        {

                            string InsertLiteSQL = "insert into " + CurrentFunctionCategoryName + " VALUES { " +
                                "AutoName: " + "\"" + AutoName + "\"" + ", " +
                                "StepID: " + CurrentStepID + ", " +
                                "FunctionID:" + "\"" + "ID_" + CurrentFunctionCategoryName + "\"" + ", " +
                                "FunctionCategoryID:" + "\"" + "ID_" + CurrentFunctionCategoryName + "\"" + ", " +
                                "FunDetail_DetailIndex:" + CurrentFunctionDetail_Index + ", " +
                               // "FunDetail_isNavigateURL:" + CurrentIsNavigateURL + ", " +
                               // "FunDetail_NavigateURL:" + "\"" + txt_URL.Text + "\"" + ", " +
                                "FunDetail_Action:" + "\"" + "Send Key" + "\"" + ", " +
                                "FunDetail_SendKeyTimes:" + "\"" + txt_Key1_Times.Text + "\"" + ", " +
                                //"FunDetail_DynamicElementTagName:" + "\"" + CurrentDynamicElementTagName + "\"" + ", " +
                                //"FunDetail_DynamicElementAttributeName:" + "\"" + CurrentDynamicElementAttributeName + "\"" + ", " +
                                //"FunDetail_DynamicElementAttributeValue1:" + "\"" + CurrentDynamicElementAttributeValue1 + "\"" + ", " +
                                //"FunDetail_DynamicElementAttributeValue2:" + "\"" + CurrentDynamicElementAttributeValue2 + "\"" + ", " +
                                //"FunDetail_ElementXPath:" + "\"" + xpathValue + "\"" + ", " +
                                "FunDetail_SendValue:" + "\"" + Key1 + "\"" + ", " +
                                "FunDetail_AfterDoneWaitSeconds:" + "\"" + txt_Key1_WaitSeconds.Text + "\"" + ", " +
                                "FunDetail_AfterDoneRedirectURL:" + "\"" + txt_Tab4_rediURL.Text + "\"" + " " +
                                "} ";
                            db.Execute(InsertLiteSQL);
                        }
                        //###############################/ end liteDB insert

                    }
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

                }
                else
                {
                    // it is new setup, copy the template stepsfunction file only
                    try { File.Copy(App_Path + "\\Functions\\FunctionStepsList_Template.json", targetFileSteps); }
                    catch (Exception e1)
                    {
                        DialogResult resulterror = MessageBox.Show("Step " + CurrentStepID + " Setup Fail, please re-select this step in the combox", "Step Setup Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    //###########################  lite db to insert the automation detail
                    LiteDB_Action.AddNewAuto_Lite(AutoName, targetFileSteps, txt_Description.Text);
                    //###########################  end litedb insert

                }

                DialogResult result = MessageBox.Show("Step " + CurrentStepID + " Saved", "Step Setup Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //save the max_step to the ini file
                string oldValue = File.ReadAllText(CurrentMax_StepFile);
                string oldMax = oldValue.Split(':')[1];
                int newMax = int.Parse(oldMax) + 1;
                CurrentStepID = newMax.ToString();
                //MaxStep:0
                oldValue = oldValue.Replace(oldValue, "MaxStep:" + newMax);
                File.WriteAllText(CurrentMax_StepFile, oldValue);

                //###########################  lite db to UPDATE THE MAX STEP
                LiteDB_Action.UpdateAuto_Lite_MaxStep(AutoName, newMax);
                //###########################  end litedb insert

            }
            //refresh the diagram

            diagram1.Controller.Model = LoadDiagramModel(CurrentAutoName);
            diagram1.Controller.Refresh();

        }
        #endregion

        private void bt_Tab1_SaveStep_Click(object sender, EventArgs e)
        {
            if (txt_XPath.Text == "" & CurrentisDynamicElementXPath=="False")
            {
                MessageBox.Show("Xpath value is empty");
                return;
            }
            if (txt_Tab1_SendValue.Text == "")
            {
                MessageBox.Show("Send Value is empty,please fill in");
                txt_Tab1_SendValue.Focus();
                return;
            }
            SaveStep();
        }

        private void bt_Tab2_SaveStep_Click(object sender, EventArgs e)
        {
            SaveStep();
        }

        private void bt_Tab3_SaveStep_Click(object sender, EventArgs e)
        {
            SaveStep();
        }

        private void bt_Tab4_SaveStep_Click(object sender, EventArgs e)
        {
            SaveStep();
        }

        private void bt_Tab5_SaveStep_Click(object sender, EventArgs e)
        {
            SaveStep();
        }

        public void setCurrentElementXPathValue(TextBox txt_box)
        {
            txt_box.Text = CurrentElementXPathValue;
        }

     
        private void bt_TestLatest_Click(object sender, EventArgs e)
        {
            //RunAutomation(CurrentFunctionsListExecuteJSONFile);
            RunAutomation_Lite(CurrentAutoName);
        }

        public bool CheckIsNavigateURL(CheckBox ck)
        {
            bool isNavigateURL = false;
            if (ck.Checked && int.Parse(ck.Name.Split('_')[1].Replace("Tab", "")) == CurrentTabIndex)
            {
                isNavigateURL = true;
            }
            return isNavigateURL;
        
        }
        private void ck_Tab1_isStartURL_CheckedChanged(object sender, EventArgs e)
        {
            CurrentIsNavigateURL = CheckIsNavigateURL(ck_Tab1_isStartURL);
        }

        private void ck_Tab2_isStartURL_CheckedChanged(object sender, EventArgs e)
        {
            CurrentIsNavigateURL = CheckIsNavigateURL(ck_Tab2_isStartURL);
        }

        private void importScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void importScriptToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frm_ImportScript frm_Import = new frm_ImportScript();
            frm_Import.ShowDialog();
        }

      

        private void bt_Tab7_SaveStep_Click(object sender, EventArgs e)
        {
            SaveStep();
        }

        private void createNewAutomationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitEverything();
            tab_Main.SelectedIndex = 1;
            gb_CreateAutomation.BackColor=Color.LightYellow;
           
            txt_AutoName.Focus();
            txt_AutoName.Select();
            gb_CreateAutomation.Text = "Create New Automation";
         
        }

        private void createNewStepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_Step frmStep = new frm_Step();
           // frm_Power_Automation frm_Parent = new frm_Power_Automation();
            frmStep.MdiParent = this;
            frmStep.Show();
           
            tab_Main.Visible = false;
        }

        private void automationDiagramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_AutoDigram frmDiagram = new frm_AutoDigram();
            // frm_Power_Automation frm_Parent = new frm_Power_Automation();
            frmDiagram.MdiParent = this;
            frmDiagram.Show();
            
            tab_Main.Visible = false;
        }

        private void toolStripCmb_FunctionSteps_Click(object sender, EventArgs e)
        {
            
        }

        private void createNewStepToolStripMenuItem1_Click(object sender, EventArgs e)
        {
          
        }

        private void createNewStepToolStripMenuItem1_TextChanged(object sender, EventArgs e)
        {
           
        }
        private void createNewStepToolStripMenuItem1_CheckedChanged(object sender, EventArgs e)
        {
          
        }
        private void bt_Tab4_CreateMappings_Click(object sender, EventArgs e)
        {
            frm_Function_Sel_Datascrap frmDataSc = new frm_Function_Sel_Datascrap();
            frmDataSc.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            frm_Function_Sel_Datascrap frmDataSc = new frm_Function_Sel_Datascrap();
            frmDataSc.Show();
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {

        }

        int getParentCount(TreeNode node)
        {
            int count = 1;
            while (node.Parent != null)
            {
                count++;
                node = node.Parent;
            }
            return count;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string selectedNode = e.Node.Text;
            string selectedNodePa = "";
            string selectedNodeFull = "";
            int nodePaCount = 0;
            try { nodePaCount = getParentCount(e.Node); } catch (Exception e2) { }
            if (nodePaCount > 1)
            {
                selectedNodePa = e.Node.Parent.FullPath.ToString();
            }
            if (selectedNodePa != "")
            {
                selectedNodePa = selectedNodePa.Replace("Click to Create Functions Step", "");
                selectedNodePa = selectedNodePa.Replace("\\","_");
                selectedNodeFull = "T" + selectedNodePa + "_" + selectedNode;
                selectedNodeFull = selectedNodeFull.Replace(" ", "");
                //###############################  liteDB insert
                FunctionSteps FunStep = new FunctionSteps();

                using (var db = new LiteDatabase(App_Path + "\\PowerAutoLiteDB.db"))
                {
                    // Get a collection (or create, if doesn't exist)
                    var col = db.GetCollection<FunctionSteps>(selectedNodeFull);
                    // Create your new customer instance
                    var AutomationFunctionStep = new FunctionSteps
                    {
                        AutoName = "ClickAction",

                    };
                    col.Insert(AutomationFunctionStep);
                }
                //after creat, delete the empty
                LiteDB_Action.DeleteAuto_Lite_TableEmpyAutoName(selectedNodeFull, "ClickAction");
                CurrentFunctionCategoryName = selectedNodeFull;

                //###############################/ end liteDB insert
            }

           // tab_Main.SelectedIndex = 1;
            //TreeviewSelectFunction(selectedNode);
            TreeviewSelectOpenFunctionForm(selectedNodeFull);
        }



        private void treeView1_Click(object sender, EventArgs e)
        {
            
        }

        private void PrintRecursive(TreeNode treeNode)
        {
            //do something with each node
            //System.Diagnostics.Debug.WriteLine(treeNode.Text);
            foreach (TreeNode tn in treeNode.Nodes)
            {
                PrintRecursive(tn);
                bool contains = Regex.IsMatch(tn.Text, Regex.Escape(txt_SearchFunctionName.Text), RegexOptions.IgnoreCase);
                if (contains)
                {
                    tn.BackColor = Color.Yellow;
                    //tn.Checked = true;
                   // treeView1.SelectedNode = tn;
                }
                else
                {
                    tn.BackColor = Color.White;
                    //tn.Checked = false;
                }
            }
        }

        // Call the procedure using the TreeView.
        private void CallRecursive(TreeView treeView)
        {
            // Print each node recursively.
            TreeNodeCollection nodes = treeView.Nodes;
            foreach (TreeNode n in nodes)
            {
                PrintRecursive(n);

             

            }
        }

        private void txt_SearchFunctionName_TextChanged(object sender, EventArgs e)
        {
            CallRecursive(treeView1);
        }

        private void txt_SearchFunctionName_Click(object sender, EventArgs e)
        {
            txt_SearchFunctionName.Text = null;
        }

       

        private void createNewFunctionToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (txt_AutoName.Text == "")
            {
                MessageBox.Show("AutoName is empty, you need to create an Automation first", "Warning", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                txt_AutoName.Focus();
                return;
            }
            treeView1.Focus();
            treeView1.BackColor = Color.LightYellow;
            
        }

        private void tab_AutoConfigs_DrawItem_1(object sender, DrawItemEventArgs e)
        {
            //Brush brBack, // Background brush
            //             brText = new SolidBrush(Color.Black); // prospects brush
            //Font ftText = new Font("Tahoma", 9.0F); // font
            //Rectangle rcItem = tab_AutoConfigs.GetTabRect(e.Index); // rectangular tab region
            ////switch (e.Index) // different tabs brush different background color
            ////{
            ////    case 0: brBack = new SolidBrush(Color.Blue); break;
            ////    case 1: brBack = new SolidBrush(Color.Red); break;
            ////    case 2: brBack = new SolidBrush(Color.Yellow); break;
            ////    case 3: brBack = new SolidBrush(Color.Purple); break;
            ////    case 4: brBack = new SolidBrush(Color.Pink); break;
            ////    case 5: brBack = new SolidBrush(Color.SeaGreen); break;
            ////    default: brBack = new SolidBrush(Color.Fuchsia); break;
            ////}
            ////e.Graphics.FillRectangle(brBack, rcItem); // with the specified color fill a rectangular region of the tab
            //e.Graphics.DrawString(tab_AutoConfigs.TabPages[e.Index].Text, ftText, brText, rcItem.Location); // draw text with the specified color and font
            ////brBack.Dispose();
            //brText.Dispose();
            //ftText.Dispose();
            //tab_AutoConfigs.TabPages[0].BackColor = Color.White;
            //for (int i = 0; i <= 90; i++)
            //{
            //    try { tab_AutoConfigs.TabPages[i].BackColor = Color.White; } catch (Exception e2) { break; }
            
            //}

        }

        private void tab_AutoConfigs_DrawItem(object sender, DrawItemEventArgs e)
        {
           
            Brush brBack, // Background brush
                         brText = new SolidBrush(Color.Black); // prospects brush
            Font ftText = new Font("Tahoma", 9.0F); // font
            Rectangle rcItem = tab_AutoConfigs.GetTabRect(e.Index); // rectangular tab region
            switch (e.Index) // different tabs brush different background color
            {
                case 0: brBack = new SolidBrush(Color.Blue); break;
                case 1: brBack = new SolidBrush(Color.Red); break;
                case 2: brBack = new SolidBrush(Color.Yellow); break;
                case 3: brBack = new SolidBrush(Color.Purple); break;
                case 4: brBack = new SolidBrush(Color.Pink); break;
                case 5: brBack = new SolidBrush(Color.SeaGreen); break;
                default: brBack = new SolidBrush(Color.Fuchsia); break;
            }
            e.Graphics.FillRectangle(brBack, rcItem); // with the specified color fill a rectangular region of the tab
            e.Graphics.DrawString(tab_AutoConfigs.TabPages[e.Index].Text, ftText, brText, rcItem.Location); // draw text with the specified color and font
            brBack.Dispose();
            brText.Dispose();
            ftText.Dispose();
            tab_AutoConfigs.TabPages[0].BackColor = Color.White;

        }

        private void diagram1_EndDragSelect(object sender, MouseEventArgs e)
        {
            MessageBox.Show("end drag");
        }

        private void diagram1_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show("diagram1_DoubleClick");
        }

        private void diagram1_Click(object sender, EventArgs e)
        {
            try
            {
                var a = diagram1.Model.SelectedShapes();

                //((Crainiate.Diagramming.Table)(new System.Linq.SystemCore_EnumerableDebugView<System.Collections.Generic.KeyValuePair<string, Crainiate.Diagramming.Shape>>(a).Items[0]).Value).SubHeading

                var b = a.Values.ToList()[0];

                //MessageBox.Show(a.get)

                var c = diagram1.Model.SelectedElements();
                var d = c.Values.ToList()[0].ToString();
                CurrentDiagram_SelectStepID = d.ToString();

                //frm_DiagramAction newMDIChild = new frm_DiagramAction();
                //// Set the Parent Form of the Child window.
                //newMDIChild.MdiParent = this;
                //// Display the new form.
                //newMDIChild.TopMost = true;
                //newMDIChild.Focus();
                //newMDIChild.Show();



                frm_DiagramAction frmDiaAction = new frm_DiagramAction();
                frmDiaAction.ShowDialog();

                diagram1.Controller.Model = LoadDiagramModel(CurrentAutoName);
                diagram1.Controller.Refresh();

                //MessageBox.Show(d.ToString());
            }
            catch (Exception e2) 
            {
                diagram1.Controller.Model = LoadDiagramModel(CurrentAutoName);
                diagram1.Controller.Refresh();
            }
           
        }

        private void ck_Tab3_isStartURL_CheckedChanged(object sender, EventArgs e)
        {
            CurrentIsNavigateURL = CheckIsNavigateURL(ck_Tab3_isStartURL);
            
        }

        private void bt_CloseChrome_Click(object sender, EventArgs e)
        {
            var chromeDriverProcesses = Process.GetProcesses().
                              Where(pr => pr.ProcessName == "chromedriver");

            foreach (var process in chromeDriverProcesses)
            {
                process.Kill();
            }

            var chromeProcesses = Process.GetProcesses().
                                Where(pr => pr.ProcessName == "chrome");

            foreach (var process in chromeProcesses)
            {
                process.Kill();
            }
        }

        private void ck_DynamicElement_CheckedChanged(object sender, EventArgs e)
        {
            if (ck_DynamicElement.Checked)
            {
               
                lbl_DynamicElType.Visible = true;
                lbl_DynamicEleAttName.Visible = true;
                txt_Dynamic_EleTagName.Visible = true;
                txt_DynamicEleAttName.Visible = true;
                lbl_DynamicElValue1.Visible = true;
                lbl_DynamicElValue2.Visible = true;
                txt_DynamicEleAttName.Enabled = true;
                txt_DynamicEleAttValue1.Enabled = true;
                txt_DynamicEleAttValue2.Enabled = true;
                txt_Dynamic_EleTagName.Enabled = true;
                tab_ElementSet.Visible = true;
            }
            else
            {

                //lbl_DynamicElType.Visible = false;
                //lbl_DynamicEleAttName.Visible = false;
                //txt_Dynamic_EleTagName.Visible = false;
                //txt_DynamicEleAttName.Visible = false;
                //lbl_DynamicElValue1.Visible = false;
                //lbl_DynamicElValue2.Visible = false;
                //txt_DynamicEleAttValue1.Visible = false;
                //txt_DynamicEleAttValue2.Visible = false;
                //tab_ElementSet.Visible = false;
                txt_DynamicEleAttName.Enabled = false;
                txt_DynamicEleAttValue1.Enabled = false;
                txt_DynamicEleAttValue2.Enabled = false;
                txt_Dynamic_EleTagName.Enabled = false;

                txt_Dynamic_EleTagName.Text = "";
                txt_DynamicEleAttName.Text = "";
                txt_DynamicEleAttValue1.Text = "";
                txt_DynamicEleAttValue2.Text = "";

            }
        }

        private void lbl_XP2_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txt_XPath_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_XPath2_TextChanged(object sender, EventArgs e)
        {

        }

        private void dg_AutoList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string ClickContent=dg_AutoList.SelectedCells[0].Value.ToString();
            int ClickRowIndex = dg_AutoList.SelectedCells[0].RowIndex;
            int ClickColumn = dg_AutoList.SelectedCells[0].ColumnIndex;
            string ClickRunFile = dg_AutoList.Rows[ClickRowIndex].Cells[3].Value.ToString();
            string ClickRunAutoName = dg_AutoList.Rows[ClickRowIndex].Cells[1].Value.ToString();
            CurrentRuningAutoFile = ClickRunFile;
            CurrentAutoName = ClickRunAutoName;
            if (ClickContent == "Run")
            {
                //update the Run_LastRuntime on Automations table
                LiteDB_Action.UpdateAuto_Lite_RunLastRuntime(CurrentAutoName,DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss"));
                //start the running function
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += new DoWorkEventHandler(bgw_Autos_DoWork);
                worker.RunWorkerAsync();

            }
            if (ClickColumn == 0)
            {
                dg_AutoList.Rows[ClickRowIndex].Selected=true;

            }
            if (ClickColumn == 1)
            {
                //get the currentAutoName and load the diagram
                List<FunctionSteps> SelectedAutoFunctionList=LiteDB_Action.GetFunctionSteps_Lite_ByAutoName(CurrentAutoName);
                diagram1.Controller.Model = LoadDiagramModel(CurrentAutoName);
                diagram1.Controller.Refresh();


            }
            if (ClickContent == "Delete")
            {
                //delete the function folder
                DialogResult dialogResult = MessageBox.Show("Are you sure to delete Automation "+ ClickRunAutoName + "?", "Confirm to delete", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    string AutoFolder = App_Path + "\\Functions\\" + ClickRunAutoName;
                    if (Directory.Exists(AutoFolder)) Directory.Delete(AutoFolder, true);

                    LiteDB_Action.DeleteAuto_Lite(ClickRunAutoName);
                    //after delete , refresh
                    GetAllAutomationsList( dg_AutoList);
                 
                }
              
            }
            if (ClickContent == "Open Result")
            {
                string CSVResult = App_Path + "\\Functions\\" + ClickRunAutoName+"\\"+ ClickRunAutoName+"_Scrapper.csv";
                try { System.Diagnostics.Process.Start(CSVResult); } catch(Exception e2){ }
               

            }
            
            int count = dg_AutoList.Rows.Count;
            List<string> List_toRun = new List<string>();
            for (int i = 0; i <= count - 1; i++)
            {
                string Selected = dg_AutoList.Rows[i].Cells[0].Value.ToString();
                if (Selected == "True" | dg_AutoList.Rows[i].Selected)
                {
                    dg_AutoList.Rows[i].Selected = true;
                }

            }


        }

        private void bgw_Autos_DoWork(object sender, DoWorkEventArgs e)
        {
            //RunAutomation_Async(CurrentRuningAutoFile);

            string RunJsonFile = CurrentRuningAutoFile.Replace("\\\\","\\");

            // // Part 1: use ProcessStartInfo class.
            // ProcessStartInfo startInfo = new ProcessStartInfo();
            // startInfo.CreateNoWindow = true;
            // startInfo.UseShellExecute = true;
            //// startInfo.RedirectStandardOutput = true;
            // startInfo.FileName = App_Path + "\\PowerAutoConsole.exe";
            // startInfo.WindowStyle = ProcessWindowStyle.Normal;          
            // // Part 2: set arguments.
            // startInfo.Arguments = RunJsonFile;
            // try
            // {
            //     // Part 3: start with the info we specified.
            //     // ... Call WaitForExit.
            //     Process exeProcess = Process.Start(startInfo);
            //     string output = exeProcess.StandardOutput.ReadToEnd();
            //     lv_AutoLogs.Items.Add(output);
            //     //while (!exeProcess.StandardOutput.EndOfStream)
            //     //{
            //     //    string line = exeProcess.StandardOutput.ReadLine();
            //     //    lv_AutoLogs.Items.Add(line);
            //     //}

            // }
            // catch (Exception e1)
            // {

            // }

            // // System.Diagnostics.Process.Start(App_Path+ "\\PowerAutoConsole.exe" , RunFile);

            //
            // Setup the process with the ProcessStartInfo class.
            //
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = App_Path + "\\PowerAutoConsole.exe"; // Specify exe name.
            start.UseShellExecute = false;
           // start.RedirectStandardOutput = true;
            start.Arguments = CurrentAutoName;
            start.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden; //Hides GUI
            start.CreateNoWindow = true; //Hides console
            //
            // Start the process.
            //
            Process process = Process.Start(start);



        }

        private void bgw_Autos_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void bgw_Autos_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void lbl_DynamicElValue1_Click(object sender, EventArgs e)
        {

        }

        private void lbl_DynamicElType_Click(object sender, EventArgs e)
        {

        }

        private void lbl_DynamicElValue2_Click(object sender, EventArgs e)
        {

        }

        private void lbl_DynamicEleAttName_Click(object sender, EventArgs e)
        {

        }

        private void txt_Dynamic_EleTagName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_DynamicEleAttName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_DynamicEleAttValue1_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_DynamicEleAttValue2_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox_Search_TextChanged(object sender, EventArgs e)
        {
            int count = dg_AutoList.Rows.Count;
            for (int i = 0; i <= count - 1; i++)
            {
                string dgText = dg_AutoList.Rows[i].Cells[2].Value.ToString();
                bool contains = Regex.IsMatch(dgText, Regex.Escape(toolStripTextBox_Search.Text), RegexOptions.IgnoreCase);

                if (contains)
                {
                    dg_AutoList.Rows[i].Selected = true;
                }
                else
                {
                    dg_AutoList.Rows[i].Selected = false;
                }

            }
        }

        private void toolStripTextBox_Search_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void toolStripTextBox_Search_Click(object sender, EventArgs e)
        {
            toolStripTextBox_Search.Text = "";
        }

        private void toolStripMenuItem_RunSelected_Click(object sender, EventArgs e)
        {
            int count = dg_AutoList.Rows.Count;
            List<string> List_toRun = new List<string>();
            for (int i = 0; i <= count - 1; i++)
            {
                string Selected = dg_AutoList.Rows[i].Cells[0].Value.ToString();
                if (Selected == "True" | dg_AutoList.Rows[i].Selected)
                {
                    CurrentRuningAutoFile = dg_AutoList.Rows[i].Cells[3].Value.ToString();
                    List_toRun.Add(CurrentRuningAutoFile);
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += new DoWorkEventHandler(bgw_Autos_DoWork);
                    worker.RunWorkerAsync();
                    Thread.Sleep(3000);
                }
               
            }

           
        }

        private void toolStripSplitButton_AutoRun_ButtonClick(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox_Search_Click_1(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox_Search_TextChanged_1(object sender, EventArgs e)
        {
            int count = dg_AutoList.Rows.Count;
            for (int i = 0; i <= count - 1; i++)
            {
                string dgText = dg_AutoList.Rows[i].Cells[2].Value.ToString();
                bool contains = Regex.IsMatch(dgText, Regex.Escape(toolStripTextBox_Search.Text), RegexOptions.IgnoreCase);

                if (contains)
                {
                    dg_AutoList.Rows[i].Selected = true;
                }
                else
                {
                    dg_AutoList.Rows[i].Selected = false;
                }

            }
        }

        private void batchRunSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int count = dg_AutoList.Rows.Count;
            List<string> List_toRun = new List<string>();
            for (int i = 0; i <= count - 1; i++)
            {
                string Selected = dg_AutoList.Rows[i].Cells[0].Value.ToString();
                if (Selected == "True" | dg_AutoList.Rows[i].Selected)
                {
                    CurrentRuningAutoFile = dg_AutoList.Rows[i].Cells[3].Value.ToString();
                    CurrentAutoName= dg_AutoList.Rows[i].Cells[1].Value.ToString();
                    List_toRun.Add(CurrentRuningAutoFile);
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += new DoWorkEventHandler(bgw_Autos_DoWork);
                    worker.RunWorkerAsync();
                    Thread.Sleep(3000);
                }

            }
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripDropDownButton1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
