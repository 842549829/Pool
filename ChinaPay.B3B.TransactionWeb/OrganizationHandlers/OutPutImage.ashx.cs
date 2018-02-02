using System;
using System.Web;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Organization.Domain;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service;

namespace ChinaPay.B3B.TransactionWeb.OrganizationHandlers
{
    /// <summary>
    /// OutPutImage 的摘要说明
    /// </summary>
    public class OutPutImage : IHttpHandler
    {
        public void OutPutStram(string companyId)
        {
            string type = HttpContext.Current.Request.QueryString["type"];
            HttpContext.Current.Response.ContentType = "image/jpeg";
            CompanyDocument companyDocument = null;
            if (!string.IsNullOrWhiteSpace(companyId))
            {
                companyDocument = AccountCombineService.QueryCompanyDocument(Guid.Parse(companyId));
            }
            if (companyDocument != null)
            {
                if (type == "bussiness" && companyDocument.BussinessLicense != null && companyDocument.BussinessLicense.Length > 0)
                {
                    HttpContext.Current.Response.BinaryWrite(companyDocument.BussinessLicense);
                }
                if (type == "certNo" && companyDocument.CertLicense != null && companyDocument.CertLicense.Length > 0)
                {
                    HttpContext.Current.Response.BinaryWrite(companyDocument.CertLicense);
                }
                if (type == "iata" && companyDocument.IATALicense != null && companyDocument.IATALicense.Length > 0)
                {
                    HttpContext.Current.Response.BinaryWrite(companyDocument.IATALicense);
                }
            }
        }
        public void OutPutApplyAttachment() {
            string applyAttachmentId = HttpContext.Current.Request.QueryString["ApplyAttachmentId"];
            if (!string.IsNullOrEmpty(applyAttachmentId))
            {
                Guid id = Guid.Parse(applyAttachmentId);
                ApplyAttachmentView apply = ApplyformQueryService.QueryApplyAttachmentView(id);
                if (apply != null)
                {
                    HttpContext.Current.Response.BinaryWrite(apply.Thumbnail);
                }
            }
        }
        public void ProcessRequest(HttpContext context)
        {
            string companyId = HttpContext.Current.Request.QueryString["companyId"];
            if (!string.IsNullOrEmpty(companyId))
            {
                OutPutStram(companyId);
            }
            else {
                OutPutApplyAttachment();
            }
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