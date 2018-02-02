<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BasePrice.aspx.cs" EnableSessionState="ReadOnly" Inherits="ChinaPay.B3B.MaintenanceWeb.BasicData.BasePrice" %>
<%@ Register src="../UserControl/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=7" />
    <!--让IE8解释和IE7一样，不可删-->
    <title>基础运价维护</title>
    <link rel="stylesheet" type="text/css" href="../css/style.css" />
</head>
<body>
    <form id="Form1" runat="server">
    <div class="contents">
      <div class="breadcrumbs">
        <span>当前位置:</span><span>基础数据管理</span>&raquo;<span>基础运价维护</span>
    </div>
        <div class="title">
            <dl>
                <dt>
                    <img src="../images/icon.png"/>&nbsp;查询条件</dt>
                <dd class="searchbk">
                    <table width="97%" border="0" cellpadding="1" cellspacing="1" class="search condition">
                        <tr>
                            <th>
                                航空公司：
                            </th>
                            <td>
                               <asp:DropDownList ID="ddlAirline" runat="server" CssClass="input2"></asp:DropDownList>
                            </td>
                            <th>
                                出发机场：
                            </th>
                            <td>
                                <asp:DropDownList ID="drpDepartAirport" runat="server" CssClass="input2"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                 到达机场：
                            </th>
                            <td colspan="3">
                                <asp:DropDownList ID="drpArrivedAirport" runat="server" CssClass="input2">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr class="operator">
                            <td colspan="6">
                                 <asp:Button ID="btnRefresh" runat="server" Text="刷新缓存" CssClass="button"  Width="120px"
                                    onclick="btnRefresh_Click" />&nbsp;
                                <asp:Button ID="btnSelect" runat="server" Text="查询" CssClass="button" onclick="btnSelect_Click" OnClientClick="SaveSearchCondition('BasePrice')" />&nbsp;
                                <input type="button" name="button3" value="添加" class="button" onclick="javascript:window.location.href='BasePrice_new.aspx?action=add'" />
                            </td>
                        </tr>
                    </table>
                </dd>
            </dl>
        </div>
        <div class="title">
            <dl>
                <dt>
                     <img src="../images/icon.png"/>&nbsp;相应列表
                </dt>
                <dd>
                    <asp:GridView ID="gvBasePrice" runat="server"  
                        AutoGenerateColumns="False" EmptyDataText="无满足条件的数据!"
                        CssClass="tab3 list" onrowdeleting="gvBasePrice_RowDeleting"  >
                        <Columns>
                            <asp:TemplateField HeaderText="航空公司代码">
                                <ItemTemplate>
                                    <asp:Label ID="labAirlineCode"  runat="server" Text='<%# Eval("AirlineCode")%>'  Font-Bold="false"  CssClass="fontColor01"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="航空公司简称">
                                <ItemTemplate>
                                    <asp:Label ID="labAirlineName"  runat="server" Text='<%# Eval("AirlineShortName")%>'  Font-Bold="false"  CssClass="fontColor01"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="出发机场简称">
                                <ItemTemplate>
                                    <asp:Label ID="lblDepartArilineName" runat="server" Font-Bold="false" CssClass="fontColor01" Text='<%#Eval("DepartureShortName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="到达机场简称">
                                <ItemTemplate>
                                    <asp:Label ID="lblArrivalAirlineName" runat="server" Font-Bold="false" CssClass="fontColor01" Text='<%#Eval("ArrivalShortName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="FlightDate" DataFormatString="{0:yyyy-MM-dd}" HtmlEncode="false" HeaderText="航班日期" />
                            <asp:BoundField DataField="ETDZDate" DataFormatString="{0:yyyy-MM-dd}" HtmlEncode="false" HeaderText="出票日期" />
                            <asp:BoundField DataField="Price" HeaderText="公布价格" />
                            <asp:BoundField DataField="Mileage" HeaderText="里程数" />
                            <asp:TemplateField HeaderText="操作">
                                <ItemTemplate>
                                <a href='BasePrice_new.aspx?action=update&Id=<%#Eval("Id") %>'>修改</a>
                                    <asp:LinkButton ID="linkDel" runat="server" CommandName="Delete" CommandArgument='<%# Eval("Id")%>' Text="删除" OnClientClick='return confirm("确定要删除吗？")'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle CssClass="altrow" />
                        <HeaderStyle CssClass="searchListTitle" />
                    </asp:GridView>
                </dd>
            </dl>
            <div class="wpager">
                <div class="wpageright"><uc1:Pager ID="Pagerl" runat="server"/></div>
                <div class="clear"></div> 
            </div>
        </div>
    </div>
    <div class="clear"></div>
    </form>
  </body>
</html>
<script type="text/javascript" language="javascript" src="/js/jquery.js"></script>
<script src="/Scripts/SaveCondition.js" type="text/javascript"></script>
<script type="text/javascript">
    pageName = "BasePrice";
</script>