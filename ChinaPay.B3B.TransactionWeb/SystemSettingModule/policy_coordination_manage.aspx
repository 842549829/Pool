<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="policy_coordination_manage.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.policy_coordination_manage" %>

<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<%@ Register Src="~/UserControl/Airport.ascx" TagName="City" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>政策协调设置</title>
    <link href="/Styles/tipTip.css" rel="stylesheet" type="text/css" />
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <h3 class="titleBg">
            政策协调</h3>
    <div class="box-a">
        <div class="condition">
            <table>
                <colgroup>
                    <col class="w35" />
                    <col class="w25" />
                    <col class="w45" />
                </colgroup>
                <tbody>
                    <tr>
                        <td>
                            <div class="input">
                                <span class="name">出发城市：</span>
                                <uc:City runat="server" ID="txtDeparture"></uc:City>
                            </div>
                        </td>
                        <td>
                            <div class="input">
                                <span class="name">航空公司：</span>
                                <asp:DropDownList ID="ddlAirline" runat="server">
                                </asp:DropDownList>
                            </div>
                        </td>
                         <td>
                            <div class="input">
                                <span class="name">航班日期：</span>
                                <asp:TextBox ID="txtTimeStart" runat="server" CssClass="text text-s"></asp:TextBox>
                                <span class="fl-l">至</span>
                                <asp:TextBox ID="txtTimeEnd" runat="server" CssClass="text text-s"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="input">
                                <span class="name">到达城市：</span>
                                <uc:City ID="txtArrival" runat="server"></uc:City>
                            </div>
                        </td>
                        <td>
                            <div class="input">
                                <span class="name">政策类型：</span>
                                <asp:DropDownList ID="ddlPolicyType" runat="server">
                                    <asp:ListItem Text="-请选择-" Value="-1" />
                                    <asp:ListItem Text="普通政策" Value="0" />
                                    <asp:ListItem Text="特价政策" Value="1" />
                                </asp:DropDownList>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" class="btns">
                            <asp:Button ID="btnQuery" CssClass="btn class1" runat="server" Text="查 询" OnClick="btnQuery_Click" />
                            <asp:Button ID="btnPublish" runat="server" CssClass="btn class2" Text="添 加" OnClick="btnPublish_Click" />
                            <asp:Button ID="btnDel" runat="server" CssClass="btn class2" Text="删 除" OnClick="btnDel_Click" />
                            <input type="button" value="清空条件" class=" btn class2" onclick="ResetSearchOption();" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <div id="data-list"  class="table">
        <asp:GridView ID="grv_back" runat="server" CssClass="info-table" Width="100%" AutoGenerateColumns="false"
            OnRowCommand="grv_back_RowCommand">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <input type="checkbox" class="chkAll" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <input type="checkbox" class="chkOne" id="chk" runat="server" value='<%#Eval("id") %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="航空公司">
                    <ItemTemplate>
                        <span class="AirLine">
                            <%#Eval("AirLine")%></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="出发城市">
                    <ItemTemplate>
                        <span class="Departure">
                            <%#Eval("Departure")%></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="到达城市">
                    <ItemTemplate>
                        <span class="Arrival">
                            <%#Eval("Arrival")%></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="协调值" DataField="HarmonyValue" />
                <asp:TemplateField HeaderText="政策类型">
                    <ItemTemplate>
                        <span class="PolicyType">
                            <%#Eval("PolicyType")%></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="返佣类型" DataField="CommissionType" />
                <asp:TemplateField HeaderText="航班日期">
                    <ItemTemplate>
                        <%#Eval("TimeInfo")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="修改时间" DataField="LastModifyTime" />
                <asp:BoundField HeaderText="修改人" DataField="LastModifyName" />
                <asp:TemplateField HeaderText="备注">
                    <ItemTemplate>
                        <span class="Remark">
                            <%#Eval("Remark")%></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作">
                    <ItemTemplate>
                        <a href='policy_coordination_addModify.aspx?id=<%#Eval("id") %>'>修改</a><br />
                        <asp:LinkButton Text="删除" CommandArgument='<%#Eval("id") %>' class="del_click" CommandName="del"
                            runat="server" />
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
    </form>
    <script src="/Scripts/checkboxchooice.js" type="text/javascript"></script>
    <script src="/Scripts/PolicyModule/City_ShowOrHidden.js" type="text/javascript"></script>
    <script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="/Scripts/selector.js?20121205" type="text/javascript"></script>
    <script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
    <script src="/Scripts/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script src="/Scripts/OrderModule/QueryList.js?20121119" type="text/javascript"></script>
    <script src="/Scripts/FixTable.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $(".AirLine,.Departure,.Arrival,.Remark").tipTip({ limitLength: 6, maxWidth: "300px" });

            $("#txtTimeStart").focus(function () {
                WdatePicker({ isShowClear: true, readOnly: true, maxDate: '#F{$dp.$D(\'txtTimeEnd\')||\'2020-10-01\'}' });
            });
            $("#txtTimeEnd").focus(function () {
                WdatePicker({ isShowClear: true, readOnly: true, minDate: '#F{$dp.$D(\'txtTimeStart\')}', maxDate: '2020-10-01' });
            });
            $(".del_click").click(function () {
                return confirm("是否删除？");
            });
            $("#btnDel").click(function () {
                if ($("#hidIds").val() == "") {
                    alert("没有选中任何行，执行被取消。");
                    return false;
                }
                return confirm("是否删除选中的 [ " + $("#hidIds").val().split(',').length + " ] 条数据？");
            });
            if ($("#grv_back tr").length > 0) {
                FixTable("grv_back", 13, 100);
            }
            SaveDefaultData();
        });
    </script>
</body>
</html>
