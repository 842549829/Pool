<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerAddOrUpdate.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.CustomerModule.CustomerAddOrUpdate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>添加/修改常旅客</title>
	<link rel="stylesheet" href="/Styles/public.css?20121118" />
	<link rel="stylesheet" href="/Styles/icon/fontello.css" />
	<link rel="stylesheet" href="/Styles/skin.css" />
</head>
<body>
<form id="form1" runat="server">
    <!-- 添加修改常旅客 -->
	<h3 class="titleBg">添加/修改常旅客</h3>
  <div class="form">
    <table>
        <colgroup>
            <col class="w25" />
            <col class="w75" />
        </colgroup>
        <tbody>
            <tr>
                <td class="title">姓名:</td>
                <td><asp:TextBox ID="txtName" runat="server" CssClass="text textarea"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="title">性别：</td>
                <td>
                    <div class="check">
                        <label><asp:RadioButton ID="rbnMale" runat="server" Text="男" Checked="true" GroupName="Sex" /></label>
                        <label><asp:RadioButton ID="rbnFemale" runat="server" Text="女" GroupName="Sex"/></label>
		            </div>
                </td>
            </tr>
            <tr style="position:relative; z-index:2;">
                <td class="title">
                    旅客类型：
                </td>
                <td>
                    <asp:DropDownList ID="dropCustomerType" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="title">联系电话：</td>
                <td>
                    <asp:TextBox ID="txtContactPhone" runat="server" CssClass="text textarea"></asp:TextBox>
                </td>
            </tr>
            <tr style="position:relative; z-index:1;">
                <td class="title">证件类型:</td>
                <td>
                    <asp:DropDownList ID="dropCertType" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="title">证件号码:</td>
                <td>
                    <asp:TextBox ID="txtCertId" runat="server" CssClass="text textarea"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="title">备注:</td>
                <td>
                    <textarea id="txtRemark" class="text"  style="width:50%; height:40px;" runat="server"></textarea>
                </td>
            </tr>
        </tbody>
    </table>
    <div class="btns">
        <asp:Button ID="btnSave" runat="server" Text="添加/修改" CssClass="btn class1" onclick="btnSave_Click"/>
        <input type="button" value="返回" class="btn class2" onclick="javascript:window.location.href='CustomerList.aspx'" />
    </div>
  </div>
    <asp:HiddenField ID="hfdAddOrUpdate" runat="server" />
    </form>
</body>
</html>
<script type="text/javascript" src="/Scripts/core/jquery.js"></script>
<script type="text/javascript" src="/Scripts/widget/jquery-ui-1.8.21.custom.min.js"></script>
<script type="text/javascript" src="/Scripts/widget/common.js"></script>
<script src="../Scripts/SystemSetting/customer.js" type="text/javascript"></script>
