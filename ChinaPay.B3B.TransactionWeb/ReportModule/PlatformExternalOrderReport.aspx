<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PlatformExternalOrderReport.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.ReportModule.PlatformExternalOrderReport" %>
<%@ Register Src="~/UserControl/Airport.ascx" TagName="City" TagPrefix="uc" %>
<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <title></title>
</head>
<body>
    <form id="query" runat="server">
    <h3 class="titleBg">
        外部订单明细表</h3>
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
                            <span class="name">支付日期：</span>
                            <asp:TextBox runat="server" ID="txtPayStartDate" class="text text-s1" onfocus="WdatePicker({ skin: 'default', isShowClear: false, maxDate: '#F{$dp.$D(\'txtPayEndDate\')||\'2020-10-01\'}' })">
                            </asp:TextBox>
                            -
                            <asp:TextBox runat="server" ID="txtPayEndDate" class="text text-s1" onfocus="WdatePicker({ skin: 'default', isShowClear: false, minDate: '#F{$dp.$D(\'txtPayStartDate\')}', maxDate:'%y-%M-%d' })">
                            </asp:TextBox><asp:HiddenField runat="server" ID="hdDefaultDate" />
                        </div>
                    </td>
                    <td>
                      <div class="input">
                            <span class="name">出票状态：</span>
                            <asp:DropDownList ID="ddlPrintStatus" runat="server" CssClass="selectarea">
                            <asp:ListItem Value="" Text="全部"></asp:ListItem>
                            <asp:ListItem Value="1" Text="已出票"></asp:ListItem>
                            <asp:ListItem Value="0" Text="未出票"></asp:ListItem>
                            <asp:ListItem Value="2" Text="取消出票"></asp:ListItem>
                            </asp:DropDownList>
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
                            <span class="name">出发城市：</span>
                            <uc:City ID="txtDeparture" runat="server" />
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">支付状态：</span>
                            <asp:DropDownList ID="ddlPayStatus" runat="server" CssClass="selectarea">
                              <asp:ListItem Value="" Text="全部"></asp:ListItem>
                              <asp:ListItem Value="1" Text="已支付"></asp:ListItem>
                              <asp:ListItem Value="0" Text="未支付"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">PNR：</span>
                            <asp:TextBox ID="txtPnr" runat="server" CssClass="text textarea"></asp:TextBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="input">
                            <span class="name">到达城市：</span>
                            <uc:City ID="txtArrival" runat="server" />
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">订单来源：</span>
                            <asp:DropDownList ID="ddlOrderSource" runat="server" CssClass="selectarea">
                            </asp:DropDownList>
                        </div>
                    </td>
                    <td>
                         <div class="input">
                            <span class="name">外部订单号：</span>
                            <asp:TextBox ID="txtExternalOrderId" runat="server" CssClass="text textarea"></asp:TextBox>
                        </div>
                        <div class="input">
                            <span class="name">内部订单号：</span>
                            <asp:TextBox ID="txtInternalOrderId" runat="server" CssClass="text textarea"></asp:TextBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="btns" colspan="3">
                        <asp:Button runat="server" ID="btnQuery" class="btn class1 submit" Text="查&nbsp;&nbsp;&nbsp;询"
                            OnClick="btnQuery_Click" />
                        <input type="button" id="btnDownload" class="btn class1" value="下&nbsp;&nbsp;载" />
                        <input type="button" class="btn class2" value="清空条件" onclick="ResetSearchOption();" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div runat="server" id="counts" visible="false" style="margin: 10px 0;">
        收款金额：<asp:Label CssClass="obvious b" runat="server" ID="lblReceiveAmount"></asp:Label>
        付款金额：<asp:Label CssClass="obvious b" runat="server" ID="lblPaymentAmount"></asp:Label>
        利润总额：<asp:Label CssClass="obvious b" runat="server" ID="lblProfitAmount"></asp:Label>
        </div>
    <div class="table data-scrop" id='data-list'>
        <asp:Repeater runat="server" ID="dataList" EnableViewState="false">
            <HeaderTemplate>
                <table style="width:1300px;">
                    <thead>
                        <tr>
                            <th>
                                支付时间
                            </th>
                            <th>
                                外部订单号
                            </th>
                            <th>
                                内部订单号
                            </th>
                            <th>
                                订单来源
                            </th>
                            <th>
                                PNR
                            </th>
                            <th>
                                起抵城市
                            </th>
                            <th>
                               航空公司
                            </th>
                            <th>
                             航班号
                            </th>
                            <th>
                              舱位
                            </th>
                            <th>
                                票面价
                            </th>
                            <th>
                                外部返点
                            </th>
                            <th>
                                机建/燃油
                            </th>
                            <th>
                                支付状态
                            </th>
                            <th>
                                出票状态
                            </th>
                            <th>
                               收款金额
                            </th>
                            <th>
                               付款金额
                            </th>
                            <th>
                                留点
                            </th>
                            <th>
                                订单利润
                            </th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                            <td>
                                <%#Eval("PayTime")%>
                            </td>
                            <td>
                                <%#Eval("ExternalOrderId")%>
                            </td>
                            <td>
                                <%#Eval("OrderId")%>
                            </td>
                            <td>
                               <%#Eval("Platform")%>
                            </td>
                            <td>
                               <%#Eval("PNR")%>
                            </td>
                            <td>
                                <%#Eval("VoyageNames")%>
                            </td>
                            <td>
                              <%#Eval("Airline")%>
                            </td>
                            <td>
                              <%#Eval("FlightNos")%>
                            </td>
                            <td>
                              <%#Eval("Bunks")%>
                            </td>
                            <td>
                               <%#Eval("Fare")%>
                            </td>
                            <td>
                                <%#Eval("OriginalRebate")%>
                            </td>
                            <td>
                               <%#Eval("Airport")%>
                            </td>
                            <td>
                                <%#Eval("Paid")%>
                            </td>
                            <td>
                                <%#Eval("ETDZed")%>
                            </td>
                            <td>
                              <%#Eval("ReceivingAmount")%>
                            </td>
                            <td>
                               <%#Eval("PayAmount")%>
                            </td>
                            <td>
                               <%#Eval("Deduct")%>
                            </td>
                            <td>
                               <%#Eval("Profit")%>
                            </td>
                        </tr>
            </ItemTemplate>
            <FooterTemplate>
              <tr>
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
                               <%=totalReceiveAmount%>
                            </td>
                            <td>
                              <%=totalPaymentAmount%>
                            </td>
                            <td>
                                 &nbsp;
                            </td>
                            <td>
                               <%=totalProfitAmount%>
                            </td>
                        </tr>
                        </tbody>
                        </table>
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
<script src="../Scripts/selector.js?20130306" type="text/javascript"></script>
<script src="../Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="../Scripts/Report/common.js" type="text/javascript"></script>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="../Scripts/Global.js?20130306" type="text/javascript"></script>
<script src="../Scripts/OrderModule/QueryList.js?20130306" type="text/javascript"></script>
<script src="../Scripts/Report/PlatformExternalOrderReport.js?20130306" type="text/javascript"></script>
<script src="../Scripts/Report/Scroll.js?20130306" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        for (var i = 0; i < $(".uc").length; i++) {
            $(".uc").eq(i).find("div").css("z-index", 5 - parseInt(i));
        }
        $("#txtExternalOrderId").LimitLength(20);
        $("#txtInternalOrderId").LimitLength(13).OnlyNumber();
        SaveDefaultData(null, '.text-s1');
    });
</script>
