<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LowerComapnyInfoUpdate.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.ExtendCompanyManage.LowerComapnyInfoUpdate.LowerComapnyInfoUpdate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../../../Styles/core.css?20121118" rel="stylesheet" type="text/css" />
    <link href="../../../../Styles/form.css?20121118" rel="stylesheet" type="text/css" />
    <link href="../../../../Styles/icon/main.css" rel="stylesheet" type="text/css" />
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
	<div class="form">
		<h2>信息修改</h2>
		<form runat="server" id="form1">
			<table>
				<colgroup>
					<col class="w15" />
					<col class="w35" />
					<col class="w15" />
					<col class="w35" />
				</colgroup>
				<tr>
					<td class="title">用户名:</td>
					<td><asp:Label ID="lblAccountNo" runat="server"></asp:Label></td>
					<td class="title">公司类型:</td>
					<td><asp:Label ID="lblCompanyType" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td class="title">公司名称:</td>
					<td><asp:Label ID="lblCompanyName" runat="server"></asp:Label></td>
					<td class="title">公司简称:</td>
					<td><asp:Label ID="lblCompanyShortName" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td class="title">所在地:</td>
					<td colspan="3">
                         <asp:Label ID="lblLoaction" runat="server"></asp:Label>
                    </td>
				</tr>
				<tr>
					<td class="title">公司地址:</td>
					<td>
                        <asp:Label ID="lblAddress" runat="server"></asp:Label>
					</td>
                    <td class="title">公司电话：</td>
					<td>
                        <asp:Label ID="lblCompanyPhone" runat="server"></asp:Label>
                    </td>
				</tr>
				<tr>
					<td class="title">负责人：</td>
                    <td>
                        <asp:Label ID="lblPrincipal" runat="server"></asp:Label>
                    </td>
					<td class="title">负责人手机：</td>
                    <td>
                        <asp:Label ID="lblPrincipalPhone" runat="server"></asp:Label>
                    </td>
				</tr>
				<tr>
					<td class="title">联系人：</td>
					<td>
                        <asp:Label ID="lblLinkman" runat="server"></asp:Label>
                    </td>
					<td class="title">联系人手机：</td>
					<td>
                        <asp:Label ID="lblLinkmanPhone" runat="server"></asp:Label>
                    </td>
				</tr>
				<tr>
					<td class="title">紧急联系人：</td>
					<td>
                        <asp:Label ID="lblUrgencyLinkMan" runat="server"></asp:Label>
                    </td>
					<td class="title">紧急联系手机：</td>
					<td>
                        <asp:Label ID="lblUrgencyLinkManPhone" runat="server"></asp:Label>
                    </td>
				</tr>
                <tbody runat="server" id="tbUpdate">
                    <tr>
                        <td class="title">
                            EMail：
                        </td>
                        <td>
                           <asp:Label ID="lblEmial" runat="server"></asp:Label>
                        </td>
                        <td class="title">
                            传真：
                        </td>
                        <td>
                            <asp:TextBox ID="txtFaxes" runat="server" CssClass="text"></asp:TextBox>
                            <span id="lblFaxes"></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            邮政编码：
                        </td>
                        <td>
                            <asp:TextBox ID="txtPostCode" runat="server" CssClass="text"></asp:TextBox>
                            <span id="lblPostCode"></span>
                        </td>
                        <td class="title">
                            QQ
                        </td>
                        <td>
                            <asp:TextBox ID="txtQQ" runat="server" CssClass="text"></asp:TextBox>
                            <span id="lblQQ"></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            MSN：
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtMSN" runat="server" CssClass="text"></asp:TextBox>
                            <span id="lblMSN"></span>
                        </td>
                    </tr>
                </tbody>
                <tbody runat="server" id="tbLookUp">
                    <tr>
                        <td class="title">
                            邮政编码：
                        </td>
                        <td>
                            <asp:Label ID="lbllPostCode" runat="server"></asp:Label>
                        </td>
                        <td class="title">
                            传真：
                        </td>
                        <td>
                            <asp:Label ID="lbllFasex" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            EMail：
                        </td>
                        <td>
                           <asp:Label ID="lbllEmail" runat="server"></asp:Label>
                        </td>
                        <td class="title">
                            QQ
                        </td>
                        <td>
                            <asp:Label ID="lbllQQ" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            MSN：
                        </td>
                        <td colspan="3">
                            <asp:Label ID="lbllMsn" runat="server"></asp:Label>
                        </td>
                    </tr>
                </tbody>
			</table>
            <div class="btns">
                <asp:Button ID="txtBtnSave" runat="server" CssClass="btn class1" Text="保存" onclick="txtBtnSave_Click" />
			    <input type="button" class="btn class2" value="返回" onclick="window.location.href='./Lower_manage.aspx'" />
            </div>
		</form>
	</div>
    <script src="../../../../Scripts/OrganizationModule/RoleModule/FixityInformation.js" type="text/javascript"></script>
    <script src="../../../../Scripts/OrganizationModule/RoleModule/UpdateLowerCompany.js" type="text/javascript"></script>
</body>
</html>
