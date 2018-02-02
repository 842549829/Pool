using System;
using System.IO;
using System.Text.RegularExpressions;
using ChinaPay.Core;

namespace ChinaPay.B3B.TransactionWeb.PublicClass {
    public class UploadHelper {
        private static string UploadPath = System.Configuration.ConfigurationManager.AppSettings["Upload"];

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileFullName">客户端文件全名</param>
        /// <param name="catelog">文件种类</param>
        /// <param name="extentionPattern">文件类型</param>
        /// <returns>服务器的文件全名</returns>
        public static string GenerateUploadFileName(string fileFullName, string catelog, string extentionPattern) {
            if(string.IsNullOrWhiteSpace(fileFullName)) throw new ArgumentNullException("fileFullName");
            if(string.IsNullOrWhiteSpace(catelog)) throw new ArgumentNullException("catelog");

            // 服务器上创建相应的文件夹
            var directoryName = Path.Combine(UploadPath, catelog, DateTime.Today.ToString("yyyyMMdd"));
            if(!Directory.Exists(directoryName)) {
                Directory.CreateDirectory(directoryName);
            }

            var fileExtension = Path.GetExtension(fileFullName);
            if(!Regex.IsMatch(fileExtension, "^." + extentionPattern + "$", RegexOptions.IgnoreCase)) throw new CustomException("请选择符合要求的文件类型");
            var fileNameOnServer = Path.GetRandomFileName() + "." + fileExtension;
            return Path.Combine(directoryName, fileNameOnServer);
        }
    }
}