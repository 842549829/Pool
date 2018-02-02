using System;
using System.Web.UI;
using ChinaPay.B3B.Service.Integral;
using ChinaPay.B3B.DataTransferObject.Integral;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.IntegralCommodity
{
    public partial class IntegralParameter : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");

            if (!IsPostBack)
            {
                NewMethod();
            }

        }

        private void NewMethod()
        {

            IntegralParameterView view = IntegralServer.GetIntegralParameter();
            if (view != null)
            {
                radEnableGongNeng.Checked = view.IsSignIn;
                radDisableGongNeng.Checked = !view.IsSignIn;
                radEnableXiaJia.Checked = view.IsDrop;
                radDisableXiaJia.Checked = !view.IsDrop;
                txtKeYong.Text = (view.AvailabilityRatio * 100).TrimInvaidZero();
                txtSheZhi.Text = view.SignIntegral + "";
                txtXiaoFei.Text = view.ConsumptionIntegral + "";
                txtMostBuckle.Text = view.MostBuckle + "";
                txtJianLi.Text = view.Multiple.TrimInvaidZero();
                selZhouQi.SelectedIndex = (int)view.RangeReset;
                txtZhiDing.Text = view.SpecifiedDate == null ? DateTime.Today.AddYears(1).ToString("yyyy-MM-dd") : view.SpecifiedDate.Value.ToString("yyyy-MM-dd");
                txtZhiDing.Style.Add(HtmlTextWriterStyle.Display, view.SpecifiedDate != null ? "" : "none");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                IntegralParameterView view = new IntegralParameterView
                {
                    IsSignIn = this.radEnableGongNeng.Checked,
                    IsDrop = this.radEnableXiaJia.Checked,
                    AvailabilityRatio = decimal.Parse(this.txtKeYong.Text) / 100,
                    SignIntegral = int.Parse(this.txtSheZhi.Text),
                    ConsumptionIntegral = int.Parse(this.txtXiaoFei.Text),
                    Multiple = decimal.Parse(this.txtJianLi.Text),
                    MostBuckle = int.Parse(txtMostBuckle.Text),
                    SpecifiedDate = this.selZhouQi.SelectedIndex != 7 ? (DateTime?)null : DateTime.Parse(txtZhiDing.Text),
                    RangeReset = (Common.Enums.IntegralRangeTime)this.selZhouQi.SelectedIndex
                };
                IntegralServer.UpdateIntegralParameter(view);
                NewMethod();
                ShowMessage("保存成功");
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "保存");
            }

        }
    }
}