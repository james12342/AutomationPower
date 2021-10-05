using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTMLElementSelect.Objects
{
  public  class Object_AutoStepsJson
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class FunctionDetail
        {
            public string FunctionID { get; set; }
            public string FunctionName { get; set; }
            public string FunctionCategoryID { get; set; }
            public string FunctionCategoryName { get; set; }
            public string FunctionExecuteJsonLocation { get; set; }
        }

        public class StepList
        {
          
            public FunctionDetail FunctionDetail { get; set; }
        }

        public class Root
        {
            public int StepID { get; set; }
            public string AutoName { get; set; }
            public string Description { get; set; }
          
            public string StartEndpointURL { get; set; }
            
        }



    }
}
