<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FixedNavigation.aspx.cs" Inherits="ChinaPay.B3B.MaintenanceWeb.BasicData.FixedNavigation" %>
<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<%@ Register Src="~/UserControl/Airport.ascx" TagName="Airport" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>非固定航线</title>
    <link rel="stylesheet" type="text/css" href="../css/style.css" />
</head>
<body>
   <form id="form1" runat="server">
    <div class="contents">
        <div class="breadcrumbs">
            <span>当前位置:</span><span>基础数据管理</span>»<span>非固定航线维护</span>
        </div>
        <div class="title">
            <dl>
                <dt>
                    <img src="../images/icon.png" alt=""/>&nbsp;查询条件</dt>
                <dd class="searchbk">
                    <table width="97%" border="0" cellpadding="1" cellspacing="1" class="search">
                        <tr>
                            <th>出发地：</th>
                            <td>
                                <uc:Airport runat="server" ID="ucDepartures"/>
                            </td>
                            <th>到底地：</th>
                            <td>
                               <uc:Airport runat="server" ID="ucArrivals" />
                            </td>
                            <td colspan="2">
                                <asp:Button ID="btnSelect" runat="server" Text="查询" CssClass="button" OnClick="btnSelect_Click"  />
                            </td>
                        </tr>
                    </table>
                </dd>
            </dl>
        </div>
        <div class="title">
            <dl>
                <dt>
                    <img src="../images/icon.png" alt=""/>&nbsp;相应列表</dt>
                <dd>
                    <asp:GridView ID="gvFixedNavigation" runat="server"  EmptyDataText="无满足条件的数据!"
                        AutoGenerateColumns="False"  CssClass="tab3 list" onrowdeleting="gvFixedNavigation_RowDeleting" 
                         >
                        <Columns>
                            <asp:BoundField DataField="Departure" HeaderText="出发地" />
                            <asp:BoundField DataField="Arrival" HeaderText="到达地" />
                            <asp:TemplateField HeaderText="操作">
                                <ItemTemplate>
                             <asp:LinkButton ID="linkDel" runat="server" CommandName="Delete" CommandArgument='<%# Eval("Departure")%>'  Text="删除"  OnClientClick= 'return confirm("您确定要删除吗？")'></asp:LinkButton>
                             <asp:Label runat="server" ID="lblArrival" Visible="false" Text='<%#Eval("Arrival") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle CssClass="altrow" />
                        <HeaderStyle CssClass="searchListTitle" />
                    </asp:GridView>
                </dd>
            </dl>
            <div class="wpager">
                <div class="wpageright"><uc:Pager ID="Pager" runat="server"/></div>
                <div class="clear"></div> 
            </div>
        </div>
        <div class="title">
            <dl>
                <dd class="searchbk">
                    <table width="97%" cellspacing="1" cellpadding="1" border="0" class="search">
                        <tbody>
                            <tr>
                                <th>
                                   出发地：
                                </th>
                                <td>
                                  <uc:Airport runat="server" ID="ucDeparture" />
                                </td>
                                <th>
                                    到达地：
                                </th>
                                <td>
                                    <uc:Airport  runat="server" ID="ucArrival"/>
                                </td>
                                <td colspan="2">
                                  <asp:Button ID="btnSave"  runat="server" CssClass="button" Text="保存" 
                                        onclick="btnSave_Click"/>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </dd>
            </dl>
        </div>
    </div>
    <div class="clear"></div>
    </form>
</body>
</html>
<script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
<script src="../Scripts/selector.js" type="text/javascript"></script>
<script src="../Scripts/airport.js" type="text/javascript"></script>
