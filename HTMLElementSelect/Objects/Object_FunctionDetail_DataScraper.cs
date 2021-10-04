using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLElementSelect.Objects
{
   public class Object_FunctionDetail_DataScraper
	{

		//     {
		//"":"FunctionDetail_isNavigateURL_SampleXXX",
		//"":"FunctionDetail_NavigateURL_SampleXXX",	 
		//"":"Data Scrap",
		//   "":"FunctionDetail_ElementXPath_SampleXXX",
		//"":"FunctionDetail_ElementXPath_SampleXXX",
		//"":"FunctionDetail_ElementXPath_SampleXXX",
		//"":"equal",  
		//"":"FunctionDetail_AfterDoneWaitSeconds_SampleXXX",  
		//"":"FunctionDetail_isElementMedia_SampleXXX",
		//"":"FunctionDetail_MediaDownloadSaveName_SampleXXX",
		//"":"FunctionDetail_MediaDownloadSaveLocation_SampleXXX"
		// },


		public string isNavigateURL { get; set; }
		public string NavigateURL { get; set; }

		public string Action { get; set; }
		public string ElementXPath { get; set; }
		public string ElementToColumnName { get; set; }
		public string ElementToColumnIndex { get; set; }
		public string ConditionCompareSymbol { get; set; }
		public string ConditionCompareValue { get; set; }
		public string isElementMedia { get; set; }
		public string MediaDownloadSaveName { get; set; }
		public string MediaDownloadSaveLocation { get; set; }
		
	}
}
