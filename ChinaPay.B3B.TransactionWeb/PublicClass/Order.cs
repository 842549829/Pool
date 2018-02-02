using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.PublicClass
{
    public static class Order
    {
        private const string  _curr = "curr";
        public static void BindOrdersStatus(System.Web.UI.HtmlControls.HtmlGenericControl orderStatusControl, Dictionary<OrderStatus, string> ordersStatus)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
             builder.AppendFormat("<li><a href='#'  class='{0}'>全部</a></li>",_curr);
            foreach (var item in ordersStatus.Values.Distinct())
            {
                builder.AppendFormat("<li><a href='#'>{0}</a></li>", item);
            }
            orderStatusControl.InnerHtml = builder.ToString();
        }
        public static void BindProductType(System.Web.UI.HtmlControls.HtmlGenericControl productTypeControl, IEnumerable<ProductType> productValues)
        {
            System.Text.StringBuilder bulider = new System.Text.StringBuilder();
            bulider.AppendFormat("<li><a href='#' accesskey='' class='{0}'>综合查询</a></li>",_curr);
            foreach (var item in productValues)
	        {
                bulider.AppendFormat("<li><a  href='#' accesskey='{0}' >{1}</a></li>", item.ToString(), item.GetDescription()); 
	        }
            productTypeControl.InnerHtml = bulider.ToString();
        }
    }
}