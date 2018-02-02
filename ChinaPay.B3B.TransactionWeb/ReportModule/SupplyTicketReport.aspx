<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SupplyTicketReport.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.ReportModule.SupplyTicketReport" %>

<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>提成明细</title>
</head>
<body>
    <form id="query" runat="server">
    <h3 class="titleBg">
        提成明细（温馨提示：当日报表数据可于下午18：00后（或20：00、22：00）进行下载，当日全部数据需在次日进行下载。）</h3>
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
                            <asp:TextBox runat="server" ID="txtFinishStartDate" class="text text-s" onfocus=" WdatePicker({ skin: 'default', isShowClear: false, maxDate: '#F{$dp.$D(\'txtFinishEndDate\')||\'2020-10-01\'}' })">
                            </asp:TextBox>
                            -
                            <asp:TextBox runat="server" ID="txtFinishEndDate" class="text text-s" onfocus="WdatePicker({ skin: 'default', isShowClear: false, minDate: '#F{$dp.$D(\'txtFinishStartDate\')}', maxDate:'%y-%M-%d' })">
                            </asp:TextBox><asp:HiddenField ID="hdDefaultDate" runat="server" />
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">订单号：</span><asp:TextBox runat="server" ID="txtOrderId" class="text textarea"></asp:TextBox>
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">PNR：</span><asp:TextBox runat="server" ID="txtPNR" class="text textarea"></asp:TextBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="input">
                            <span class="name">航空公司：</span>
                            <asp:DropDownList ID="ddlAirlines" runat="server" CssClass="selectarea">
                            </asp:DropDownList>
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">机票状态：</span>
                            <asp:DropDownList ID="ddlTicketStatus" runat="server" CssClass="selectarea">
                            </asp:DropDownList>
                        </div>
                    </td>
                    <td>
                      <div class="input">
                        <span class="name">特殊票类型：</span>
                        <asp:DropDownList ID="ddlSpecialType" runat="server" CssClass="selectarea">
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
        实收款：<asp:Label CssClass="obvious b" runat="server" ID="lblTradeAmount"></asp:Label> 
    </div>
    <div class="table data-scrop" id='data-list'>
        <asp:Repeater runat="server" ID="dataList" EnableViewState="false">
            <HeaderTemplate>
                <table style="width: 2500px">
                    <thead>
                        <tr>
                            <th>
                                订单号
                            </th>
                            <th>
                                机票状态
                            </th>
                            <th>
                                行程
                            </th>
                            <th>
                                客票类型
                            </th>
                            <th>
                             特殊票类型
                            </th>
                            <th>
                                结算码
                            </th>
                            <th>
                                票号
                            </th>
                            <th>
                                航班号
                            </th>
                            <th>
                                航程(中文)
                            </th>
                            <th>
                                航程(代码)
                            </th>
                            <th>
                                支付时间
                            </th>
                            <th>
                                出票时间
                            </th>
                            <th>
                                乘机时间
                            </th>
                            <th>
                                退废时间
                            </th>
                            <th>
                                航空公司
                            </th>
                            <th>
                                舱位
                            </th>
                            <th>
                                乘机人
                            </th>
                            <th>
                                订座PNR
                            </th>
                            <th>
                                票面价
                            </th>
                            <th>
                                服务费
                            </th>
                            <th>
                                民航基金
                            </th>
                            <th>
                                燃油附加费
                            </th>
                            <th>
                                返点
                            </th>
                            <th>
                                提成
                            </th>
                            <th>
                                应收款
                            </th>
                            <th>
                                交易手续费
                            </th>
                            <th>
                                实收款
                            </th>
                            <th>
                                账单状态
                            </th>
                            <th>
                                出票PNR
                            </th>
                            <th>
                                收款账号
                            </th>
                            <th>
                                备注
                            </th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <%# Eval("Id") %>
                    </td>
                    <td>
                        <%# Eval("Type")%>
                    </td>
                    <td>
                        <%# Eval("TripType")%>
                    </td>
                    <td>
                        <%# Eval("TicketType")%>
                    </td>
                    <td>
                      <%#Eval("SpecialProductType")%>
                    </td>
                    <td>
                        <%# Eval("SettleCode")%>
                    </td>
                    <td>
                        <%# Eval("TicketNo")%>
                    </td>
                    <td>
                        <%# Eval("FlightNo")%>
                    </td>
                    <td class="obvious b">
                        <%# Eval("DepartureCityName")%><%#Eval("DepartureName")%>-<%#Eval("ArrivalCityName")%><%#Eval("ArrivalName")%>
                    </td>
                    <td>
                        <%# Eval("Departure")%>-<%#Eval("Arrival")%>
                    </td>
                    <td>
                        <%#Eval("PayTime")%>
                    </td>
                    <td>
                        <%#Eval("ETDZTime")%>
                    </td>
                    <td>
                        <%#Eval("TakeoffTime")%>
                    </td>
                    <td>
                        <%# Eval("RefundTime")%>
                    </td>
                    <td>
                        <%#Eval("CarrierName")%>
                    </td>
                    <td>
                        <%#Eval("Bunk")%>
                    </td>
                    <td>
                        <%#Eval("Passenger")%>
                    </td>
                    <td>
                        <%#Eval("PNR")%>
                    </td>
                    <td>
                        <%#Eval("Fare")%>
                    </td>
                    <td>
                        <%#Eval("ServiceCharge")%>
                    </td>
                    <td>
                        <%#Eval("AirportFee")%>
                    </td>
                    <td>
                        <%#Eval("BAF")%>
                    </td>
                    <td>
                        <%#Eval("Rebate")%>
                    </td>
                    <td>
                        <%#Eval("Commission")%>
                    </td>
                    <td>
                        <%#Eval("Anticipation")%>
                    </td>
                    <td>
                        <%#Eval("TradeFee")%>
                    </td>
                    <td>
                        <%#Eval("TradeAmount")%>
                    </td>
                    <td>
                        <%#Eval("Success") %>
                    </td>
                    <td>
                        <%#Eval("NewPNR")%>
                    </td>
                    <td>
                        <%#Eval("TradeAccount")%>
                    </td>
                    <td>
                        <%#Eval("Remark")%>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                <tr>
                    <td  style="*padding-bottom:20px;">
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
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <%=totalTradeAmount%>
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
                </tr>
                </tbody> </table>
            </FooterTemplate>
        </asp:Repeater>
        <div class="box" runat="server" visible="false" id="emptyDataInfo">
            没有任何符合条件的查询结果
        </div>
    </div>
    <div class="btns">
        <uc:Pager runat="server" ID="pager" Visible="false">
        </uc:Pager>
    </div>
    <asp:HiddenField ID="hfdSupplyCompanyId" runat="server" />
    </form>
</body>
</html>
<script src="../Scripts/core/jquery.js" type="text/javascript"></script>
<script src="../Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="../Scripts/Report/common.js" type="text/javascript"></script>
<script src="../Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="../Scripts/OrderModule/QueryList.js?20121119" type="text/javascript"></script>
<script src="../Scripts/Report/SupplyTicketReport.js" type="text/javascript"></script>
<script src="../Scripts/Report/Scroll.js?20130104" type="text/javascript"></script>