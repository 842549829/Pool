<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProviderFinancialReport.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.ReportModule.ProviderFinancialReport" %>

<%@ Register Src="~/UserControl/Airport.ascx" TagName="City" TagPrefix="uc" %>
<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title>卖出资金报表</title>
</head>
<body>
    <form id="query" runat="server">
    <h3 class="titleBg">
        卖出资金报表（温馨提示：当日报表数据可于下午18：00后（或20：00、22：00）进行下载，当日全部数据需在次日进行下载。）</h3>
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
                            </asp:TextBox>
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
                            <span class="name">操作员：</span>
                            <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="selectarea" Width="178px">
                            </asp:DropDownList>
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">到达城市：</span>
                            <uc:City ID="txtArrivals" runat="server" />
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">OFFICE号：</span>
                            <asp:DropDownList ID="ddlOffice" runat="server" CssClass="selectarea">
                            </asp:DropDownList>
                        </div>
                    </td>
                </tr>
                <tbody id="seniorCondition" runat="server">
                    <tr>
                        <td>
                            <div class="input">
                                <span class="name resetName">支付日期：</span>
                                <asp:TextBox runat="server" ID="txtPayStartDate" class="text text-s1 reset" onfocus=" WdatePicker({ skin: 'default', isShowClear: false, maxDate: '#F{$dp.$D(\'txtPayEndDate\')||\'2020-10-01\'}' })">
                                </asp:TextBox>
                                <span class="resetName"> -</span>
                                <asp:TextBox runat="server" ID="txtPayEndDate" class="text text-s1 reset" onfocus="WdatePicker({ skin: 'default', isShowClear: false, minDate: '#F{$dp.$D(\'txtPayStartDate\')}', maxDate: '2020-10-01' })">
                                </asp:TextBox><asp:HiddenField runat="server" ID="hdDefaultDate" />
                            </div>
                        </td>
                        <td>
                            <div class="input">
                                <span class="name resetName">订单号：</span>
                                <asp:TextBox ID="txtOrderId" runat="server" CssClass="text textarea reset" Width="172px"></asp:TextBox>
                            </div>
                        </td>
                        <td>
                            <div class="input up-index" style="z-index: 53">
                                <span class="name fl resetName">政策类型：</span>
                                <div class="dlLeft fl">
                                    <asp:DropDownList ID="ddlPolicyType" runat="server" CssClass="selectarea reset">
                                        <asp:ListItem Selected="True" Value="" Text="全部" />
                                        <asp:ListItem Value="0" Text="普通" />
                                        <asp:ListItem Value="1" Text="特价" />
                                        <asp:ListItem Value="2" Text="特殊" />
                                        <asp:ListItem Value="3" Text="团队" />
                                    </asp:DropDownList>
                                </div>
                                <div class="dlLeft fl">
                                    <asp:DropDownList ID="ddlSpecialType" runat="server" Width="75px" CssClass="specialType">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="input">
                                <span class="name resetName">销售关系：</span>
                                <asp:DropDownList ID="ddlRelationType" runat="server" CssClass="selectarea fl reset">
                                </asp:DropDownList>
                                <asp:DropDownList ID="LowerCompany" runat="server">
                                </asp:DropDownList>
                                <asp:DropDownList ID="SubordinateCompany" runat="server">
                                </asp:DropDownList>
                            </div>
                        </td>
                        <td>
                            <div class="input">
                                <span class="name resetName">乘机人：</span>
                                <asp:TextBox ID="txtPassenger" runat="server" CssClass="text textarea reset" Width="172px"></asp:TextBox>
                            </div>
                        </td>
                        <td>
                            <div class="input">
                                <span class="name resetName">客票类型：</span>
                                <asp:DropDownList ID="ddlTiketType" runat="server" CssClass="selectarea reset">
                                </asp:DropDownList>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        
                        <td>
                            <div class="input">
                                <span class="name resetName">机票状态：</span>
                                <asp:DropDownList ID="ddlTicketStatus" runat="server" CssClass="selectarea reset" Width="178px">
                                </asp:DropDownList>
                            </div>
                        </td>
                        <td>
                            <div class="input">
                                <span class="name resetName">票号：</span><asp:TextBox runat="server" ID="txtTicketNo"
                                    class="text textarea reset" Width="172px"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                </tbody>
                <tr>
                    <td class="btns" colspan="3">
                        <asp:Button runat="server" ID="btnQuery" class="btn class1 submit" Text="查&nbsp;&nbsp;&nbsp;询"
                            OnClick="btnQuery_Click" />
                        <input type="button" id="btnDownload" class="btn class1" value="下&nbsp;&nbsp;载" />
                        <input type="button" class="btn class2" onclick="ResetSearchOption();" value="清空条件" />
                        <input type="button" class="btn class2" value="更多条件" id="btnSeniorCondition" runat="server" />
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
                <table style="width: 3500px">
                    <thead>
                        <tr>
                            <th style="width: 30px">
                                机票状态
                            </th>
                            <th style="width: 100px">
                                订单号
                            </th>
                            <th style="width: 40px">
                                行程
                            </th>
                            <th style="width: 30px">
                                客票类型
                            </th>
                            <th style="width: 30px">
                                政策类型
                            </th>
                            <th>
                                特殊票类型
                            </th>
                            <th style="width: 30px">
                                销售关系
                            </th>
                            <th>
                                采购方
                            </th>
                            <th style="width: 40px">
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
                            <th style="width: 70px">
                                支付时间
                            </th>
                            <th style="width: 70px">
                                出票时间
                            </th>
                            <th>
                                乘机时间
                            </th>
                            <th style="width: 70px">
                                退废时间
                            </th>
                            <th style="width: 30px">
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
                                出票PNR
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
                                佣金
                            </th>
                            <th>
                                退改签手续费
                            </th>
                            <th>
                                应收款
                            </th>
                            <th>
                                支付手续费
                            </th>
                            <th>
                                实收款
                            </th>
                            <th style="width: 30px">
                                账单状态
                            </th>
                            <th style="width: 60px">
                                OFFICE号
                            </th>
                            <th>
                                操作员
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
                        <%#Eval("Type")%>
                    </td>
                    <td>
                        <%#Eval("Id") %>
                    </td>
                    <td>
                        <%#Eval("TripType")%>
                    </td>
                    <td>
                        <%#Eval("TicketType")%>
                    </td>
                    <td>
                        <%#Eval("Product")%>
                    </td>
                    <td>
                        <%#Eval("SpecialProductType")%>
                    </td>
                    <td>
                        <%#Eval("Releation")%>
                    </td>
                    <td>
                        <%#Eval("PurchaserName")%>
                    </td>
                    <td>
                        <%#Eval("SettleCode")%>
                    </td>
                    <td>
                        <%#Eval("TicketNos")%>
                    </td>
                    <td>
                        <%#Eval("FlightNos")%>
                    </td>
                    <td class="obvious b">
                        <%#Eval("AirportPairNames")%>
                    </td>
                    <td>
                        <%#Eval("AirportPairs")%>
                    </td>
                    <td>
                        <%#Eval("PayTime")%>
                    </td>
                    <td>
                        <%#Eval("ETDZTime")%>
                    </td>
                    <td>
                        <%#Eval("TakeoffTimes")%>
                    </td>
                    <td>
                        <%#Eval("RefundTime")%>
                    </td>
                    <td>
                        <%#Eval("CarrierName")%>
                    </td>
                    <td>
                        <%#Eval("Bunks")%>
                    </td>
                    <td>
                        <%#Eval("Passenger")%>
                    </td>
                    <td>
                        <%#Eval("PNR")%>
                    </td>
                    <td>
                        <%#Eval("NewPNR") %>
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
                        <%#Eval("RefundFee")%>
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
                        <%#Eval("OfficeNo")%>
                    </td>
                    <td>
                        <%#Eval("Processor")%>
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
                        <%=totalAmount%>
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
    <asp:HiddenField ID="hfdProviderCompanyId" runat="server" />
    <asp:HiddenField ID="hfdSeniorCondition" runat="server" />
    </form>
</body>
</html>
<script src="../Scripts/core/jquery.js" type="text/javascript"></script>
<script src="../Scripts/selector.js?20121205" type="text/javascript"></script>
<script src="../Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="../Scripts/Report/common.js" type="text/javascript"></script>
<script src="../Scripts/OrderModule/QueryList.js?20121119" type="text/javascript"></script>
<script src="../Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="../Scripts/Report/ProviderFinancialReport.js?20130225" type="text/javascript"></script>
<script src="../Scripts/Report/Scroll.js?20130104" type="text/javascript"></script>