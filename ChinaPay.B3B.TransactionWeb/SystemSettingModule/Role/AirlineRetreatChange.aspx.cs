using System;
using System.Linq;
using System.Web.UI;
using ChinaPay.B3B.Service;

namespace ChinaPay.B3B.TransactionWeb.SystemSettingModule.Role
{
    public partial class AirlineRetreatChange : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                BindAirline();
            }
        }
        private void BindAirline()
        {
            hdfAirlineCode.Value =  Request.QueryString["Carrier"];
            var airline_list = FoundationService.RefundAndReschedulings.OrderBy(item => item.Level);
            airlines.InnerHtml = "";

            foreach (var item in airline_list)
            {
                if (airlines.InnerHtml == "")
                {
                    airlines.InnerHtml += "<a href='javascript:;' class='cur' value='" + item.AirlineCode.Value + "'>" + item.Airline.ShortName + "</a>";
                    //加载第一条数据作为默认显示
                    table_body.InnerHtml += "<tr id='" + item.AirlineCode.Value + "' class='table_tr'> <td class='box'> " + item.Airline.ShortName + "<br />联系电话：" + item.AirlineTel + " </td> <td class='box'>" + item.Refund + " </td>  <td class='box'>" + item.Scrap + "  </td> <td class='box'>" + item.Change + " </td> <td class='box'>" + item.Remark + "</td></tr>";
                }
                else
                {
                    airlines.InnerHtml += "<a href='javascript:;' value='" + item.AirlineCode.Value + "'>" + item.Airline.ShortName + "</a>";

                }
            }
            if (airline_list.Count() > 10)
            {
                string tr_count_height = ((Math.Floor((double)(airline_list.Count() / 10)) * 30) + 50) + "px";
                airlines_div.Style.Add(HtmlTextWriterStyle.Height, tr_count_height);
            }
        }
    }
}