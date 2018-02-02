<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PurchaseStatisticsReport.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.ReportModule.PurchaseStatisticsReport" %>

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
        平台采购量统计</h3>
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
                        <div class="input">
                            <span class="name">采购商：</span>
                            <uc1:Company_1 ID="PurchaseCompany" runat="server" />
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">仅产生交易：</span>
                            <asp:CheckBox ID="chkHasTrade" runat="server" Checked="true" />&nbsp;
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
<script src="../Scripts/Report/PurchaseStatisticsReport.js?20121205" type="text/javascript"></script>
