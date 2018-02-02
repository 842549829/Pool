<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="base_policy_view.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.PolicyModule.TransactionPolicy.base_policy_view" %>

<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<%@ Register Src="~/UserControl/Airport.ascx" TagName="City" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>用户普通政策对比</title>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
    <link rel="stylesheet" href="/Styles/icon/fontello.css" />
    <link href="/Styles/tipTip.css" rel="stylesheet" type="text/css" />
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        普通政策对比</h3>
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
                            <div class="input radio_reset">
                                <span class="name">行程类型：</span>
                                <asp:DropDownList ID="ddlVoyageType" runat="server">
                                    <asp:ListItem Value="1" Text="单程" />
                                    <asp:ListItem Value="2" Text="往返" />
                                    <asp:ListItem Value="4" Text="单程/往返" />
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
    <div  id="data-list" class="table">
        <asp:GridView ID="grv_normalLook" runat="server" CssClass="nfo-table info"  AutoGenerateColumns="false"
            Width="100%">
            <Columns>
                <asp:BoundField HeaderText="航空公司" DataField="Airline" />
                <asp:TemplateField HeaderText="出发城市">
                    <ItemTemplate>
                        <span class="Departure">
                            <%#Eval("Departure")%></span></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="中转城市">
                    <ItemTemplate>
                        <span class="Transit">
                            <%#Eval("Transit")%></span></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="到达城市">
                    <ItemTemplate>
                        <span class="Arrival">
                            <%#Eval("Arrival")%></span></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="票证行程">
                    <ItemTemplate>
                        <%#Eval("TicketType")%>
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
                <asp:TemplateField HeaderText="航班限制">
                    <ItemTemplate>
                        <span class="Include">
                            <%#Eval("Include")%></span></ItemTemplate>
                    <ItemStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="排除航段">
                    <ItemTemplate>
                        <span class="ExceptAirways">
                            <%#Eval("ExceptAirways")%></span></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="适用舱位">
                    <ItemTemplate>
                        <span class="Berths">
                            <%#Eval("Berths")%></span></ItemTemplate>
                </asp:TemplateField>
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
    </div>
    <br />
    <div class="btns">
        <uc:Pager runat="server" ID="pager" Visible="false"></uc:Pager>
    </div>
    <div class="box" id="showempty" visible="false" runat="server">
        没有任何符合条件的查询结果</div>
    <asp:HiddenField ID="hidIds" runat="server" />
    <asp:HiddenField ID="hdDefaultDate" runat="server" />
    <a id="divOpcial" style="display: none;" data="{type:'pop',id:'divPolicy'}"></a>
    <div id="divPolicy" class="form layer" style="display: none">
        <h2>
            普通政策修改返佣</h2>
        <table>
            <colgroup>
                <col class="w20" />
                <col class="w30" />
                <col class="w20" />
                <col class="w30" />
            </colgroup>
            <tr>
                <td class="title allowBrotherPurchase">
                    同行返佣
                </td>
                <td class=" allowBrotherPurchase">
                    <asp:TextBox ID="txtProfessionCommission" Width="50px" MaxLength="4" CssClass="text"
                        runat="server"></asp:TextBox>%
                </td>
                <td class="title">
                    下级返佣
                </td>
                <td>
                    <asp:TextBox ID="txtSubordinateCommission" Width="50px" MaxLength="4" CssClass="text"
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
    </form> 
    <script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="/Scripts/selector.js?20121205" type="text/javascript"></script> 
    <script src="/Scripts/widget/common.js" type="text/javascript"></script>
    <script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
    <script src="/Scripts/PolicyModule/CommissionVaildate.js?20121115" type="text/javascript"></script>
    <script src="/Scripts/PolicyModule/polciy_public.js" type="text/javascript"></script>
    <script src="/Scripts/PolicyModule/policy_view_vaildate.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tipTip.minified.js" type="text/javascript"></script>
    <%--<script src="/Scripts/FixTable.js" type="text/javascript"></script>--%>
    <script src="/Scripts/OrderModule/QueryList.js?20121119" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $(".Departure,.Arrival,.Transit,.Berths,.ExceptAirways,.DepartureWeekFilter,.Include,.Exclude").tipTip({ limitLength: 10, maxWidth: "300px" });
            $(".DepartureDateFilter").tipTip({ limitLength: 26, maxWidth: "300px" });
            $("#txtStartTime").click(function () {
                WdatePicker({ skin: 'default', maxDate: '#F{$dp.$D(\'txtEndTime\')||\'2020-10-01\'}' })
            });
            $("#txtEndTime").click(function () {
                WdatePicker({ skin: 'default', minDate: '#F{$dp.$D(\'txtStartTime\')}', maxDate: '2020-10-01' })
            });
            $("#btnSave").click(function () {
                return vaildateCommission(null);
            });
//            if ($("#grv_normalLook tr").length > 0) {
//                FixTable("grv_normalLook", 12, 750);
//            }
            SaveDefaultData();
        });
        //基础政策
        function ModifyCommissionBase(id, InternalCommission, SubordinateCommission, ProfessionCommission, canHaveSubordinate, allowBrotherPurchase) {
            $("#hidIds").val(id);
            $("#txtInternalCommission").val(InternalCommission);
            $("#txtSubordinateCommission").val(SubordinateCommission);
            $("#txtProfessionCommission").val(ProfessionCommission);
            $("#divOpcial").click();
            if (canHaveSubordinate == "False") {
                $(".canHaveSubordinate").hide();
            }
            if (allowBrotherPurchase == "False") {
                $(".allowBrotherPurchase").hide();
            }
        }
    </script>
</body>
</html>
