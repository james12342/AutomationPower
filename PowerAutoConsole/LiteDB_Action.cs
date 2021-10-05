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
        public static string LiteDBPath = @"C:\work\git\AutomationPower\HTMLElementSelect\bin\Debug\PowerAutoLiteDB.db";
        //public static string LiteDBPath = @"C:\work\HJ\AutomationPower\HTMLElementSelect\bin\Debug\PowerAutoLiteDB.db";

        #region "UPDATE"
        public static void UpdateAuto_Lite_MaxStep(string AutoName, int maxStep)
        {
            using (var db = new LiteDatabase(@"C:\work\HJ\AutomationPower\HTMLElementSelect\bin\Debug\PowerAutoLiteDB.db"))
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
            //using (var db = new LiteDatabase(frm_Power_Automation.App_Path + "\\PowerAutoLiteDB.db")
            using (var db = new LiteDatabase(@"C:\work\HJ\AutomationPower\HTMLElementSelect\bin\Debug\PowerAutoLiteDB.db"))
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
        // Get the Function Setp in AutoFunctionSteps table
        public static List<FunctionSteps> GetFunctionSteps_Lite_ByAutoName(string AutoName)
        {
            //###############################  liteDB insert function steps
            List<FunctionSteps> AutoFunctionList = new List<FunctionSteps>();
            using (var db = new LiteDatabase(LiteDBPath))
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

        // Get the Function Action in table it belong to (T_xxx table)

        public static Object_FunctionActionDetail GetFunctionAction_Lite_ByTableName_AutoName_StepID(string TableName,string AutoName,string StepID)
        {
            //###############################  liteDB search function action
            Object_FunctionActionDetail FunctionActionDetail = new Object_FunctionActionDetail();

            using (var db = new LiteDatabase(LiteDBPath))
            {
                TableName = TableName.Replace("\"","");
                AutoName = AutoName.Replace("\"", "");
                string sql = "SELECT $ FROM "+ TableName + " where AutoName=" +"\""+ AutoName + "\""+ " and StepID="+StepID+"";
                
                using (var bsonReader = db.Execute(sql))
                {
                    var output = new List<BsonValue>();
                    int i = 0;
                    while (bsonReader.Read())
                    {
                       

                        output.Add(bsonReader.Current);
                       
                        FunctionActionDetail._id = output[i]["_id"].ToString().TrimStart('"').TrimEnd('"');
                        FunctionActionDetail.AutoName = output[i]["AutoName"].ToString().TrimStart('"').TrimEnd('"');
                        FunctionActionDetail.StepID = output[i]["StepID"].ToString().TrimStart('"').TrimEnd('"');
                        FunctionActionDetail.FunctionID = output[i]["FunctionID"].ToString().TrimStart('"').TrimEnd('"');
                        FunctionActionDetail.FunctionCategoryID = output[i]["FunctionCategoryID"].ToString().TrimStart('"').TrimEnd('"');
                        FunctionActionDetail.FunDetail_DetailIndex = output[i]["FunDetail_DetailIndex"].ToString().TrimStart('"').TrimEnd('"');
                        FunctionActionDetail.FunDetail_isNavigateURL = output[i]["FunDetail_isNavigateURL"].ToString().TrimStart('"').TrimEnd('"');
                        FunctionActionDetail.FunDetail_NavigateURL = output[i]["FunDetail_NavigateURL"].ToString().TrimStart('"').TrimEnd('"');
                        FunctionActionDetail.FunDetail_Action = output[i]["FunDetail_Action"].ToString().TrimStart('"').TrimEnd('"');
                        FunctionActionDetail.FunDetail_isDynamicElementXPath = output[i]["FunDetail_isDynamicElementXPath"].ToString().TrimStart('"').TrimEnd('"');
                        FunctionActionDetail.FunDetail_DynamicElementTagName = output[i]["FunDetail_DynamicElementTagName"].ToString().TrimStart('"').TrimEnd('"');
                        FunctionActionDetail.FunDetail_DynamicElementAttributeName = output[i]["FunDetail_DynamicElementAttributeName"].ToString().TrimStart('"').TrimEnd('"');
                        FunctionActionDetail.FunDetail_DynamicElementAttributeValue1 = output[i]["FunDetail_DynamicElementAttributeValue1"].ToString().TrimStart('"').TrimEnd('"');
                        FunctionActionDetail.FunDetail_DynamicElementAttributeValue2 = output[i]["FunDetail_DynamicElementAttributeValue2"].ToString().TrimStart('"').TrimEnd('"');
                        FunctionActionDetail.FunDetail_ElementXPath = output[i]["FunDetail_ElementXPath"].ToString().TrimStart('"').TrimEnd('"');
                        FunctionActionDetail.FunDetail_SendValue = output[i]["FunDetail_SendValue"].ToString().TrimStart('"').TrimEnd('"');
                        FunctionActionDetail.FunDetail_AfterDoneWaitSeconds = output[i]["FunDetail_AfterDoneWaitSeconds"].ToString().TrimStart('"').TrimEnd('"');
                        FunctionActionDetail.FunDetail_AfterDoneRedirectURL = output[i]["FunDetail_AfterDoneRedirectURL"].ToString().TrimStart('"').TrimEnd('"');
                        //send key
                        FunctionActionDetail.FunDetail_SendKeyTimes= output[i]["FunDetail_SendKeyTimes"].ToString().TrimStart('"').TrimEnd('"');



                    }
                }
               
            }
            // return output;
            return FunctionActionDetail;

        }
        #endregion

        #region "ADD"
        public static void AddNewAuto_Lite(string AutoName, string desc, string tragetFileSteps)
        {
            using (var db = new LiteDatabase(LiteDBPath))
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
     
        #endregion

        #region "DELETE"
        //delete the whole automation by autoname
        public static void DeleteAuto_Lite(string AutoName)
        {
            using (var db = new LiteDatabase(LiteDBPath))
            {
                string deleteLiteSQL_Automations = "Delete Automations  where AutoName ='" + AutoName + "'";
                db.Execute(deleteLiteSQL_Automations);
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
            using (var db = new LiteDatabase(LiteDBPath))
            {

                string deleteLiteSQL = "Delete " + TableName + "  where AutoName ='" + AutoName + "'";
                db.Execute(deleteLiteSQL);
            }
        }

        //delete the function in automation by autoname
        public static void DeleteFuntionStepInAuto_Lite(string AutoName,string StepID)
        {
            using (var db = new LiteDatabase(@"C:\work\HJ\AutomationPower\HTMLElementSelect\bin\Debug\PowerAutoLiteDB.db"))
            {
                string deleteLiteSQL_AutoFunctionSteps = "Delete AutoFunctionSteps  where AutoName ='" + AutoName + "' AND StepID="+StepID;
                db.Execute(deleteLiteSQL_AutoFunctionSteps);
               
            }
        }
        #endregion

    }
}
