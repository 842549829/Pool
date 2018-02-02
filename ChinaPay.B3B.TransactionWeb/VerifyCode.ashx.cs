using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ChinaPay.Utility;

namespace ChinaPay.B3B.TransactionWeb
{
    /// <summary>
    /// 验证码显示
    /// </summary>
    public class VerifyCode : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context) {
            string para = context.Request.QueryString["verifyType"];
            string code = VerifyCodeUtility.CreateVerifyCode(5);
            VerifyCodeService v = new VerifyCodeService() { 
                 Length = 5,
                 FontSize = 15,
            };
            v.CreateImageOnPage(code, context);
            context.Session[para] = code;
        }
        public bool IsReusable { get { return true; } }
    }
}