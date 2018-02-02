<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MarketingAreaRelationSet.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.Operate.MarketingAreaRelationSet" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
    <link rel="stylesheet" href="/Styles/public.css?20121118" />
    <link rel="stylesheet" href="/Styles/icon/fontello.css" />
<body>
<h2>设置销售区域关系</h2>
<div class="form">
	<form id="form1" runat="server">
		<table>
			<tr>
				<td class="title">省份名称</td>
				<td><asp:Label ID="lblProvinceName" runat="server"></asp:Label></td>
			</tr>
			<tr>
				<td class="title">销售区域</td>
				<td>
					<asp:DropDownList ID="dropArea" runat="server"></asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td></td>
				<td>
                    <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn class1" 
                        onclick="btnSave_Click" />
                    <input type="button" id="btnBack" class="btn class2" onclick="javascript:window.location.href='AreaRelateList.aspx'" value="返回" />
				</td>
			</tr>
		</table>
	</form>
</div>
</body>
</html>
