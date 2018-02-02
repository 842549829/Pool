<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RefundChangeRuleList.aspx.cs" Inherits="ChinaPay.B3B.MaintenanceWeb.BasicData.RefundChangeRuleList" %>
<%@ Register src="../UserControl/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=7" />
    <!--让IE8解释和IE7一样，不可删-->
    <title>航空公司退改签约定查询</title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="contents">
        <div class="breadcrumbs">
            <span>当前位置:</span><span>基础数据管理</span>&raquo;<span>航空公司退改签约定查询</span>
        </div>
        <div class="title">
            <dl>
                <dt>
                    <img src="../images/icon.png"/>&nbsp;查询条件</dt>
                <dd class="searchbk">
                    <table width="97%" class="search">
                        <tr>
                            <th>
                                航空公司：
                            </th>
                            <td>
                                <asp:DropDownList ID="ddlAirline" CssClass="input2" runat="server" AppendDataBoundItems="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                               &nbsp;
                            </td>
                            <td>
                               <asp:Button ID="btnSelect" runat="server" Text="查询" CssClass="button" onclick="btnSelect_Click" />
                                <input type="button" value="添加" class="button" onclick="location.href='/BasicData/RefundChangeTicketNewUpdate.aspx?action=add'" />
                            </td>
                        </tr>
                        <tr class="operator">
                            <td colspan="6">
                                
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
                        CssClass="tab3 list" EmptyDataText="无满足条件的数据!" >
                        <Columns>
                            <asp:TemplateField HeaderText="航空公司代码">
                                <ItemTemplate>
                                    <asp:Label ID="labAirline" runat="server" Text='<%# Eval("Carrier")%>' Font-Bold="false"
                                        CssClass="fontColor01"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                           
                            <asp:BoundField DataField="CarrierName" HeaderText="航空公司名称" />
                            <asp:BoundField DataField="HasRules" HeaderText="基础信息" />
                            <asp:BoundField DataField="RulesCount" DataFormatString="已添加{0}条" 
                                HeaderText="详细信息" />
                           
                            <asp:TemplateField HeaderText="操作">
                                <ItemTemplate>
                                    <a href='RefundChangeRuleDetail.aspx?Code=<%#Eval("Carrier") %>'>查看详情</a>
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
