<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddGroupMember.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.CompanyGroup.AddGroupMember" %>
<%@ Register TagPrefix="uc" TagName="Pager_1" Src="~/UserControl/Pager.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta charset="UTF-8">
	<title>添加公司组成员</title>
</head>
	<link rel="stylesheet" href="/Styles/public.css?20121118" />
	<link rel="stylesheet" href="/Styles/icon/fontello.css" />
	<link rel="stylesheet" href="/Styles/skin.css" />
<body>
    <form id="form1" runat="server" DefaultButton="btnQuery">
	
	<h2>添加公司组成员</h2>
	<div class="box-a">
        <div class="condition">
            <table>
                <colgroup>
                    <col class="w30" />
                    <col class="w30" />
                    <col class="w30" />
                    <col class="w10" />
                </colgroup>
                <tbody>
                    <tr>
                        <td>
                            <div class="input">
                                <span class="name">公司名称：</span>
                                <asp:TextBox ID="txtName" runat="server" CssClass="text"/>
                            </div>
                        </td>
                        <td>
                            <div class="input">
                                <span class="name">公司账号：</span>
                                <asp:TextBox id="txtCompanyAccount"  runat="server" CssClass="text"/>
                            </div>
                        </td>
                        <td colspan="2">
                            <div class="input">
                                <span class="name">联系人：</span>
                                <asp:TextBox id="txtContract" runat="server" CssClass="text" />
                            </div>
                        </td>
                    </tr>
                    <tr class="btns">
                        <td colspan="3">
                            <asp:Button ID="btnQuery" class="submit btn class1" runat="server" Text="查询" onclick="btnQuery_Click"  OnClientClick="SaveSearchCondition('AddGroupMember')"/>
                            <asp:button text="选择" ID="btnChose" runat="server" CssClass="btn class1" 
                                onclick="btnChose_Click" />
				            <input type="button" class="btn class2" value="清空条件" onclick="ResetSearchOption()" />
                            <asp:Button text="返回" ID="btnBack" runat="server" CssClass="btn class2" 
                                onclick="btnBack_Click" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
	
	<div class="column table" id='data-list' >
        <asp:Repeater runat="server" ID="dataList" OnPreRender="AddEmptyTemplate">
            <HeaderTemplate>
                <table>
			<thead>
				<tr>
				    <th><input type="checkbox" onclick="checkAll(this)"/></th>
					<th>公司名称</th>
				<th>公司账号</th>
				<th>所在城市</th>
				<th>联系人</th>
				<th>联系电话</th>
				<th>注册时间</th>
				</tr>
			</thead><tbody>
            </HeaderTemplate>
            <itemtemplate>
                
			<tr>
			    <td><asp:CheckBox ID="checkboxone"  runat="server"/>
                    <asp:HiddenField runat="server" ID="hdID" Value='<%#Eval("CompanyId") %>'/>
                </td>
				<td><%#Eval("CompanyName")%></td>
				<td><%#Eval("UserName")%></td>
				<td><%#Eval("City")%></td>
				<td><%#Eval("Contact")%></td>
				<td><%#Eval("ContactPhone")%></td>
				<td><%#Eval("RegisterTime", "{0:yyyy-MM-dd}")%></td>
			</tr>
		
            </itemtemplate>
            <FooterTemplate>
                </tbody></table>
            </FooterTemplate>
        </asp:Repeater>
	</div>
        <div class="btns">
        <uc:Pager_1 runat="server" ID="pager"></uc:Pager_1>
    </div>

<script type="text/javascript" src="/Scripts/core/jquery.js"></script>
<script type="text/javascript" src="/Scripts/widget/template.js"></script>
<script type="text/javascript" src="/Scripts/widget/common.js"></script>
    <script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="/Scripts/OrderModule/QueryList.js?20121119" type="text/javascript"></script>
    <script type="text/javascript">
        function checkAll(sender) {
            if (sender.checked) {
                $("#data-list input[type='checkbox']").attr("checked", "checked");
            }
            else {
                $("#data-list input[type='checkbox']").removeAttr("checked");
            }
        }
        $(function () {
            pageName = 'AddGroupMembers';
        });
    </script>
    </form>
</body>
</html>
