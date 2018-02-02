using System;
using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.TransactionWeb.PublicClass;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Operate
{
    public partial class UploadAttachment : BasePage
    {
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            decimal attachmentId;
            string id = Request.QueryString["AttachmentId"];
            if (decimal.TryParse(id, out attachmentId))
            {
                try
                {
                    string filePath = FileService.Upload(fileAttachment, "RefundApplyformView");
                    List<ApplyAttachmentView> list = new List<ApplyAttachmentView>();
                    var bytes = ChinaPay.B3B.Service.FileService.GetFileBytes(filePath);
                    Thumbnail thumbnail = new Thumbnail();
                    list.Add(new ApplyAttachmentView
                    {
                        Id = Guid.NewGuid(),
                        ApplyformId = attachmentId,
                        FilePath = filePath,
                        Thumbnail = thumbnail.MakeThumb(100, bytes),
                        Time = DateTime.Now
                    });
                    ApplyformQueryService.AddApplyAttachmentView(list,CurrentUser.UserName);
                    Response.Redirect("ProcessRefund.aspx?id=" + id);
                }
                catch (Exception ex)
                {
                    BasePage.ShowExceptionMessage(this, ex, "上传");
                }
            }
        }
    }
}