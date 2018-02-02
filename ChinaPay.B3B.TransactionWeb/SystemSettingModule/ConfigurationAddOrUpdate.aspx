<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfigurationAddOrUpdate.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.ConfigurationAddOrUpdate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
	<link rel="stylesheet" href="/Styles/public.css?20121118" />
	<link rel="stylesheet" href="/Styles/icon/fontello.css" />
	<link rel="stylesheet" href="/Styles/skin.css" />
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<body>
    <form id="form1" runat="server">
    <div class="box-a">
		<div class="condition">
			<table>
				<colgroup>
					<col class="w20" />
					<col class="w80" />
				</colgroup>
				<tbody>
					<tr>
						<td class="title">配置类型:</td>
						<td><asp:DropDownList ID="dropConfigType" runat="server"></asp:DropDownList></td>
					</tr>
					<tr>
						<td class="title">配置用户名:</td>
						<td>
                            <asp:TextBox ID="txtConfigName" runat="server" CssClass="text null"></asp:TextBox>
							<span class="tips-txt">例：o1234567</span>
						</td>
					</tr>
					<tr>
						<td class="title">配置密码:</td>
						<td>
                            <asp:TextBox ID="txtConfigPwd" runat="server" CssClass="text null" TextMode="Password"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<td class="title">服务器地址:</td>
						<td>
                            <asp:TextBox ID="txtServerAddress" runat="server" CssClass="text null"></asp:TextBox>
							<span class="tips-txt">例：202.106.139.87</span>
						</td>
					</tr>
					<tr>
						<td class="title">服务器端口:</td>
						<td>
                            <asp:TextBox ID="txtServerDk" runat="server" CssClass="text null" ></asp:TextBox>
							<span class="tips-txt">例：443</span>
						</td>
					</tr>
                    <tr>
                      <td class="title">Office号:</td>
                      <td><asp:TextBox ID="txtOfficeNo" runat="server" CssClass="text null"></asp:TextBox></td>
                    </tr>
					<tr>
						<td class="title">SI: 工作号/密码</td>
						<td>
                            <asp:TextBox ID="txtSI" runat="server" CssClass="text null"></asp:TextBox>
							<span class="tips-txt">例：SI:43750/12345A</span>
						</td>
					</tr>
					<tr>
						<td class="title">打票机序号</td>
						<td>
                            <asp:TextBox ID="txtSequence" runat="server" CssClass="text null"></asp:TextBox>
							<span class="tips-txt">例：打印机设置为18号，请输入18</span>
						</td>
					</tr>
					<tr>
						<td></td>
						<td>
                            <asp:Button CssClass="btn class1" Text="保存" runat="server" ID="btnSave" 
                                onclick="btnSave_Click" />
                            <input type="button" value="返回" class="btn class2" onclick="javascript:window.location.href='ConfigurationList.aspx'" />
						</td>
					</tr>
				</tbody>
			</table>
		</div>
	</div>
    <asp:HiddenField ID="hfdAddOrUpdate" runat="server" />
    </form>
</body>
</html>
<script src="../Scripts/SystemSetting/configuration.js" type="text/javascript"></script>
