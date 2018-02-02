<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RefundChangeTicket.aspx.cs" Inherits="ChinaPay.B3B.MaintenanceWeb.BasicData.RefundChangeTicket" %>
<%@ Register src="../UserControl/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=7" />
    <!--让IE8解释和IE7一样，不可删-->
    <title>退改签规定维护</title>
</head>
<body>
    <div class="contents">
        <div class="breadcrumbs">
            <span>当前位置:</span><span>基础数据管理</span>&raquo;<span>退改签规定维护</span>
        </div>
        <form id="Form1" runat="server" onsubmit="XHCheck.Validate(this);">
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
                                <asp:DropDownList ID="ddlAirline" runat="server" CssClass="input2"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr class="operator">
                            <td colspan="2">
                                <asp:Button ID="btnSelect" runat="server" Text="查询" CssClass="button" onclick="btnSelect_Click"/>&nbsp;
                                <input type="button" name="button3" value="添加" class="button" onclick="javascript:window.location.href='RefundChangeTicketUpdate.aspx?action=add'" />
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
                <div class="gundong">
                    <asp:GridView ID="gvRefundChangeTecket" runat="server" EmptyDataText="无满足条件的数据!"
                        AutoGenerateColumns="False" CssClass="list tab3" 
                        onrowdeleting="gvRefundChangeTecket_RowDeleting">
                        <Columns>
                            <asp:TemplateField HeaderText="航空公司代码">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_ID" runat="server" Text='<%# Eval("AirlineCode.Value") %>' Font-Bold="false" CssClass="fontColor01"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="航空公司简称">
                                <ItemTemplate>
                                    <asp:Label ID="labAirline" runat="server" Text='<%# Eval("Airline.ShortName")%>' Font-Bold="false" CssClass="fontColor01"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="退票规定">
                                <ItemTemplate>
                                    <div class="epty"><%# Eval("Refund")%></div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="废票规定">
                                <ItemTemplate>
                                    <div class="epty"><%# Eval("Scrap")%></div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="更改规定">
                                <ItemTemplate>
                                    <div class="epty"><%# Eval("Change")%></div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="AirlineTel" HeaderText="航空公司电话" />
                            <asp:TemplateField HeaderText="备注">
                                <ItemTemplate>
                                    <div class="epty"><%# Eval("Remark")%></div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Level" HeaderText="排序值" />
                            <asp:TemplateField HeaderText="操作">
                                <ItemTemplate>
                                    <a href='RefundChangeTicketUpdate.aspx?action=update&code=<%#Eval("AirlineCode.Value") %>'>修改</a>
                                    <asp:LinkButton ID="linkDel" runat="server" CommandName="Delete" CommandArgument='<%# Eval("AirlineCode.Value")%>'
                                        Text="删除" OnClientClick='return confirm("确定要删除吗？")'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle CssClass="altrow" />
                        <HeaderStyle CssClass="searchListTitle" />
                    </asp:GridView>
                    </div>
                </dd>
            </dl><div class="wpager">
                <div class="wpageright"><uc1:Pager ID="Pager1" runat="server"/></div>
                <div class="clear"></div> 
            </div>
        </div>
    </form>
    </div>
    <div class="clear"></div>
</body>
</html>
<style type="text/css">
    .epty
    {
        width:80px;
        white-space:nowrap;
        text-overflow: ellipsis;
        -o-text-overflow: ellipsis;
        overflow: hidden;
    }
</style>
<link rel="stylesheet" type="text/css" href="../css/style.css" />
<script type="text/javascript" language="javascript" src="../js/jquery.js"></script>
<script type="text/javascript" language="javascript" src="../js/Check.js"></script>