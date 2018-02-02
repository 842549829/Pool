<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="low_price_policy_view.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.PolicyModule.TransactionPolicy.low_price_policy_view" %>

<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<%@ Register Src="~/UserControl/Airport.ascx" TagName="City" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>特价政策对比</title>
 </head>
   <link href="/Styles/icon/fontello.css" rel="stylesheet" />
    <link href="/Styles/tipTip.css" rel="stylesheet" type="text/css" />
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        特价政策对比</h3>
    <div class="box-a">
        <div class="condition">
            <table>
                <colgroup>
                    <col class="w35" />
                    <col class="w35" />
                    <col class="w30" />
                </colgroup>
                <tbody>
                    <tr>
                        <td>
                            <div class="input">
                                <span class="name">航空公司：</span>
                                <asp:DropDownList ID="ddlAirline" runat="server">
                                </asp:DropDownList>
                            </div>
                        </td>
                         <td>
                            <div class="input">
                                <span class="name">出发城市：</span>
                                <uc:City runat="server" ID="txtDeparture"></uc:City>
                            </div>
                        </td>
                        <td>
                            <div class="input radio_reset">
                                <span class="name">票证类型：</span>
                                <asp:DropDownList ID="ddlTicketType" runat="server">
                                  <asp:ListItem Value="" Text="全部"></asp:ListItem>
                                   <asp:ListItem Value="0" Text="B2B"></asp:ListItem>
                                  <asp:ListItem Value="1" Text="BSP"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </td>
                    </tr>
                    <tr>
                    <td>
                            <div class="input">
                                <span class="name">航班日期：</span>
                                <asp:TextBox ID="txtStartTime" runat="server" CssClass="text" onclick="WdatePicker({ readOnly:true, isShowClear: false })" ></asp:TextBox>
                            </div>
                        </td>
                        <td>
                            <div class="input">
                                <span class="name">到达城市：</span>
                                <uc:City runat="server" ID="txtArrival"></uc:City>
                            </div>
                        </td>
                        <td>
                          <div class="input">
                          <span class="name">行程类型：</span>
                             <asp:DropDownList ID="ddlVoyageType" runat="server">
                                    <asp:ListItem Value="1" Text="单程" />
                                    <asp:ListItem Value="2" Text="往返" />
                                     <asp:ListItem Value="8" Text="中转联程" />
                                </asp:DropDownList>
                          </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" class="btns">
                            <asp:Button ID="btnQuery" runat="server" CssClass="btn class1" Text="查 询" OnClick="btnQuery_Click" />
                            <input type="button" value="清空条件" class=" btn class2" onclick="ResetSearchOption();" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <div id="data-list" class="table">
        <asp:GridView ID="grv_bargain" runat="server" CssClass="nfo-table info" Width="100%"
            AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField HeaderText="航空公司" DataField="Airline" />
                <asp:TemplateField HeaderText="出发城市">
                    <ItemTemplate>
                        <span class="Departure">
                            <%#Eval("Departure")%>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="到达城市">
                    <ItemTemplate>
                        <span class="Arrival">
                            <%#Eval("Arrival")%>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="中转城市">
                    <ItemTemplate>
                        <span class="Transit">
                            <%#Eval("Transit")%>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="票证行程">
                    <ItemTemplate>
                        <%#Eval("Ticket")%>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="排除日期">
                   <ItemTemplate>
                    <span class="DepartureDateFilter">
                     <%#Eval("DepartureDateFilter")%>
                    </span>
                   </ItemTemplate>
                 </asp:TemplateField>
                <asp:TemplateField HeaderText="适用班期">
                    <ItemTemplate>
                        <span class="DepartureWeekFilter">
                            <%#Eval("DepartureWeekFilter")%></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="适用航班">
                    <ItemTemplate>
                        <span class="Include">
                            <%#Eval("Include")%></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="提前天数">
                    <ItemTemplate> 
                            <%#Eval("BeforehandDays")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="适用舱位" DataField="Berths" />
                <asp:BoundField HeaderText="价格/折扣" DataField="PriceInfo" />
                <asp:TemplateField HeaderText="政策返佣">
                    <ItemTemplate>
                        <%#Eval("Commission")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="去程日期">
                    <ItemTemplate>
                        <%#Eval("DepartureDates")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作">
                    <ItemTemplate>
                        <%#Eval("Policy_link")%><br />
                        <%#Eval("Commission_link")%>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <br />
        <div class="btns">
            <uc:Pager runat="server" ID="pager" Visible="false"></uc:Pager>
        </div>
        <asp:HiddenField ID="hidIds" runat="server" />
        <asp:HiddenField ID="hdDefaultDate" runat="server" />
    </div>
    <div class="box" id="showempty" visible="false" runat="server">
        没有任何符合条件的查询结果</div>
    <a id="divOpcial" style="display: none;" data="{type:'pop',id:'divPolicy'}"></a>
    <div id="divPolicy" class="form layer" style="display: none">
        <h2>
            特价政策修改返佣</h2>
        <table>
            <colgroup>
                <col class="w20" />
                <col class="w30" />
                <col class="w20" />
                <col class="w30" />
            </colgroup>
            <tr id="price">
                <td class="title">
                    <input type="radio" id="radPrice" runat="server" name="pricetype" value="0" />
                    <label for="radPrice">
                        价格：</label><br />
                    <input type="radio" id="radDiscount" class="discount1" runat="server" name="pricetype" value="1" />
                    <label for="radDiscount" class="discount1">
                        折扣：</label>
                    <input type="radio" id="radCommission" class="commission1" runat="server" name="pricetype"
                        value="3" />
                    <label for="radCommission" class="commission1">
                        按返佣</label>
                </td>
                <td colspan="3">
                    <asp:Label ID="lblSign" CssClass="priceInfo" runat="server">￥</asp:Label><asp:TextBox
                        ID="txtPrice" Width="50px" MaxLength="4" CssClass="text priceInfo" runat="server"></asp:TextBox><br />
                    <asp:TextBox ID="txtDisCount" Width="50px" MaxLength="3" CssClass="text discount discount1"
                        runat="server"></asp:TextBox><asp:Label ID="lblDiscount" CssClass="discount discount1" runat="server">折</asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    下级返佣
                </td>
                <td>
                    <asp:TextBox ID="txtSubordinateCommission" Width="50px" MaxLength="4" CssClass="text"
                        runat="server"></asp:TextBox>%
                </td>
                <td class="title  allowBrotherPurchase">
                    同行返佣
                </td>
                <td class="allowBrotherPurchase">
                    <asp:TextBox ID="txtProfessionCommission" Width="50px" MaxLength="4" CssClass="text"
                        runat="server"></asp:TextBox>%
                </td>
            </tr>
            <tr>
                <td class="title canHaveSubordinate">
                    内部返佣
                </td>
                <td class="canHaveSubordinate">
                    <asp:TextBox ID="txtInternalCommission" Width="50px" MaxLength="4" CssClass="text"
                        runat="server"></asp:TextBox>%
                </td>
                <td class="title">
                </td>
                <td>
                </td>
            </tr>
            <tr class="btns">
                <td colspan="4">
                    <asp:Button ID="btnSave" CssClass="btn class1" runat="server" Text="保&nbsp;&nbsp;&nbsp;存"
                        OnClick="btnSave_Click" />
                    <label class="close btn class2" title="关闭">
                        关闭</label>
                </td>
            </tr>
        </table>
    </div>
      <asp:HiddenField runat="server" ID="hfdVoyageType" />
    <asp:HiddenField runat="server" ID="hfdIsPeer" />
    <asp:HiddenField runat="server" ID="hfdIsInternal" />
    </form>
    <script src="/Scripts/core/jquery.js" type="text/javascript"></script>
    <script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="/Scripts/selector.js?20121205" type="text/javascript"></script>
    <script src="/Scripts/widget/common.js" type="text/javascript"></script>
    <script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
    <script src="/Scripts/PolicyModule/CommissionVaildate.js" type="text/javascript"></script>
    <script src="/Scripts/PolicyModule/polciy_public.js" type="text/javascript"></script>
    <script src="/Scripts/PolicyModule/policy_view_vaildate.js" type="text/javascript"></script>
    <%-- <script src="/Scripts/FixTable.js" type="text/javascript"></script>--%>
    <script src="/Scripts/jquery.tipTip.minified.js?20121114" type="text/javascript"></script>
    <script src="/Scripts/OrderModule/QueryList.js?20121119?20121114" type="text/javascript"></script>
    <script src="/Scripts/PolicyModule/LowPricePolicy.js?2012111401" type="text/javascript"></script>
</body>
</html>
