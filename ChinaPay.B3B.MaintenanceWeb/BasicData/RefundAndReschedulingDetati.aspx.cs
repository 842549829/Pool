using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service.Foundation.Domain;
using ChinaPay.B3B.Service;

namespace ChinaPay.B3B.MaintenanceWeb.BasicData
{
    public partial class RefundAndReschedulingDetati : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                setButton();
                var data = FoundationService.QueryDetailList(Request.QueryString["airline"]);
                this.gvRefundChangeTecket.DataSource = data;
                this.gvRefundChangeTecket.DataBind();
                if (data.Any())
                {
                    dataList.Visible = true;
                }
                else
                {
                    dataList.Visible = false;
                }
                var detailId = Request.QueryString["Id"];
                if (!string.IsNullOrWhiteSpace(detailId))
                {
                    var detail = FoundationService.QueryRefundAndReschedulingNewDetail(Guid.Parse(detailId));
                    if (detail != null)
                    {
                        if (!string.IsNullOrWhiteSpace(detail.Bunks))
                            this.txtBunks.Text = detail.Bunks;
                        if (!string.IsNullOrWhiteSpace(detail.Endorse))
                            this.txtEndorse.InnerText = detail.Endorse;
                        if (detail.ScrapAfter == detail.ScrapBefore)
                        {
                            this.hfdScrap.Value = "merge";
                            this.txtScrap.InnerText = detail.ScrapBefore;
                        }
                        else
                        {
                            this.hfdScrap.Value = "split";
                            this.txtScrapBefore.InnerText = detail.ScrapBefore;
                            this.txtScrapAfter.InnerText = detail.ScrapAfter;
                        }
                        if (detail.ChangeBefore == detail.ChangeAfter)
                        {
                            this.hfdChange.Value = "merge";
                            this.txtChange.InnerText = detail.ChangeBefore;
                        }
                        else
                        {
                            this.hfdChange.Value = "split";
                            this.txtChangeBefore.InnerText = detail.ChangeBefore;
                            this.txtChangeAfter.InnerText = detail.ChangeAfter;
                        }
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
             if (Request.QueryString["action"] != null)
            {
                
                if (Request.QueryString["action"].ToString() == "add")
                {
                    RefundAndReschedulingDetail detail = new RefundAndReschedulingDetail()
                    {
                        Bunks = this.txtBunks.Text.Trim(),
                        Endorse = this.txtEndorse.InnerText.Trim()
                    };
                    if (this.hfdScrap.Value == "split")
                    {
                        detail.ScrapBefore = this.txtScrapBefore.InnerText.Trim();
                        detail.ScrapAfter = this.txtScrapAfter.InnerText.Trim();
                    }
                    else
                    {
                        detail.ScrapBefore = detail.ScrapAfter = this.txtScrap.InnerText.Trim();
                    }
                    if (this.hfdChange.Value == "split")
                    {
                        detail.ChangeBefore = this.txtChangeBefore.InnerText.Trim();
                        detail.ChangeAfter = this.txtChangeAfter.InnerText.Trim();
                    }
                    else
                    {
                        detail.ChangeBefore = detail.ChangeAfter = this.txtChange.InnerText.Trim();
                    }
                    detail.Airline = Request.QueryString["airline"];
                    try
                    {
                        FoundationService.AddRefundAndReschedulingNewDetail(detail, CurrentUser.UserName);
                        RegisterScript("alert('添加成功！'); window.location.href='RefundChangeRuleDetail.aspx?Code=" + Request.QueryString["airline"] + "'");
                    } catch(Exception ex) {
                        ShowExceptionMessage(ex, "添加");
                    }
                }
                else
                {
                    RefundAndReschedulingDetail detail = new RefundAndReschedulingDetail(Guid.Parse(Request.QueryString["Id"]))
                    {
                        Bunks = this.txtBunks.Text.Trim(),
                        Endorse = this.txtEndorse.InnerText.Trim()
                    };
                    if (this.hfdScrap.Value == "split")
                    {
                        detail.ScrapBefore = this.txtScrapBefore.InnerText.Trim();
                        detail.ScrapAfter = this.txtScrapAfter.InnerText.Trim();
                    }
                    else
                    {
                        detail.ScrapBefore = detail.ScrapAfter = this.txtScrap.InnerText.Trim();
                    }
                    if (this.hfdChange.Value == "split")
                    {
                        detail.ChangeBefore = this.txtChangeBefore.InnerText.Trim();
                        detail.ChangeAfter = this.txtChangeAfter.InnerText.Trim();
                    }
                    else
                    {
                        detail.ChangeBefore = detail.ChangeAfter = this.txtChange.InnerText.Trim();
                    }
                    detail.Airline = Request.QueryString["airline"];
                    try
                    {
                        FoundationService.UpdateRefundAndReschedulingNewDetail(detail, CurrentUser.UserName);
                        RegisterScript("alert('修改成功！'); window.location.href='RefundChangeRuleDetail.aspx?Code=" + Request.QueryString["airline"] + "'");
                    } catch(Exception ex) {
                        ShowExceptionMessage(ex, "修改");
                    }
                }
            }
        }

        private void setButton()
        {
            this.btnBack.Attributes.Add("onclick", "window.location.href='RefundChangeRuleDetail.aspx?Code=" + Request.QueryString["airline"] + "';return false;");
           
        }
    }
}