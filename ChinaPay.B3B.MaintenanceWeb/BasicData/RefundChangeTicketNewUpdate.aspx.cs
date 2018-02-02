using System;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Foundation;
using ChinaPay.B3B.Service;

namespace ChinaPay.B3B.MaintenanceWeb.BasicData
{
    public partial class RefundChangeTicketNewUpdate : BasePage
    {
        #region 数据加载
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DropList();
                setButton();
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
            ChinaPay.B3B.Service.Foundation.Domain.RefundAndReschedulingBase refundAndRescheduling = FoundationService.QueryRefundAndReschedulingNew(new Core.UpperString(code));
            if (refundAndRescheduling == null) return;
            this.ddlAirline.SelectedValue = refundAndRescheduling.AirlineCode.Value;
            if (refundAndRescheduling.AirlineTel != null) this.txtPhone.Text = refundAndRescheduling.AirlineTel;
            if (refundAndRescheduling.Condition != null) this.txtCondition.Text = refundAndRescheduling.Condition;
            if (refundAndRescheduling.Scrap != null) this.txtScrap.Text = refundAndRescheduling.Scrap;
            if (refundAndRescheduling.Upgrade != null) this.txtUpgrade.InnerText = refundAndRescheduling.Upgrade;
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
                RefundAndReschedulingBaseView refundAndReschedulingView = new RefundAndReschedulingBaseView()
                {
                    Airline = this.ddlAirline.SelectedValue,
                    AirlineTel = this.txtPhone.Text.Trim(),
                    Upgrade = this.txtUpgrade.InnerText.Trim(),
                    Scrap = this.txtScrap.Text.Trim(),
                    Condition = this.txtCondition.Text.Trim(),
                    Remark = this.txtRemark.InnerText.Trim(),
                    Level = Convert.ToInt32(this.txtOrderby.Text.Trim())

                };
                if (Request.QueryString["action"].ToString() == "add")
                {
                    try
                    {
                        FoundationService.AddRefundAndReschedulingNewBase(refundAndReschedulingView, CurrentUser.UserName);
                            RegisterScript("alert('添加成功！'); window.location.href='RefundChangeRuleList.aspx'");
                    }
                    catch (Exception ex)
                    {
                        ShowExceptionMessage(ex, "添加");
                    }
                }
                else
                {
                    try
                    {
                        FoundationService.UpdateRefundAndReschedulingNewBase(refundAndReschedulingView, CurrentUser.UserName);
                        RegisterScript("alert('修改成功！'); window.location.href='RefundChangeRuleDetail.aspx?Code=" + Request.QueryString["code"] + "'");
                    }
                    catch (Exception ex)
                    {
                        ShowExceptionMessage(ex, "修改");
                    }
                }
            }
        }
        #endregion

        private void setButton()
        {
            if (Request.QueryString["action"] == "add")
            {
                this.btnBack.Attributes.Add("onclick", "window.location.href='RefundChangeRuleList.aspx';return false;");
            }
            else
            {
                this.btnBack.Attributes.Add("onclick", "window.location.href='RefundChangeRuleDetail.aspx?Code=" + Request.QueryString["code"] + "';return false;");
            }
        }
    }
}
