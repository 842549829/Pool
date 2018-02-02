namespace ChinaPay.B3B.TransactionWeb.OrderModule.Provide
{
    public class ConditionItem
    {
        public string ControlId
        {
            get;
            set;
        }
        public int ConditionType
        {
            get;
            set;
        }
        public string ConditionValue
        {
            get;
            set;
        }

        public ConditionItem(string controlId, string content)
        {
            ControlId = controlId;
            var itemContent = content.Split('|');
            if (itemContent.Length != 2)
            {
                ConditionType = 1;
                ConditionValue = string.Empty;
            }
            else
            {
                ConditionType = int.Parse(itemContent[0]);
                ConditionValue = itemContent[1];
            }
        }
    }
}