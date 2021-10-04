using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLElementSelect.Objects
{
    public class Object_AutomationInfo
    {
        public int Id { get; set; }
        public string AutoName { get; set; }
        public string Description { get; set; }
        public string AutoStepFunctionLocation { get; set; }
        public bool IsActive { get; set; }
        public string Run_ResultDataFileName { get; set; }
        public string Run_LastRuntime { get; set; }
        public int Run_Step { get; set; }
        public int MaxSteps { get; set; }

        

    }
}
