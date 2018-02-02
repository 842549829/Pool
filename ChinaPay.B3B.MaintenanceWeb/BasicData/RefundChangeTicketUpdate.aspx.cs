using System;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Foundation;
using ChinaPay.B3B.Service;

namespace ChinaPay.B3B.MaintenanceWeb.BasicData
{
    public partial class RefundChangeTicketUpdate : BasePage
    {
        #region 数据加载
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DropList();
                if (Request.QueryString["action"] != null)
                {
                    if (Request.QueryString["action"].ToString() == "update")
                    {
                        Refresh(Request.QueryString["code"].ToString());
                        this.ddlAirline.Enabled = false;
                    }

                }
            }
        }
        private void Refresh(string code)
        {
            ChinaPay.B3B.Service.Foundation.Domain.RefundAndRescheduling refundAndRescheduling = FoundationService.QueryRefundAndRescheduling(code);
            if(refundAndRescheduling ==null) return;
            this.ddlAirline.SelectedValue = refundAndRescheduling.AirlineCode.Value;
            if (refundAndRescheduling.AirlineTel != null) this.txtPhone.Text = refundAndRescheduling.AirlineTel;
            if (refundAndRescheduling.Refund != null) this.txtRefund.InnerText = refundAndRescheduling.Refund;
            if (refundAndRescheduling.Scrap != null) this.txtScrap.InnerText = refundAndRescheduling.Scrap;
            if (refundAndRescheduling.Change != null) this.txtChange.InnerText = refundAndRescheduling.Change;
            if (refundAndRescheduling.Remark != null) this.txtRemark.InnerText = refundAndRescheduling.Remark;
            this.txtOrderby.Text = refundAndRescheduling.Level.ToString();
        }
        private void DropList()
        {
            foreach (var item in FoundationService.Airlines)
            {
                this.ddlAirline.Items.Add(new ListItem(item.Code.Value + "-" + item.ShortName, item.Code.Value));
            }
            this.ddlAirline.Items.Insert(0, new ListItem("-所有-", "-所有-"));
        }
        #endregion

        #region 保存
        protected void btnSave_Click(object sender, EventArgs e)
        {
           if (Request.QueryString["action"] != null)
            {
                RefundAndReschedulingView refundAndReschedulingView = new RefundAndReschedulingView()
                {
                    Airline = this.ddlAirline.SelectedValue,
                    AirlineTel = this.txtPhone.Text.Trim(),
                    Refund = this.txtRefund.InnerText.Trim(),
                    Scrap = this.txtScrap.InnerText.Trim(),
                    Change = this.txtChange.InnerText.Trim(),
                    Remark = this.txtRemark.InnerText.Trim(),
                    Level = Convert.ToInt32(this.txtOrderby.Text.Trim())

                };
                if (Request.QueryString["action"].ToString() == "add")
                {
                    try
                    {
                        FoundationService.AddRefundAndRescheduling(refundAndReschedulingView, CurrentUser.UserName);
                        RegisterScript("alert('添加成功！'); window.location.href='RefundChangeTicket.aspx'");
                    } catch(Exception ex) {
                        ShowExceptionMessage(ex, "添加");
                    }
                }
                else
                {
                    try
                    {
                        FoundationService.UpdateRefundAndRescheduling(refundAndReschedulingView, CurrentUser.UserName);
                        RegisterScript("alert('修改成功！'); window.location.href='RefundChangeTicket.aspx'");
                    } catch(Exception ex) {
                        ShowExceptionMessage(ex, "修改");
                    }
                }
            }
        } 
        #endregion
    } 
}
