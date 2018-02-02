<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Check_In.aspx.cs" Inherits="ChinaPay.B3B.MaintenanceWeb.BasicData.Check_In" %>
<%@ Register src="../UserControl/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
<%@ Register Src="~/UserControl/Ariline.ascx" TagName="Ariline" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>值机</title>
    <link rel="stylesheet" type="text/css" href="../css/style.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="contents">
        <div class="breadcrumbs">
            <span>当前位置:</span><span>基础数据管理</span>»<span>值机维护</span>
        </div>
        <div class="title">
            <dl>
                <dt>
                    <img src="../images/icon.png" />&nbsp;查询条件</dt>
                <dd class="searchbk">
                    <table width="97%" border="0" cellpadding="1" cellspacing="1" class="search condition">
                        <tr>
                            <th>
                                航空公司代码：
                            </th>
                            <td>
                                <uc:Ariline runat="server" ID="ucAriline" ClientIDMode="Static" />
                            </td>
                            <th>
                                时间：
                            </th>
                            <td>
                                <asp:TextBox ID="txtStratDate" CssClass="input1" runat="server" onfocus="WdatePicker({isShowClear:true,readOnly:true})"></asp:TextBox>至
                                <asp:TextBox ID="txtEndDate" CssClass="input1" runat="server" onfocus="WdatePicker({isShowClear:true,readOnly:true})"></asp:TextBox>
                            </td>
                            <th>
                                操作员：
                            </th>
                            <td>
                                <asp:TextBox ID="txtOperator" CssClass="input1" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr class="operator">
                            <td colspan="6">
                                <asp:Button ID="btnSelect" runat="server" Text="查询" CssClass="button" OnClick="btnSelect_Click" />&nbsp;
                                <input type="button" name="button3" value="添加" class="button" onclick="javascript:window.location.href='Check_In_New.aspx?action=add'" />
                            </td>
                        </tr>
                    </table>
                </dd>
            </dl>
        </div>
        <div class="title">
            <dl>
                <dt>
                    <img src="/images/icon.png" />&nbsp;相应列表</dt>
                <dd>
                    <asp:GridView ID="gvCheck_In" runat="server" EmptyDataText="无满足条件的数据!" AutoGenerateColumns="False" onrowdeleting="gvAirline_RowDeleting"
                        CssClass="tab3 list">
                        <Columns>
                            <asp:BoundField  DataField="AirlineName" HeaderText="航空公司" />
                            <asp:BoundField  DataField="OperatingHref" HeaderText="链接操作" />
                            <asp:BoundField  DataField="Opertor" HeaderText="操作员" />
                            <asp:BoundField  DataField="Time" HeaderText="时间" />
                            <asp:BoundField  DataField="Remark" HeaderText="备注" />
                            <asp:TemplateField HeaderText="操作">
                                <ItemTemplate>
                                    <a href="Check_In_New.aspx?action=upate&Id=<%# Eval("Id") %>">修改</a>
                                    <asp:LinkButton ID="linkDel" runat="server" CommandName="Delete" CommandArgument='<%# Eval("Id")%>'
                                        Text="删除" OnClientClick='return confirm("您确定要删除吗？")'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle CssClass="altrow" />
                        <HeaderStyle CssClass="searchListTitle" />
                    </asp:GridView>
                </dd>
            </dl>
            <div class="wpager">
                <div class="wpageright">
                    <uc1:pager id="Pagerl" runat="server" />
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
<script type="text/javascript" language="javascript" src="/js/jquery.js"></script>
<script src="../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="/Scripts/SaveCondition.js" type="text/javascript"></script>
<script src="/Scripts/selector.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">
    pageName = "Check_In";
</script>