using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Foundation.Domain;

namespace ChinaPay.B3B.TransactionWeb.About
{
    public partial class ThreeCharCode : UnAuthBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("core.css");
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            RegisterOEMSkins("ticket.css");
            if (!IsPostBack)
            {
                if (HttpContext.Current.Session["CurrentCompany"] != null)
                {
                    hfdIsLogined.Value = "true";
                }
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            threeCodeList.Visible = false;
            string keyWord = txtKeyWord.Text.Trim();
            if (!string.IsNullOrWhiteSpace(keyWord))
            {
                IEnumerable<Airport> airports = from airport in FoundationService.Airports
                                                where airport.Valid && isMatched(airport, keyWord)
                                                select airport;
                var list = from item in airports
                           select new
                               {
                                   Location = item.Location.Name,
                                   item.Code,
                                   item.ShortName,
                                   EnglishName = item.Location.Spelling,
                                   Destination = item.Location.Name + "[" + item.Code + "]",
                                   Departure = item.Location.Name + "[" + item.Code + "]"
                               };
                dataSource.DataSource = list;
                dataSource.DataBind();
                if (list.Count() <= 0)
                {
                    emptyDataInfo.Visible = true;
                }
                else
                {
                    dataSource.HeaderRow.TableSection = TableRowSection.TableHeader;
                    emptyDataInfo.Visible = false;
                }
            }
            else
            {
                lblWarnInfo.Style.Add(HtmlTextWriterStyle.Color, "red");
                lblWarnInfo.Text = "请输入关键字查询!";
            }
        }

        protected void dataSource_DataBound(object sender, EventArgs e)
        {
            if (hfdIsLogined.Value != "true")
            {
                for (int i = 0; i < dataSource.Rows.Count; i++)
                {
                    if (dataSource.Rows[i].FindControl("hlkDeparture") != null && dataSource.Rows[i].FindControl("hlkDestination") != null)
                    {
                        var hlkDeparture = dataSource.Rows[i].FindControl("hlkDeparture") as HyperLink;
                        var hlkDestination = dataSource.Rows[i].FindControl("hlkDestination") as HyperLink;
                        hlkDeparture.NavigateUrl = "~/logon.aspx";
                        hlkDestination.NavigateUrl = "~/logon.aspx";
                    }
                }
            }
        }

        private bool isMatched(Airport airport, string inputContent)
        {
            if (compareMatched(airport.Code.Value, inputContent)
                || compareMatched(airport.ShortName, inputContent)
                || compareMatched(airport.Location.Spelling, inputContent)
                || compareMatched(airport.Location.Name, inputContent))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool compareMatched(string source, string inputContent)
        {
            bool isMatched = false;
            if (source.Length >= inputContent.Length)
            {
                if (source.Substring(0, inputContent.Length).ToUpper() == inputContent.ToUpper())
                {
                    isMatched = true;
                }
                else
                {
                    isMatched = false;
                }
            }
            return isMatched;
        }
    }
}