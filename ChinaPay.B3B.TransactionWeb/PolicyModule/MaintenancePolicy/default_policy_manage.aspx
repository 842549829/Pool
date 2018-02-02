<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default_policy_manage.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.PolicyModule.MaintenancePolicy.default_policy_manage" %>

<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<%@ Register TagPrefix="uc1" TagName="Company_2" Src="~/UserControl/CompanyC.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>默认政策设置</title>
    <script type="text/javascript" src="/Scripts/jquery-1.7.2.min.js"></script>
</head>    <link rel="stylesheet" href="/Styles/public.css?20130301" />

<body>
    <form id="form1" runat="server" autocomplete="off">
        <h3 class="titleBg">
            普通默认政策</h3>
        <div class="box-a">
            <div class="condition">
                <table>
                    <colgroup>
                        <col class="w35">
                        <col class="w35">
                        <col class="w30">
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
                                <div class="input" style="z-index:3;">
                                    <span class="name">成人出票方：</span>
                                    <uc1:Company_2 runat="server" ID="ucAudltProvider" />
                                </div>
                            </td>
                            <td>
                                <div class="input">
                                    <span class="name">儿童出票方：</span>
                                    <uc1:Company_2 runat="server" ID="ucChildProvider" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="btns" colspan="3">
                            <div style="z-index:2;">
                                <asp:Button ID="btnQuery" runat="server" Text="查询" CssClass="btn class1" OnClick="btnQuery_Click" />
                                <input type="button" value="添加" class="btn class1" onclick="javascript:window.location.href='default_policy_addormodify.aspx'" /></div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div id="Div1" class="table">
                <div>
                </div>
            </div>
        </div>
        <div class="table">
        <asp:GridView ID="dataSource" runat="server" AutoGenerateColumns="False" EnableViewState="False">
            <Columns>
                <asp:BoundField HeaderText="航空公司" DataField="Airline" />
                <asp:BoundField HeaderText="成人默认出票方" DataField="AdultProviderAbbreviateName" />
                <asp:BoundField HeaderText="成人默认佣金" DataField="AdultCommission" />
                <asp:BoundField HeaderText="儿童默认出票方" DataField="ChildProviderAbbreviateName" />
                <asp:BoundField HeaderText="儿童默认佣金" DataField="ChildCommission" />
                <asp:TemplateField HeaderText="操作">
                    <ItemTemplate>
                        <a href='default_policy_addormodify.aspx?airline=<%#Eval("Airline") %>'>修改</a>
                        <%--                            <asp:LinkButton ID="lnkDel" runat="server" CommandName="del" CommandArgument='<%#Eval("Airline") %>' OnClientClick='return confirm("您确定要删除吗？")'>删除</asp:LinkButton>--%>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        </div>
        <div class="box" runat="server" visible="false" id="emptyDataInfo">
            没有任何符合条件的查询结果
        </div>
    <div class="btns">
        <uc:Pager runat="server" ID="pager" Visible="false"></uc:Pager>
    </div>
    </form>
</body>
</html>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="/Scripts/selector.js" type="text/javascript"></script>
