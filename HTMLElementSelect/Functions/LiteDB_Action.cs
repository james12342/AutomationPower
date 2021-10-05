using HTMLElementSelect.Objects;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HTMLElementSelect.Objects.Object_Lite_FunctionSteps;

namespace HTMLElementSelect.Functions
{
    public class LiteDB_Action
    {

        #region "UPDATE"
        public static void UpdateAuto_Lite_MaxStep(string AutoName, int maxStep)
        {
            using (var db = new LiteDatabase(frm_Power_Automation.App_Path + "\\PowerAutoLiteDB.db"))
            {
                // Get a collection (or create, if doesn't exist)
                var col = db.GetCollection<Object_AutomationInfo>("Automations");

                // Create your new customer instance
                var Automation = new Object_AutomationInfo
                {
                    AutoName = AutoName

                };

                string updateLiteSQL = "update Automations set MaxSteps =" + maxStep + " where AutoName ='" + AutoName + "'";
                db.Execute(updateLiteSQL);
                // col.Update(Automation);
            }
        }

        public static void UpdateAuto_Lite_RunLastRuntime(string AutoName, string LastRuntime)
        {
            using (var db = new LiteDatabase(frm_Power_Automation.App_Path + "\\PowerAutoLiteDB.db"))
            {
                // Get a collection (or create, if doesn't exist)
                var col = db.GetCollection<Object_AutomationInfo>("Automations");

                // Create your new customer instance
                var Automation = new Object_AutomationInfo
                {
                    AutoName = AutoName

                };

                string updateLiteSQL = "update Automations set Run_LastRuntime ='" + LastRuntime + "' where AutoName ='" + AutoName + "'";
                db.Execute(updateLiteSQL);
                // col.Update(Automation);
            }
        }

        #endregion

        #region "SELECT"
        public static List<FunctionSteps> GetFunctionSteps_Lite_ByAutoName(string AutoName)
        {
            //###############################  liteDB insert function steps
            List<FunctionSteps> AutoFunctionList = new List<FunctionSteps>();
            using (var db = new LiteDatabase(frm_Power_Automation.App_Path + "\\PowerAutoLiteDB.db"))
            {
                string sql = "SELECT $ FROM AutoFunctionSteps where AutoName=" + "\"" + AutoName + "\"" + " order by StepID";
                using (var bsonReader = db.Execute(sql))
                {
                    var output = new List<BsonValue>();
                    int i = 0;
                    while (bsonReader.Read())
                    {
                        FunctionSteps itemAutoSteps = new FunctionSteps();

                        output.Add(bsonReader.Current);
                        //if (output[i]["AutoName"].IsString) 
                        //{
                        itemAutoSteps.AutoName = output[i]["AutoName"].ToString().TrimStart('"').TrimEnd('"');
                        itemAutoSteps.Description = output[i]["Description"].ToString().TrimStart('"').TrimEnd('"');
                        itemAutoSteps.StepID = int.Parse(output[i]["StepID"].ToString().TrimStart('"').TrimEnd('"'));
                        itemAutoSteps.FunctionTableName = output[i]["FunctionTableName"].ToString();
                        itemAutoSteps.FunctionName = output[i]["FunctionName"].ToString().TrimStart('"').TrimEnd('"');
                        AutoFunctionList.Add(itemAutoSteps);
                        i++;
                    }
                }
                // return output;
            }
            return AutoFunctionList;
            //###############################/ end liteDB insert
        }

        public static List<FunctionSteps> GetFunctionDetail_Lite_ByAutoName(string AutoName)
        {
            //###############################  liteDB insert function steps
            List<FunctionSteps> AutoFunctionList = new List<FunctionSteps>();
            using (var db = new LiteDatabase(frm_Power_Automation.App_Path + "\\PowerAutoLiteDB.db"))
            {
                string sql = "SELECT $ FROM AutoFunctionSteps where AutoName=" + "\"" + AutoName + "\"" + " order by StepID";
                using (var bsonReader = db.Execute(sql))
                {
                    var output = new List<BsonValue>();
                    int i = 0;
                    while (bsonReader.Read())
                    {
                        FunctionSteps itemAutoSteps = new FunctionSteps();

                        output.Add(bsonReader.Current);
                        //if (output[i]["AutoName"].IsString) 
                        //{
                        itemAutoSteps.AutoName = output[i]["AutoName"].ToString().TrimStart('"').TrimEnd('"');
                        itemAutoSteps.Description = output[i]["Description"].ToString().TrimStart('"').TrimEnd('"');
                        itemAutoSteps.StepID = int.Parse(output[i]["StepID"].ToString().TrimStart('"').TrimEnd('"'));
                        itemAutoSteps.FunctionTableName = output[i]["FunctionTableName"].ToString();
                        itemAutoSteps.FunctionName = output[i]["FunctionName"].ToString().TrimStart('"').TrimEnd('"');
                        AutoFunctionList.Add(itemAutoSteps);
                        i++;
                    }
                }
                // return output;
            }
            return AutoFunctionList;
            //###############################/ end liteDB insert
        }
        #endregion

        #region "ADD"
        public static void AddNewAuto_Lite(string AutoName, string desc, string tragetFileSteps)
        {
            using (var db = new LiteDatabase(frm_Power_Automation.App_Path + "\\PowerAutoLiteDB.db"))
            {
                // Get a collection (or create, if doesn't exist)
                var col = db.GetCollection<Object_AutomationInfo>("Automations");

                // Create your new customer instance
                var Automation = new Object_AutomationInfo
                {
                    AutoName = AutoName,
                    Description = desc,
                    AutoStepFunctionLocation = tragetFileSteps,
                    IsActive = true,
                    Run_Step = 0,
                    Run_ResultDataFileName = AutoName + ".CSV OR Others",
                    Run_LastRuntime = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss")
                };

                //firstly, check if the autoName is dupicated in Litedb, if yes, do nothing, if not, insert it 
                var isExist = col.Exists(x => x.AutoName == AutoName);
                if (!isExist)
                {
                    col.Insert(Automation);
                    Console.WriteLine("New Automation:" + Automation + " Added");
                }
                else
                {
                    Console.WriteLine("Automation:" + Automation + " Alreade exist");
                }

            }
        }
        public static void AddNewAuto_Lite_FunctionStep(string AutoName)
        {
            //###############################  liteDB insert function steps
            FunctionSteps FunStep = new FunctionSteps();
            using (var db = new LiteDatabase(frm_Power_Automation.App_Path + "\\PowerAutoLiteDB.db"))
            {
                // Get a collection (or create, if doesn't exist)
                var col = db.GetCollection<FunctionSteps>("AutoFunctionSteps");

                string FuntionName = "F_" + frm_Power_Automation.CurrentFunctionName + "_N_" + AutoName + "_FID_" + frm_Power_Automation.CurrentStepID;
                FuntionName = FuntionName.Replace(" ", "_").Replace("|", "_");

                // Create your new customer instance
                var AutomationFunctionStep = new FunctionSteps
                {
                    AutoName = AutoName,

                    FunctionName = FuntionName,
                    FunctionTableName = frm_Power_Automation.CurrentFunctionCategoryName,
                    StepID = int.Parse(frm_Power_Automation.CurrentStepID)

                };
                col.Insert(AutomationFunctionStep);
            }
            //###############################/ end liteDB insert
        }
        #endregion

        #region "DELETE"
        //delete the whole automation by autoname
        public static void DeleteAuto_Lite(string AutoName)
        {
            using (var db = new LiteDatabase(frm_Power_Automation.App_Path + "\\PowerAutoLiteDB.db"))
            {
                string deleteLiteSQL_Automations = "Delete Automations  where AutoName ='" + AutoName + "'";
                db.Execute(deleteLiteSQL_Automations);
                //delete Autoname=null
                string deleteLiteSQL_AutomationsNull = "Delete Automations  where AutoName =null";
                db.Execute(deleteLiteSQL_AutomationsNull);

                string deleteLiteSQL_AutoFunctionSteps = "Delete AutoFunctionSteps  where AutoName ='" + AutoName + "'";
                db.Execute(deleteLiteSQL_AutoFunctionSteps);
                string deleteLiteSQL_T_Web_Actions_SendText = "Delete T_Web_Actions_SendText  where AutoName ='" + AutoName + "'";
                db.Execute(deleteLiteSQL_T_Web_Actions_SendText);
                string deleteLiteSQL_T_Web_Actions_ClickAction = "Delete T_Web_Actions_ClickAction  where AutoName ='" + AutoName + "'";
                db.Execute(deleteLiteSQL_T_Web_Actions_ClickAction);
                string deleteLiteSQL_T_Web_Actions_DataScraper = "Delete T_Web_Actions_DataScraper  where AutoName ='" + AutoName + "'";
                db.Execute(deleteLiteSQL_T_Web_Actions_DataScraper);
                string deleteLiteSQL_T_OperationSystem_KeyAction_SendKey = "Delete T_OperationSystem_KeyAction_SendKey  where AutoName ='" + AutoName + "'";
                db.Execute(deleteLiteSQL_T_OperationSystem_KeyAction_SendKey);
            }
        }
        public static void DeleteAuto_Lite_TableEmpyAutoName(string TableName, string AutoName)
        {
            using (var db = new LiteDatabase(frm_Power_Automation.App_Path + "\\PowerAutoLiteDB.db"))
            {

                string deleteLiteSQL = "Delete " + TableName + "  where AutoName ='" + AutoName + "'";
                db.Execute(deleteLiteSQL);
            }
        }

        //delete the function in automation by autoname
        public static void DeleteFuntionStepInAuto_Lite(string AutoName,string StepID)
        {
            using (var db = new LiteDatabase(frm_Power_Automation.App_Path + "\\PowerAutoLiteDB.db"))
            {
                string deleteLiteSQL_AutoFunctionSteps = "Delete AutoFunctionSteps  where AutoName ='" + AutoName + "' AND StepID="+StepID;
                db.Execute(deleteLiteSQL_AutoFunctionSteps);
               
            }
        }
        #endregion

    }
}
