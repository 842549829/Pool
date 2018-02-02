<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompanyGroupMemberList.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.CompanyGroup.CompanyGroupMemberList" %>
<%@ Register TagPrefix="uc" TagName="Pager_1" Src="~/UserControl/Pager.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta charset="UTF-8">
	<title>公司组成员管理</title>
</head>
	<link rel="stylesheet" href="/Styles/public.css?20121118" />
	<link rel="stylesheet" href="/Styles/icon/fontello.css" />
	<link rel="stylesheet" href="/Styles/skin.css" />
<body>
    <form id="form1" runat="server" DefaultButton="btnQuery">
	<h2>公司组成员管理</h2>
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
                                <span class="name">公司帐号：</span>
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
                            <asp:Button type="button" ID="AddMember" Text="添加" class="submit btn class1" runat="server"/>
				            <asp:Button ID="btnQuery" class="submit btn class1" runat="server" Text="查询"  onclick="btnQuery_Click" />
				            <asp:Button ID="btnDelete" class="submit btn class1" runat="server" Text="删除" onclick="btnDelete_Click" OnClientClick="return confirmDelete()"/>
				            <input type="button" class="btn class2" value="清空条件" onclick="ResetSearchOption()" />
                            <asp:Button text="返回" ID="btnBack" runat="server" CssClass="btn class2"  onclick="btnBack_Click" />
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
				    <th><input type="checkbox" id="check_All" onclick="checkAll(this)"/></th>
					<th>公司名称</th>
				<th>公司帐号</th>
				<th>所在城市</th>
				<th>联系人</th>
				<th>联系电话</th>
				<th>所属公司组</th>
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
				<td><%#Eval("UserNo")%></td>
				<td><%#Eval("City")%></td>
				<td><%#Eval("Contact")%></td>
				<td><%#Eval("ContactPhone")%></td>
				<td><%#Eval("Group") %></td>
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
<script src="/Scripts/OrderModule/QueryList.js?20121119" type="text/javascript"></script>
    <script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script type="text/javascript" src="/Scripts/widget/common.js"></script>

    <script type="text/javascript">
        function checkAll(sender) {
            if (sender.checked) {
                $("#data-list input[type='checkbox']").attr("checked", "checked");
            }
            else {
                $("#data-list input[type='checkbox']").removeAttr("checked");
            }
        }
        function addMembers(id) {
            location.href = "AddGroupMember.aspx?GroupId=" + id + "&backUrl=" + location.href;
        }
        function checkIfAllChecked() {
            var allChecked = $("#data-list td input[type='checkbox']").not(":checked").size() == 0;
            allChecked ? $("#check_All").attr("checked", "checked") : $("#check_All").removeAttr("checked");
        }

        function confirmDelete() {
            if ($("#data-list td input:checked").size() == 0) {
                alert("未选择要删除的公司组成员！");
                return false;
            }
            return confirm('确定要删除成员吗?');
        }

        $(function() {
            $("#data-list td input[type='checkbox']").click(checkIfAllChecked);
        });
    </script>
    </form>
</body>
</html>
