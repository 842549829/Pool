<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DistributionOemLower.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.DistributionOemLower" %>

<%@ Register TagPrefix="uc" TagName="Pager" Src="~/UserControl/Pager.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>下级公司管理</title>
</head>
    <link rel="stylesheet" href="/Styles/public.css?20121118" />
    <link href="/Styles/masklayer/masklayer.css" rel="stylesheet" type="text/css" />
<body>
    <form id="form1" runat="server" defaultbutton="btnQuery">
    <h3 class="titleBg">
        下级公司管理</h3>
    <div class="box-a">
        <div class="condition">
            <table>
                <colgroup>
                    <col class="w30" />
                    <col class="w35" />
                    <col class="w35" />
                </colgroup>
                <tr>
                    <td>
                        <div class="input">
                            <span class="name">公司简称：</span>
                            <asp:TextBox ID="txtAbbreviateName" class="text  textarea" type="text" runat="server" />
                        </div>
                    </td>
                    <td>
                        <div class="input up-index">
                            <span class="name">状态：</span>
                            <asp:DropDownList runat="server" ID="ddlStatus">
                                <asp:ListItem Text="全部" Value="">
                                </asp:ListItem>
                                <asp:ListItem Text="正常" Value="1">
                                </asp:ListItem>
                                <asp:ListItem Text="禁用" Value="2">
                                </asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>
                    <td>
                        <div class="input up-index" style="z-index: 53;">
                            <span class="name">公司类别：</span>
                            <div class="fl">
                                <asp:DropDownList ID="ddlRelationType" runat="server" Width="88px">
                                </asp:DropDownList>
                            </div>
                            <div class="fl">
                                <asp:DropDownList runat="server" ID="ddlAccountType" Width="65px">
                                    <asp:ListItem Value="" Text="全部"></asp:ListItem>
                                    <asp:ListItem Text="个人" Value="0" />
                                    <asp:ListItem Text="企业" Value="1" />
                                </asp:DropDownList>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="input">
                            <span class="name">用户名：</span>
                            <asp:TextBox ID="txtUserName" class="text  textarea" type="text" runat="server" />
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">联系人：</span>
                            <asp:TextBox ID="txtContact" class="text  textarea" type="text" runat="server" />
                        </div>
                    </td>
                    <td style="padding-left: 30px">
                        <asp:Button class="btn class1 submit" ID="btnQuery" runat="server" Text="查询" OnClick="BtnQuery_Click"></asp:Button>
                        <%--<input class="btn class2" type="button" value="返回" onclick="window.location.href='./DistributionOemAuthorizationDetail.aspx';" />--%>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id='data-list' class="table">
        <asp:Repeater runat="server" ID="datalist"  OnPreRender="AddEmptyTemplate">
            <HeaderTemplate>
                <table cellspacing="0">
                    <thead>
                        <tr>
                            <th>
                                用户名
                            </th>
                            <th>
                                公司类别
                            </th>
                            <th>
                                公司简称
                            </th>
                            <th>
                                联系人
                            </th>
                            <th>
                                公司组
                            </th>
                            <th>
                                状态
                            </th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <%#Eval("UserNo")%>
                    </td>
                    <td>
                        <%#Eval("RelationType")%>(<%#Eval("AccountType") %>)
                    </td>
                    <td>
                        <%#Eval("AbbreviateName")%>
                    </td>
                    <td>
                        <%#Eval("Contact")%>
                    </td>
                    <td>
                        <%#Eval("Group")%>
                    </td>
                    <td>
                        <%#(bool)Eval("Enabled")?"正常":"禁用"%>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody> </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>
    <div class="btns">
        <uc:Pager runat="server" ID="pager" Visible="false"></uc:Pager>
    </div>
    </form>
</body>
</html>
<script type="text/javascript" src="/Scripts/core/jquery.js"></script>