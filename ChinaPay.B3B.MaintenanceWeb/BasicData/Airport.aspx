<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Airport.aspx.cs"   EnableSessionState="ReadOnly" Inherits="ChinaPay.B3B.MaintenanceWeb.BasicData.Airport" %>
<%@ Register src="../UserControl/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=7" />
    <!--让IE8解释和IE7一样，不可删-->
    <title>机场代码维护</title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="contents">
    <div class="breadcrumbs">
        <span>当前位置:</span><span>基础数据管理</span>»<span>机场代码维护</span>
    </div>
        <div class="title">
            <dl>
                <dt>
                    <img src="../images/icon.png"/>&nbsp;查询条件</dt>
                <dd class="searchbk">
                    <table width="97%" border="0" cellpadding="1" cellspacing="1" class="search condition">
                        <tr>
                            <th>
                                机场代码：
                            </th>
                            <td height="23">
                                <asp:TextBox ID="txtAirportCode" CssClass="input1" runat="server" ></asp:TextBox>
                            </td>
                            <th>
                                机场状态：
                            </th>
                            <td>
                                <asp:DropDownList ID="ddlAirportStatus" CssClass="input2" runat="server">
                                    <asp:ListItem  Value="-1">不限</asp:ListItem>
                                    <asp:ListItem Value="True">启用</asp:ListItem>
                                    <asp:ListItem Value="False">禁用</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr class="operator">
                            <td colspan="6">
                                <asp:Button ID="btnSelect" runat="server" Text="查询" CssClass="button" 
                                    OnClientClick="return btnSubmit();" onclick="btnSelect_Click" />&nbsp;
                                <input type="button" name="button3" value="添加" class="button" onclick="javascript:window.location.href='Airport_new.aspx?action=add'" />
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
                      <asp:GridView ID="gvAirport" runat="server" EmptyDataText="无满足条件的数据!"
                        AutoGenerateColumns="False" CssClass="tab3 list" 
                          onrowdeleting="gvAirport_RowDeleting" onrowcommand="gvAirport_RowCommand" >
                        <Columns>
                           <asp:TemplateField HeaderText="机场代码">
                                <ItemTemplate>
                                    <asp:Label ID="labAirportCode"  runat="server" Text='<%# Eval("Code.Value")%>'  Font-Bold="false"  CssClass="fontColor01"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Name" HeaderText="机场名称" />
                            <asp:BoundField DataField="ShortName" HeaderText="机场简称" />
                            <asp:TemplateField HeaderText="机场状态">
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus"   runat="server" Text='<%# GetState(Eval("Valid").ToString()) %>'  Font-Bold="false" CssClass="fontColor01" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="是否为主机场">
                                <ItemTemplate>
                                    <asp:Label ID="lblIsMain"   runat="server" Text='<%# GetMain(Eval("IsMain").ToString()) %>'  Font-Bold="false" CssClass="fontColor01" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="所属地代码">
                                <ItemTemplate>
                                     <asp:Label ID="lanCityCode"   runat="server" Text='<%#Eval("Location.Code") %>'  Font-Bold="false" CssClass="fontColor01" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="所属地名称">
                                <ItemTemplate>
                                     <asp:Label ID="labCityName"   runat="server" Text='<%#Eval("Location.Name") %>'  Font-Bold="false" CssClass="fontColor01" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="操作">
                                <ItemTemplate> 
                                    <asp:LinkButton ID="linkOprate"  CommandName="opdate"  CommandArgument='<%# Eval("Code.Value")%>' runat="server" Text='<%# ButtonState(Eval("Valid").ToString())%>' OnClientClick='return confirm("您确定要修改状态吗？")'></asp:LinkButton>
                                    <a href='Airport_new.aspx?action=upate&code=<%# Eval("Code.Value")%>'>修改</a>
                                    <asp:LinkButton ID="linkDel" runat="server" CommandName="Delete" CommandArgument='<%# Eval("Code.Value")%>' Text="删除"  OnClientClick='return confirm("您确定要删除吗？")'></asp:LinkButton>                                   
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
    </div>
    <div class="clear"></div>
    </form>
</body>
</html>
<link rel="stylesheet" type="text/css" href="../css/style.css" />
<script type="text/javascript" language="javascript" src="../js/jquery.js"></script>
<script src="/Scripts/SaveCondition.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">
    pageName = "Airport";
    $("#btnSelect").click(function () { SaveSearchCondition(pageName); });
    function btnSubmit() {
        var txtAirportCode = $("#txtAirportCode").val();
        var regucity = /^[A-Za-z]{3}$/;
        if ($.trim(txtAirportCode) != "" && !($.trim(txtAirportCode).match(regucity))) {
            alert("机场代码格式不正确，必须是长度为3的字母!");
            $("#txtAirportCode").select();
            return false;
        }
    }   
</script>