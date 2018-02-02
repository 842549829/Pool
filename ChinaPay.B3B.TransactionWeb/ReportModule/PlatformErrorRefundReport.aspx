<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PlatformErrorRefundReport.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.ReportModule.PlatformErrorRefundReport" %>

<%@ Register Src="~/UserControl/Airport.ascx" TagName="City" TagPrefix="uc" %>
<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<%@ Register Src="~/UserControl/CompanyC.ascx" TagName="Company" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../Scripts/widget/common.js" type="text/javascript"></script>
    <script src="../Scripts/selector.js?20121205" type="text/javascript"></script>
    <script src="../Scripts/json2.js" type="text/javascript"></script>
</head>
<body>
    <form id="query" runat="server">
    <h3 class="titleBg">
        差错退款统计</h3>
    <div class="box-a">
        <div class="condition">
            <table>
                <colgroup>
                    <col class="w25" />
                    <col class="w25" />
                    <col class="w25" />
                    <col class="w25" />
                </colgroup>
                <tbody>
                    <tr>
                        <td>
                            <div class="input">
                                <span class="name">申请时间：</span>
                                <asp:TextBox runat="server" ID="txtApplyStartDate" class="text text-s1" onfocus=" WdatePicker({ skin: 'default', isShowClear: false, maxDate: '#F{$dp.$D(\'txtApplyEndDate\')||\'2020-10-01\'}' })"></asp:TextBox>
                                -
                                <asp:TextBox runat="server" ID="txtApplyEndDate" class="text text-s1" onfocus="WdatePicker({ skin: 'default', isShowClear: false, minDate: '#F{$dp.$D(\'txtApplyStartDate\')}', maxDate:'%y-%M-%d' })"></asp:TextBox>
                            </div>
                        </td>
                        <td>
                            <div class="input">
                                <span class="name">订单号：</span><asp:TextBox runat="server" ID="txtOrderId" class="text" />
                            </div>
                        </td>
                        <td>
                            <div class="input">
                                <span class="name">出发城市：</span>
                                <uc:City ID="txtDeparture" runat="server" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="input">
                                <span class="name">票号：</span>
                                <asp:TextBox runat="server" ID="txtSettleCode" class="text" Style="width: 40px;" />
                                <asp:TextBox runat="server" ID="txtTicketNo" class="text" />
                            </div>
                        </td>
                        <td>
                            <div class="input">
                                <span class="name">申请单号：</span><asp:TextBox runat="server" ID="txtApplyformId"
                                    class="text" />
                            </div>
                        </td>
                        <td>
                            <div class="input">
                                <span class="name">到达城市：</span>
                                <uc:City ID="txtArrivals" runat="server" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="input">
                                <span class="name">采购方：</span>
                                <uc:Company ID="txtPurchase" runat="server" />
                            </div>
                        </td>
                        <td>
                            <div class="input">
                                <span class="name">出票方：</span>
                                <uc:Company ID="txtProvider" runat="server" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" class="btns">
                            <asp:Button runat="server" ID="btnQuery" class="btn class1 submit" Text="查&nbsp;&nbsp;&nbsp;询"
                                OnClick="btnQuery_Click" />
                            <input type="button" id="btnDownload" class="btn class1" value="下&nbsp;&nbsp;载" />
                            <input type="button" class="btn class2" value="清空条件" onclick="ResetSearchOption();" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <div class="table data-scrop" id='data-list'>
        <asp:Repeater runat="server" ID="dataList" EnableViewState="false">
            <HeaderTemplate>
                <table style="width: 2500px">
                    <thead>
                        <tr>
                            <th>
                                采购方
                            </th>
                            <th>
                                出票方
                            </th>
                            <th>
                                订单号
                            </th>
                            <th>
                                结算码
                            </th>
                            <th>
                                票号
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
                                航程
                            </th>
                            <th>
                                乘机人
                            </th>
                            <th>
                                申请单号
                            </th>
                            <th>
                                申请时间
                            </th>
                            <th>
                                处理时间
                            </th>
                           
                            <th>
                                采购方所得退款
                            </th>
                            
                            <th>
                                出票方应退金额
                            </th>
                           
                            <th>
                                出票方手续费金额
                            </th>
                            
                            <th>
                                出票方实退金额
                            </th>
                            <th>
                                平台应退金额
                            </th>
                            <th>
                                平台手续费金额
                            </th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <%#Eval("PurchaserName")%>
                    </td>
                    <td>
                        <%#Eval("ProviderName")%>
                    </td>
                    <td>
                        <%#Eval("OrderId")%>
                    </td>
                    <td>
                        <%#Eval("SettleCode")%>
                    </td>
                    <td>
                        <%#Eval("TicketNos")%>
                    </td>
                    <td>
                        <%#Eval("CarrierName")%>
                    </td>
                    <td>
                        <%#Eval("FlightNos")%>
                    </td>
                    <td>
                        <%#Eval("Bunks")%>
                    </td>
                    <td>
                        <%#Eval("VoyageNames")%>
                    </td>
                    <td>
                        <%#Eval("Passenger")%>
                    </td>
                    <td>
                        <%#Eval("ApplyformId")%>
                    </td>
                    <td>
                        <%#Eval("AppliedTime")%>
                    </td>
                    <td>
                        <%#Eval("ProcessedTime")%>
                    </td>
                    <td>
                        <%#Eval("PurchaserAmount")%>
                    </td>
                    <td>
                        <%#Eval("ProviderAnticipation")%>
                    </td>
                    <td>
                        <%#Eval("ProviderTradeFee")%>
                    </td>
                    <td>
                        <%#Eval("ProviderAmount")%>
                    </td>
                    <td>
                        <%#Eval("PlatformTradeFee ")%>
                    </td>
                    <td>
                        <%#Eval("PlatformAmount")%>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
               
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
<script src="../Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
 <script src="../Scripts/Report/common.js?20130514" type="text/javascript"></script>
<script src="../Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="../Scripts/OrderModule/QueryList.js?20121119" type="text/javascript"></script>
<script src="../Scripts/Report/Scroll.js?20130104" type="text/javascript"></script>
<script src="../Scripts/Report/PlatformErrorRefundReport.js?20130515" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $("#txtOrderId").LimitLength(13).OnlyNumber();
        $("#txtApplyformId").LimitLength(13).OnlyNumber();
    })
</script>