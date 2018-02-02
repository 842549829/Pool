<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoyaltyProfitReport.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.ReportModule.RoyaltyProfitReport" %>

<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<%@ Register TagPrefix="uc1" TagName="Company_2" Src="~/UserControl/CompanyC.ascx" %>
<%@ Import Namespace="ChinaPay.B3B.Common.Enums" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .input
        {
            margin-bottom: 0px;
        }
    </style>
    <script type="text/javascript" src="../Scripts/jquery-1.7.2.min.js"></script>
</head>
<body>
    <form id="query" runat="server">
    <h3 class="titleBg">
        下级提成明细</h3>
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
                            <span class="name">出票日期：</span>
                            <asp:TextBox runat="server" ID="txtStartDate" class="text text-s1" onfocus=" WdatePicker({ skin: 'default', isShowClear: false, maxDate: '#F{$dp.$D(\'txtEndDate\')||\'2020-10-01\'}' })">
                            </asp:TextBox>
                            -
                            <asp:TextBox runat="server" ID="txtEndDate" class="text text-s1" onfocus="WdatePicker({ skin: 'default', isShowClear: false, minDate: '#F{$dp.$D(\'txtStartDate\')}', maxDate:'%y-%M-%d' })">
                            </asp:TextBox><asp:HiddenField runat="server" ID="hdDefaultDate" />
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">PNR：</span>
                            <asp:TextBox ID="txtPnr" runat="server" CssClass="text"></asp:TextBox>
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">订单号：</span>
                            <asp:TextBox ID="txtOrderId" runat="server" CssClass="text"></asp:TextBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="input">
                            <span class="name">票号：</span>
                            <asp:TextBox ID="txtTicketNo" runat="server" CssClass="text"></asp:TextBox>
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">类型：</span>
                            <asp:DropDownList ID="ddlType" runat="server">
                                <asp:ListItem Value="" Text="全部"></asp:ListItem>
                                <asp:ListItem Value="0" Text="收款"></asp:ListItem>
                                <asp:ListItem Value="1" Text="退款"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">支付状态：</span>
                            <asp:DropDownList ID="ddlPayStatus" runat="server">
                                <asp:ListItem Value="" Text="全部"></asp:ListItem>
                                <asp:ListItem Value="1" Text="成功"></asp:ListItem>
                                <asp:ListItem Value="0" Text="失败"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>
                  
                </tr>
                <tr>
                     <td>
                        <div class="input">
                            <span class="name">支付方式：</span>
                            <asp:DropDownList ID="ddlPayType" runat="server">
                                <asp:ListItem Value="" Text="全部"></asp:ListItem>
                                <asp:ListItem Value="0" Text="非国付通支付"></asp:ListItem>
                                <asp:ListItem Value="1" Text="国付通支付"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>
                    <td>
                      <div class="input"  style="z-index:3;" class="uc">
                       <span class="name">采购方：</span>
                            <uc1:Company_2 runat="server" ID="txtPurchaseCompany" />
                            <asp:DropDownList ID="ddlPurchaseCompany" runat="server"></asp:DropDownList>
                      </div>
                    </td>
                    <td id="incomeGroup" runat="server">
                       <div class="input">
                         <span class="name">用户组：</span>
                         <asp:DropDownList ID="ddlIncomeGroup" runat="server"></asp:DropDownList>
                       </div>
                    </td>
                     <td id="royalty" runat="server">
                        <div class="input">
                            <span class="name">分润方：</span>
                            <asp:DropDownList ID="ddlRoyaltyCompany" runat="server">
                            </asp:DropDownList>
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
        手续费：<asp:Label CssClass="obvious b" runat="server" ID="lblTradeFee"></asp:Label>
        提成：<asp:Label CssClass="obvious b" runat="server" ID="lblTradeRoyalty"></asp:Label>
        实收款：<asp:Label CssClass="obvious b" runat="server" ID="lblTradeAmount"></asp:Label>
    </div>
    <div class="table data-scrop" id='data-list'>
        <asp:Repeater runat="server" ID="dataList" EnableViewState="false">
            <HeaderTemplate>
                <table style="width: 2200px">
                    <thead>
                        <tr>
                           <th style='display:<%=(bool)(this.CurrentCompany.CompanyType == CompanyType.Platform)?"":"none"%>'>
                             分润方
                           </th>
                            <th style="width:30px;">
                                类型
                            </th>
                            <th style="width:30px;">
                                支付状态
                            </th>
                            
                            <th>
                                结算码
                            </th>
                            <th>
                                票号
                            </th>
                            <th style="width:30px;">
                                机票状态
                            </th>
                            <th>
                                订座PNR
                            </th>
                            <th>
                             出票PNR
                            </th>
                            <th>
                                承运人
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
                                票面价
                            </th>
                            <th>
                                税费
                            </th>
                            <th>
                                扣点
                            </th>
                            <th>
                                加价
                            </th>
                               <th>
                                提成
                            </th>
                            <th>
                                手续费
                            </th>
                         
                            <th>
                                实收款
                            </th>
                            <th>
                                采购方
                            </th>
                            <th>
                              用户组
                            </th>
                            <th>
                                订单号
                            </th>
                             <th>
                                支付时间
                            </th>
                            <th>
                                出票时间
                            </th>
                            <th>
                            退票时间
                            </th>
                            <th>
                                支付方式
                            </th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                   <td style='display:<%=(bool)(this.CurrentCompany.CompanyType == CompanyType.Platform)?"":"none"%>'>
                     <%#Eval("RoyaltyName")%>
                   </td>
                    <td>
                        <%#Eval("BillType")%>
                    </td>
                    <td>
                        <%#Eval("Success")%>
                    </td>
                    
                    <td>
                        <%#Eval("SettleCode")%>
                    </td>
                    <td>
                        <%#Eval("TicketNos")%>
                    </td>
                    <td>
                        <%#Eval("Type")%>
                    </td>
                    <td>
                        <%#Eval("PNR")%>
                    </td>
                    <td>
                        <%#Eval("NewPNR")%>
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
                        <%#Eval("Fare")%>
                    </td>
                    <td>
                        <%#Eval("Tax")%>
                    </td>
                    <td>
                        <%#Eval("Rebate")%>
                    </td>
                    <td>
                        <%#Eval("Increasing")%>
                    </td>
                     <td>
                         <%#Eval("Commission")%>
                    </td>
                    <td>
                         <%#Eval("TradeFee")%>
                    </td>
                   
                    <td>
                         <%#Eval("Anticipation")%>
                    </td>
                    <td>
                        <%#Eval("PurchaserName")%>
                    </td>
                    <td>
                      <%#Eval("IncomeGroupName") %>
                    </td>
                    <td>
                        <%#Eval("Id")%>
                    </td>
                    <td>
                         <%#Eval("PayTime")%>
                    </td>
                    <td>
                         <%#Eval("ETDZTime")%>
                    </td>
                     <td>
                       <%#Eval("RefundTime")%>
                     </td>
                    <td>
                         <%#Eval("IsPoolpay")%>
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
                        <%=totalTradeRoyalty%>
                    </td>
                    <td>
                       <%=totalTradeFee%>
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
    <asp:HiddenField ID="hfdCompanyId" runat="server" />
    <asp:HiddenField ID="hfdCompanyType" runat="server" />
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
<script src="../Scripts/Report/Scroll.js?20130104" type="text/javascript"></script>
<script src="../Scripts/Report/RoyaltyProfitReport.js?20130508" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        for (var i = 0; i < $(".uc").length; i++) {
            $(".uc").eq(i).find("div").css("z-index", 5 - parseInt(i));
        }
    });
</script>
