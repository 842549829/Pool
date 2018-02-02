using System;

namespace ChinaPay.B3B.DataTransferObject.Foundation
{
    /// <summary>
    /// ���չ�˾�˹�ͳ����Ϣ
    /// </summary>
    public class AirlineRulesView
    {
        readonly Func<string, string> getArilineName;
        public string AirlineCode { get; set; }
        public string AirlineName
        {
            get;
            set;
        }
        public int RulesCount { get; set; }

        /// <summary>
        /// �Ƿ����������
        /// </summary>
        public bool HasRules { get; set; }
    }
}