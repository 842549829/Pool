using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using ChinaPay.Core;

namespace ChinaPay.B3B.Service
{
    public class FileService
    {
        private static string FileWeb = System.Configuration.ConfigurationManager.AppSettings["FileWeb"];

        public static string Upload(Stream stream, string catelog, string fileName)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            if (string.IsNullOrWhiteSpace(catelog)) throw new ArgumentNullException("catelog");
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentNullException("fileName");
            return Upload(stream, GetFileName(catelog, Path.GetExtension(fileName)));
        }
        public static string Upload(System.Web.UI.WebControls.FileUpload uploadControl, string catelog)
        {
            return Upload(uploadControl, catelog, null, int.MaxValue);
        }
        public static string Upload(System.Web.UI.WebControls.FileUpload uploadControl, string catelog, string fileExtentionPattern, int fileMaxLength)
        {
            if (uploadControl == null) throw new ArgumentNullException("uploadControl");
            if (string.IsNullOrWhiteSpace(catelog)) throw new ArgumentNullException("catelog");
            if (!uploadControl.HasFile) throw new CustomException("缺少文件");
            var fileExtension = Path.GetExtension(uploadControl.FileName);
            if (!string.IsNullOrWhiteSpace(fileExtentionPattern))
            {
                // 验证文件类型
                if (!Regex.IsMatch(fileExtension, "^." + fileExtentionPattern + "$", RegexOptions.IgnoreCase))
                    throw new CustomException("请选择符合要求的文件类型");
            }
            // 验证文件大小
            if (uploadControl.PostedFile.ContentLength > fileMaxLength) throw new CustomException("文件超出指定大小");
            return Upload(uploadControl.PostedFile.InputStream, GetFileName(catelog, fileExtension));
        }
        private static string Upload(Stream stream, string fileName)
        {
            if (stream.Length == 0) throw new CustomException("文件大小为零");
            var data = new byte[stream.Length];
            stream.Read(data, 0, data.Length);
            var fileService = new File.WebReference.FileService()
            {
                Url = FileWeb + "/FileService.asmx"
            };
            var fileNameOnServer = fileService.Upload(data, fileName);
            return fileNameOnServer;
        }
        private static string GetFileName(string catelog, string fileExtention)
        {
            return string.Format("{0}/{1}/{2}{3}", catelog, DateTime.Today.ToString("yyyyMMdd"), Path.GetRandomFileName(), fileExtention);
        }
        /// <summary>
        /// 根据文件相对路径获取到文件流
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static byte[] GetFileBytes(string filePath) 
        {
            var fileService = new File.WebReference.FileService();
            var b = fileService.GetFileBase64String(filePath);
            return b ==""? new byte[0]: Convert.FromBase64String(b);
        }
    }
}