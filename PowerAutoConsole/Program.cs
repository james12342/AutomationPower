using LiteDB;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PowerAutoConsole
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string[] Phones { get; set; }
        public bool IsActive { get; set; }
    }

    public class Automation
    {
        public int Id { get; set; }
        public string AutoName { get; set; }
        public string Description { get; set; }
        public string AutomationExecuteFileName { get; set; }
        public bool IsActive { get; set; }
        public string Run_ResultDataFileName { get; set; }
        public string Run_LastRuntime { get; set; }
        public string Run_Status { get; set; }
    }



    class Program
    {
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
        //db
        //#################  end scraper  #####################

        //##############  Functions ##########
        public static List<string> CurrentAutomationList = new List<string>();

        public static string CurrentRuningAutoFile = string.Empty;
        //############# end function list ######

        static void Main(string[] args)
        {

            //test db
            using (var db = new LiteDatabase(@"C:\Users\19135\OneDrive\Desktop\HTMLElementSelect\HTMLElementSelect\bin\Debug\PowerAutoLiteDB.db"))
            {
                using (var bsonReader = db.Execute("SELECT $ FROM Automations order by _id desc"))
                {
                  
                }
            }
            //test db
            App_Path = System.IO.Directory.GetCurrentDirectory();
            Console.WriteLine("App_Path====>" + App_Path);

            if (args.Length==0)
            {
                Console.WriteLine("argument is null");
               // RunAutomation_Async(@"C:\Users\james\Downloads\HTMLElementSelect\HTMLElementSelect\HTMLElementSelect\bin\Debug\Functions\LogicBroker pickList\LogicBroker pickList_FunctionSteps.json");
            }
            else
            {
                // Step 2: print length, and loop over all arguments.
               // Console.Write("args length is ");
               // Console.WriteLine(args);
                string argument = string.Empty;
                for (int i = 0; i < args.Length; i++)
                {
                    argument = argument+" "+args[i];
                   
                }
                Console.Write("Running " + argument);
                if (argument != "")
                {
                    argument = argument.TrimStart(' ');
                    RunAutomation_Async(argument);
                }
               
            }
            //Console.ReadLine();
            Environment.Exit(0);
        }
        public static string GetLatestStepListFile(string DirPath, string spec)
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
        public static void RunAutomation_Async(string _CurrentAutoName)
        {
            //App_Path = System.IO.Directory.GetCurrentDirectory();
            // first, get the function list step file
            try
            {              
                string runJsonStepFile = App_Path + "\\Functions\\"+_CurrentAutoName+"\\"+ _CurrentAutoName + "_FunctionSteps.json";
                Console.WriteLine("Analysising====>" + runJsonStepFile);
                JO_ExecuteFunctionListJson = JObject.Parse(File.ReadAllText(runJsonStepFile));
            }
            catch (Exception e1)
            {
                Console.WriteLine("##### !!!!! Someting wrong,get the latest json from folder !!!! ######");
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
                                Console.WriteLine("File:"+ Current_Scaper_SaveDataFileName+" generated");
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
                         
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }

                       
                        //System.Diagnostics.Process.Start(Current_Scaper_SaveDataFileName);
                        //end get function
                    }

                    // diagram1.Controller.Model = LoadDiagramModel();
                    // diagram1.Controller.Refresh();
                    Console.WriteLine("File:" + Current_Scaper_SaveDataFileName + " generated");
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
            Console.WriteLine(JO_ExecuteFunctionListJson["AutoName"].ToString()+" Automation finish at:" +DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss"));
          
        }

        public static void RunAutomationLite_Async(string _CurrentAutoName)
        {
            //App_Path = System.IO.Directory.GetCurrentDirectory();
            // first, get the function list step file
            try
            {
                string runJsonStepFile = App_Path + "\\Functions\\" + _CurrentAutoName + "\\" + _CurrentAutoName + "_FunctionSteps.json";
                Console.WriteLine("Analysising====>" + runJsonStepFile);
                JO_ExecuteFunctionListJson = JObject.Parse(File.ReadAllText(runJsonStepFile));
            }
            catch (Exception e1)
            {
                Console.WriteLine("##### !!!!! Someting wrong,get the latest json from folder !!!! ######");
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
                                Console.WriteLine("File:" + Current_Scaper_SaveDataFileName + " generated");
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

                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }


                        //System.Diagnostics.Process.Start(Current_Scaper_SaveDataFileName);
                        //end get function
                    }

                    // diagram1.Controller.Model = LoadDiagramModel();
                    // diagram1.Controller.Refresh();
                    Console.WriteLine("File:" + Current_Scaper_SaveDataFileName + " generated");
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
            Console.WriteLine(JO_ExecuteFunctionListJson["AutoName"].ToString() + " Automation finish at:" + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss"));

        }
    }
}
