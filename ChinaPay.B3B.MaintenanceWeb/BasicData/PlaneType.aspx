<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PlaneType.aspx.cs" Inherits="ChinaPay.B3B.MaintenanceWeb.BasicData.PlaneType" EnableSessionState="ReadOnly" %>
<%@ Register src="../UserControl/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=7" />
    <!--让IE8解释和IE7一样，不可删-->
    <title>机型代码维护</title>
</head>
<body>
    <div class="contents">
     <div class="breadcrumbs">
        <span>当前位置:</span><span>基础数据管理</span>&raquo;<span>机型代码维护</span>
    </div>
        <form id="Form1" runat="server">
        <div class="title">
            <dl>
                <dt>
                    <img src="../images/icon.png"/>&nbsp;查询条件</dt>
                <dd class="searchbk">
                    <table width="97%" border="0" cellpadding="1" cellspacing="1" class="search condition">
                        <tr>
                            <th>
                                机型代码：
                            </th>
                            <td>
                                <asp:TextBox ID="txtPlaneTypeCode" runat="server" CssClass="input1"></asp:TextBox>
                            </td>
                            <th>
                                状态：
                            </th>
                            <td>
                               <asp:DropDownList ID="ddlStatus" CssClass="input1" runat="server">
                                    <asp:ListItem  Value="-1">不限</asp:ListItem>
                                    <asp:ListItem Value="True">启用</asp:ListItem>
                                    <asp:ListItem Value="False">禁用</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr class="operator">
                            <td colspan="6">
                                <asp:Button ID="btnSelect" runat="server" Text="查询" CssClass="button" 
                                    onclick="btnSelect_Click" />&nbsp;
                                <input type="button" name="button3" value="添加" class="button" onclick="javascript:window.location.href='PlaneType_new.aspx?action=add'" />
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
         
                    <asp:GridView ID="gvPlaneType" runat="server"
                        AutoGenerateColumns="False" CssClass="tab3 list" EmptyDataText="无满足条件的数据!"
                        onrowcommand="gvPlaneType_RowCommand" onrowdeleting="gvPlaneType_RowDeleting">
                        <Columns>  
                           <asp:TemplateField HeaderText="机型代码">
                            <ItemTemplate>
                                <asp:Label ID="labCode" runat="server" Text='<%# Eval("Code.Value")%>'  Font-Bold="false"  CssClass="fontColor01"></asp:Label>
                            </ItemTemplate>
                           </asp:TemplateField>
                            <asp:BoundField DataField="Name" HeaderText="机型名称" />
                            <asp:BoundField  DataField="AirportFee" HeaderText="民航发展基金" />
                            <asp:BoundField DataField="Manufacturer" HeaderText="制造商" />
                            <asp:TemplateField HeaderText="描述信息">
                            <ItemTemplate>
                                <div id="epty" style="width:200px;"> <%# Eval("Description")%></div>
                            </ItemTemplate>
                           </asp:TemplateField>
                            <asp:TemplateField HeaderText="状态">
                                <ItemTemplate>
                                    <asp:Label ID="labValid" runat="server" Text='<%# GetState(Eval("Valid").ToString()) %>'  Font-Bold="false"  CssClass="fontColor01"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="操作">
                                <ItemTemplate> 
                                    <asp:LinkButton ID="linkOprate"  CommandName="opdate"  CommandArgument='<%# Eval("Id")%>' runat="server" Text='<%# ButtonState(Eval("Valid").ToString())%>' OnClientClick='return confirm("确定要修改状态吗?")'></asp:LinkButton>
                                    <a href='PlaneType_new.aspx?action=update&Id=<%#Eval("Id") %>'>修改</a>
                                    <asp:LinkButton ID="linkDel" runat="server" CommandName="Delete"  CommandArgument='<%# Eval("Id")%>' Text="删除" OnClientClick='return confirm("确定要删除吗?")'></asp:LinkButton>                                   
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle CssClass="altrow" />
                        <HeaderStyle CssClass="searchListTitle" />
                    </asp:GridView>
                </dd>
            </dl>
            <div class="wpager">
                <div class="wpageright"><uc1:Pager ID="pagerl" runat="server"/></div>
                <div class="clear"></div> 
            </div>
        </div>
    </form>
    </div>
    <div class="clear"></div>
</body>
<style type="text/css">
    #epty
    {
        white-space:nowrap;
        text-overflow: ellipsis;
	    -o-text-overflow: ellipsis;
	    overflow: hidden;
    }
</style>
<link rel="stylesheet" type="text/css" href="../css/style.css" />
<script type="text/javascript" language="javascript" src="../js/jquery.js"></script>
<script src="/Scripts/SaveCondition.js" type="text/javascript"></script>
<script type="text/javascript">
    pageName = "PlaneType";
    $("#btnSelect").click(function () { SaveSearchCondition(pageName); });
</script>
</html>
