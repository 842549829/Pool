using System.IO;
using System.Web.Services;
using System;

namespace ChinaPay.B3B.FileWeb
{
    /// <summary>
    /// FileService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class FileService : System.Web.Services.WebService
    {

        [WebMethod]
        public string Upload(byte[] data, string fileName)
        {
            var fileFullName = "/Upload/" + fileName;
            var fileInfo = new FileInfo(Server.MapPath(fileFullName));
            if (!fileInfo.Directory.Exists)
            {
                Directory.CreateDirectory(fileInfo.DirectoryName);
            }
            using (var fileStream = new FileStream(fileInfo.FullName, FileMode.CreateNew))
            {
                fileStream.Write(data, 0, data.Length);
            }
            return fileFullName;
        }
        [WebMethod]
        public string GetFileBase64String(string fileFullName)
        {
            var path = Server.MapPath("/" + fileFullName);
            string file = "";
            if (File.Exists(path))
            {
                file = Convert.ToBase64String(File.ReadAllBytes(path));
            }
            return file;
        }
    }
}
