<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QueryLog.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.CompanyGroup.QueryLog" %>
<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>查看日志</title>
</head>
<body>
    <form runat="server" method="post" id="form1">
            <h2>查看日志</h2>
	<div class="column table">
		<asp:Repeater runat="server" ID="datalist">
            <HeaderTemplate>
                <table>
                    <tr>
				        <th style="min-width:150px;">时间</th>
				        <th style="min-width:150px;">操作人</th>
				        <th style="min-width:100px;">操作项</th>
				        <th>操作内容</th>
			        </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td><%#Eval("Time")%></td>
                    <td><%#Eval("Account")%></td>
                    <td><%#Eval("OperationType")%></td>
                    <td><%#Eval("Content")%></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
	</div>
    <div class="btns">
        <uc:Pager  runat="server" ID="pager" Visible="false"/>
    </div>
    </form>
</body>
</html>
