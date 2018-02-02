using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace ChinaPay.B3B.TransactionWeb.OrganizationHandlers
{
    /// <summary>
    /// FileProxy 的摘要说明
    /// </summary>
    public class FileProxy : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            var fileName = context.Request.QueryString["fileName"];
            var fileContent  = ReadFile(context,fileName);
            if (!string.IsNullOrEmpty(fileContent))
            {
                context.Response.ContentType = "text/css";
                context.Response.Write(fileContent);
            }
        }
        static object locker = new object();
        private string ReadFile(HttpContext context,string fileName)
         {
             var runDirectory = context.Server.MapPath("/");
             var oem = UnAuthBasePage.OEM;
             var skinPath = oem!=null&&oem.OEMStyle != null ? oem.OEMStyle.TemplatePath : "/Styles/skins/Default/";
            
             string result;
            string path = runDirectory + skinPath + fileName+".css";
            if (!File.Exists(path))
            {
                return string.Empty;
            }
            lock (locker)
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        result = sr.ReadToEnd();
                    }
                }  
            }
             return result;  
         }  



        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}