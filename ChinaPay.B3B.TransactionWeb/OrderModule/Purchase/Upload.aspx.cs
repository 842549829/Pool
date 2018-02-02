using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using ChinaPay.B3B.Service;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Purchase
{
    public partial class Upload : System.Web.UI.Page
    {
        protected void btnUploadFile_Click(object sender, EventArgs e)
        {
            if (Request.Files.Count > 0)
            {
                HttpPostedFile file = Request.Files[0];
                if (validation(file))
                {
                    hfdFilePath.Value = FileService.Upload(file.InputStream, "RefundApplyformView", file.FileName);
                }
                else {
                    BasePage.RegisterJavaScript(this, "window.onload = function(){window.parent.document.getElementById('txtAttachment').value = '';}");
                }
            }
        }
        private bool validation(HttpPostedFile file) 
        {
            if (file.ContentLength > 1024 * 600)
            {
                BasePage.ShowMessage(this, "文件大小不能超过600KB");
                return false;
            }
            int point =file.FileName.LastIndexOf('.');
            if (point == -1)
            {
                BasePage.ShowMessage(this, "文件格式不正确");
                return false;
            }
            string filePathExtension =file.FileName.Substring(point).ToLower();
            if (filePathExtension != ".jpg" && filePathExtension != ".png" && filePathExtension != ".bmp")
            {
                BasePage.ShowMessage(this, @"文件格式不正确只支持jpg\png\bmp的图片");
                return false;
            }
            return true;
        }
    }
}