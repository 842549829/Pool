<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="special_policy_view.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.PolicyModule.TransactionPolicy.special_policy_view" %>

<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<%@ Register Src="~/UserControl/Airport.ascx" TagName="City" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>特殊政策对比</title>
</head>
    <link rel="stylesheet" href="/Styles/icon/fontello.css" />
    <link href="/Styles/tipTip.css" rel="stylesheet" type="text/css" />
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        特殊政策对比</h3>
    <div class="box-a">
        <div class="condition">
            <table>
                <colgroup>
                    <col class="w50" />
                    <col class="w50" />
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
                    </tr>
                    <tr>
                        <td>
                            <div class="input">
                                <span class="name">航班日期：</span>
                               <asp:TextBox ID="txtStartTime" runat="server" CssClass="text" onclick="WdatePicker({ readOnly:true, isShowClear: false })" ></asp:TextBox>
                                <%--<asp:TextBox ID="txtEndTime" runat="server" CssClass="text text-s" onclick="WdatePicker({isShowClear:false, readOnly:true, minDate: '#F{$dp.$D(\'txtStartTime\')}', maxDate: '2020-10-01' })" ></asp:TextBox>--%>
                            </div>
                        </td>
                        <td>
                            <div class="input">
                                <span class="name">到达城市：</span>
                                <uc:City runat="server" ID="txtArrival"></uc:City>
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
    <div id="data-list"  class="table">
        <asp:GridView ID="grv_specical" runat="server" CssClass="nfo-table info" AutoGenerateColumns="false">
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
                <asp:BoundField HeaderText="排除日期" DataField="DepartureDateFilter" />
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
                <asp:BoundField HeaderText="提前天数" DataField="BeforehandDays" />
                <asp:BoundField HeaderText="发布价格" DataField="Price" />
                <asp:TemplateField HeaderText="航班日期">
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
        <div class="box" id="showempty" visible="false" runat="server">没有任何符合条件的查询结果</div>
    </div>
    <asp:HiddenField ID="hidIds" runat="server" />
    <asp:HiddenField ID="hdDefaultDate" runat="server" />
    <a id="divOpcial" style="display: none;" data="{type:'pop',id:'divPolicy'}"></a>
    <div id="divPolicy" class="form layer" style="display: none">
        <h2>
            特殊政策修改价格</h2>
         <table>
            <colgroup>
                <col class="w20" />
                <col class="w30" />
                <col class="w20" />
                <col class="w30" />
            </colgroup>
            <tr class="priceInfoT">
                <td class="title">
                    <input type="radio" id="radPrice" runat="server" name="pricetype" value="0" />
                    <label for="radPrice">
                        价格：</label><br />
                    <input type="radio" id="radLapse" runat="server" class="subString" name="pricetype" value="2" />
                    <label for="radLapse" class="subString">
                        直减：</label>
                </td>
                <td>
                    <asp:Label ID="lblSign" CssClass="priceInfo" runat="server">￥</asp:Label>
                    <asp:TextBox ID="txtPrice" Width="50px" MaxLength="5" CssClass="text priceInfo" runat="server"></asp:TextBox><br />
                    <asp:TextBox ID="txtLapse" Width="50px" MaxLength="4" CssClass="text discount subString" runat="server"></asp:TextBox><asp:Label
                        ID="lblDiscount" CssClass="discount subString" runat="server">%</asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title subordinate">
                    下级返佣
                </td>
                <td class="subordinate">
                    <asp:TextBox ID="txtSubordinateCommission" Width="50px" MaxLength="4" CssClass="text"
                        runat="server"></asp:TextBox><asp:Label ID="lblSubordernate" CssClass="sign" runat="server">%</asp:Label>
                </td>
                <td class="title  allowBrotherPurchase">
                    同行返佣
                </td>
                <td class="allowBrotherPurchase">
                    <asp:TextBox ID="txtProfessionCommission" Width="50px" MaxLength="4" CssClass="text"
                        runat="server"></asp:TextBox><asp:Label ID="lblProffession" CssClass="sign" runat="server">%</asp:Label>
                </td>
            </tr>
            <tr id="commission">
                <td class="title canHaveSubordinate">
                    内部返佣
                </td>
                <td colspan="3" class="canHaveSubordinate">
                    <asp:TextBox ID="txtInternalCommission" Width="50px" MaxLength="4" CssClass="text"
                        runat="server"></asp:TextBox><asp:Label ID="Label1" CssClass="sign" runat="server">%</asp:Label>
                </td>
            </tr>
            <tr class="btns">
                <td colspan="2">
                    <asp:Button ID="btnSave" CssClass="btn class1" runat="server" Text="保&nbsp;&nbsp;&nbsp;存"
                        OnClick="btnSave_Click" />
                    <label class="close btn class2" title="关闭">
                        关闭</label>
                </td>
            </tr>
        </table>
    </div>
        <asp:HiddenField runat="server" ID="hfdSpecialType" />
    <asp:HiddenField runat="server" ID="hfdIsInternal" />
    <asp:HiddenField runat="server" ID="hfdIsPeer" />
    </form>
    <script src="/Scripts/DatePicker/WdatePicker.js?20121101" type="text/javascript"></script>
    <script src="/Scripts/selector.js?20121205?20121101" type="text/javascript"></script>
    <script src="/Scripts/widget/common.js?20121101" type="text/javascript"></script>
    <script src="/Scripts/Global.js?20121118" type="text/javascript?20121101"></script>
    <script src="/Scripts/PolicyModule/CommissionVaildate.js?20121101" type="text/javascript"></script>
    <script src="/Scripts/PolicyModule/polciy_public.js?20121101" type="text/javascript"></script>
    <script src="/Scripts/PolicyModule/policy_view_vaildate.js?20121101" type="text/javascript"></script>
    <script src="/Scripts/jquery.tipTip.minified.js?20121101" type="text/javascript"></script>
    <script src="/Scripts/OrderModule/QueryList.js?20121119?20121101" type="text/javascript"></script>
    <script src="/Scripts/PolicyModule/SpecialPolicy.js?20121101" type="text/javascript"></script>
<%--    <script type="text/javascript">
        $(function () {
            $(".DepartureWeekFilter,.Include,.Exclude").tipTip({ limitLength: 10, maxWidth: "300px" });
            $("#btnSave").click(function () {
                return vaildateCommission(null);
            });
            SaveDefaultData();
        });
        //特殊政策
        function ModifyCommission(id, price) {
            $("#hidIds").val(id);
            $("#txtPrice").val(price);
            $("#divOpcial").click();
        }
    </script>--%>
</body>
</html>
