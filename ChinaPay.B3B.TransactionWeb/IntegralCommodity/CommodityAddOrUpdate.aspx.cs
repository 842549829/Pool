using System;
using System.IO;
using System.Web.UI;
using ChinaPay.B3B.DataTransferObject.Commodity;
using ChinaPay.B3B.Service.Commodity;
using ChinaPay.B3B.TransactionWeb.PublicClass;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.TransactionWeb.IntegralCommodity
{
    public partial class CommodityAddOrUpdate : BasePage
    {
        private static string FileWeb = System.Configuration.ConfigurationManager.AppSettings["FileWeb"];
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");

            if (!IsPostBack)
            {
                title.InnerText = "添加商品";
                txtTime.Text = DateTime.Today.AddMonths(3).ToString("yyyy-MM-dd");
                if (Request.QueryString["id"] != null)
                {
                    CommodityView view = CommodityServer.GetCommodity(Guid.Parse(Request.QueryString["id"]));
                    if (view != null)
                    {
                        btnAdd.Text = "修改";
                        OldImgUrlDiv.Visible = true;
                        OldImgUrl.Text = FileWeb + view.CoverImgUrl;
                        OldImgUrl.Style.Add(HtmlTextWriterStyle.Display, "none");
                        title.InnerText = "修改商品";
                        txtTime.Text = view.ValidityTime.ToString("yyyy-MM-dd");
                        txtCoommodityName.Text = view.CommodityName;
                        txtNeedIntegral.Text = view.NeedIntegral + "";
                        txtRemark.Text = view.Remark;
                        txtSort.Text = view.SortNum + "";
                        ddlType.SelectedValue = ((int)view.Type).ToString();
                        txtNum.Text = view.ExchangSmsNumber.ToString();
                        lblName.Text = view.CommodityName;
                        hidName.Value = view.CommodityName;
                    }
                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (!IsVaild()) return;
            string msg = "添加";
            try
            {
                string path = fileImg.HasFile ? Service.FileService.Upload(fileImg, "Commodity", "(jpg)|(gif)|(png)|(jpeg)", int.MaxValue) : OldImgUrl.Text.Replace(FileWeb, "");
                CommodityView view = new CommodityView
                                         {
                                             ID = Guid.NewGuid(),
                                             CommodityName = ddlType.SelectedValue == "1" ? txtCoommodityName.Text : hidName.Value,
                                             NeedIntegral = int.Parse(txtNeedIntegral.Text),
                                             SortNum = int.Parse(txtSort.Text),
                                             ExchangeNumber = 0,
                                             State = radEnable.Checked,
                                             StockNumber = 0,
                                             SurplusNumber = 0,
                                             ValidityTime = DateTime.Parse(txtTime.Text),
                                             Remark = txtRemark.Text,
                                             ExchangSmsNumber = ddlType.SelectedValue == "2" ? int.Parse(txtNum.Text) : 0,
                                             Type = (CommodityType)int.Parse(ddlType.SelectedValue)
                                         };
                if (Request.QueryString["id"] != null)
                {
                    CommodityView views = CommodityServer.GetCommodity(Guid.Parse(Request.QueryString["id"]));
                    view.ExchangeNumber = views.ExchangeNumber;
                    view.StockNumber = views.StockNumber;
                    view.SurplusNumber = views.SurplusNumber;
                    msg = "修改";
                    view.ID = Guid.Parse((Request.QueryString["id"]));
                    view.CoverImgUrl = path;
                    CommodityServer.UpdateCommodity(view, CurrentUser.UserName);
                }
                else
                {
                    view.CoverImgUrl = path;
                    CommodityServer.InsertCommodity(view, this.CurrentUser.UserName);
                }
                RegisterScript(Page, "alert('" + msg + "成功');window.location.href='./CommodityList.aspx';");
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, msg);
            }
        }
        private bool IsVaild()
        {
            if (ddlType.SelectedValue == "1" && txtCoommodityName.Text.Trim() == "")
            {
                ShowMessage("请先填写商品名称");
                return false;
            }
            if (ddlType.SelectedValue == "2" && txtNeedIntegral.Text.Trim() == "")
            {
                ShowMessage("请先填写商品所需积分");
                return false;
            }
            if (OldImgUrl.Text == "")
            {
                if (!fileImg.HasFile)
                {
                    ShowMessage("请先选择商品图片");
                    return false;
                }
                string str = fileImg.FileName.Substring(fileImg.FileName.LastIndexOf('.') + 1).ToUpper();
                if (str != "JPG" && str != "JPEG" && str != "PNG" && str != "GIF")
                {
                    ShowMessage("上传图片格式必须是jpg,jpeg,png,gif");
                    return false;
                }
            }

            return true;
        }
    }
}