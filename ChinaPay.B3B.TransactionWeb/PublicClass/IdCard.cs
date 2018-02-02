using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChinaPay.IdentityCard;

namespace ChinaPay.B3B.TransactionWeb.PublicClass
{
    public static class IdCard
    {
        /// <summary>
        /// 验证身份证
        /// </summary>
        public static void CheckIdentityCard(string identitycard) {
            Validator validator = new ChinaPay.IdentityCard.Validator(identitycard);
            if (!validator.Execute())
            {
                throw new ChinaPay.Core.Exception.InvalidValueException(validator.ErrorMessage);
            }
        }
        /// <summary>
        /// 截取身份证后六位用作支付密码
        /// </summary>
        public static string GetPayPassword(string strIdCard)
        {
            if (strIdCard.Length == 18)
            {
                return strIdCard.Substring(12);
            }
            else
            {
                return strIdCard.Substring(9);
            }
        }
    }
}