using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLElementSelect.Objects
{
	//##########################format:  Obj_FunctionAction_T_Table Name in the litedb ######################
	//send text to a web text
	public class Object_FunctionActionDetail
	{
		//send text to a web text
		public string _id { get; set; }
		public string AutoName { get; set; }
		public string isNavigateURL { get; set; }
		public string StepID { get; set; }
		public string FunctionID { get; set; }
		public string FunctionCategoryID { get; set; }
		public string FunDetail_DetailIndex { get; set; }
		public string FunDetail_isNavigateURL { get; set; }
		public string FunDetail_NavigateURL { get; set; }
		public string FunDetail_Action { get; set; }
		public string FunDetail_isDynamicElementXPath { get; set; }
		public string FunDetail_DynamicElementTagName { get; set; }
		public string FunDetail_DynamicElementAttributeName { get; set; }
		public string FunDetail_DynamicElementAttributeValue1 { get; set; }
		public string FunDetail_DynamicElementAttributeValue2 { get; set; }
		public string FunDetail_ElementXPath { get; set; }
		public string FunDetail_SendValue { get; set; }
		public string FunDetail_AfterDoneWaitSeconds { get; set; }
		public string FunDetail_AfterDoneRedirectURL { get; set; }

		//#################### click on a web the same as send text ############################

		//send Key system
		public string FunDetail_SendKeyTimes { get; set; }
		
	}
	
	

	
	}
