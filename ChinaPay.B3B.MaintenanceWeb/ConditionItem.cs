using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.MaintenanceWeb
{
    public class ConditionItem
    {
        public string ControlId{get;set;}
        public int ConditionType{get;set;}
        public string ConditionValue{get;set;}
        public ConditionItem(string controlId, string content)
        {
            this.ControlId = controlId;
            string[] itemContent = content.Split('|');
            if (itemContent.Length != 2)
            {
                this.ConditionType = 1;
                this.ConditionValue = string.Empty;
            }
            else
            {
                this.ConditionType = int.Parse(itemContent[0]);
                this.ConditionValue = itemContent[1];
            }
        }
    }
}
