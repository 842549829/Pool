<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PlatformTicketReport.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.ReportModule.PlatformTicketReport" %>

<%@ Register TagPrefix="uc" TagName="Pager" Src="~/UserControl/Pager.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Company_2" Src="~/UserControl/CompanyC.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>机票明细</title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="query" runat="server" autocomplete="off">
    <h3 class="titleBg">
        机票明细</h3>
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
                            <span class="name">销售日期：</span>
                            <asp:TextBox runat="server" ID="txtStartDate" class="text text-s1" onfocus="WdatePicker({ skin: 'default', isShowClear: false, maxDate: '#F{$dp.$D(\'txtEndDate\')||\'2020-10-01\'}' })">
                            </asp:TextBox>
                            -
                            <asp:TextBox runat="server" ID="txtEndDate" class="text text-s1" onfocus="WdatePicker({ skin: 'default', isShowClear: false, minDate: '#F{$dp.$D(\'txtStartDate\')}', maxDate:'%y-%M-%d' })">
                            </asp:TextBox>
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">订单号：</span>
                            <asp:TextBox ID="txtOrderId" runat="server" CssClass="text textarea"></asp:TextBox>
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
                        <div class="input" style="z-index:4;" class="uc">
                            <span class="name">产品方：</span>
                            <uc1:Company_2 runat="server" ID="txtProductCompany" />
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">乘机人：</span><asp:TextBox runat="server" ID="txtPassenger" class="text textarea"></asp:TextBox>
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">机票状态：</span>
                            <asp:DropDownList ID="ddlTicketStatus" runat="server" CssClass="selectarea">
                            </asp:DropDownList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="input" style="z-index:3;" class="uc">
                            <span class="name">采购方：</span>
                            <uc1:Company_2 runat="server" ID="txtPurchaseCompany" />
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">票号：</span><asp:TextBox runat="server" ID="txtTicketNo" class="text textarea"></asp:TextBox>
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">销售关系：</span>
                            <asp:DropDownList ID="ddlRelationType" runat="server" CssClass="selectarea">
                            </asp:DropDownList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="input" style="z-index:2;" class="uc">
                            <span class="name">出票方：</span>
                            <uc1:Company_2 runat="server" ID="txtProviderCompany" />
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">PNR：</span><asp:TextBox runat="server" ID="txtPNR" class="text textarea"></asp:TextBox>
                        </div>
                    </td>
                   <td>
                     <div class="input">
                       <span class="name">支付方式：</span>
                       <asp:DropDownList ID="ddlPayType" CssClass="selectarea" runat="server">
                          <asp:ListItem Value="" Text="全部"></asp:ListItem>
                          <asp:ListItem Value="0" Text="非国付通支付"></asp:ListItem>
                          <asp:ListItem Value="1" Text="国付通支付"></asp:ListItem>
                       </asp:DropDownList>
                     </div>
                   </td>
                </tr>
                <tr>
                   <td>
                        <div class="input" style="z-index:1;">
                            <span class="name">航班日期：</span>
                            <asp:TextBox runat="server" ID="txtTakeOffLowerTime" class="text text-s1" onfocus="WdatePicker({ skin: 'default', isShowClear: false, maxDate: '#F{$dp.$D(\'txtTakeOffUpperTime\')||\'2020-10-01\'}' })">
                            </asp:TextBox>
                            -
                            <asp:TextBox runat="server" ID="txtTakeOffUpperTime" class="text text-s1" onfocus="WdatePicker({ skin: 'default', isShowClear: false, minDate: '#F{$dp.$D(\'txtTakeOffLowerTime\')}' })">
                            </asp:TextBox><asp:HiddenField runat="server" ID="hdDefaultDate" />
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
    <div runat="server" id="counts" visible="false" style="margin:10px 0;">
        采购支付金额：<asp:Label CssClass="obvious b" runat="server" ID="lblPurchaserAmount"></asp:Label>
        出票方实收款：<asp:Label CssClass="obvious b" runat="server" ID="lblProviderAmount"></asp:Label>
        产品方应收款：<asp:Label CssClass="obvious b" runat="server" ID="lblSupplierAmount"></asp:Label>
        分润方应收款：<asp:Label CssClass="obvious b" runat="server" ID="lblRoyaltyAmount"></asp:Label>
        平台改签收入：<asp:Label CssClass="obvious b" runat="server" ID="lblPostponeFee"></asp:Label>
        政策扣点/贴点收益：<asp:Label CssClass="obvious b" runat="server" ID="lblPlatformCommission"></asp:Label>

        <br /><br />
        溢价收入:<asp:Label CssClass="obvious b" runat="server" ID="lblPlatformPremium"></asp:Label>
        平台利润：<asp:Label CssClass="obvious b" runat="server" ID="lblPlatformProfit"></asp:Label>
        </div>
    <div class="table data-scrop" id='data-list'>
        <asp:Repeater runat="server" ID="dataList" EnableViewState="false">
            <HeaderTemplate>
                <table style="width: 3500px">
                    <thead>
                        <tr>
                            <th style="width:90px;" >
                                订单号
                            </th>
                            <th style="width:110px;">
                                销售时间
                            </th>
                            <th>
                                出票方
                            </th>
                            <th>
                                产品方
                            </th>
                            <th>
                                采购方
                            </th>
                            <th style="width:30px;">
                                销售关系
                            </th>
                            <th>
                                结算码
                            </th>
                            <th style="width:70px;">
                                票号
                            </th>
                             <th>
                              乘机人
                            </th>
                            <th>
                                行程
                            </th>
                            <th style="width:30px;">
                                机票状态
                            </th>
                            <th>
                                航班号
                            </th>
                            <th style="width:30px;">
                                航空公司
                            </th>
                            <th>
                                航程(中文)
                            </th>
                            <th style="width:80px;">
                                航程(代码)
                            </th>
                            <th style="width:120px;">
                                航班时间
                            </th>
                            <th style="width:90px;">
                                订座PNR
                            </th>
                            <th style="width:90px;">
                                出票PNR
                            </th>
                            <th>
                                舱位
                            </th> 
                            <th>
                                票面价
                            </th>
                            <th>
                                服务费
                            </th>
                            <th style="width:60px;">
                                民航基金
                            </th>
                            <th style="width:80px;">
                                燃油附加费
                            </th>
                            <th style="width:30px;">
                                卖出返点
                            </th>
                            <th  style="width:30px;">
                                买入返点
                            </th>
                            <th style="width:50px;">
                                平台扣点/贴点
                            </th>
                            <th style="width:40px;">
                                产品方返点
                            </th>
                            <th  style="width:40px;">
                              分润方返点
                            </th>
                            <th style="width:70px;">
                                采购方支付金额
                            </th>
                            <th  style="width:40px;">
                                出票方应收款
                            </th>
                            <th  style="width:60px;">
                                出票方手续费率
                            </th>
                            <th  style="width:60px;">
                                出票方手续费
                            </th>
                            <th  style="width:60px;">
                                出票方实收款
                            </th>
                            <th  style="width:40px;">
                                产品方佣金
                            </th>
                            <th  style="width:40px;">
                                产品方应收款
                            </th>
                            <th  style="width:60px;">
                                产品方手续费率
                            </th>
                            <th  style="width:40px;">
                                产品方手续费
                            </th>
                            <th  style="width:60px;">
                                产品方实收款
                            </th>
                             <th  style="width:40px;">
                                分润方佣金
                            </th>
                            <th  style="width:60px;">
                              分润方加价金额
                            </th>
                            <th  style="width:40px;">
                                分润方应收款
                            </th>
                            <th  style="width:60px;">
                                分润方手续费率
                            </th>
                            <th  style="width:40px;">
                                分润方手续费
                            </th>
                            <th  style="width:60px;">
                                分润方实收款
                            </th>
                            <th  style="width:60px;">
                                平台改签收入
                            </th>
                            <th>
                                政策扣点/贴点收益
                            </th>
                            <th>
                                溢价收入
                            </th>
                            <th>
                                平台利润
                            </th>
                            <th>
                                支付方式
                            </th>
                            <th>
                                出票效率(分)
                            </th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <%#Eval("Id")%>
                    </td>
                    <td>
                        <%#Eval("FinishTime")%>
                    </td>
                    <td>
                        <%#Eval("ProviderName")%>
                    </td>
                    <td>
                        <%#Eval("SupplierName")%>
                    </td>
                    <td>
                        <%#Eval("PurchaserName")%>
                    </td>
                    <td>
                        <%#Eval("SellReleation")%>
                    </td>
                    <td>
                        <%#Eval("SettleCode")%>
                    </td>
                    <td>
                        <%#Eval("TicketNo")%>
                    </td>
                     <td>
                      <%#Eval("Passenger")%>
                    </td>
                    <td>
                        <%#Eval("TripType")%>
                    </td>
                    <td>
                        <%#Eval("Type")%>
                    </td>
                    <td>
                        <%#Eval("FlightNo")%>
                    </td>
                    <td>
                        <%#Eval("CarrierName")%>
                    </td>
                    <td class="obvious b">
                        <%#Eval("DepartureCityName")%><%#Eval("DepartureName")%>-<%#Eval("ArrivalCityName")%><%#Eval("ArrivalName")%>
                    </td>
                    <td>
                        <%#Eval("Departure")%>-<%#Eval("Arrival")%>
                    </td>
                    <td>
                        <%#Eval("TakeoffTime")%>
                    </td>
                    <td>
                        <%#Eval("PNR")%>
                    </td>
                    <td>
                        <%#Eval("NewPNR")%>
                    </td>
                    <td>
                        <%#Eval("Bunk")%>
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
                        <%#Eval("ProviderRebate")%>
                    </td>
                    <td>
                        <%#Eval("PurchaserRebate")%>
                    </td>
                    <td>
                        <%#Eval("PlatformRebate")%>
                    </td>
                    <td>
                        <%#Eval("SupplierRebate")%>
                    </td>
                    <td>
                      <%#Eval("RoyaltyRebate") %>
                    </td>
                    <td>
                        <%#Eval("PurchaserAmount")%>
                    </td>
                    <td>
                        <%#Eval("ProviderAnticipation")%>
                    </td>
                    <td>
                        <%#Eval("ProviderTradeRate")%>
                    </td>
                    <td>
                        <%#Eval("ProviderTradeFee")%>
                    </td>
                    <td>
                        <%#Eval("ProviderAmount")%>
                    </td>
                    <td>
                        <%#Eval("SupplierCommission")%>
                    </td>
                    <td>
                        <%#Eval("SupplierAnticipation")%>
                    </td>
                    <td>
                        <%#Eval("SupplierTradeRate")%>
                    </td>
                    <td>
                        <%#Eval("SupplierTradeFee")%>
                    </td>
                    <td>
                        <%#Eval("SupplierAmount")%>
                    </td>

                     <td>
                        <%#Eval("RoyaltyCommission")%>
                    </td>
                    <td>
                      <%#Eval("RoyaltyIncreasing")%>
                    </td>
                    <td>
                        <%#Eval("RoyaltyAnticipation")%>
                    </td>
                    <td>
                        <%#Eval("RoyaltyTradeRate")%>
                    </td>
                    <td>
                        <%#Eval("RoyaltyTradeFee")%>
                    </td>
                    <td>
                        <%#Eval("RoyaltyAmount")%>
                    </td>
                    <td>
                        <%#Eval("PostponeFee")%>
                    </td>
                    <td>
                        <%#Eval("PlatformCommission")%>
                    </td>
                    <td>
                        <%#Eval("Premium")%>
                    </td>
                    <td>
                        <%#Eval("PlatformProfit")%>
                    </td>
                    <td>
                        <%#Eval("PayType")%>
                    </td>
                    <td>
                        <%#Eval("Speed")%>
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
                        <%=totalPurchaserAmount%>
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
                        <%=totalProviderAmount%>
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
                        <%=totalSupplierAmount%>
                    </td>
                    <td>
                    &nbsp;</td>
                    <td>

                    &nbsp;</td>
                    <td>
                    &nbsp;</td>
                    <td>
                    &nbsp;</td>
                    <td>
                    &nbsp;</td>
                    <td>
                      <%=totalRoyaltyAmount %>
                    </td>
                    <td>
                        <%=totalPostponeFee%>
                    </td>
                    <td>
                        <%=totalPlatformCommission%>
                    </td>
                    <td>
                       <%=totalPlatformPremium%>
                    </td>
                    <td>
                        <%=totalPlatformProfit%>
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
    </form>
</body>
</html> 
<script src="../Scripts/selector.js?20121205" type="text/javascript"></script>
<script src="../Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="../Scripts/Report/common.js" type="text/javascript"></script>
<script src="/Scripts/json2.js" type="text/javascript"></script>
    <script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="../Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="../Scripts/OrderModule/QueryList.js?20121119" type="text/javascript"></script>
<script src="../Scripts/Report/PlatformTicketReport.js?20130114" type="text/javascript"></script>
<script src="../Scripts/Report/Scroll.js?20130104" type="text/javascript"></script>
<script type="text/javascript">
    $(function(){
       for (var i = 0; i <  $(".uc").length; i++) {
            $(".uc").eq(i).find("div").css("z-index",5-parseInt(i));
        }
    });
</script>