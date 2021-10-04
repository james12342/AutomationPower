using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PowerAutomationTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void bt_TestLatest_Click(object sender, EventArgs e)
        {
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
            driver.Url = "https://www.facebook.com/";
            driver.FindElement(By.XPath("//*[@id=\"email\"]")).SendKeys("6263833666");
            driver.FindElement(By.XPath("//*[@id=\"pass\"]")).SendKeys("kentxy_01");

            IList<IWebElement> all_login = driver.FindElements(By.CssSelector("login"));
            String[] allText_SKU = new String[all_login.Count];
           
            foreach (IWebElement item in all_login)
            {

                string linkValue = item.GetAttribute("id");
                string skuvalue_href = item.Text;
            }


            }
    }
}
