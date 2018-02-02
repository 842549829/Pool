using System;

namespace ChinaPay.B3B.DataTransferObject.FlightQuery {
    public class PolicyView {
        /// <summary>
        /// 政策编号
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 政策类型
        /// </summary>
        public B3B.Common.Enums.PolicyType Type { get; set; }
        /// <summary>
        /// 政策发布者
        /// </summary>
        public Guid Owner { get; set; }
        /// <summary>
        /// 客户自已管理资源
        /// </summary>
        public bool CustomerResource { get; set; }
        /// <summary>
        /// 发布价格
        /// </summary>
        public decimal PublishFare { get; set; }

        public static PolicyView Parse(string args) {
            PolicyView result = null;
            var policyItemArray = args.Split('|');
            if(policyItemArray.Length == 5) {
                result = new PolicyView {
                    Id = Guid.Parse(policyItemArray[0]),
                    Owner = Guid.Parse(policyItemArray[1]),
                    Type = (B3B.Common.Enums.PolicyType)int.Parse(policyItemArray[2]),
                    CustomerResource = bool.Parse(policyItemArray[3]),
                    PublishFare = decimal.Parse(policyItemArray[4])
                };
            }
            return result;
        }
    }
}
