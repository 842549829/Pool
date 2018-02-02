<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChildTicketMaintain.aspx.cs" Inherits="ChinaPay.B3B.MaintenanceWeb.BasicData.ChildTicketMaintain" %>
<%@ Register src="../UserControl/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=7" />
    <!--让IE8解释和IE7一样，不可删-->
    <title>儿童可预订舱位维护</title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="contents">
        <div class="breadcrumbs">
            <span>当前位置:</span><span>基础数据管理</span>&raquo;<span>儿童票舱位维护</span>
        </div>
        <div class="title">
            <dl>
                <dt>
                    <img src="../images/icon.png"/>&nbsp;查询条件</dt>
                <dd class="searchbk">
                    <table width="97%" border="0" cellpadding="1" cellspacing="1" class="search">
                        <tr>
                            <th>
                                航空公司：
                            </th>
                            <td>
                                <asp:DropDownList ID="ddlAirline" CssClass="input2" runat="server" AppendDataBoundItems="true">
                                </asp:DropDownList>
                            </td>
                            <th>
                                舱&nbsp;&nbsp;位：
                            </th>
                            <td>
                                <asp:TextBox ID="txtCwCode" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr class="operator">
                            <td colspan="6">
                                <asp:Button ID="btnSelect" runat="server" Text="查询" CssClass="button" 
                                    onclick="btnSelect_Click" />&nbsp;
                                <input type="button" onclick="javascript:window.location.href='ChildTicketClass_Add.aspx?action=add'"
                        class="button" value="添加" name="button3" />
                            </td>
                        </tr>
                    </table>
                </dd>
            </dl>
        </div>
        <div class="title">
            <dl>
                <dt>
                    <img src="../images/icon.png"/>&nbsp;相应列表</dt>
                <dd>
                    <asp:GridView ID="gvChildTicketClassInfo" runat="server" AutoGenerateColumns="False"
                        CssClass="tab3 list" EmptyDataText="无满足条件的数据!" onrowdeleting="gvChildTicketClassInfo_RowDeleting">
                        <Columns>
                            <asp:TemplateField HeaderText="航空公司代码">
                                <ItemTemplate>
                                    <asp:Label ID="labAirline" runat="server" Text='<%# Eval("AirlineCode.Value")%>' Font-Bold="false"
                                        CssClass="fontColor01"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="舱位">
                                <ItemTemplate>
                                    <asp:Label ID="lanBunkName" runat="server" Text='<%# Eval("BunkCode.Value")%>' Font-Bold="false"
                                        CssClass="fontColor01"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="折扣">
                                <ItemTemplate>
                                    <asp:Label ID="labDiscount" runat="server" Text='<%# CountDiscount(Eval("Discount"))%>' Font-Bold="false"
                                        CssClass="fontColor01"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="操作">
                                <ItemTemplate>
                                    <a href='ChildTicketClass_Add.aspx?action=update&Id=<%#Eval("Id") %>'>修改</a>
                                    <asp:LinkButton ID="linkDel" runat="server" CommandName="Delete" CommandArgument='<%# Eval("Id")%>'
                                        Text="删除" OnClientClick='return confirm("确定要删除吗?")'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle CssClass="altrow" />
                        <HeaderStyle CssClass="searchListTitle" />
                    </asp:GridView>
                </dd>
            </dl>
            <div class="wpager">
                <div class="wpageright"><uc1:Pager ID="Pager1" runat="server"/></div>
                <div class="clear"></div> 
            </div>
        </div>
    </div>
    <div class="clear"></div>
    </form>
</body>
</html>
<link rel="stylesheet" type="text/css" href="../css/style.css" />
<script type="text/javascript" language="javascript" src="../js/jquery.js"></script>
<script type="text/javascript" language="javascript" src="../js/forms.js"></script>
<script language="javascript" type="text/javascript">
    function btnSubmit() {
        var regu = /^[A-Za-z]{1,2}$/;
        var code = $("#txtCwCode").val();
        if (code != "" && !(code.length == 1 || code.length == 2) && !($.trim(code).match(regu))) {
            alert("舱位格式不正确！");
            return false;
        }
    }  
</script>