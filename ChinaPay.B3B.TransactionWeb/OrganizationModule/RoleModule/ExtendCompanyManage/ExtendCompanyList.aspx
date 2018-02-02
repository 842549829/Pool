<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExtendCompanyList.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.ExtendCompanyManage.ExtendCompanyList" %>

<%@ Register TagPrefix="uc" TagName="Pager" Src="~/UserControl/Pager.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>推广公司管理</title>
</head>
    <link rel="stylesheet" href="/Styles/public.css?20121118" />
    <link rel="stylesheet" href="/Styles/skin.css" />
    <style type="text/css">
        .dlLeft
        {
            float: left;
        }
        .tuiguang
        {
            margin-left:20px;
            line-height:1.5;
        }
        .tuiguang a
        {
            padding:0 15px;
        }
    </style>
<body>
    <form id="form1" runat="server" defaultbutton="btnQuery">
    <h3 class="titleBg">
        推广公司管理
    </h3>
    <div class="box-a">
        <div class="condition" style="position:relative;z-index:1;">
            <table>
                <colgroup>
                    <col class="w30" />
                    <col class="w35" />
                    <col class="w35" />
                </colgroup>
                <tbody>
                    <tr>
                        <td>
                            <div class="input">
                                <span class="name">公司简称：</span>
                                <asp:TextBox value="" ID="txtAbbreviateName" class="text textarea" type="text" runat="server" />
                            </div>
                        </td>
                        <td>
                            <div class="input up-index">
                                <span class="name">公司性质：</span>
                                <div class="dlLeft">
                                    <asp:DropDownList runat="server" ID="ddlCompanyType" AppendDataBoundItems="True" Width="78px">
                                        <asp:ListItem Text="全部" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="dlLeft">
                                    <asp:DropDownList runat="server" Width="75px" ID="ddlAccountType">
                                        <asp:ListItem Value="" Text="全部"></asp:ListItem>
                                        <asp:ListItem Text="个人" Value="0" />
                                        <asp:ListItem Text="企业" Value="1" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="input up-index">
                                <span class="name">状态：</span>
                                <asp:DropDownList runat="server" ID="ddlStatus" AppendDataBoundItems="True">
                                    <asp:ListItem Text="全部" Value=""></asp:ListItem>
                                    <asp:ListItem Text="正常" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="禁用" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="input">
                                <span class="name">用户名：</span>
                                <asp:TextBox ID="txtAccount" class="text textarea" type="text" runat="server" />
                            </div>
                        </td>
                        <td>
                            <div class="input">
                                <span class="name">联系人：</span>
                                <asp:TextBox ID="txtContact" class="text textarea" type="text" runat="server" />
                            </div>
                        </td>
                        <td>
                          <div class="input">
                            <span class="name">推广员工：</span>
                            <asp:DropDownList ID="ddlEmployeNo" CssClass="textarea" runat="server"></asp:DropDownList>
                          </div>
                        </td>
                    </tr>
                    <tr class="btns">
                       <td colspan="3">
                            <asp:Button ID="btnQuery" class="btn class1 submit" type="button" runat="server"
                                Text="查询" OnClick="btnQuery_Click" OnClientClick="SaveSearchCondition('ExtendCompany')" />
                            <input class="btn class1" type="button" value="开户" onclick="window.location.href='../../CommonContent/AddAccount/OpenAccount.aspx'" />
                            <input class="btn class2" type="button" value="清空条件" onclick="ResetSearchOption()" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <div class="box-a clearfix">
     <asp:Image ID="img" runat="server" ImageUrl="~/Images/ico-1.png" class="fl" />
      <div class="fl tuiguang">
        您的专属推广链接为：<asp:Label ID="lblAddress" class="obvious-a" runat="server"></asp:Label><a id="lblCope" class="obvious" href="javascript:void(0)">复制</a><br />
        您可以将该链接发给您的朋友，您的朋友注册后将自动与您形成绑定关系，成为您的推广用户；您的朋友发生购票交易后，您就可以在平台享受分红收益！
      </div>
    </div>
    <div id="data-list" class="column table">
        <asp:Repeater runat="server" ID="datalist" OnPreRender="AddEmptyTemplate">
            <HeaderTemplate>
                <table>
<%--                    <colgroup>
                        <col span="6" class="w15" />
                        <col />
                    </colgroup>--%>
                    <thead>
                        <tr>
                            <th>
                                用户名
                            </th>
                            <th>
                                平台角色
                            </th>
                            <th>
                                公司简称
                            </th>
                            <th>
                                联系人
                            </th>
                            <th>
                                联系人手机
                            </th>
                            <%--					<th>城市</th>
					<th>公司电话</th>
					<th>上次登录时间</th>
					<th>注册时间</th>
                    <th>审核</th>--%>
                            <th>
                                状态
                            </th>
                            <th>
                             推广员工
                            </th>
                            <th class="w10">
                                操作
                            </th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <a href='LookUpInfo/SpreadCompanyDetailInfo.aspx?CompanyId=<%#Eval("ID") %>'>
                            <%#Eval("Admin")%></a>
                    </td>
                    <td>
                        <%#Eval("Type")%>(<%#Eval("AccountType")%>)
                    </td>
                    <td>
                        <%#Eval("AbbreviateName")%>
                    </td>
                    <td>
                        <%#Eval("Contact")%>
                    </td>
                    <td>
                        <%#Eval("ContactCellphone")%>
                    </td>
                    <%--		    <td><%#Eval("AirportCode")%></td>
		        <td><%#Eval("OfficePhone")%></td>
		        <td><%#Eval("LastLoginTime")%></td>
		        <td><%#Eval("RegisterTime")%></td>
                <td><label style='display:<%#(bool)Eval("DisplayAudit")?"":"none;"%>'><%#(bool)Eval("Audited")?"已审":"未审"%></label></td>--%>
                    <td>
                        <%#(bool)Eval("Enabled")?"正常":"禁用"%>
                    </td>
                    <td>
                      <%#Eval("OperatorAccount")%>
                    </td>
                    <td>
                        <a href='LookUpInfo/SpreadCompanyDetailInfo.aspx?CompanyId=<%#Eval("ID") %>'>查看</a>
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
<script type="text/javascript" src="/Scripts/widget/common.js"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="/Scripts/OrderModule/QueryList.js?20121119" type="text/javascript"></script>
<script type="text/javascript">
    pageName = 'ExtendCompany';
    $("#lblCope").click(function () {
        var spreadAddress = $("#lblAddress").text();
        copyToClipboard(spreadAddress);
    });
</script>
<%--    <script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript">         SaveDefaultData()</script>
<script src="/scripts/my97datepicker/wdatepicker.js" type="text/javascript"></script>
    --%>