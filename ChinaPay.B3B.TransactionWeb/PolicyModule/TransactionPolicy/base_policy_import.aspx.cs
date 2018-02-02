using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.B3B.TransactionWeb.PublicClass;
using ChinaPay.Core.Extension;
using ChinaPay.ExportExcel;
using ChinaPay.B3B.Service.Organization;

namespace ChinaPay.B3B.TransactionWeb.PolicyModule.TransactionPolicy
{

    public partial class base_policy_import : BasePage
    {
        public static DataTable dt;
        public static List<NormalPolicy> normalPolicys;
        public static DataTable errorPolicy;
        public static int error;
        public static int success;
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                this.lnkPolicyTemplate.HRef =
                System.Configuration.ConfigurationManager.AppSettings["FileWeb"] + System.Configuration.ConfigurationManager.AppSettings["BasePolciyTemplate"];
            }
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(fudPath.PostedFile.FileName))
            {
                ShowMessage("请选择需要上传的文件");
            }
            else
            {
                try
                {
                    var savePath = Service.FileService.Upload(fudPath, "BasePolicy", "xlsx?", int.MaxValue);
                    this.lblWarnInfo.Text = "文件已经成功上传至服务器";
                    try
                    {
                        DataSet data = NPOIExcelHelper.ImportDataSetFromExcel(fudPath.PostedFile.InputStream, 0);

                        dt = data.Tables[0];
                        this.dataSource.DataSource = data.Tables[0];
                        this.dataSource.DataBind();
                        this.upload.Visible = false;
                        this.confirm.Style.Add(HtmlTextWriterStyle.Display, "");
                    }
                    catch (Exception ex)
                    {
                        Service.LogService.SaveExceptionLog(ex);
                    }
                }
                catch (Exception ex)
                {
                    this.lblWarnInfo.Style.Add(HtmlTextWriterStyle.Color, "red");
                    this.lblWarnInfo.Text = ex.Message;
                }
            }
        }

        protected void dataSource_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.dataSource.PageIndex = e.NewPageIndex;
            this.dataSource.DataSource = dt;
            this.dataSource.DataBind();
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            this.reCheck.Style.Add(HtmlTextWriterStyle.Display, "");
            this.confirm.Style.Add(HtmlTextWriterStyle.Display, "none");
            TransferToObj(dt);
            var list = from item in normalPolicys
                       select new
                       {
                           //航空公司
                           Airline = item.Airline,
                           //行程类型
                           VoyageType = item.VoyageType.GetDescription(),
                           //出发城市
                           Departure = item.Departure,
                           //到达城市
                           Arrival = item.Arrival,
                           //排除日期
                           DepartureDatesFilter = item.DepartureDateFilter,
                           //班期限制
                           DepartureWeekFilter = StringOperation.TransferToChinese(item.DepartureWeekFilter),
                           //去程适用航班号
                           DepartureInclude = item.DepartureFlightsFilterType == LimitType.None ? "所有" : (item.DepartureFlightsFilterType == LimitType.Include ? item.DepartureFlightsFilter : ""),
                           //去程不适用航班号
                           DepartureExclude = item.DepartureFlightsFilterType == LimitType.Exclude ? item.DepartureFlightsFilter : "",
                           //回程适用航班号
                           ReturnInclude = item.VoyageType == VoyageType.OneWay ? "" : (item.ReturnFlightsFilterType == LimitType.None ? "所有" : (item.ReturnFlightsFilterType == LimitType.Include ? item.ReturnFlightsFilter : "")),
                           //回程不适用航班号
                           ReturnExclude = item.VoyageType == VoyageType.OneWay ? "" : (item.ReturnFlightsFilterType == LimitType.Exclude ? item.ReturnFlightsFilter : ""),
                           //排除航线
                           ExceptAirways = item.ExceptAirways,
                           //适用舱位
                           Berths = item.Berths,
                           //返佣信息
                           Commission = (item.IsInternal ? ("内部:" + (item.InternalCommission * 100).TrimInvaidZero() + "%") : "")
                                + "下级:" + (item.SubordinateCommission * 100).TrimInvaidZero() + "%"
                                + (item.IsPeer ? ("同行:" + (item.ProfessionCommission * 100).TrimInvaidZero() + "%") : ""),
                           //去程日期
                           DepartureDates = item.DepartureDateStart.ToString("yyyy-MM-dd") + "<br />" + item.DepartureDateEnd.ToString("yyyy-MM-dd"),
                           //出票日期
                           ETDZDate = item.StartPrintDate.ToString("yyyy-MM-dd"),
                           //政策类型
                           TicketType = item.TicketType.GetDescription(),
                           //OFFICE号
                           OfficeCode = item.OfficeCode,
                           CustomerCode = item.CustomCode,
                           //是否适用往返降舱
                           SuitReduce = (item.VoyageType == VoyageType.OneWay ? "" : (item.SuitReduce == true ? "是" : "否")),
                           //是否换编码出票
                           ChangePNR = item.ChangePNR == true ? "是" : "否",
                           //审核状态
                           Sudit = item.Audited == true ? "已审" : "未审",
                           //政策备注
                           Remark = item.Remark,
                           //出票条件
                           DrawerCondition = item.DrawerCondition,
                           PrintBeforeTwoHours = item.PrintBeforeTwoHours ? "是" : "否"
                       };

            this.lblError.Text = error.ToString();
            this.lblAvailable.Text = success.ToString();
            this.lblTotal.Text = (error + success).ToString();
            this.checkDataSource.DataSource = list.ToList();
            this.checkDataSource.DataBind();
            if (list.Any())
            {
                this.btnCheck.Visible = true;
            }
            else
            {
                this.btnCheck.Visible = false;
            }
            errorDataSource.DataSource = errorPolicy;
            errorDataSource.DataBind();
        }

        private void TransferToObj(DataTable dt)
        {
            errorPolicy = dt.Clone();
            errorPolicy.Columns.Add("错误信息");
            normalPolicys = new List<NormalPolicy>();
            error = 0;
            success = 0;
            var officeNumbers = CompanyService.QueryOfficeNumbers(this.CurrentCompany.CompanyId).ToList();
            var workSetting = CompanyService.GetWorkingSetting(this.CurrentCompany.CompanyId);
            var customerCodes = CompanyService.GetCustomNumberByEmployee(this.CurrentUser.Id);
            var airlines = PolicySetService.QueryAirlines(this.CurrentCompany.CompanyId);
            var companyParmeter = CompanyService.GetCompanyParameter(this.CurrentCompany.CompanyId);
            foreach (DataRow item in dt.Rows)
            {
                string errorInfo = BasePolicy.valiate(item, officeNumbers, workSetting, customerCodes, airlines, companyParmeter, GetAirport(this.CurrentCompany.CompanyId));
                if (string.IsNullOrWhiteSpace(errorInfo))
                {
                    success++;
                    NormalPolicy normalPolicy = BasePolicy.saveInfo(item, officeNumbers, workSetting, customerCodes, airlines, companyParmeter, GetAirport(this.CurrentCompany.CompanyId));
                    normalPolicys.Add(normalPolicy);
                }
                else
                {
                    error++;
                    DataRow dr = errorPolicy.NewRow();
                    for (var i = 0; i < dt.Columns.Count; i++)
                    {
                        dr[i] = item[i];
                    }

                    dr["错误信息"] = errorInfo;
                    errorPolicy.Rows.Add(dr);
                }
            }
        }

        protected void btnCheck_Click(object sender, EventArgs e)
        {
            try
            {
                PolicyManageService.ReleaseNormalImportPolicies(normalPolicys, this.CurrentCompany.CompanyId, this.CurrentUser.UserName);
                Response.Redirect("base_policy_manage.aspx", false);
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "导入");
            }
        }

        protected void checkDataSource_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.errorList.Style.Add(HtmlTextWriterStyle.Display, "none");
            this.reCheck.Style.Add(HtmlTextWriterStyle.Display, "");
            var list = from item in normalPolicys
                       select new
                       {
                           //航空公司
                           Airline = item.Airline,
                           //行程类型
                           VoyageType = item.VoyageType.GetDescription(),
                           //出发城市
                           Departure = item.Departure,
                           //到达城市
                           Arrival = item.Arrival,
                           //排除日期
                           DepartureDatesFilter = item.DepartureDateFilter,
                           //班期限制
                           DepartureWeekFilter = StringOperation.TransferToChinese(item.DepartureWeekFilter),
                           //去程适用航班号
                           DepartureInclude = item.DepartureFlightsFilterType == LimitType.None ? "所有" : (item.DepartureFlightsFilterType == LimitType.Include ? item.DepartureFlightsFilter : ""),
                           //去程不适用航班号
                           DepartureExclude = item.DepartureFlightsFilterType == LimitType.Exclude ? item.DepartureFlightsFilter : "",
                           //回程适用航班号
                           ReturnInclude = item.VoyageType == VoyageType.OneWay ? "" : (item.ReturnFlightsFilterType == LimitType.None ? "所有" : (item.ReturnFlightsFilterType == LimitType.Include ? item.ReturnFlightsFilter : "")),
                           //回程不适用航班号
                           ReturnExclude = item.VoyageType == VoyageType.OneWay ? "" : (item.ReturnFlightsFilterType == LimitType.Exclude ? item.ReturnFlightsFilter : ""),
                           //排除航线
                           ExceptAirways = item.ExceptAirways,
                           //适用舱位
                           Berths = item.Berths,
                           //返佣信息
                           Commission = (item.IsInternal ? ("内部:" + (item.InternalCommission * 100).TrimInvaidZero() + "%<br />") : "")
                                + "下级:" + (item.SubordinateCommission * 100).TrimInvaidZero() + "%<br />"
                                + (item.IsPeer ? ("同行:" + (item.ProfessionCommission * 100).TrimInvaidZero() + "%") : ""),
                           //去程日期
                           DepartureDates = item.DepartureDateStart.ToString("yyyy-MM-dd") + "<br />" + item.DepartureDateEnd.ToString("yyyy-MM-dd"),
                           //出票日期
                           ETDZDate = item.StartPrintDate.ToString("yyyy-MM-dd"),
                           //政策类型
                           TicketType = item.TicketType.GetDescription(),
                           //OFFICE号
                           OfficeCode = item.OfficeCode,
                           CustomerCode = item.CustomCode,
                           //是否适用往返降舱
                           SuitReduce = (item.VoyageType == VoyageType.OneWay ? "" : (item.SuitReduce == true ? "是" : "否")),
                           //是否换编码出票
                           ChangePNR = item.ChangePNR == true ? "是" : "否",
                           //审核状态
                           Sudit = item.Audited == true ? "已审" : "未审",
                           //政策备注
                           Remark = item.Remark,
                           //出票条件
                           DrawerCondition = item.DrawerCondition,
                           PrintBeforeTwoHours = item.PrintBeforeTwoHours ? "是" : "否"
                       };
            this.checkDataSource.PageIndex = e.NewPageIndex;
            this.checkDataSource.DataSource = list.ToList();
            this.checkDataSource.DataBind();
        }

        protected void errorDataSource_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.errorList.Style.Add(HtmlTextWriterStyle.Display, "");
            this.reCheck.Style.Add(HtmlTextWriterStyle.Display, "none");
            errorDataSource.PageIndex = e.NewPageIndex;
            errorDataSource.DataSource = errorPolicy;
            errorDataSource.DataBind();
        }

        //private string valiateOfficeNo(string officeNo)
        //{
        //    string isValid = "";
        //    var officeNumbers = CompanyService.GetOfficeNumbers(this.CurrentCompany.CompanyId);
        //    if (!officeNumbers.Contains(officeNo))
        //    {
        //        isValid = "";
        //    }
        //    return isValid;
        //}

        private static List<string> GetAirport(Guid id)
        {
            List<string> list = new List<string>();
            var policy = CompanyService.GetPolicySetting(id);
            if (policy != null)
            {
                string[] airports = policy.Departure.Split('/');
                for (int i = 0; i < airports.Length; i++)
                {
                    list.Add(airports[i]);
                }
            }
            return list;
        }
    }
}
