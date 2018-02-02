<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProviderStatisticsReport.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.ReportModule.ProviderStatisticsReport" %>

<%@ Register Src="~/UserControl/Airport.ascx" TagName="City" TagPrefix="uc" %>
<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<%@ Register TagPrefix="uc1" TagName="Company_1" Src="~/UserControl/CompanyC.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>平台采购量统计</title>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="query" runat="server" autocomplete="off">
    <h3 class="titleBg">
        平台出票量统计</h3>
    <div class="box-a">
        <div class="condition">
            <table>
                <colgroup>
                    <col class="w35" />
                    <col class="w35" />
                    <col class="w35" />
                </colgroup>
                <tr>
                    <td>
                        <div class="input">
                            <span class="name">日期：</span>
                            <asp:TextBox runat="server" ID="txtStartDate" class="text text-s1" onfocus=" WdatePicker({ skin: 'default', isShowClear: false, maxDate: '#F{$dp.$D(\'txtEndDate\')||\'2020-10-01\'}' })">
                            </asp:TextBox>
                            -
                            <asp:TextBox runat="server" ID="txtEndDate" class="text text-s1" onfocus="WdatePicker({ skin: 'default', isShowClear: false, minDate: '#F{$dp.$D(\'txtStartDate\')}', maxDate:'%y-%M-%d' })">
                            </asp:TextBox><asp:HiddenField runat="server" ID="hdDefaultDate" />
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">出发城市：</span>
                            <uc:City ID="txtDeparture" runat="server" />
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">航空公司：</span>
                            <asp:DropDownList ID="ddlAirlines" runat="server" CssClass="selectarea">
                            </asp:DropDownList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="input" style="z-index:4;" >
                            <span class="name">出票方：</span>
                            <uc1:Company_1 ID="ProviderCompany" runat="server" />
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">到达城市：</span>
                            <uc:City ID="CityArrival" runat="server" />
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">销售关系：</span>
                            <asp:DropDownList ID="ddlSaleRelation" runat="server" CssClass="selectarea" AppendDataBoundItems="True">
                                <asp:ListItem runat="server" Text="全部" Value=""></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="input" style="z-index:3;" >
                            <span class="name">仅产生交易：</span>
                            <asp:CheckBox ID="chkHasTrade" runat="server" Checked="true" style="padding-top:5px" />&nbsp;
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">产品类型：</span>
                            <asp:DropDownList ID="ddlProductType" runat="server" CssClass="selectarea" AppendDataBoundItems="True">
                                <asp:ListItem runat="server" Text="全部" Value=""></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>
                    <td>
                        <div class="input" id="spType" style="display:none;">
                            <span class="name">特殊产品：</span>
                            <asp:DropDownList ID="ddlSpecialTickType" runat="server" CssClass="selectarea" AppendDataBoundItems="True">
                                <asp:ListItem runat="server" Text="全部" Value=""></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="btns" colspan="3">
                        <asp:Button runat="server" ID="btnQuery" class="btn class1 submit" Text="查&nbsp;&nbsp;&nbsp;询"
                            OnClick="btnQuery_Click" />
                        <input type="button" id="btnDownload" class="btn class1" value="下&nbsp;&nbsp;载" />
                        <input type="button" class="btn class2" onclick="ResetSearchOption();" value="清空条件" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div runat="server" id="counts" visible="false" style="margin:10px 0;">
        满足条件的查询结果统计：
        订单数：<asp:Label CssClass="obvious b" runat="server" ID="lblOrderCount"></asp:Label>
        票号数：<asp:Label CssClass="obvious b" runat="server" ID="lblPiaoCount"></asp:Label>
    </div>
    <div class="table data-scrop" id='data-list'>
        <asp:Repeater runat="server" ID="dataList" EnableViewState="false">
            <HeaderTemplate>
                <table>
                    <thead>
                        <tr>
                            <th>
                                公司名称
                            </th>
                            <th>
                                用户名
                            </th>
                            <th>
                                联系人
                            </th>
                            <th>
                                联系电话
                            </th>
                            <th>
                                注册日期
                            </th>
                            <th>
                                统计日期
                            </th>
                            <th>
                                订单数
                            </th>
                            <th>
                                票号数
                            </th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <%#Eval("Name")%>
                    </td>
                    <td>
                        <%#Eval("UserName")%>
                    </td>
                    <td>
                        <%#Eval("Contact")%>
                    </td>
                    <td>
                        <%#Eval("Mobile")%>
                    </td>
                    <td>
                        <%#Eval("RegisterDate")%>
                    </td>
                    <td>
                        <%#Eval("ReportDate")%>
                    </td>
                    <td>
                        <%#Eval("OrderCount")%>
                    </td>
                    <td>
                        <%#Eval("TicketCount")%>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                <tr>
                    <td style="*padding-bottom: 20px;">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <%=totalOrderCount%>
                    </td>
                    <td>
                        <%=totalTicketCount%>
                    </td>
                </tr>
                </tbody> </table>
            </FooterTemplate>
        </asp:Repeater>
        <div class="box" runat="server" visible="false" id="emptyDataInfo">
            没有任何符合条件的查询结果
        </div>
    </div>
    <div class="btns">
        <uc:Pager runat="server" ID="pager" Visible="false"></uc:Pager>
    </div>
    </form>
</body>
</html>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="../Scripts/selector.js?20121205" type="text/javascript"></script>
<script src="../Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="../Scripts/Report/common.js" type="text/javascript"></script>
<script src="../Scripts/OrderModule/QueryList.js?20121205" type="text/javascript"></script>
<script src="../Scripts/Global.js?20121205" type="text/javascript"></script>
<script type="text/javascript">
    var specialTypeText = "特殊";
    $(function ()
    {
        $("#ddlProductType").change(function ()
        {
            if ($.trim($("#ddlProductType option:checked").text()) == specialTypeText)
            {
                $("#spType").show();
            } else
            {
                $("#spType").hide();
            }
        }).trigger("change");

        $("#btnDownload").click(function ()
        {
            var startDate = $.trim($("#txtStartDate").val());
            var endDate = $.trim($("#txtEndDate").val());
            var departure = $.trim($("#txtDeparture_ddlAirports").val());
            var airlines = $.trim($("#ddlAirlines").val());
            var providerCompany = $.trim($("#ProviderCompany_hidCompanyId").val());
            var arrival = $.trim($("#CityArrival").val());
            var saleRelation = $.trim($("#ddlSaleRelation option:checked").val());
            var productType = $.trim($("#ddlProductType option:checked").val());
            var specialProductType = $.trim($("#ddlSpecialTickType option:checked").val());
            var hasTrade;
            if ($("#chkHasTrade").attr("checked") == "checked")
            {
                hasTrade = "true";
            } else
            {
                hasTrade = "false";
            }
            var condition = [startDate, endDate, departure, airlines, providerCompany, hasTrade, arrival, saleRelation, productType, specialProductType];
            window.open('ReportDownload.aspx?ProviderStatisticsCondition=' + condition.join(","), 'newWindow');
        });
        SaveDefaultData(null, '.text-s1');
    });

</script>
