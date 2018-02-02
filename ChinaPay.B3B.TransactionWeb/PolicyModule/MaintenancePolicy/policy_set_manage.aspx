<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="policy_set_manage.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.PolicyModule.MaintenancePolicy.policy_set_manage" %>

<%@ Register Src="~/UserControl/Airport.ascx" TagName="City" TagPrefix="uc" %>
<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>政策设置管理</title>
</head>
    <link href="/Styles/public.css?20130301" rel="stylesheet" type="text/css" />
    <link href="/Styles/icon/fontello.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/tipTip.css" rel="stylesheet" type="text/css" />
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        政策设置管理</h3>
    <div class="box-a">
        <div class="condition">
            <table>
                <colgroup>
                    <col class="w35" />
                    <col class="w35" />
                    <col class="w35" />
                </colgroup>
                <tbody>
                    <tr>
                        <td>
                            <div class="input">
                                <span class="name">航空公司：</span>
                                <asp:DropDownList ID="ddlAirlines" runat="server">
                                </asp:DropDownList>
                            </div>
                        </td>
                        <td>
                            <div class="input">
                                <span class="name">航班日期：</span>
                                <asp:TextBox ID="txtStartDate" runat="server" CssClass="datepicker datefrom btn class3 text-s"></asp:TextBox>
                                <span class="fl-l">至</span>
                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="datepicker datefrom btn class3 text-s"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hdDefaultDate" />
                            </div>
                        </td>
                        <td>
                            <div class="input">
                                <span class="name">设置类型：</span>
                                <asp:DropDownList runat="server" ID="dropTieType">
                                    <asp:ListItem Text="所有" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="扣点" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="贴点" Value="-1"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="input">
                                <span class="name">出发城市：</span>
                                <uc:City runat="server" ID="txtDeparture"></uc:City>
                            </div>
                        </td>
                        <td>
                            <div class="input">
                                <span class="name">到达城市：</span>
                                <uc:City runat="server" ID="txtArrival"></uc:City>
                            </div>
                        </td>
                        <td></td>
                    </tr> 
                    <tr>
                        <td colspan="3" class="btns">
                            <asp:Button ID="btnQuery" CssClass="btn class1" Text="查询" runat="server" OnClick="btnQuery_Click" />
                            <input type="button" value="添加" class="btn class2" onclick="javascript:window.location.href='policy_set_addormodify.aspx'" />
                            <asp:Button ID="btnDelete" CssClass="btn class2" Text="删除" runat="server" OnClientClick="return showCheckbox();"
                                OnClick="btnDelete_Click" />
                            <input type="button" class="btn class2" onclick="ResetSearchOption();" value="清空条件" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <div id="data-list" class="table data-scrop">
        <asp:GridView ID="dataSource" runat="server" AutoGenerateColumns="False" CssClass="info-table"
            Width="100%" OnRowCommand="dataSource_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="ID" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblId" runat="server" Text='<%#Eval("Id") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <input id="chkAll" type="checkbox" onclick="checkAll(this);" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkBox" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="航空公司" DataField="Airline" />
                <asp:TemplateField HeaderText="出发城市">
                    <ItemTemplate>
                        <span class="Departure">
                            <%#Eval("Departure")%></span></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="到达城市">
                    <ItemTemplate>
                        <span class="Arrival">
                            <%#Eval("Arrival")%></span></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="设置值">
                    <ItemTemplate>
                        <span class="Commission">
                            <%#Eval("Commission")%></span></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="适用舱位">
                    <ItemTemplate>
                        <span class="Berths">
                            <%#Eval("Berths")%></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="生效日期">
                    <ItemTemplate>
                        <%#Eval("EffectiveTime")%></ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="添加人" DataField="Creator" />
                <asp:BoundField HeaderText="最后修改时间" DataField="LastModifyTime" />
                <asp:TemplateField HeaderText="备注">
                    <ItemTemplate>
                        <span class="Remark">
                            <%#Eval("Remark")%></span></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作">
                    <ItemTemplate>
                        <a href='policy_set_addormodify.aspx?Id=<%#Eval("Id") %>'>修改</a><br />
                        <asp:LinkButton ID="lnkDel" runat="server" CommandArgument='<%#Eval("Id") %>' CommandName="del"
                            OnClientClick='return confirm("你确定要删除吗？")'>删除</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
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
<script src="/Scripts/core/jquery.js" type="text/javascript"></script>
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="/Scripts/checkboxchooice.js" type="text/javascript"></script>
<script src="/Scripts/selector.js?20121205" type="text/javascript"></script>
<script src="/Scripts/jquery.tipTip.minified.js" type="text/javascript"></script>
<script src="/Scripts/OrderModule/QueryList.js?20121119" type="text/javascript"></script>
<%--<script src="/Scripts/FixTable.js" type="text/javascript"></script>--%>
<script type="text/javascript">
    function checkAll(all) {
        var checks = document.getElementsByTagName("input");
        for (var i = 0; i < checks.length; i++) {
            if (checks[i].type == "checkbox") {
                checks[i].checked = all.checked;
            }
        }
    }
    function showCheckbox() {
        var a = $(":checkbox:checked");
        if (a.length < 1) { alert("请先选择需要被删除的数据行！"); return false; }
        var flag = confirm('确认要删除吗？');
        if (flag == false) {
            return false;
        }
    }
    $(function () {
        $(".Departure,.Arrival,.Berths,.Remark,.Commission").tipTip({ limitLength: 3, maxWidth: "330px" });
        $("#txtStartDate").focus(function () {
            WdatePicker({ skin: 'default', isShowClear: false, maxDate: '#F{$dp.$D(\'txtEndDate\')||\'2020-10-01\'}' });
        });
        $("#txtEndDate").focus(function () {
            WdatePicker({ skin: 'default', isShowClear: false, minDate: '#F{$dp.$D(\'txtStartDate\')}', maxDate: '2020-10-01' });
        });
        //        if ($("#dataSource tr").length > 0) {
        //            FixTable("dataSource", 11, 100);
        //        }
        SaveDefaultData();
    });
</script>
